using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Pure_Fitness
{
    public partial class Payment : Form
    {
        private List<ComboBoxItem> memberList; // List to store ComboBoxItems

        public Payment()
        {
            InitializeComponent();
            memberList = new List<ComboBoxItem>();
            LoadMemberData();

            // Attach the event handler to the SelectedIndexChanged event of comboBoxMemberName
            comboBoxMemberName.SelectedIndexChanged += ComboBoxMemberName_SelectedIndexChanged;

            // Set the txtCharge TextBox to read-only
            txtCharge.ReadOnly = true;
        }

        private void ComboBoxMemberName_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected item from comboBoxMemberName
            ComboBoxItem selectedItem = (ComboBoxItem)comboBoxMemberName.SelectedItem;

            // Get the membership time from the selected item
            string membershipTime = selectedItem.MembershipTime;

            // Set the equivalent amount based on the membership time
            decimal amount = 0;
            switch (membershipTime)
            {
                case "SESSION ONLY":
                    amount = 80;
                    break;
                case "1 MONTH":
                    amount = 2000;
                    break;
                case "2 MONTHS":
                    amount = 4000;
                    break;
                case "6 MONTHS":
                    amount = 10000;
                    break;
                case "12 MONTHS":
                    amount = 15000;
                    break;
            }

            // Display the equivalent amount in txtCharge
            txtCharge.Text = amount.ToString();
        }

        private void LoadMemberData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection("data source = DESKTOP-J1E1JGM\\SQLEXPRESS; database = gym; integrated security = True"))
                {
                    con.Open();
                    string query = "SELECT Member_ID, First_Name, Last_Name, Membership_Time FROM NewMember ORDER BY Member_ID";
                    SqlCommand cmd = new SqlCommand(query, con);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int memberId = reader.GetInt32(0);
                            string firstName = reader.GetString(1);
                            string lastName = reader.GetString(2);
                            string membershipTime = reader.GetString(3);

                            string memberInfo = $"   {memberId}:                            {firstName}                              {lastName}                    {membershipTime}";

                            ComboBoxItem comboBoxItem = new ComboBoxItem(memberInfo, memberId.ToString(), membershipTime);
                            memberList.Add(comboBoxItem);
                        }
                    }
                }

                // Populate the ComboBox with the stored member data
                comboBoxMemberName.DataSource = memberList;
                comboBoxMemberName.DisplayMember = "Text"; // Display the Text property
                comboBoxMemberName.ValueMember = "Tag"; // Use the Tag property as the underlying value
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                MessageBox.Show("An error occurred while retrieving member data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateReceiptNumber()
        {
            Random random = new Random();
            int firstPart = random.Next(100, 999); // Generate a random number between 100 and 999
            int secondPart = random.Next(100, 999); // Generate a random number between 1000 and 9999
            string receiptNumber = $"{firstPart}-{secondPart}";

            // Truncate the receipt number if it exceeds 9 characters
            if (receiptNumber.Length > 9)
            {
                receiptNumber = receiptNumber.Substring(0, 9);
            }

            return receiptNumber;
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            if (comboBoxMemberName.SelectedItem == null)
            {
                MessageBox.Show("Please select a member.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtAmount.Text))
            {
                MessageBox.Show("Please enter the payment amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amountPaid))
            {
                MessageBox.Show("Invalid payment amount. Please enter a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                decimal charge = decimal.Parse(txtCharge.Text);
                decimal balance = amountPaid - charge;

                if (balance >= 0)
                {
                    string receiptNumber = GenerateReceiptNumber();

                    // Check if the same member has already made a payment
                    using (SqlConnection con = new SqlConnection("data source = DESKTOP-J1E1JGM\\SQLEXPRESS; database = gym; integrated security = True"))
                    {
                        con.Open();

                        // Prepare the query
                        string query = "SELECT COUNT(*) FROM Payment WHERE Member_ID = @Member_ID";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@Member_ID", ((ComboBoxItem)comboBoxMemberName.SelectedItem).Tag);

                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("The selected member has already made a payment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Payment is sufficient
                    MessageBox.Show($"Payment successful! Balance: {balance:C2}\nReceipt Number: {receiptNumber}", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Store the payment details in the database
                    using (SqlConnection con = new SqlConnection("data source = DESKTOP-J1E1JGM\\SQLEXPRESS; database = gym; integrated security = True"))
                    {
                        con.Open();

                        // Prepare the query
                        string query = "INSERT INTO Payment (Receipt_Number, Amount, Balance, Payment_Date, Member_ID) VALUES (@Receipt_Number, @Amount, @Balance, @Payment_Date, @Member_ID)";
                        SqlCommand cmd = new SqlCommand(query, con);

                        // Set the parameter values
                        cmd.Parameters.AddWithValue("@Receipt_Number", receiptNumber);
                        cmd.Parameters.AddWithValue("@Amount", amountPaid);
                        cmd.Parameters.AddWithValue("@Balance", balance);
                        cmd.Parameters.AddWithValue("@Payment_Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@Member_ID", ((ComboBoxItem)comboBoxMemberName.SelectedItem).Tag);

                        // Execute the query
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Payment is insufficient
                    MessageBox.Show($"Insufficient payment amount! Balance: {balance:C2}", "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during payment: " + ex.Message, "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public class ComboBoxItem
        {
            public string Text { get; set; }
            public string Tag { get; set; }
            public string MembershipTime { get; set; }

            public ComboBoxItem(string text, string tag, string membershipTime)
            {
                Text = text;
                Tag = tag;
                MembershipTime = membershipTime;
            }

            public override string ToString()
            {
                return Text;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            comboBoxMemberName.ResetText();
            dateTimePickerJoinDate.Value = DateTime.Now;
            txtCharge.Clear();
            txtAmount.Clear();
        }

        private void btnReceipt_Click(object sender, EventArgs e)
        {
            Receipt ss = new Receipt();
            ss.Show();
        }
    }
}