using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BussnissLayerDVLD;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLDProject
{
    public partial class frmManageDetainedLicense : Form
    {

        DataTable _dtListDetainedLicenses;
        public frmManageDetainedLicense()
        {
            InitializeComponent();
        }

        private void frmManageDetainedLicense_Load(object sender, EventArgs e)
        {
            cmbFiltter.SelectedIndex = 0;
            _dtListDetainedLicenses=clsDetainLicense.GetAllDetainedLicenses();

            dgvDetainLicenses.DataSource = _dtListDetainedLicenses;

            lblRecords.Text = dgvDetainLicenses.Rows.Count.ToString();



            if(_dtListDetainedLicenses.Rows.Count>0)
            {
                dgvDetainLicenses.Columns[0].HeaderText = "D  ID";
                dgvDetainLicenses.Columns[0].Width = 90;

                dgvDetainLicenses.Columns[1].HeaderText = "L ID";
                dgvDetainLicenses.Columns[1].Width = 90;


                dgvDetainLicenses.Columns[2].HeaderText = "D Date";
                dgvDetainLicenses.Columns[2].Width = 140;

             

                dgvDetainLicenses.Columns[3].HeaderText = "Is Released";
                dgvDetainLicenses.Columns[3].Width = 100;

                dgvDetainLicenses.Columns[4].HeaderText = "Fine Fees ";
                dgvDetainLicenses.Columns[4].Width = 90;

                dgvDetainLicenses.Columns[5].HeaderText = "Released Date ";
                dgvDetainLicenses.Columns[5].Width = 140;

                dgvDetainLicenses.Columns[6].HeaderText = "N NO ";
                dgvDetainLicenses.Columns[6].Width = 90;

                dgvDetainLicenses.Columns[7].HeaderText = "Full Name";
                dgvDetainLicenses.Columns[7].Width = 200;

                dgvDetainLicenses.Columns[8].HeaderText = "Release App ID";
                dgvDetainLicenses.Columns[8].Width = 110;


            }



        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            //None
            //Detain ID
            //National No
            //Full Name
            //Release Application ID
            //Status

            string FilterColumn = "";


            switch(cmbFiltter.Text)
            {
                case "Detain ID":
                    FilterColumn = "DetainID";
                    break;
                case "National No":
                    FilterColumn = "NationalNo";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "Release Application ID":
                    FilterColumn = "ReleaseApplicationID";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }

            if(FilterColumn == "None" || txtFilter.Text.Trim() == "")
            {
                _dtListDetainedLicenses.DefaultView.RowFilter = "";
                lblRecords.Text = _dtListDetainedLicenses.Rows.Count.ToString();

                return;
            }

            if(cmbFiltter.Text== "Full Name" || cmbFiltter.Text == "National No")
                _dtListDetainedLicenses.DefaultView.RowFilter=String.Format("[{0}] LIKE '{1}%'",FilterColumn,txtFilter.Text);
            else
            {
                _dtListDetainedLicenses.DefaultView.RowFilter = String.Format("[{0}] = {1}", FilterColumn, txtFilter.Text);
            }


            lblRecords.Text= _dtListDetainedLicenses.Rows.Count.ToString(); 

        }

        private void cmbFiltter_SelectedIndexChanged(object sender, EventArgs e)
        {

         
            if(cmbFiltter.Text== "Is Released")
            {
                txtFilter.Visible = false;
                cbIsReleased.Visible = true;
                cbIsReleased.Focus();
                cbIsReleased.SelectedIndex = 0;

            }
            else
            {
                txtFilter.Visible = (cmbFiltter.Text != "None");
                cbIsReleased.Visible = false;

                if(cmbFiltter.Text=="None")
                {
                    txtFilter.Enabled=false;


                }
                else
                {
                    txtFilter.Enabled = true ;
                    txtFilter.Text = "";
                    txtFilter.Focus();

                }


            }





            // My Solution
            //if (cmbFiltter.Text=="None")
            //{
            //    cbIsReleased.Visible = txtFilter.Visible = false;
            //    return;
            //}
           
            //if(cmbFiltter.Text== "Status")
            //{
            //    txtFilter.Visible = !(cbIsReleased.Visible = true);
            //    txtFilter.Text = "";
            //    cbIsReleased.Focus();
            //}
            //else
            //{
            //    cbIsReleased.Visible = !(txtFilter.Visible = true);
            //    cbIsReleased.SelectedIndex=0;
            //    cbIsReleased.Focus();
            //}


        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsReleased";
            string FilterValue = "";

            switch(cbIsReleased.Text)
            {
                case "All":
                    break;

                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;


            }

            if(cbIsReleased.Text=="All")
            {
                _dtListDetainedLicenses.DefaultView.RowFilter = "";
            }
            else
                _dtListDetainedLicenses.DefaultView.RowFilter =String.Format("[{0}] = {1}",FilterColumn,FilterValue);



          lblRecords.Text = _dtListDetainedLicenses.Rows.Count.ToString();

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
             bool IsReleased = (bool)dgvDetainLicenses.CurrentRow.Cells[3].Value;

             releaseDetainedLicenseToolStripMenuItem.Enabled = !IsReleased;


           
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvDetainLicenses.CurrentRow.Cells[1].Value;

            frmReleaseLicense frm = new frmReleaseLicense(LicenseID);
            frm.ShowDialog();


        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();

        }

        private void btnReleaseLicense_Click(object sender, EventArgs e)
        {
            frmReleaseLicense frm = new frmReleaseLicense();
            frm.ShowDialog();

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvDetainLicenses.CurrentRow.Cells[1].Value;


            int PersonID =clsLicense.FindByID(LicenseID).Driverinfo.PersonID;

            frmDriversLicensesHistory frm = new frmDriversLicensesHistory(PersonID);
            frm.ShowDialog();


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (cmbFiltter.Text == "Release Application ID" || cmbFiltter.Text == "Detain ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);


        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int LicenseID = (int)dgvDetainLicenses.CurrentRow.Cells[1].Value;

            frmLicenseInfo frm = new frmLicenseInfo(LicenseID);
            frm.ShowDialog();


        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvDetainLicenses.CurrentRow.Cells[1].Value;


            int PeronsID = clsLicense.FindByID(LicenseID).Driverinfo.PersonID;
            frmPersonInformation frm = new frmPersonInformation(PeronsID);
            frm.ShowDialog();



        }
    }
}
