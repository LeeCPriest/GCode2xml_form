using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCode2xml
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new InputForm());
        }

        public static string GetPartName(string inputPath)
        {
            string[] splitCharPath = { "\\" };
            string[] pathSplit = inputPath.Split(splitCharPath, StringSplitOptions.RemoveEmptyEntries);
            string fileName = pathSplit[pathSplit.Length - 1];

            string[] splitCharFile = { "." };
            string[] filenameSplit = fileName.Split(splitCharFile, StringSplitOptions.RemoveEmptyEntries);
            string partName = filenameSplit[0];

            return partName;
        }
    }
}
