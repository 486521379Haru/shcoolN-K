using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace school
{
    public partial class Form1 : Form
    {
        private const string ConnectionString = "Data Source=db.db;Version=3;";

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        private void EXIT_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnWord_Click(object sender, EventArgs e)
        {
            string russianWord = txtWord.Text.Trim();
            string englishWord = txtTranslation.Text.Trim();

            if (!string.IsNullOrEmpty(russianWord) && !string.IsNullOrEmpty(englishWord))
            {
                AddWordToDatabase(russianWord, englishWord);
                LoadData();
                ClearInputFields();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите русское и английское слово.");
            }
        }

        private void AddWordToDatabase(string russia, string english)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO Word (Russia, English) VALUES (@russia, @english)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@russia", russia);
                    command.Parameters.AddWithValue("@english", english);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void LoadData()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT Russia, English FROM Word"; // Изменено для выбора нужных столбцов
                using (var adapter = new SQLiteDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataWord.DataSource = dataTable;

                    // Добавление кнопки удаления в DataGridView
                    if (!dataWord.Columns.Contains("Delete"))
                    {
                        DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();
                        deleteButtonColumn.Name = "Delete";
                        deleteButtonColumn.HeaderText = "Удалить";
                        deleteButtonColumn.Text = "Удалить";
                        deleteButtonColumn.UseColumnTextForButtonValue = true;
                        dataWord.Columns.Add(deleteButtonColumn);
                    }
                }
            }
        }

        private void DeleteWordFromDatabase(string russia)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM Word WHERE Russia = @russia";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@russia", russia);
                    int rowsAffected = command.ExecuteNonQuery(); // Получаем количество затронутых строк

                    // Отладочная информация
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Слово успешно удалено.");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка: слово не найдено.");
                    }
                }
            }
        }
        

        private void ClearInputFields()
        {
            txtWord.Clear();
            txtTranslation.Clear();
        }

        private void dataWord_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что нажата кнопка удаления
            if (e.ColumnIndex == dataWord.Columns["Delete"].Index)
            {
                // Получаем русское слово из соответствующей строки
                string russianWord = dataWord.Rows[e.RowIndex].Cells["Russia"].Value.ToString();

                // Подтверждение удаления
                var result = MessageBox.Show($"Вы уверены, что хотите удалить слово '{russianWord}'?",
                                               "Подтверждение удаления",
                                               MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    DeleteWordFromDatabase(russianWord);
                    LoadData(); // Обновляем данные после удаления
                }
            }
        }
    }
}
