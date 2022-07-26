using System;
using System.Windows.Forms;

namespace TaskComponent
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)

        {
            if (args.Length > 0)
            {  
                //Start Any other process here. It will be executed whenever command line arguments are provided to the program when launching the application from command line/propmt.
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
           
        }
    }
}
