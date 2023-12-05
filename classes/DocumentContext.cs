using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Documents_Galkin.classes
{
	public class DocumentContext : Model.Document, Interfaces.IDocument
	{
		public string Respo = "Привет";
		public List<DocumentContext> AllDocuments()
		{
			List<DocumentContext> allDocuments = new List<DocumentContext>();
			OleDbConnection connection = Common.DBConnection.Connection();
			OleDbDataReader dataDocuments = Common.DBConnection.Query("SELECT * FROM [Документы]", connection);
			while (dataDocuments.Read())
			{
				DocumentContext newDocument = new DocumentContext();
				newDocument.id = dataDocuments.GetInt32(0);
				newDocument.src = dataDocuments.GetString(1);
				newDocument.name = dataDocuments.GetString(2);
				// Добавление ответсвтенных
				newDocument.Respo = dataDocuments.GetString(3);
				newDocument.id_document = dataDocuments.GetString(4);
				newDocument.date = dataDocuments.GetDateTime(5);
				newDocument.status = dataDocuments.GetInt32(6);
				newDocument.vector = dataDocuments.GetString(7);


				allDocuments.Add(newDocument);
			}
			Common.DBConnection.CloseConnection(connection);

			return allDocuments;
		}

		public void Save(bool Update = false)
		{
			if (Update)
			{
				OleDbConnection connection = Common.DBConnection.Connection();
				Common.DBConnection.Query("UPDATE [Документы]" +
					"SET " +
					$"[Изображение] = '{this.src}'," +
					$"[Наименование] = '{this.name}'," +
					$"[Ответственный] = '{this.Respo}'," +
					$"[Код документа] = '{this.id_document}'," +
					$"[Дата поступления] = '{this.date.ToString("dd.MM.yyyy")}'," +
					$"[Статус] = '{this.status}'," +
					$"[Направление] = '{this.vector}'" +
                    $"WHERE [Код] = {this.id}", connection);

				Common.DBConnection.CloseConnection(connection); 
			}
			else
			{
				OleDbConnection connection = Common.DBConnection.Connection();
				Common.DBConnection.Query("INSERT INTO " +
					"[Документы]" +
						"([Изображение], " +
						"[Наименование], " +
						"[Ответственный], " +
						"[Код документа], " +
						"[Дата поступления], " +
						"[Статус], " +
						"[Направление]) " +
					"VALUES (" +
						$"'{this.src}', "+
						$"'{this.name}', "+
						$"'{this.Respo}', "+
						$"'{this.id_document}', "+
						$"'{this.date.ToString("dd.MM.yyyy")}', "+
						$"{this.status}, "+
						$"'{this.vector}')", connection);

				Common.DBConnection.CloseConnection(connection);
			}
		}

		public void Delete()
		{
			OleDbConnection connection = Common.DBConnection.Connection();
			Common.DBConnection.Query($"DELETE FROM [Документы] WHERE [Код] = {this.id}", connection);
			Common.DBConnection.CloseConnection(connection);
		}

        /// <summary>
        /// Добавление ответственных
        /// </summary>

        public void SaveRespo()
        {
			try
			{
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query($"INSERT INTO [Ответственные] ([Имя]) VALUES ('{this.Respo}'", connection);
                Common.DBConnection.CloseConnection(connection);
            }
			catch { MessageBox.Show("Ошибка"); }
        }

        public List<string> AllRespo()
        {

                List<string> allRespo = new List<string>();
                OleDbConnection connection = Common.DBConnection.Connection();
                OleDbDataReader dataRespo = Common.DBConnection.Query("SELECT * FROM [Ответственные]", connection);

                while (dataRespo.Read())
                {
                    object value = dataRespo.GetValue(1);

                    if (value != null && value != DBNull.Value)
                    {
                        allRespo.Add(value.ToString());
                    }
                    else
                    {
                        allRespo.Add(string.Empty);
                    }
                }
                Common.DBConnection.CloseConnection(connection);
                return allRespo;

        }

        public void DeleteRespo(string respo)
        {
			try
			{
                OleDbConnection connection = Common.DBConnection.Connection();
                Common.DBConnection.Query($"DELETE FROM [Ответственные] WHERE [Имя] = '{respo}'", connection);
                Common.DBConnection.CloseConnection(connection);
            }
            catch { MessageBox.Show("Ошибка в delete"); }
        }
    }
}
