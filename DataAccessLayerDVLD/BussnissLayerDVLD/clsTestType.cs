using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using BussnissLayerDVLD;
using DataAccessLayerDVLD;

namespace BussnissLayerDVLD
{
    public  class clsTestType
    {

       private enum enMode { Add=1,Update=2 }
       private enMode _Mode;


      public enum enTestTyp {VisionTest=1,WrittenTest=2,StreetTest=3 }
      public clsTestType.enTestTyp ID { get; set; }

      public  string TestTitle      {get;set;}
      public  string Description    {get;set;}
      public float TestTypeFees    { get; set; }

        
      public clsTestType ()
      {
            this.ID = clsTestType.enTestTyp.VisionTest;
            this.TestTitle = "";
            this.Description = "";
            this.TestTypeFees = 0;
        
            _Mode=enMode.Add;

      }

     private clsTestType(clsTestType.enTestTyp TestTypeID, string TestTitle,
            string Description, float TestTypeFees)
     {

            this.ID = TestTypeID;
            this.TestTitle = TestTitle;
            this.Description = Description;
            this.TestTypeFees = TestTypeFees;
            _Mode = enMode.Update;

     }

    public static DataTable GetAllTestTypes()
    {
           return clsTestTypeDataAccess.GetAllTestTypes();
    }

   
    private bool UpdateTestType()
    {
            return clsTestTypeDataAccess
                .UpdateTestType((int)this.ID,this.TestTitle,this.Description,this.TestTypeFees);     

    }

        public bool Save()
        {
            switch (_Mode)
            { 
                case enMode.Add:
                    break;

                case enMode.Update:
                    return UpdateTestType();



            }



            return false;


        }


        public static clsTestType Find(int TestTypeID)
    {
            string testTypeTitle = "", TestTypeDescription = "";
            float testTypeFees = 0;

            bool IsFound = clsTestTypeDataAccess.FindTestTypeByID(TestTypeID,ref testTypeTitle,ref
                TestTypeDescription,ref testTypeFees);

            if (IsFound)
            {
                return new clsTestType((clsTestType.enTestTyp)TestTypeID, testTypeTitle, TestTypeDescription, testTypeFees);
            }

            else
                return null;
    }



    }
}
