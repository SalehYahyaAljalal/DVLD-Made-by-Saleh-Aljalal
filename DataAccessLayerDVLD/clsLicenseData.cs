using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

namespace DataAccessLayerDVLD
{
    public static  class clsLicenseData
    {
        public static int AddNewLicense(int ApplicationID, int DriverID, int LicenseClass,
            DateTime IssuedDate, DateTime ExpiraionDate, string Notes, float PaisFees,
            bool IsActive, byte IssuedReason, int CreatedByUserID)
        {
            int LicenseID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Qeury = @"
                           Insert into Licenses (ApplicationID,DriverID,LicenseClass,IssueDate,ExpirationDate,Notes,
                           PaidFees,IsActive,IssueReason,CreatedByUserID)
                           Values
                           (@ApplicationID,@DriverID,@LicenseClass,@IssuedDate,@ExpiraionDate,@Notes,
                                @PaisFees,@IsActive,@IssueReason,@CreatedByUserID)
                           Select SCOPE_IDENTITY();
                                ;";


            SqlCommand Command = new SqlCommand(Qeury, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
            Command.Parameters.AddWithValue("@IssuedDate", IssuedDate);
            Command.Parameters.AddWithValue("@ExpiraionDate", ExpiraionDate);
            if (Notes != "")
                Command.Parameters.AddWithValue("@Notes", Notes);
            else
                Command.Parameters.AddWithValue("@Notes", DBNull.Value);


            Command.Parameters.AddWithValue("@PaisFees", PaisFees);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@IssueReason", IssuedReason);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);


            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && (int.TryParse(Result.ToString(), out int InsertedID)))
                    LicenseID = InsertedID;


            }
            catch (Exception Ex)
            {
                //  Console.WriteLine(Ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return LicenseID;

        }


        public static bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass,
         DateTime IssuedDate, DateTime ExpiraionDate, string Notes, float PaisFees,
         bool IsActive, byte IssuedReason, int CreatedByUserID)
        {

            int Rowsaffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Qeury = @"
                          
                            Update Licenses 
		                            Set ApplicationID=@ApplicationID,
		                            DriverID=@DriverID,
		                            LicenseClass=@LicenseClass,
		                            IssueDate=@IssuedDate,
		                            ExpirationDate=@ExpiraionDate,
		                            Notes=@Notes,
		                            PaidFees=@PaisFees,
		                            IsActive=@IsActive,
		                            IssueReason=@IssuedReason,
		                            CreatedByUserID=@CreatedByUserID

                            WHERE LicenseID=@LicenseID
                                ;";

            SqlCommand Command = new SqlCommand(Qeury, Connection);

            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
            Command.Parameters.AddWithValue("@IssuedDate", IssuedDate);
            Command.Parameters.AddWithValue("@ExpiraionDate", ExpiraionDate);
            if (Notes != "")
                Command.Parameters.AddWithValue("@Notes", Notes);
            else
                Command.Parameters.AddWithValue("@Notes", DBNull.Value);


            Command.Parameters.AddWithValue("@PaisFees", PaisFees);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@IssueReason", IssuedReason);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);


            try
            {
                Connection.Open();

                Rowsaffected = Command.ExecuteNonQuery();



            }
            catch (Exception Ex)
            {
                //  Console.WriteLine(Ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return Rowsaffected > 0;

        }


        public static bool GetLicenseByID(int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClass,
           ref DateTime IssuedDate, ref DateTime ExpirationDate,
           ref string Notes, ref float PaisFees, ref bool IsActive,
           ref byte IssuedReason, ref int CreatedByUserID)
        {
            bool IsFOund = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Qurey = @"
                             Select * from Licenses 
                             wHERE LicenseID=@LicenseID
                            ";
            SqlCommand Command = new SqlCommand(Qurey, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsFOund = true;

                    ApplicationID = (int)Reader["ApplicationID"];
                    DriverID = (int)Reader["DriverID"];
                    LicenseClass = (int)Reader["LicenseClass"];
                    IssuedDate = (DateTime)Reader["IssueDate"];
                    ExpirationDate = (DateTime)Reader["ExpirationDate"];
                    if (Reader["Notes"] != DBNull.Value)
                    {
                        Notes = (string)Reader["Notes"];
                    }
                    else
                        Notes = "";

                    PaisFees = Convert.ToSingle(Reader["PaidFees"]);
                    IsActive = (bool)Reader["IsActive"];
                    IssuedReason = Convert.ToByte(Reader["IssueReason"]);
                    CreatedByUserID = (int)Reader["CreatedByUserID"];

                }


                Reader.Close();

            }
            catch (Exception EX)
            {
                //  Console.WriteLine(EX.Message);

                IsFOund = false;

            }
            finally
            {
                Connection.Close();
            }

            return IsFOund;

        }


        public static DataTable GetAllLicenses()
        {

            string Query = @"Select * from Licenses Order By IssueDate
                ";

            DataTable dtAllincense = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {
                Connection.Open();

                SqlDataReader reader = Command.ExecuteReader();

                if (reader.HasRows)

                {
                    dtAllincense.Load(reader);
                }

                reader.Close();

            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }

            finally
            {
                Connection.Close();
            }

            return dtAllincense;

        }


        public static int GetActiveLicensePerPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;


            string Query = @"
            Select ActiveLicenseID=LicenseID
	            from Licenses Inner Join Drivers On
	            Licenses.DriverID=Drivers.DriverID 

            Where Licenses.LicenseClass=@LicenseClassID  and IsActive=1 
            AND Drivers.PersonID=@PersonID
                ";

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();
                if (Result != null && (int.TryParse(Result.ToString(), out int InsertedID)))
                    LicenseID = InsertedID;



            }
            catch (Exception Ex)
            {
                LicenseID = -1;
            }
            finally
            {

                Connection.Close();
            }

            return LicenseID;


        }

        public static bool DeactiveLicense(int LicenseID)
        {
            int rowsAffected = -1;
            string Query = @" 

              Update Licenses 
	              SET	 IsActive=0 
              Where LicenseID=@LicenseID

                    ";


            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);


            try
            {
                Connection.Open();

                rowsAffected = Command.ExecuteNonQuery();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }

            finally
            {
                Connection.Close();
            }

            return rowsAffected > 0;
        }


        public static bool IsThereAnActiveLicensePerPersonID(int PersonID, int LicenseClassID)
        {
            return GetActiveLicensePerPersonID(PersonID, LicenseClassID) > 0;
        }
        public static DataTable GetDriverLicenses(int DriverID)
        {
            string Query = @"
            Select LicenseID ,ApplicationID,LicenseClasses.ClassName,IssueDate,ExpirationDate, IsActive
		            from Licenses Inner JOIN LicenseClasses On
		            Licenses.LicenseClass=LicenseClasses.LicenseClassID
            WHERE DriverID=@DriverID
            order by IsActive desc,ExpirationDate desc
                    ";


            DataTable dtAllincense = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@DriverID", DriverID);


            try
            {
                Connection.Open();

                SqlDataReader reader = Command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtAllincense.Load(reader);
                }

                reader.Close();

            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }

            finally
            {
                Connection.Close();
            }

            return dtAllincense;



        }






    }
}
