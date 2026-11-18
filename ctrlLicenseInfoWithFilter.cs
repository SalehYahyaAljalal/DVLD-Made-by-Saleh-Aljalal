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
    public partial class ctrlLicenseInfoWithFilter : UserControl
    {

        public event Action<int> OnLicenseSelected;
        private int _LicenseID = -1;

      
        public int LicenseID
        {
            get
            {
                return ctrlLicenseInfo11.LicenseID;
            }
        }
        public clsLicense SelectedLicense
        {
            get
            {
                return ctrlLicenseInfo11.LicenseInfo;
            }
        }

        private bool _FilterEnabled = true;

        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
         
            set
            {
                _FilterEnabled = value;
                gpFilter.Enabled = _FilterEnabled;
            }
        }



        public ctrlLicenseInfoWithFilter()
        {
            InitializeComponent();
        }


        public void LoadDataOfLicense(int LisenseID)
        {
            txtFilter.Text = LisenseID.ToString();
            ctrlLicenseInfo11.LoadLicenseInfo(LisenseID);
            _LicenseID = ctrlLicenseInfo11.LicenseID;


            if (OnLicenseSelected != null && _FilterEnabled)
                OnLicenseSelected(_LicenseID);

        }


        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {

            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);


            if (e.KeyChar == (char)13  )
            {
                btnFind.PerformClick();

            }

           
         

          

        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren ())
            {
                MessageBox.Show("Ther's An Error Put mouse On Red Icon To See Type Error !!","Error",
                    MessageBoxButtons.OK,MessageBoxIcon.Error);
                txtFilter.Focus();
                return;
            }
            _LicenseID = int.Parse(txtFilter.Text);

             LoadDataOfLicense(_LicenseID);

        }

        public void FocusFilter()
        {
            txtFilter.Focus();
        }

        private void txtFilter_Validating(object sender, CancelEventArgs e)
        {

           if(string.IsNullOrWhiteSpace(txtFilter.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFilter,"License ID Is Requirde ");

            }
           else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFilter,null);

            }


        }
    }
}
