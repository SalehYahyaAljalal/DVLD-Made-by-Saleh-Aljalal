using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using DataAccessLayerDVLD;

namespace BussnissLayerDVLD
{
    public  class clsDetainLicense
    {
        public enum enMode {Add=1,Update=2 }
        public enMode Mode;

       public int DetainID            {set;get;}
       public int LicenseID           {set;get;}
       public DateTime DetainDate     {set;get;}
       public float FineFees          {set;get;}
       public int CreatedByUserID     {set;get;}
       public bool IsReleased         {set;get;}
       public DateTime ReleaseDate    {set;get;}  
       public int ReleasedByUserID    {set;get;}   
       public int ReleaseApplicationID { set; get; }

        public clsUsers DetaindByUserInfo;
        public clsUsers ReleasedByUserInfo;

        public clsDetainLicense()
        {
            this.DetainID = 0;
            this.LicenseID = 0;
            this.DetainDate = DateTime.Now;
            this.FineFees = 0;
            this.CreatedByUserID = 0;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.Now;
            this.ReleasedByUserID = 0;
            this.ReleaseApplicationID = 0;


            this.Mode = enMode.Add;
        }

        public static bool IsThisLicenseDetained(int LicenceID)
        {
             return clsDetainLicenseData.IsDetain(LicenceID);
        }

        
        private  clsDetainLicense( int DetainID, int LicenseID,  DateTime DetainDate,  float FineFees,
         int CreatedByUserID,  bool IsReleased,  DateTime ReleaseDate,  int ReleasedByUserID
            ,  int ReleaseApplicationID)
        {
            this.DetainID = DetainID;
            this.LicenseID = LicenseID;
            this.DetainDate = DetainDate;
            this.FineFees = FineFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsReleased = IsReleased;
            this.ReleaseDate = ReleaseDate;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleaseApplicationID = ReleaseApplicationID;

            this.DetaindByUserInfo=clsUsers.FindByUserID(CreatedByUserID);
            this.ReleasedByUserInfo = clsUsers.FindByUserID(ReleasedByUserID);


            this.Mode = enMode.Update;
        }

        private bool _AddNewDetainLicense()
        {
            this.DetainID = clsDetainLicenseData.DetainNewLicense(this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID,
                this.IsReleased);

            return true;
        }
        private bool _UpdateDetainedLicense()
        {
            return clsDetainLicenseData.UpdateDetainedLicense(this.DetainID,this.LicenseID,this.DetainDate,this.FineFees,
                this.CreatedByUserID);
        }
      

        public  bool ReleaseDetainedLicense(int ReleaseByUserID,int ReleasedAPplicationID)
        {

             return   clsDetainLicenseData.ReleaseLicense(this.LicenseID, ReleaseByUserID, ReleasedAPplicationID);

        }



        public bool Save()
        {



            switch (Mode)
            {
                case enMode.Add:
                    if (_AddNewDetainLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else return false;

                case enMode.Update:
                    return _UpdateDetainedLicense();


            }

            return false;


        }



        public static clsDetainLicense FindDetainLicenseByLicenseID(int LicenseID)
        {
            int DetainID = -1;  DateTime DetainDate=DateTime.Now; ;
            float FineFees = 0;
            int CreatedByUserID = -1;  bool IsReleased = false; DateTime ReleaseDate = DateTime.Now;
            int ReleasedByUserID = -1;
              int ReleaseApplicationID = -1;


            bool IsFind = clsDetainLicenseData.GetDetainedLicenseByLicenseID(ref DetainID, LicenseID, ref DetainDate,
                ref FineFees, ref CreatedByUserID, ref IsReleased, ref ReleaseDate, ref ReleasedByUserID
            , ref ReleaseApplicationID);


            if (IsFind)
            {
                return new clsDetainLicense(DetainID, LicenseID, DetainDate,
                 FineFees, CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID
            , ReleaseApplicationID);


            }
            else
                return null;


        }


        public static clsDetainLicense FindDetainLicenseByDetainID(int DetainID)
        {
            int LicenseID = -1; DateTime DetainDate = DateTime.Now; ;
            float FineFees = 0;
            int CreatedByUserID = -1; bool IsReleased = false; DateTime ReleaseDate = DateTime.Now;
            int ReleasedByUserID = -1;
            int ReleaseApplicationID = -1;


            bool IsFind = clsDetainLicenseData.GetDetainedLicenseByDetainID( DetainID,ref  LicenseID, ref DetainDate,
                ref FineFees, ref CreatedByUserID, ref IsReleased, ref ReleaseDate, ref ReleasedByUserID
            , ref ReleaseApplicationID);


            if (IsFind)
            {
                return new clsDetainLicense(DetainID, LicenseID, DetainDate,
                 FineFees, CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID
            , ReleaseApplicationID);


            }
            else
                return null;

        }

        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainLicenseData.GetAllDetaindLicenses();
        }



    }


}
