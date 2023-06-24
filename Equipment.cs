using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Pure_Fitness
{
    public partial class Equipment : Form
    {
        public Equipment()
        {
            InitializeComponent();
        }

        
        private void btnReset_Click(object sender, EventArgs e)
        {
            txtEquipName.Clear();
            txtPurpose.Clear();
            txtMusclesUsed.Clear();
            txtCost.Clear();
            dateTimePickerDeliveryDate.Value = DateTime.Now;
        }

        private void btnViewEq_Click(object sender, EventArgs e)
        {
            ViewEquipment ve = new ViewEquipment();

            ve.Show();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string equipName = txtEquipName.Text.Trim();
                string purpose = txtPurpose.Text.Trim();
                string musclesUsed = txtMusclesUsed.Text.Trim();
                string costStr = txtCost.Text.Trim();
                string deliveryDate = dateTimePickerDeliveryDate.Text.Trim();

                // Check for missing input data
                if (string.IsNullOrEmpty(equipName) || string.IsNullOrEmpty(purpose) ||
                    string.IsNullOrEmpty(musclesUsed) || string.IsNullOrEmpty(costStr) ||
                    string.IsNullOrEmpty(deliveryDate))
                {
                    MessageBox.Show("Please fill in all the required fields.", "Incomplete Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Stop further execution
                }

                if (!decimal.TryParse(costStr, out decimal cost))
                {
                    throw new ArgumentException("Invalid cost value. Please enter a valid number.");
                }

                SqlConnection con = new SqlConnection();
                con.ConnectionString = "data source=DESKTOP-J1E1JGM\\SQLEXPRESS; database=gym; integrated security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "SET IDENTITY_INSERT Equipment ON; " + // Disable identity specification for the table
                                  "INSERT INTO Equipment (Equipment_ID, Equipment_Name, Purpose, Muscle_Used, Cost, Delivery_Date) " +
                                  "VALUES (@Equipment_ID, @Equipment_Name, @Purpose, @Muscle_Used, @Cost, @Delivery_Date); " +
                                  "SET IDENTITY_INSERT Equipment OFF;"; // Re-enable identity specification

                cmd.Parameters.AddWithValue("@Equipment_ID", GetMaxEquipmentID() + 1); // Generate new Equipment ID
                cmd.Parameters.AddWithValue("@Equipment_Name", equipName);
                cmd.Parameters.AddWithValue("@Purpose", purpose);
                cmd.Parameters.AddWithValue("@Muscle_Used", musclesUsed);
                cmd.Parameters.AddWithValue("@Cost", cost);
                cmd.Parameters.AddWithValue("@Delivery_Date", deliveryDate);

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


        private int GetMaxEquipmentID()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "data source=DESKTOP-J1E1JGM\\SQLEXPRESS; database=gym; integrated security=True";
            SqlCommand cmd = new SqlCommand("SELECT MAX(Equipment_ID) FROM Equipment", con);
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

    }
}
