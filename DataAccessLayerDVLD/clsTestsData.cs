using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;



namespace DataAccessLayerDVLD
{
    public static class clsTestsData
    {

   //     TestID TestAppointmentID   TestResult Notes   CreatedByUserID

        public static int AddNewTest(int TestAppointmentID ,bool TestResult
            ,string  Notes ,int CreatedByUserID)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
                
                
                


                Insert into Tests (TestAppointmentID,TestResult,Notes,CreatedByUserID)
                Values
                (@TestAppointmentID,@TestResult,@Notes,@CreatedByUserID)
                Select SCOPE_IDENTITY();

            
                Update TestAppointments 
	                Set IsLocked=1 
                Where TestAppointmentID=@TestAppointmentID

                        ";

            SqlCommand Command = new SqlCommand(Query,Connection);  
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            Command.Parameters.AddWithValue("@TestResult", TestResult);
            Command.Parameters.AddWithValue("@Notes", Notes);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            int TestID = -1;

            try
            {
                Connection.Open();

                object obj = Command.ExecuteScalar();

                if(obj != null && int.TryParse(obj.ToString(),out int InsertedID))
                    TestID=InsertedID;


            }
            catch (Exception Ex)
            {
                TestID = -1;
             }
            finally
            {
                Connection.Close();
            }


            return TestID;  
        }

        public static bool UpdateTest(int TestID,int TestAppointmentID, bool TestResult
          , string Notes, int CreatedByUserID)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
               
                Update Tests 
                Set 
                TestAppointmentID=@TestAppointmentID,
                TestResult=@TestResult,
                Notes=@Notes,
                CreatedByUserID=@CreatedByUserID
                Where TestID=@TestID
;
                        ";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestID", TestID);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            Command.Parameters.AddWithValue("@TestResult", TestResult);
            Command.Parameters.AddWithValue("@Notes", Notes);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            int RowsAffected = 0;

            try
            {
                Connection.Open();

                RowsAffected=Command.ExecuteNonQuery();


            }
            catch (Exception Ex)
            {
                RowsAffected = 0;
            }
            finally
            {
                Connection.Close();
            }


            return RowsAffected>0;
        }


        public static bool GetTestByID(int TestID, ref int TestAppointmentID,ref  bool TestResult
          , ref string Notes,ref  int CreatedByUserID)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Query = @"
Select * from Tests Where TestID=@TestID
            ";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestID", TestID);

            bool IsFound = false;

            try
            {
                Connection.Open();

                SqlDataReader Reader=Command.ExecuteReader();   

                if(Reader.Read())
                {
                    IsFound = true;
                    TestAppointmentID =(int)Reader["TestAppointmentID"];
                    TestResult = (bool)Reader["TestResult"];

                    if (Reader["Notes"] == DBNull.Value)
                        Notes = "";
                    else
                        Notes = (string)Reader["Notes"];


                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                }

                Reader.Close();

            }
            catch (Exception Ex)
            {

                IsFound = false;
            }
            finally
            {
                Connection.Close();
            }


            return IsFound;

        }




        //wiht doctor
        public static bool GetLastTestByPersonIDAndTestTypeIDAndLicenseClass(ref int TestID,ref  int TestAppointmentID, ref bool TestResult
     , ref string Notes, ref int CreatedByUserID,int LicenseClassID,int TestTypeID,int PersonID)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Query = @"
                   
            SELECT  TOP 1  TestID,Tests.TestAppointmentID,TestResult,Notes,Tests.CreatedByUserID 
	            FROM LocalDrivingLicenseApplications 
	            INNER JOIN  TESTS INNER JOIN TestAppointments 
				            ON
			            TESTS.TestAppointmentID=TestAppointments.TestAppointmentID	
				            ON
	            LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID=TestAppointments.LocalDrivingLicenseApplicationID 
				            INNER JOIN Applications ON 
	            LocalDrivingLicenseApplications.ApplicationID=Applications.ApplicationID 

            WHERE LocalDrivingLicenseApplications.LicenseClassID=@LicenseClassID
            AND ApplicationS.ApplicantPersonID=@PersonID
            AND TestAppointments.TestTypeID=@TestTypeID
                Order By TestAppointments.TestAppointmentID desc
                            ";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);



            bool IsFound = false;

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsFound = true;
                    TestID = (int)Reader["TestID"];
                    TestAppointmentID = (int)Reader["TestAppointmentID"];
                    TestResult = (bool)Reader["TestResult"];

                    if (Reader["Notes"] == DBNull.Value)
                        Notes = "";
                    else
                         Notes = (string)Reader["Notes"];
                    
                    
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                }

                Reader.Close();

            }
            catch (Exception Ex)
            {

                IsFound = false;
            }
            finally
            {
                Connection.Close();
            }


            return IsFound;

        }


        public static byte  GetTotallPassedTest(int LocalDrvingLicenseApplicationID)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
        
                select TestPassed=count(TestTypeID)
		                from TestAppointments Inner Join Tests 
		                On
		                TestAppointments.TestAppointmentID=tests.TestAppointmentID

                Where TestAppointments.LocalDrivingLicenseApplicationID=@LocalDrvingLicenseApplicationID
                and TestResult=1
            ";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrvingLicenseApplicationID", LocalDrvingLicenseApplicationID);

            byte TestCount = 0;

            try
            {
                Connection.Open();

                object obj = Command.ExecuteScalar();

                if(obj!=null    && byte.TryParse(obj.ToString(),out byte ptCount))
                {
                    TestCount = ptCount;
                }


            }
            catch (Exception Ex)
            {
                TestCount = 0;


            }
            finally
            {
                Connection.Close();
            }
            return TestCount;

        }
   



    }
}
