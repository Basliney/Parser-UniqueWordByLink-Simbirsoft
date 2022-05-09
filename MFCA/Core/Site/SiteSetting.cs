namespace MFCA.Core.Site
{
    class SiteSetting : IParserSettings
    {
        public SiteSetting(string url)
        {
            BaseURL = "https://habr.com/ru/all/page1/";
            if (url != null && !url.Equals(""))
            {
                BaseURL = url;
            }
        }

        public string BaseURL { get; set; }
    }
}
