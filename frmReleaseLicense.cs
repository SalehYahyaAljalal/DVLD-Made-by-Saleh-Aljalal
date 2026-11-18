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
    public partial class frmReleaseLicense : Form
    {
        private int _SelectedLicenseID = -1;
        public frmReleaseLicense()
        {
            InitializeComponent();
        }

        public frmReleaseLicense(int LicenseID)
        {
            InitializeComponent();
           _SelectedLicenseID = LicenseID;

            ctrlLicenseInfoWithFilter1.LoadDataOfLicense(LicenseID);
            ctrlLicenseInfoWithFilter1.FilterEnabled = false;
        }


        private void ctrlLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _SelectedLicenseID=obj;
            lblLicenseID.Text = _SelectedLicenseID.ToString();

            if (_SelectedLicenseID==-1)
            {
                btnRelease.Enabled = false;
                llshowHistoryLicenses.Enabled = false;
                llShowNewLicenseInfo.Enabled = false;
                return;
            }

            if (!ctrlLicenseInfoWithFilter1.SelectedLicense.IsActive)
            {
                MessageBox.Show("Selected License Is Not Active ,Please Select Another One  !","Not Allowed",MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                btnRelease.Enabled = false;
                llshowHistoryLicenses.Enabled = false;
                llShowNewLicenseInfo.Enabled = false;
                return;

            }
            if (!ctrlLicenseInfoWithFilter1.SelectedLicense.IsDetained)
            {
                MessageBox.Show("Selected License Is Not Detain ,Please Select Another One  !", "Not Allowed", MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
                btnRelease.Enabled = false;
                llshowHistoryLicenses.Enabled = false;
                llShowNewLicenseInfo.Enabled = false;
                return;

            }


            lblDetainID.Text = ctrlLicenseInfoWithFilter1.SelectedLicense.DetainLicenseInfo.DetainID.ToString();
            lblDetainDate.Text = ctrlLicenseInfoWithFilter1.SelectedLicense.DetainLicenseInfo.DetainDate.ToShortDateString();
            lblFineFees.Text = ctrlLicenseInfoWithFilter1.SelectedLicense.DetainLicenseInfo.FineFees.ToString();
            lblApplicationFees.Text = clsApplicationsTypes.Find((int)clsApplications.enApplicationTypes.ReleaseDetainedDrivingLicsense).ApplicationTypeFees.ToString();
            lblTotalFees.Text = (Convert.ToSingle(lblFineFees.Text) + Convert.ToSingle(lblApplicationFees.Text)).ToString();



            btnRelease.Enabled = true;
            llshowHistoryLicenses.Enabled = true;


        }



        private void frmReleaseLicense_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = clsCurrentUser.CurrentUser.UserName;
            lblLicenseID.Text=_SelectedLicenseID.ToString();


        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure That You Want ot Release  this License ???", "Confirm "
             , MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            int ReleasedApplicationID=-1;
            bool IsReleased= ctrlLicenseInfoWithFilter1.SelectedLicense.ReleaseLicense(clsCurrentUser.CurrentUser.UserID,ref ReleasedApplicationID);

            lblApplicationID.Text = ReleasedApplicationID.ToString();

            if ( ! IsReleased)
            {
                MessageBox.Show("Release For Selected License Is Field ", "Error "
             , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            MessageBox.Show("Release Selected License Is Done Successfully  ", "Success ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
            ctrlLicenseInfoWithFilter1.FilterEnabled = false;    

            llShowNewLicenseInfo.Enabled=true;
            btnRelease.Enabled = false;

        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_SelectedLicenseID);
            frm.ShowDialog();

        }

        private void llshowHistoryLicenses_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int PersonID=ctrlLicenseInfoWithFilter1.SelectedLicense.Driverinfo.PersonID;

            frmDriversLicensesHistory frm=new frmDriversLicensesHistory(PersonID);
            frm.ShowDialog();

        }

        private void frmReleaseLicense_Activated(object sender, EventArgs e)
        {
            ctrlLicenseInfoWithFilter1.FocusFilter();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
