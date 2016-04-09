using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ntrclient
{
    public class gateshark
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

        static void addlog(String n)
        {
            Program.gCmdWindow.Addlog(n);
        }

        public void execute()
        {
            int index = 0;
            int dummy_count = 0;
            Boolean gs_if = true;
            Boolean valid = true;
            Int32 gs_if_layer = 0;
            Int32 gs_if_sLayer = 0;
            do
            {
                gateshark_ar gs_ar = lines[index];
                int cmd = gs_ar.getCMD();

                if (cmd != 0xff)
                    addlog(String.Format("GS | {0:X} {1:X} {2:X} -> [{3}, {4}, {5}, {6:X}]", cmd, gs_ar.getBlock_A(), gs_ar.getBlock_B(), valid, gs_if_layer, gs_if_sLayer, offset));
                    
                if (gs_if_layer == 0 && valid)
                {

                    if ((cmd == 0) || (cmd == 1) || (cmd == 2))
                    {
                        Program.sm.gs_used += 1;
                        valid = gs_ar.execute(offset);
                        
                    }
                    // Conditional codes
                    else if (cmd == 0x3)
                    {
                        UInt32 read = Convert.ToUInt32(CmdWindow.fromLE(Program.gCmdWindow.readValue(gs_ar.getBlock_A() + offset, 4), 0));
                        gs_if = (read < gs_ar.getBlock_B());
                    }
                    else if (cmd == 0x4)
                    {
                        UInt32 read = Convert.ToUInt32(CmdWindow.fromLE(Program.gCmdWindow.readValue(gs_ar.getBlock_A() + offset, 4), 0));
                        gs_if = (read > gs_ar.getBlock_B());
                    }
                    else if (cmd == 0x5)
                    {
                        UInt32 read = Convert.ToUInt32(CmdWindow.fromLE(Program.gCmdWindow.readValue(gs_ar.getBlock_A() + offset, 4), 0));
                        gs_if = (read == gs_ar.getBlock_B());
                    }
                    else if (cmd == 0x6)
                    {
                        UInt32 read = Convert.ToUInt32(CmdWindow.fromLE(Program.gCmdWindow.readValue(gs_ar.getBlock_A() + offset, 4), 0));
                        gs_if = (read != gs_ar.getBlock_B());
                    }
                    else if (cmd == 0x7)
                    {
                        UInt32 read = Convert.ToUInt32(CmdWindow.fromLE(Program.gCmdWindow.readValue(gs_ar.getBlock_A() + offset, 2), 2));
                        gs_if = (read < gs_ar.getBlock_B());
                    }
                    else if (cmd == 0x8)
                    {
                        UInt32 read = Convert.ToUInt32(CmdWindow.fromLE(Program.gCmdWindow.readValue(gs_ar.getBlock_A() + offset, 2), 2));
                        gs_if = (read > gs_ar.getBlock_B());
                    }
                    else if (cmd == 0x9)
                    {
                        UInt32 read = Convert.ToUInt32(CmdWindow.fromLE(Program.gCmdWindow.readValue(gs_ar.getBlock_A() + offset, 2), 2));
                        gs_if = (read == gs_ar.getBlock_B());
                    }
                    else if (cmd == 0xA)
                    {
                        UInt32 read = Convert.ToUInt32(CmdWindow.fromLE(Program.gCmdWindow.readValue(gs_ar.getBlock_A() + offset, 2), 2));
                        gs_if = (read != gs_ar.getBlock_B());
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
                    else if (cmd == 0xD2) // RESET
                    {
                        loop = false;
                        loop_count = 0;
                        loop_index = 0;

                        offset = 0;
                        gs_if_layer = 0;
                        gs_if_sLayer = 0;
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
                    else if (cmd == 0xD4)
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


                        dxData += b_;
                    }
                    else if (cmd == 0xD5) // DxData WRITE
                    {
                        dxData = Convert.ToInt32(gs_ar.getBlock_B());
                    }
                    else if (cmd == 0xD6) // DxData WORD
                    {

                        int addr = Convert.ToInt32(gs_ar.getBlock_B()) + offset;
                        dxData = CmdWindow.fromLE(Program.gCmdWindow.readValue(addr, 4), 4);
                    }
                    else if (cmd == 0xD7) // DxData SHORT
                    {
                        int addr = Convert.ToInt32(gs_ar.getBlock_B()) + offset;
                        dxData = CmdWindow.fromLE(Program.gCmdWindow.readValue(addr, 2), 2);
                    }
                    else if (cmd == 0xD8) // DxData Byte
                    {
                        int addr = Convert.ToInt32(gs_ar.getBlock_B()) + offset;
                        dxData = Program.gCmdWindow.readValue(addr, 1);
                    }
                    else if (cmd == 0xD9) // DxData READ WORD
                    {
                        int len = 4;
                        String cmd_string = Program.gCmdWindow.generateWriteString(Convert.ToInt32(gs_ar.getBlock_B()) + offset, dxData, len);
                        Program.gCmdWindow.runCmd(cmd_string);
                        offset += len;
                    }
                    else if (cmd == 0xDA) // DxData READ SHORT
                    {
                        int len = 2;
                        String cmd_string = Program.gCmdWindow.generateWriteString(Convert.ToInt32(gs_ar.getBlock_B()) + offset, dxData, len);
                        Program.gCmdWindow.runCmd(cmd_string);
                        offset += len;
                    }
                    else if (cmd == 0xDB) // DxData READ BYTE
                    {
                        int len = 1;
                        String cmd_string = Program.gCmdWindow.generateWriteString(Convert.ToInt32(gs_ar.getBlock_B()) + offset, dxData, len);
                        Program.gCmdWindow.runCmd(cmd_string);
                        offset += len;
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
                            "DUMMY: {6}\r\n" +
                            "LAYERS: {7} {8}"
                            , index, offset, loop, loop_index, loop_count, dxData, dummy_count, gs_if_sLayer, gs_if_layer));
                    }

                    if (!gs_if)
                    {
                        gs_if = true;
                        gs_if_layer += 1;
                    }
                }
                else if (cmd >= 0x3 && cmd <= 0xA)
                {
                    gs_if_sLayer += 1;
                }
                else if (cmd == 0xD0)
                {
                    if (gs_if_sLayer > 0)
                        gs_if_sLayer -= 1;
                    else if (gs_if_layer > 0)
                        gs_if_layer -= 1;
                } else if (cmd == 0xD2)
                {
                    loop = false;
                    loop_count = 0;
                    loop_index = 0;

                    offset = 0;
                    gs_if_layer = 0;
                    gs_if_sLayer = 0;
                }
                index++;
            } while (index < lines.Count);
        }

        public gateshark_ar[] getAllCodes()
        {
            return lines.ToArray();
        }
    }

    public class gateshark_ar
    {
        String line;
        Int32 cmd;
        Int32 block_a;
        UInt32 block_b;
        public gateshark_ar(String ar)
        {
            line = ar;
            String[] blocks = ar.Split(' ');

            if (ar.Length != 17)
            {
                cmd = 0xff;
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

        public bool execute(Int32 offset)
        {
            if ((cmd == 0) || (cmd == 1) || (cmd == 2))
            {
                int len = 4;
                if (cmd == 1) len = 2;
                else if (cmd == 2) len = 1;
                String cmd_string = "";
                if (Program.gCmdWindow.isMemValid(block_a + offset))
                {
                    cmd_string = Program.gCmdWindow.generateWriteString(block_a + offset, block_b, len);
                    Program.gCmdWindow.runCmd(cmd_string);
                    return true;
                }
            }
            return false;
        }
    }
}
