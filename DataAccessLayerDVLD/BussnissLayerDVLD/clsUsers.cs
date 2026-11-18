using System;
using System.Data;
using System.Data.SqlClient;
using DataAccessLayerDVLD;
namespace BussnissLayerDVLD
{
    public  class clsUsers
    {
        private enum enMode { add=1,update=2}
        private enMode _Mode;

        public int UserID { get; set; } 
        public int PersonID { get; set; }
        public string UserName { get; set; }

        public clsPerson PersonInfo;

        public string Password { get; set; }
        public bool IsActive { get; set; }  

        public clsUsers()
        {
            this.UserID = -1;
            this.PersonID=-1;
            this.UserName="";
            this.Password="";
            this.IsActive = false;

            _Mode = enMode.add;
        }
        public clsUsers(int UserID,int PersonID, string UserName, string Password, bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;

            this.PersonInfo = clsPerson.Find(PersonID);

            this.UserName = UserName;
            this.Password=Password;
            this.IsActive = IsActive; 
            

            _Mode=enMode.update;
        }


        public static DataTable GetAllUsers()
        {
            return clsUsersDataAccess.GetAllUsers();
        }

        private bool _AddNerUser()
        {
            this.UserID=clsUsersDataAccess.AddNewUser(this.PersonID,this.UserName,this.Password,this.IsActive);
            return this.UserID>0;

        }
        private bool _UpdateUser()
        {
            return clsUsersDataAccess.UpdateUser(this.PersonID, this.UserName, this.Password, this.IsActive, this.UserID);
        }

        public static clsUsers FindByUserID(int UserID)
        {
            int PersonID = -1;
            string UserName="",Password = "";
            bool IsActive = false;

            bool IsFound = false;

            IsFound = clsUsersDataAccess.GetUserByUserID(ref PersonID, ref UserName, ref Password, ref IsActive, UserID);
            

            if(IsFound)
            {
                return new clsUsers(UserID, PersonID, UserName, Password, IsActive);
            }

            else
            {
                return null;
            }

        }
        public static clsUsers FindByUserName(string UserName)
        {
            int PersonID = -1;
            int UserID=-1;
            string  Password = "";
            bool IsActive = false;
            bool IsFound = false;


            IsFound = clsUsersDataAccess.GetUserInfoByUserName(ref PersonID, UserName, ref Password, ref IsActive, ref UserID);
            

            if (IsFound)
            {
                return new clsUsers(UserID, PersonID, UserName, Password, IsActive);
            }

            else
            {
                return null;
            }

        }
        public static clsUsers FindUserByPersonID(int  PersonID)
        {
            int UserID = -1;
            string Password = "", UserName="";
            bool IsActive = false;
            bool IsFound = false;


            IsFound = clsUsersDataAccess.GetUserByPersonID(PersonID, ref UserName, ref Password, ref IsActive, ref UserID);
            

            if (IsFound)
            {
                return new clsUsers(UserID, PersonID, UserName, Password, IsActive);
            }

            else
            {
                return null;
            }

        }

        public static clsUsers FindUserByUserNameAndPassword(string UserName,string Password)
        {
            int UserID = -1;
            int PersonID = -1;
            bool IsActive = false;


            bool IsFound = false;



            IsFound = clsUsersDataAccess.GetUserInfoByUserNameAndPassword(UserName,  Password,ref PersonID, ref IsActive, ref UserID);


            if (IsFound)
            {
                return new clsUsers(UserID, PersonID, UserName, Password, IsActive);
            }

            else
            {
                return null;
            }



        }




        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.add:
                    if (_AddNerUser())
                    {
                        _Mode = enMode.update;
                        return true;
                    }else
                        return false;
                       

                case enMode.update:
                   return  _UpdateUser();
                 
            }

            return false;
                
        }


        public static bool IsThisUserExist(string UserName)
        {
            return clsUsersDataAccess.IsThisUserExist(UserName);
        }
        public static bool IsThisUserExist(int  UserID)
        {
            return clsUsersDataAccess.IsThisUserExist(UserID);
        }

        public static bool IsThisPersonExistsAsUser (int PersonID)
        {
            return clsUsersDataAccess.IsThisPerssonExistAsUser(PersonID);
        }

        public static bool DeleteUser(int UserID)
        {
            return clsUsersDataAccess.DeleteUser(UserID);   

        }


    }
}
