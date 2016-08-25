using System;
using System.IO;
using System.Windows.Forms;
using ntrclient.Prog.CS;

namespace ntrclient.Prog.Window
{
    public partial class AsmEditWindow : Form
    {
        private const string AsPath = "arm-none-eabi-as";
        private const string OcPath = "arm-none-eabi-objcopy";
        private const string LdPath = "arm-none-eabi-ld";
        private byte[] _compileResult;

        public AsmEditWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CompileAsmCode();
        }

        public static bool CallToolchain(string asOpts, string ldOpts, string ocOpts, ref string result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            string output = null;

            result = "";
            var ret = Utility.RunCommandAndGetOutput(AsPath, asOpts, ref output);
            result += AsPath + asOpts + "\r\n" + output + "\r\n";
            if (ret != 0) return false;

            ret = Utility.RunCommandAndGetOutput(LdPath, ldOpts, ref output);
            result += LdPath + ldOpts + "\r\n" + output + "\r\n";
            if (ret != 0) return false;

            ret = Utility.RunCommandAndGetOutput(OcPath, ocOpts, ref output);
            result += OcPath + ocOpts + "\r\n" + output + "\r\n";
            if (ret != 0) return false;

            return true;
        }

        public void CompileAsmCode()
        {
            _compileResult = null;
            string asmCode = txtAsmText.Text;
            string[] instructOpts = comboBox1.Text.Split(',');
            string arch = instructOpts[0];
            string asOpts = " ";
            string ldOpts = " ";
            string ocOpts = " ";
            uint baseAddr = Convert.ToUInt32(textBox1.Text, 16);

            File.WriteAllText("payload.s", asmCode);

            asOpts += "-o payload.o -mlittle-endian";
            asOpts += " -march=" + arch;
            if (instructOpts.Length > 1)
            {
                if (instructOpts[1] == "thumb")
                {
                    asOpts += " -mthumb";
                }
            }
            asOpts += " payload.s";
            ldOpts += " -Ttext 0x" + baseAddr.ToString("X8") + " payload.o";
            ocOpts += " -I elf32-little -O binary a.out payload.bin ";

            string result = "";
            bool isSuccessed = CallToolchain(asOpts, ldOpts, ocOpts, ref result);
            if (!isSuccessed)
            {
                result += "compile failed...";
            }
            else
            {
                _compileResult = File.ReadAllBytes("payload.bin");
                result += "result: \r\n" + Utility.ConvertByteArrayToHexString(_compileResult);
            }
            textBox2.Text = result;
        }
    }
}