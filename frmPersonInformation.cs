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
    public partial class frmPersonInformation : Form
    {

        public frmPersonInformation(int PersonID)
        {
            InitializeComponent();
            ctrlPersoInfo1.LoadPersonData(PersonID);
        }

        public frmPersonInformation(string NationalNO )
        {
            InitializeComponent();
            ctrlPersoInfo1.LoadPersonData( NationalNO );
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
