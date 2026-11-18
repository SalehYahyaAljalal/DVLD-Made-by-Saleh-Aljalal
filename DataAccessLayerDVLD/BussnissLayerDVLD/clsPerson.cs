using System;
using System.Data;
using System.Data.SqlClient;
using DataAccessLayerDVLD;

namespace BussnissLayerDVLD
{
    public class clsPerson
    {
        enum enMode { AddNew = 0, UpdateNew = 1 }
        enMode _Mode;
        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte Gendor { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public byte NationalityCountryID { get; set; }

        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.SecondName +
                    " " + this.ThirdName + " " + this.LastName;
            }
        }

        private string _ImagePath;

        public clsCountries CountryInfo;


        public string ImagePath
        {
            set { _ImagePath = value; } 
            get { return _ImagePath; }
        }

        private clsPerson(int personID, string nationalNo, string firstName, string secondName, string thirdName, string lastName, DateTime dateOfBirth, byte gendor, string address, string phone, string email, byte nationalityCountryID, string imagePath)
        {
            this.PersonID=personID;
            this.NationalNo=nationalNo;
            this.FirstName=firstName;
            this.SecondName=secondName;
            this.ThirdName=thirdName;
            this.LastName=lastName;
            this.DateOfBirth=dateOfBirth;
            this.Gendor=gendor;
            this.Address=address;
            this.Phone=phone;
            this.Email=email;

            this.CountryInfo=clsCountries.Find(nationalityCountryID);
            
            this.ImagePath=imagePath;
            _Mode=enMode.UpdateNew;
        }
        public clsPerson()
        {

          this. PersonID=-1;
          this. NationalNo="";
          this. FirstName="";
          this. SecondName="";
          this. ThirdName="";
          this. LastName="";
          this. DateOfBirth=DateTime.Now;
          this. Gendor=0;
          this. Address="";
          this. Phone="";
          this. Email="";
          this. NationalityCountryID=0;
          this. ImagePath="";

            _Mode=enMode.AddNew;
        }



        private bool _AddNewPerson()
        {

         

            this.PersonID=clsPersonData.AddNewPerson(this.NationalNo, this.FirstName, this.SecondName,
                                                     this.ThirdName, this.LastName,
                                                     this.DateOfBirth, this.Gendor, this.Address, this.Phone,
                                                     this.Email, this.NationalityCountryID,
                                                     this.ImagePath);
            return this.PersonID!=-1;




        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        _Mode=enMode.UpdateNew;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.UpdateNew:
                    return _Update();


            }
            return false;
        }

        public static DataTable AllPeople()
        {
            return clsPersonData.GetAllPeople();
        }

    
        
        public static clsPerson Find(int PersonID)
        {
            string NationalNo = "",
                     FirstName = "",
                     SecondName = "",
                     ThirdName = "",
                     LastName = "",
                     Address = "",
                     Phone = "",
                     Email = "",
                     ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            byte Gendor = 0; 
            byte NationalityCountryID = 0;
            bool IsFound = false;

            IsFound=clsPersonData.FindPersonByID(PersonID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName,
                       ref LastName,
                        ref DateOfBirth, ref Gendor, ref Address,
                       ref Phone, ref Email, ref NationalityCountryID, ref ImagePath);




            if (IsFound)
            {
                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName,
                            LastName,
                            DateOfBirth, Gendor, Address,
                            Phone, Email, NationalityCountryID, ImagePath);

            }
            else
            {
                return null;

            }

        }
        public static clsPerson Find(string NationalNumber)
        {
            int PersonID = -1;

            string FirstName = "",
                    SecondName = "",
                    ThirdName = "",
                    LastName = "",
                    Address = "",
                    Phone = "",
                    Email = "",
                    ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            byte Gendor = 0;
            byte NationalityCountryID = 0;
            bool IsFound = false;

            IsFound= clsPersonData.FindPersonByNational(ref PersonID, NationalNumber, ref FirstName, ref SecondName, ref ThirdName,
                                                        ref LastName,
                                                        ref DateOfBirth, ref Gendor, ref Address,
                                                        ref Phone, ref Email, ref NationalityCountryID, ref ImagePath);

            if(IsFound)
            {
                return new clsPerson(PersonID, NationalNumber, FirstName, SecondName, ThirdName,
                                    LastName,
                                    DateOfBirth, Gendor, Address,
                                    Phone, Email, NationalityCountryID, ImagePath);
            }
            else
            {
                return null;

            }

        }



        private bool _Update()
        {
            return clsPersonData.UpdatePersone(this.PersonID, this.NationalNo, this.FirstName, this.SecondName, this.ThirdName, this.LastName, this.DateOfBirth
                , this.Gendor, this.Address, this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);

        }


        public static bool  DeletePerson(int PersonID)
        {
            return clsPersonData.DeletePerson(PersonID);
        }

        public static bool IsPersonExist(int PersonID)
        {
            return clsPersonData.IsPersonExist(PersonID);
        }

        public static bool IsPersonExist(string NationalNo)
        {
            return clsPersonData.IsPersonExist(NationalNo);
        }

    }
}
