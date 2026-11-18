using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccessLayerDVLD;

namespace BussnissLayerDVLD
{
    public  class clsApplications
    {

            
        public enum enMode { Add=1,Update=2}
        public enMode Mode;

        public enum enApplicationTypes {
         NewLocalDrivingLicenseService=1,
        RenewDrivingLicenseService=2,ReplacementforaLostDrivingLicense=3,ReplacementforaDamagedDrivingLicense=4,
        ReleaseDetainedDrivingLicsense=5,
        NewInternationalLicense=6,
        RetakeTest=8
                }

        public enum enApplicationStatus { New=1,Cancell=2,Compeleted=3}


        public string StatusText
        {
            get
            {
                switch(this.ApplicationStatus)
                {
                    case enApplicationStatus.New:
                        return "New";
                    case enApplicationStatus.Cancell:
                        return "Cancell";
                    case enApplicationStatus.Compeleted:
                        return "Compeleted";
                   default:
                        return "UnKnown";
                        

                }



            }
        }
         public  int ApplicationID               {get;set;}
         public  int ApplicantPersonID           {get;set;}
        public string ApplicantFullName
        {
            get
            {

                return clsPerson.Find(this.ApplicantPersonID).FullName;
            }
        }
         public clsPerson PersonInfo;
         public  DateTime ApplicationDate        {get;set;}
         public  int ApplicationTypeID           {get;set;}
        public clsApplicationsTypes ApplicationTypeInfo;
         public enApplicationStatus ApplicationStatus   {get;set;}
         public  DateTime LastStatusDate         {get;set;}
         public  float PaidFees                  { get; set; }
         public  int CreatedByUserID             { get; set; }
         public clsUsers CreatedByUserInfo;



        public  clsApplications()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID= -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = enApplicationStatus.New;
            this.LastStatusDate=DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;

            Mode=enMode.Add;
        }

        public clsApplications(int ApplicationID,int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
             enApplicationStatus ApplicationStatus, DateTime LastStatusDate, float PaidFees, int
            CreatedByUserID)
        {

            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;

            this.ApplicationTypeInfo = clsApplicationsTypes.Find(ApplicationTypeID);
            this.CreatedByUserInfo = clsUsers.FindByUserID(CreatedByUserID);
            this.PersonInfo = clsPerson.Find(ApplicantPersonID);
            

            Mode = enMode.Update;
        }

        private bool _AddNewApplication()
        {
            this.ApplicationID = clsApplicationDataAccess.AddNewApplication(this.ApplicantPersonID,
                this.ApplicationDate,this.ApplicationTypeID,(byte)this.ApplicationStatus,this.LastStatusDate,
                this.PaidFees,this.CreatedByUserID);

            return this.ApplicationID > 0;
        }

        private bool _UpdateApplication()
        {
            return clsApplicationDataAccess.UpdateApplicaion(this.ApplicationID,this.ApplicantPersonID,
                this.ApplicationDate, this.ApplicationTypeID, (byte)this.ApplicationStatus, this.LastStatusDate,
                this.PaidFees, this.CreatedByUserID);
        }


        public static clsApplications FindBaseApplication(int ApplicationID)
        {
            int ApplicantPersonID=-1;DateTime APpplicationDate=DateTime.Now;
            DateTime LastDate=DateTime.Now;byte ApplicationStatus = 0;
            int applicationTypID = -1;float paidFees = 0;int CreatedByUserID = 0;


            bool IsFound = clsApplicationDataAccess.GetApplicationInfoByID(ApplicationID,
                ref ApplicantPersonID,ref APpplicationDate,ref applicationTypID,ref ApplicationStatus,
              ref LastDate,ref paidFees,ref CreatedByUserID);


            if (IsFound)
            {
                return new clsApplications(ApplicationID,
                 ApplicantPersonID, APpplicationDate, applicationTypID, (enApplicationStatus)ApplicationStatus,
               LastDate, paidFees, CreatedByUserID);

            }
            else
                return null;

        }

        public bool Cancell()
        {
            return clsApplicationDataAccess.UpdateApplicaionStatus(this.ApplicationID, 2);
        }

        public bool SetComplete()
        {
            return clsApplicationDataAccess.UpdateApplicaionStatus(this.ApplicationID, 3);
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.Add:
                    if (_AddNewApplication())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdateApplication();

              
                   
            }

            return false;
        }

        public bool Delete()
        {
            return clsApplicationDataAccess.DeleteApplication(this.ApplicationID);
        }

        public static bool IsApplicationExists(int ApplicationID)
        {
            return clsApplicationDataAccess.IsApplicationExists(ApplicationID);
        }

        public static bool DoesPersonHaveActiveApplication(int APplicationID,int ApplicationTypeID)
        {
            return clsApplicationDataAccess.DoesPersonHaveActiveApplicationID(APplicationID, ApplicationTypeID);
        }

        public  bool DoesPersonHaveActiveApplication(int ApplicationTypeID)
        {
            return clsApplicationDataAccess.DoesPersonHaveActiveApplicationID(this.ApplicantPersonID, ApplicationTypeID);
        }


        public static int GetActiveApplicationID(int PersonID,clsApplications.enApplicationTypes ApplicationTypeID)
        {
            return clsApplicationDataAccess.GetActiveApplicationID(PersonID, (int)ApplicationTypeID);
        }
        public  int GetActiveApplicationID( clsApplications.enApplicationTypes ApplicationTypeID)
        {
            return clsApplicationDataAccess.GetActiveApplicationID(this.ApplicantPersonID, (int)ApplicationTypeID);
        }

        public  static int GetActiveApplicationIDForLicenseClass(int PersonID,clsApplications.enApplicationTypes ApplicationTypeID,
           int LicenseClassID )
        {
            return clsApplicationDataAccess.GetActiveApplicationIDForLicenseClass(PersonID, (int)ApplicationTypeID, LicenseClassID);
        }



    }
}
