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
    public partial class frmFind : Form
    {
        public delegate void DataBackEventHandler(object sender,int PersonID);
        public event DataBackEventHandler DataBack;


        public frmFind()
        {
            InitializeComponent();
        }

        private void btnCLose_Click(object sender, EventArgs e)
        {

            DataBack?.Invoke(this, ctrlPersonCardWithFilter1.PersonID);
          this.Close();

        }

        private void frmFind_Load(object sender, EventArgs e)
        {

        }
    }
}
