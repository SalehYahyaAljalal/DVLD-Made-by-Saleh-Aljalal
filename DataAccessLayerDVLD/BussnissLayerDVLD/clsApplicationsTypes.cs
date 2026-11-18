using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayerDVLD;
using System.Data;



namespace BussnissLayerDVLD
{



    public  class clsApplicationsTypes
    {

        private enum enMode { Add=0,Update=1}
        private enMode _Mode;

        public int ApplicationTypeID            {get;set ;}
        public string ApplicationTypeTitle      {get;set ;}
        public float ApplicationTypeFees { get; set; }

        private clsApplicationsTypes(int ApplicationTypeID,string ApplicationTypeTitle,
            float ApplicationTypeFees)
        {
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeTitle=ApplicationTypeTitle;
            this.ApplicationTypeFees = ApplicationTypeFees;

            _Mode = enMode.Update;
        }

        public clsApplicationsTypes()
        {
            this.ApplicationTypeID = -1;
            this.ApplicationTypeTitle = "";
             this.ApplicationTypeFees = 0;

            _Mode = enMode.Add;
        }

        public static clsApplicationsTypes Find(int ApplicationTypeID)
        {
            string APplicationTypeTitle = "";
            float ApplicationTypeFees = 0;

            bool IsFound=false;

            IsFound = clsApplicationTypeData.FindApplicationTypeInfoByID(ApplicationTypeID,ref APplicationTypeTitle,
                ref ApplicationTypeFees);

            if(IsFound)
            {
                return new clsApplicationsTypes(ApplicationTypeID, APplicationTypeTitle,
                    ApplicationTypeFees
                    );
            }
            
            return null;

        }

        public static DataTable GellAllApplicationTypes()
        {
            return clsApplicationTypeData.GetAllApplicationTypes();
        }


        private bool AddNewApplicationType()
        {
            this.ApplicationTypeID = clsApplicationTypeData.AddNewApplicationType(this.ApplicationTypeTitle,
                this.ApplicationTypeFees);


            return this.ApplicationTypeID > 0;
        }

        private bool _Update()
        {
            return clsApplicationTypeData.UpdateApplicationType(this.ApplicationTypeID,
                this.ApplicationTypeTitle,this.ApplicationTypeFees);

        }

        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.Add:
                    if (AddNewApplicationType())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _Update();




            }

            return false;
        }



    }
}
