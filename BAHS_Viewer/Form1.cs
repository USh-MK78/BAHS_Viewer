using BAHSLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAHS_Viewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public BAHS BAHS { get; set; }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog Open_BAHS = new OpenFileDialog
            {
                Title = "Open BAHS",
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "bahs file|*.bahs|All Files|*.*"
            };

            if (Open_BAHS.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(Open_BAHS.FileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);

                BAHS = new BAHS();
                BAHS.Read_BAHS(br, EndianConvert.Endian.LittleEndian);

                br.Close();
                fs.Close();

                foreach (var item in BAHS.UnknownDataArea_0.BAHS_Shader_StructData.DefinedShaderStructs)
                {
                    DefinedNameListBox.Items.Add(item.DefinedName);
                }

                foreach (var item in BAHS.ShaderAttributeNameArray)
                {
                    VariableNameListBox.Items.Add(item);
                }
            }
            else return;
        }

        private void DefinedNameListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = BAHS.UnknownDataArea_0.BAHS_Shader_StructData.DefinedShaderStructs[DefinedNameListBox.SelectedIndex];
        }

        private void VariableNameListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //???
        }
    }
}
