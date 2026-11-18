using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace DataAccessLayerDVLD
{
    public static  class clsDetainLicenseData
    {
        public static int DetainNewLicense(int LicenseID  ,DateTime DetainDate,float FineFees,   
         int   CreatedByUserID ,bool IsReleased)
        {

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
                       Insert Into DetainedLicenses 
            (LicenseID ,DetainDate,FineFees,CreatedByUserID,IsReleased)
            Values
            (@LicenseID ,@DetainDate,@FineFees,@CreatedByUserID,@IsReleased
            )

                        ";


            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID",LicenseID);
            Command.Parameters.AddWithValue("@DetainDate", DetainDate);
            Command.Parameters.AddWithValue("@FineFees", FineFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@IsReleased", IsReleased);


            int DetainID = -1;


            try
            {
                Connection.Open();

                object obj = Command.ExecuteScalar();

                if(obj!=null  && int.TryParse(obj.ToString(),out int DeatainIDInserted))
                {
                    DetainID = DeatainIDInserted;
                }



            }catch(Exception Ex)
            {
                DetainID = -1;
            }
            finally
            {
                Connection.Close();

            }

            return DetainID;



        }

        public static bool ReleaseLicense(int LicenseID, 
              int  ReleasedByUserID ,int   ReleaseApplicationID)
        {

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
                    Update DetainedLicenses 
	                    Set IsReleased=1,
	                    ReleaseDate=@ReleaseDate,
	                    ReleasedByUserID=@ReleasedByUserID,
	                    ReleaseApplicationID=@ReleaseApplicationID
                    Where LicenseID=@LicenseID
                           
                        ";


            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
            Command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            Command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);


            int RwosAffected = -1;


            try
            {
                Connection.Open();

                RwosAffected=Command.ExecuteNonQuery();

            }
            catch (Exception Ex)
            {
                RwosAffected = 0;
            }
            finally
            {
                Connection.Close();

            }

            return RwosAffected>0;


        }

        public static bool GetDetainedLicenseByLicenseID(ref int DetainID,int LicenseID,ref DateTime DetainDate,ref float FineFees,
        ref int CreatedByUserID,ref bool IsReleased, ref DateTime ReleaseDate,ref int ReleasedByUserID
            ,ref int ReleaseApplicationID)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
                        Select* from DetainedLicenses
            Where LicenseID =@LicenseID
                        ";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID",LicenseID);

            bool IsFind = false;

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

         //       int LicenseID, DateTime DetainDate, float FineFees,
          //   int CreatedByUserID, bool IsReleased, DateTime ReleaseDate ,int ReleasedByUserID,int ReleaseApplicationID
             
                if (Reader.Read())
                {
                    IsFind = true;
                    DetainID = (int)Reader["DetainID"];
                    DetainDate = (DateTime)Reader["DetainDate"];
                    FineFees = Convert.ToSingle(Reader["FineFees"]);
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IsReleased = (bool)Reader["IsReleased"];

                    if(Reader["ReleaseDate"]==DBNull.Value)
                        ReleaseDate=DateTime.Now;

                    else
                        ReleaseDate = (DateTime)Reader["ReleaseDate"];

                    if (Reader["ReleasedByUserID"] == DBNull.Value)
                        ReleasedByUserID = -1;
                    else
                        ReleasedByUserID = (int)Reader["ReleasedByUserID"];

                    if (Reader["ReleaseApplicationID"] == DBNull.Value)
                        ReleaseApplicationID = -1;
                    else
                        ReleaseApplicationID = (int)Reader["ReleaseApplicationID"];





                }

                Reader.Close();


            }
            catch (Exception Ex)
            {
                IsFind = false;
            }
            finally
            {
                Connection.Close();
            }


            return IsFind;



        }

        public static bool GetDetainedLicenseByDetainID( int DetainID,ref  int LicenseID, ref DateTime DetainDate, ref float FineFees,
        ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate, ref int ReleasedByUserID
            , ref int ReleaseApplicationID)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string Query = @"
                        Select* from DetainedLicenses
            Where Detained =@DetainID)
                        ";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@DetainID", DetainID);

            bool IsFind = false;

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                //       int LicenseID, DateTime DetainDate, float FineFees,
                //   int CreatedByUserID, bool IsReleased, DateTime ReleaseDate ,int ReleasedByUserID,int ReleaseApplicationID

                if (Reader.Read())
                {
                    IsFind = true;
                    LicenseID = (int)Reader["LicenseID"];
                    DetainDate = (DateTime)Reader["DetainDate"];
                    FineFees = Convert.ToSingle(Reader["FineFees"]);
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IsReleased = (bool)Reader["IsReleased"];
                   
                    if (Reader["ReleaseDate"] == DBNull.Value)
                        ReleaseDate = DateTime.Now;

                    else
                        ReleaseDate = (DateTime)Reader["ReleaseDate"];

                    if (Reader["ReleasedByUserID"] == DBNull.Value)
                        ReleasedByUserID = -1;
                    else
                        ReleasedByUserID = (int)Reader["ReleasedByUserID"];

                    if (Reader["ReleaseApplicationID"] == DBNull.Value)
                        ReleaseApplicationID = -1;
                    else
                        ReleaseApplicationID = (int)Reader["ReleaseApplicationID"];



                }

                Reader.Close();


            }
            catch (Exception Ex)
            {
                IsFind = false;
            }
            finally
            {
                Connection.Close();
            }


            return IsFind;



        }


        public static DataTable GetAllDetaindLicenses()
        {
            SqlConnection Connenction = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Query = @"
              
Select  DetainedLicenses.DetainID ,Licenses.LicenseID,DetainDate,IsReleased,FineFees,ReleaseDate,People.NationalNo,
		FullName=People.FirstName+' '+People.SecondName+' '+ISNULL(People.ThirdName,'')+People.LastName,
		DetainedLicenses.ReleaseApplicationID
		from DetainedLicenses  Inner Join Licenses On
		DetainedLicenses.LicenseID=Licenses.LicenseID Inner Join Drivers On
		Licenses.DriverID=Drivers.DriverID Inner Join People On
		Drivers.PersonID=People.PersonID
;	
                                        ";

            SqlCommand Command = new SqlCommand(Query, Connenction);


            DataTable dtDetainsLecenses = new DataTable(); ;

            try
            {
                Connenction.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                    dtDetainsLecenses.Load(Reader);


                Reader.Close();
            }
            catch (Exception Ex)
            {


            }
            finally
            {
                Connenction.Close();
            }

            return dtDetainsLecenses;
        }
        public static bool UpdateDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate, float FineFees,
          int CreatedByUserID)
        {
            SqlConnection Connenction = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Query = @"
                Update DetainedLicenses 
                Set LicenseID=@LicenseID
	                ,DetainDate=@DetainDate
	                ,FineFees=@FineFees
	                ,CreatedByUserID=@CreatedByUserID
                WHERE DetainID=@DetainID
                ";

            SqlCommand Command = new SqlCommand(Query, Connenction);

            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@DetainDate", DetainDate);
            Command.Parameters.AddWithValue("@FineFees", FineFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@DetainID", DetainID);


            int RowsAffected = 0;


            try
            {
                Connenction.Open();

                RowsAffected = Command.ExecuteNonQuery();



            }
            catch (Exception Ex)
            {
                RowsAffected = -1;

            }
            finally
            {
                Connenction.Close();
            }

            return RowsAffected > 0;


        }

        public static bool IsDetain(int LicenseID)
        {

            SqlConnection Connenction = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Query = @"
                select found=1 from DetainedLicenses 
	                Where IsReleased =0 
                    and LicenseID=@LicenseID
                                ";

            SqlCommand Command = new SqlCommand(Query, Connenction);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

         bool   IsFound = false;

            try
            {
                Connenction.Open();

                object obj = Command.ExecuteScalar();

                if(obj!=null)
                {
                    IsFound = true;
                }
                


            }
            catch (Exception Ex)
            {

                IsFound = false;
            }
            finally
            {
                Connenction.Close();
            }

         return    IsFound;




        }




    }
}
