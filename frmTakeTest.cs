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
    public partial class frmTakeTest : Form
    {
        private clsTest _Test;
        private clsTestAppointment _TestAppointment;
        private int _TestAppointmentID;
        private clsTestType.enTestTyp _TestTypeID;

        public frmTakeTest(clsTestType.enTestTyp TestTypeID, int TestAppoinmnetID)
        {
            InitializeComponent();
            _TestAppointmentID=TestAppoinmnetID;
            _TestTypeID=TestTypeID;


        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {

            ctrlScheduledTest1.TestTypeID = _TestTypeID;
            ctrlScheduledTest1.LoadDataOfScheduledTest(_TestAppointmentID);

            if (_TestAppointmentID == -1)
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;


            if (ctrlScheduledTest1.TestID != -1)
            {
                lblUserMessage.Text = "Connont test ,This Test Is Sat ! Only You Can Retake Test !";
                lblUserMessage.Visible = true;
              


              _Test = clsTest.FindByTestID(ctrlScheduledTest1.TestID);
            
                if (_Test.TestResult)
                    rdPass.Checked = true;
                else
                    rdFaild.Checked = true;


              
                txtNote.Text = _Test.Notes;
                rdFaild.Enabled = false;
                rdPass.Enabled = false;


            }
            else
                _Test = new clsTest();



        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {


            if (MessageBox.Show("Are you sure you want to save? After that you cannot change the Pass/Fail results after you save?.",
                     "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No
            )
            {
                return;
            }


            _Test.TestAppointmentID = ctrlScheduledTest1.TestAppointmentID;
            _Test.TestResult = rdPass.Checked;
            _Test.Notes=txtNote.Text.Trim();
            _Test.CreatedByUserID = clsCurrentUser.CurrentUser.UserID;




            if(_Test.Save())
            {
                MessageBox.Show("Data Saved Successfully ","Saved",MessageBoxButtons.OK,MessageBoxIcon.Information);
                btnSave.Enabled = false;
            }
            else
            {
                MessageBox.Show("Error: Data Not  Saved Successfully ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }
    }
}
