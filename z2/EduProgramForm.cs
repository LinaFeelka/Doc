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
    public partial class EduProgramForm : Form
    {
        private readonly checkUser _user;
        Database database = new Database();
        int selectedRow;
        public EduProgramForm(checkUser user)
        {
            _user = user;
            InitializeComponent();
        }
        private void IsAdmin()
        {
            buttonDelete.Enabled = _user.IsAdmin;
        }
        private void EduProgramForm_Load(object sender, EventArgs e)
        {
            loginLabel.Text = $"{_user.Login}:{_user.Status}";
            IsAdmin();

            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
            Clearfields();
        }

        private void CreateColumns()//колонки для datagrid
        {
            dataGridView1.Columns.Add("id_program", "ID"); //0
            dataGridView1.Columns.Add("name", "Название"); //1
            dataGridView1.Columns.Add("study_period", "Срок обучения"); //2
            dataGridView1.Columns.Add("qualification", "Квалификация"); //3
            dataGridView1.Columns.Add("study_cost", "Стоимость обучения");//4           
            dataGridView1.Columns.Add("IsNew", string.Empty);
        }
        private void Clearfields() //очищение textbox
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }
        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetString(4), RowState.ModfiedNew);
        }
        private void RefreshDataGrid(DataGridView dgw)//обновление datagrid
        {
            dgw.Rows.Clear();
            string queryString = $"select * from edu_program";

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
            }
        }
        private void Update()//метод для проверки RowState
        {
            database.OpenConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[5].Value;

                if (rowState == RowState.Exited)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var id_program = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from edu_program where id_program = {id_program}";

                    var command = new NpgsqlCommand(deleteQuery, database.GetConnection());
                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modfied)
                {
                    var id_program = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var name = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var study_period = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var qualification = dataGridView1.Rows[index].Cells[3].Value.ToString();
                    var study_cost = dataGridView1.Rows[index].Cells[4].Value.ToString();
                    
                    var changeQuery = $"update edu_program set name = '{name}', study_period = '{study_period}', qualification = '{qualification}', study_cost = '{study_cost}' where id_program = '{id_program}'";

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
                dataGridView1.Rows[index].Cells[5].Value = RowState.Deleted;
                return;
            }
            dataGridView1.Rows[index].Cells[5].Value = RowState.Deleted;
        }

        private void Change()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            var id_program = textBox1.Text;
            var name = textBox2.Text;
            var study_period = textBox3.Text;
            var qualification = textBox4.Text;
            var study_cost = textBox5.Text;
            
            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView1.Rows[selectedRowIndex].SetValues(id_program, name, study_period, qualification, study_cost);
                dataGridView1.Rows[selectedRowIndex].Cells[5].Value = RowState.Modfied;
            }
        }
        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string searchString = $"select * from edu_program where concat (id_program, name, study_period, qualification, study_cost) like '%" + textBoxSearch.Text + "%'";

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

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddEduProgForm addepForm = new AddEduProgForm(_user);
            addepForm.Show();
            Hide();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            Change();
            Clearfields();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DeleteRow();
            Clearfields();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Update();
            MessageBox.Show("Успешно", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clearfields();
        }
    }
}
