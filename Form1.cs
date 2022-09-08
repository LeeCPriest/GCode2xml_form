using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCode2xml
{
    public partial class InputForm : Form
    {
        public InputForm()
        {
            InitializeComponent();
        }

        private void buttonProcess_Click(object sender, EventArgs e)
        {
            if ( textBoxImport.Text != "" && textBoxExport.Text != "")
            {
                string partName = Program.GetPartName(textBoxImport.Text); // extract partname from path (i.e. filename without extension plus required prefix 'L1-')

                Point[] plyPoints = readGcode.ParseGcodeFile(textBoxImport.Text); // read the coordinates from the Gcode file

                SerXML.Export2XML(plyPoints, partName, textBoxExport.Text); // write points in FARO XML format
            }
            else if ( textBoxImport.Text == "" ) { MessageBox.Show("Please specify an import GCode file",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Exclamation);}
            else if ( textBoxExport.Text == "" ) { MessageBox.Show("Please specify an export path", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void buttonBrowseImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.ShowDialog();

            if (fd.FileName != "") 
            { textBoxImport.Text = fd.FileName; }
        }

        private void buttonBrowseExport_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.ShowDialog();

            if (fb.SelectedPath != "")
            { textBoxExport.Text = fb.SelectedPath; }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
