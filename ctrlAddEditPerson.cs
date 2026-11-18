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
namespace DVLDProject
{
    public partial class ctrlAddEditPerson : UserControl
    {

        public ctrlAddEditPerson()
        {
            InitializeComponent();
        }


        private enum _enMode { enAdd = 0, enUpdate = 1 }
        private _enMode _Mode=_enMode.enAdd;
        private int _PersoneID = -1;
        private clsPerson persone;
        private enum _enTypeUserProcisure
        {
            InvalidData = 0, PersonExist = 1, LettersInstedNumbers = 2,
            InvalidEmail = 3,AddedSuccessfully=4,UpdatedSuccessfully=5,FaildToAdd=6,FaildToUpdate=7
        }

        private bool isValidData(string Faild)
        {
            return Faild.Length>=3;
        }
        private bool _AreFeildsValid()
        {
            return (isValidData(txtFirst.Text) 
                && isValidData(txtSecond.Text)
                &&isValidData(txtLast.Text)
                &&isValidData(txtPhone.Text)&&
                isValidData(txtAddress.Text));
        }
        private bool IsPersonExist()
        {
            persone=clsPerson.Find(txtNationalNo.Text);
            if (persone!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private byte _rdGendorChecked()
        {
            return Convert.ToByte((rdFmail.Checked) ? 1 : (rdFmail.Checked) ? 1 : 0);
        }

        private bool IsThereRealImageToPerson()
        {
            return (pcPersonImage.Image!=null)&&
            (pcPersonImage.Image!=Properties.Resources._1 || pcPersonImage.Image!=Properties.Resources._0);


        }

        private bool AddDataToPersonOpject()
        {
            if(_PersoneID==-1)
                persone=new clsPerson();


            persone.FirstName=txtFirst.Text;
            persone.SecondName=txtSecond.Text;
            if (string.IsNullOrEmpty(txtThird.Text))
            {
                persone.ThirdName="";
            }
            else
            {
                persone.ThirdName=txtThird.Text;
            }

            persone.LastName=txtLast.Text;
            persone.NationalNo=txtNationalNo.Text;
            persone.Address=txtAddress.Text;
            persone.Phone=txtPhone.Text;


            persone.NationalityCountryID=Convert.ToByte( clsCountries.Find(cmbCountry.SelectedItem.ToString()).CountryID);
            persone.Gendor=_rdGendorChecked();
            persone.DateOfBirth=dtpDate.Value;
            if(isValidData(txtEmail.Text))
            {
                persone.Email=txtEmail.Text;
            }
            else
            {
                persone.Email="";
            }

            if(!IsThereRealImageToPerson())
            {
                persone.ImagePath=_CopyImage(pcPersonImage.ImageLocation);
            }
            else
            {
                persone.ImagePath="";
            }
           
            return persone.Save();
        }

        private string _NameImageWithGUID()
        {
            Guid ImageName =  Guid.NewGuid();
            return ImageName.ToString();
        }

        private string _CopyImage(string ImagePath)
        {
            string DestionationPathFolder = @"C:\\Users\\pc\\source\\repos\\DVLDProject\\ImagesPeople";
            string DestionationPath = Path.Combine(DestionationPathFolder,
            Path.GetFileName(_NameImageWithGUID()+Path.GetExtension(ImagePath)));
            
            File.Copy(ImagePath, DestionationPath,true);

            if(DestionationPath.Length>3)
            {
                return DestionationPath;    
            }
            else
            {
                return "";
            }
        }
        private void _DeleteOldImage(string OldImage)
        {
            if(OldImage!="")
            {
                File.Delete(OldImage);
            }
        }
        private _enTypeUserProcisure _AddNewPerson()
        {
            persone=new clsPerson();
            if (!_AreFeildsValid())
            {
                return _enTypeUserProcisure.InvalidData;
            }
            if (IsPersonExist())
            {
                return _enTypeUserProcisure.PersonExist;
            }
            return AddDataToPersonOpject() ? _enTypeUserProcisure.AddedSuccessfully : _enTypeUserProcisure.FaildToAdd;

        }
        private void SetTypeGendor(byte TypeGendor)
        {
            rdMail.Checked=(TypeGendor==0) ? true : (rdFmail.Checked=true);
        }

        private void SetControlsWithDataFromPersonOpject()
        {
            txtFirst.Text=persone.FirstName;
            txtLast.Text=persone.LastName;
            txtSecond.Text=persone.SecondName;
            if(persone.ThirdName!="")
            {
                txtThird.Text=persone.ThirdName;
            }
           
            SetTypeGendor(persone.Gendor);
            dtpDate.Value=persone.DateOfBirth;
            txtAddress.Text=persone.Address;
            txtPhone.Text=persone.Phone;
            cmbCountry.SelectedIndex=cmbCountry.FindString(persone.CountryInfo.CountryName);
            if(persone.Email!="")
            {
                txtEmail.Text=persone.Email;
            }
            if(persone.ImagePath!="")
            {
                pcPersonImage.ImageLocation=persone.ImagePath;
                llRemov.Visible=true;
            }
            txtNationalNo.Text=persone.NationalNo;

        }
        private _enTypeUserProcisure _UpdatePerson(bool IsFirstTime = false)
        {
            bool IsDoneUpdated=false;
            //if (!_AreFeildsValid())
            //{
            //    return _enTypeUserProcisure.InvalidData;
            //}
            persone=clsPerson.Find(_PersoneID);
            if (persone!=null)
            {
                if (IsFirstTime)
                {
                    SetControlsWithDataFromPersonOpject();
                    lblCaption.Text="Update Person";
                    lblPersonID.Text=persone.PersonID.ToString();
                }
                else
                {
                    IsDoneUpdated= AddDataToPersonOpject();    
                }

            }
            return (IsDoneUpdated) ? _enTypeUserProcisure.UpdatedSuccessfully : _enTypeUserProcisure.FaildToUpdate;


        }


        private void FetchDataCountriesCBCountries()
        {
            foreach(DataRow row in clsCountries.AllCountries().Rows)
            {
                cmbCountry.Items.Add(row["CountryName"]);
            }
            cmbCountry.SelectedIndex = 190;
        }


        private void ctrlAddEditPerson_Load(object sender, EventArgs e)
        {
            FetchDataCountriesCBCountries();
        }

        private void TypeGendorChecked(object sender, EventArgs e)
        {

        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if(IsPersonExist())
            {
                epPersonExist.SetError(txtNationalNo, "This Person Is Allredy Exist");
                e.Cancel=true;
            }
            else
            {
                epPersonExist.SetError(txtNationalNo, "");
                e.Cancel=false;

            }
        }

        private bool _IsAtExist(char Item)
        {
            return (Item=='@');
        }
        private short _returnIndexOfAt(string Email)
        {
            for(short Item=0;Item<Email.Length;Item++)
            {
                if(_IsAtExist(Email[Item]))
                {
                    return Item;
                }
            }
            return -1;
        }

        private bool IsValidEmail(string Email)
        {
            return (Email.ToLower()=="gmail.com".ToLower());
        }
        private bool _ReadAfterAt(string Email)
        {
            if(_returnIndexOfAt(Email)==-1)
            {
                return false;
            }
            else
            {
               return IsValidEmail(Email.Substring(_returnIndexOfAt(Email)+1));
            }

        }
       

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if(isValidData(txtEmail.Text))
            {
                if(!_ReadAfterAt(txtEmail.Text))
                {
                    epInvalidEmail.SetError(txtEmail, "Invalid Email");
                    e.Cancel=true;
                }
                else
                {
                    epInvalidEmail.SetError(txtEmail, "");
                    e.Cancel=false;
                }
            }
            else
            {
                epInvalidEmail.SetError(txtEmail, "");
                e.Cancel=false;
            }
        }

        private bool _IsNumber(char Item)
        {
            
          return char.IsDigit(Item);
        }

        private bool _CheckPhone()
        {
            for(byte Item=0;Item<txtPhone.Text.Length;Item++)
            {
                if(!_IsNumber(txtPhone.Text[Item]))
                {
                    return true;
                }
            }
            
                return false;   
            
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            if(_CheckPhone())
            {
                epInvalidPhone.SetError(txtPhone, "Invalid Phone Number");
                e.Cancel = true;    
            }
            else
            {
                epInvalidPhone.SetError(txtPhone, "");
                e.Cancel = false;

            }
        }

        private void LinkSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                pcPersonImage.Load(openFileDialog1.FileName);
                if (pcPersonImage.Image != null)
                {


                }
                llRemov.Visible = (openFileDialog1.FileName != "") ? true : false;
            }
        }

