using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BussnissLayerDVLD;

namespace DVLDProject
{
    public partial class frmApplicationTypes : Form
    {
        public frmApplicationTypes()
        {
            InitializeComponent();
        }

        private void _LoadApplicationsTypes()
        {
            dgvApplicationsTypes.DataSource = clsApplicationsTypes.GellAllApplicationTypes();

        }



        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmApplicationTypes_Load(object sender, EventArgs e)
        {
            _LoadApplicationsTypes();
        }

        private void updateApplicationsTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmUpdateApplicationsTypes frm = 
                new frmUpdateApplicationsTypes((int)dgvApplicationsTypes.CurrentRow.Cells[0].Value);

            frm.ShowDialog();

            _LoadApplicationsTypes();

        }

        private void ctrlPersonCardWithFilter1_Load(object sender, EventArgs e)
        {

        }
    }
}
