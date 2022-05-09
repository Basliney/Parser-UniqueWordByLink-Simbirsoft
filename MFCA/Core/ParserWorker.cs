using AngleSharp.Html.Parser;
using System;

namespace MFCA.Core
{
    class ParserWorker<T> where T : class
    {
        private IParser<T> parser;
        private IParserSettings parserSettings;

        private HtmlLoader loader;

        private bool isActive;

        #region Propetries

        public event Action<object, T> OnNewData;
        public event Action<object> OnCompleted;

        public IParser<T> Parser
        {
            get
            {
                return parser;
            }

            set
            {
                parser = value;
            }
        }

        public IParserSettings ParserSettings
        {
            get
            {
                return parserSettings;
            }
            set
            {
                parserSettings = value;
                loader = new HtmlLoader(value);
            }
        }

        public bool IsActive
        {
            get
            {
                return isActive;
            }
        }

        #endregion


        public ParserWorker(IParser<T> parser)
        {
            this.parser = parser;
        }

        public ParserWorker(IParser<T> parser, IParserSettings settings) : this(parser)
        {
            this.ParserSettings = settings;
        }

        public void Start()
        {
            isActive = true;
            Worker();
        }

        public void Abort()
        {
            OnCompleted?.Invoke(this);
            isActive = false;
        }

        private async void Worker()
        {
            try
            {
                if (!isActive)
                {
                    return;
                }

                var source = await loader.GetSourceByPage();
                var domParser = new HtmlParser();

                var document = await domParser.ParseDocumentAsync(source);

                var result = parser.Parse(document);

                OnNewData?.Invoke(this, result);
            }
            catch (Exception e)
            {
                Logger.Write($"{e.Source}\n \n{e.Message}\n \n{e.StackTrace}");
                Console.WriteLine($"{e.Source}\n \n{e.Message}\n \n{e.StackTrace}");
            }
            finally
            {
                OnCompleted?.Invoke(this);
                isActive = false;
            }
        }
    }
}
