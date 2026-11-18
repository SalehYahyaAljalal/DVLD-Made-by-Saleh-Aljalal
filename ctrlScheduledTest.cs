using BussnissLayerDVLD;
using DVLDProject.Properties;
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
    public partial class ctrlScheduledTest : UserControl
    {

        private clsTestType.enTestTyp _TestTypeID;
        private clsLocalDrvingLicenseApplications _LocalDrivingLicenseApplicationInfo;
        private clsTestAppointment _TestAppointmentInfo;
        private int _TestAppointmentID;
        private int _LocalDrivingLicenseAPplicationID=-1;
        private int _TestID=-1;

        public ctrlScheduledTest()
        {
            InitializeComponent();
        }

        public int TestAppointmentID
        {
            get
            {
                return _TestAppointmentID;
            }
            set
            {
                _TestAppointmentID = value;
            }
        }

        public int TestID
        {
            get
            {
                return _TestID;
            }
            set
            {
                _TestID= value; 
                lblTestID.Text = (_TestID != -1) ? _TestID.ToString() : "Not Taken Yet!";
            }
           
        }




        public clsTestType.enTestTyp TestTypeID
        {
            get
            {
                return _TestTypeID;
            }

            set
            {
                _TestTypeID = value;
                switch (_TestTypeID)
                {

                    case clsTestType.enTestTyp.VisionTest:
                        {
                            lblScheduled.Text = "Vision Test Appointment";
                            pcCurrentTest.Image = Resources.Vision_512;
                            break;

                        }

                    case clsTestType.enTestTyp.WrittenTest:
                        {
                            lblScheduled.Text = "Written Test Appointment";
                            pcCurrentTest.Image = Resources.Written_Test_512;
                            break;


                        }

                    case clsTestType.enTestTyp.StreetTest:
                        {

                            lblScheduled.Text = "Street Test Appointment";
                            pcCurrentTest.Image = Resources.Schedule_Test_512;
                            break;

                        }
                }



            }


        }


        public void LoadDataOfScheduledTest(int TestAppointmentID)
        {
            _TestTypeID=TestTypeID;
            _TestAppointmentID = TestAppointmentID;

            _TestAppointmentInfo = clsTestAppointment.Find(_TestAppointmentID);

            if (_TestAppointmentInfo == null)
            {
                MessageBox.Show("Thers No TestAPpointmnet With ID "+_TestAppointmentID+" ] ","Not Found! ",MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return; 
            }

            TestID=_TestAppointmentInfo.GetTestID();
            _LocalDrivingLicenseApplicationInfo = clsLocalDrvingLicenseApplications.
                FindByLocalDrivingApplicationID(_TestAppointmentInfo.LocalDrivingLicenseApplication);


            if ( _LocalDrivingLicenseApplicationInfo == null)
            {
                MessageBox.Show("Thers No Local Application  With ID " + _TestAppointmentInfo.LocalDrivingLicenseApplication + " ] ", "Not Found! ", MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                return;
            }

            _LocalDrivingLicenseAPplicationID = _LocalDrivingLicenseApplicationInfo.LocalDrvingLicenseApplicationID;


            lblApplicant.Text = _LocalDrivingLicenseApplicationInfo.ApplicantFullName;
            lblDateOfTest.Text=_TestAppointmentInfo.AppointmentDate.ToShortDateString();
            lblTrial.Text = _LocalDrivingLicenseApplicationInfo.TotalTrialPerTestType(_TestTypeID).ToString();
            lblLicenseClass.Text = _LocalDrivingLicenseApplicationInfo.LicenseClassInfo.ClassName;
            lblFees.Text = _TestAppointmentInfo.PaidFees.ToString();
            LblDLAppID.Text = _LocalDrivingLicenseAPplicationID.ToString();



        }

        
    }
}
