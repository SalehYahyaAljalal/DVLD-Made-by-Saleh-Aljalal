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
    public partial class ctrlPersonCardWithFilter : UserControl
    {

        public event Action<int> OnPersonSelected;

        protected virtual void PersonSelected(int PersonID)
        {
            Action<int> Handler=OnPersonSelected;
            if(Handler!=null)
                Handler(PersonID);

        }


        private bool _ShobtnAddNewPerson = true;

        public bool ShowbtnAddNewPerson
        {
            get { return _ShobtnAddNewPerson; }
            set { 
                _ShobtnAddNewPerson = value; 
                btnAddPerson.Visible = _ShobtnAddNewPerson;
            }
        }


        private int _PersonID=-1;

        public int PersonID
        {
            get { return ctrlPersonCard1.PersonID; }

        }

        public clsPerson SelectedPersonInfo
        {
            get { return ctrlPersonCard1.Person; }
        }




        private bool _ShowFilter = true;
        public bool ShowFilter
        {
            get { return _ShowFilter; }
            set
            {
                _ShowFilter=value;  
                gbFilter.Enabled = _ShowFilter;
            }


        }


        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }


      

        public  void LoadPersonData(int PersonID)
        {
            cmFind.SelectedIndex = 0;
            txtFind.Text = PersonID.ToString();
            ctrlPersonCard1.LoadPersonData(PersonID);

        }
        public void LoadPersonData(string NationalNo)
        {
            cmFind.SelectedIndex = 1;
            txtFind.Text = NationalNo;

            ctrlPersonCard1.LoadPersonData(NationalNo);

        }

        private void _FindNow()
        {

            if (txtFind.Text == "")
                return;

            switch(cmFind.Text)
            {
                case "Person ID":
                   ctrlPersonCard1.LoadPersonData(int.Parse(txtFind.Text));
                    break;

                case "National No":
                    ctrlPersonCard1.LoadPersonData(txtFind.Text);
                    break;
            
            }


            if(OnPersonSelected !=null && ShowFilter)
                OnPersonSelected(ctrlPersonCard1.PersonID);


            // PersonSelected(ctrlPersonCard1.PersonID);

        }


        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cmFind.SelectedIndex = 0;
            txtFind.Focus();
        }

        private void btnSearchPerson_Click(object sender, EventArgs e)
        {
          if(!this.ValidateChildren())
            {
                MessageBox.Show("Some Filed are not valid ,put mouse over the red to see Error ",
                    "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FindNow();

        }

        private void cmFind_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFind.Text = "";
            txtFind.Focus();

        }

        private void txtFind_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFind.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFind, "This Field Is Requisted !!");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFind, null);
            }


        }

        private void DataBack(object obj,int PersonID)
        {
            cmFind.SelectedIndex = 1;
            txtFind.Text=PersonID.ToString();
            ctrlPersonCard1.LoadPersonData(PersonID);

        }

        public void FilterFocus()
        {
            txtFind.Focus();
        }


        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddEditPerson addEditPerson=new frmAddEditPerson();
            addEditPerson.DataBack += DataBack;
            addEditPerson.ShowDialog();

        }

        private void txtFind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==(char)13)
            {
                btnSearchPerson.PerformClick();
            }

            if(cmFind.Text=="Person ID")
                e.Handled=!char.IsDigit(e.KeyChar) &&  !char.IsControl(e.KeyChar);



        }
    }
}
