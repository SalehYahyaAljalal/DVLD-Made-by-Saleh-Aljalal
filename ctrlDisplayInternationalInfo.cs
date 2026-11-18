using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Windows.Forms;
using BussnissLayerDVLD;

using DVLDProject.Properties;

namespace DVLDProject
{
    public partial class ctrlDisplayInternationalInfo : UserControl
    {
        private clsInternationalLicense _InternationalLicenseInfo;
        private int _InternationalLicenseID=-1;

        public ctrlDisplayInternationalInfo()
        {
            InitializeComponent();
        }

      
        public int InternationalLicenseID
        {
            get
            {
                return _InternationalLicenseID;
            }
        }


        private void _LoadImageOfPerson()
        {
            if (_InternationalLicenseInfo.DriverInfo.PersonInfo.Gendor == 0)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;


            string ImagePath = _InternationalLicenseInfo.DriverInfo.PersonInfo.ImagePath;
            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    pbPersonImage.Load(ImagePath);
                else
                    MessageBox.Show("Connot Find this Image !" + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);




        }
     

        public void LoadInfoOfInternationalLicense(int InterNationalLicenseID)
        {
            _InternationalLicenseInfo = clsInternationalLicense.FindByInternationalLicenseID(InterNationalLicenseID);

            if (_InternationalLicenseInfo == null)
            {
                MessageBox.Show("There is No International License With Local  ID [" + InterNationalLicenseID + "]", "Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _InternationalLicenseID = -1;
                return;
            }


            _InternationalLicenseID= InterNationalLicenseID;
            lblFullName.Text = _InternationalLicenseInfo.DriverInfo.PersonInfo.FullName;
            lblLocalLicenseID.Text = _InternationalLicenseInfo.IssueingUsingLocalLicenseID.ToString();
            lblNationalNo.Text = _InternationalLicenseInfo.DriverInfo.PersonInfo.NationalNo;
            lblGendor.Text = (_InternationalLicenseInfo.DriverInfo.PersonInfo.Gendor == 0) ? "Male" : "Female";
            lblExpirationDate.Text = _InternationalLicenseInfo.ExpiraionDate.ToShortDateString();
            lblIssueDate.Text = _InternationalLicenseInfo.IssuedDate.ToShortDateString();
            lblIsActive.Text = _InternationalLicenseInfo.IsActive ? "Yes" : "No";
            lblDriverID.Text = _InternationalLicenseInfo.DriverID.ToString();
            lblDateOfBirth.Text = _InternationalLicenseInfo.DriverInfo.PersonInfo.DateOfBirth.ToShortDateString();
            lblInternationalLicenseID.Text= _InternationalLicenseInfo.InternationalLicenseID.ToString();
            lblApplicationID.Text = _InternationalLicenseInfo.ApplicationID.ToString();




        }


        
    }
}
