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

namespace DVLDProject
{
    public partial class frmAddEditLocalDrivingApplication : Form
    {
        private int _LocalDrivingLicenseApplicationID = -1;
        private int _SelectedPersonID = -1;
        private clsLocalDrvingLicenseApplications _LocalDrvingLicenseApplicationsInfo;


        private enum enMode { Add=1,Update=2}
        private enMode _Mode;

        public frmAddEditLocalDrivingApplication(int LocalDrivingLicenseApplication)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID=LocalDrivingLicenseApplication;

            _Mode = enMode.Update;
        }
        public frmAddEditLocalDrivingApplication()
        {
            InitializeComponent();

            _Mode = enMode.Add;

        }


        private  void _FillComboboxWithLicenseClasses()
        {
            DataTable LicenseClasses=clsLicenseClasses.GetAllLicensesClasses();

            foreach(DataRow row in LicenseClasses.Rows)
            {
                cmbClasses.Items.Add(row["ClassName"]);
            }

        }


        private void _ResetDefaultValues()
        {
            _FillComboboxWithLicenseClasses();


            if(_Mode==enMode.Add)
            {
                this.Text = "Add New Local Driving Application";
                lblCation.Text = this.Text;

                _LocalDrvingLicenseApplicationsInfo = new clsLocalDrvingLicenseApplications();
                ctrlPersonCardWithFilter1.FilterFocus();

                tbApplicationInfo.Enabled = false;
                cmbClasses.SelectedIndex = 2;


                lblFees.Text = clsApplicationsTypes
                    .Find((int)clsApplications.enApplicationTypes.NewLocalDrivingLicenseService).ApplicationTypeFees.ToString();

                lblAppDate.Text=DateTime.Now.ToShortDateString();
                lblCreatedByUser.Text = clsCurrentUser.CurrentUser.UserName;



            }

            else
            {
                this.Text = "Update Local Driving Application";
                lblCation.Text = this.Text;

                tbApplicationInfo.Enabled = true;
                btnSave.Enabled = true;



            }




        }

              
        private void _LoadData()
        {
            ctrlPersonCardWithFilter1.ShowFilter = false;
            _LocalDrvingLicenseApplicationsInfo = clsLocalDrvingLicenseApplications
                .FindByLocalDrivingApplicationID(_LocalDrivingLicenseApplicationID);



            if(_LocalDrvingLicenseApplicationsInfo==null)
            {
                MessageBox.Show("No APplication With ID ["+ _LocalDrivingLicenseApplicationID + "] Not Found","Not Found !!"
                  ,MessageBoxButtons.OK,MessageBoxIcon.Error      );

                return;
            }


            ctrlPersonCardWithFilter1.LoadPersonData(_LocalDrvingLicenseApplicationsInfo.ApplicantPersonID);
            lblAppID.Text= _LocalDrivingLicenseApplicationID.ToString();
            lblAppDate.Text = _LocalDrvingLicenseApplicationsInfo.ApplicationDate.ToShortDateString();
           cmbClasses.SelectedIndex=
         cmbClasses.FindString(clsLicenseClasses.FindLicenseClassInfoByID(_LocalDrvingLicenseApplicationsInfo.LicenseClassID).ClassName);
            lblFees.Text=_LocalDrvingLicenseApplicationsInfo.PaidFees.ToString();

            lblCreatedByUser.Text = clsUsers.FindByUserID(_LocalDrvingLicenseApplicationsInfo.CreatedByUserID).UserName;



        }

        private void frmAddEditLocalDrivingApplication_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
                _LoadData();

        }

      
        private void btnSave_Click(object sender, EventArgs e)
        {
            int LicenseClassID = clsLicenseClasses.FindLicenseClassInfoByClassName(cmbClasses.Text).LicenseClassID;


            int ActiveApplicationID = clsLocalDrvingLicenseApplications
                .GetActiveApplicationIDForLicenseClass(_SelectedPersonID,
                clsApplications.enApplicationTypes.NewLocalDrivingLicenseService,LicenseClassID);

            if(ActiveApplicationID!=-1)
            {
                MessageBox.Show("Choos Another License Class ,Selected Person Already Have An Active Application for The Selected Class"+ActiveApplicationID,
                    "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);

                cmbClasses.Focus();
                return;
            }

            if(clsLicense.IsThereAnActiveLicensePerPersonID(ctrlPersonCardWithFilter1.PersonID,LicenseClassID))
            {
                MessageBox.Show("Choos Another License Class ,Selected License Class,this person Already Have An Active License for The Selected Class" + ActiveApplicationID,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                cmbClasses.Focus();
                return;

            }



            _LocalDrvingLicenseApplicationsInfo.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID;
            _LocalDrvingLicenseApplicationsInfo.ApplicationDate = DateTime.Now;
            _LocalDrvingLicenseApplicationsInfo.LastStatusDate = DateTime.Now;
            _LocalDrvingLicenseApplicationsInfo.ApplicationTypeID = 1;

            _LocalDrvingLicenseApplicationsInfo.ApplicationStatus = clsApplications.enApplicationStatus.New;
            _LocalDrvingLicenseApplicationsInfo.PaidFees = Convert.ToSingle(lblFees.Text.Trim());
            _LocalDrvingLicenseApplicationsInfo.CreatedByUserID = clsCurrentUser.CurrentUser.UserID;

            _LocalDrvingLicenseApplicationsInfo.LicenseClassID = LicenseClassID;

            if(_LocalDrvingLicenseApplicationsInfo.Save())
            {
                lblAppID.Text = _LocalDrvingLicenseApplicationsInfo.LocalDrvingLicenseApplicationID.ToString();
                _Mode = enMode.Update;
                lblCation.Text = "Update Local Driving License APplication ";

                this.Text = lblCation.Text;

                MessageBox.Show("Saved Successfully ","Saved",MessageBoxButtons.OK,MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Saved Successfully ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);



        }





        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {

            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tbApplicationInfo.Enabled = true;
                tabControl1.SelectedTab = tabControl1.TabPages["tbApplicationInfo"];

            }

            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {
                btnSave.Enabled = true;
                tbApplicationInfo.Enabled = true;
                tabControl1.SelectedTab = tabControl1.TabPages["tbApplicationInfo"];


            }

            else
            {
                MessageBox.Show("Please Select A person First !!", "Select a Person !", MessageBoxButtons.OK
                    , MessageBoxIcon.Error);

                ctrlPersonCardWithFilter1.FilterFocus();
            }

        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPersonID = obj;
        }

        private void frmAddEditLocalDrivingApplication_Activated(object sender, EventArgs e)
        {

            ctrlPersonCardWithFilter1.FilterFocus();

        }
    }
}
