using AngleSharp.Html.Dom;

namespace MFCA.Core
{
    interface IParser<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}
