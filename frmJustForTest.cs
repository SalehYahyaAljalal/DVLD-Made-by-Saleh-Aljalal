using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDProject
{
    public partial class frmJustForTest : Form
    {
        public frmJustForTest()
        {
            InitializeComponent();
        }
        private void DataBack(object obj,int PersonID)
        {
            label1.Text = PersonID.ToString();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            frmFind frm = new frmFind();
            frm.DataBack += DataBack;
            frm.ShowDialog();
        }
    }
}
