using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccessLayerDVLD;

using System.Data;

namespace BussnissLayerDVLD
{
    public class clsTestAppointment
    {
        public enum enMode { Add=1,Update=2}
        public enMode Mode;

       public  int TestAppointmentID { get; set; }

        public clsTestType.enTestTyp TestTypeID;
       public  int LocalDrivingLicenseApplication { get; set; }
        public clsTestType TestTypeInfo;
       public  DateTime AppointmentDate { get; set; }
       public  float PaidFees { get; set; }
       public  int CreatedByUserID { get; set; }
        public clsUsers CreatedByUserInfo;

       public  bool IsLocked { get; set; }
     

        public int RetakeApplicationID { get; set; }
        public clsApplications RetakeApplicationInfo;


        public int TestID
        {
            get
            {
                return GetTestID(); 
            }
        }

        public clsTestAppointment()
       {
            this.TestAppointmentID = -1;
            this.TestTypeID = clsTestType.enTestTyp.VisionTest;
            this.LocalDrivingLicenseApplication = -1;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            this.IsLocked = false;
            this.RetakeApplicationID = -1;

            Mode = enMode.Add;   

       }

        public clsTestAppointment(int TestAppointmentID, clsTestType.enTestTyp TestTypeID
            , int LocalDrivingLicenseApplicationID, DateTime AppointmentDate,
            float PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this.LocalDrivingLicenseApplication = LocalDrivingLicenseApplicationID;
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsLocked = IsLocked;

            this.TestTypeInfo = clsTestType.Find((int)TestTypeID);

            this.CreatedByUserInfo = clsUsers.FindByUserID(CreatedByUserID);
            
            this.RetakeApplicationID = RetakeTestApplicationID;
            this.RetakeApplicationInfo = clsApplications.FindBaseApplication(RetakeTestApplicationID);




            Mode = enMode.Update;
        }
        public static clsTestAppointment Find(int TestAppointmentID)
        {
            int TestTypeID = -1;
            ; int LocalDrivingLicenseApplicationID = -1; DateTime AppointmentDate=DateTime.MinValue;
            float PaidFees = 0; int CreatedByUserID=-1; bool IsLocked = false;
             int retakeTestApplicationID = 0;


            bool IsFound = clsTestAppointmentData.GetTestAppointmentByID(TestAppointmentID,ref TestTypeID,ref LocalDrivingLicenseApplicationID,
                ref AppointmentDate,ref PaidFees,ref CreatedByUserID,ref IsLocked,ref retakeTestApplicationID);



            if(IsFound)
            {
                return new clsTestAppointment( TestAppointmentID,(clsTestType.enTestTyp)TestTypeID
            ,  LocalDrivingLicenseApplicationID,  AppointmentDate,
             PaidFees,  CreatedByUserID,  IsLocked, retakeTestApplicationID);
            
            }
            else
            {
                return  null;
            }

        }
        public static clsTestAppointment FindLastTestAppintment(int TestAppointmentID)
        {
            int TestTypeID = -1;
            ; int LocalDrivingLicenseApplicationID = -1; DateTime AppointmentDate = DateTime.MinValue;
            float PaidFees = 0; int CreatedByUserID = -1; bool IsLocked = false;
            int retakeTestApplicationID = 0;


            bool IsFound = clsTestAppointmentData.GetLastTestAppointment(ref TestAppointmentID,  TestTypeID,  LocalDrivingLicenseApplicationID,
                ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref retakeTestApplicationID);



            if (IsFound)
            {
                return new clsTestAppointment(TestAppointmentID, (clsTestType.enTestTyp)TestTypeID
            , LocalDrivingLicenseApplicationID, AppointmentDate,
             PaidFees, CreatedByUserID, IsLocked, retakeTestApplicationID);

            }
            else
            {
                return null;
            }

        }

        private bool _AddNewTestAppointment()
        {
            this.TestAppointmentID=clsTestAppointmentData.AddNewTestAppointment((int)this.TestTypeID,this.LocalDrivingLicenseApplication,
                this.AppointmentDate,this.PaidFees,this.CreatedByUserID,this.IsLocked,this.RetakeApplicationID);

            return this.TestAppointmentID > 0;

        }

        private bool _UpdateTestAppointment()
        {
            return clsTestAppointmentData.UpdateTestAppointment(this.TestAppointmentID,(int)this.TestTypeID, this.LocalDrivingLicenseApplication,
                this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked,this.RetakeApplicationID);
        }

        public bool Save()
        {


            switch(Mode)
            {
                case enMode.Add:

                    if(_AddNewTestAppointment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else return false;

                case enMode.Update:
                    return _UpdateTestAppointment();
            }



            return false;
        }



        public static DataTable GetAllTestAppoinmentPerTestType(int LocalDrivingLIcenseApplicationID,clsTestType.enTestTyp TestTypeID )
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentPerTestType(LocalDrivingLIcenseApplicationID,(int)TestTypeID);
        }

        public  DataTable GetAllTestAppoinmentPerTestType( clsTestType.enTestTyp TestTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentPerTestType(this.LocalDrivingLicenseApplication, (int)TestTypeID);
        }


        public int GetTestID()
        {
            return clsTestAppointmentData.GetTestID(this.TestAppointmentID);
        }

        public  static int GetTestID(int TestAppoinmentID)
        {
            return clsTestAppointmentData.GetTestID(TestAppoinmentID);
        }



    }
}
