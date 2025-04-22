using System;
using System.Drawing;
using System.Windows.Forms;

namespace AlarmClockWinForms
{
    public partial class Form1 : Form
    {
        private DateTime targetTime; // Stores the alarm time
        private Random random = new Random(); // For background color changes

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // Try to parse time from the textbox input
            if (DateTime.TryParse(txtTime.Text, out targetTime))
            {
                // Set the full datetime with today's date
                targetTime = DateTime.Today.Add(targetTime.TimeOfDay);

                // Start the timer (tick every 1 second)
                timer1.Start();
            }
            else
            {
                MessageBox.Show("Invalid time format! Please enter in HH:MM:SS format.", "Input Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Change the background color randomly every second
            this.BackColor = Color.FromArgb(
                random.Next(256),
                random.Next(256),
                random.Next(256)
            );

            // Check if current system time has reached the target
            if (DateTime.Now >= targetTime)
            {
                timer1.Stop(); // Stop changing colors
                MessageBox.Show("⏰ Time's up! Alarm triggered!", "Alarm",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
