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
    public partial class frmAddEditUser : Form
    {
        private int _UserID = -1;
        private clsUsers _User;
        private enum enMode { Add=0,Update=1}
        private enMode _Mode;


        public frmAddEditUser()
        {
            InitializeComponent();

            _Mode = enMode.Add;

           
        }
        public frmAddEditUser(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
            _Mode=enMode.Update;
            ctrlPersonCardWithFilter1.ShowFilter = false;
        }


        private void _SetDefualtValues()
        {
            if(_Mode==enMode.Add)
            {


                lblCaption.Text = "Add New User";
                this.Text = "Add New User";
                _User = new clsUsers();
                tpAccountINfo.Enabled = false; 

            }
            else
            {

                tpAccountINfo.Enabled=true;
                btnSave.Enabled = true;
                lblCaption.Text = "Update User";
                this.Text = "Update User";

            }


        }


        private void _FillInNow()
        {

            ctrlPersonCardWithFilter1.LoadPersonData(_User.PersonID);

            lblUserID.Text = _User.UserID.ToString();
            txtUsername.Text= _User.UserName;
            txtPassword.Text= _User.Password;
            txtConfrmPasspword.Text = _User.Password;
            chkIsActive.Checked= _User.IsActive;

        }


        private void _LoadData()
        {
            _User=clsUsers.FindByUserID(_UserID);

            if( _User==null )
            {
                MessageBox.Show("User With ID [" + _UserID + "] Not Found !!", "Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                
            }


            _FillInNow();
        }


        private void frmAddEditUser_Load(object sender, EventArgs e)
        {
            _SetDefualtValues();

            if (_Mode == enMode.Update)
                _LoadData();

        }

        private bool AddingDataToUserObject()
        {
            if (_Mode == enMode.Add)
                 _User = new clsUsers();


            _User.UserName = txtUsername.Text.Trim();
            _User.Password = txtPassword.Text.Trim();
            _User.IsActive = chkIsActive.Checked;

            _User.PersonID = ctrlPersonCardWithFilter1.PersonID;

            return _User.Save();

        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some Fieled are Invalide ,put Mouse on Red Icon !", "Error ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            if(AddingDataToUserObject())
            {

                lblUserID.Text = _User.UserID.ToString();
                _Mode = enMode.Update;
                lblCaption.Text ="Update User";
                this.Text = "Update User";

                MessageBox.Show("Data Saved Successfully ", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error :Data Not Saved Successfully ", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtUsername.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUsername, "This Field Is Required !!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtUsername,null);
            }

            if (_Mode == enMode.Add)
            {

                if (clsUsers.IsThisUserExist(txtUsername.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUsername, "This UserName Is Exists Enter Another One !!");
                }
                else
                {
                    e.Cancel = false;
                    errorProvider1.SetError(txtUsername, null);
                }
            }

        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "This Field Is Required !!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPassword, null);
            }

        }

        private void txtConfrmPasspword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtConfrmPasspword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfrmPasspword, "This Field Is Required !!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtConfrmPasspword, null);
            }


            if (txtPassword.Text.Trim()!=txtConfrmPasspword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfrmPasspword, "Confirmed Password Deosn't Match the Orginal Password!!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtConfrmPasspword, null);
            }


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       

        private void btnNext_Click(object sender, EventArgs e)
        {
            
            if(_Mode==enMode.Update)
            {
                btnSave.Enabled = true;
                tpAccountINfo.Enabled = true;   
                tpUserInfoANdAccounInfo.SelectedTab = tpUserInfoANdAccounInfo.TabPages["tpAccountINfo"];
                return;
            }


            if(ctrlPersonCardWithFilter1.PersonID !=-1)
            {

                if (clsUsers.IsThisPersonExistsAsUser(ctrlPersonCardWithFilter1.PersonID))
                {

                    MessageBox.Show("This Person Exist As User Plaese Choose Another One!!", "Exists ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
                else
                {
                    btnSave.Enabled = true;
                    tpAccountINfo.Enabled = true;
                    tpUserInfoANdAccounInfo.SelectedTab = tpUserInfoANdAccounInfo.TabPages["tpAccountINfo"];
                }
                
            }
            else
            {
                MessageBox.Show("Person Not Selected Please Select A Person ?", "Person Not Selected!!",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);

            }



        }
    }
}
