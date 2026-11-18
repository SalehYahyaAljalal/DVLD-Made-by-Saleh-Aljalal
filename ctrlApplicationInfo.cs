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
    public partial class ctrlApplicationInfo : UserControl
    {
       private clsApplications _ApplicationsInfo;

        private int _APplicationID = -1;
        private int _PersonID = -1;
        public ctrlApplicationInfo()
        {
            InitializeComponent();
        }


        public int APplicationID
        {
            get
            {
                return _APplicationID;
            }
        }

       public void ResetApplicationInfo()
       {

            lblApplicationDate.Text= "[????]"; ;
            lblCreatedByUser.Text= "[????]"; 
            lblAPP_ID.Text= "[????]";
            lblFees.Text= "[????]";
            lblName.Text= "[????]";
            lblLastApplicationDate.Text = "[????]";
            lblStatus.Text = "[????]";
            _APplicationID = -1;

            llShowPersonInfo.Enabled = false; 

        }


        private void _FillApplicationInfo()
        {
            _PersonID = _ApplicationsInfo.ApplicantPersonID;

            lblApplicationDate.Text = _ApplicationsInfo.ApplicationDate.ToShortDateString();
            lblLastApplicationDate.Text = _ApplicationsInfo.LastStatusDate.ToShortDateString();
            lblAPP_ID.Text = _ApplicationsInfo.ApplicationID.ToString();
            lblApplicationType.Text = _ApplicationsInfo.ApplicationTypeInfo.ApplicationTypeTitle;
            lblFees.Text = _ApplicationsInfo.PaidFees.ToString();
            lblStatus.Text = _ApplicationsInfo.StatusText;
            lblName.Text = _ApplicationsInfo.ApplicantFullName;
            lblCreatedByUser.Text = _ApplicationsInfo.CreatedByUserInfo.UserName;

            llShowPersonInfo.Enabled = (_PersonID != -1);


        }
        public void LoadApplicationInfo(int ApplicationID)
        {
            _APplicationID=ApplicationID;

            _ApplicationsInfo=clsApplications.FindBaseApplication(ApplicationID);

            if(_ApplicationsInfo == null )
            {
                MessageBox.Show("No Application Wiht ID [ "+_APplicationID+" ] FOund","Not Found",
                    MessageBoxButtons.OK,MessageBoxIcon.Error);

                ResetApplicationInfo();

                return;
            }


            _FillApplicationInfo();

        }

        private void llShowPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonInformation frm = new frmPersonInformation(_PersonID);
            frm.ShowDialog();


            LoadApplicationInfo(_APplicationID);

        }
    }
}
