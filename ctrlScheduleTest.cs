
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

using BussnissLayerDVLD;

namespace DVLDProject
{
    public partial class ctrlScheduleTest : UserControl
    {

        private enum enMode { Add=1,Update=2};
        private enMode _Mode;

        private enum enCreationMode {FirstTimeCreation=1,RetaktestSchedule=2 };
        private enCreationMode _CreationMode;


        private int _TestAppointmentID = -1;
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsTestType.enTestTyp _TestTypeID=clsTestType.enTestTyp.VisionTest;
        private clsLocalDrvingLicenseApplications _LocalDrvingAPplicationInfo;
        private clsTestAppointment _TestAppointment;




        public ctrlScheduleTest()
        {
            InitializeComponent();
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
               switch(_TestTypeID)
                {

                    case clsTestType.enTestTyp.VisionTest:
                        {
                            lblCurrentTest.Text = "Vision Test Appointment";
                            pcCurrentTest.Image = Resources.Vision_512;
                            break;

                        }

                    case clsTestType.enTestTyp.WrittenTest:
                        {
                            lblCurrentTest.Text = "Written Test Appointment";
                            pcCurrentTest.Image = Resources.Written_Test_512;
                            break;


                        }

                    case clsTestType.enTestTyp.StreetTest:
                        {

                            lblCurrentTest.Text = "Street Test Appointment";
                            pcCurrentTest.Image = Resources.Schedule_Test_512;
                            break;

                        }
                }



            }


        }



