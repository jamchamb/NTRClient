using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ntrclient
{
    class gateshark
    {
        List<gateshark_ar> lines = new List<gateshark_ar>();
        Int32 offset;
        Int32 dxData;
        Boolean loop;
        Int32 loop_index;
        UInt32 loop_count;
        public gateshark(String code)
        {
            String[] l = Regex.Split(code, "\r\n|\r|\n");
            foreach (String line in l)
            {
                lines.Add(new gateshark_ar(line));

            }
        }
            /*

            [TEST]
            D3000000 10000000
            C0000000 00000003
            DF000000 00000000
            D1000000 00000001
            D3000000 00000000
            
            
            */

        public void execute()
        {
            int index = 0;
            int dummy_count = 0;
            Boolean gs_if = true;
            do
            {
                gateshark_ar gs_ar = lines[index];
                int cmd = gs_ar.getCMD();
                if (gs_if)
                {

                    if ((cmd == 0) || (cmd == 1) || (cmd == 2))
                    {
                        gs_ar.execute(offset);
                    }
                    else if (cmd == 0x9)
                    {
                        UInt32 read = Convert.ToUInt32(CmdWindow.fromLE(Program.gCmdWindow.readValue(gs_ar.getBlock_A() + offset, 2), 2));
                        Task.Delay(100);
                        gs_if = (read == gs_ar.getBlock_B());
                        //MessageBox.Show(String.Format("0x9 READ: {0:X} {1:X}", gs_ar.getBlock_A() + offset, Convert.ToInt32(read)));
                    }
                    else if (cmd == 0xB)
                    {
                        offset = CmdWindow.fromLE(Program.gCmdWindow.readValue(gs_ar.getBlock_A() + offset, 4), 4);
                        //MessageBox.Show(String.Format("SET > O: {0:X}", offset));
                    }
                    else if (cmd == 0xC)
                    {
                        loop = true;
                        loop_index = index;
                        loop_count = gs_ar.getBlock_B() + 1;
                    }
                    else if (cmd == 0xD1)
                    {
                        //Task.Delay(100);
                        if (loop)
                        {
                            loop_count--;
                            if (loop_count == 0)
                            {
                                loop = false;
                                //MessageBox.Show("Stopped the loop");
                            }
                            else
                            {
                                index = loop_index;
                                offset += Convert.ToInt32(gs_ar.getBlock_B());
                                //MessageBox.Show("Continuing loop");
                            }
                        }
                    }
                    else if (cmd == 0xD3) // Read offset
                    {
                        // Fix for Issue #8
                        UInt32 b = gs_ar.getBlock_B();
                        Int32 b_ = 0;
                        if (b > Int32.MaxValue)
                        {
                            // Offset in negative.
                            Int32 r = Convert.ToInt32(b % 0x80000000);
                            b_ = Convert.ToInt32(Int32.MinValue + r); 
                        } else
                            b_ = Convert.ToInt32(b);

                        offset = b_;
                    }
                    else if (cmd == 0xD5) // DxData WRITE
                    {
                        dxData = Convert.ToInt32(gs_ar.getBlock_B());
                    }
                    else if (cmd == 0xD6) // DxData WORD
                    {
                        int len = 4;
                        String cmd_string = Program.gCmdWindow.generateWriteString(Convert.ToInt32(gs_ar.getBlock_B()) + offset, dxData, len);
                        Program.gCmdWindow.runCmd(cmd_string);
                        offset += len;
                    }
                    else if (cmd == 0xD7) // DxData SHORT
                    {
                        int len = 2;
                        String cmd_string = Program.gCmdWindow.generateWriteString(Convert.ToInt32(gs_ar.getBlock_B()) + offset, dxData, len);
                        Program.gCmdWindow.runCmd(cmd_string);
                        offset += len;
                    }
                    else if (cmd == 0xD8) // DxData Byte
                    {
                        int len = 1;
                        String cmd_string = Program.gCmdWindow.generateWriteString(Convert.ToInt32(gs_ar.getBlock_B()) + offset, dxData, len);
                        Program.gCmdWindow.runCmd(cmd_string);
                        offset += len;
                    }
                    else if (cmd == 0xD9) // DxData READ WORD
                    {
                        int addr = Convert.ToInt32(gs_ar.getBlock_B()) + offset;
                        dxData = CmdWindow.fromLE(Program.gCmdWindow.readValue(addr, 4), 4);
                    }
                    else if (cmd == 0xDA) // DxData READ SHORT
                    {
                        int addr = Convert.ToInt32(gs_ar.getBlock_B()) + offset;
                        dxData = CmdWindow.fromLE(Program.gCmdWindow.readValue(addr, 2), 2);
                    }
                    else if (cmd == 0xDB) // DxData READ BYTE
                    {
                        int addr = Convert.ToInt32(gs_ar.getBlock_B()) + offset;
                        dxData = Program.gCmdWindow.readValue(addr, 1);
                    }
                    else if (cmd == 0xDC)
                    {
                        // Fix for Issue #8
                        UInt32 b = gs_ar.getBlock_B();
                        Int32 b_ = 0;
                        if (b > Int32.MaxValue)
                        {
                            // Offset in negative.
                            Int32 r = Convert.ToInt32(b % 0x80000000);
                            b_ = Convert.ToInt32(Int32.MinValue + r);
                        }
                        else
                            b_ = Convert.ToInt32(b);


                        offset += b_;
                    }
                    else if (cmd == 0xDF)
                    {
                        // This doesn't actually exist! It's for testing only!
                        dummy_count++;
                        MessageBox.Show(String.Format(
                            "I: {0} \r\n" +
                            "O: {1:X} \r\n" +
                            "LOOP: {2} {3} {4} \r\n" + 
                            "DX: {5:X} \r\n" +
                            "DUMMY: {6}"
                            , index, offset, loop, loop_index, loop_count, dxData, dummy_count));
                    }
                }
                else if (cmd == 0xD0)
                {
                    gs_if = true;
                }
                index++;
            } while (index < lines.Count);
        }

        public gateshark_ar[] getAllCodes()
        {
            return lines.ToArray();
        }
    }

    class gateshark_ar
    {
        String line;
        Int32 cmd;
        Int32 block_a;
        UInt32 block_b;
        public gateshark_ar(String ar)
        {
            line = ar;
            int o = 0;
            String[] blocks = ar.Split(' ');

            if (ar.Length != 17)
            {
                cmd = 0x0f;
                block_a = 0x0fffffff;
                block_b = 0x7fffffff;
                return;
            }
            ar.Replace(" ", String.Empty); // remove blanks
            cmd = Convert.ToInt32(ar[0].ToString(), 16);
            /*

            0   Write   Word
            1   Write   Short
            2   Write   Byte

            3   X > Y   
            4   X < Y
            5   X = Y
            6   X ~ Y

            B   OFFSET = READ(X)
            D3  OFFSET = X
            */

            if (cmd == 0xD)
            {
                cmd = 0xD0;
                cmd += Convert.ToInt32(ar[1].ToString(), 16); // DX codes
                block_a = 0x0;
            } else
            {
                block_a = Convert.ToInt32(blocks[0], 16);
                block_a -= cmd * 0x10000000;
            }
            

            block_b = Convert.ToUInt32(blocks[1], 16);
            
        }

        public Int32 getCMD()
        {
            return cmd;
        }

        public Int32 getBlock_A()
        {
            return block_a;
        }

        public UInt32 getBlock_B()
        {
            return block_b;
        }

        public void execute(Int32 offset)
        {
            if ((cmd == 0) || (cmd == 1) || (cmd == 2))
            {
                int len = 4;
                if (cmd == 1) len = 2;
                else if (cmd == 2) len = 1;
                String cmd_string = Program.gCmdWindow.generateWriteString(block_a+offset, block_b, len);
                Program.gCmdWindow.runCmd(cmd_string);
            }
        }
    }
}
