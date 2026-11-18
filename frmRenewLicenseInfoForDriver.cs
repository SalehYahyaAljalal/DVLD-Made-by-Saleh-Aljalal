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
    public partial class frmRenewLicenseInfoForDriver : Form
    {
        private int _NewLicenseID = -1;

        public frmRenewLicenseInfoForDriver()
        {
            InitializeComponent();
        }
      

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _NewLicenseID = obj; 

            if(_NewLicenseID == -1) return;


            llshowHistoryLicenses.Enabled =(_NewLicenseID!=-1);




            if (!ctrlLicenseInfoWithFilter1.SelectedLicense.IsThisLicenseExpired())
            {
             DateTime ExpireDate=   ctrlLicenseInfoWithFilter1.SelectedLicense.ExpirationDate;
                MessageBox.Show("This License Not Expired Yet It'll Expire in " + ExpireDate.ToShortDateString() + ", Pllease Inter Another On !",
                    "Not Allowed!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenew.Enabled = false;

                

                return;
            }


            if (!ctrlLicenseInfoWithFilter1.SelectedLicense.IsActive)
            {
                MessageBox.Show("This License Not Active , Pllease Inter Another On !",
                   "Not Allowed!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenew.Enabled = false;
                return;
            }

            int DefaultValidityLength= ctrlLicenseInfoWithFilter1.SelectedLicense.LiceneseClassesInfo.DefaultValidityLength;
            lblOldLicenseID.Text = ctrlLicenseInfoWithFilter1.SelectedLicense.LicesnseID.ToString();
            lblExpirationDate.Text = DateTime.Now.AddYears(DefaultValidityLength).ToShortDateString();
            txtNotes.Text=ctrlLicenseInfoWithFilter1.SelectedLicense.Notes;

            lblLicenseFees.Text = ctrlLicenseInfoWithFilter1.SelectedLicense.LiceneseClassesInfo.ClassFees.ToString();
            lblTotallFees.Text = (Convert.ToSingle(lblLicenseFees.Text) +Convert.ToSingle(lblApplicationfees.Text)).ToString();
         
            
            btnRenew.Enabled = true;


        }

        private void btnRenew_Click(object sender, EventArgs e)
        {

           if(MessageBox.Show("Are You Sure that You Want to Renew this  License !!","Confirme",MessageBoxButtons.YesNo,MessageBoxIcon.Question)
                ==DialogResult.No)
            {
                return;
            }


        clsLicense NewLicenseInfo = ctrlLicenseInfoWithFilter1.SelectedLicense.RenewLicense(txtNotes.Text,
                clsCurrentUser.CurrentUser.UserID);



            if (NewLicenseInfo == null)
            {

                MessageBox.Show("License NOt  Renewed Seccussfully ", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

               _NewLicenseID = NewLicenseInfo.LicesnseID;

              lblRenewdLicenseID.Text = _NewLicenseID.ToString();
              lblRenewdApplicationID.Text = NewLicenseInfo.ApplicationID.ToString();
            
                
              MessageBox.Show("License Has Renewed Seccussfully With ID [ " + _NewLicenseID + " ] ", "Renewd ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            
               btnRenew.Enabled = false;
              ctrlLicenseInfoWithFilter1.FilterEnabled = false;
              llShowNewLicenseInfo.Enabled = true;
             

        }

        private void frmRenewLicenseInfoForDriver_Activated(object sender, EventArgs e)
        {
            ctrlLicenseInfoWithFilter1.FocusFilter();
        }

        private void frmRenewLicenseInfoForDriver_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();

            lblApplicationfees.Text = clsApplicationsTypes.
              Find((int)clsApplications.enApplicationTypes.RenewDrivingLicenseService).ApplicationTypeFees.ToString();
            lblCreatedByUser.Text = clsCurrentUser.CurrentUser. UserName;

        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_NewLicenseID);
            frm.ShowDialog();


        }

        private void llshowHistoryLicenses_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {




        }
    }
}
