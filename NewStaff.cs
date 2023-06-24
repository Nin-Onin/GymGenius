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
    public partial class NewStaff : Form
    {
        public NewStaff()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string fname = txtFName.Text.Trim();
            string lname = txtLname.Text.Trim();
            string gender = radioButton1.Checked ? radioButton1.Text : radioButton2.Text;
            string dob = dateTimePickerDOB.Text.Trim();
            string mobileStr = txtMobile.Text.Trim();
            string email = txtEmail.Text.Trim();
            string gymrole = txtGymRole.Text.Trim();
            string joindate = dateTimePickerJOINDate.Text.Trim();

            // Check for missing input data
            if (string.IsNullOrEmpty(fname) || string.IsNullOrEmpty(lname) || string.IsNullOrEmpty(dob) || string.IsNullOrEmpty(gender) ||
                string.IsNullOrEmpty(mobileStr) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(gymrole) ||
                string.IsNullOrEmpty(joindate))
            {
                MessageBox.Show("Please fill in all the required fields.", "Incomplete Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Stop further execution
            }

            if (!Int64.TryParse(mobileStr, out Int64 mobile))
            {
                MessageBox.Show("Invalid mobile number. Please enter a valid number.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Stop further execution
            }

            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = "data source=DESKTOP-J1E1JGM\\SQLEXPRESS; database=gym; integrated security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "SET IDENTITY_INSERT NewStaff ON; " + // Disable identity specification for the table
                    "INSERT INTO NewStaff (Staff_ID, First_Name, Last_Name, Gender, Date_Of_Birth, Mobile_Number, Email_Address, Join_Date, Gym_Role) " +
                    "VALUES (@Staff_ID, @First_Name, @Last_Name, @Gender, @Date_Of_Birth, @Mobile_Number, @Email_Address, @Join_Date, @Gym_Role); " +
                    "SET IDENTITY_INSERT NewStaff OFF;"; // Re-enable identity specification

                cmd.Parameters.AddWithValue("@Staff_ID", GetMaxStaffID() + 1); // Generate new Staff ID
                cmd.Parameters.AddWithValue("@First_Name", fname);
                cmd.Parameters.AddWithValue("@Last_Name", lname);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Date_Of_Birth", dob);
                cmd.Parameters.AddWithValue("@Mobile_Number", mobile);
                cmd.Parameters.AddWithValue("@Email_Address", email);
                cmd.Parameters.AddWithValue("@Join_Date", joindate);
                cmd.Parameters.AddWithValue("@Gym_Role", gymrole); 
                //cmd.Parameters.AddWithValue("@Staff_ID". GetMaxStaffID() + 1) //description into Max Value s

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

        private int GetMaxStaffID()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "data source=DESKTOP-J1E1JGM\\SQLEXPRESS; database=gym; integrated security=True";
            SqlCommand cmd = new SqlCommand("SELECT MAX(Staff_ID) FROM NewStaff", con);
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

        private void button1_Click(object sender, EventArgs e)
        {
            txtFName.Clear();
            txtLname.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            txtMobile.Clear();
            txtGymRole.Clear();
            dateTimePickerDOB.Value = DateTime.Now;
            dateTimePickerJOINDate.Value = DateTime.Now;
            txtEmail.Clear();
        }
    }
}
