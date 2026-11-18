using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccessLayerDVLD;

namespace BussnissLayerDVLD
{
    public   class clsDrivers
    {

        public int DriverID { get; set; }
        public int PersonID { get; set; }

        public clsPerson PersonInfo;
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        private enum enMode { Add = 0, Update = 1 }

        private enMode _Mode;

        public clsDrivers(int DriverID, int PersonID, int CreateByUser, DateTime CreatedTime)
        {

            this.DriverID = DriverID;
            this.PersonID = PersonID;

            this.PersonInfo = clsPerson.Find(PersonID);

            this.CreatedByUserID = CreateByUser;
            this.CreatedDate = CreatedTime;
            _Mode = enMode.Update;

        }

        public clsDrivers()
        {
            this.DriverID = -1;
            this.PersonID = -1;
            this.CreatedByUserID = -1;
            this.CreatedDate = DateTime.Now;
            _Mode = enMode.Add;
        }

        private bool _AddNewDriver()
        {
            this.DriverID = (clsDriverData.AddNewDriver(this.PersonID, this.CreatedByUserID));
            return this.DriverID > 0;
        }

        private bool _Update()
        {
            return clsDriverData.UpdateDriver(this.DriverID, this.PersonID, this.CreatedByUserID);
        }

        public bool Save()
        {

            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddNewDriver())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;


                case enMode.Update:
                    return _Update();


            }


            return false;
        }

        public static clsDrivers FindDriverInfoByDriverID(int DriverID)
        {
            int PersonID = -1, CreatedByUser = -1;
            DateTime CreatedDate = DateTime.Now;

            bool IsFound = clsDriverData.GetDriverInfoByDriverID(DriverID, ref PersonID, ref CreatedByUser, ref CreatedDate);


            if (IsFound)
                return new clsDrivers(DriverID, PersonID, CreatedByUser, CreatedDate);

            else
                return null;
        }

        public static clsDrivers FindDriverInfoByPersonID(int PersonID)
        {
            int DriverID = -1, CreatedByUser = -1;
            DateTime CreatedDate = DateTime.Now;

            bool IsFound = clsDriverData.GetDriverInfoByPersonID(ref DriverID, PersonID, ref CreatedByUser, ref CreatedDate);


            if (IsFound)
                return new clsDrivers(DriverID, PersonID, CreatedByUser, CreatedDate);

            else
                return null;
        }

        public static DataTable GetAllDrivers()
        {
            return clsDriverData.GetAllDrivers();
        }

        public static DataTable GetLocaldriverslicenses(int driverid)
        {
             return clsLicense.GetLocalicensesForDriverByDriverID(driverid);
        }

        public static DataTable GetInternationaldriverslicenses(int driverid)
        {
            return clsInternationalLicense.GetAllInternationalsLicensesByDriverID(driverid);
        }








    }

}
