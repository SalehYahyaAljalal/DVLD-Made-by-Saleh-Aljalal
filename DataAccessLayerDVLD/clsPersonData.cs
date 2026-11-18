using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Security.Policy;

namespace DataAccessLayerDVLD
{
    public static class clsPersonData
    {


        public static bool DeletePerson(int PersonID)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = "DELETE  from People Where PersonID=@PersonID";
            SqlCommand command=new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                RowAffected=command.ExecuteNonQuery();
                


            }
            catch(Exception ex) { }
            finally { connection.Close(); }
            
            return RowAffected > 0;
        }

   
        public static DataTable GetAllPeople()
        {

            DataTable AllPeople = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string guery = @"select People.PersonID,People.NationalNo,People.FirstName,People.SecondName
                            ,People.ThirdName,People.LastName,People.DateOfBirth,
                            case 
	                            when People.Gendor=0 then 'Male'
	                            else 'Fmail' 
	                            end as Gendor,
                            People.Address,People.Phone,People.Email,
                            People.ImagePath,Countries.CountryName 
                            from People inner join Countries on
                            People.NationalityCountryID=Countries.CountryID";
            SqlCommand command = new SqlCommand(guery, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    AllPeople.Load(reader);

                }
                reader.Close();




            }
            catch (Exception ex)
            {

            }
            finally { connection.Close(); }


            return AllPeople;

        }


        public static int AddNewPerson(string NationalNo, string FirstName, string SecondName, string ThirdName,
        string LastName,
        DateTime DateOfBirth, byte Gendor, string Address,
        string Phone, string Email, byte NationalityCountryID, string ImagePath)
        {

            int PerosnID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string guery = @"
                insert into People (NationalNo,FirstName,SecondName,ThirdName,LastName,
                            DateOfBirth,Gendor,Address,
                            Phone,Email,NationalityCountryID,ImagePath)
                            Values
                (@NationalNo, @FirstName, @SecondName, @ThirdName,
               @LastName,
               @DateOfBirth, @Gendor, @Address,
               @Phone, @Email, @NationalityCountryID, @ImagePath);
                            SELECT SCOPE_IDENTITY(); ";

            SqlCommand Command = new SqlCommand(guery, connection);

            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@SecondName", SecondName);

            if (ThirdName!="")
                Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                Command.Parameters.AddWithValue("@ThirdName", DBNull.Value);



            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            Command.Parameters.AddWithValue("@Gendor", Gendor);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@Phone", Phone);


            if (Email != "")
                Command.Parameters.AddWithValue("@Email", Email);
            else
                Command.Parameters.AddWithValue("@Email", DBNull.Value);


            Command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (ImagePath!="")
                Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                Command.Parameters.AddWithValue("@ImagePath", DBNull.Value);


            try
            {
                connection.Open();
                
                object Result = Command.ExecuteScalar();

                if (Result!=null &&(int.TryParse(Result.ToString(), out int InsertedID)))
                {
                    PerosnID = InsertedID;

                }

            }
            catch (Exception ex)
            {

            }
            finally { connection.Close(); }


            return PerosnID;



        }

      
        public static bool UpdatePersone(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName,
        string LastName,
        DateTime DateOfBirth, byte Gendor, string Address,
        string Phone, string Email, byte NationalityCountryID, string ImagePath)
        {
            int RowesEffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string guery = @"
                update People set 
                NationalNo=@NationalNo,
                FirstName=@FirstName,
                SecondName=@SecondName,
                ThirdName=@ThirdName,
                LastName=@LastName,
                DateOfBirth=@DateOfBirth,
                Gendor=@Gendor,
                Address=@Address,
                Phone=@Phone,
                Email=@Email,
                NationalityCountryID=@NationalityCountryID,
                ImagePath=@ImagePath

                WHERE PersonID=@PersonID   ";

            SqlCommand Command = new SqlCommand(guery, connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@SecondName", SecondName);

            if (ThirdName!="")
                Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                Command.Parameters.AddWithValue("@ThirdName", DBNull.Value);



            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            Command.Parameters.AddWithValue("@Gendor", Gendor);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@Phone", Phone);


            if (Email != "")
                Command.Parameters.AddWithValue("@Email", Email);
            else
                Command.Parameters.AddWithValue("@Email", DBNull.Value);


            Command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (ImagePath!="")
                Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                Command.Parameters.AddWithValue("@ImagePath", DBNull.Value);
            try
            {
                connection.Open();
                RowesEffected=Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return RowesEffected>0;

        }

        
        public static bool FindPersonByID(int PersonID, ref string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName,
        ref string LastName,
        ref DateTime DateOfBirth, ref byte Gendor, ref string Address,
        ref string Phone, ref string Email, ref byte NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string guery = @"select * from People where PersonID=@PersonID";
            SqlCommand command = new SqlCommand(guery, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);



            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    PersonID=(int)reader["PersonID"];
                    NationalNo=(string)reader["NationalNo"];
                    FirstName=(string)reader["FirstName"];
                    SecondName=(string)reader["SecondName"];
                    if (reader["ThirdName"]!=DBNull.Value)
                    {
                        ThirdName=(string)reader["ThirdName"];
                    }
                    else
                    {
                        ThirdName="";
                    }
                    LastName=(string)reader["LastName"];
                    DateOfBirth=(DateTime)reader["DateOfBirth"];
                    Gendor=Convert.ToByte(reader["Gendor"]);
                    Address=(string)reader["Address"];
                    Phone=(string)reader["Phone"];
                    if (reader["Email"]!=DBNull.Value)
                    {
                        Email=(string)reader["Email"];
                    }
                    else
                    {
                        Email="";
                    }
                    NationalityCountryID=Convert.ToByte(reader["NationalityCountryID"]);
                    if (reader["ImagePath"]!=DBNull.Value)
                    {
                        ImagePath=(string)reader["ImagePath"];
                    }
                    else
                    {
                        ImagePath="";
                    }

                }

                reader.Close();



            }
            catch (Exception ex) { }
            finally { connection.Close(); }

            return isFound;

        }


        public static bool FindPersonByNational(ref int PersonID, string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName,
        ref string LastName,
        ref DateTime DateOfBirth, ref byte Gendor, ref string Address,
        ref string Phone, ref string Email, ref byte NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string guery = @"select * from People where NationalNo=@NationalNo";
            SqlCommand command = new SqlCommand(guery, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);



            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    PersonID=(int)reader["PersonID"];
                    NationalNo=(string)reader["NationalNo"];
                    FirstName=(string)reader["FirstName"];
                    SecondName=(string)reader["SecondName"];
                    if (reader["ThirdName"]!=DBNull.Value)
                    {
                        ThirdName=(string)reader["ThirdName"];
                    }
                    else
                    {
                        ThirdName="";
                    }
                    LastName=(string)reader["LastName"];
                    DateOfBirth=(DateTime)reader["DateOfBirth"];
                    Gendor=Convert.ToByte(reader["Gendor"]);
                    Address=(string)reader["Address"];
                    Phone=(string)reader["Phone"];
                    if (reader["Email"]!=DBNull.Value)
                    {
                        Email=(string)reader["Email"];
                    }
                    else
                    {
                        Email="";
                    }
                    NationalityCountryID=Convert.ToByte(reader["NationalityCountryID"]);
                    if (reader["ImagePath"]!=DBNull.Value)
                    {
                        ImagePath=(string)reader["ImagePath"];
                    }
                    else
                    {
                        ImagePath="";
                    }

                }
                reader.Close();



            }
            catch (Exception ex)
            {
                isFound=false;
                Console.WriteLine(ex.Message);
            }
            finally { connection.Close(); }

            return isFound;

        }

        public static bool IsPersonExist(string NationalNo)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string guery = @"select found=1 from People where NationalNo=@NationalNo";
            SqlCommand command = new SqlCommand(guery, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    isFound = true;
                }
                reader.Close();







            }
            catch (Exception ex) { }
            finally { connection.Close(); }
            return isFound;
        }
        public static bool IsPersonExist(int PersonID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string guery = @"select found=1 from People where PersonID=@PersonID";
            SqlCommand command = new SqlCommand(guery, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader(); 
                if(reader.HasRows)
                {
                   isFound = true;  
                }
                reader.Close();







            }catch(Exception ex) { }
            finally { connection.Close(); } 
            return isFound;
        }


    }
}
