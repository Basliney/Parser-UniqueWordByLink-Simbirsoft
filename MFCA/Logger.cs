using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MFCA
{
    static class Logger
    {
        static StreamWriter sw = null;
        static string writePath = $@"C:\Users\{Environment.UserName}\";//ErrorLog{DateTime.Now.ToString()}.txt";

        /// <summary>
        /// Открытие файла для записи
        /// </summary>
        /// <param name="name">Название файла</param>
        public static void Open(string name)
        {
            try
            {
                sw = new StreamWriter(writePath + name + ".txt", false, System.Text.Encoding.UTF8);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Запись текста в файл
        /// </summary>
        /// <param name="text">Текст для записи</param>
        public static async void Write(string text)
        {
            try
            {
                if (sw == null)
                {
                    sw = new StreamWriter(writePath + $"ErrorLog{ DateTime.Now.Day }_{DateTime.Now.Month}_{DateTime.Now.Year}.txt", false, System.Text.Encoding.UTF8);
                }
                await sw.WriteAsync(text + "\n");
                Console.WriteLine($"Error has been looged by path:{writePath}");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Закрытие потока
        /// </summary>
        public static void Close()
        {
            sw?.Close();
        }
    }
}
