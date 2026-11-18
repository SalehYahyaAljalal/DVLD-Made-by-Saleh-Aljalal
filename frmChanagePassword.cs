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
    public partial class frmChanagePassword : Form
    {
        private int _UserID = -1;
        private clsUsers _User;




        private void _ResetDefualtValues()
        {
            txtConfrmPasspword.Text = "";
            txtNewPassword.Text = "";
            txtCurrentPassword.Text = "";

            txtCurrentPassword.Focus();

            _UserID = -1;

        }

        public frmChanagePassword(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
         
        }


       
      

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCurrentPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Enter Current Password !!");
            }
            else
            {

                e.Cancel = false;
                errorProvider1.SetError(txtCurrentPassword, null);
            }

            if (_User.Password.Trim() != txtCurrentPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword,
                    "This Password Does Not Match Originaal Password");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtCurrentPassword, null);
            }

           

            
           

        }



        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
                        
            if(!string.IsNullOrEmpty(txtCurrentPassword.Text.Trim()))
            {
                if(string.IsNullOrEmpty(txtNewPassword.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtNewPassword, "Please Enter a New Password ");
                }
                else
                {
                    e.Cancel = false;
                    errorProvider1.SetError(txtNewPassword, null);
                }

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNewPassword, null);
            }

        }

        private void txtConfrmPasspword_Validating(object sender, CancelEventArgs e)
        {
           
                if(txtConfrmPasspword.Text.Trim()!= txtNewPassword.Text.Trim())
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtConfrmPasspword,"This Password Does not Match Original Password!!");
                }
                else
                {
                    e.Cancel = false;
                    errorProvider1.SetError(txtConfrmPasspword, null);
                }


        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some Error  In SOme Fields ,put Error on Red Icons To See Error !!", 
                    "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _User.Password = txtNewPassword.Text.Trim();

            if (_User.Save())
            {
                _ResetDefualtValues();
                MessageBox.Show("New Password Saved Successfully !!","Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: New Password NOT Saved Successfully !!", "Error To Save",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);


        }

        private void frmChanagePassword_Load(object sender, EventArgs e)
        {


            _User = clsUsers.FindByUserID(_UserID);
          
            if(_User==null)
            {
                MessageBox.Show("User With ID ["+_UserID+"] Not Found !!","Not FOund",MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                _ResetDefualtValues();

                this.Close();
                return;
            }



            ctrlUserInfo1.LoadUserInformation(_UserID);



        }
    }
}
