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
using System.IO;
using System.Xml.Serialization;
using System.Runtime.InteropServices;

namespace MHA
{
    public partial class Form1 : Form
    {         
        GraphPane plotGain;
        double[][] plotdata;
        double[][] plotdataTrim;
        
        string matfile; // .MAT file with DSL prescription
        

        chapro.CHA_DSL dsl;
        string Filename = "";

        public Form1()
        {
            InitializeComponent();

            dsl = new chapro.CHA_DSL();
            LoadDefaultSettings();
            UpdateUI(false);

            plotGain = zedGC.GraphPane;
            plotGain.XAxis.Title.Text = "frequency (kHz)";
            plotGain.YAxis.Title.Text = "output level (dB SPL)";
        }

        private void UpdateUI(bool applyValues)
        {
            if (applyValues) // read UI
            {
                int n = dGV.Rows.Count-1;
                dsl.nchannel = n;
                for (int i = 0; i < dGV.Rows.Count - 1; i++)
                {
                    dsl.cross_freq[i] = double.Parse(dGV.Rows[i].Cells["CrossFrequency"].Value.ToString());
                    dsl.tk[i] = double.Parse(dGV.Rows[i].Cells["TK"].Value.ToString());
                    dsl.tkgain[i] = double.Parse(dGV.Rows[i].Cells["TKgain"].Value.ToString());
                    dsl.cr[i] = double.Parse(dGV.Rows[i].Cells["CR"].Value.ToString());
                    dsl.bolt[i] = double.Parse(dGV.Rows[i].Cells["Bolt"].Value.ToString());
                }
                dsl.attack = double.Parse(txtAttack.Text);
                dsl.release = double.Parse(txtRelease.Text);
                dsl.maxdB = double.Parse(txtMaxdB.Text);
                numUDchannels.Value = dsl.nchannel;
                toolStripStatusLabel1.Text = "Update complete";
            }
            else // update UI
            {
                dGV.Rows.Clear();

                for (int i = 0; i < dsl.nchannel; i++)
                {
                    dGV.Rows.Add(new object[] { dsl.cross_freq[i], dsl.tk[i], dsl.tkgain[i], dsl.cr[i], dsl.bolt[i]});
                    dGV.Rows[i].HeaderCell.Value = (i + 1).ToString();
                    dGV.EndEdit();
                }
                txtAttack.Text = dsl.attack.ToString();
                txtRelease.Text = dsl.release.ToString();
                txtMaxdB.Text = dsl.maxdB.ToString();
                numUDchannels.Value = dsl.nchannel;
            }
 
        }


        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loadSDL()
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
                    if (content["DSL"] is MLStructure)
                    {
                        MLStructure matStr = (MLStructure)content["DSL"];
                        //for (int i = 0; i < matStr.N; i++)
                        //{
                            //MLDouble attack = (MLDouble)matStr["attack\0",i];
                            MLDouble attack = (MLDouble)matStr["", 0];
                        //}
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading .MAT file" + ex, "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("You did not select a file! Using current parameter values", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Filename = "";
            }
            toolStripStatusLabel1.Text = "Loaded DSL prescription " + Filename;
        }

