using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace z2
{
    public partial class AddForm : Form
    {
        private readonly checkUser _user;
        Database database = new Database();
       
        public AddForm(checkUser user)
        {
            _user = user;
            InitializeComponent();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            NatPersonForm npForm = new NatPersonForm(_user);
            npForm.Show();
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AddForm_Load(object sender, EventArgs e)
        {
            loginLabel.Text = $"{_user.Login}:{_user.Status}";
        }

            
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            database.OpenConnection();

            var last_name = textBox2.Text;
            var name = textBox3.Text;
            var middle_name = textBox4.Text;
            var birthday = textBox5.Text;
            var passport = textBox6.Text;
            var home_adress = textBox7.Text;
            var e_mail = textBox8.Text;
            var phone = textBox9.Text;
            var position = textBox10.Text;
            var work_adress = textBox11.Text;

            string addQuery = $"insert into nature_person (last_name, name, middle_name, birthday, passport, home_adress, e_mail, phone, position, work_adress) values ('{last_name}', '{name}', '{middle_name}', '{birthday}', '{passport}', '{home_adress}', '{e_mail}', '{phone}', '{position}', '{work_adress}')";

            NpgsqlCommand command = new NpgsqlCommand(addQuery, database.GetConnection());
            command.ExecuteNonQuery();

            MessageBox.Show("Запись успешно добавлена", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            database.CloseConnection();

            EduProgramForm epForm = new EduProgramForm(_user);
            epForm.Show();
            Hide();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox6.Text, "[^0-9]"))
            {
                MessageBox.Show("Вводите только цифры!");
                textBox6.Text = textBox6.Text.Remove(textBox6.Text.Length - 1);
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox9.Text, "[^0-9]"))
            {
                MessageBox.Show("Вводите только цифры!");
                textBox9.Text = textBox9.Text.Remove(textBox9.Text.Length - 1);
            }
        }
    }
}
