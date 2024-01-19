using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace z2
{
    public partial class SystemForm : Form
    {
        Database database = new Database();
        private readonly checkUser _user;
        public SystemForm(checkUser user)
        {
            _user = user;
            InitializeComponent();
        }

        private void IsAdmin ()
        {
            addBut.Enabled = _user.IsAdmin;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SystemForm_Load(object sender, EventArgs e)
        {
            IsAdmin();
            loginLabel.Text = $"{_user.Login}:{_user.Status}";
        }

        private void addBut_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            NatPersonForm npForm = new NatPersonForm(_user);
            npForm.Show();
            Hide();
        }

        private void loginLabel_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            EduProgramForm edpForm = new EduProgramForm(_user);
            edpForm.Show();
            Hide();
        }
    }
}
