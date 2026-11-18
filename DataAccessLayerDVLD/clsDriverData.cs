using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayerDVLD
{
    public static  class clsDriverData
    {
        public static int AddNewDriver(int PersonID, int CreatedByUserID)
        {
            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"Insert into Drivers (PersonID,CreatedByUserID,CreatedDate)
                                Values
                              (@PersonID,@CreatedByUserID,@CreatedDate)
                            Select SCOPE_Identity();
                                      ";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);


            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                    RowsAffected = InsertedID;


            }
            catch (Exception Ex)
            {
                //  Console.WriteLine(Ex.Message);


            }
            finally
            {
                Connection.Close();
            }

            return RowsAffected;
        }


        public static bool UpdateDriver(int DriverID, int PersonID, int CreatedByUserID
           )
        {
            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Query = @"
                      
                            Update Drivers 
	                        SET PersonID=@PersonID,
	                        CreatedByUserID=@CreatedByUserID,
	                        CreatedDate =@CreatedDate,
                        Where DriverID=@DriverID
                                      ";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);


            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int Rowsaffect))
                    RowsAffected = Rowsaffect;


            }
            catch (Exception Ex)
            {
                //  Console.WriteLine(Ex.Message);


            }
            finally
            {
                Connection.Close();
            }

            return RowsAffected > 0;
        }



        public static bool GetDriverInfoByDriverID(int DriverID, ref int PersonID, ref int CreatedByUserID
            , ref DateTime CreatedDate)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Query = @"
                             Select * from Drivers 
                             Where DriverID=@DriverID
                             ";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    PersonID = (int)Reader["PersonID"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    CreatedDate = (DateTime)Reader["CreatedDate"];

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
        public static bool GetDriverInfoByPersonID(ref int DriverID, int PersonID, ref int CreatedByUserID
     , ref DateTime CreatedDate)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Query = @"
                             Select * from Drivers 
                             Where PersonID=@PersonID
                             ";



            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);


            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    DriverID = (int)Reader["DriverID"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    CreatedDate = (DateTime)Reader["CreatedDate"];

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

        public static DataTable GetAllDrivers()
        {
            DataTable dtDrivers = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Query = @"
                Select * from Drivers_View Order by FullName desc;
                                      ";
            SqlCommand Command = new SqlCommand(Query, Connection);


            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                    dtDrivers.Load(Reader);

                Reader.Close();

            }
            catch (Exception Ex)
            {
                //  Console.WriteLine(Ex.Message);


            }
            finally
            {
                Connection.Close();
            }

            return dtDrivers;

        }





    }
}
