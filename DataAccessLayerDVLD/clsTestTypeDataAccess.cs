using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayerDVLD
{
    public static class clsTestTypeDataAccess
    {
        public static DataTable GetAllTestTypes()
        {
            DataTable dtAllTests = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"Select * from TestTypes";


            SqlCommand Command = new SqlCommand(Query,Connection);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                    dtAllTests.Load(Reader);


                Reader.Close();

            }
            catch (Exception Ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return dtAllTests;  
        }
        public static bool UpdateTestType(int TestTypeID,string TestTypeTitle,
            string Description,float TestTypeFees)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
                Update TestTypes 
	                Set TestTypeTitle=@TestTypeTitle,
	                TestTypeDescription=@Description,
	                TestTypeFees =@TestTypeFees
                Where TestTypeID=@TestTypeID
                ";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
            Command.Parameters.AddWithValue("@Description", Description);
            Command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            int RowsAffected = 0;

            try
            {
                Connection.Open();

                RowsAffected=Command.ExecuteNonQuery(); 




            }
            catch (Exception Ex)
            {

            }finally
            {
                Connection.Close();
            }

            return RowsAffected>0;
        }

        public static bool FindTestTypeByID(int TestTypeID, ref string TestTitle,
           ref string Description, ref float TestTypeFees)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
            Select * from TestTypes Where TestTypeID=@TestTypeID;
                            ";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            bool IsFound = false;

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if(Reader.Read())
                {
                    IsFound=true;
                    TestTitle = (string)Reader["TestTypeTitle"];

                    Description= (string)Reader["TestTypeDescription"];
                    TestTypeFees = Convert.ToSingle(Reader["TestTypeFees"]);
                }

                Reader.Close();

            }catch(Exception Ex)
            {
                IsFound = false;
            }
            finally
            {

                Connection.Close();
            }

            return  IsFound;
        }

    }
}
