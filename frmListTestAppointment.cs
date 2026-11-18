using DVLDProject.Properties;
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
    public partial class frmListTestAppointment : Form
    {
        private clsTestType.enTestTyp _TestTypeID;
        private int _LocalDrivingLicenseApplicationID = -1;
        private DataTable dtListTestAppointments;


        public frmListTestAppointment(int LocalApplicationID, clsTestType.enTestTyp TestTypeID)
        {
            InitializeComponent();
            _TestTypeID = TestTypeID;
            _LocalDrivingLicenseApplicationID = LocalApplicationID;   
        }


        private void _SetImageAndTitleOfCurrentTest()
        {
            switch (_TestTypeID)
            {

                case clsTestType.enTestTyp.VisionTest:
                    {

                        lblCurrentTest.Text = "Vision Test Appointment";
                        this.Text = lblCurrentTest.Text;
                        pcCurrentTest.Image = Resources.Vision_512;
                        break;
                    }
                    case clsTestType.enTestTyp.WrittenTest:
                    {

                        lblCurrentTest.Text = "Written Test Appointment";
                        this.Text = lblCurrentTest.Text;
                        pcCurrentTest.Image = Resources.Written_Test_512;
                        break;
                    }
                case clsTestType.enTestTyp.StreetTest:
                    {

                        lblCurrentTest.Text = "Street Test Appointment";
                        this.Text = lblCurrentTest.Text;
                        pcCurrentTest.Image = Resources.Schedule_Test_512;
                        break;
                    }

            }



        }
        private void frmTestAppointment_Load(object sender, EventArgs e)
        {
            _SetImageAndTitleOfCurrentTest();

            ctrlLocalDrvinLicenseApplicationInfo1.LoadDataLocalDrivingLicenseApplicationByLocalID(_LocalDrivingLicenseApplicationID);
               
              dtListTestAppointments =  clsTestAppointment.GetAllTestAppoinmentPerTestType(_LocalDrivingLicenseApplicationID, _TestTypeID);


            dgvListTestAppointment.DataSource = dtListTestAppointments;

            if(dtListTestAppointments.Rows.Count>0)
            {

                dgvListTestAppointment.Columns[0].HeaderText = "Appointment ID ";
                dgvListTestAppointment.Columns[0].Width = 150;


                dgvListTestAppointment.Columns[1].HeaderText = "Appointment Date ";
                dgvListTestAppointment.Columns[1].Width = 200;

                dgvListTestAppointment.Columns[2].HeaderText = "Paid Fees ";
                dgvListTestAppointment.Columns[2].Width = 150;


                dgvListTestAppointment.Columns[3].HeaderText = "Is Locked ";
                dgvListTestAppointment.Columns[3].Width = 150;


            }


            lblRecords.Text = dtListTestAppointments.Rows.Count.ToString();

        }

        private void btnAddAppointment_Click(object sender, EventArgs e)
        {

            clsLocalDrvingLicenseApplications LocalDrvingLicenseApplication
                = clsLocalDrvingLicenseApplications.FindByLocalDrivingApplicationID(_LocalDrivingLicenseApplicationID);

            if (LocalDrvingLicenseApplication == null)
                return;

            if(LocalDrvingLicenseApplication.IsTherAnActiveTestAppointmentForThisTest(_TestTypeID))
            {
                MessageBox.Show("Thers Is Already Active Appointment For This Test !", "Thers is Active Appointment !",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }


            clsTest LastTest=LocalDrvingLicenseApplication.FindLastTestAppoinment(_TestTypeID);

            if(LastTest==null)
            {
                frmScheduleTest frm1 = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestTypeID);
                frm1.ShowDialog();
                
                frmTestAppointment_Load(null, null);

                return;

            }

            if(LastTest.TestResult)
            {
                MessageBox.Show("Person Already Passed This Test ","Not Allowed !",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            frmScheduleTest frm = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestTypeID);
            frm.ShowDialog();

            frmTestAppointment_Load(null, null);

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmnetID = (int)dgvListTestAppointment.CurrentRow.Cells[0].Value;


            frmScheduleTest frm = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestTypeID, TestAppointmnetID);
            frm.ShowDialog();


            frmTestAppointment_Load(null,null);
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmnetID = (int)dgvListTestAppointment.CurrentRow.Cells[0].Value;

            frmTakeTest frm = new frmTakeTest(_TestTypeID,TestAppointmnetID);

            frm.ShowDialog();

            frmTestAppointment_Load(null,null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
