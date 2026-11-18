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
    public partial class frmReplacementLicenseForLostLicenseOrDamaged : Form
    {
        private int _NewLiceseID = -1;
        private int LicenseFeesAccordingToTypeReplacement = 0;

        private clsLicense.enIssueReason _IssueReplacementFor;
        public frmReplacementLicenseForLostLicenseOrDamaged()
        {
            InitializeComponent();
        }

        private clsLicense.enIssueReason _GetIssueReason()
        {
            if (rdDamaged.Checked)
                return clsLicense.enIssueReason.DamagedReplacement;
            else
                return clsLicense.enIssueReason.LostReplacement;

        }

        private int _GetApplicationTypeID()
        {
            if(rdDamaged.Checked)
                return (int)clsApplications.enApplicationTypes.ReplacementforaDamagedDrivingLicense;
            else
                return (int)clsApplications.enApplicationTypes.ReplacementforaLostDrivingLicense;


        }

        private void rdDamaged_CheckedChanged(object sender, EventArgs e)
        {
            lblReplacementFor.Text = "Replacement For Damaged";
            _IssueReplacementFor = clsLicense.enIssueReason.DamagedReplacement;

            lblApplicationFees.Text=clsApplicationsTypes.Find(_GetApplicationTypeID() ).ApplicationTypeFees. ToString();

        }

        private void rdLost_CheckedChanged(object sender, EventArgs e)
        {
            lblReplacementFor.Text = "Replacement For Lost";
            _IssueReplacementFor = clsLicense.enIssueReason.LostReplacement;
            lblApplicationFees.Text = clsApplicationsTypes.Find(_GetApplicationTypeID()).ApplicationTypeFees.ToString();

        }

        private void ctrlLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;

            llshowHistoryLicenses.Enabled = (SelectedLicenseID != -1);

            if (SelectedLicenseID == -1) return;

            if(!ctrlLicenseInfoWithFilter1.SelectedLicense.IsActive)
            {
                MessageBox.Show("License Should Be Active !","Not Allowed!!",MessageBoxButtons.OK,MessageBoxIcon.Error);
                btnIssueReplacement.Enabled = false;
                return;
            }
            lblOldLicenseID.Text = ctrlLicenseInfoWithFilter1.SelectedLicense.LicesnseID.ToString();

            btnIssueReplacement.Enabled = true;




        }

        private void frmReplacementLicenseForLostLicenseOrDamaged_Load(object sender, EventArgs e)
        {
            lblApplicationDtae.Text = DateTime.Now.ToShortDateString();
            lblCreatedByUser.Text = clsCurrentUser.CurrentUser.UserName;

              _IssueReplacementFor = clsLicense.enIssueReason.DamagedReplacement;
             rdDamaged.Checked = true;


        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show(" Are You Sure that You Want to Replacement this  License !!", "Confirme", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
             == DialogResult.No)
            {
                return;
            }

            int NewApplicationID = -1;

            clsLicense NewLicense = ctrlLicenseInfoWithFilter1.SelectedLicense.
                ReplacementForLostOrDamaged(clsCurrentUser.CurrentUser.UserID, _GetIssueReason()) ;

            if(NewLicense==null)
            {

                MessageBox.Show("License Not  Replacement Seccussfully ", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            lblLRApplicationID.Text = (NewApplicationID = NewLicense.ApplicationID).ToString();
            _NewLiceseID = NewLicense.LicesnseID;
            lblRLicenseID.Text = _NewLiceseID.ToString();


            MessageBox.Show("License   Replacement Seccussfully ", "Saved",
                  MessageBoxButtons.OK, MessageBoxIcon.Information);

            ctrlLicenseInfoWithFilter1.FilterEnabled = false;
            btnIssueReplacement.Enabled = false;
            gpReplacement.Enabled = false;


            llShowNewLicenseInfo.Enabled = true;




        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_NewLiceseID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
