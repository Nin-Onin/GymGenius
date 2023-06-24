using System;
using System.Windows.Forms;

namespace Pure_Fitness
{
    public partial class LogOut : Form
    {
        private Dashboard dashboard; // Reference to the Dashboard form

        public LogOut(Dashboard dashboard)
        {
            InitializeComponent();
            this.dashboard = dashboard;
        }

        private void yesBtn_Click(object sender, EventArgs e)
        {
            dashboard.Close(); // Close the Dashboard form
            this.Close(); // Close the LogOut form
            LogIn me = new LogIn();
            me.Show();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
