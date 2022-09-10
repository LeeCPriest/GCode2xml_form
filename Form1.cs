using System;
using System.Windows.Forms;

namespace GCode2xml
{
    public partial class InputForm : Form
    {
        public InputForm()
        {
            InitializeComponent();
            textBoxExport.Text = Properties.Settings.Default.LastExportFolder;
        }

        private void buttonProcess_Click(object sender, EventArgs e)
        {
            if ( textBoxImport.Text != "" && textBoxExport.Text != "")
            {
                string partName = Program.GetPartName(textBoxImport.Text); // extract partname from path (i.e. filename without extension plus required prefix 'L1-')
                string message;
                string resultMsg;
                
                Ply[] plys = new Ply[0]; // create array of the Ply class
                resultMsg = readGcode.ParseGcodeFile(ref plys, textBoxImport.Text); // read the coordinates from the Gcode file

                if (resultMsg == "")
                {
                    resultMsg = SerXML.Export2XML(plys, partName, textBoxExport.Text); // write points in FARO XML format
                    if (resultMsg == "") { message = "GCode export to XML complete"; }
                    else { message = "Error exporting XML file: ";  }
                        
                }
                else { message = "Error reading GCode file"; }


                if (resultMsg == "")
                {
                    MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(message + Environment.NewLine + Environment.NewLine + resultMsg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            else if ( textBoxImport.Text == "" ) { MessageBox.Show("Please specify an import GCode file",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Exclamation); }
            else if ( textBoxExport.Text == "" ) { MessageBox.Show("Please specify an export path", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void buttonBrowseImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "GCode txt files (*.txt)|*.txt|All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.RestoreDirectory = true;
            fd.ShowDialog();

            if (fd.FileName != "") 
            { textBoxImport.Text = fd.FileName; }
        }

        private void buttonBrowseExport_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.SelectedPath = textBoxExport.Text;
           
            if (fb.ShowDialog() == DialogResult.OK)
            { 
                textBoxExport.Text = fb.SelectedPath;
                
                Properties.Settings.Default.LastExportFolder = fb.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
