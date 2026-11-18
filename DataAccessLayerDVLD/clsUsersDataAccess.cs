using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Input;

namespace DataAccessLayerDVLD
{
    public static class clsUsersDataAccess
    {


        public static DataTable GetAllUsers()
        {
            DataTable AllUsers = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
           string Query = @"
                            Select Users.UserID,Users.PersonID,
                FullName=People.FirstName+' '+People.SecondName+' '+People.ThirdName+' '+People.LastName,
                Users.UserName,Users.IsActive 
	                from Users Inner Join People 
	                On
	                Users.PersonID=People.PersonID
	";

            SqlCommand Command=new SqlCommand(Query,Connection);

            try
            {
                Connection.Open();

                SqlDataReader Reader=Command.ExecuteReader();

                if(Reader.HasRows)
                {
                    AllUsers.Load(Reader);
                }


            }catch(Exception Ex)
            {

            }
            finally
            {
                Connection.Close();
            }


            return AllUsers;
        }


        public static int AddNewUser(int PersonID,string UserName,string Password, bool IsActive)
        {
            int UserID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = @"Insert INTO Users (PersonID,UserName,Password,IsActive)
                            Values(@PersonID,@UserName,@Password,@IsActive);
                            SELECT SCOPE_IDENTITY();";
            SqlCommand comand=new SqlCommand(query, connection);

            
            comand.Parameters.AddWithValue("@PersonID", PersonID);
            comand.Parameters.AddWithValue("@UserName", UserName);
            comand.Parameters.AddWithValue("@Password", Password);
            comand.Parameters.AddWithValue("@IsActive", IsActive);




            try
            {
                connection.Open();
                object RowAdded=comand.ExecuteScalar();
                if(RowAdded!=null && (int.TryParse(RowAdded.ToString(),out int InsertedValue)))
                {
                    UserID = InsertedValue;
                }


            }catch(Exception ex) { }
            finally { connection.Close(); }

            return UserID;


        } 

        public static bool UpdateUser(int PersonID,  string UserName,
            string Password,  bool IsActive, int UserID)
        {
            int RowsAffected = 0; 
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = @"update Users set
                            PersonID=@PersonID,
                            UserName=@UserName,
                            Password=@Password,
                            IsActive=@IsActive

                            Where UserID=@UserID";
            SqlCommand comand = new SqlCommand(query, connection);
             comand.Parameters.AddWithValue("@UserID", UserID);
             comand.Parameters.AddWithValue("@PersonID", PersonID);
             comand.Parameters.AddWithValue("@UserName", UserName);
             comand.Parameters.AddWithValue("@Password", Password);
             comand.Parameters.AddWithValue("@IsActive", IsActive);


            try
            {
                connection.Open();
                RowsAffected=comand.ExecuteNonQuery();



            }
            catch (Exception ex) { }finally { connection.Close(); }
            return RowsAffected>0;

        }


        public static bool GetUserByUserID(ref int PersonID,ref string UserName,
           ref string Password,ref bool IsActive, int UserID )
        {
            bool IsFound=false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = @"select * from Users Where UserID=@UserID";
            SqlCommand comand = new SqlCommand(query, connection);
            comand.Parameters.AddWithValue("@UserID", UserID);
            try
            {
                connection.Open();
                SqlDataReader Reader = comand.ExecuteReader();
                if(Reader.Read())
                {
                    IsFound=true;

                    PersonID = (int)Reader["PersonID"];
                    UserName = (string)Reader["UserName"];
                    Password = (string)Reader["Password"];
                    IsActive = (bool)Reader["IsActive"];

                }

                Reader.Close();



            }catch(Exception ex) 
            {
                IsFound = false;
            }
            finally { connection.Close(); }
          
            return IsFound;

        }


        public static bool GetUserByPersonID( int PersonID, ref string UserName,
        ref string Password, ref bool IsActive,ref  int UserID)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = @"select * from Users Where PersonID=@PersonID";
            SqlCommand comand = new SqlCommand(query, connection);
            comand.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                connection.Open();
                SqlDataReader Reader = comand.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;

