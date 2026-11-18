using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using DataAccessLayerDVLD;
using DVLDProject;

namespace BussnissLayerDVLD
{
    public  class clsCountries
    {

        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public static DataTable AllCountries()
        {
            return clsCountriesDataAccess.GetAllCountries();

        }

        public clsCountries(int CountryID, string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName=CountryName;
        }

        public static clsCountries Find(string CountryName)
        {
            int CountryID = -1;
            if(clsCountriesDataAccess.Find(CountryName,ref CountryID))
            {
                return new clsCountries(CountryID,CountryName);
            }
            else
            {
                return null;
            }
        }

        public static clsCountries Find(int COuntryID)
        {
            string  CountryName = "";
            if (clsCountriesDataAccess.FindByCountryID(ref CountryName,  COuntryID))
            {
                return new clsCountries(COuntryID, CountryName);
            }
            else
            {
                return null;
            }
        }

    }
}
