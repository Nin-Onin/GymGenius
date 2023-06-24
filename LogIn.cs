using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Pure_Fitness
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
            passwordTxt.PasswordChar = '*'; // Set PasswordChar property to '*'
        }

        private void logInBtn_Click(object sender, EventArgs e)
        {
            string username = usernameTxt.Text;
            string password = passwordTxt.Text;

            // Check if the username and password fields are empty
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate the username and password
            if (ValidateLogin(username, password))
            {
                // Login successful, proceed to the Dashboard
                Dashboard dashboard = new Dashboard();
                dashboard.Show();
                this.Hide();
            }
            else
            {
                // Invalid username or password
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateLogin(string username, string password)
        {
            using (SqlConnection con = new SqlConnection("data source=DESKTOP-J1E1JGM\\SQLEXPRESS; database=gym; integrated security=True"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Login WHERE Username = @Username AND Password = @Password", con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
    }
}
