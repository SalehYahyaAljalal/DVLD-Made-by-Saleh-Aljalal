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
    public partial class frmListLocalDrivingLicenseApplications : Form
    {
        private DataTable _dtLocalDrivingLicenseApplication;
        public frmListLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frlListLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            _dtLocalDrivingLicenseApplication = clsLocalDrvingLicenseApplications.GetAllLocalDrivingLicenseApplications();
            dgvLDLApp.DataSource = _dtLocalDrivingLicenseApplication;   

            if(_dtLocalDrivingLicenseApplication.Rows.Count>0)
            {
                dgvLDLApp.Columns[0].HeaderText = "LDLApp ID";
                dgvLDLApp.Columns[0].Width = 120;

                dgvLDLApp.Columns[1].HeaderText = "Driving Class";
                dgvLDLApp.Columns[1].Width = 230;


                dgvLDLApp.Columns[2].HeaderText = "National NO";
                dgvLDLApp.Columns[2].Width = 120;

                dgvLDLApp.Columns[3].HeaderText = "Full Name";
                dgvLDLApp.Columns[3].Width = 350;

                dgvLDLApp.Columns[4].HeaderText = "Application Date";
                dgvLDLApp.Columns[4].Width = 170;

                dgvLDLApp.Columns[5].HeaderText = "Passed Tests";
                dgvLDLApp.Columns[5].Width = 90;

                dgvLDLApp.Columns[6].HeaderText = "Status";
                dgvLDLApp.Columns[6].Width = 90;



            }


            lblRecords.Text= _dtLocalDrivingLicenseApplication.Rows.Count.ToString();
            cmbFiltter.SelectedIndex = 0;

        }

        private void cmbFiltter_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtFilter.Visible = (cmbFiltter.Text != "None");

            if (txtFilter.Visible)
            {
                txtFilter.Text = "";
                txtFilter.Focus();

             }

            _dtLocalDrivingLicenseApplication.DefaultView.RowFilter = "";
            lblRecords.Text = _dtLocalDrivingLicenseApplication.Rows.Count.ToString();

        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            //            None
            //            L.D.L.App ID
            //            National No
            //            Full Name
            //            Status

        


            switch (cmbFiltter.Text)
            {
               case "L.D.L.App ID":
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    break;
                case "National No":
                    FilterColumn = "NationalNo";
                    break ;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "Status":
                    FilterColumn = "Status";
                    break;


            }

            if(txtFilter.Text.Trim()=="" && FilterColumn=="None")
            {
                _dtLocalDrivingLicenseApplication.DefaultView.RowFilter = "";
            lblRecords.Text = _dtLocalDrivingLicenseApplication.Rows.Count.ToString();

                return;
            }


            if (FilterColumn == "LocalDrivingLicenseApplicationID")
                _dtLocalDrivingLicenseApplication.DefaultView.RowFilter = String.Format("[{0}] = {1}", FilterColumn,txtFilter.Text.Trim());

            else
                _dtLocalDrivingLicenseApplication.DefaultView.RowFilter = String.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text);



            lblRecords.Text = _dtLocalDrivingLicenseApplication.Rows.Count.ToString();

        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFiltter.Text == "L.D.L.App ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);


        }

        private void canceleApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure that You Want to Cancell this APplications ?", "Confirm",
                MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
                return;

            int LocalApplicationID = (int)dgvLDLApp.CurrentRow.Cells[0].Value;

            clsLocalDrvingLicenseApplications LocalDrvingLicenseApplicationsInfo=clsLocalDrvingLicenseApplications
                .FindByLocalDrivingApplicationID(LocalApplicationID);

            if(LocalDrvingLicenseApplicationsInfo!=null)
            {
                if(LocalDrvingLicenseApplicationsInfo.Cancell())
                {
                    MessageBox.Show("Application Cancelled Successfully","cancelled",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    frlListLocalDrivingLicenseApplications_Load(null,null);
                }
                else
                    MessageBox.Show("Error:Field to  Cancelle Application  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void showApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDVLAppID = (int)dgvLDLApp.CurrentRow.Cells[0].Value;

            frmShowApplicationInfo frm = new frmShowApplicationInfo(LDVLAppID);
            frm.ShowDialog();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmAddEditLocalDrivingApplication frm = new frmAddEditLocalDrivingApplication();

            frm.ShowDialog();

            frlListLocalDrivingLicenseApplications_Load(null,null);
        
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure that You Want to Delete this APplications ?", "Confirm",
    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            int LocalApplicationID = (int)dgvLDLApp.CurrentRow.Cells[0].Value;

            clsLocalDrvingLicenseApplications LocalDrvingLicenseApplicationsInfo = clsLocalDrvingLicenseApplications
                .FindByLocalDrivingApplicationID(LocalApplicationID);

            if (LocalDrvingLicenseApplicationsInfo != null)
            {
                if (LocalDrvingLicenseApplicationsInfo.Delete())
                {
                    MessageBox.Show("Application Deleted Successfully", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frlListLocalDrivingLicenseApplications_Load(null, null);
                }
                else
                    MessageBox.Show("Error:Field to  Delete Application  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalAPplicationID = (int)dgvLDLApp.CurrentRow.Cells[0].Value;



            frmAddEditLocalDrivingApplication frm = new frmAddEditLocalDrivingApplication(LocalAPplicationID);

            frm.ShowDialog();

            frlListLocalDrivingLicenseApplications_Load(null, null);


        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int LocalAPplicationID = (int)dgvLDLApp.CurrentRow.Cells[0].Value;

            frmListTestAppointment frm = new frmListTestAppointment(LocalAPplicationID,clsTestType.enTestTyp.VisionTest);
            frm.ShowDialog();

            frlListLocalDrivingLicenseApplications_Load(null, null);

        }

        private void scheduleWriitenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalAPplicationID = (int)dgvLDLApp.CurrentRow.Cells[0].Value;

            frmListTestAppointment frm = new frmListTestAppointment(LocalAPplicationID, clsTestType.enTestTyp.WrittenTest);
            frm.ShowDialog();

            frlListLocalDrivingLicenseApplications_Load(null, null);

        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalAPplicationID = (int)dgvLDLApp.CurrentRow.Cells[0].Value;

            frmListTestAppointment frm = new frmListTestAppointment(LocalAPplicationID, clsTestType.enTestTyp.StreetTest);
            frm.ShowDialog();


            frlListLocalDrivingLicenseApplications_Load(null, null);


        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int LocalDrivingLicenseApplicationID=(int)dgvLDLApp.CurrentRow.Cells[0].Value;

            clsLocalDrvingLicenseApplications LocalDrivinglicenseApplicationInfo =
                clsLocalDrvingLicenseApplications.FindByLocalDrivingApplicationID(LocalDrivingLicenseApplicationID);


            if (LocalDrivinglicenseApplicationInfo == null) return;

            int TotallPassedTets = (int)dgvLDLApp.CurrentRow.Cells[5].Value;

            bool IsThereAnActiveLicense = LocalDrivinglicenseApplicationInfo.IsThereAnActiveLicense();

            editApplicationToolStripMenuItem.Enabled = !IsThereAnActiveLicense && LocalDrivinglicenseApplicationInfo.ApplicationStatus
                == clsApplications.enApplicationStatus.New;

            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = TotallPassedTets == 3 && !IsThereAnActiveLicense;

            showLicenseToolStripMenuItem.Enabled = IsThereAnActiveLicense;
            editApplicationToolStripMenuItem.Enabled = !IsThereAnActiveLicense && LocalDrivinglicenseApplicationInfo.ApplicationStatus ==
                clsApplications.enApplicationStatus.New;

            scheduleToolStripMenuItem.Enabled = !IsThereAnActiveLicense;


            canceleApplicationToolStripMenuItem.Enabled = LocalDrivinglicenseApplicationInfo
                .ApplicationStatus == clsApplications.enApplicationStatus.New;

            deleteApplicationToolStripMenuItem.Enabled = LocalDrivinglicenseApplicationInfo.ApplicationStatus == clsApplications.enApplicationStatus.New;


            bool IsPassedVisionTest = LocalDrivinglicenseApplicationInfo.DoesPassedThisTest(clsTestType.enTestTyp.VisionTest);
            bool IsPassedWrittenTest = LocalDrivinglicenseApplicationInfo.DoesPassedThisTest(clsTestType.enTestTyp.WrittenTest);
            bool IsPassedStreetTest = LocalDrivinglicenseApplicationInfo.DoesPassedThisTest(clsTestType.enTestTyp.StreetTest);


            scheduleToolStripMenuItem.Enabled=(!IsPassedVisionTest || !IsPassedWrittenTest || !IsPassedStreetTest )&&
                LocalDrivinglicenseApplicationInfo.ApplicationStatus==clsApplications.enApplicationStatus.New;


            if (scheduleToolStripMenuItem.Enabled)
            {
                scheduleVisionTestToolStripMenuItem.Enabled = !IsPassedVisionTest;
                scheduleWriitenTestToolStripMenuItem.Enabled = IsPassedVisionTest && !IsPassedWrittenTest;
                scheduleStreetTestToolStripMenuItem.Enabled= (IsPassedVisionTest && IsPassedWrittenTest)&& !IsPassedStreetTest;

            }







        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalAPplicationID = (int)dgvLDLApp.CurrentRow.Cells[0].Value;

            frmIssueLicenseForFirstTime frm = new frmIssueLicenseForFirstTime(LocalAPplicationID);
            frm.ShowDialog();

            frlListLocalDrivingLicenseApplications_Load(null, null);

        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int LocalDrvingLicenseApplicationID = (int)dgvLDLApp.CurrentRow.Cells[0].Value;

            int LicenseID = clsLocalDrvingLicenseApplications.FindByLocalDrivingApplicationID(LocalDrvingLicenseApplicationID).GetActiveLicenseID();

           
            frmLicenseInfo frm = new frmLicenseInfo(LicenseID);
                frm.ShowDialog();


        }
    }
}
