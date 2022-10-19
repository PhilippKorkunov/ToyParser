using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyParser.ParserDir
{
    internal class Parser
    {
        public Dictionary<string, string> ClassNames { get; protected private set; } = new Dictionary<string, string>(); //Словарь необходимых class-name'ов
        private static IBrowsingContext Context { get; set; }

        static Parser()
        {
            var config = Configuration.Default.WithDefaultLoader();
            Context = BrowsingContext.New(config);
        }

        public Dictionary<string, IHtmlCollection<IElement>> OwnParse(string address) //парсинг страницы с адресом=address по class-name'у
        {
            var document = GetAsyncDocument(address);
            var outDictionary
                = new Dictionary<string, IHtmlCollection<IElement>>();
            foreach (var key in ClassNames.Keys)
            {
                var result = document.GetElementsByClassName(ClassNames[key]);
                outDictionary[key] = result;
            }
            return outDictionary;
        }

        private protected IDocument GetAsyncDocument(string address) => Context.OpenAsync(address).Result;


    }
}
