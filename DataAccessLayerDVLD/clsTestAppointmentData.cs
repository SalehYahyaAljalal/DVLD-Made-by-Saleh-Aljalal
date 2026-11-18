using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace DataAccessLayerDVLD
{
    public static class clsTestAppointmentData
    {
        //TestAppointmentID TestTypeID  LocalDrivingLicenseApplicationID AppointmentDate PaidFees CreatedByUserID IsLocked
       public static bool GetTestAppointmentByID(int TestAppointmentID ,ref int TestTypeID ,ref int LocalDrivingLicenseApplicationID,
          ref  DateTime AppointmentDate ,ref float PaidFees ,ref int CreatedByUserID ,ref bool  IsLocked,ref int RetakeApplicarionID)
        {

            SqlConnection COnnection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"

  Select * from TestAppointments
  Where TestAppointmentID=@TestAppointmentID

            ";

           SqlCommand Command=new SqlCommand(Query, COnnection);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            bool IsFound = false;

            try
            {
                COnnection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if(Reader.Read())
                {
                    IsFound = true;

                    TestTypeID = (int)Reader["TestTypeID"];
                    LocalDrivingLicenseApplicationID = (int)Reader["LocalDrivingLicenseApplicationID"];
                   AppointmentDate = (DateTime)Reader["AppointmentDate"];
                   PaidFees = Convert.ToSingle(Reader["PaidFees"]);
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IsLocked = (bool)Reader["IsLocked"];

                    if (Reader["RetakeTestApplicationID"] == DBNull.Value)
                        RetakeApplicarionID = -1;
                    else
                        RetakeApplicarionID = (int)Reader["RetakeTestApplicationID"];

                }

                Reader.Close();

            }
            catch(Exception Ex)
            {
                IsFound=false;
            }
            finally
            {

                COnnection.Close();
            }
        

            return IsFound;


        }


        public static bool GetLastTestAppointment(ref int TestAppointmentID,  int TestTypeID,  int LocalDrivingLicenseApplicationID,
       ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked,ref int RetakeApplicarionID)
        {

            SqlConnection COnnection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
              Select Top 1 * 
              from TestAppointments Where TestTypeID=@TestTypeID
              and LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID
              Order by TestAppointmentID Desc
            ";

            SqlCommand Command = new SqlCommand(Query, COnnection);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);


            bool IsFound = false;

            try
            {
                COnnection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsFound = true;

                    TestAppointmentID = (int)Reader["TestAppointmentID"];

                    AppointmentDate = (DateTime)Reader["AppointmentDate"];
                    PaidFees = Convert.ToSingle(Reader["PaidFees"]);
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IsLocked = (bool)Reader["IsLocked"];

                    if (Reader["RetakeApplicarionID"] == DBNull.Value)
                        RetakeApplicarionID = -1;
                    else
                        RetakeApplicarionID = (int)Reader["RetakeApplicarionID"];


                }
                Reader.Close();

            }
            catch (Exception Ex)
            {
                IsFound = false;
            }
            finally
            {

                COnnection.Close();
            }


            return IsFound;


        }

        public static DataTable GetAllTestAppointments()
        {
            SqlConnection COnnection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
  
Select * from TestAppointments_View
Order by AppointmentDate desc

            ";

            SqlCommand Command = new SqlCommand(Query, COnnection);
            DataTable dtAllTestsAppointments = new DataTable();

            try
            {

                COnnection.Open();

                SqlDataReader Reader=Command.ExecuteReader();

                if (Reader.HasRows)
                    dtAllTestsAppointments.Load(Reader);


                Reader.Close();

            }
            catch(Exception E)
            {

            }
            finally
            {
                COnnection.Close();


            }

            return dtAllTestsAppointments;

        }


        public static DataTable GetApplicationTestAppointmentPerTestType(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {
            SqlConnection COnnection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"

  Select TestAppointmentID,AppointmentDate,PaidFees,IsLocked
  from TestAppointments
  Where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID
  and TestTypeID=@TestTypeID

  Order by TestAppointmentID Desc;


            ";

            SqlCommand Command = new SqlCommand(Query, COnnection);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


            DataTable dtAllTestsAppointments = new DataTable();

            try
            {

                COnnection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                    dtAllTestsAppointments.Load(Reader);


                Reader.Close();

            }
            catch (Exception E)
            {

            }
            finally
            {
                COnnection.Close();


            }

            return dtAllTestsAppointments;

        }

        public static int AddNewTestAppointment(  int TestTypeID,  int LocalDrivingLicenseApplicationID,
        DateTime AppointmentDate,  float PaidFees,  int CreatedByUserID,  bool IsLocked, int RetakeApplicarionID)
        {
            SqlConnection COnnection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
                          Insert into TestAppointments 
              (TestTypeID ,
              LocalDrivingLicenseApplicationID
              , AppointmentDate
              , PaidFees 
              ,CreatedByUserID
              ,IsLocked,RetakeTestApplicationID
              )


              Values(@TestTypeID ,
              @LocalDrivingLicenseApplicationID
              , @AppointmentDate
              , @PaidFees 
              ,@CreatedByUserID
              ,@IsLocked,@RetakeTestApplicationID)
              Select SCOPE_IDENTITY();
                        ";

            SqlCommand Command = new SqlCommand(Query, COnnection);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@IsLocked", IsLocked);

            if (RetakeApplicarionID == -1)
                Command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
            else
                Command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeApplicarionID);




            int TestAppointmnetID = -1;

            try
            {
                COnnection.Open();

                object obj = Command.ExecuteScalar();

                if(obj!=null && int.TryParse(obj.ToString(),out int InsertedID))
                {
                    TestAppointmnetID = InsertedID;
                }



            }catch(Exception Ex)
            {
                TestAppointmnetID = -1;

            }
            finally
            {
                COnnection.Close();
            }

            return TestAppointmnetID;



        }

        public static bool UpdateTestAppointment(int TestAppoitmentID,int TestTypeID, int LocalDrivingLicenseApplicationID,
     DateTime AppointmentDate, float PaidFees, int CreatedByUserID, bool IsLocked,int RetakeApplicarionID)
        {
            SqlConnection COnnection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
                                      Update  TestAppointments 
             SET TestTypeID=@TestTypeID ,
              LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID
              , AppointmentDate=@AppointmentDate
              , PaidFees=@PaidFees
              ,CreatedByUserID=@CreatedByUserID
              ,IsLocked=@IsLocked 
                ,RetakeTestApplicationID=@RetakeTestApplicationID

            Where TestAppointmentID=@TestAppoitmentID
                        ";

            SqlCommand Command = new SqlCommand(Query, COnnection);
            Command.Parameters.AddWithValue("@TestAppoitmentID", TestAppoitmentID);

            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@IsLocked", IsLocked);


            if (RetakeApplicarionID == -1)
                Command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
            else
                Command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeApplicarionID);



            int Rowaffected = 0;

            try
            {
                COnnection.Open();

                Rowaffected = Command.ExecuteNonQuery();



            }
            catch (Exception Ex)
            {
                Rowaffected = 0;

            }
            finally
            {
                COnnection.Close();
            }

            return Rowaffected > 0; ;



        }


        public static int GetTestID(int TestAppointmentID)
        {
            SqlConnection COnnection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
           

            Select TestID 
            from Tests 
            Where TestAppointmentID=@TestAppointmentID

                        ";

            SqlCommand Command = new SqlCommand(Query, COnnection);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            int TestID = -1;

            try
            {
                COnnection.Open();
              
                
                object obj = Command.ExecuteScalar();

                if (obj != null && int.TryParse(obj.ToString(), out int InsertedID))
                {
                    TestID = InsertedID;
                }



            }
            catch (Exception E)
            {
                TestID = -1;

            }

            return TestID;

        }







    }
}
