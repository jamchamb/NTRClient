using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ntrclient.Prog.Window;

namespace ntrclient.Prog.CS
{
    public class NtrClient
    {
        public string Host;
        public int Port;
        public TcpClient Tcp;
        public NetworkStream NetStream;
        public Thread PacketRecvThread;
        private readonly object _syncLock = new object();
        private int _heartbeatSendable;

        public delegate void LogHandler(string msg);

        public event LogHandler OnLogArrival;
        private uint _currentSeq;
        private uint _lastReadMemSeq;
        private string _lastReadMemFileName;
        public volatile int Progress = -1;

        private int ReadNetworkStream(Stream stream, byte[] buf, int length)
        {
            int index = 0;
            bool useProgress = length > 100000;
            
            try
            {
            do
            {
                if (useProgress)
                {
                        Progress = (int)((double)index / length * 100);
                }
                int len = stream.Read(buf, index, length - index);
                if (len == 0)
                {
                    return 0;
                }
                index += len;
            } while (index < length);
            Progress = -1;
            return length;

        }
            catch (Exception e)
            {
                Log("Connection timed out");
                return 0;
            }
        }

        private void PacketRecvThreadStart()
        {
            byte[] buf = new byte[84];
            uint[] args = new uint[16];
            NetworkStream stream = NetStream;

            while (true)
            {
                try
                {
                    int ret = ReadNetworkStream(stream, buf, buf.Length);
                    if (ret == 0)
                    {
                        break;
                    }
                    int t = 0;
                    uint magic = BitConverter.ToUInt32(buf, t);
                    t += 4;
                    uint seq = BitConverter.ToUInt32(buf, t);
                    t += 4;
                    // ReSharper disable once UnusedVariable
                    uint type = BitConverter.ToUInt32(buf, t);
                    t += 4;
                    uint cmd = BitConverter.ToUInt32(buf, t);
                    for (int i = 0; i < args.Length; i++)
                    {
                        t += 4;
                        args[i] = BitConverter.ToUInt32(buf, t);
                    }
                    t += 4;
                    uint dataLen = BitConverter.ToUInt32(buf, t);


                    if (cmd != 0)
                    {
                        Log(string.Format("packet: cmd = {0}, dataLen = {1}", cmd, dataLen));
                    }

                    if (magic != 0x12345678)
                    {
                        Log(string.Format("broken protocol: magic = {0}, seq = {1}", magic, seq));
                        break;
                    }

                    if (cmd == 0)
                    {
                        if (dataLen != 0)
                        {
                            byte[] dataBuf = new byte[dataLen];
                            ReadNetworkStream(stream, dataBuf, dataBuf.Length);
                            string logMsg = Encoding.UTF8.GetString(dataBuf);
                            // Tinkering even more with the Debugger

                            if (logMsg.StartsWith("valid memregions:"))
                            {
                                // Setting memregions 
                                Program.GCmdWindow.textBox_memlayout.Invoke(
                                    new CmdWindow.SetMemregionsCallback(Program.GCmdWindow.SetMemregions), logMsg);
                            }
                            else if (logMsg.StartsWith("pid: "))
                            {
                                Program.GCmdWindow.textBox_processes.Invoke(
                                    new CmdWindow.SetProcessesCallback(Program.GCmdWindow.SetProcesses), logMsg);
                            }
                            else if (logMsg.StartsWith("patching smdh") || logMsg.StartsWith("rtRecvSocket failed: "))
                            {
                                Program.GCmdWindow.RunProcessesCmd();
                            }
                            // END

                            Log(logMsg);
                        }
                        lock (_syncLock)
                        {
                            _heartbeatSendable = 1;
                        }
                        continue;
                    }
                    if (dataLen != 0)
                    {
                        byte[] dataBuf = new byte[dataLen];
                        ReadNetworkStream(stream, dataBuf, dataBuf.Length);
                        HandlePacket(cmd, seq, dataBuf);
                    }
                }
                catch (Exception e)
                {
                    Log(e.Message);
                    Log(e.StackTrace);
                    //log("An error occured!");
                    break;
                }
            }

            Log("Server disconnected.");
            Disconnect(false);
        }

        // ReSharper disable once UnusedParameter.Local
        private static string ByteToHex(IEnumerable<byte> datBuf, int type)
        {
            return datBuf.Aggregate("", (current, t) => current + t.ToString("X2"));
        }

        private void HandleReadMem(uint seq, byte[] dataBuf)
        {
            if (seq != _lastReadMemSeq)
            {
                Log("seq != lastReadMemSeq, ignored");
                return;
            }
            _lastReadMemSeq = 0;
            string fileName = _lastReadMemFileName;
            if (fileName != null)
            {
                FileStream fs = new FileStream(fileName, FileMode.Create);
                fs.Write(dataBuf, 0, dataBuf.Length);
                fs.Close();
                Log("dump saved into " + fileName + " successfully");
                return;
            }
            string hex = ByteToHex(dataBuf, 0);
            Log("Read memory: " + hex);

            // Read Byte, Short and Word
            int length = dataBuf.Length;
            if ((length == 1) || (length == 2) || (length == 4))
            {
                Program.GCmdWindow.SetReadValue(Convert.ToUInt32(hex, 16));
            }
        }

