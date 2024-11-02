using System;
using System.Windows.Forms;

namespace TheInfiniteBlack2CraftingCalculator
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}