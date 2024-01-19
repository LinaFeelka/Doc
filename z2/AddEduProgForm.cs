using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace z2
{
    public partial class AddEduProgForm : Form
    {
        private readonly checkUser _user;
        Database database = new Database();
        public AddEduProgForm(checkUser user)
        {
            _user = user;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            NatPersonForm npForm = new NatPersonForm(_user);
            npForm.Show();
            Hide();
        }

        private void AddEduProgForm_Load(object sender, EventArgs e)
        {
            loginLabel.Text = $"{_user.Login}:{_user.Status}";
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            database.OpenConnection();

            var name = textBox2.Text;
            var study_period = textBox3.Text;
            var qualification = textBox4.Text;
            var study_cost = textBox5.Text;

            string addQuery = $"insert into edu_program (name, study_period, qualification, study_cost) values ('{name}', '{study_period}', '{qualification}', '{study_cost}')";

            NpgsqlCommand command = new NpgsqlCommand(addQuery, database.GetConnection());
            command.ExecuteNonQuery();

            MessageBox.Show("Запись успешно добавлена", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            database.CloseConnection();

            NatPersonForm npForm = new NatPersonForm(_user);
            npForm.Show();
            Hide();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
