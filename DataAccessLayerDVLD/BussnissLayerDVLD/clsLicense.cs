using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


using DataAccessLayerDVLD;

namespace BussnissLayerDVLD
{
    public  class clsLicense
    {

        private enum enMode { Add = 0, Update = 1 }
        private enMode _Mode;
        public enum enIssueReason { FirstTime = 1, ReNew = 2, DamagedReplacement = 3, LostReplacement = 4 }

        public int LicesnseID { get; set; }
        public int ApplicationID { get; set; }

        public int DriverID { get; set; }
        public clsDrivers Driverinfo;
        public int LicenseClassID { get; set; }
        public clsLicenseClasses LiceneseClassesInfo;

        public clsDetainLicense DetainLicenseInfo;
        public bool IsDetained
        {
            get
            {
                return clsDetainLicense.IsThisLicenseDetained(this.LicesnseID);
            }
        }


        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public float PaisFees { get; set; }
        public bool IsActive { get; set; }
        public enIssueReason IssueReason { get; set; }
        public int CreatedByUserID;
        public clsUsers CreatedByUserInfo;

        public string IssueText
        {
            get
            {
                switch(IssueReason)
                {
                    case enIssueReason.FirstTime:
                        return "First Time ";
                    case enIssueReason.ReNew:
                        return "Renew";
                    case enIssueReason.DamagedReplacement:
                        return "Damaged Replacement ";

                    case enIssueReason.LostReplacement:
                        return "Lost Replacement ";

                        default: return "First Time";
                }

            }

        }

        public clsLicense()
        {
            this.LicesnseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.IsActive = false;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.LicenseClassID = -1;
            this.Notes = "";
            this.PaisFees = 0;
            this.IssueReason = enIssueReason.FirstTime;
            this.CreatedByUserID = -1;

            _Mode = enMode.Add;
        }

        private clsLicense (int LicenseID,  int ApplicationID,  int DriverID,  int LicenseClass,
            DateTime IssuedDate,  DateTime ExpirationDate,
            string Notes,  float PaisFees,  bool IsActive,
            enIssueReason IssuedReason,  int CreatedByUserID)
        {
            this.LicesnseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.IsActive = IsActive;
            this.IssueDate = IssuedDate;
            this.ExpirationDate =ExpirationDate;
            this.LicenseClassID = LicenseClass;
            this.Notes = Notes;
            this.PaisFees = PaisFees;
            this.IssueReason = IssuedReason;
            this.CreatedByUserID =CreatedByUserID;


            this.Driverinfo=clsDrivers.FindDriverInfoByDriverID(DriverID);
            this.LiceneseClassesInfo=clsLicenseClasses.FindLicenseClassInfoByID(LicenseClassID);
            this.CreatedByUserInfo = clsUsers.FindByUserID(CreatedByUserID);
            this.DetainLicenseInfo=clsDetainLicense.FindDetainLicenseByLicenseID(LicenseID);    

            _Mode = enMode.Update;
        }

        private bool _AddNewLicense()
        {
            this.LicesnseID = clsLicenseData.AddNewLicense(this.ApplicationID, this.DriverID, this.LicenseClassID, this.IssueDate,
                this.ExpirationDate, this.Notes, this.PaisFees, this.IsActive,(byte) this.IssueReason, this.CreatedByUserID);

            return this.LicesnseID > -1;
        }

        private bool _UpdateLicense()
        {
            return clsLicenseData.UpdateLicense(this.LicesnseID, this.ApplicationID, this.DriverID, this.LicenseClassID, this.IssueDate,
            this.ExpirationDate, this.Notes, this.PaisFees, this.IsActive, (byte)this.IssueReason, this.CreatedByUserID); ;

            
        }


        public bool Save()
        {

            switch(_Mode)
            {
                case enMode.Add:
                    if(_AddNewLicense())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateLicense();


            }

            return false;
        }


        public static clsLicense FindByID(int LicenseID)
        {
            int ApplicationID = -1;
            int DriverID = -1;
            int LicenseClassID = -1; DateTime IssueDate = DateTime.Now;
            DateTime ExpirationDate = DateTime.Now; string Notes = "";
            float PaisFees = 0;
            bool IsActive = false;
            byte IssueReason = 0;
            int CreatedByUserID = -1;


            bool IsFound = clsLicenseData.GetLicenseByID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClassID,
                ref IssueDate, ref ExpirationDate, ref Notes, ref PaisFees, ref IsActive, ref IssueReason,
                ref CreatedByUserID);


