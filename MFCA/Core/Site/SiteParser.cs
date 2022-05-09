using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AngleSharp.Html.Dom;
using MFCA.Pattern;

namespace MFCA.Core.Site
{
    class SiteParser : IParser<WordsCollection>
    {
        public WordsCollection Parse(IHtmlDocument document)
        {
            var collection = new WordsCollection();
            var items = document.QuerySelectorAll("*").Where(item => item.Children.Count() == 0);   //Получение элементов у которых нет дочерних эл-ов

            foreach (var item in items)
            {
                collection.AddItem(item.TextContent);
            }

            return collection;
        }
    }
}
