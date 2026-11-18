using DataAccessLayerDVLD;
using System;
using System.Data;
using System.Data.SqlClient;


namespace DVLDProject
{
    public static class clsCountriesDataAccess
    {
        public static DataTable GetAllCountries()
        {
            DataTable AllCOuntries = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = "select * from Countries;";
            SqlCommand command = new SqlCommand(query, connection);


            try
            {
                connection.Open();
                SqlDataReader reader= command.ExecuteReader();
                if(reader.HasRows)
                {
                    AllCOuntries.Load(reader);
                }
                reader.Close();




            }catch(Exception ex) { }
            finally { connection.Close(); }

            return AllCOuntries;

        }


        public static bool Find(string CountryName,ref int CountryID)
        {
            bool isFind = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = @"select * from Countries where CountryName=@CountryName";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryName", CountryName);
            
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    isFind = true;
                    CountryID=(int)reader["CountryID"];


                }
                reader.Close();




            }catch(Exception ex) { }
            finally { connection.Close(); }

            return isFind;
        }


        public static bool FindByCountryID(ref string CountryName,  int CountryID)
        {
            bool isFind = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = @"select * from Countries where CountryID=@CountryID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryID", CountryID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFind = true;
                    CountryName=(string)reader["CountryName"];


                }
                reader.Close();




            }
            catch (Exception ex) { }
            finally { connection.Close(); }

            return isFind;
        }


    }
}
