using AngleSharp.Dom;

namespace ToyParser.ParserDir
{
    internal class ItemPageParser : Parser
    {
        public ItemPageParser()
        {
            ClassNames["Region"] = "col-12 select-city-link";
            ClassNames["Breadcrumbs"] = "breadcrumb-item hide-mobile";
            ClassNames["Name"] = "detail-name";
            ClassNames["Price"] = "price";
            ClassNames["OldPrice"] = "old-price";
            ClassNames["IsInStock"] = "ok";
            ClassNames["LinksToPicture"] = "img-fluid";
        }
    }
}
