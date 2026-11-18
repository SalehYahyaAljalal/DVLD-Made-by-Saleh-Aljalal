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
using static System.Net.Mime.MediaTypeNames;

namespace DVLDProject
{
    public partial class frmListInternationalLicenses : Form
    {
        private DataTable _dtAllInternationalLicenses;
  
        public frmListInternationalLicenses()
        {
            InitializeComponent();
        }

        private void frmListInternationalLicenses_Load(object sender, EventArgs e)
        {
            _dtAllInternationalLicenses = clsInternationalLicense.GetAllInternationalsLicenses();
            cmbFiltter.SelectedIndex = 0;

            dgvInternationalLicenses.DataSource = _dtAllInternationalLicenses;
            lblRecords.Text = _dtAllInternationalLicenses.Rows.Count.ToString();

            if (dgvInternationalLicenses.Rows.Count > 0)
            {
                dgvInternationalLicenses.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicenses.Columns[0].Width = 160;

                dgvInternationalLicenses.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicenses.Columns[1].Width = 150;

                dgvInternationalLicenses.Columns[2].HeaderText = "Driver ID";
                dgvInternationalLicenses.Columns[2].Width = 130;

                dgvInternationalLicenses.Columns[3].HeaderText = "L.License ID";
                dgvInternationalLicenses.Columns[3].Width = 130;

                dgvInternationalLicenses.Columns[4].HeaderText = "Issue Date";
                dgvInternationalLicenses.Columns[4].Width = 180;

                dgvInternationalLicenses.Columns[5].HeaderText = "Expiration Date";
                dgvInternationalLicenses.Columns[5].Width = 180;

                dgvInternationalLicenses.Columns[6].HeaderText = "Is Active";
                dgvInternationalLicenses.Columns[6].Width = 120;

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
                
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);


        }

        private void cmbFiltter_SelectedIndexChanged(object sender, EventArgs e)
        {


            if(cmbFiltter.Text== "Is Active")
            {
                txtFilter.Visible = false;
                cbIsActive.Visible=true;
                cbIsActive.Focus();
                cbIsActive.SelectedIndex = 0;
                
            }
            else
            {
                txtFilter.Visible = (cmbFiltter.Text != "None");
                cbIsActive.Visible = false;

                if(cmbFiltter.Text=="None")
                {
                    txtFilter.Visible = false;
                }
                else
                    txtFilter.Visible = true;

                txtFilter.Text = "";
                txtFilter.Focus();

            }


        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {


            string FilterColumn = "IsActive";
            string FilterValue = "Yes";

            switch(cbIsActive.Text)
            {
                case "All":
                    FilterValue = "All";
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;


            }


            if(FilterValue=="All")
            {
                _dtAllInternationalLicenses.DefaultView.RowFilter = "";
            }
            else
               _dtAllInternationalLicenses.DefaultView.RowFilter = String.Format("[{0}] = {1}", FilterColumn, FilterValue);
            

            lblRecords.Text = _dtAllInternationalLicenses.Rows.Count.ToString();

        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            //            None
            //International License ID
            //Application ID
            //Driver ID
            //Local License ID
            //Is Active


            //InternationalLicenseID,ApplicationID,DriverID,
            //                  IssuedUsingLocalLicenseID, IssueDate,ExpirationDate,IsActive
            //              from InternationalLicenses

            string FilterColumn = "";

            switch(cmbFiltter.Text)
            {
                case "None":
                    FilterColumn = "None";
                    break;

                case "International License ID":
                    FilterColumn = "InternationalLicenseID";
                    break;
                case "Application ID":
                    FilterColumn = "ApplicationID";
                    break;
                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;
                case "Local License ID":
                    FilterColumn = "IssuedUsingLocalLicenseID";
                    break;


            }

            if(txtFilter.Text .Trim()== "" || FilterColumn=="None")
            {
                _dtAllInternationalLicenses.DefaultView.RowFilter = "";
                lblRecords.Text = _dtAllInternationalLicenses.Rows.Count.ToString();
                return;
            }

            _dtAllInternationalLicenses.DefaultView.RowFilter = String.Format("[{0}] = {1}", FilterColumn,txtFilter.Text.Trim());
            lblRecords.Text = _dtAllInternationalLicenses.Rows.Count.ToString();






        }

        private void btnAddNewInternationalLicnese_Click(object sender, EventArgs e)
        {
            frmAddNewInternationalLicenseLicense frm = new frmAddNewInternationalLicenseLicense();
            frm.ShowDialog();

            frmListInternationalLicenses_Load(null,null);

        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID =(int)dgvInternationalLicenses.CurrentRow.Cells[2].Value ;
            int PersonID=clsDrivers.FindDriverInfoByDriverID(DriverID).PersonID; 

            frmPersonInformation frm = new frmPersonInformation(PersonID);
            frm.ShowDialog();
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int InternationalLicenseID = (int)dgvInternationalLicenses.CurrentRow.Cells[0].Value;

            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(InternationalLicenseID);
            frm.ShowDialog();


        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvInternationalLicenses.CurrentRow.Cells[2].Value;
            int PersonID = clsDrivers.FindDriverInfoByDriverID(DriverID).PersonID;

            frmDriversLicensesHistory frm = new frmDriversLicensesHistory(PersonID);
            frm.ShowDialog();

        }
    }
}
