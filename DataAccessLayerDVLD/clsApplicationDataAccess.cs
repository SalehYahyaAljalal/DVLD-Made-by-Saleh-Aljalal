using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayerDVLD
{
    public static  class clsApplicationDataAccess
    {
     
        public static bool GetApplicationInfoByID(int ApplicationID,
            ref int ApplicantPersonID,ref DateTime ApplicationDate,ref int ApplicationTypeID,
            ref byte ApplicationStatus,ref DateTime LastStatusDate,ref float PaidFees,ref int
            CreatedByUserID)
        {
            bool IsFound=false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
            Select * from Applications Where ApplicationID=@ApplicationID
            ";

            SqlCommand Command = new SqlCommand(Query,Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();


                //ApplicationID ApplicantPersonID   ApplicationDate
                //    ApplicationTypeID   ApplicationStatus 
                //    LastStatusDate  PaidFees CreatedByUserID

                if (Reader.Read())
                {
                    IsFound=true;
                    ApplicantPersonID = (int)Reader["ApplicantPersonID"];
                    ApplicationDate = (DateTime)Reader["ApplicationDate"] ;
                    ApplicationStatus = (byte)Reader["ApplicationStatus"];
                    ApplicationTypeID=(int)Reader["ApplicationTypeID"];
                    LastStatusDate = (DateTime)Reader["LastStatusDate"];
                    PaidFees = Convert.ToSingle(Reader["PaidFees"]); 
                    CreatedByUserID = (int)Reader["CreatedByUserID"];


                }
                Reader.Close(); 


            }catch (Exception E)

            {
                IsFound = false;
            }
            finally
            {
                Connection.Close();
            }

            return  IsFound;

        }

        public static int AddNewApplication(int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
             byte ApplicationStatus, DateTime LastStatusDate, float PaidFees, int
            CreatedByUserID)
        {

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
        
        Insert Into Applications (ApplicantPersonID,ApplicationDate,ApplicationTypeID,ApplicationStatus,
        LastStatusDate,PaidFees,CreatedByUserID)
        Values
        (@ApplicantPersonID,@ApplicationDate,@ApplicationTypeID,@ApplicationStatus,
        @LastStatusDate,@PaidFees,@CreatedByUserID);
        Select SCOPE_IDENTITY();
                    ";



            int ApplicationID = -1;

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            Command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            Command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                Connection.Open();


                object obj = Command.ExecuteScalar();

                if (obj != null && int.TryParse(obj.ToString(), out int InsertedID))
                    ApplicationID = InsertedID;


            }
            catch (Exception E)

            {
                ApplicationID = -1;
            }
            finally
            {
                Connection.Close();
            }


            return ApplicationID;

        }

        public static bool UpdateApplicaion(int ApplicationID,int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
                byte ApplicationStatus, DateTime LastStatusDate, float PaidFees, int
               CreatedByUserID)
        {

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
        
       
                Update Applications  
                set ApplicantPersonID=@ApplicantPersonID,
                ApplicationDate=@ApplicationDate
                ,ApplicationTypeID=@ApplicationTypeID,

                ApplicationStatus=@ApplicationStatus
                ,LastStatusDate=@LastStatusDate,
                PaidFees=@PaidFees
                ,CreatedByUserID=@CreatedByUserID

                Where ApplicationID=@ApplicationID;
                                    ";



            int Rowsaffected = 0;

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            Command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            Command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);



            try
            {
                Connection.Open();


                Rowsaffected = Command.ExecuteNonQuery();

                

            }
            catch (Exception E)

            {
                Rowsaffected = 0;
            }
            finally
            {
                Connection.Close();
            }


            return Rowsaffected > 0;

        }


        public static bool DeleteApplication(int ApplicationID)
        {

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
       
            Delete Applications Where ApplicationID=@ApplicationID
                                                ";

            int Rowsaffected = 0;

            SqlCommand Command = new SqlCommand(Query, Connection);

         
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);



            try
            {
                Connection.Open();


                Rowsaffected = Command.ExecuteNonQuery();



            }
            catch (Exception E)

            {
                Rowsaffected = 0;
            }
            finally
            {
                Connection.Close();
            }


            return Rowsaffected>0;

        }


        public static bool IsApplicationExists(int ApplicationID)
        {

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
       
           
            Select Found=1 from Applications Where ApplicationID=@ApplicationID
                                                            ";

            bool IsFound = false;

            SqlCommand Command = new SqlCommand(Query, Connection);


            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);



            try
            {
                Connection.Open();


                SqlDataReader Reader=Command.ExecuteReader();

                IsFound=Reader.HasRows;

                Reader.Close();


            }
            catch (Exception E)

            {
                IsFound = false ;
            }
            finally
            {
                Connection.Close();
            }


            return IsFound;

        }

        public static int GetActiveApplicationID(int PersonID,int ApplicationTypeID)
        {
            string Query = @"
            
            Select ApplicationID
	            from Applications Where ApplicantPersonID=@PersonID 
	            and ApplicationTypeID=@ApplicationTypeID
            and ApplicationStatus=1 
            ";

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand Command = new SqlCommand(Query,Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            int ApplicationID = -1;

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                    ApplicationID =(int)Reader["ApplicationID"];

                Reader.Close();

            }
            catch(Exception Ex)
            {
                ApplicationID = -1;

            }
            finally
            {
                Connection.Close();
            }


            return ApplicationID;
        }

        public static bool DoesPersonHaveActiveApplicationID(int PersonID, int ApplicationTypeID)
        {
            return (GetActiveApplicationID(PersonID, ApplicationTypeID)!=-1);
        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID, int ApplicationTypeID,int LicenseClassID)
        {
            string Query = @"
            Select ActiveApplicationID=Applications.ApplicationID  from Applications 
            Inner Join LocalDrivingLicenseApplications on Applications.ApplicationID=
            LocalDrivingLicenseApplications.ApplicationID 

            WHERE ApplicantPersonID=@PersonID
            and ApplicationTypeID=@ApplicationTypeID
            and LocalDrivingLicenseApplications.LicenseClassID=@LicenseClassID
            and ApplicationStatus=1
                ";

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString );
            SqlCommand Command = new SqlCommand(Query,Connection);


            Command.Parameters.AddWithValue("@PersonID",PersonID);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            int ActiveApplication = -1;

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                    ActiveApplication = (int)Reader["ActiveApplicationID"];


                Reader.Close();

            }

            catch(Exception)
            {

            }finally
            {

                Connection.Close ();

            }

            return ActiveApplication;

        }


        public static bool UpdateApplicaionStatus(int ApplicationID,short NewStatus)
        {

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
        
       
                Update Applications  
                set 
                ApplicationStatus=@ApplicationStatus
                ,LastStatusDate=@LastStatusDate
               
                Where ApplicationID=@ApplicationID;
                                    ";



            int Rowsaffected = 0;

            SqlCommand Command = new SqlCommand(Query, Connection);


            Command.Parameters.AddWithValue("@ApplicationStatus", NewStatus);
            Command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);



            try
            {
                Connection.Open();


                Rowsaffected = Command.ExecuteNonQuery();



            }
            catch (Exception E)

            {
                Rowsaffected = 0;
            }
            finally
            {
                Connection.Close();
            }


            return Rowsaffected > 0;

        }


    }
}