            if (IsFound)
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClassID,
                 IssueDate, ExpirationDate, Notes, PaisFees, IsActive, (clsLicense.enIssueReason)IssueReason,
                 CreatedByUserID);

            }
            else
                return null;


        }



        public  bool IsThisLicenseExpired()
        {
            return DateTime.Now >= this.ExpirationDate;
            
        }

        public bool DeactiveLicense()
        {
            return clsLicenseData.DeactiveLicense(this.LicesnseID);
        }

        public static  bool DeactiveLicense(int LicenseID)
        {
            return clsLicenseData.DeactiveLicense(LicenseID);
        }

        public static DataTable GetLocalicensesForDriverByDriverID(int DriverID)
        {
           return clsLicenseData.GetDriverLicenses(DriverID);

        }

        public static DataTable GetAllLicenses()
        {
            return clsLicenseData.GetAllLicenses();
        }


        public clsLicense RenewLicense(string Notes,int CreatedByUserID)
        {

            clsApplications NewApplication = new clsApplications();
            NewApplication.ApplicantPersonID = this.Driverinfo.PersonID;
            NewApplication.ApplicationDate = DateTime.Now;
            NewApplication.ApplicationTypeID = clsApplicationsTypes.Find(
                (int)clsApplications.enApplicationTypes.RenewDrivingLicenseService).ApplicationTypeID;

            NewApplication.ApplicationStatus = clsApplications.enApplicationStatus.Compeleted;
            NewApplication.LastStatusDate= DateTime.Now;
            NewApplication.PaidFees = clsApplicationsTypes.Find((int)clsApplications.enApplicationTypes.RenewDrivingLicenseService)
                .ApplicationTypeFees;
            NewApplication.CreatedByUserID = CreatedByUserID;

            if(!NewApplication.Save())
            {
                return null;
            }


            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = NewApplication.ApplicationID;
            NewLicense.DriverID=this.DriverID;
            NewLicense.LicenseClassID=this.LicenseClassID;
            NewLicense.LicesnseID=this.LicesnseID;
            NewLicense.IssueDate = DateTime.Now;

            NewLicense.ExpirationDate=DateTime.Now.AddYears(
                this.LiceneseClassesInfo.DefaultValidityLength);

            NewLicense.Notes = Notes;
            NewLicense.PaisFees = clsApplicationsTypes.Find((int)clsApplications.enApplicationTypes.RenewDrivingLicenseService).ApplicationTypeFees;
            NewLicense.IsActive=true;   
            NewLicense.IssueReason=clsLicense.enIssueReason.ReNew;
            NewLicense.CreatedByUserID=CreatedByUserID;

            
            if(!NewLicense.Save())
            {
                return null;
            }

            DeactiveLicense();

          return  NewLicense;



        }

        public clsLicense ReplacementForLostOrDamaged( int CreatedByUserID,clsLicense.enIssueReason IssueReason)
        {

            clsApplications NewApplication = new clsApplications();
            NewApplication.ApplicantPersonID = this.Driverinfo.PersonID;
            NewApplication.ApplicationDate = DateTime.Now;
            NewApplication.ApplicationTypeID = clsApplicationsTypes.Find(
                (int)clsApplications.enApplicationTypes.RenewDrivingLicenseService).ApplicationTypeID;

            NewApplication.ApplicationStatus = clsApplications.enApplicationStatus.Compeleted;
            NewApplication.LastStatusDate = DateTime.Now;
            NewApplication.PaidFees = clsApplicationsTypes.Find((int)clsApplications.enApplicationTypes.RenewDrivingLicenseService)
                .ApplicationTypeFees;
            NewApplication.CreatedByUserID = CreatedByUserID;

            if (!NewApplication.Save())
            {
                return null;
            }


            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = NewApplication.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClassID = this.LicenseClassID;
            NewLicense.IssueDate = DateTime.Now;



            NewLicense.ExpirationDate = DateTime.Now.AddYears(
                this.LiceneseClassesInfo.DefaultValidityLength);

            NewLicense.Notes = this.Notes;
            NewLicense.PaisFees = clsApplicationsTypes.Find((int)clsApplications.enApplicationTypes.RenewDrivingLicenseService).ApplicationTypeFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = IssueReason;
            NewLicense.CreatedByUserID = CreatedByUserID;


            if (!NewLicense.Save())
            {
                return null;  ;
            }


            DeactiveLicense();

            return NewLicense;

        }

        public int DetainLicense(float FineFees,int CreatedByUserID)
        {
            clsDetainLicense DetainNewLicense = new clsDetainLicense();

            DetainNewLicense.LicenseID = this.LicesnseID;
            DetainNewLicense.DetainDate = DateTime.Now;
            DetainNewLicense.FineFees = FineFees;
            DetainNewLicense.CreatedByUserID = CreatedByUserID;

            if (!DetainNewLicense.Save())
                return -1;


            else
                return DetainNewLicense.LicenseID;


        }

      public bool  ReleaseLicense(int ReleasedByUserID,ref int ReleaseApplicationID)
      {
            clsApplications NewAPplication = new clsApplications();

            NewAPplication.ApplicantPersonID = this.Driverinfo.PersonID;
            NewAPplication.ApplicationDate = DateTime.Now;
            NewAPplication.ApplicationTypeID=(int)clsApplications.enApplicationTypes.ReleaseDetainedDrivingLicsense;

            NewAPplication.ApplicationStatus = clsApplications.enApplicationStatus.Compeleted;
            NewAPplication.LastStatusDate=DateTime.Now;
          
            NewAPplication.PaidFees= clsApplicationsTypes.
                Find((int)clsApplications.enApplicationTypes.ReleaseDetainedDrivingLicsense).ApplicationTypeFees;

            NewAPplication.CreatedByUserID = ReleasedByUserID;

            if(!NewAPplication.Save())
            {
                ReleaseApplicationID = -1;
                return false;
            }

            ReleaseApplicationID=  NewAPplication.ApplicationID;


            bool IsRelease= this.DetainLicenseInfo.ReleaseDetainedLicense(ReleasedByUserID,ReleaseApplicationID);

            return IsRelease;

        }




        public static bool IsThereAnActiveLicensePerPersonID(int PersonID, int LicenseClassID)
        {

            return clsLicenseData.IsThereAnActiveLicensePerPersonID(PersonID, LicenseClassID);

        }
        public  bool IsThereAnActiveLicensePerPersonID(int PersonID)
        {
       
            return clsLicenseData.IsThereAnActiveLicensePerPersonID(PersonID, this.LicenseClassID);
       
        }


        public  int GetAnActiveLicenseID(int PersonID)
        {

            return clsLicenseData.GetActiveLicensePerPersonID(PersonID, this.LicenseClassID);

        }
        public static int GetAnActiveLicenseID(int PersonID ,int LicenseClassID)
        {

            return clsLicenseData.GetActiveLicensePerPersonID(PersonID, LicenseClassID);

        }





    }
}
