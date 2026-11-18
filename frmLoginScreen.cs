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
    public partial class frmLoginScreen : Form
    {
    
        private clsUsers _User;

        public frmLoginScreen()
        {
            InitializeComponent();
        }

        private void frmLoginScreen_Load(object sender, EventArgs e)
        {

            string Username = "", Password = "";
            if(clsUtilty.GetCredentioals(ref Username, ref Password))
            {
                txtUserName.Text = Username;
                txtPassword.Text = Password;
                chkRememberMe.Checked = true;
            }

        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            _User = clsUsers.FindUserByUserNameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());
             
            if(_User!=null)
            {
               
                if(!_User.IsActive)
                {
                    MessageBox.Show("This User Not Active Plz Contact an Admin !","Not Allowed !",
                        MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }


                if(chkRememberMe.Checked)
                {
                  clsUtilty.RememberUserNameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());
                }
                else
                {
                    clsUtilty.DeleteUserCredentioalsInfo();
                }
                clsCurrentUser.CurrentUser = _User;
                this.Hide();
                frmMain Main = new frmMain(this);
                Main.ShowDialog();

            }
            else
            {
                MessageBox.Show("InValid UserName Or Password !!", "Error To Login!"
                   , MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