        private void _ShowMessageAccordingToResult(_enTypeUserProcisure TypeUserProcisure)
        {
            switch(TypeUserProcisure)
            {
                case _enTypeUserProcisure.InvalidData:
                    MessageBox.Show("Invalid Data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case _enTypeUserProcisure.PersonExist:
                    MessageBox.Show("This Person Is Exist", "Exist", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case _enTypeUserProcisure.FaildToAdd:
                    MessageBox.Show("This Person is Faild To Add Enter Correct Data", "Faild To Add", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case _enTypeUserProcisure.FaildToUpdate:
                    MessageBox.Show("This Person is Faild To Update Enter Correct Data", "Faild To Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case _enTypeUserProcisure.LettersInstedNumbers:
                    MessageBox.Show("Enter Numbers Inster Letter","Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            switch (_Mode)
            {
                case _enMode.enAdd:
                    if(_AddNewPerson()==_enTypeUserProcisure.AddedSuccessfully)
                    {
                        lblPersonID.Text = persone.PersonID.ToString();
                        _Mode=_enMode.enUpdate;
                        lblCaption.Text="Update Person";
                        _PersoneID=persone.PersonID;
                        MessageBox.Show("Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        _ShowMessageAccordingToResult(_AddNewPerson());
                    }

                    break;

                case _enMode.enUpdate:
                    if(_UpdatePerson()==_enTypeUserProcisure.UpdatedSuccessfully)
                    {
                        MessageBox.Show("Updated Successfully", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        _ShowMessageAccordingToResult(_UpdatePerson());

                    }
                    break;
            }

        }


        public void LoadDataForPerson(int PersonID)
        {
           
            _PersoneID=PersonID;
            _Mode=(_PersoneID!=-1) ? _enMode.enUpdate : _enMode.enAdd;
            if (_Mode==_enMode.enUpdate)
            {
                _UpdatePerson(true);
            }
            else
                persone=new clsPerson();   
        }

        private void llRemov_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(pcPersonImage.Image!=null)
            {
                pcPersonImage.ImageLocation=null;
                llRemov.Visible=false;
            }
        }

        private void _ChangeImageAccordingToGendor(object sender, EventArgs e)
        {
            pcPersonImage.Image=(rdMail.Checked==true) ? Properties.Resources._1 : Properties.Resources._0;
        }

        private void rdMail_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rdFmail_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ctrlPersoInfo1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
