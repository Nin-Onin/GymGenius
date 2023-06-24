using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pure_Fitness
{
    public partial class Receipt : Form
    {
        public string ReceiptNumber { get; set; }
        public string PaymentDate { get; set; }
        public string FirstName { get; set; }
        public string MembershipTime { get; set; }
        public decimal Charge { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance { get; set; }

        public Receipt()
        {
            InitializeComponent();
        }

        

        private void btnPay_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
