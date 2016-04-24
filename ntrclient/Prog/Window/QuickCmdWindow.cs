using System.Windows.Forms;
using ntrclient.Prog.CS;

namespace ntrclient.Prog.Window
{
    public partial class QuickCmdWindow : Form
    {
        public QuickCmdWindow()
        {
            InitializeComponent();

            LoadCmds();
        }

        public void LoadCmds()
        {
            for (int i = 0; i <= 9; i++)
            {
                string[] t = new string[2];
                t[0] = i.ToString();
                t[1] = Program.Sm.QuickCmds[i];
                // ReSharper disable once CoVariantArrayConversion
                dataGridView1.Rows.Add(t);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            string t = (string) dataGridView1.Rows[e.RowIndex].Cells[1].Value;
            string id = (string) dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            Program.Sm.QuickCmds[int.Parse(id)] = t;
        }
    }
}