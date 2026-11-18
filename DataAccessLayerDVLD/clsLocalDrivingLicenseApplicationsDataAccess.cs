using System;
using System.Data;
using System.Data.SqlClient;


using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DataAccessLayerDVLD
{
    public static  class clsLocalDrivingLicenseApplicationsDataAccess
    {
        public static DataTable GetAllLocalLicenseApplicatins()
        {
            string Query = @"
                Select * from LocalDrivingLicenseApplications_View ;
                ";


            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            SqlCommand command = new SqlCommand(Query, Connection);

            DataTable dtAllLocalLicenseApplicatinsList=new DataTable();


            try
            { 
                Connection.Open();

                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.HasRows)
                    dtAllLocalLicenseApplicatinsList.Load(Reader);

                Reader.Close();

            }
            catch (Exception Ex)
            {


            }finally
            {

                Connection.Close();
            }

            return dtAllLocalLicenseApplicatinsList;


        }

        public static int AddNewLocalDrvingLicenseApplication(
            int ApplicationID,int LicenseClassID)
        {
            string Query = @"
            
            Insert into LocalDrivingLicenseApplications
            (ApplicationID,LicenseClassID)
            Values
            (@ApplicationID,@LicenseClassID);
            Select SCOPE_IDENTITY();
                ";

            SqlConnection Connection= new SqlConnection(clsDataAccessSetting.ConnectionString);

            SqlCommand Command = new SqlCommand(Query,Connection);
          
            Command.Parameters.AddWithValue("@ApplicationID",ApplicationID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            int LocalDrivingID = -1;

            try
            {
                Connection.Open();


                object obj = Command.ExecuteScalar();

                if(obj!=null && (int.TryParse(obj.ToString(),out int InsertedId  )))
                    LocalDrivingID=InsertedId;


            }
            catch (Exception Ex )
            {
                 LocalDrivingID = -1;
            }
            finally
            {
                Connection.Close();
            }

            return LocalDrivingID;
        }


        public static bool UpdateLocalDrvingLicenseApplication(int LocalDrvingLicenseApplicationID,
          int ApplicationID, int LicenseClassID)
        {
            string Query = @"
           
            UPDATE LocalDrivingLicenseApplications 
		            Set ApplicationID=@ApplicationID,
		            LicenseClassID=@LicenseClassID
            WHERE LocalDrivingLicenseApplicationID=@LocalDrvingLicenseApplicationID
                ";

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@LocalDrvingLicenseApplicationID", LocalDrvingLicenseApplicationID);



            int rowsaffected = 0;

            try
            {
                Connection.Open();
                rowsaffected = Command.ExecuteNonQuery();

            }
            catch (Exception Ex)
            {
                rowsaffected = 0;
            }
            finally
            {
                Connection.Close();
            }

            return rowsaffected>0;
        }


        public static bool GetLocalDrvingLicenseApplicationByID(int LocalDrvingLicenseApplicationID,
          ref  int ApplicationID,ref  int LicenseClassID)
        {

            string Query = @"
                Select * from LocalDrivingLicenseApplications 
	                Where LocalDrivingLicenseApplicationID=@LocalDrvingLicenseApplicationID";

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LocalDrvingLicenseApplicationID", LocalDrvingLicenseApplicationID);


            bool IsFound = false;


            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();


                if (Reader.Read())
                {
                    IsFound = true;
                    ApplicationID = (int)Reader["ApplicationID"];
                    LicenseClassID = (int)Reader["LicenseClassID"];



                }

                Reader.Close();
            }
            catch (Exception ex)
            {
                IsFound = false;

            }finally
            {
                Connection.Close();
            }

            return IsFound;
        }


        public static bool GetLocalDrvingLicenseApplicationByApplicationID(ref int LocalDrvingLicenseApplicationID,
       int ApplicationID, ref int LicenseClassID)
        {

            string Query = @"
                Select * from LocalDrivingLicenseApplications 
	                Where ApplicationID=@ApplicationID";

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);


            bool IsFound = false;


            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();


                if (Reader.Read())
                {
                    IsFound = true;
                    LocalDrvingLicenseApplicationID = (int)Reader["LocalDrivingLicenseApplicationID"];
                    LicenseClassID = (int)Reader["LicenseClassID"];

                }

                Reader.Close();
            }
            catch (Exception ex)
            {
                IsFound = false;

            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }

        public static bool DeleteLocalDrvingLicenseApplication(int LocalDrvingLicenseApplicationID)
        {
            string Query = @"
           Delete LocalDrivingLicenseApplications 
	                Where LocalDrivingLicenseApplicationID=@LocalDrvingLicenseApplicationID
                ";

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrvingLicenseApplicationID", LocalDrvingLicenseApplicationID);



            int rowsaffected = 0;

            try
            {
                Connection.Open();

                rowsaffected = Command.ExecuteNonQuery();

            }
            catch (Exception Ex)
            {
                rowsaffected = 0;
            }
            finally
            {
                Connection.Close();
            }

            return rowsaffected > 0;
        }

            
        public static bool DoesPassedThisTest(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {
            string Query = @"
                    select   Top 1 TestResult
	                    from LocalDrivingLicenseApplications Inner Join TestAppointments On
	                    LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID=TestAppointments.LocalDrivingLicenseApplicationID
	                    Inner Join Tests On
	                    TestAppointments.TestAppointmentID=Tests.TestAppointmentID
                    Where 
                    LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID
                    And TestAppointments.TestTypeID=@TestTypeID
                    Order by TestAppointments.TestAppointmentID desc 
                ";

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


            bool IsFound = false;

            try
            {
                Connection.Open();

                object obj=Command.ExecuteScalar();

                if(obj != null && bool.TryParse(obj.ToString(),out bool findresult))
                {
                    IsFound = findresult;
                }



            }catch(Exception ex)
            {
                IsFound = false;
            }
            finally
            {

                Connection.Close();
            }

            return IsFound;
        }

        public static bool DoesAttendThisTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            string Query = @"
                    select   Top 1 Found=1
	                    from LocalDrivingLicenseApplications Inner Join TestAppointments On
	                    LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID=TestAppointments.LocalDrivingLicenseApplicationID
	                    Inner Join Tests On
	                    TestAppointments.TestAppointmentID=Tests.TestAppointmentID
                    Where 
                    LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID
                    And TestAppointments.TestTypeID=@TestTypeID
                    Order by TestAppointments.TestAppointmentID desc 
                ";

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


            bool IsFound = false;

            try
            {
                Connection.Open();

                object obj = Command.ExecuteScalar();

                if (obj != null )
                {
                    IsFound = true;
                }



            }
            catch (Exception ex)
            {
                IsFound = false;
            }
            finally
            {

                Connection.Close();
            }

            return IsFound;
        }

        public static byte TotallTrailsPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            string Query = @"
                    select TotallTrialPerTest=Count(TestID)
	                    from LocalDrivingLicenseApplications Inner Join TestAppointments On
	                    LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID=TestAppointments.LocalDrivingLicenseApplicationID
	                    Inner Join Tests On
	                    TestAppointments.TestAppointmentID=Tests.TestAppointmentID
                    Where 
                    LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID
                    And TestAppointments.TestTypeID=@TestTypeID
                   
                ";

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


            byte TotallTrials = 0;

            try
            {
                Connection.Open();

                object obj = Command.ExecuteScalar();

                if (obj != null && byte.TryParse(obj.ToString(),out byte Result))
                {
                    TotallTrials = Result;
                }



            }
            catch (Exception ex)
            {
                TotallTrials = 0;
            }
            finally
            {

                Connection.Close();
            }

            return TotallTrials;
        }

        public static bool IsTherAnActiveTestAppointmentForThisTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            string Query = @"
                    select   Top 1 Found=1
	                    from LocalDrivingLicenseApplications Inner Join TestAppointments On
	                    LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID=TestAppointments.LocalDrivingLicenseApplicationID
	                    Inner Join Tests On
	                    TestAppointments.TestAppointmentID=Tests.TestAppointmentID
                    Where 
                    LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID
                    And TestAppointments.TestTypeID=@TestTypeID and IsLocked=0
                    Order by TestAppointments.TestAppointmentID desc 
                ";

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


            bool IsFound = false;

            try
            {
                Connection.Open();

                object obj = Command.ExecuteScalar();

                if (obj != null && bool.TryParse(obj.ToString(), out bool findresult))
                {
                    IsFound = findresult;
                }



            }
            catch (Exception ex)
            {
                IsFound = false;
            }
            finally
            {

                Connection.Close();
            }

            return IsFound;
        }



    }
}
