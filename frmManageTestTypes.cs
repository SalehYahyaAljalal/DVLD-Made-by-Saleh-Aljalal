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
    public partial class frmManageTestTypes : Form
    {
        private DataTable _dtTestsTypes;

        public frmManageTestTypes()
        {
            InitializeComponent();
        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageTestTypes_Load(object sender, EventArgs e)
        {
            _dtTestsTypes = clsTestType.GetAllTestTypes();
            dgvTestTypes.DataSource = _dtTestsTypes;
            lblRecords.Text = _dtTestsTypes.Rows.Count.ToString();


            dgvTestTypes.Columns[0].HeaderText = "ID";
            dgvTestTypes.Columns[0].Width = 120;

            dgvTestTypes.Columns[1].HeaderText = "Title";
            dgvTestTypes.Columns[1].Width = 200;

            dgvTestTypes.Columns[2].HeaderText = "Description";
            dgvTestTypes.Columns[2].Width = 400;

            dgvTestTypes.Columns[3].HeaderText = "Fees";
            dgvTestTypes.Columns[3].Width = 100;




        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void updateTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestTypeID = (int)dgvTestTypes.CurrentRow.Cells[0].Value;

            frmUpdateTestType frmup = new frmUpdateTestType(TestTypeID);

            frmup.ShowDialog();


            frmManageTestTypes_Load(null,null);

        }
    }
}
