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
    enum RowState
    {
        Exited,
        New,
        Modfied,
        ModfiedNew,
        Deleted
    }
    public partial class NatPersonForm : Form
    {
        private readonly checkUser _user;
        Database database = new Database();
        int selectedRow;
        public NatPersonForm(checkUser user)
        {
            _user = user;
            InitializeComponent();
        }
        private void IsAdmin()
        {
            buttonDelete.Enabled = _user.IsAdmin;
        }
        private void NatPersonForm_Load(object sender, EventArgs e)
        {
            loginLabel.Text = $"{_user.Login}:{_user.Status}";
            IsAdmin();

            CreateColumns();
            RefreshDataGrid(dataGridView1);   
        }
        private void CreateColumns()//колонки для datagrid
        {
            dataGridView1.Columns.Add("id_user", "ID"); //0
            dataGridView1.Columns.Add("last_name", "Фамилия"); //1
            dataGridView1.Columns.Add("name", "Имя"); //2
            dataGridView1.Columns.Add("middle_name", "Отчество"); //3
            dataGridView1.Columns.Add("birthday", "Дата рождения");//4
            dataGridView1.Columns.Add("passport", "Серия номер паспорта");
            dataGridView1.Columns.Add("home_adress", "Место проживания"); //5
            dataGridView1.Columns.Add("e_mail", "Электронная почта"); //6
            dataGridView1.Columns.Add("phone", "Номер телефона"); //7
            dataGridView1.Columns.Add("position", "Должность"); //8
            dataGridView1.Columns.Add("work_adress", "Место работы"); //9
            //10
            dataGridView1.Columns.Add("IsNew", string.Empty);
        }
        private void Clearfields() //очищение textbox
        {
            textBox1.Text = "";
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
        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetString(4), record.GetString(5), record.GetString(6), record.GetString(7), record.GetString(8), record.GetString(9), record.GetString(10), RowState.ModfiedNew);
        }

        private void RefreshDataGrid(DataGridView dgw)//обновление datagrid
        {
            dgw.Rows.Clear();
            string queryString = $"select * from nature_person";

            NpgsqlCommand command = new NpgsqlCommand(queryString, database.GetConnection());

            database.OpenConnection();

            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();
            database.CloseConnection();
        }
        
       //вывод информации из dataGridView1 в textbox'ы
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                textBox1.Text = row.Cells[0].Value.ToString();
                textBox2.Text = row.Cells[1].Value.ToString();
                textBox3.Text = row.Cells[2].Value.ToString();
                textBox4.Text = row.Cells[3].Value.ToString();
                textBox5.Text = row.Cells[4].Value.ToString();
                textBox6.Text = row.Cells[5].Value.ToString();
                textBox7.Text = row.Cells[6].Value.ToString();
                textBox8.Text = row.Cells[7].Value.ToString();
                textBox9.Text = row.Cells[8].Value.ToString();
                textBox10.Text = row.Cells[9].Value.ToString();
                textBox11.Text = row.Cells[10].Value.ToString();
            }
        }

        private void Update()//метод для проверки RowState
        {
            database.OpenConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[11].Value;

                if (rowState == RowState.Exited)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value); 
                    var deleteQuery = $"delete from nature_person where id_user = {id}";

                    var command = new NpgsqlCommand(deleteQuery, database.GetConnection());
                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modfied)
                {
                    var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var last_name = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var name = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var middle_name = dataGridView1.Rows[index].Cells[3].Value.ToString();
                    var birthday = dataGridView1.Rows[index].Cells[4].Value.ToString();
                    var passport = dataGridView1.Rows[index].Cells[5].Value.ToString();
                    var home_adress = dataGridView1.Rows[index].Cells[6].Value.ToString();
                    var e_mail = dataGridView1.Rows[index].Cells[7].Value.ToString();
                    var phone = dataGridView1.Rows[index].Cells[8].Value.ToString();
                    var position = dataGridView1.Rows[index].Cells[9].Value.ToString();
                    var work_adress = dataGridView1.Rows[index].Cells[10].Value.ToString();

                    var changeQuery = $"update nature_person set last_name = '{last_name}', name = '{name}', middle_name = '{middle_name}', birthday = '{birthday}', passport = '{passport}', home_adress = '{home_adress}', e_mail = '{e_mail}', phone = '{phone}', position = '{position}', work_adress = '{work_adress}' where id_user = '{id}'";

                    var comm = new NpgsqlCommand(changeQuery, database.GetConnection());
                    comm.ExecuteNonQuery();
                }

            }
            database.CloseConnection();
        }

        private void DeleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[11].Value = RowState.Deleted;
                return;
            }
            dataGridView1.Rows[index].Cells[11].Value = RowState.Deleted;
        }
        private void Change()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            var id = textBox1.Text;
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

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView1.Rows[selectedRowIndex].SetValues(id, last_name, name, middle_name, birthday, passport, home_adress, e_mail, phone, position, work_adress);
                dataGridView1.Rows[selectedRowIndex].Cells[11].Value = RowState.Modfied;
            }
        }
        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string searchString = $"select * from nature_person where concat (id_user, last_name, name, middle_name, birthday, passport, home_adress, e_mail, phone, position, work_adress) like '%" + textBoxSearch.Text + "%'";

            NpgsqlCommand comm = new NpgsqlCommand(searchString, database.GetConnection());

            database.OpenConnection();

            NpgsqlDataReader read = comm.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRow(dgw, read);
            }

            read.Close();
            database.CloseConnection();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Update();
            MessageBox.Show("Успешно", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DeleteRow();
            Clearfields();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            Change();
            Clearfields();
        }
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
            Clearfields();
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddForm addForm = new AddForm(_user);
            addForm.Show();
            Hide();
        }
        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clearfields();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            SystemForm systemForm = new SystemForm(_user);
            this.Hide();
            systemForm.ShowDialog();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void label7_Click(object sender, EventArgs e)
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