        private bool _LoadDataTestAppointment()
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);
            if (_TestAppointment == null)
            {
                MessageBox.Show("NO _TestAppointment With ID " + _TestAppointmentID + " Found !", "Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;

                return false;
            }


            lblfees.Text = _TestAppointment.PaidFees.ToString();

            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointmentDate) < 0)
                dtAppointmentDate.MinDate = DateTime.Now;
            else
                dtAppointmentDate.MinDate = _TestAppointment.AppointmentDate;

            dtAppointmentDate.Value = _TestAppointment.AppointmentDate;


            if (_TestAppointment.RetakeApplicationID == -1)
            {
                lblRetakeAppFees.Text = "0";
                lblRetakeTestID.Text = "N/A";
            }
            else
            {
                lblRetakeAppFees.Text = _TestAppointment.RetakeApplicationInfo.PaidFees.ToString();
                lblRetakeTestID.Text = _TestAppointment.RetakeApplicationID.ToString();
                gpRetakeTest.Enabled = true;
                lblCurrentTest.Text = "Schedule Retake Test ";
            }


            return true;
        }


        public void LoadTestAppointmentInfo(int LocalDrivingLicenseApplicationID, clsTestType.enTestTyp TestTypeID,
            int TestAppointmentID=-1)
        {
            _LocalDrivingLicenseApplicationID= LocalDrivingLicenseApplicationID;
            _TestAppointmentID= TestAppointmentID;
            _TestTypeID= TestTypeID;

            if (_TestAppointmentID == -1)
                _Mode = enMode.Add;
            else
                _Mode = enMode.Update;

            _LocalDrvingAPplicationInfo=clsLocalDrvingLicenseApplications.FindByLocalDrivingApplicationID(LocalDrivingLicenseApplicationID);

            if(_LocalDrvingAPplicationInfo==null)
            {
                MessageBox.Show("NO Local APplication With ID " + _LocalDrivingLicenseApplicationID + " Found !", "Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;    
                return;
            }


            if (_LocalDrvingAPplicationInfo.DeosAttedThisTestType(_TestTypeID))
                _CreationMode = enCreationMode.RetaktestSchedule;
            else
                _CreationMode = enCreationMode.FirstTimeCreation;


            if(_CreationMode==enCreationMode.RetaktestSchedule)
            {
                gpRetakeTest.Enabled = true;
                lblRetakeAppFees.Text = clsApplicationsTypes.Find((int)clsApplications.enApplicationTypes.RetakeTest).ApplicationTypeFees.ToString();
                lblRetakeTestID.Text = "0";
                lblCurrentTest.Text = "Schedule Retake Test";
            }
            else
            {
                lblRetakeTestID.Text = "N/A";
                lblCurrentTest.Text = "Schedule Test";
                lblRetakeAppFees.Text = "0";
                gpRetakeTest.Enabled = false;   
            }


            lblDLAppiD.Text = _LocalDrivingLicenseApplicationID.ToString();
            lblClass.Text = _LocalDrvingAPplicationInfo.LicenseClassInfo.ClassName;
            lblName.Text=_LocalDrvingAPplicationInfo.ApplicantFullName;

            lblTrials.Text = _LocalDrvingAPplicationInfo.TotalTrialPerTestType(_TestTypeID).ToString();

            if (_Mode == enMode.Add)
            {
                lblfees.Text = clsTestType.Find((int)_TestTypeID).TestTypeFees.ToString();
                lblRetakeTestID.Text= "N/A";
                dtAppointmentDate.Value = DateTime.Now;

                _TestAppointment = new clsTestAppointment();

            }
            else
            {
               _LoadDataTestAppointment();
                  
            }


            lblTotallFees.Text = (Convert.ToSingle(lblfees.Text.Trim()) + Convert.ToSingle(lblRetakeAppFees.Text.Trim())).ToString();


            if (!_HandleTestAppoinmentWhenItsPassedTest())
                return;

            if (!_HandleActiveTestAppointment())
                return;

            if(!_HandlePreviousTestConstranit())
                return;

            if (!_HandleTestAppointmentWhenItsLocked())
                return;



        }


        private bool _HandleTestAppointmentWhenItsLocked()
        {
            if (_TestAppointment.IsLocked)
            {
                lblMessageUser.Text = "Connot Scheduled, this Appointment Is Locked";
                lblMessageUser.Visible = true;
                btnSave.Enabled = false;
                dtAppointmentDate.Enabled = false;

                return false;
            }
            else
                lblMessageUser.Visible = false;



            return true;
        }
        private bool _HandleActiveTestAppointment()
        {
            if (_Mode == enMode.Add && _LocalDrvingAPplicationInfo.IsTherAnActiveTestAppointmentForThisTest(_TestTypeID))
            {
                lblMessageUser.Text = "Person Already Have An Active Test Appointment!! ";
                lblMessageUser.Visible = true;
                btnSave.Enabled = false;
                dtAppointmentDate.Enabled = false;

                return false;
            }
            else
                lblMessageUser.Visible = false;


            return true;
        }
        private bool _HandleTestAppoinmentWhenItsPassedTest()
        {
            if (_LocalDrvingAPplicationInfo.DoesPassedThisTest(TestTypeID))
            {
                lblMessageUser.Visible = true;
                lblMessageUser.Text = "Connont Scheduled ,This Person Passed This Test";
                btnSave.Enabled = false;
                dtAppointmentDate.Enabled = false;

                return false;
            }
            else
                lblMessageUser.Visible = false;


            return true;
        }
        private bool _HandlePreviousTestConstranit()
        {

            
            switch(_TestTypeID)
            {
                case clsTestType.enTestTyp.VisionTest:
                    lblMessageUser.Visible = false;

                    return true;
                case clsTestType.enTestTyp.WrittenTest:
                    if(!_LocalDrvingAPplicationInfo.DoesPassedThisTest(clsTestType.enTestTyp.VisionTest))
                    {
                        lblMessageUser.Visible = true;
                        lblMessageUser.Text = "Connot Scheduled , Person Should Passed Vision Test !!";
                        btnSave.Enabled = false;
                        dtAppointmentDate.Enabled = false;

                        return false;
                    }
                    else
                    {
                        lblMessageUser.Visible = false ;
                        btnSave.Enabled = true;
                        dtAppointmentDate.Enabled = true;
                       
                    }
                    return true;


                case clsTestType.enTestTyp.StreetTest:
                    if (!_LocalDrvingAPplicationInfo.DoesPassedThisTest(clsTestType.enTestTyp.WrittenTest))
                    {
                        lblMessageUser.Visible = true;
                        lblMessageUser.Text = "Connot Scheduled , Person Should Passed Written Test !!";
                        btnSave.Enabled = false;
                        dtAppointmentDate.Enabled = false;

                        return false;
                    }
                    else
                    {
                        lblMessageUser.Visible = false;
                        btnSave.Enabled = true;
                        dtAppointmentDate.Enabled = true;

                    }
                    return true;




            }


            return true;
        }
        private bool _handlRetakeTestApplication()
        {

            if (_Mode == enMode.Add && _CreationMode == enCreationMode.RetaktestSchedule)
            {
                clsApplications RetaketestApplication = new clsApplications();

                RetaketestApplication.ApplicantPersonID = _LocalDrvingAPplicationInfo.ApplicantPersonID;
                RetaketestApplication.ApplicationDate = DateTime.Now;

                RetaketestApplication.PaidFees = clsApplicationsTypes.Find((int)clsApplications.enApplicationTypes.RetakeTest).ApplicationTypeFees;
                RetaketestApplication.ApplicationTypeID = clsApplicationsTypes.Find((int)clsApplications.enApplicationTypes.RetakeTest).ApplicationTypeID;


                RetaketestApplication.ApplicationStatus = clsApplications.enApplicationStatus.New;
                RetaketestApplication.LastStatusDate = DateTime.Now;
                RetaketestApplication.CreatedByUserID = clsCurrentUser.CurrentUser.UserID;

                if (!RetaketestApplication.Save())
                {
                    MessageBox.Show("Faild to Created Application !", "Faild !!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _TestAppointment.RetakeApplicationID = -1;
                    return false;
                }

                _TestAppointment.RetakeApplicationID = RetaketestApplication.ApplicationID;
            }

            return true;
        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!_handlRetakeTestApplication())
                    return;

            _TestAppointment.TestTypeID = _TestTypeID;
            _TestAppointment.LocalDrivingLicenseApplication = _LocalDrvingAPplicationInfo.LocalDrvingLicenseApplicationID;
            _TestAppointment.AppointmentDate = dtAppointmentDate.Value;
            _TestAppointment.PaidFees = Convert.ToSingle(lblfees.Text.Trim());

             _TestAppointment.CreatedByUserID=clsCurrentUser.CurrentUser.UserID;
            _TestAppointment.IsLocked = false;

            if(_TestAppointment.Save())
            {
                MessageBox.Show("Data Saved Successfully ", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _Mode = enMode.Update;
            }
            else
            {
                MessageBox.Show("Error : Data  Not Saved  ", "Error to Save!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
    }
}
