using System.Windows;
using System.Windows.Controls;
using System.IO;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System;
using System.Collections.Generic;

namespace Documents_Galkin.Pages
{
    /// <summary>
    /// Логика взаимодействия для Add.xaml
    /// </summary>
    public partial class Add : Page
    {
        public Model.Document Document;
        public string s_src = "";
        public Add(Model.Document Document = null)
        {
            InitializeComponent();

            // Загружаем список ответственных
            List<string> respo = new classes.DocumentContext().AllRespo();
            tb_user.ItemsSource = respo;
            if (Document != null)
            {
                this.Document = Document;
                if (File.Exists(Document.src))
                {
                    s_src = Document.src;
                    src.Source = new BitmapImage(new Uri(s_src));
                }
                tb_name.Text = this.Document.name;
                tb_user.Text = this.Document.user;
                tb_id.Text = this.Document.id_document;
                tb_date.Text = this.Document.date.ToString("dd.MM.yyyy");
                tb_status.SelectedIndex = this.Document.status;
                tb_vector.Text = this.Document.vector;
                bthAdd.Content = "Изменить";
            }
        }

        private void Back(object sender, RoutedEventArgs e) => 
            MainWindow.init.OpenPages(MainWindow.pages.main);

        private void SelectImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
            openFileDialog.FilterIndex = 2;
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            {
                src.Source = new BitmapImage(new System.Uri(openFileDialog.FileName));
                s_src = openFileDialog.FileName;
            }
        }

        private void AddDocument(object sender, RoutedEventArgs e)
        {
            if (s_src.Length == 0)
            {
                MessageBox.Show("Выберите изображение");
                return;
            }
            if (tb_name.Text.Length == 0)
            {
                MessageBox.Show("Укажите наименование");
                return;
            }
            if (tb_user.Text.Length == 0)
            {
                MessageBox.Show("Укажите ответственного");
                return;
            }
            if (tb_id.Text.Length == 0)
            {
                MessageBox.Show("Укажите код");
                return;
            }
            if (tb_date.Text.Length == 0)
            {
                MessageBox.Show("Укажите дату поступления");
                return;
            }
            if (tb_status.Text.Length == 0)
            {
                MessageBox.Show("Укажите статус");
                return;
            }
            if (tb_vector.Text.Length == 0)
            {
                MessageBox.Show("Укажите направление");
                return;
            }
            string selectedRespo = (string)tb_user.SelectedItem;

            // Проверяем, что значение не пустое
            if (string.IsNullOrEmpty(selectedRespo))
            {
                MessageBox.Show("Выберите ответственного");
                return;
            }
            if (Document == null)
            {
                classes.DocumentContext newDocument = new classes.DocumentContext();
                newDocument.src = s_src;
                newDocument.name = tb_name.Text;
                newDocument.Respo = selectedRespo;  // Получение ответственного из ComboBox
                newDocument.id_document = tb_id.Text;

                DateTime newDate = new DateTime();
                DateTime.TryParse(tb_date.Text, out newDate);
                newDocument.date = newDate;
                newDocument.status = tb_status.SelectedIndex;
                newDocument.vector = tb_vector.Text;
                newDocument.Save();
                MessageBox.Show("Документ добавлен");
            }

            else
            {

                classes.DocumentContext newDocument = new classes.DocumentContext();
                newDocument.src = s_src;
                newDocument.id = Document.id;
                newDocument.name = tb_name.Text;
                newDocument.Respo = selectedRespo;
                newDocument.id_document = tb_id.Text;

                DateTime newDate = new DateTime();
                DateTime.TryParse(tb_date.Text, out newDate);
                newDocument.date = newDate;
                newDocument.status = tb_status.SelectedIndex;
                newDocument.vector = tb_vector.Text;
                newDocument.Save(true);
                MessageBox.Show("Документ изменён<>");
            }
            MainWindow.init.AllDocuments = new classes.DocumentContext().AllDocuments();
            MainWindow.init.OpenPages(MainWindow.pages.main);
        }
    }
}
