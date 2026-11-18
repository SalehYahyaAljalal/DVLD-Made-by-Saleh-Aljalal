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
    public partial class frmUpdateApplicationsTypes : Form
    {
        clsApplicationsTypes ApplicationsTypes;
        private int AppplicationTypeID = -1;
        public frmUpdateApplicationsTypes(int appplicationTypeID)
        {
            InitializeComponent();
            AppplicationTypeID = appplicationTypeID;
        }

        private void _LoadData()
        {
             ApplicationsTypes = clsApplicationsTypes.Find(AppplicationTypeID);
            if(ApplicationsTypes==null)
            {
                MessageBox.Show(" Application Type With ID[" + AppplicationTypeID + "] Not Found !!",
                    "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
                return; 
            }


            lblApplicationID.Text = AppplicationTypeID.ToString();
            txtTitle.Text = ApplicationsTypes.ApplicationTypeTitle;
            txtFees.Text = ApplicationsTypes.ApplicationTypeFees.ToString();


        }



        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmUpdateApplicationsTypes_Load(object sender, EventArgs e)
        {
            _LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields are Invalide ,put Mouse on Red Icon To See Error !!",
                    "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ;
            }

            ApplicationsTypes.ApplicationTypeTitle=txtTitle.Text.Trim();
            ApplicationsTypes.ApplicationTypeFees = Convert.ToSingle(txtFees.Text.Trim());

            if(ApplicationsTypes.Save())
            {
                MessageBox.Show("Data Saved Succefully ",
                   "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show(" Data Not Saves Succeefully !!",
                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtTitle.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "This Field Connot Be Blank !");
            }
            else
            errorProvider1.SetError(txtTitle, "");


        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "This Field Connot Be Blank !");
            }
            else
                errorProvider1.SetError(txtFees, "");

       }
    }
}
