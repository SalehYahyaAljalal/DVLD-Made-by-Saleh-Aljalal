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
using BussnissLayerDVLD;

namespace DVLDProject
{
    public partial class frmListUsers : Form
    {
       private  DataTable _dtAllUsers;


        public frmListUsers()
        {
            InitializeComponent();
        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            _dtAllUsers = clsUsers.GetAllUsers();

            dgvAllUsers.DataSource= _dtAllUsers;
            lblCount.Text= _dtAllUsers.Rows.Count.ToString();

            cmbFilter.SelectedIndex = 0;

            if(_dtAllUsers.Rows.Count>0)
            {
                dgvAllUsers.Columns[0].HeaderText = "User ID";
                dgvAllUsers.Columns[0].Width = 100;

                dgvAllUsers.Columns[1].HeaderText = "Person ID";
                dgvAllUsers.Columns[1].Width = 100;


                dgvAllUsers.Columns[2].HeaderText = "Full Name";
                dgvAllUsers.Columns[2].Width = 250;

                dgvAllUsers.Columns[3].HeaderText = "UserName";
                dgvAllUsers.Columns[3].Width = 120;

                dgvAllUsers.Columns[4].HeaderText = "Is Active";
                dgvAllUsers.Columns[4].Width = 100;

            }

        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
         
            if(cmbFilter.Text.Trim() == "Is Active") 
            {

                txtFilter.Visible = false;
                cmbIsActive.Visible = true;
                cmbIsActive.Focus();

                cmbIsActive.SelectedIndex = 0;
                
            }
            else
            {
                txtFilter.Visible = (cmbFilter.Text.Trim() != "None");
                txtFilter.Text = "";
                cmbIsActive.Visible = false;

                txtFilter.Focus();

            }


        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {

            string FilterColumn = "";
            
            switch(cmbFilter.Text.Trim())
            {
                case "User ID":
                    FilterColumn = "UserID";
                    break;

                case "UserName":
                    FilterColumn = "UserName";
                    break;
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
               default:
                    FilterColumn = "None";
                    break;
            }

            if(FilterColumn == "None" || txtFilter.Text.Trim()=="")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                lblCount.Text = _dtAllUsers.Rows.Count.ToString();
                return;
            }

            if (FilterColumn != "FullName" || cmbFilter.Text == "UserName")
            {
                _dtAllUsers.DefaultView.RowFilter = String.Format("[{0}] = {1}",FilterColumn,txtFilter.Text.Trim());
            }
            else
                _dtAllUsers.DefaultView.RowFilter=string.Format("[{0}] LIKE '{1}%' ",FilterColumn,txtFilter.Text.Trim());



            lblCount.Text = _dtAllUsers.Rows.Count.ToString();

        }

        private void cmbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {

            string FilerColumn = "IsActive";
            string FilterValue=cmbIsActive.Text;

            switch(cmbIsActive.Text.Trim())
            {
                case "All":
                    break ;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;
            }

            if (FilterValue == "All")
                _dtAllUsers.DefaultView.RowFilter = "";
           
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilerColumn, FilterValue);
           

            lblCount.Text= _dtAllUsers.Rows.Count.ToString();

        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilter.Text == "Person ID" || cmbFilter.Text == "User ID")

                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);



        }

        private void showUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvAllUsers.CurrentRow.Cells[0].Value;

            frmAddEditUser User = new frmAddEditUser(UserID);
            User.ShowDialog();


            frmListUsers_Load(null, null);

        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditUser User = new frmAddEditUser();
            User.ShowDialog();


            frmListUsers_Load(null, null);

        }

        private void editUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditUser User = new frmAddEditUser((int)dgvAllUsers.CurrentRow.Cells[0].Value);
            User.ShowDialog();

            frmListUsers_Load(null, null);

        }

        private void deleteUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are You Sure That You Want TO Delete This User ?","Delete",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)==DialogResult.OK)
            {

                if (clsUsers.DeleteUser((int)dgvAllUsers.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show(" Deleted Successfully ", "Delete",
                         MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    frmListUsers_Load(null,null);

                }else
                    MessageBox.Show("Error:User Not  Deleted Because there is Data Likned to It ", "Delete",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);

            }



        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            frmChanagePassword frmchnagePassword = new frmChanagePassword((int)dgvAllUsers.CurrentRow.Cells[0].Value);
            frmchnagePassword.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditUser User = new frmAddEditUser((int)dgvAllUsers.CurrentRow.Cells[0].Value);
            User.ShowDialog();

            frmListUsers_Load(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
