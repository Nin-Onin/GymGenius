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
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        Boolean b = true;
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (b == true)
            {
                menuStrip1.Dock = DockStyle.Left;
                b = false;
                toolStripMenuItem1.Image = Image.FromFile(@"D:\FILES\Desktop\Software Development\GymGenius - System Application\Assets\fRA.png");
            }
            else
            {
                menuStrip1.Dock = DockStyle.Top;
                b = true;
                toolStripMenuItem1.Image = Image.FromFile(@"D:\FILES\Desktop\Software Development\GymGenius - System Application\Assets\fDA.png");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripMenuItem1.Image = Image.FromFile(@"D:\FILES\Desktop\Software Development\GymGenius - System Application\Assets\fDA.png");
        }

        private void newMeberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewMember nm = new NewMember();
            nm.Show();
        }

        private void newStaffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewStaff ns = new NewStaff();
            ns.Show();
        }

        private void equipmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Equipment equip = new Equipment();
            equip.Show();
        }

        private void searchMemberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageMember mm = new ManageMember();
            mm.Show();
        }

        private void deleteMemberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageStaff ms = new ManageStaff();
            ms.Show();
        }

        private void paymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Payment pay = new Payment();
            pay.Show();
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogOut lo = new LogOut(this); // Pass the Dashboard form reference to LogOut form
            lo.Show();
        }
    }
}
