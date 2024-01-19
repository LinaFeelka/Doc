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

namespace z2
{
    public partial class DropPassForm : Form
    {
        Database database = new Database();
        public DropPassForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dropPass();
            MessageBox.Show("Пароль изменен!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Form1 form1 = new Form1();
            form1.Show();
            Hide();
        }

        private void dropPass()
        {
            var login = loginTB.Text;
            var password = passwordTB.Text;

            database.OpenConnection();

            var changeQuery = $"update users set password = '{password}' where login = '{login}'";

            var command = new NpgsqlCommand(changeQuery, database.GetConnection());
            command.ExecuteNonQuery();

            database.CloseConnection();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            Hide();
        }
    }
}
