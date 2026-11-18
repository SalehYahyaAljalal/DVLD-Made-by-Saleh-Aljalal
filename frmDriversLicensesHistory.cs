using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDProject
{
    public partial class frmDriversLicensesHistory : Form
    {
        private int _PersonID = -1;
        public frmDriversLicensesHistory(int PersnoID)
        {
            InitializeComponent();
            _PersonID = PersnoID;
         
        }
        public frmDriversLicensesHistory()
        {
            InitializeComponent();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _PersonID=obj;
            if (_PersonID == -1) 
            {
                ctrlDriverLicenses1.Clear();
            }
            else

            ctrlDriverLicenses1.LoadDriversLicensesByPersonID(_PersonID);


        }

        private void frmDriversLicenses_Load(object sender, EventArgs e)
        {

            if(_PersonID!=-1)
            {
                ctrlPersonCardWithFilter1.ShowFilter = false;
                ctrlPersonCardWithFilter1.LoadPersonData(_PersonID);
                ctrlDriverLicenses1.LoadDriversLicensesByPersonID(_PersonID);

                
                ctrlDriverLicenses1.LoadDriversLicensesByPersonID(ctrlPersonCardWithFilter1.PersonID);


            }
            else
            {
                ctrlPersonCardWithFilter1.ShowFilter = true;
                ctrlPersonCardWithFilter1.FilterFocus();

            }


        }

      
    }
}
