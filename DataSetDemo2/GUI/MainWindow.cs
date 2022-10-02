using DataSetDemo2.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataSetDemo2.GUI {
    public partial class MainWindow : Form {

        private ProgramBusiness programBusiness;

        public MainWindow(ProgramBusiness programBusiness) {
            this.programBusiness = programBusiness;
            InitializeComponent();
        }

        private void buttonSaveChanges_Click(object sender, EventArgs e) {
            this.programBusiness.UpdateDataInDatabase();
        }

        private void buttonExit_Click(object sender, EventArgs e) {
            this.programBusiness.CloseProgramWindow();
        }

        private void buttonLoadData_Click(object sender, EventArgs e) {
            this.programBusiness.LoadDataFromDatabase();
        }

        public void BindDataTabletoGridView(DataTable table) {
            this.dataGridView1.DataSource = table;
            this.dataGridView1.Refresh();
        }
    }
}
