using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using BussnissLayerDVLD;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DVLDProject
{
    public partial class frmAllPeople : Form
    {
        private  static DataTable _dtAllPeople = clsPerson.AllPeople();
        private DataTable _dtPeople = _dtAllPeople.DefaultView.ToTable(false, @"PersonID", "NationalNO", "FirstName",
            "SecondName", "ThirdName", "LastName", "Gendor", "DateOfBirth","CountryName", "Phone", "Email");


        private void frmAllPeople_Load(object sender, EventArgs e)
        {
            dgvPeople.DataSource = _dtPeople;
            lblRecords.Text = _dtPeople.Rows.Count.ToString();

            cmbFiltter.SelectedIndex = 0;

            if(_dtPeople.Rows.Count>0)
            {
                dgvPeople.Columns[0].HeaderText = "Person ID";
                dgvPeople.Columns[0].Width = 110;

                dgvPeople.Columns[1].HeaderText = "National NO";
                dgvPeople.Columns[1].Width = 120;

                dgvPeople.Columns[2].HeaderText = "First Name";
                dgvPeople.Columns[2].Width = 120;

                dgvPeople.Columns[3].HeaderText = "Second Name";
                dgvPeople.Columns[3].Width = 120;
              
                dgvPeople.Columns[4].HeaderText = "Third Name";
                dgvPeople.Columns[4].Width = 120;

                dgvPeople.Columns[5].HeaderText = "Last Name";
                dgvPeople.Columns[5].Width = 120;
               
                dgvPeople.Columns[6].HeaderText = "Gendor";
                dgvPeople.Columns[6].Width = 120;

                dgvPeople.Columns[7].HeaderText = "Date Of Birth";
                dgvPeople.Columns[7].Width = 140;

                dgvPeople.Columns[8].HeaderText = "Country Name";
                dgvPeople.Columns[8].Width = 120;

                dgvPeople.Columns[9].HeaderText = "Phone";
                dgvPeople.Columns[9].Width = 120;
                
                dgvPeople.Columns[10].HeaderText = "Email";
                dgvPeople.Columns[10].Width = 170;


            }




        }
        public frmAllPeople()
        {
            InitializeComponent();
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbFiltter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible = (cmbFiltter.Text != "None");



            if (txtFilter.Visible)
            {
                txtFilter.Text = "";
                txtFilter.Focus();
            }
            else
                txtFilter.Clear();

        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {


            string FilterColumn = "";

            switch(cmbFiltter.Text.Trim())
            {
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "National No":
                    FilterColumn = "NationalNo";
                    break;
                case "First Name":
                    FilterColumn = "FirstName";
                    break ;
                case "Second Name":
                    FilterColumn = "SecondName";
                    break;
                case "Third Name":
                    FilterColumn = "ThirdName";
                        break;
                case "Last Name":
                    FilterColumn = "LastName";
                    break;

                case "Country Name":
                    FilterColumn = "CountryName";
                    break;
                case "Gendor":
                    FilterColumn = "Gendor";
                    break;
                case "Phone":
                    FilterColumn = "Phone";
                    break;
                case "Email":
                    FilterColumn = "Email";
                    break;
            }

            if(FilterColumn == "None"|| txtFilter.Text=="")
            {
                _dtPeople.DefaultView.RowFilter = "";
                txtFilter.Clear();
               return;
            }

            if (FilterColumn == "PersonID")
            {
                _dtPeople.DefaultView.RowFilter =
                    String.Format("[{0}] = {1}", FilterColumn, txtFilter.Text.Trim());
            }
            else
                _dtPeople.DefaultView.RowFilter = String.Format("[{0}] LIKE '{1}%'", FilterColumn,
                    txtFilter.Text.Trim());



            lblRecords.Text = dgvPeople.Rows.Count.ToString();

        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (cmbFiltter.Text.Trim() == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);


        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmPersonInformation frmPerson =
                new frmPersonInformation((int)dgvPeople.CurrentRow.Cells[0].Value);
            frmPerson.ShowDialog();

            frmAllPeople_Load(null,null);
        }
        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frmAddEditPerson = new frmAddEditPerson();
            frmAddEditPerson.ShowDialog();

            frmAllPeople_Load(null, null);

        }

        private void updatePersonToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmAddEditPerson frmAddEditPerson =
                new frmAddEditPerson((int)dgvPeople.CurrentRow.Cells[0].Value);
            frmAddEditPerson.ShowDialog();
           
            frmAllPeople_Load(null, null);

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are You Sure You Want To Delete This Person ?","Deleting",
                MessageBoxButtons.OKCancel,MessageBoxIcon.Warning)==DialogResult.OK)
            {

                if (clsPerson.DeletePerson((int)dgvPeople.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Deleted Successfully", "Deleted",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    frmAllPeople_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Field To Delete Successfully !!!", "Deleted",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }



        }

        private void dgvPeople_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmPersonInformation frmPerson =
              new frmPersonInformation((int)dgvPeople.CurrentRow.Cells[0].Value);
            frmPerson.ShowDialog();

            frmAllPeople_Load(null, null);

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frmAddEditPerson = new frmAddEditPerson();
            frmAddEditPerson.ShowDialog();

            frmAllPeople_Load(null, null);

        }
    }
}
