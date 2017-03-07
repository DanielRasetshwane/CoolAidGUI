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

namespace GenericHearingAid_Controller
{
    public partial class Form1 : Form
    {         
        GraphPane plotGain;
        double[][] plotdata;
        double[][] plotdataTrim;
        
        string matfile; // .MAT file with DSL prescription
        

        chapro.CHA_DSL dsl;
        int cs = 128;

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
                numUDchannels.Value = dsl.nchannel;
                dsl.attack = double.Parse(txtAttack.Text);
                dsl.release = double.Parse(txtRelease.Text);
                dsl.maxdB = double.Parse(txtMaxdB.Text);
                cs = int.Parse(txtChunkSize.Text);
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
                numUDchannels.Value = dsl.nchannel;
                txtAttack.Text = dsl.attack.ToString();
                txtRelease.Text = dsl.release.ToString();
                txtMaxdB.Text = dsl.maxdB.ToString();
                txtChunkSize.Text = cs.ToString();
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
                        for (int i = 0; i < matStr.N; i++)
                        {
                            //MLDouble attack = (MLDouble)matStr["attack\0",i];
                            MLDouble attack = (MLDouble)matStr["@", i];
                            //MLDouble attack = (MLDouble)matStr["", 0];
                        }
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
            dsl.tk[6] = 34.3;
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
            //DEBUG
            System.Console.WriteLine("Storage size for int : " + sizeof(int));
            System.Console.WriteLine("Storage size for long : " + sizeof(long));
            System.Console.WriteLine("Storage size for short : " + sizeof(short));
            System.Console.WriteLine("Storage size for double : " + sizeof(double));
            System.Console.WriteLine("Storage size for float : " + sizeof(float));
            // end DEBUG
                
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

        public unsafe void UploadTeensy()
        {
            IntPtr[] cpi;
            cpi = new IntPtr[chapro.NPTR];
            
            int nc = dsl.nchannel;
            double fs = 24000;
            //double atk = dsl.attack;
            //double rel = dsl.release;
            double atk, rel;
            float[] alfa_beta;
            int nw = 128;
            int wt = 0;
            //int cs = 128;
            
            
            chapro.CHA_WDRC gha = new chapro.CHA_WDRC();
            gha.attack = 1;
            gha.release = 50;
            gha.fs = 24000;
            gha.maxdB = 119;
            gha.tkgain = 0;
            gha.tk = 105;
            gha.cr = 10;
            gha.bolt = 105;

            atk = gha.attack;
            rel = gha.release;

            alfa_beta = new float[2];
            alfa_beta = chapro.time_const(atk,rel,fs);
            
            
            chapro.CHA_IVAR[chapro._cs] = cs;
            chapro.CHA_IVAR[chapro._nw] = nw;
            chapro.CHA_IVAR[chapro._nc] = nc;
            /*
            chapro.CHA_DVAR[chapro._alfa] = alfa_beta[0];
            chapro.CHA_DVAR[chapro._beta] = alfa_beta[1];
            chapro.CHA_DVAR[chapro._fs] = gha.fs;
            chapro.CHA_DVAR[chapro._mxdb] = gha.maxdB;
            chapro.CHA_DVAR[chapro._tkgn] = gha.tkgain;
            chapro.CHA_DVAR[chapro._cr] = gha.cr;
            chapro.CHA_DVAR[chapro._tk] = gha.tk;
            chapro.CHA_DVAR[chapro._bolt] = gha.bolt;
            chapro.CHA_DVAR[chapro._gcalfa] = alfa_beta[0];
            chapro.CHA_DVAR[chapro._gcbeta] = alfa_beta[1];
            */

            double[] cf = dsl.cross_freq;

            try
            {
                fixed (void* cp = &cpi[0])
                {
                    // prepare FIRFB
                    chapro.cha_firfb_prepare(cp, cf, nc, fs, nw, wt, cs);

                    // prepare chunk buffers
                    chapro.cha_allocate(cp, nc * cs * 2, sizeof(float), 3);

                    // Initialize unmanged memory to hold the struct
                    //IntPtr dsl_pnt = Marshal.AllocHGlobal(Marshal.SizeOf(dsl));

                    // Copy dsl struct to unmanaged memory.
                    //Marshal.StructureToPtr(dsl, dsl_pnt, true);

                    // prepare AGC
                    //chapro.cha_agc_prepare(cp, ref dsl_pnt, ref gha);

                    //chapro.cha_allocate(cp, nc, sizeof(float), 8);
                    //chapro.cha_allocate(cp, nc, sizeof(float), 9);
                    //chapro.cha_allocate(cp, nc, sizeof(float), 10);
                    //chapro.cha_allocate(cp, nc, sizeof(float), 11);
                    //chapro.cha_allocate(cp, nc, sizeof(float), 12);

                    // generate C code from prepared data                    
                    //chapro.cha_data_gen(cp, "cha_ff_data_c.h");

                    // Free the unmanaged memory
                    //Marshal.FreeHGlobal(dsl_pnt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // Initialize unmanged memory to hold the struct
            //IntPtr dsl_pnt = Marshal.AllocHGlobal(Marshal.SizeOf(dsl));

            // Copy dsl struct to unmanaged memory.
            //Marshal.StructureToPtr(dsl, dsl_pnt, true);

            // prepare AGC
            chapro.cha_agc_prepare(cpi, dsl, gha);
            
            // Free the unmanaged memory
            //Marshal.FreeHGlobal(dsl_pnt);


            // generate C code from prepared data
            string fn = "cha_ff_data128.h";
            chapro.cha_data_gen(cpi, fn);

            // copy file to working directory for Arduino
            string dst = @"C:\GenericHearingAid\" + fn;
            File.Copy(fn,dst,true);

            Console.WriteLine("Inside UploadTeensy:");

            int* cpsiz;
            for (int i = 0; i < 15; i++)
            {
                cpsiz = (int*)cpi[0]; 
                System.Console.WriteLine("i = " + i + " : size = " + cpsiz[i]);
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

            //chapro.CHA_WDRC gha = new chapro.CHA_WDRC();
            //gha.attack = 1;
            //gha.release = 50;
            //gha.fs = 24000;
            //gha.maxdB = 119;
            //gha.tkgain = 0;
            //gha.tk = 105;
            //gha.cr = 10;
            //gha.bolt = 105;

            //chapro.UploadBoard(dsl,gha);

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

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }

    }
}
