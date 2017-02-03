using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using csmatio.types;
using csmatio.io;

namespace MHA
{
    public partial class Form1 : Form
    {
        GraphPane plot1;
        
        string matfile; // .MAT file with DSL prescription
        string ghafile; // GHA data file

        public Form1()
        {
            InitializeComponent();

            plot1 = zedGraphControl1.GraphPane;
            plot1.XAxis.Title.Text = "frequency (kHz)";
            plot1.YAxis.Title.Text = "output level (dB SPL)";
        }

        private void loadDLbutton_Click(object sender, EventArgs e)
        {
            loadSDL();
        }

        private void generateGHAbutton_Click(object sender, EventArgs e)
        {
            loadSDL();
        }

        private void loadDSLPrescriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void generateGHADatafileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool loadSDL()
        {
            OpenFileDialog fo = new OpenFileDialog();
            fo.Filter = "MATLAB file (*.mat)|*.mat";
            if (fo.ShowDialog() == DialogResult.OK)
            {
                matfile = fo.FileName;
                try
                {
                    MatFileReader r = new MatFileReader(fo.FileName);
                    Dictionary<string, MLArray> content = r.Content;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading mat file");
                    return false;
                }
                return true;
            }
            else return false;
        }

        private bool ClearAll()
        {
            
            return true; 
        }
        
    }
}
