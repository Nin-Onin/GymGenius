using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pure_Fitness
{
    public partial class NewMember : Form
    {
        public NewMember()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string fname = txtFirstName.Text.Trim();
                string lname = txtLastName.Text.Trim();
                string gender = radioButton1.Checked ? radioButton1.Text : radioButton2.Text;
                string dob = dateTimePickerDOB.Text.Trim();
                string mobileStr = txtMobile.Text.Trim();
                string email = txtEmail.Text.Trim();
                string joindate = dateTimePickerJoinDate.Text.Trim();
                string address = txtAddress.Text.Trim();
                string gymTime = comboBoxGameTime.Text.Trim();
                string gymTrainer = comboBoxTrainer.Text.Trim();
                string membership = comboBoxMembership.Text.Trim();

                // Check for missing input data
                if (string.IsNullOrEmpty(fname) || string.IsNullOrEmpty(lname) || string.IsNullOrEmpty(gender) ||
                    string.IsNullOrEmpty(dob) || string.IsNullOrEmpty(mobileStr) || string.IsNullOrEmpty(email) ||
                    string.IsNullOrEmpty(joindate) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(gymTime) || string.IsNullOrEmpty(gymTrainer) || string.IsNullOrEmpty(membership))
                {
                    MessageBox.Show("Please fill in all the required fields.", "Incomplete Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Stop further execution
                }

                if (!Int64.TryParse(mobileStr, out Int64 mobile))
                {
                    throw new ArgumentException("Invalid mobile number. Please enter a valid number.");
                }

                SqlConnection con = new SqlConnection();
                con.ConnectionString = "data source=DESKTOP-J1E1JGM\\SQLEXPRESS; database=gym; integrated security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "SET IDENTITY_INSERT NewMember ON; " + // Disable identity specification for the table
                    "INSERT INTO NewMember (Member_ID, First_Name, Last_Name, Gender, Mobile_Number, Date_Of_Birth,  Email_Address, Join_Date, Member_Address, Gym_Time, Gym_Trainer, Membership_Time) " +
                    "VALUES (@Member_ID, @First_Name, @Last_Name, @Gender,  @Mobile_Number, @Date_Of_Birth, @Email_Address, @Join_Date, @Member_Address, @Gym_Time, @Gym_Trainer, @Membership_Time); " +
                    "SET IDENTITY_INSERT NewMember OFF;"; // Re-enable identity specification

                cmd.Parameters.AddWithValue("@Member_ID", GetMaxMemberID() + 1); // Generate new Member ID
                cmd.Parameters.AddWithValue("@First_Name", fname);
                cmd.Parameters.AddWithValue("@Last_Name", lname);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Mobile_Number", mobile);
                cmd.Parameters.AddWithValue("@Date_Of_Birth", dob);
                cmd.Parameters.AddWithValue("@Email_Address", email);
                cmd.Parameters.AddWithValue("@Join_Date", joindate);
                cmd.Parameters.AddWithValue("@Member_Address", address);
                cmd.Parameters.AddWithValue("@Gym_Time", gymTime);
                cmd.Parameters.AddWithValue("@Gym_Trainer", gymTrainer);
                cmd.Parameters.AddWithValue("@Membership_Time", membership);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Data saved.", "Inserted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to save data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetMaxMemberID()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "data source=DESKTOP-J1E1JGM\\SQLEXPRESS; database=gym; integrated security=True";
            SqlCommand cmd = new SqlCommand("SELECT MAX(Member_ID) FROM NewMember", con);
            con.Open();
            object result = cmd.ExecuteScalar();
            con.Close();

            int maxID;
            if (result != DBNull.Value && result != null)
            {
                maxID = Convert.ToInt32(result);
            }
            else
            {
                maxID = 0;
            }

            return maxID;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;

            txtMobile.Clear();
            txtEmail.Clear();

            comboBoxGameTime.ResetText();
            comboBoxTrainer.ResetText();
            comboBoxMembership.ResetText();
            txtAddress.Clear();

            dateTimePickerDOB.Value = DateTime.Now;
            dateTimePickerJoinDate.Value = DateTime.Now;
        }

        
    }
}
