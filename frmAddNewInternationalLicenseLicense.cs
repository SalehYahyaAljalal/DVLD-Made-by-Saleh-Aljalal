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
    public partial class frmAddNewInternationalLicenseLicense : Form
    {

        private int _InternationalLicenseID= -1;

        public frmAddNewInternationalLicenseLicense()
        {
            InitializeComponent();
        }
    


        private void ctrlLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int  SelectedLocalLicenseID= obj;   
            lblLicenseID.Text = SelectedLocalLicenseID.ToString();

            llshowHistoryLicenses.Enabled = (SelectedLocalLicenseID != -1);

            if (SelectedLocalLicenseID == -1) return;



            if (ctrlLicenseInfoWithFilter1.SelectedLicense.LicenseClassID != 3)
            {
                MessageBox.Show("Selected License Should Be From License Class 3 ,Please Select Another One !", "Not Allowed", MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
                btnIssue.Enabled = false;
                llShowNewLicenseInfo.Enabled = false;
                return;

            }






            int ActiveInternationalLicenseID = clsInternationalLicense
                .GetActiveInternationalLicenseIDByDriverID(ctrlLicenseInfoWithFilter1.SelectedLicense.DriverID);


            if (ActiveInternationalLicenseID!=-1)
            {
                MessageBox.Show("This Person Already Have International License", "Not Allowed", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                btnIssue.Enabled = false;
                _InternationalLicenseID=ActiveInternationalLicenseID;
                lblInternationalLicenseID.Text = _InternationalLicenseID.ToString();
                llShowNewLicenseInfo.Enabled = true; ;
                return;

            }



            btnIssue.Enabled = true;



        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure That You Want ot Issue  this Internatioal License  ???", "Confirm "
             , MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            clsInternationalLicense NewInternationalLicense = new clsInternationalLicense();


            NewInternationalLicense.ApplicantPersonID = ctrlLicenseInfoWithFilter1.SelectedLicense.Driverinfo.PersonID;
            NewInternationalLicense.ApplicationDate = DateTime.Now;
            NewInternationalLicense.ApplicationTypeID = (int)clsApplications.enApplicationTypes.NewInternationalLicense;
            NewInternationalLicense.ApplicationStatus = clsApplications.enApplicationStatus.Compeleted;
            NewInternationalLicense.LastStatusDate = DateTime.Now;

            NewInternationalLicense.PaidFees = clsApplicationsTypes.
                Find((int)clsApplications.enApplicationTypes.ReleaseDetainedDrivingLicsense).ApplicationTypeFees;

            NewInternationalLicense.CreatedByUserID = clsCurrentUser.CurrentUser.UserID;




            NewInternationalLicense.DriverID = ctrlLicenseInfoWithFilter1.SelectedLicense.DriverID;
            NewInternationalLicense.IssueingUsingLocalLicenseID = ctrlLicenseInfoWithFilter1.SelectedLicense.LicesnseID ;
            NewInternationalLicense.IssuedDate=DateTime.Now;
            NewInternationalLicense.ExpiraionDate=DateTime.Now.AddYears(1);

            if(!NewInternationalLicense.Save())
            {
                MessageBox.Show("Faild TO add New International License !", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ;
            }

            MessageBox.Show("Data Saved Successfully !", "Saved !", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _InternationalLicenseID=NewInternationalLicense.InternationalLicenseID;
            llShowNewLicenseInfo.Enabled=true;
            btnIssue.Enabled = false;
            ctrlLicenseInfoWithFilter1.FilterEnabled = false;






        }

        private void frmAddNewInternationalLicenseLicense_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text= DateTime.Now.ToShortDateString();
            lblCreatedBy.Text = clsCurrentUser.CurrentUser.UserName;
            lblFees.Text = clsApplicationsTypes.Find((int)clsApplications.enApplicationTypes.NewInternationalLicense).ApplicationTypeFees.ToString();
            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToString();


        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();
        }

        private void frmAddNewInternationalLicenseLicense_Activated(object sender, EventArgs e)
        {
            ctrlLicenseInfoWithFilter1.FocusFilter();
        }
    }
}
