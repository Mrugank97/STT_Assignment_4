using System;
using System.Windows.Forms;

namespace AlarmClockWinForms
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1()); // ✅ This line launches your form
        }
    }
}
