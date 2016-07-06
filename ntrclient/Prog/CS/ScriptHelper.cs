using System.IO;

namespace ntrclient.Prog.CS
{
    public class ScriptHelper
    {
        public void Bpadd(uint addr, string type = "code.once")
        {
            uint num = 0;
            if (type == "code")
            {
                num = 1;
            }
            if (type == "code.once")
            {
                num = 2;
            }
            if (num != 0)
            {
                Program.NtrClient.SendEmptyPacket(11, num, addr, 1);
            }
        }

        public void Bpdis(uint id)
        {
            Program.NtrClient.SendEmptyPacket(11, id, 0, 3);
        }

        public void Bpena(uint id)
        {
            Program.NtrClient.SendEmptyPacket(11, id, 0, 2);
        }

        public void Resume()
        {
            Program.NtrClient.SendEmptyPacket(11, 0, 0, 4);
        }
        
        public void Connect(string host, int port)
        {
            Program.NtrClient.SetServer(host, port);
            Program.NtrClient.ConnectToServer();
        }

        public void Reload()
        {
            Program.NtrClient.SendReloadPacket();
        }

        public void Listprocess()
        {
            Program.NtrClient.SendEmptyPacket(5);
        }

        public void Listthread(int pid)
        {
            Program.NtrClient.SendEmptyPacket(7, (uint) pid);
        }

        public void Attachprocess(int pid, uint patchAddr = 0)
        {
            Program.NtrClient.SendEmptyPacket(6, (uint) pid, patchAddr);
        }

        public void Queryhandle(int pid)
        {
            Program.NtrClient.SendEmptyPacket(12, (uint) pid);
        }

        public void Memlayout(int pid)
        {
            Program.NtrClient.SendEmptyPacket(8, (uint) pid);
        }

        public void Disconnect()
        {
            Program.NtrClient.Disconnect();
        }

        public void Sayhello()
        {
            Program.NtrClient.SendHelloPacket();
        }

        public void Data(uint addr, uint size = 0x100, int pid = -1, string filename = null)
        {
            if (filename == null && size > 1024)
            {
                size = 1024;
            }
            Program.NtrClient.SendReadMemPacket(addr, size, (uint) pid, filename);
        }

        public void Write(uint addr, byte[] buf, int pid = -1)
        {
            Program.NtrClient.SendWriteMemPacket(addr, (uint) pid, buf);
        }

        public void Write(uint addr, byte buf, int pid = -1)
        {
            byte[] temp = {buf};
            Program.NtrClient.SendWriteMemPacket(addr, (uint) pid, temp);
        }

        public void Sendfile(string localPath, string remotePath)
        {
            FileStream fs = new FileStream(localPath, FileMode.Open);
            byte[] buf = new byte[fs.Length];
            fs.Read(buf, 0, buf.Length);
            fs.Close();
            Program.NtrClient.SendSaveFilePacket(remotePath, buf);
        }

        public void Read(uint addr, uint size = 4, int pid = -1)
        {
            Data(addr, size, pid);
        }

        public void Remoteplay(uint priorityMode = 0, uint priorityFactor = 5, uint quality = 90, uint qosValue = 100)
        {
            Program.NtrClient.SendRemoteplayPacket(priorityMode, priorityFactor, quality, qosValue);
            if (Program.GCmdWindow.checkBox_disconnect.Checked)
            {
                Program.NtrClient.Log("Disconnecting in 10 seconds to improve performance");
                Program.GCmdWindow.StartAutoDisconnect();
            }
        }

        public void Debug()
        {
            Program.Dc.Show();
        }
    }
}