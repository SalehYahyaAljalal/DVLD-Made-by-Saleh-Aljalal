using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using DataAccessLayerDVLD;

namespace BussnissLayerDVLD
{
    public  class clsLocalDrvingLicenseApplications :clsApplications
    {
        public enum enMode { Add=1,Update=2}
        public enMode Mode;


         public int LocalDrvingLicenseApplicationID {get;set;}
         public int LicenseClassID { get; set; }

         public clsLicenseClasses LicenseClassInfo;


        public string  PersonFullName
        {
            get
            {
                return base.PersonInfo.FullName;
            }
        }

        public  clsLocalDrvingLicenseApplications()
        {
            this.LocalDrvingLicenseApplicationID = -1;
            this.LicenseClassID = -1;

            Mode = enMode.Add;
        }

        public clsLocalDrvingLicenseApplications(int LocalDrvingLicenseApplicationID,
          int ApplicationID, int LicenseClassID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
             enApplicationStatus ApplicationStatus, DateTime LastStatusDate, float PaidFees, int
            CreatedByUserID)
        {
            this.LocalDrvingLicenseApplicationID = LocalDrvingLicenseApplicationID;
            this.ApplicationID = ApplicationID;
            this.LicenseClassID = LicenseClassID;

            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;



            this.LicenseClassInfo = clsLicenseClasses.FindLicenseClassInfoByID(LicenseClassID);
           Mode = enMode.Update;


        }
            
            


        private bool _AddNewLocalDrvingLicenseApplication()
        {
            this.LocalDrvingLicenseApplicationID = clsLocalDrivingLicenseApplicationsDataAccess.
                AddNewLocalDrvingLicenseApplication(this.ApplicationID,this.LicenseClassID);

            return this.LocalDrvingLicenseApplicationID > 0;
        }
        private bool _UpdateLocalDrvingLicenseApplication()
        {
           return  clsLocalDrivingLicenseApplicationsDataAccess.
                UpdateLocalDrvingLicenseApplication(this.LocalDrvingLicenseApplicationID, this.ApplicationID, this.LicenseClassID);

        }


        public static clsLocalDrvingLicenseApplications FindByLocalDrivingApplicationID(int LocalDrvingLicenseApplicationID)
        {
            int ApplicatonID = -1;int LicenseClassID = -1;

            bool IsFound = clsLocalDrivingLicenseApplicationsDataAccess
                .GetLocalDrvingLicenseApplicationByID(LocalDrvingLicenseApplicationID,ref ApplicatonID,ref LicenseClassID);



            if (IsFound)
            {
                clsApplications Application = clsApplications.FindBaseApplication(ApplicatonID);


                return new clsLocalDrvingLicenseApplications(LocalDrvingLicenseApplicationID, ApplicatonID,
                    LicenseClassID, Application.ApplicantPersonID, Application.ApplicationDate, Application.ApplicationTypeID,
                    Application.ApplicationStatus, Application.LastStatusDate, Application.PaidFees, Application.CreatedByUserID);



            }
            else
                return null;


        }

        public static clsLocalDrvingLicenseApplications FindLocalDrivingApplicationByApplicationID(int LocalDrvingLicenseApplicationID)
        {
            int ApplicatonID = -1; int LicenseClassID = -1;

            bool IsFound = clsLocalDrivingLicenseApplicationsDataAccess
                .GetLocalDrvingLicenseApplicationByApplicationID(ref LocalDrvingLicenseApplicationID,  ApplicatonID, ref LicenseClassID);



            if (IsFound)
            {
                clsApplications Application = clsApplications.FindBaseApplication(ApplicatonID);


                return new clsLocalDrvingLicenseApplications(LocalDrvingLicenseApplicationID, ApplicatonID,
                    LicenseClassID, Application.ApplicantPersonID, Application.ApplicationDate, Application.ApplicationTypeID,
                    Application.ApplicationStatus, Application.LastStatusDate, Application.PaidFees, Application.CreatedByUserID);



            }
            else
                return null;

        }




