using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccessLayerDVLD;

namespace BussnissLayerDVLD
{
    public class clsTest
    {

        public enum enMode { Add=1,Update=2}
        public enMode Mode;
         public int TestID               {get;set;}
         public int TestAppointmentID    {get;set;}
        public clsTestAppointment TestAppointmentInfo;
         public bool TestResult          {get;set;}   
         public string Notes             {get;set;}     
         public int CreatedByUserID      { get; set; }

        public clsUsers CreatedByUserInfo;

        public clsTest()
        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.Notes = "";
            this.TestResult = false;
            this.CreatedByUserID = -1;


            Mode = enMode.Add;
        }

        private clsTest(int TestID,  int TestAppointmentID,  bool TestResult
          ,  string Notes,  int CreatedByUserID)
        {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.Notes = Notes;
            this.TestResult = TestResult;
            this.CreatedByUserID = CreatedByUserID;

            this.TestAppointmentInfo = clsTestAppointment.Find(TestAppointmentID);
            this.CreatedByUserInfo=clsUsers.FindByUserID(CreatedByUserID);

            Mode = enMode.Update;
        }
        
        public static clsTest FindByTestID(int TestID)
        {
            int TestAppointmentID=-1;  bool TestResult = false;
            string Notes = ""; int CreatedByUserID = -1;

            bool IsFound = clsTestsData.GetTestByID(TestID, ref TestAppointmentID, ref TestResult, ref Notes,
                ref CreatedByUserID);

            if (IsFound)
            {
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            }

            else
                return null;

        }

        public static clsTest FindLastTest(int LicenseClassID, int TestTypeID, int PersonID)
        {
            int TestAppointmentID = -1; bool TestResult = false;
            string Notes = ""; int CreatedByUserID = -1;
            int TestID = -1;


            bool IsFound = clsTestsData.GetLastTestByPersonIDAndTestTypeIDAndLicenseClass(ref TestID, ref TestAppointmentID, ref TestResult, ref Notes,
                ref CreatedByUserID,LicenseClassID,TestTypeID,PersonID);

            if (IsFound)
            {
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            }

            else
                return null;

        }


       

        private bool  _AddNewTest()
        {
            this.TestID = clsTestsData.AddNewTest(this.TestAppointmentID,this.TestResult,this.Notes,this.CreatedByUserID);

            return this.TestID > 0;
        }


        private bool _UpdateTest()
        {
            return clsTestsData.UpdateTest(this.TestID, this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
        }

        public bool Save()
        {


            switch (Mode)
            {

                case enMode.Add:
                    if (_AddNewTest())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else return false;
                case enMode.Update:
                    return _UpdateTest();
            
            
            
            }

            return false;
        }

        public byte GetTotallPassedTest(int LocalDrivingLicenseAPplicationID)
        {

            return clsTestsData.GetTotallPassedTest(LocalDrivingLicenseAPplicationID);
        }

        public static byte GettotallPassedTests(int localdrivinglicenseapplicationid)
        {

            return clsTestsData.GetTotallPassedTest(localdrivinglicenseapplicationid);
        }


        public bool IsPassedALlTests(int LocalDrivingLicenseAPplicationID)
        {
            return GetTotallPassedTest(LocalDrivingLicenseAPplicationID)==3;
        }





    }
}
