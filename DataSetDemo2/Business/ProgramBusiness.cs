using DataSetDemo2.DataAccess;
using DataSetDemo2.GUI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataSetDemo2.Business {
    public class ProgramBusiness {

        private DataSet APP_DATASET;
        private MainWindow mainWindow;

        public ProgramBusiness() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            this.APP_DATASET = new DataSet();
            this.mainWindow = new MainWindow(this);
        }
        

        public void OpenProgramWindow() {
            Application.Run(this.mainWindow);
        }

        public void CloseProgramWindow() {
            Application.Exit();
        }

        public void LoadDataFromDatabase() {
            DemoTable.FillDataSet(this.APP_DATASET);
            this.mainWindow.BindDataTabletoGridView(this.APP_DATASET.Tables[DemoTable.DATASET_TABLE_NAME]);
        }

        public void UpdateDataInDatabase() {
            DemoTable.UpdateDataSet(this.APP_DATASET);
        }
    }
}
