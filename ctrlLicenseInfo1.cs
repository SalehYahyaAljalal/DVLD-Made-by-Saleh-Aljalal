using BussnissLayerDVLD;
using DVLDProject.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using BussnissLayerDVLD;

namespace DVLDProject
{
    public partial class ctrlLicenseInfo1 : UserControl
    {
        public ctrlLicenseInfo1()
        {
            InitializeComponent();
        }
        private int _LicenseID = -1;
        private clsLicense _LicenseInfo;



        public int LicenseID
        {
            get { return _LicenseID; }
        }

        public clsLicense LicenseInfo
        {
            get { return _LicenseInfo; }
        }


        public void ResetDefaultValues()
        {

            lblClass.Text = "[????]";
            lblName.Text = "[????]";
            lblLicenseID.Text = "[????]";
            lblNationalNO.Text = "[????]";
            lblGender.Text = "[????]";
            lblIssueDate.Text = "[????]";
            lblIssueReason.Text = "[????]";
            lblNotes.Text = "[????]";

            lblIsActive.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblDriverID.Text = "[????]";

            lblExpirationDate.Text = "[????]";

            _LicenseID = -1;

            pcPersonImage.Image = Resources.Male_512;

        }


        private void _LoadImageOfPerson()
        {
            if (_LicenseInfo.Driverinfo.PersonInfo.ImagePath != "")
                if(File.Exists(_LicenseInfo.Driverinfo.PersonInfo.ImagePath))
                       pcPersonImage.ImageLocation = _LicenseInfo.Driverinfo.PersonInfo.ImagePath;

            else
            {
                if (_LicenseInfo.Driverinfo.PersonInfo.Gendor == 0)
                    pcPersonImage.Image = Resources.Male_512;
                else
                    pcPersonImage.Image = Resources.Female_512;
            }


        }
        public void LoadLicenseInfo(int LicenseID)
        {

            _LicenseID = LicenseID;

            _LicenseInfo = clsLicense.FindByID(_LicenseID);

            if (_LicenseInfo == null)
            {
                MessageBox.Show("Ther is No License With id [ " + _LicenseID + " ] ", "not Found", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                ResetDefaultValues();
                return;
            }

            lblClass.Text = _LicenseInfo.LiceneseClassesInfo.ClassName;
            lblDateOfBirth.Text = _LicenseInfo.Driverinfo.PersonInfo.DateOfBirth.ToShortDateString();
            lblDriverID.Text = _LicenseInfo.DriverID.ToString();
            lblIsDetained.Text = _LicenseInfo.IsDetained ? "Yes" : "NO";
            lblIsActive.Text=(_LicenseInfo.IsActive)?"Yes":"NO";
            lblIssueDate.Text = _LicenseInfo.IssueDate.ToShortDateString();
            lblExpirationDate.Text=_LicenseInfo.ExpirationDate.ToShortDateString(); 
            lblIssueReason.Text = _LicenseInfo.IssueText;
            lblLicenseID.Text = _LicenseID.ToString();
            lblName.Text = _LicenseInfo.Driverinfo.PersonInfo.FullName;
            lblNationalNO.Text = _LicenseInfo.Driverinfo.PersonInfo.NationalNo;
            lblNotes.Text = _LicenseInfo.Notes;
            lblGender.Text = _LicenseInfo.Driverinfo.PersonInfo.Gendor == 0 ? "Male" : "Female";

            _LoadImageOfPerson();



        }

    }
}