        /// <summary>
        /// Loads hard-coded values
        /// </summary>
        private void LoadDefaultSettings()
        {
            dsl.attack = 5;
            dsl.release = 50;
            dsl.maxdB = 119;
            dsl.ear = 0;
            dsl.nchannel = 8;
            dsl.cross_freq = new double[chapro.DSL_MXCH];
            dsl.cross_freq[0] = 317.1666;
            dsl.cross_freq[1] = 502.9734; 
            dsl.cross_freq[2] = 797.6319; 
            dsl.cross_freq[3] = 1264.9; 
            dsl.cross_freq[4] = 2005.9; 
            dsl.cross_freq[5] = 3181.1; 
            dsl.cross_freq[6] = 5044.7;

            dsl.tkgain = new double[chapro.DSL_MXCH];
            dsl.tkgain[0] = -13.5942;
            dsl.tkgain[1] = -16.5909;
            dsl.tkgain[2] = -3.7978;
            dsl.tkgain[3] = 6.6176;
            dsl.tkgain[4] = 11.3050;
            dsl.tkgain[5] = 23.7183;
            dsl.tkgain[6] = 35.8586;
            dsl.tkgain[7] = 37.3885;

            dsl.cr = new double[chapro.DSL_MXCH];
            dsl.cr[0] = 0.7;
            dsl.cr[1] = 0.9;
            dsl.cr[2] = 1;
            dsl.cr[3] = 1.1;
            dsl.cr[4] = 1.2;
            dsl.cr[5] = 1.4;
            dsl.cr[6] = 1.6;
            dsl.cr[7] = 1.7;

            dsl.tk = new double[chapro.DSL_MXCH];
            dsl.tk[0] = 32.2;
            dsl.tk[1] = 26.5;
            dsl.tk[2] = 26.7;
            dsl.tk[3] = 26.7;
            dsl.tk[4] = 29.8;
            dsl.tk[5] = 33.6;
            dsl.tk[6] = 34.7;
            dsl.tk[7] = 32.7;

            dsl.bolt = new double[chapro.DSL_MXCH];
            dsl.bolt[0] = 78.7667;
            dsl.bolt[1] = 88.2;
            dsl.bolt[2] = 90.7;
            dsl.bolt[3] = 92.8333;
            dsl.bolt[4] = 98.2;
            dsl.bolt[5] = 103.3;
            dsl.bolt[6] = 101.9;
            dsl.bolt[7] = 99.8;
        }

        
        /// <summary>
        /// Saves parameters to XML file
        /// </summary>
        /// <returns></returns>
        private void Save_Settings(string FileName)
        {
            StreamWriter myWriter = null;
            XmlSerializer mySerializer = null;
            try
            {   // Create an XmlSerializer for the dsl type.
                mySerializer = new XmlSerializer(typeof(chapro.CHA_DSL));
                myWriter = new StreamWriter(FileName, false);
                // Serialize this instance of the dsl class to the XML file.
                mySerializer.Serialize(myWriter, dsl);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // If the FileStream is open, close it.
                if (myWriter != null)
                {
                    myWriter.Close();
                }
            }              
        }

        /// <summary>
        /// Loads parameters from xml file
        /// </summary>
        /// <returns></returns>
        private bool Load_Settings(string FileName)
        {
            XmlSerializer mySerializer = null;
            FileStream myFileStream = null;
            bool fileExists = false;
            try
            {
                // Create an XmlSerializer for the dsl type.
                mySerializer = new XmlSerializer(typeof(chapro.CHA_DSL));
                FileInfo fi = new FileInfo(FileName);
                // If the config file exists, open it.
                if (fi.Exists)
                {
                    myFileStream = fi.OpenRead();
                    // Create a new instance of the dsl by
                    // deserializing the config file.
                    dsl = (chapro.CHA_DSL)mySerializer.Deserialize(myFileStream);
                    if (dsl.cross_freq == null)                   
                    {
                       LoadDefaultSettings();
                       return false;                  
                    }
                    fileExists = true;                                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message );
                LoadDefaultSettings();
                return false;
            }
            finally
            {
                // If the FileStream is open, close it.
                if (myFileStream != null)
                {
                    myFileStream.Close();
                }
            }
            return fileExists;
        }
            

        private bool ClearAll()
        {
            // clear plot
            plotGain.CurveList.Clear();
            plotGain.GraphObjList.Clear();
            zedGC.Refresh();

            return true; 
        }

        private void PlotData()
        {
                
            plotdata = new double[4][];
            plotdata[0] = CrossFreq_CenterFreq(dsl.cross_freq);
            plotdata[1] = dsl.tkgain;
            plotdata[2] = dsl.tk;
            plotdata[3] = dsl.bolt;

            plotdataTrim = new double[4][];
            TrimData();
            DrawGraphs(plotdataTrim);  
        }

        private double[] CrossFreq_CenterFreq(double[] cross_freq)
        {
            double[] temp = new double[chapro.DSL_MXCH];
            double[] freq = new double[chapro.DSL_MXCH];

            temp[0] = 0;
            for (int i = 0; i < chapro.DSL_MXCH-1; i++)
            {
                temp[i+1] = cross_freq[i];
            }
            temp[dsl.nchannel] = 12000;

            for (int i = 0; i < dsl.nchannel; i++)
            {
                freq[i] = (temp[i] + temp[i + 1])/2;
            }
            return freq;
        }

        private void TrimData()
        {
            for (int i = 0; i < plotdata.Length; i++)
            {
                plotdataTrim[i] = new double[dsl.nchannel];
                for (int j = 0; j < dsl.nchannel; j++)                    
                {
                    plotdataTrim[i][j] = plotdata[i][j];
                }
            }
        }

        private void DrawGraphs(double[][] datalist)
        {
            plotGain.CurveList.Clear();
            plotGain.GraphObjList.Clear();
            zedGC.Refresh();

            SetXY(plotGain, "TKgain", datalist[0], datalist[1], Color.Blue, SymbolType.Circle);
            SetXY(plotGain, "TK", datalist[0], datalist[2], Color.Red, SymbolType.Square);
            SetXY(plotGain, "BOLT", datalist[0], datalist[3], Color.Green, SymbolType.Triangle);

            zedGC.RestoreScale(plotGain);
            zedGC.Refresh();

        }