                    UserID = (int)Reader["UserID"];
                    UserName = (string)Reader["UserName"];
                    Password = (string)Reader["Password"];
                    IsActive = (bool)Reader["IsActive"];

                }

                Reader.Close();



            }
            catch (Exception ex)
            {
                IsFound = false;
            }
            finally { connection.Close(); }

            return IsFound;

        }



        public static bool GetUserInfoByUserName(ref int PersonID,  string UserName,
          ref string Password, ref bool IsActive,ref int UserID)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = @"select * from Users Where UserName=@UserName";
            SqlCommand comand = new SqlCommand(query, connection);
            comand.Parameters.AddWithValue("@UserName", UserName);
            try
            {
                connection.Open();
                SqlDataReader Reader = comand.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;

                    PersonID = (int)Reader["PersonID"];
                    UserName = (string)Reader["UserName"];
                    Password = (string)Reader["Password"];
                    IsActive = (bool)Reader["IsActive"];
                    UserID = (int)Reader["UserID"];

                }

                Reader.Close();


            }
            catch (Exception ex) { IsFound = false; }
            finally { connection.Close(); }
            return IsFound;

        }

        public static bool GetUserInfoByUserNameAndPassword(string UserName,
            string Password, ref int PersonID, ref bool IsActive, ref int UserID)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string Query = @"
                Select * from Users Where UserName=@UserName And Password=@Password
                ";
            SqlCommand Command=new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@Password", Password);

            bool IsFound = false;

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                
                if(Reader.Read())
                {
                    IsFound=true;

                    PersonID = (int)Reader["PersonID"];
                    UserID = (int)Reader["UserID"];

                    IsActive = (bool)Reader["IsActive"];


                }

                Reader.Close();

            }
            catch(Exception Ex)
            {
                IsFound = false;    
            }
            finally
            {
                Connection.Close();
            }

           
            return IsFound;

        }



        public static bool DeleteUser(int UserID)
        {
            int RowsEfected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = @"DELETE  Users Where UserID=@UserID";
            SqlCommand comand = new SqlCommand(query, connection);
            comand.Parameters.AddWithValue("@UserID", UserID);
            try
            {
                connection.Open();

                RowsEfected=comand.ExecuteNonQuery();


            }
            catch(Exception ex) { }
            finally { connection.Close(); }


            return RowsEfected > 0;
            
        }

        public static bool IsThisUserExist(string UserName)
        {

            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = @"Select Found=1 from Users 
                              Where UserName=@UserName";


            SqlCommand Command = new SqlCommand(query, connection);

            Command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if(Reader.Read())
                {
                    IsFound = true;
                }

                Reader.Close();

            }catch(Exception ex) 
            {
                IsFound = false;
            }
            finally
            {
                connection.Close();
            }


            return IsFound;
        }
        public static bool IsThisUserExist(int UserID)
        {

            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string query = @"Select Found=1 from Users 
                              Where UserID=@UserID";


            SqlCommand Command = new SqlCommand(query, connection);

            Command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsFound = true;
                }

                Reader.Close();

            }
            catch (Exception ex)
            {
                IsFound = false;
            }
            finally
            {
                connection.Close();
            }


            return IsFound;
        }

        public static bool IsThisPerssonExistAsUser(int PersonID)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string query = @"Select Found=1 from Users 
                         Where PersonID=@PersonID";



            SqlCommand Command = new SqlCommand(query, connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsFound = true;
                }

                Reader.Close();

            }
            catch (Exception ex)
            {
                IsFound = false;
            }
            finally
            {
                connection.Close();
            }


            return IsFound;


        }

        public static bool ChangePassword(int UserID, string NewPassword)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string query = @"
                Update Users 
		                SET Password =@NewPassword Where UserID=@UserID";



            SqlCommand Command = new SqlCommand(query, connection);
            Command.Parameters.AddWithValue("@UserID", UserID);
            Command.Parameters.AddWithValue("@NewPassword", NewPassword);

            int RowsAffected =0;

            try
            {
                connection.Open();


                RowsAffected=Command.ExecuteNonQuery();


            }
            catch(Exception Ex)
            {
                RowsAffected = 0;
            }
            finally
            {
                connection.Close();

            }

            return RowsAffected > 0;
        }



    }


}
