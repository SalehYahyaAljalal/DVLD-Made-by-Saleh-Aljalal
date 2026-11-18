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
    public partial class frmCurrentUserInfo : Form
    {
        private int _UserID = -1;
        public frmCurrentUserInfo(int UserID)
        {
            InitializeComponent();

            _UserID = UserID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCurrentUserInfo_Load(object sender, EventArgs e)
        {
            ctrlUserInfo1.LoadUserInformation(_UserID);

        }
    }
}
