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
                string resultMsg = "";

                Point[] plyPoints = readGcode.ParseGcodeFile(textBoxImport.Text); // read the coordinates from the Gcode file

                if (plyPoints != null)
                {
                    resultMsg = SerXML.Export2XML(plyPoints, partName, textBoxExport.Text); // write points in FARO XML format
                }
                else { resultMsg = "Unknown reading GCode file"; }
                

                if (resultMsg == "")
                {
                    MessageBox.Show("GCode export to XML complete", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("GCode export error: " + Environment.NewLine 
                        + Environment.NewLine + resultMsg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
