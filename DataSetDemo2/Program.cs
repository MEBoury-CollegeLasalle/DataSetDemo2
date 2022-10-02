using DataSetDemo2.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataSetDemo2 {
    internal static class Program {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            ProgramBusiness manager = new ProgramBusiness();
            manager.OpenProgramWindow();
        }
    }
}
