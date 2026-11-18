using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BussnissLayerDVLD;
using System.IO;
using DVLDProject.Properties;



namespace DVLDProject
{
    public partial class ctrlPersonCard : UserControl
    {
        private clsPerson _Person;
        private int _PersonID=-1;

        public int PersonID
        {
            get { return _PersonID; }
        }

        public clsPerson Person
        {
            get { return _Person; }
        }
        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        private void _LoadPersonImage()
        {
            if (_Person.Gendor == 0)
                pcPersonImage.Image = Resources.Male_512;
            else
                pcPersonImage.Image = Resources.Female_512;


            if (_Person.ImagePath != "")
                if (File.Exists(_Person.ImagePath))
                    pcPersonImage.ImageLocation = _Person.ImagePath;
                
        }

        private void _FillPersonData()
        {
            llEdidPersonInfo.Enabled = true;
            _PersonID=_Person.PersonID;
            lblPersonID.Text = _Person.PersonID.ToString();
            lblName.Text= _Person.FullName;
            lblNationalNumber.Text = _Person.NationalNo;
            lblGendor.Text = (_Person.Gendor == 0) ? "Male" : "Female";
            lblEmail.Text = _Person.Email;
            lblAddress.Text = _Person.Address;
            lblDataOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblPhone.Text= _Person.Phone;
            lblCountry.Text=_Person.CountryInfo.CountryName;

            _LoadPersonImage();

        }

        public void LoadPersonData(int PersonID)
        {
            _Person=clsPerson.Find(PersonID);
            if(_Person==null)
            {
                _SetDefaultValues();
                MessageBox.Show("Person With ID ["+ PersonID +"] Not Found",
                    "Not Found",MessageBoxButtons.OK, MessageBoxIcon.Error);
               

                return;
            }

            _FillPersonData();

        }

        private void _SetDefaultValues()
        {
            llEdidPersonInfo.Enabled = false;

            lblPersonID.Text="[???]";
            lblName.Text="[???]";
            lblNationalNumber.Text="[???]";
            lblGendor.Text="[???]";
            lblEmail.Text="[???]";
            lblAddress.Text="[???]";
            lblDataOfBirth.Text="[???]";
            lblPhone.Text="[???]";
            lblCountry.Text="[???]";
            _PersonID = -1;

        }
        public void LoadPersonData(string NationalNO)
        {
            _Person=clsPerson.Find(NationalNO);
            if (_Person==null)
            {
                _SetDefaultValues();
                MessageBox.Show("Person With NationNo [" + NationalNO + "] Not Found",
                   "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
              

                return;
            }

            _FillPersonData();
        }

      

        private void ctrlPersoInfo_Load(object sender, EventArgs e)
        {
            //_SetDefaultValues();
            

        }

        private void llEdidPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            frmAddEditPerson AddEdit=new frmAddEditPerson(_PersonID);
            AddEdit.ShowDialog();


            LoadPersonData(_PersonID);

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
