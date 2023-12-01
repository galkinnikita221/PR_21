using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents_Galkin.classes
{
	public class DocumentContext : Model.Document, Interfaces.IDocument
	{
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
				newDocument.user = dataDocuments.GetString(3);
				newDocument.id_document = dataDocuments.GetInt32(4);
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
					$"[Ответственный] = '{this.user}'," +
					$"[Код документа] = '{this.id_document}'," +
					$"[Дата поступления] = '{this.date.ToString("dd.MM.yyyy")}'," +
					$"[Статус] = '{this.status}'," +
					$"[Направление] = '{this.vector}'," +
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
					$"'{this.src}', " +
					$"'{this.name}', " +
					$"'{this.user}', " +
					$"'{this.id_document}', " +
					$"'{this.date.ToString("dd.MM.yyyy")}', " +
					$"'{this.status}', " +
					$"'{this.vector}'", connection);
				Common.DBConnection.CloseConnection(connection);
			}
		}

		public void Delete()
		{
			OleDbConnection connection = Common.DBConnection.Connection();
			Common.DBConnection.Query($"DELETE FROM [Документы] WHERE [Код] = {this.id}", connection);
			Common.DBConnection.CloseConnection(connection);
		}
	}
}
