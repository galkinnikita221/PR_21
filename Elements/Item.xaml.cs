using System;
using Documents_Galkin.classes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Documents_Galkin.Elements
{
    /// <summary>
    /// Логика взаимодействия для Item.xaml
    /// </summary>
    public partial class Item : UserControl
    {
        DocumentContext Document;
        public Item(DocumentContext Document)
        {
            InitializeComponent();
            img.Source = new BitmapImage(new Uri(Document.src));
            IName.Content = Document.name;
            IUser.Content = $"Ответственный: {Document.Respo}";
            ICode.Content = $"Код документа: {Document.id_document}";
            IDate.Content = $"Дата поступления {Document.date.ToString("dd.MM.yyyy")}";
            IStatus.Content = Document.status == 0 ? $"Статус: Входящий" : $"Статус: Исходящий";
            IDirect.Content = $"Направление:" + Document.user;
            this.Document = Document;

        }

        private void EditDocument(object sender, RoutedEventArgs e)
        {
            MainWindow.init.frame.Navigate(new Pages.Add(Document));
        }

        private void DeleteDocument(object sender, RoutedEventArgs e)
        {
            Document.Delete();
            MainWindow.init.AllDocuments = new DocumentContext().AllDocuments();
            MainWindow.init.OpenPages(MainWindow.pages.main);
        }
    }
}
