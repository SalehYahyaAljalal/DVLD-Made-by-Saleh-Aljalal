using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataAccessLayerDVLD;



namespace BussnissLayerDVLD
{
    public class clsInternationalLicense :clsApplications
    {
        public enum enMode { Add = 1, Update = 2 }
        public enMode Mode;
        public int InternationalLicenseID { set; get; }
        public int DriverID { set; get; }
        public int IssueingUsingLocalLicenseID { set; get; }
        public DateTime IssuedDate { set; get; }
        public DateTime ExpiraionDate { set; get; }
        public bool IsActive { set; get; }

        public clsUsers CreateByUserInfo;
        public clsLicense LicenseInfo;
        public clsDrivers DriverInfo;


        public clsInternationalLicense()
        {
            this.InternationalLicenseID = 0;
            this.ApplicationID = 0;
            this.DriverID = -1;
            this.IssueingUsingLocalLicenseID = -1;
            this.IssuedDate = DateTime.MinValue;
            this.ExpiraionDate = DateTime.Now;
            this.IsActive = false;
            this.CreatedByUserID = -1;


            Mode = enMode.Add;
        }



        private clsInternationalLicense(int InternationalLicenseID, int DriverID, int IssueingUsingLocalLicenseID,
            DateTime IssuedDate, DateTime ExpiraionDate, bool IsActive,
            int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
            clsApplications.enApplicationStatus ApplicationStatus, DateTime LastStatusDate, float PaidFees, int
            CreatedByUserID
            )
        {
            base.ApplicationID = ApplicationID;
            base.ApplicantPersonID = ApplicantPersonID;
            base.ApplicationDate = ApplicationDate;
            base.ApplicationTypeID =(int)clsApplications.enApplicationTypes.NewInternationalLicense;
            base.ApplicationStatus = ApplicationStatus;
            base.LastStatusDate = LastStatusDate;
            base.PaidFees = PaidFees;
            base.CreatedByUserID = CreatedByUserID;


            this.InternationalLicenseID = InternationalLicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.IssueingUsingLocalLicenseID = IssueingUsingLocalLicenseID;
            this.IssuedDate = IssuedDate;
            this.ExpiraionDate = ExpiraionDate;
            this.IsActive = IsActive;
            this.CreatedByUserID = CreatedByUserID;


            this.DriverInfo = clsDrivers.FindDriverInfoByDriverID(DriverID);
            this.CreateByUserInfo = clsUsers.FindByUserID(CreatedByUserID);
            this.LicenseInfo = clsLicense.FindByID(IssueingUsingLocalLicenseID);

            Mode = enMode.Update;

        }

        private bool _AddNewInternationalLicense()
        {
            this.InternationalLicenseID = clsinternationalLicenseData
         .AddNewInternationalLicense(this.ApplicationID, this.DriverID, this.IssueingUsingLocalLicenseID,
         this.IssuedDate, this.ExpiraionDate, this.CreatedByUserID);

            return this.InternationalLicenseID > 0;

        }

        private bool _UpdateInternationalLicense()
        {
            return clsinternationalLicenseData
          .UpdateInternationalLicense(this.InternationalLicenseID, this.ApplicationID, this.DriverID, this.IssueingUsingLocalLicenseID,
          this.IssuedDate, this.ExpiraionDate, this.IsActive, this.CreatedByUserID);


        }

        public bool Save()
        {
            base.Mode =(clsApplications.enMode)Mode;


            if (!base.Save())
                  return false;


            switch (Mode)
            {
                case enMode.Add:
                    if (_AddNewInternationalLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else return false;

                case enMode.Update:
                    return _UpdateInternationalLicense();


            }


            return false;
        }



        public static clsInternationalLicense FindByInternationalLicenseID(int InternationalLicenseID)
        {
            int DriverID = -1; int ApplicationID = -1;
            int IssueingUsingLocalLicenseID = -1;
            DateTime IssuedDate = DateTime.Now;
            DateTime ExpiraionDate = DateTime.Now;
            bool IsActive = false;
            int CreatedByUserID = 0;


            bool IsFound = clsinternationalLicenseData.
                GetInternationalLicenseByID( InternationalLicenseID, ref ApplicationID, ref DriverID, ref IssueingUsingLocalLicenseID,
                ref IssuedDate, ref ExpiraionDate,ref IsActive, ref CreatedByUserID);


            if (IsFound)
            {
                clsApplications NewApplication=clsApplications.FindBaseApplication(ApplicationID);


                return new clsInternationalLicense(InternationalLicenseID, DriverID, IssueingUsingLocalLicenseID,
                    IssuedDate, ExpiraionDate, IsActive, NewApplication.ApplicationID, NewApplication.ApplicantPersonID, 
                    NewApplication.ApplicationDate, NewApplication.ApplicationTypeID,
                     NewApplication.ApplicationStatus, NewApplication.LastStatusDate, 
                     NewApplication.PaidFees, NewApplication.CreatedByUserID);
            }
            else
                return null;


        }

       
        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            return clsinternationalLicenseData.GetActiveInternationalLicenseIDByDriverID(DriverID);
        }

        public static DataTable GetAllInternationalsLicenses()
        {
            return clsinternationalLicenseData.GetAllInternationalsLicenses();
        }

        public static DataTable GetAllInternationalsLicensesByDriverID(int DriverID)
        {
            return clsinternationalLicenseData.GetInternationalLicensesForDriver(DriverID);
        }


    }
}
