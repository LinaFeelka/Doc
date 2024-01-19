using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;

namespace z2
{
    public partial class LoginForm : Form
    {
        Database database = new Database();
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            Hide();
        }

       

        private void loginBut_Click(object sender, EventArgs e)
        {
            var login = loginTB.Text;
            var password = passwordTB.Text;

            string querystring = $"insert into users (login, password, id_role) values ('{login}','{password}', '3')";

            NpgsqlCommand command = new NpgsqlCommand(querystring, database.GetConnection());

            database.OpenConnection();



            if (checkuser()) 
                return;

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт успешно создан!", "Успех!");
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Аккаунт создать не удалось!");
            }
            database.CloseConnection();

            //if (IsLetter(passwordTB) == false || IsNumber(passwordTB) == false || IsSymbol(passwordTB)) { MessageBox.Show("Пароль не отвечает требованиям"); }
        }

        private Boolean checkuser()
        {
            var login = loginTB.Text;
            var password = passwordTB.Text;

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"select id, id_role, login, password from users where login = '{login}' and password = '{password}'  ";

            NpgsqlCommand command = new NpgsqlCommand(querystring, database.GetConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Пользователь уже существует!");
                return true;
            }
            else
            {
                return false;
            }
        }
        bool IsLetter(TextBox text)
        {
            foreach (var item in text.Text.Reverse())
            {
                if (text.TextLength >= 5)
                {
                    if (char.IsLetter(item)) { return true; }
                }
            }
            return false;
        }
        bool IsNumber(TextBox text)
        {
            foreach (var item in text.Text.Reverse())
            {
                if (text.TextLength >= 8)
                {
                    if (char.IsNumber(item)) { return true; }
                }
            }
            return false;
        }
        bool IsSymbol(TextBox text)
        {
            foreach (var item in text.Text.Reverse())
            {   
                    if (char.IsSymbol(item)) { return true; }
            }
            return false;
        }
        private void passwordTB_TextChanged(object sender, EventArgs e)
        {
            if (IsLetter(passwordTB))
            {
                labelLetter.ForeColor = Color.Green;
            } else { 
                labelLetter.ForeColor = Color.Red;
            }

            if (IsNumber(passwordTB))
            {
                labelNumber.ForeColor = Color.Green;
            }
            else
            {
                labelNumber.ForeColor = Color.Red;
            }

            if (IsSymbol(passwordTB))
            {
                labelSymbol.ForeColor = Color.Green;
            }
            else
            {
                labelSymbol.ForeColor = Color.Red;
            }

            
            
        }
    }
}
