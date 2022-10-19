using AngleSharp.Dom;

namespace ToyParser.ParserDir
{
    internal class MainPageParser : Parser
    {

        public string DefaultAddress { get; private set; } = "https://www.toy.ru";

        public MainPageParser()
        {
            ClassNames["Items"] = "d-block p-1 product-name gtm-click";
            ClassNames["Page"] = "page-link";
            ClassNames["Region"] = "form-control mb-2 mr-sm-2 suggestions-input";
        }

        public int FindPageNumber(string address) //Поиск номера последней страницы
        {
            var document = GetAsyncDocument(address);
            var result = document.GetElementsByClassName(ClassNames["Page"]);
            ClassNames.Remove("Page");
            Int32.TryParse(result[result.Length - 2].TextContent, out int pageNumber);
            return pageNumber;
        }

        public void ChangeRegion(string address, string newRegion)
        {

        }
    }
}
