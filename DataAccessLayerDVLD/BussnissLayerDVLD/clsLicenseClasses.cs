using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccessLayerDVLD;

using System.Data;

namespace BussnissLayerDVLD
{
    public  class clsLicenseClasses
    {
        private enum enMode { Add = 0, Update = 1 }
        private enMode _Mode;
        public int LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassDecsription { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte DefaultValidityLength { get; set; }
        public float ClassFees { get; set; }


        private clsLicenseClasses()
        {
            this.LicenseClassID = -1;
            this.ClassName = "";
            this.ClassDecsription = "";
            this.MinimumAllowedAge = 0;
            this.DefaultValidityLength = 0;
            this.ClassFees = 0;

            _Mode = enMode.Add;
        }
        private clsLicenseClasses(int LicensesClassID, string ClassName, string ClassDecription,
            byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFess)
        {
            this.LicenseClassID = LicensesClassID;
            this.ClassName = ClassName;
            this.ClassDecsription = ClassDecription;
            this.MinimumAllowedAge = MinimumAllowedAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.ClassFees = ClassFess;

            _Mode = enMode.Update;
        }


        public static clsLicenseClasses FindLicenseClassInfoByClassName(string ClassName)
        {
            int LicensesClassID = -1;
            byte MinimumAllowedAge = 0, DefaultValidityLength = 0;
            string ClassDecsription = "";
            float ClassFess = 0;

            if (clsLicenseClassData.GetLicenseClassInfoByClassName(ref LicensesClassID, ClassName, ref ClassDecsription,
                ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFess))
                return new clsLicenseClasses(LicensesClassID, ClassName, ClassDecsription,
                    MinimumAllowedAge, DefaultValidityLength, ClassFess);

            else
                return null;
        }


        private bool _AddNewLicenseClass()
        {
            this.LicenseClassID = clsLicenseClassData.AddNewLicenseClass(this.ClassName, this.ClassDecsription, this.MinimumAllowedAge,
                 this.DefaultValidityLength, this.ClassFees);

            return this.LicenseClassID > 0;
        }



        private bool _UpdateLicenseClass()
        {
            return clsLicenseClassData.UpdateLicenseClass(this.LicenseClassID, this.ClassName, this.ClassDecsription,
                this.MinimumAllowedAge, this.DefaultValidityLength, this.ClassFees);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.Add:
                    if (_AddNewLicenseClass())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdateLicenseClass();


            }

            return false;
        }

        public static clsLicenseClasses FindLicenseClassInfoByID(int LicensesClassID)
        {
            string ClassName = "";
            byte MinimumAllowedAge = 0, DefaultValidityLength = 0;
            string ClassDecsription = "";
            float ClassFess = 0;

            if (clsLicenseClassData.GetLicenseClassInfoByID(LicensesClassID, ref ClassName, ref ClassDecsription,
                ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFess))
                return new clsLicenseClasses(LicensesClassID, ClassName, ClassDecsription,
                    MinimumAllowedAge, DefaultValidityLength, ClassFess);

            else
                return null;

        }




        public static DataTable GetAllLicensesClasses()
        {
            return clsLicenseClassData.GetAllLicenseClass();
        }




    }
}
