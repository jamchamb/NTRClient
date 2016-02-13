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
        Boolean loop;
        int loop_index;
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
            do
            {
                gateshark_ar gs_ar = lines[index];
                int cmd = gs_ar.getCMD();
                if ((cmd == 0) || (cmd == 1) || (cmd == 2))
                {
                    gs_ar.execute(offset);
                }
                else if (cmd == 0xB)
                {
                    offset = CmdWindow.fromLE(Program.gCmdWindow.readValue(gs_ar.getBlock_A(), 4), 4);
                    //MessageBox.Show(String.Format("SET > O: {0:X}", offset));
                }
                else if (cmd == 0xC)
                {
                    loop = true;
                    loop_index = index;
                    loop_count = gs_ar.getBlock_B()+1;
                }
                else if (cmd == 0xD1)
                {
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
                else if (cmd == 0xD3)
                {
                    offset = Convert.ToInt32(gs_ar.getBlock_B());
                }
                else if (cmd == 0xDC)
                {
                    offset += Convert.ToInt32(gs_ar.getBlock_B());
                }
                else if (cmd == 0xDF)
                {
                    // This doesn't actually exist! It's for testing only!
                    dummy_count++;
                    MessageBox.Show(String.Format("DUMMY: {0:X}", dummy_count));
                }
                //MessageBox.Show(String.Format("I: {0} O: {1:X} \r\nL:{2}-{3} LI:{4}", index, offset, loop_count, loop, loop_index));
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
            String[] blocks = ar.Split(' ');

            if (ar.Length != 17)
            {
                cmd = 0x0f;
                block_a = 0x0fffffff;
                block_b = 0xffffffff;
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

            B   OFFSET += READ(X)
            D3  OFFSET = X
            DC  OFFSET2 ?
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
                if ((cmd == 1) || (cmd == 2)) len = 2;
                String cmd_string = Program.gCmdWindow.generateWriteString(block_a+offset, block_b, len);
                Program.gCmdWindow.runCmd(cmd_string);
            }
        }
    }
}
