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
            string russiaWord = txtWord.Text.Trim();
            string englishWord = txtTranslation.Text.Trim();

            if (!string.IsNullOrEmpty(russiaWord) && !string.IsNullOrEmpty(englishWord))
            {
                AddWordToDatabase(russiaWord, englishWord);
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
                string query = "SELECT * FROM Word";
                using (var adapter = new SQLiteDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataWord.DataSource = dataTable; 
                }
            }
        }

        private void ClearInputFields()
        {
            txtWord.Clear();
            txtTranslation.Clear();
        }
    }
}