        private void HandlePacket(uint cmd, uint seq, byte[] dataBuf)
        {
            if (cmd == 9)
            {
                HandleReadMem(seq, dataBuf);
            }
        }

        public void SetServer(string serverHost, int serverPort)
        {
            Host = serverHost;
            Port = serverPort;
        }

        public void ConnectToServer()
        {
            if (Tcp != null)
            {
                Disconnect();
            }
            Tcp = new TcpClient {NoDelay = true};
            Tcp.Connect(Host, Port);
            _currentSeq = 0;
            NetStream = Tcp.GetStream();
            _heartbeatSendable = 1;
            PacketRecvThread = new Thread(PacketRecvThreadStart);
            PacketRecvThread.Start();
            Log("Server connected.");
        }

        public void Disconnect(bool waitPacketThread = true)
        {
            try
            {
                Tcp?.Close();
                if (waitPacketThread)
                {
                    PacketRecvThread?.Join();
                }
            }
            catch (Exception)
            {
                Log("Disconnect " + waitPacketThread);
            }
            Tcp = null;
        }

        public void SendPacket(uint type, uint cmd, uint[] args, uint dataLen)
        {
            int t = 0;
            _currentSeq += 1000;
            byte[] buf = new byte[84];
            BitConverter.GetBytes(0x12345678).CopyTo(buf, t);
            t += 4;
            BitConverter.GetBytes(_currentSeq).CopyTo(buf, t);
            t += 4;
            BitConverter.GetBytes(type).CopyTo(buf, t);
            t += 4;
            BitConverter.GetBytes(cmd).CopyTo(buf, t);
            for (int i = 0; i < 16; i++)
            {
                t += 4;
                uint arg = 0;
                if (args != null)
                {
                    arg = args[i];
                }
                BitConverter.GetBytes(arg).CopyTo(buf, t);
            }
            t += 4;
            BitConverter.GetBytes(dataLen).CopyTo(buf, t);
            NetStream.Write(buf, 0, buf.Length);
        }

        public void SendReadMemPacket(uint addr, uint size, uint pid, string fileName)
        {
            SendEmptyPacket(9, pid, addr, size);
            _lastReadMemSeq = _currentSeq;
            _lastReadMemFileName = fileName;
        }

        public void SendWriteMemPacket(uint addr, uint pid, byte[] buf)
        {
            uint[] args = new uint[16];
            args[0] = pid;
            args[1] = addr;
            args[2] = (uint) buf.Length;
            SendPacket(1, 10, args, args[2]);
            NetStream.Write(buf, 0, buf.Length);
        }

        public void SendHeartbeatPacket()
        {
            if (Tcp != null)
            {
                lock (_syncLock)
                {
                    if (_heartbeatSendable == 1)
                    {
                        _heartbeatSendable = 0;
                        SendPacket(0, 0, null, 0);
                    }
                }
            }
        }

        public void SendHelloPacket()
        {
            SendPacket(0, 3, null, 0);
        }

        public void SendReloadPacket()
        {
            SendPacket(0, 4, null, 0);
        }

        public void SendEmptyPacket(uint cmd, uint arg0 = 0, uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0)
        {
            uint[] args = new uint[16];

            args[0] = arg0;
            args[1] = arg1;
            args[2] = arg2;
            args[3] = arg3;
            args[4] = arg4;
            SendPacket(0, cmd, args, 0);
        }

        public void SendRemoteplayPacket(uint priorityMode = 0, uint priorityFactor = 5, uint quality = 90, uint qosValue = 100)
        {
            uint num1 = 1U;
            if ((int)priorityMode == 0)
                num1 = 0U;
            uint num2 = (qosValue * 0x20000);
            Program.NtrClient.SendEmptyPacket(901U, num1 << 8 | priorityFactor, quality, num2);
        
        }

        public void SendSaveFilePacket(string fileName, byte[] fileData)
        {
            byte[] fileNameBuf = new byte[0x200];
            Encoding.UTF8.GetBytes(fileName).CopyTo(fileNameBuf, 0);
            SendPacket(1, 1, null, (uint) (fileNameBuf.Length + fileData.Length));
            NetStream.Write(fileNameBuf, 0, fileNameBuf.Length);
            NetStream.Write(fileData, 0, fileData.Length);
        }

        public void Log(string msg)
        {
            OnLogArrival?.Invoke(msg);
            try
            {
                Program.GCmdWindow.BeginInvoke(Program.GCmdWindow.DelAddLog, msg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}