        private void SetXY(GraphPane gp, string strcurve, double[] x, double[] y, Color col, SymbolType symbol)
        {
            PointPairList pointlist = new PointPairList();

            for (int i = 0; i < x.Length; i++)
                pointlist.Add(x[i], y[i]);
            gp.AddCurve(strcurve, pointlist, col, symbol);
        }        

        /*
        public class dsl
        {
            public double attack { get; set; }           // attack time (ms)
            public double release { get; set; }          // release time (ms)
            public double maxdB { get; set; }            // maximum signal (dB SPL)
            public int ear { get; set; }                 // 0=left, 1=right
            public int nchannel { get; set; }            // number of channels
            public double[] cross_freq { get; set; }     // cross frequencies (Hz)
            public double[] tkgain { get; set; }         // compression-start gain
            public double[] cr { get; set; }             // compression ratio
            public double[] tk { get; set; }             // compression-start kneepoint
            public double[] bolt { get; set; }           // broadband output limiting threshold
        }
        */

        private void SaveXML()
        {
            SaveFileDialog fo = new SaveFileDialog();
            fo.Filter = "XML file (*.xml)|*.xml";
            if (fo.ShowDialog() == DialogResult.OK)
            {
                Filename = fo.FileName;
                Save_Settings(Filename);
            }
            else
            {
                MessageBox.Show("You did not save the data!", "Data not saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Filename = "";
            }
            toolStripStatusLabel1.Text = "Saved parameter as " + Filename;
        }

        private void LoadXML()
        {
            OpenFileDialog fo = new OpenFileDialog();
            fo.Filter = "XML file (*.xml)|*.xml";
            if (fo.ShowDialog() == DialogResult.OK)
            {
                Filename = fo.FileName;
                Load_Settings(Filename);
            }
            else
            {
                MessageBox.Show("You did not select a file! Using default parameter values", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Filename = "";
            }
            toolStripStatusLabel1.Text = "Loaded file " + Filename;
            UpdateUI(false);
        }

        private unsafe void UploadTeensy()
        {
            //void** cp;
            //cp = {0};
            
            IntPtr[] cpi;
            cpi = new IntPtr[chapro.NPTR];

            //IntPtr cpi1;
            //cp = 
            //cpi[0] = IntPtr.Zero;
            
            //cpi = new cpi[64];
            //cp[0] = (void)0;
            
            
            int nc = dsl.nchannel;
            double fs = 24000;
            int nw = 128;
            int wt = 0;
            int cs = 128;
            
            
            chapro.CHA_WDRC gha = new chapro.CHA_WDRC();
            gha.attack = 1;
            gha.release = 50;
            gha.fs = 24000;
            gha.maxdB = 119;
            gha.tkgain = 0;
            gha.tk = 105;
            gha.cr = 10;
            gha.bolt = 105;
            

            double[] cf = dsl.cross_freq;

            try
            {
                fixed (void* cp = &cpi[0])
                {
                    // prepare FIRFB
                    chapro.cha_firfb_prepare(cp, cf, nc, fs, nw, wt, cs);

                    // Initialize unmanged memory to hold the struct
                    IntPtr dsl_pnt = Marshal.AllocHGlobal(Marshal.SizeOf(dsl));

                    // Copy dsl struct to unmanaged memory.
                    Marshal.StructureToPtr(dsl, dsl_pnt, false);

                    // prepare AGC
                    chapro.cha_agc_prepare(cp, ref dsl_pnt, ref gha);

                    // generate C code from prepared data
                    chapro.cha_data_gen(cp, "cha_ff_data.h");

                    // Free the unmanaged memory
                    Marshal.FreeHGlobal(dsl_pnt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            toolStripStatusLabel1.Text = "Uploading to board... ";
        }

        private void Updatebutton_Click(object sender, EventArgs e)
        {
            UpdateUI(true);
        }

        private void Plotbutton_Click(object sender, EventArgs e)
        {
            PlotData();
        }
        private void Savebutton_Click(object sender, EventArgs e)
        {
            SaveXML();
        }

        private void Loadbutton_Click(object sender, EventArgs e)
        {
            LoadXML();
        }

        private void Uploadbutton_Click(object sender, EventArgs e)
        {
            UploadTeensy();
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateUI(true);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveXML();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadXML();
        }

        private void UploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UploadTeensy();
        }

        private void loadDSLPrescriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadSDL();
        }

        private void plotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlotData();
        }
    }
}
