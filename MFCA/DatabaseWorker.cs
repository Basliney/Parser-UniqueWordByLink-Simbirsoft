using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using MFCA.Pattern;

namespace MFCA
{
    static class DatabaseWorker
    {
        private static SqlConnection sqlConnection = null;
        private static SqlDataReader sqlDataReader = null;

        #region Strings
        private static string connectionString = ConfigurationManager.ConnectionStrings["DatabasePath"].ConnectionString;//@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\PC\source\repos\MFCA\MFCA\DB.mdf;Integrated Security = True";
        private static string INSERT_COMMAND = "insert into Link (link, count_entries) values ";
        private static string INSERT_COMMAND_WORD_ENTRY = "insert into WordEntries (site_id, site_link, word, count) values ";
        private static string SELECT_FROM_LINK = "select Id from [Link] where link =";
        #endregion

        /// <summary>
        /// Инициализация соединения с БД
        /// </summary>
        public static void InitializeConnection()
        {
            try
            {
                sqlConnection = new SqlConnection(connectionString);
            }
            catch (Exception e)
            {
                Logger.Write($"{e.StackTrace}");
            }
        }

        /// <summary>
        /// Открытие соединения с БД
        /// </summary>
        public static void OpenConnection()
        {
            try {
                InitializeConnection();
                sqlConnection.Open();
                return;
            }
            catch(Exception e)
            {
                Logger.Write($"Can't open connection \n\n{e.StackTrace}");
            }
        }

        /// <summary>
        /// Закрытие соединения с БД
        /// </summary>
        public static void CloseConnection()
        {
            try
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }

                if (sqlDataReader != null)
                {
                    sqlDataReader.Close();
                }
                return;
            }
            catch (Exception e)
            {
                Logger.Write($"Can't close connection \n\n{e.StackTrace}");
            }
        }

        /// <summary>
        /// Занесение статистики в БД
        /// </summary>
        /// <param name="link">Ссылка на сайт</param>
        /// <param name="wordsMap">Список слов</param>
        /// <returns>Состояние записи</returns>
        public static bool Insert(string link, WordsMap wordsMap)
        {
            try
            {
                Console.WriteLine(wordsMap.GetItems().Count);
                SqlCommand sqlCommand;
                SqlCommand check_into = new SqlCommand($"{SELECT_FROM_LINK} '{link}'",
                    sqlConnection);

                sqlDataReader = check_into.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    RefreshConnection();
                    //delete from <table> where WHAT = ?
                    sqlCommand = new SqlCommand($"delete from WordEntries" +
                        $" where Site_link = '{link}'", sqlConnection);
                    sqlCommand.ExecuteNonQuery();

                    RefreshConnection();
                    //update table set <column> where WHAT = ?
                    sqlCommand = new SqlCommand($"update Link set" +
                        $" count_entries = {wordsMap.GetItems().Count} where link = '{link}'", sqlConnection);
                    sqlCommand.ExecuteNonQuery();
                }
                else
                {
                    RefreshConnection();

                    string command = $"{INSERT_COMMAND}('{link}', {wordsMap.GetItems().Count})";
                    //Console.WriteLine(command);
                    sqlCommand = new SqlCommand(command, sqlConnection);
                    sqlCommand.ExecuteNonQuery();
                }
                sqlCommand = new SqlCommand($"{SELECT_FROM_LINK} '{link}'", sqlConnection);
                sqlDataReader = sqlCommand.ExecuteReader();

                int linkId = 0;
                while (sqlDataReader.Read())
                {
                    linkId = (int)sqlDataReader["Id"];
                }

                RefreshConnection();

                for (int i = 0; i < 1000 && i < wordsMap.GetItems().Count; i++)
                {
                    InsertWordEntry(linkId, link, wordsMap.GetItems()[i]);
                }
                return true;
            }
            catch (Exception e)
            {
                Logger.Write($"{e.Source}\n \n{e.Message}\n \n{e.StackTrace}");
            }
            return false;
        }

        /// <summary>
        /// Занесение слова в БД
        /// </summary>
        /// <param name="siteID">Идентификатор сайта в БД</param>
        /// <param name="siteLink">Ссылка на сайт</param>
        /// <param name="word">Слово</param>
        /// <returns></returns>
        private static bool InsertWordEntry(int siteID, string siteLink, Word word)
        {
            try
            {
                string command = string.Empty;
                if (word.Key.Substring(0, 1).Equals("'"))
                {
                    command = $"{INSERT_COMMAND_WORD_ENTRY}({siteID}, '{siteLink}', N{word.Key}, {word.Value})";
                }
                else
                {
                    command = $"{INSERT_COMMAND_WORD_ENTRY}({siteID}, '{siteLink}', N'{word.Key}', {word.Value})";
                }
                SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                sqlCommand.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Logger.Write($"{e.Source}\n \n{e.Message}\n \n{e.StackTrace}");
            }
            return false;
        }

        /// <summary>
        /// Обновление соединения
        /// </summary>
        private static void RefreshConnection()
        {
            sqlConnection.Close();
            OpenConnection();
        }
    }
}
