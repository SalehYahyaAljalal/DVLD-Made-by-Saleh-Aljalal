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
    public partial class frmShowApplicationInfo : Form
    {
        private int _LocalApplicationID = -1;

        public frmShowApplicationInfo(int LocalAPplicationID)
        {
            InitializeComponent();
            _LocalApplicationID = LocalAPplicationID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmShowApplicationInfo_Load(object sender, EventArgs e)
        {

            ctrlLocalDrvinLicenseApplicationInfo1
                .LoadDataLocalDrivingLicenseApplicationByLocalID(_LocalApplicationID);
        }




    }
}
