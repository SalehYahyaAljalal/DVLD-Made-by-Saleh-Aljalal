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
using DVLDProject.Properties;
using System.IO;


namespace DVLDProject
{
    public partial class frmAddEditPerson : Form
    {
        private int _PersonID = -1;

        private enum _enMode { Add=0,Update=1}
        private _enMode _Mode;

        public delegate void DataBackHandeler(object obj, int PersonID);
        public event DataBackHandeler DataBack;
        private enum enGendor { Male=0,Female=1}



        private clsPerson _Person;
        public frmAddEditPerson(int PersonID)
        {
            
            InitializeComponent();
            _PersonID = PersonID;
            _Mode = _enMode.Update;
        }

        public frmAddEditPerson()
        {

            InitializeComponent();
            _Mode=_enMode.Add;
        }
       

        private void _FillCountriesInCompobox()
        {
            DataTable Countries = clsCountries.AllCountries();
            foreach(DataRow Row in  Countries.Rows)
            {
                cmbCountry.Items.Add(Row["CountryName"]);
            }
        }

        private void _LoadData()
        {
            _Person=clsPerson.Find(_PersonID);
            if(_Person==null)
            {
                MessageBox.Show("No Person With Id = "+_PersonID
                    , "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            lblPersonID.Text=_PersonID.ToString();
            txtFirst.Text=_Person.FirstName;
            txtSecond.Text=_Person.LastName;
            txtThird.Text=_Person.ThirdName;
            txtLast.Text=_Person.LastName;
            txtNationalNo.Text=_Person.NationalNo;
            dtpDate.Value=_Person.DateOfBirth;

            if(_Person.Gendor==0)
            {
                rdMale.Checked=true;
            }
            else
            {
                rdfemale.Checked=true;
            }
            txtAddress.Text=_Person.Address;
            txtPhone.Text=_Person.Phone;
            txtEmail.Text=_Person.Email;
            cmbCountry.SelectedIndex=cmbCountry.FindString(_Person.CountryInfo.CountryName);

            if(_Person.ImagePath!="")
            {
                pcPersonImage.ImageLocation=_Person.ImagePath;
            }

            llRemov.Visible=(_Person.ImagePath!="");

        }

        private void _ResetDefaultValue()
        {
            _FillCountriesInCompobox();
            if(_Mode==_enMode.Add)
            {
                lblCaption.Text="Add New Person";
                _Person=new clsPerson();
            }
            else
            {
                lblCaption.Text="Update Person";
            }
            rdMale.Checked=true;

            if (rdMale.Checked)
            {
                pcPersonImage.Image=Resources.Male_512;
            }
            else
            {
                pcPersonImage.Image=Resources.Female_512;
            }

            llRemov.Visible=(pcPersonImage.ImageLocation!=null);    
            dtpDate.MaxDate=DateTime.Now.AddYears(-18);
            dtpDate.Value=dtpDate.MaxDate;  

            dtpDate.MinDate=(DateTime.Now.AddYears(-100));
            cmbCountry.SelectedIndex=cmbCountry.FindString("Yemen");
           
            txtFirst.Text="";
            txtSecond.Text="";
            txtThird.Text="";
            txtLast.Text="";
            txtNationalNo.Text="";
            txtPhone.Text="";
            txtEmail.Text="";
            txtAddress.Text="";


        }
        private void frmAddEditPerson_Load(object sender, EventArgs e)
        {

            _ResetDefaultValue();
            if(_Mode ==_enMode.Update)
            {
                _LoadData();
            }
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {

        }


        private bool _HandelPersonIamge()
        {
            if(_Person.ImagePath!=pcPersonImage.ImageLocation)
            {
                if(_Person.ImagePath!="")
                {
                    File.Delete(_Person.ImagePath);
                }


                if(pcPersonImage.ImageLocation!=null)
                {
                    string SourceImage = pcPersonImage.ImageLocation;
                    if (clsUtilty.CopyImageToFolderProject(ref SourceImage))
                    {
                        pcPersonImage.ImageLocation = SourceImage;
                        return true;
                    }
                    else
                        return false;
                }

            }

            return true;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some Filds Are Not Valid But The Mous over The Red Icon To See What Is The Error Type");
                return;
            }

            if (!_HandelPersonIamge())
            {
                return;
            }


            byte NationalityCountryID=(byte)clsCountries.Find(cmbCountry.Text).CountryID;
            _Person.FirstName=txtFirst.Text.Trim();
            _Person.SecondName=txtSecond.Text.Trim();
            _Person.ThirdName=txtThird.Text.Trim();
            _Person.LastName=txtLast.Text.Trim();

            _Person.NationalNo=txtNationalNo.Text.Trim();
            _Person.Email=txtEmail.Text.Trim();
            _Person.Phone=txtPhone.Text.Trim();
            _Person.Address=txtAddress.Text.Trim();
            _Person.NationalityCountryID=NationalityCountryID;
            if(rdMale.Checked)
            {
                _Person.Gendor=(byte)enGendor.Male;
            }
            else
            {
                _Person.Gendor=(byte)enGendor.Female;

            }
            if(pcPersonImage.ImageLocation!=null)
            {
                _Person.ImagePath=pcPersonImage.ImageLocation;
            }

            if(_Person.Save())
            {
                lblPersonID.Text=_Person.PersonID.ToString();
                _Mode=_enMode.Update;
                lblCaption.Text="Updete Person";
                MessageBox.Show("Data Saved Successfully ", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataBack?.Invoke(this, _Person.PersonID);
            }
            else
            {
                MessageBox.Show("Error Faild To Save Data ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ValidateEmptyTextBox(object sender,CancelEventArgs e)
        {
            TextBox temp = ((TextBox)sender);
            if(string.IsNullOrEmpty(temp.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(temp, "This Faild Is Requird");
            }
            else
            {
                errorProvider1.SetError(temp, null);
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if(txtEmail.Text.Trim()=="")
            {
                return;
            }
            if(!clsValidation.ValidateEmail(txtEmail.Text))
            {
                e.Cancel=true;
                errorProvider1.SetError(txtEmail, "Invalid EmailAddress Format");
            }
            else
            {
                errorProvider1.SetError(txtEmail, null);
            }
        }


        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtNationalNo.Text))
            {
                errorProvider1.SetError(txtNationalNo, "This Faild Is Reqaird");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo,null);
            }

            if(txtNationalNo.Text!= _Person.NationalNo && clsPerson.IsPersonExist(txtNationalNo.Text))
            {
                e.Cancel=true;
                errorProvider1.SetError(txtNationalNo, "National Number Is Used For Another Person");
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }
        }

      

        private void rdMale_Click(object sender, EventArgs e)
        {
            if (pcPersonImage.ImageLocation == null)
                pcPersonImage.Image = Resources.Male_512;
        }

        private void rdfemale_Click(object sender, EventArgs e)
        {
            if (pcPersonImage.ImageLocation == null)
                pcPersonImage.Image = Resources.Female_512;


        }

        private void LinkSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                if (pcPersonImage.Image != null)
                {
                    string SelectedImage=openFileDialog1.FileName;
                    pcPersonImage.Load(SelectedImage);
                    llRemov.Visible = true;

                }
            }

        }

        private void llRemov_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pcPersonImage.ImageLocation = null;

            if(rdMale.Checked)
                pcPersonImage.Image= Resources.Male_512;
            else
                pcPersonImage.Image = Resources.Female_512;


            llRemov.Visible=false;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
