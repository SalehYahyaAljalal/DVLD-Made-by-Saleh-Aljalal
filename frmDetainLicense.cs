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
    public partial class frmDetainLicense : Form
    {
        private int _SelectedLicenseID = -1;

        public frmDetainLicense()
        {
            InitializeComponent();
        }
        public frmDetainLicense(int LicenseID)
        {
            InitializeComponent();
            _SelectedLicenseID = LicenseID;
        }


        private void frmDetainLicense_Load(object sender, EventArgs e)
        {


            lblDetainDate.Text = DateTime.Now.ToShortDateString();
            lblCreatedBy.Text = clsCurrentUser.CurrentUser.UserName;


            //if(_LicenseID!=-1)
            //{
            //    btnDetain.Enabled=true;
            //    llshowHistoryLicenses.Enabled=true;
            //    txtFineFees.Focus();

            //}else
            //{
            //    btnDetain.Enabled = false; ;
            //    llshowHistoryLicenses.Enabled = true;

            //}

        }

        private void ctrlLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {

            _SelectedLicenseID = obj;

            lblLicenseID.Text = _SelectedLicenseID.ToString();

            if (_SelectedLicenseID == -1)
            {
                btnDetain.Enabled = false;
                txtFineFees.Enabled = false;
                ctrlLicenseInfoWithFilter1.FocusFilter();
                return;
            }

            if (ctrlLicenseInfoWithFilter1.SelectedLicense.IsDetained)
            {
                MessageBox.Show("Select License Is Already Detained ,Select Another One !!");
                btnDetain.Enabled = false;
                txtFineFees.Enabled = false;
                ctrlLicenseInfoWithFilter1.FocusFilter();
                return;
            }


            if (!ctrlLicenseInfoWithFilter1.SelectedLicense.IsActive)
            {
                MessageBox.Show("Select License Is Not  Active  ,Select Another One !!");
                btnDetain.Enabled = false;
                txtFineFees.Enabled = false;
                ctrlLicenseInfoWithFilter1.FocusFilter();
                return;
            }

            _SelectedLicenseID = ctrlLicenseInfoWithFilter1.SelectedLicense.LicesnseID;

            btnDetain.Enabled = true;
            llshowHistoryLicenses.Enabled = true;
            txtFineFees.Focus();





        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are You Sure That You Want ot Detain this License ???","Confirm "
                ,MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.No)
            {
                return;
            }

            float FineFees = int.Parse(txtFineFees.Text.Trim());

            int DetainID = ctrlLicenseInfoWithFilter1.SelectedLicense.DetainLicense(FineFees, clsCurrentUser.CurrentUser.UserID);

            if(DetainID ==-1)
            {
                MessageBox.Show(" License Not  Detained Successfully ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                return;
            }

            llShowNewLicenseInfo.Enabled = true;
            MessageBox.Show(" License   Detained Successfully", "Detained", MessageBoxButtons.OK, MessageBoxIcon.Information); ;
            lblDetainID.Text = DetainID.ToString();

            btnDetain.Enabled = false;
            txtFineFees.Enabled = false;
            ctrlLicenseInfoWithFilter1.FilterEnabled = false; 



        }

        private void frmDetainLicense_Activated(object sender, EventArgs e)
        {
            ctrlLicenseInfoWithFilter1.FocusFilter();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llshowHistoryLicenses_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int PersonID = ctrlLicenseInfoWithFilter1.SelectedLicense.Driverinfo.PersonID;

            frmDriversLicensesHistory frm = new frmDriversLicensesHistory(PersonID);

            frm.ShowDialog();

        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_SelectedLicenseID);

            frm.ShowDialog();

        }

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {

            if(string.IsNullOrWhiteSpace(txtFineFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees,"This Field Is Required !!!");


            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFineFees,null);
            }


        }
    }
}
