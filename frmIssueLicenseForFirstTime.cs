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
    public partial class frmIssueLicenseForFirstTime : Form
    {
 
        private int _LocalDrivingApplicationID = -1;
        public frmIssueLicenseForFirstTime(int LocalDrivingApplicationID)
        {
            InitializeComponent();
            _LocalDrivingApplicationID = LocalDrivingApplicationID;
            ctrlLocalDrvinLicenseApplicationInfo1.LoadDataLocalDrivingLicenseApplicationByLocalID
                (_LocalDrivingApplicationID);

            ; ;
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (ctrlLocalDrvinLicenseApplicationInfo1.LocalDrvingLicenseApplicationsInfo == null) return;

            int LicenseID = ctrlLocalDrvinLicenseApplicationInfo1.
                LocalDrvingLicenseApplicationsInfo.IssueLicenseForTheFirtTime(txtNotes.Text.Trim()
                ,clsCurrentUser.CurrentUser.UserID);

            if(LicenseID!=-1)
            {
                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                btnIssue.Enabled = false;
                return;
            }
            else
                MessageBox.Show("Error: Data Not Saved Successfully", "Saved", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);





        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
