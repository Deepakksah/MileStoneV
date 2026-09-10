using System;
using System.Windows.Forms;
using MilestoneLicenseGenerator.UI;

namespace MilestoneLicenseGenerator
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LicenseGeneratorForm());
        }
    }
}
