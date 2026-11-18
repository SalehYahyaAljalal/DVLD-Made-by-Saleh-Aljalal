using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
namespace DataAccessLayerDVLD
{
    public static  class clsApplicationTypeData
    {
        public static DataTable GetAllApplicationTypes()
        {
            DataTable AllApplicationsTypes = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Query = @"
                           
                    select * from ApplicationTypes Order by ApplicationTypeTitle

                           ";
            SqlCommand Command = new SqlCommand(Query,Connection);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if(Reader.HasRows)
                {
                    AllApplicationsTypes.Load(Reader);
                }

                Reader.Close(); 

            }catch(Exception Ex)
            {


            }
            finally
            {
                Connection.Close();
            }

            return AllApplicationsTypes;
        }

        public static int AddNewApplicationType(string Title,float Fees)
        {
            int ApplicationTypeID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"

            insert into ApplicationTypes 
	            (ApplicationTypeTitle,ApplicationFees)
	            Values
	            (@Title,@Fees) ;
	        sELECT SCOPE_IDENTITY();
                    ";

            SqlCommand Command=new SqlCommand(Query,connection);
            Command.Parameters.AddWithValue("@Title", Title);
            Command.Parameters.AddWithValue("@Fees", Fees);

            try
            {
                connection.Open();

                object result=Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    ApplicationTypeID = InsertedID;


                
            }
            catch(Exception Ex)
            {

                ApplicationTypeID = -1;
            }
            finally
            {

                connection.Close();
            }

            return ApplicationTypeID;

        }

        public static bool UpdateApplicationType(int ApplicationTypeID,string ApplicationTypeTitle,float ApplicationFees)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            int RowsAffected = 0;

            string Query = @"
                            Update ApplicationTypes 
                            Set ApplicationTypeTitle=@ApplicationTypeTitle,
                            	ApplicationFees=@ApplicationFees
                            Where ApplicationTypeID=@ApplicationTypeID;
                           ";
            SqlCommand Command = new SqlCommand(Query,Connection);
            Command.Parameters.AddWithValue("@ApplicationTypeTitle",ApplicationTypeTitle);
            Command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);


            try
            {
                Connection.Open();

                RowsAffected=Command.ExecuteNonQuery();

            }
            catch(Exception EX)
            {
                RowsAffected = 0;
            }
            finally
            {
                Connection.Close ();
            }



            return RowsAffected > 0;

        }
        public static bool FindApplicationTypeInfoByID(int ApplicationTypeID,
           ref  string ApplicationTypeTitle, ref float ApplicationFees)
        {

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Query = @"
                          Select * from ApplicationTypes 
                            Where ApplicationTypeID=@ApplicationTypeID

                           ";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
         
            bool IsFound=false;
            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsFound = true;
                    ApplicationTypeTitle = (string)Reader["ApplicationTypeTitle"];
                    ApplicationFees = Convert.ToSingle( Reader["ApplicationFees"]);

                }

                Reader.Close();

            }
            catch (Exception Ex)
            {
                IsFound=false;

            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }


    }
}
