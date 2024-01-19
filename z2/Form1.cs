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
using Npgsql.Internal;

namespace z2
{
    public partial class Form1 : Form
    {
        Database database = new Database();
        public Form1()
        {
            InitializeComponent();

            //passAgainBut.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            Hide();
        }

        private void enterBut_Click(object sender, EventArgs e)
        {
            database.OpenConnection();

            var login = loginTB.Text;
            var password = passwordTB.Text;

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            DataTable table = new DataTable();

            var query = $"select id, login, password, id_role from users where login = '{login}' and password = '{password}'";

            NpgsqlCommand comm = new NpgsqlCommand(query, database.GetConnection());

            adapter.SelectCommand = comm;
            adapter.Fill(table);
                                    
            if (table.Rows.Count == 1)
            {
                var user = new checkUser(table.Rows[0].ItemArray[1].ToString(), Convert.ToBoolean(table.Rows[0].ItemArray[3]));

                MessageBox.Show("Вы успешно вошли!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SystemForm systemForm = new SystemForm(user);
                this.Hide();
                systemForm.Show();
            }
            else
            {
                MessageBox.Show("Такого аккаунта не существует!", "Аккаунта не существует!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }  
            database.CloseConnection();
        }
                
        private void passAgainBut_Click(object sender, EventArgs e)
        {
            DropPassForm dpForm = new DropPassForm();
            dpForm.Show();
            Hide();
        }
    }
}
