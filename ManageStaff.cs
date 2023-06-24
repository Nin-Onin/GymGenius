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
    public partial class ManageStaff : Form
    {
        public ManageStaff()
        {
            InitializeComponent();
            RefreshDataGridView();
        }










        private void RefreshDataGridView()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "data source = DESKTOP-J1E1JGM\\SQLEXPRESS; database = gym; integrated security = True";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            cmd.CommandText = "SELECT * FROM NewStaff ORDER BY Staff_ID"; // Add ORDER BY clause to sort by MID
            SqlDataAdapter DA = new SqlDataAdapter(cmd);
            DataSet DS = new DataSet();
            DA.Fill(DS);

            dataGridView1.DataSource = DS.Tables[0];

            // Make column headers bold and centered
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.Font = new Font(dataGridView1.Font.FontFamily, 14, FontStyle.Bold);
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Set font size of all data values
            DataGridViewCellStyle dataCellStyle = new DataGridViewCellStyle();
            dataCellStyle.Font = new Font(dataGridView1.Font.FontFamily, 10);

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.HeaderCell.Style = headerStyle;
                column.DefaultCellStyle = dataCellStyle; // Set font size of data values
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Center align data values
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // Set auto-sizing mode
            }

            // Set the background color of column headers
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#11F522");

            // Bold the data value under "Member_ID" column
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "Staff_ID")
            {
                e.CellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            }
        }






        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != "")
            {
                int sid;
                if (int.TryParse(txtSearch.Text, out sid))
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = "data source = DESKTOP-J1E1JGM\\SQLEXPRESS; database = gym; integrated security = True";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;

                    cmd.CommandText = "DELETE FROM NewStaff WHERE Staff_ID = @Staff_ID";
                    cmd.Parameters.AddWithValue("@Staff_ID", sid);

                    try
                    {
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record deleted successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Refresh the data in the DataGridView
                            RefreshDataGridView();

                            // Auto-sort the MID column
                            dataGridView1.Sort(dataGridView1.Columns["Staff_ID"], ListSortDirection.Ascending);
                        }
                        else
                        {
                            MessageBox.Show("No record found with the specified Staff_ID.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid ID. Please enter a valid integer value.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please enter an ID.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            if (!int.TryParse(txtSearch.Text, out int searchId))
            {
                MessageBox.Show("Please enter a valid ID.");
                return;
            }

            SqlConnection con = new SqlConnection();
            con.ConnectionString = "data source = DESKTOP-J1E1JGM\\SQLEXPRESS; database = gym; integrated security = True";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            cmd.CommandText = "SELECT * FROM NewStaff ORDER BY Staff_ID";
            SqlDataAdapter DA = new SqlDataAdapter(cmd);
            DataSet DS = new DataSet();

            try
            {
                DA.Fill(DS);

                if (DS.Tables[0].Rows.Count > 0)
                {
                    bool memberFound = false;

                    // Clear any existing selection
                    dataGridView1.ClearSelection();

                    // Highlight the rows with the matching Member_ID
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        int memberId = Convert.ToInt32(row.Cells["Staff_ID"].Value);
                        if (memberId == searchId)
                        {
                            row.Selected = true;
                            dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
                            memberFound = true;
                            break;
                        }
                    }

                    if (!memberFound)
                    {
                        MessageBox.Show("ID not found in the table.");
                    }

                    // Auto-size the columns to fit the data
                    dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
                else
                {
                    // No data in the table
                    MessageBox.Show("No data found in the table.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }


        }
    }
}