        public bool Save()
        {

            base.Mode = (clsApplications.enMode)Mode;

            if(!base.Save())
                return   false;



            switch (Mode)
            {

                case enMode.Add:
                    if (_AddNewLocalDrvingLicenseApplication())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateLocalDrvingLicenseApplication();
            }



            return false; 
        }


        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseApplicationsDataAccess.GetAllLocalLicenseApplicatins();
        }


        public bool Delete()
        {
            bool IsLocalDrivingApplicationDeleted = false;
            bool IsBaseApplicationDeleted = false;


            IsLocalDrivingApplicationDeleted = clsLocalDrivingLicenseApplicationsDataAccess.
                DeleteLocalDrvingLicenseApplication(this.LocalDrvingLicenseApplicationID);

            if(!IsLocalDrivingApplicationDeleted)
                return false;

            IsBaseApplicationDeleted = base.Delete();


            return IsBaseApplicationDeleted;


        }

        public bool DeosAttedThisTestType(clsTestType.enTestTyp TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationsDataAccess.
                DoesAttendThisTestType(this.LocalDrvingLicenseApplicationID,(int)TestTypeID);

        }

        public bool DoesPassedThisTest(clsTestType.enTestTyp TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationsDataAccess.
                DoesPassedThisTest(this.LocalDrvingLicenseApplicationID,(int)TestTypeID);

        }

        public byte TotalTrialPerTestType(clsTestType.enTestTyp TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationsDataAccess.
                TotallTrailsPerTest(this.LocalDrvingLicenseApplicationID,(int)TestTypeID);

        }
        public bool IsTherAnActiveTestAppointmentForThisTest(clsTestType.enTestTyp TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationsDataAccess.
                IsTherAnActiveTestAppointmentForThisTest(this.LocalDrvingLicenseApplicationID, (int)TestTypeID);
        }

        public int IssueLicenseForTheFirtTime(string Notes, int CreatedByUserID)
        {
            int DriverID = -1;

            clsDrivers Driver = clsDrivers.FindDriverInfoByPersonID(this.ApplicantPersonID);

            if (Driver == null)
            {
                //we check if the driver already there for this person.
                Driver = new clsDrivers();

                Driver.PersonID = this.ApplicantPersonID;
                Driver.CreatedByUserID = CreatedByUserID;
                if (Driver.Save())
                {
                    DriverID = Driver.DriverID;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                DriverID = Driver.DriverID;
            }
            //now we diver is there, so we add new licesnse

            clsLicense License = new clsLicense();
            License.ApplicationID = this.ApplicationID;
            License.DriverID = DriverID;
            License.LicenseClassID = this.LicenseClassID;
            License.IssueDate = DateTime.Now;
            License.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            License.Notes = Notes;
            License.PaisFees = this.LicenseClassInfo.ClassFees;
            License.IsActive = true;
            License.IssueReason = clsLicense.enIssueReason.FirstTime;
            License.CreatedByUserID = CreatedByUserID;

            if (License.Save())
            {
                //now we should set the application status to complete.
                this.SetComplete();

                return License.LicesnseID;
            }

            else
                return -1;
        }

        public int GetTestID(int TestAppointment)
        {
            return clsTestAppointment.GetTestID(TestAppointment);
        }

        public clsTest FindLastTestAppoinment(clsTestType.enTestTyp TestTypeID)
        {
            return clsTest.FindLastTest(this.LicenseClassID,(int)TestTypeID,this.ApplicantPersonID);
        }

        public bool IsThereAnActiveLicense()
        {
            return clsLicense.IsThereAnActiveLicensePerPersonID(this.ApplicantPersonID, this.LicenseClassID) ;
        }

        public int GetTotallPassedTests()
        {
            return clsTest.GettotallPassedTests(this.LocalDrvingLicenseApplicationID);
        }

        public int GetActiveLicenseID()
        {
            return clsLicenseData.GetActiveLicensePerPersonID(this.ApplicantPersonID,this.LicenseClassID);
        }

    }
}
