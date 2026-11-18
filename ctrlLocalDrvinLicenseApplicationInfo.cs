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
    public partial class ctrlLocalDrvinLicenseApplicationInfo : UserControl
    {

        private clsLocalDrvingLicenseApplications _LocalDrvingLicenseApplicationInfo;
        private int  _LocalDrvingLicenseApplicationID=-1;

        private int _LicenseID = -1;

        public ctrlLocalDrvinLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        public int LocalDrvingLicenseApplicationID
        {
            get
            {
                return _LocalDrvingLicenseApplicationID;
            }
        }

        public int ApplicationID
        {
            get
            {
                return ctrlApplicationInfo1.APplicationID;
            }
        }

        public clsLocalDrvingLicenseApplications LocalDrvingLicenseApplicationsInfo
        {
            get
            {
                return _LocalDrvingLicenseApplicationInfo;
            }
        }



        private void _ResetLocalDrvingLicenseApplicationInfo()
        {
            lblPassedTest.Text = "[????]";
            lblLDLAppID.Text= "[????]";
            lblAppliedForLicense.Text= "[????]";
            _LocalDrvingLicenseApplicationID = -1;
            ctrlApplicationInfo1.ResetApplicationInfo();

        }

       private void _FillLocalDrivingLicenseApplicationINfo()
        {
            //_LicenseID=
            lblLDLAppID.Text = _LocalDrvingLicenseApplicationID.ToString();

            lblAppliedForLicense.Text = _LocalDrvingLicenseApplicationInfo.LicenseClassInfo.ClassName;
            lblPassedTest.Text = _LocalDrvingLicenseApplicationInfo.GetTotallPassedTests().ToString()+"/3";

            ctrlApplicationInfo1.LoadApplicationInfo(_LocalDrvingLicenseApplicationInfo.ApplicationID);



        }
        public void LoadDataLocalDrivingLicenseApplicationByApplicationID(int ApplicacationID)
        {

            _LocalDrvingLicenseApplicationInfo = clsLocalDrvingLicenseApplications
                .FindLocalDrivingApplicationByApplicationID(ApplicacationID);




            if (_LocalDrvingLicenseApplicationInfo == null)
            {
                MessageBox.Show("No Local Application With [ " + ApplicacationID + " ] Not Found", "Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                _ResetLocalDrvingLicenseApplicationInfo();

                return;
            }

            _FillLocalDrivingLicenseApplicationINfo();

        }
        public void LoadDataLocalDrivingLicenseApplicationByLocalID(int LocalID)
        {


            _LocalDrvingLicenseApplicationID = LocalID;
            
            _LocalDrvingLicenseApplicationInfo = clsLocalDrvingLicenseApplications
                .FindByLocalDrivingApplicationID(LocalID);




            if(_LocalDrvingLicenseApplicationInfo==null)
            {
                MessageBox.Show("No Local Application With [ "+LocalID+" ] Not Found","Not Found",
                    MessageBoxButtons.OK,MessageBoxIcon.Error);

                _ResetLocalDrvingLicenseApplicationInfo();

                return;
            }


            _FillLocalDrivingLicenseApplicationINfo();
        }





    }
}
