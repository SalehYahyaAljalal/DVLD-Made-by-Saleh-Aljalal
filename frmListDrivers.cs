using BussnissLayerDVLD;
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
    public partial class frmListDrivers : Form
    {
        private DataTable _dtAllDrivers;

        public frmListDrivers()
        {
            InitializeComponent();
        }

        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            cbFillterBy.SelectedIndex = 0;

            _dtAllDrivers = clsDrivers.GetAllDrivers();
            dgvDrivers.DataSource = _dtAllDrivers;
            lblRecords.Text = _dtAllDrivers.Rows.Count.ToString();

            if (_dtAllDrivers.Rows.Count > 0)
            {
                dgvDrivers.Columns[0].HeaderText = "Driver ID";
                dgvDrivers.Columns[0].Width = 90;

                dgvDrivers.Columns[1].HeaderText = "Person ID";
                dgvDrivers.Columns[1].Width = 90;

                dgvDrivers.Columns[2].HeaderText = "National NO";
                dgvDrivers.Columns[2].Width = 90;

                dgvDrivers.Columns[3].HeaderText = "Full Name";
                dgvDrivers.Columns[3].Width = 300;

                dgvDrivers.Columns[4].HeaderText = "Create Date";
                dgvDrivers.Columns[4].Width = 150;

                dgvDrivers.Columns[5].HeaderText = "Active License";
                dgvDrivers.Columns[5].Width = 130;


            }



        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            //None
            //Person ID
            //Driver ID
            //National No
            //Full Name

            switch (cbFillterBy.Text)
            {
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;

                case "National No":
                    FilterColumn = "NationalNo";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                default:
                    FilterColumn = "PersonID";
                    break;

            }

            if (cbFillterBy.Text == "None" || txtFilter.Text == "")
            {
                _dtAllDrivers.DefaultView.RowFilter = "";
                lblRecords.Text = _dtAllDrivers.DefaultView.Count.ToString();

                return;
            }


            if (cbFillterBy.Text == "Driver ID" || cbFillterBy.Text == "Person ID")
                _dtAllDrivers.DefaultView.RowFilter = String.Format("[{0}]  = {1} ", FilterColumn, txtFilter.Text.Trim());

            else
                _dtAllDrivers.DefaultView.RowFilter = String.Format("[{0}]  LIKE '{1}%' ", FilterColumn, txtFilter.Text.Trim());


            lblRecords.Text = _dtAllDrivers.DefaultView.Count.ToString();

        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFillterBy.Text == "Driver ID" || cbFillterBy.Text == "Person ID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }

        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvDrivers.CurrentRow.Cells[1].Value;
            frmPersonInformation frm = new frmPersonInformation(PersonID);

            frm.ShowDialog();


        }

        private void cbFillterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
             txtFilter.Visible=  !(cbFillterBy.Text == "None");

            if (cbFillterBy.Text != "None")
                txtFilter.Visible = true ;



        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Not Impelement Yet!!!");

            int PersonID = (int)dgvDrivers.CurrentRow.Cells[1].Value;
            frmDriversLicensesHistory frm = new frmDriversLicensesHistory(PersonID);
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
