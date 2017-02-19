using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

// Wrapper for chapro.dll

namespace MHA
{
    public unsafe class chapro
    {

        public chapro()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        
        public const int DSL_MXCH = 32;              // maximum number of channels
        public const int NPTR = 64;
        public const int _size = 0;
        public const int _ivar = 1;
        public const int _dvar = 2;
        public const int _reserve = 3;

        // integer variable indices
        public const int _cs = 0; 
        public const int _nw = 1;
        public const int _nc = 2;

        // double variable indices
        public const int _alfa     =0;
        public const int _beta     =1;
        public const int _fs       =2;
        public const int _mxdb     =3;
        public const int _tkgn     =4;
        public const int _tk       =5;
        public const int _cr       =6;
        public const int _bolt     =7;
        public const int _gcalfa   =8;
        public const int _gcbeta   =9;

        public static int[] CHA_IVAR = new int[NPTR];
        public static double[] CHA_DVAR = new double[NPTR];


        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct CHA_DSL{
            public double attack { get; set; }              // attack time (ms)
            public double release { get; set; }             // release time (ms)
            public double maxdB { get; set; }               // maximum signal (dB SPL)
            public int ear { get; set; }                    // 0=left, 1=right
            public int nchannel { get; set; }               // number of channels
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=DSL_MXCH)]
            public double[] cross_freq;        // cross frequencies (Hz)            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = DSL_MXCH)]
            public double[] tkgain;            // compression-start gain
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = DSL_MXCH)]
            public double[] cr;                // compression ratio
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = DSL_MXCH)]
            public double[] tk;                // compression-start kneepoint
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = DSL_MXCH)]
            public double[] bolt;              // broadband output limiting threshold
        }


        public unsafe struct CHA_WDRC
        {
            public double attack { get; set; }               // attack time (ms)
            public double release { get; set; }              // release time (ms)
            public double fs { get; set; }                   // sampling rate (Hz)
            public double maxdB { get; set; }                // maximum signal (dB SPL)
            public double tkgain { get; set; }               // compression-start gain
            public double tk { get; set; }                   // compression-start kneepoint
            public double cr { get; set; }                   // compression ratio
            public double bolt { get; set; }                 // broadband output limiting threshold
        }
         

        
        #region Dll Imports

        [DllImport("chapro.dll", EntryPoint = "cha_firfb_prepare", CallingConvention = CallingConvention.StdCall)]
        public static extern int cha_firfb_prepare(void* cp, double[] cf, int nc, double fs, int nw, int wt, int cs);

        [DllImport("chapro.dll", EntryPoint = "cha_allocate", CallingConvention = CallingConvention.StdCall)]
        public static extern int cha_allocate(void* cp, int cnt, int siz, int idx);

        [DllImport("chapro.dll", EntryPoint = "cha_agc_prepare", CallingConvention = CallingConvention.StdCall)]
        public static extern int cha_agc_prepare(void* cp, ref IntPtr dsl, ref CHA_WDRC gha);
        
        [DllImport("chapro.dll", EntryPoint = "cha_data_gen", CallingConvention = CallingConvention.StdCall)]
        public static extern int cha_data_gen(void* cp, string fn);

        //[DllImport("chapro.dll", EntryPoint = "cha_data_gen", CallingConvention = CallingConvention.StdCall)]
        //public static extern int cha_data_gen(IntPtr[] cp, string fn);

        #endregion

        public static unsafe int cha_data_gen(IntPtr[] cp, string fn)
        {                
            int ptsiz, arsiz, arlen, i, j; 
            int *cpsiz;
            uint *ulptr;

                //FILE *fp;

            string[] head = {
                "#ifndef CHA_DATA_H",
                "#define CHA_DATA_H",
                ""
            };

            string[] tail = {
                "",
                "#endif // CHA_DATA_H"
            };
            int hdsz = head.Length;// / sizeof(char *);
            int tlsz = tail.Length;// / sizeof(char *);
            int ptpl = 16;
            int arpl = 8;
            int ivpl = 8;
            int dvpl = 5;
            const int SIZEOFLONG = 4;

            using (StreamWriter sw = new StreamWriter(fn))
            {
                if (sw.BaseStream == null) {
                    return (1);
                }

                cpsiz = (int *) cp[_size];
                if (cpsiz == null) {
                    sw.Close();
                    return (2);
                }

                // print header
                arsiz = 0;
                for (i = 0; i < NPTR; i++)
                {
                    arsiz += cpsiz[i];
                }
                if (arsiz == 0) {
                    sw.Close();
                    return (3);
                }

                sw.Write("// cha_data.h - array size = " + arsiz + " bytes\n");
                for (i = 0; i < hdsz; i++)
                {
                    sw.Write(head[i] + "\n");
                }

                // initialize ptr arrays
                ptsiz = 0;
                for (i = 0; i < NPTR; i++) {
                    if (cp[i]!=IntPtr.Zero) ptsiz = i + 1;
                }
                for (i = 0; i < ptsiz; i++) {
                    System.Console.WriteLine("i=" + i + ", cpsiz[i]=" + cpsiz[i]);
                    if (i == _size) {
                        arlen = cpsiz[i] / SIZEOFLONG;
                        arsiz = 0;
                        ulptr = (uint *) cp[i];
                        if (ulptr!=null) {
                            for (j = 0; j < arlen; j++) {                                
                                if (ulptr[j]!=0) arsiz = j + 1;
                            }
                        }
                        //fprintf(fp, "static CHA_DATA p%02d[%8d] = { // _size\n", i, arlen);
                        sw.Write(System.String.Format("static CHA_DATA p{0,2}[{1,8}]", i.ToString("D2"), arlen.ToString("D")) + " = { // _size\n");
                        for (j = 0; j < arsiz; j++) {
                            if ((j % arpl) == 0) sw.Write("        "); //fprintf(fp, "        ");
                            //fprintf(fp, "%10lu", ulptr[j]);
                            sw.Write(System.String.Format("{0,10}", ulptr[j]));
                            if (j < (arsiz - 1)) sw.Write(",");  //fprintf(fp, ",");
                            if ((j % arpl) == (arpl - 1)) sw.Write("\n"); //fprintf(fp, "\n");                                
                        }
                        if ((j % arpl) != 0) sw.Write("\n"); //fprintf(fp, "\n");
                        //fprintf(fp, "};\n");
                        sw.Write("};\n");
                        } else if (i == _ivar) {
                        arlen = cpsiz[i] / sizeof(int);
                        arsiz = 1;
                        for (j = 1; j < arlen; j++) {
                            if (CHA_IVAR[j]!=0) arsiz = j + 1;
                        }
                        //fprintf(fp, "static CHA_DATA p%02d[%8d] = { // _ivar\n", i, arlen);
                        sw.Write(System.String.Format("static CHA_DATA p{0,2}[{1,8}]", i.ToString("D2"), arlen.ToString("D")) + " = { // _ivar\n");
                        for (j = 0; j < arsiz; j++) {
                            if ((j % ivpl) == 0) sw.Write("        "); //fprintf(fp, "        ");
                            //fprintf(fp, "%10d", CHA_IVAR[j]);
                            sw.Write(System.String.Format("{0,10}", CHA_IVAR[j].ToString("D")));
                            if (j < (arsiz - 1)) sw.Write(","); //fprintf(fp, ",");
                            if ((j % ivpl) == (ivpl - 1)) sw.Write("\n"); //fprintf(fp, "\n");
                        }
                        if ((j % ivpl) != 0) sw.Write("\n"); //fprintf(fp, "\n");
                        sw.Write("};\n"); //fprintf(fp, "};\n");
                    } else if (i == _dvar) {
                        arlen = cpsiz[i] / sizeof(double);
                        arsiz = 1;
                        for (j = 1; j < arlen; j++) {
                            if (CHA_DVAR[j]!=0) arsiz = j + 1;
                        }
                        sw.Write(System.String.Format("static double   p{0,2}[{1,8}]", i.ToString("D2"), arlen.ToString("D")) + " = { // _dvar\n");
                        //fprintf(fp, "static double   p%02d[%8d] = { // _dvar\n", i, arlen);
                        for (j = 0; j < arsiz; j++) {
                            if ((j % dvpl) == 0) sw.Write("        "); //fprintf(fp, "        ");
                            sw.Write(System.String.Format("{0,15:9}", CHA_DVAR[j].ToString("G"))); //fprintf(fp, "%15.9g", CHA_DVAR[j]);
                            if (j < (arsiz - 1)) sw.Write(","); //fprintf(fp, ",");
                            if ((j % dvpl) == (dvpl - 1)) sw.Write("\n"); //fprintf(fp, "\n");
                        }
                        if ((j % dvpl) != 0) sw.Write("\n"); //fprintf(fp, "\n");
                        sw.Write("};\n"); //fprintf(fp, "};\n");
                    } else if (cpsiz[i] == 0) {
                        sw.Write(System.String.Format("// empty array ->     p%{0,2}\n", i.ToString("D2")));//fprintf(fp, "// empty array ->     p%02d\n", i);
                    } else if ((cpsiz[i] % SIZEOFLONG) == 0) {
                        arlen = cpsiz[i] / SIZEOFLONG;
                        arsiz = 0;
                        ulptr = (uint *) cp[i];
                        if (ulptr!=null) {
                            for (j = 0; j < arlen; j++) {
                                if (ulptr[j]!=0) arsiz = j + 1;
                            }
                        }
                        if (arsiz < 2) {
                            sw.Write(System.String.Format("static CHA_DATA p{0,2}[{1,8}] = ", i.ToString("D2"), arlen.ToString("D")) + "{" + System.String.Format("{0,10}", ulptr[0].ToString("D")) + "};\n");
                            //fprintf(fp, "static CHA_DATA p%02d[%8d] = {%10lu};\n",i, arlen, ulptr[0]);
                        } else {
                            sw.Write(System.String.Format("static CHA_DATA p{0,2}[{1,8}]", i.ToString("D2"), arlen.ToString("D")) + " = {\n");
                            //fprintf(fp, "static CHA_DATA p%02d[%8d] = {\n", i, arlen);
                            for (j = 0; j < arsiz; j++) {
                                if ((j % arpl) == 0) sw.Write("        "); //fprintf(fp, "        ");
                                sw.Write(System.String.Format("0x{0,8}", ulptr[j].ToString("X8")));
                                //fprintf(fp, "0x%08lX", ulptr[j]);
                                if (j < (arsiz - 1)) sw.Write(","); //fprintf(fp, ",");
                                if ((j % arpl) == (arpl - 1)) sw.Write("\n"); //fprintf(fp, "\n");
                            }
                            if ((j % arpl) != 0) sw.Write("\n"); //fprintf(fp, "\n");
                            sw.Write("};\n"); //fprintf(fp, "};\n");
                        }
                    } else if ((cpsiz[i] % sizeof(short)) == 0) {
                        arlen = cpsiz[i] / sizeof(short);
                        sw.Write("// NOTE: Only zero data implemented unless size%%4==0.");
                        //fprintf(fp, "// NOTE: Only zero data implemented unless size%%4==0.\n");
                        sw.Write(System.String.Format("static unsigned short p{0,2}[(1,8)] = ;", i.ToString("D2"), arlen.ToString("D")) + "{0}\n");
                        //fprintf(fp, "static unsigned short p%02d[%8d] = {0};\n", i, arlen);
                    } else {
                        arlen = cpsiz[i];
                        sw.Write("// NOTEL: Only zero data implemented unless size%%4==0.");
                        //fprintf(fp, "// NOTEL Only zero data implemented unless size%%4==0.\n");
                        sw.Write(System.String.Format("static unsigned char  p{0,2}[{1,8}] = ;", i.ToString("D2"), arlen.ToString("D")) + "{0}\n");
                        //fprintf(fp, "static unsigned char  p%02d[%8d] = {0};\n", i, arlen);
                    }
                }
                sw.Write("\n"); //fprintf(fp, "\n");
                // initialize ptr
                if (ptsiz < 1) {
                    sw.Write("static CHA_DATA *cha_data[NPTR] = {0};\n");
                    //fprintf(fp, "static CHA_DATA *cha_data[NPTR] = {0};\n");
                } else {
                    sw.Write("static CHA_DATA *cha_data[NPTR] = {\n");
                    //fprintf(fp, "static CHA_DATA *cha_data[NPTR] = {\n");
                    sw.Write("    ");
                    //fprintf(fp, "    ");
                    for (i = 0; i < _reserve; i++) {
                        sw.Write(System.String.Format("(CHA_DATA *)p{0,2},", i.ToString("D2")));
                        //fprintf(fp, "(CHA_DATA *)p%02d,", i);
                    }
                    sw.Write("\n"); //fprintf(fp, "\n");
                    int k=0;
                    for (i = _reserve; i < ptsiz; i++) {
                        j = i - _reserve;
                        if ((j % ptpl) == 0) sw.Write("    "); //fprintf(fp, "    ");
                        if (cpsiz[i] == 0) {
                            sw.Write("NULL"); //fprintf(fp, "NULL");
                        } else {
                            sw.Write(System.String.Format(" p{0,2}", i.ToString("D2"))); //fprintf(fp, " p%02d", i);
                        }
                        if (i < (ptsiz - 1)) sw.Write(","); //fprintf(fp, ",");
                        if ((j % ptpl) == (ptpl - 1)) sw.Write("\n"); //fprintf(fp, "\n");
                        k = j;
                    }
                    j = k;
                    if ((j % ptpl) != (ptpl - 1)) sw.Write("\n"); //fprintf(fp, "\n");
                    sw.Write("};\n"); //fprintf(fp, "};\n");
                }
                // print trailer
                for (i = 0; i < tlsz; i++) {
                    sw.Write(tail[i] + "\n");
                    //fprintf(fp, "%s\n", tail[i]);
                }
                //fclose(fp);
                sw.Close();
                
            }
            return (0);


        }
    }
    
}
