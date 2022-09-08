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

            //string ouptutFolder = "C:\\Users\\lpriest\\OneDrive - Christie Digital Systems USA, Inc\\Documents\\SWx Macros\\WeldmentID\\3-xml\\RT\\PARTS\\";
            //string inputPath = "C:\\Users\\lpriest\\OneDrive - Christie Digital Systems USA, Inc\\Documents\\SWx Macros\\WeldmentID\\3-xml\\Gcode_DR.txt";


        }

        public static string GetPartName(string inputPath)
        {
            string[] splitCharPath = { "\\" };
            string[] pathSplit = inputPath.Split(splitCharPath, StringSplitOptions.RemoveEmptyEntries);
            string fileName = pathSplit[pathSplit.Length - 1];

            string[] splitCharFile = { "." };
            string[] filenameSplit = fileName.Split(splitCharFile, StringSplitOptions.RemoveEmptyEntries);
            string partName = "L1-" + filenameSplit[0];

            return partName;
        }
    }
}
