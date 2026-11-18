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
    public partial class frmMain : Form
    {
        private frmLoginScreen _loginScreen;
        public frmMain(frmLoginScreen loginScreen)
        {
            InitializeComponent();
            _loginScreen = loginScreen;
        }

        private void peoopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAllPeople people= new frmAllPeople();  
            people.ShowDialog();
        }

     

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void signOunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsCurrentUser.CurrentUser = null;
            _loginScreen.Show();
            this.Close();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListUsers Ulist = new frmListUsers();
            Ulist.ShowDialog();


        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmCurrentUserInfo CurrentUser = new frmCurrentUserInfo(clsCurrentUser.CurrentUser.UserID);

            CurrentUser.ShowDialog();

        }

        private void changePassowrdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChanagePassword chp_User = new frmChanagePassword(clsCurrentUser.CurrentUser.UserID);

            chp_User.ShowDialog();

        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmApplicationTypes frmappTypes = new frmApplicationTypes();

            frmappTypes.ShowDialog();   

        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmManageTestTypes frm = new frmManageTestTypes();
            frm.ShowDialog();

        }

        private void localDrvingLicenseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListLocalDrivingLicenseApplications frm=new frmListLocalDrivingLicenseApplications();
            frm.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditLocalDrivingApplication frm = new frmAddEditLocalDrivingApplication();

            frm.ShowDialog();


        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenewLicenseInfoForDriver frm = new frmRenewLicenseInfoForDriver();
            frm.ShowDialog();



        }

        private void replacementForDamagedOrLostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReplacementLicenseForLostLicenseOrDamaged frm = new frmReplacementLicenseForLostLicenseOrDamaged();
            frm.ShowDialog();


        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListDrivers frm = new frmListDrivers();

            frm.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();


        }

        private void releaseLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseLicense frm = new frmReleaseLicense();
            frm.ShowDialog();
        }

        private void manageDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageDetainedLicense frm = new frmManageDetainedLicense();
            frm.ShowDialog();

        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmAddNewInternationalLicenseLicense frm = new frmAddNewInternationalLicenseLicense();
            frm.ShowDialog();



        }

        private void internationalLicenseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListInternationalLicenses frm = new frmListInternationalLicenses();
            frm.ShowDialog();

        }

        
    }
}
