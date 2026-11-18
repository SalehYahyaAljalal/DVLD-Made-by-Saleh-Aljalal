using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using BussnissLayerDVLD;

namespace DVLDProject
{
    public partial class ctrlDriverLicenses : UserControl
    {
        private int _DriverID;
        private clsDrivers _Driver;
        private DataTable _dtDriverLocalLicensesHistory;
        private DataTable _dtDriverInternationalLicensesHistory;


        private void _LoadLocalLicenseAccordingToDriverID()
        {
            _dtDriverLocalLicensesHistory = clsDrivers.GetLocaldriverslicenses(_DriverID);


            dgvLocalLicensesHistory.DataSource = _dtDriverLocalLicensesHistory;
            lblRecords.Text = dgvLocalLicensesHistory.Rows.Count.ToString();

            if (dgvLocalLicensesHistory.Rows.Count > 0)
            {
                dgvLocalLicensesHistory.Columns[0].HeaderText = "Lic.ID";
                dgvLocalLicensesHistory.Columns[0].Width = 110;

                dgvLocalLicensesHistory.Columns[1].HeaderText = "App.ID";
                dgvLocalLicensesHistory.Columns[1].Width = 110;

                dgvLocalLicensesHistory.Columns[2].HeaderText = "Class Name";
                dgvLocalLicensesHistory.Columns[2].Width = 270;

                dgvLocalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicensesHistory.Columns[3].Width = 170;

                dgvLocalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicensesHistory.Columns[4].Width = 170;

                dgvLocalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvLocalLicensesHistory.Columns[5].Width = 110;

            }
        }

        private void _LoadInternationalLicenseHistory()
        {
            _dtDriverInternationalLicensesHistory = clsDrivers.GetInternationaldriverslicenses(_DriverID);


            dgvInternationalLicensesHistory.DataSource = _dtDriverInternationalLicensesHistory;
            lblRecords.Text = dgvInternationalLicensesHistory.Rows.Count.ToString();

            if (dgvInternationalLicensesHistory.Rows.Count > 0)
            {
                dgvInternationalLicensesHistory.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicensesHistory.Columns[0].Width = 160;

                dgvInternationalLicensesHistory.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicensesHistory.Columns[1].Width = 130;

                dgvInternationalLicensesHistory.Columns[2].HeaderText = "L.License ID";
                dgvInternationalLicensesHistory.Columns[2].Width = 130;

                dgvInternationalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvInternationalLicensesHistory.Columns[3].Width = 180;

                dgvInternationalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvInternationalLicensesHistory.Columns[4].Width = 180;

                dgvInternationalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvInternationalLicensesHistory.Columns[5].Width = 120;

            }


        }


        public void LoadDriversLicensesByDriverID(int DriverID) 
        {
            _Driver=clsDrivers.FindDriverInfoByDriverID(DriverID);
            if (_Driver == null)
            {
                MessageBox.Show("No Licenses Linked to This Driver !", "Not Found!!", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                return;
            }
            _DriverID = _Driver.DriverID;

            _LoadLocalLicenseAccordingToDriverID();
            _LoadInternationalLicenseHistory();

        }



        public void LoadDriversLicensesByPersonID(int PersonID)
        {
            _Driver = clsDrivers.FindDriverInfoByPersonID(PersonID);

            if(_Driver == null )
            {
                MessageBox.Show("No Licenses Linked to This Person !", "Not Found!!", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                return;
            }
            _DriverID = _Driver.DriverID;

            _LoadLocalLicenseAccordingToDriverID();
            _LoadInternationalLicenseHistory();

        }

        public void Clear()
        {
            _dtDriverInternationalLicensesHistory.Clear();
            _dtDriverLocalLicensesHistory.Clear (); 
        }


        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }

        private void showLocalLincenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvLocalLicensesHistory.CurrentRow.Cells[0].Value;

            frmLicenseInfo frm = new frmLicenseInfo(LicenseID);
            frm.ShowDialog();


        }

        private void showInternationalLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int InternationalLicenseID = (int)dgvInternationalLicensesHistory.CurrentRow.Cells[0].Value;

            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(InternationalLicenseID);
            frm.ShowDialog();


        }
    }
}
