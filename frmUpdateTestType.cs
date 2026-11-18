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

    public partial class frmUpdateTestType : Form
    {
        private int _TestTypeID=-1;
        private clsTestType TestType;

        public frmUpdateTestType(int TestTypeID)
        {
            InitializeComponent();
            _TestTypeID = TestTypeID;

        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void frmUpdateTestType_Load(object sender, EventArgs e)
        {
             TestType = clsTestType.Find(_TestTypeID);

            if (TestType == null)
            {
                MessageBox.Show("Test Type With ID [" + _TestTypeID +"] not Found","Not Found",MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return; 
            }

            txtTitle.Text = TestType.TestTitle;
            txtDescription.Text = TestType.Description;
            txtFees.Text = TestType.TestTypeFees.ToString();

            lblID.Text = TestType.ID.ToString();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            TestType.TestTitle = txtTitle.Text.Trim();
            TestType.Description = txtDescription.Text.Trim();
            TestType.TestTypeFees = Convert.ToSingle(txtFees.Text.Trim());


            if(TestType.Save())
            {
                MessageBox.Show("Data Saved Successfully ","Success",MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Not Saved Successfully ", "Error", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);

        }
    }
}
