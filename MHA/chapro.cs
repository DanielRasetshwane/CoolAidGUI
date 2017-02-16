using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

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
        
        [DllImport("chapro.dll")]//, EntryPoint = "cha_agc_prepare", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cha_agc_prepare(void* cp, ref IntPtr dsl, ref CHA_WDRC gha);
        
        [DllImport("chapro.dll", EntryPoint = "cha_data_gen", CallingConvention = CallingConvention.StdCall)]
        public static extern int cha_data_gen(void* cp, string fn);        

        #endregion
    }

    



}
