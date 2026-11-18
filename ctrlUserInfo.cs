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
    public partial class ctrlUserInfo : UserControl
    {
        private clsUsers User;
        private int _UserID = -1;


        public int UserID
        {
            get { return _UserID; }
        }
        public ctrlUserInfo()
        {
            InitializeComponent();
        }

        private void _FillNow()
        {

            ctrlPersonCard1.LoadPersonData(User.PersonID);

            lblUserID.Text = User.UserID.ToString();
            lblUserName.Text = User.UserName;
            lblIsActive.Text = (User.IsActive) ? "Yes" : "No";


        }


        private void _RestDefualtValuse()
        {
            lblIsActive.Text = "???";
            lblUserName.Text = "???";
            lblUserID.Text = "???";
        }

        public void LoadUserInformation(int UserID)
        {
            User=clsUsers.FindByUserID(UserID);

            _UserID = UserID;

            if(User==null)
            {
                _RestDefualtValuse();
                MessageBox.Show("User With ID [" + UserID + "]Not Found ", "User Not Found !!", MessageBoxButtons.OK,
                  MessageBoxIcon.Error);

                return;
            }


            _FillNow();


        }


        
    }
}
