using MFCA.Core;
using MFCA.Core.Site;
using MFCA.Pattern;
using System;
using System.Text.RegularExpressions;

namespace MFCA
{
    class Program
    {
        private static ParserWorker<WordsCollection> parser;

        private static WordsCollection wordsCollection = new WordsCollection();
        private static WordsMap wordsMap;

        private static string link;
        private static char[] Separators = { '`', '@', '/', '&', ' ', ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t', '-', '_', '<', '>', '*' };

        static void Main(string[] args)
        {
            DatabaseWorker.OpenConnection();

            Console.Write("Type link here >> ");
            link = Console.ReadLine();

            parser = new ParserWorker<WordsCollection>(
                new SiteParser(),
                new SiteSetting(link)
                );

            parser.OnCompleted += Parser_OnCompleted;
            parser.OnNewData += Parser_OnNewData;

            parser.Start();
            Console.ReadKey();
        }

        private static void Parser_OnNewData(object arg1, WordsCollection arg2)
        {
            foreach (var item in arg2)
            {
                Regex reg_space = new Regex(@"\s+");    //Лишние пробелы
                Regex reg_numbers = new Regex(@"\d+");  //Цифры
                Regex reg_symbols = new Regex(@"\W+");  //Символы
                string target = " ";

                string res = reg_numbers.Replace(item.ToString(), target);
                res = reg_symbols.Replace(res, target);
                res = reg_space.Replace(res, target);

                var newList = res.Split(Separators);
                foreach (var nItem in newList)
                {
                    if (nItem.Length > 3 && nItem.Length <= 45)
                    {
                        wordsCollection.AddItem(nItem);
                    }
                }
            }
        }

        private static void Parser_OnCompleted(object obj)
        {
            wordsMap = new WordsMap(wordsCollection);
            wordsMap.Sort(true);
            wordsMap.Printer();

            DatabaseWorker.Insert(link.Equals("") ? "https://habr.com/ru/all/page1/" : link, wordsMap);

            Console.WriteLine($"\nComplete!!");
            DatabaseWorker.CloseConnection();
            Logger.Close();
        }
    }
}
