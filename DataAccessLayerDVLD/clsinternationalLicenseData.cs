using System;
using System.Data;

using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccessLayerDVLD
{
    public static class clsinternationalLicenseData
    {

        public static int AddNewInternationalLicense(int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID,
            DateTime IssuedDate, DateTime ExpiraionDate, int CreatedByUserID)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Qeury = @"

                     Update InternationalLicenses 
		             SET IsActive=0 
                     WHERE DriverID=@DriverID;       
        
                    Insert Into InternationalLicenses (ApplicationID,DriverID,IssuedUsingLocalLicenseID,
                        IssueDate,ExpirationDate,IsActive,CreatedByUserID)
	                    Values(@ApplicationID,@DriverID,@IssuedUsingLocalLicenseID,
	                    @IssueDate,@ExpirationDate,1,@CreatedByUserID);
                    Select SCOPE_IDENTITY();
                                ";


            SqlCommand Command = new SqlCommand(Qeury, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);

            Command.Parameters.AddWithValue("@IssueDate", IssuedDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpiraionDate);


            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);


            int InternatioalLicenseID = -1;

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && (int.TryParse(Result.ToString(), out int InsertedID)))
                    InternatioalLicenseID = InsertedID;


            }
            catch (Exception Ex)
            {
                InternatioalLicenseID = -1;
            }
            finally
            {
                Connection.Close();
            }

            return InternatioalLicenseID;

        }
        
        public static bool UpdateInternationalLicense(int InternationalLicenseID,int ApplicationID, int DriverID, int IssueingUsingLocalLicenseID,
            DateTime IssuedDate, DateTime ExpiraionDate, bool IsActive, int CreatedByUserID)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Qeury = @"
                   
                        UPDATE InternationalLicenses 
	                        SET ApplicationID=@ApplicationID,
		                        DriverID=@DriverID,
		                        IssuedUsingLocalLicenseID=@IssueingUsingLocalLicenseID,
		                        IssueDate=@IssuedDate,
		                        ExpirationDate=@ExpiraionDate,
		                        IsActive=@IsActive,
		                        CreatedByUserID=@CreatedByUserID 
                        Where InternationalLicenseID=@InternationalLicenseID
                            ";


            SqlCommand Command = new SqlCommand(Qeury, Connection);

            Command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
            
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@IssuedDate", IssuedDate);
            Command.Parameters.AddWithValue("@ExpiraionDate", ExpiraionDate);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            int RowsAffected = 0;

            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();


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

        public static bool DeleteInternationalLicense(int InternationalLicenseID)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Qeury = @"
        
                DELETE InternationalLicenses 
                Where InternationalLicenseID=@InternationalLicenseID
                            ";


            SqlCommand Command = new SqlCommand(Qeury, Connection);

            Command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
      
            int RowsAffected = 0;

            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();


            }
            catch (Exception Ex)
            {
                RowsAffected = 0;
            }
            finally
            {
                Connection.Close();
            }

            return RowsAffected > 0;

        }


        public static DataTable GetAllInternationalsLicenses()
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Qeury = @"
               
                Select  InternationalLicenseID,ApplicationID,DriverID,
                              IssuedUsingLocalLicenseID, IssueDate,ExpirationDate,IsActive
                          from InternationalLicenses 
                Order By IsActive ,ExpirationDate Desc

                            ";


            SqlCommand Command = new SqlCommand(Qeury, Connection);

            DataTable dtAllInternationalLicnses = new DataTable();

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                    dtAllInternationalLicnses.Load(Reader);


                Reader.Close();
            }
            catch (Exception Ex)
            {
                ;
            }
            finally
            {
                Connection.Close();
            }

            return dtAllInternationalLicnses;

        }

        public static DataTable GetInternationalLicensesForDriver(int DriverID)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
      
            string Qeury = @"
                SELECT InternationalLicenseID,ApplicationID,IssuedUsingLocalLicenseID,
	                IssueDate,ExpirationDate,IsActive
		            from InternationalLicenses
                WHERE DriverID=@DriverID
                        Order BY ExpirationDate desc
                            ";

            SqlCommand Command = new SqlCommand(Qeury, Connection);
            Command.Parameters.AddWithValue("@DriverID", DriverID);

            DataTable dtAllInternationalLicnses = new DataTable();

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                    dtAllInternationalLicnses.Load(Reader);


                Reader.Close();
            }
            catch (Exception Ex)
            {
                ;
            }
            finally
            {
                Connection.Close();
            }

            return dtAllInternationalLicnses;

        }

        public static bool GetInternationalLicenseByID(int InternationalLicenseID, ref int ApplicationID,ref int DriverID,ref int IssueingUsingLocalLicenseID,
          ref  DateTime IssuedDate,ref DateTime ExpiraionDate,ref bool IsActive,ref int CreatedByUserID)
        {

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Qeury = @"
                               Select * 
	                from InternationalLicenses 
                Where InternationalLicenseID=@InternationalLicenseID

                                            ";


            SqlCommand Command = new SqlCommand(Qeury, Connection);
            Command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);


            bool IsFound = false;

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();
                if(Reader.Read())
                {
                    IsFound = true;

                    ApplicationID = (int)Reader["ApplicationID"];
                    DriverID = (int)Reader["DriverID"];
                    IssueingUsingLocalLicenseID = (int)Reader["IssuedUsingLocalLicenseID"];
                    IsActive = (bool)Reader["IsActive"];
                    IssuedDate = (DateTime)Reader["IssueDate"];
                    ExpiraionDate = (DateTime)Reader["ExpirationDate"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];


                }




                Reader.Close();
            }
            catch (Exception Ex)
            {
                IsFound = false; ;
            }
            finally
            {
                Connection.Close();
            }

            return IsFound;



        }


        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Qeury = @"
                    SELECT TOP 1 InternationalLicenseID 
	                    FROM InternationalLicenses
                    WHERE DriverID=@DriverID  AND GetDate() between IssueDate and ExpirationDate 
                    ORDER BY ExpirationDate DESC

                            ";

            SqlCommand Command = new SqlCommand(Qeury, Connection);
            Command.Parameters.AddWithValue("@DriverID", DriverID);

            int InternationalLicenseID = -1;

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if( Reader.Read())
                {
                    InternationalLicenseID = (int)Reader["InternationalLicenseID"];
                }

                Reader.Close();
            }
            catch (Exception Ex)
            {
                InternationalLicenseID = -1;
            }
            finally
            {
                Connection.Close();
            }

         
            return InternationalLicenseID;
        }



    }
}
