using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayerDVLD
{
    public static class clsLicenseClassData
    {




        public static DataTable GetAllLicenseClass()
        {
            DataTable AllLicenseClasses = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Query = @" Select* From LicenseClasses";

            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                    AllLicenseClasses.Load(Reader);

                Reader.Close();

            }
            catch (Exception Ex)
            {

            }
            finally
            {
                Connection.Close();

            }

            return AllLicenseClasses;


        }

        public static bool GetLicenseClassInfoByClassName(ref int LinceseClassID, string LicenseClassName
            , ref string ClassDescription, ref byte MinimumAllowedAge, ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Query = @" 
              Select* From LicenseClasses
              WHERE ClassName= @LicenseClassName
                ";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseClassName", LicenseClassName);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;

                    LinceseClassID = (int)Reader["LicenseClassID"];
                    LicenseClassName = (string)Reader["ClassName"];
                    ClassDescription = (string)Reader["ClassDescription"];
                    MinimumAllowedAge = Convert.ToByte(Reader["MinimumAllowedAge"]);
                    DefaultValidityLength = Convert.ToByte(Reader["DefaultValidityLength"]);
                    ClassFees = Convert.ToSingle(Reader["ClassFees"]);

                }

                Reader.Close();

            }
            catch (Exception Ex)
            {
                // Console.WriteLine(Ex.Message);
                IsFound = false;

            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }


        public static bool GetLicenseClassInfoByID(int LicenseClassID, ref string LicenseClassName
               , ref string ClassDescription, ref byte MinimumAllowedAge, ref byte DefaultValidityLength,
            ref float ClassFees)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Query = @" 
              Select* From LicenseClasses
              WHERE LicenseClassID= @LicenseClassID
                ";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;

                    LicenseClassName = (string)Reader["ClassName"];
                    ClassDescription = (string)Reader["ClassDescription"];
                    MinimumAllowedAge = Convert.ToByte(Reader["MinimumAllowedAge"]);
                    DefaultValidityLength = Convert.ToByte(Reader["DefaultValidityLength"]);
                    ClassFees = Convert.ToSingle(Reader["ClassFees"]);

                }

                Reader.Close();

            }
            catch (Exception Ex)
            {
                //   Console.WriteLine(Ex.Message);
                IsFound = false;

            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }



        public static int AddNewLicenseClass(string LicenseClassName
               , string ClassDescription, byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            int LicenseClassID = -1;

            string Query = @"
            
insert into LicenseClasses (ClassName,ClassDescription,
			MinimumAllowedAge,DefaultValidityLength,ClassFees)
		VALUES (@ClassName,@ClassDescription,
			@MinimumAllowedAge,@DefaultValidityLength,@ClassFees)
                ";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ClassName", LicenseClassName);
            Command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
            Command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
            Command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
            Command.Parameters.AddWithValue("@ClassFees", ClassFees);


            try
            {

                Connection.Open();

                object obj = Command.ExecuteScalar();

                if (obj != null && (int.TryParse(obj.ToString(), out int insertedID)))
                    LicenseClassID = insertedID;



            }
            catch (Exception Ex)
            {


            }
            finally
            {
                Connection.Close();
            }


            return LicenseClassID;


        }

        public static bool UpdateLicenseClass(int LinceseClassID, string LicenseClassName
                 , string ClassDescription, byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            int RowsAffected = -1;

            string Query = @"
           
update LicenseClasses 
	Set ClassName=@LicenseClassName,
	ClassDescription=@ClassDescription,
	MinimumAllowedAge=@MinimumAllowedAge,
	DefaultValidityLength=@DefaultValidityLength,
	ClassFees =@ClassFees,
WHERE LicenseClassID=@LinceseClassID
                ";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ClassName", LicenseClassName);
            Command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
            Command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
            Command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
            Command.Parameters.AddWithValue("@ClassFees", ClassFees);
            Command.Parameters.AddWithValue("@LinceseClassID", LinceseClassID);


            try
            {

                Connection.Open();

                object obj = Command.ExecuteScalar();

                if (obj != null && (int.TryParse(obj.ToString(), out int RsAffected)))
                    RowsAffected = RsAffected;



            }
            catch (Exception Ex)
            {


            }
            finally
            {
                Connection.Close();
            }


            return RowsAffected > 0;


        }

    }
}
