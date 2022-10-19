using AngleSharp.Dom;
using System.Diagnostics;
using System.Text;
using ToyParser;
using ToyParser.ParserDir;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch(); //Измеряем время
            stopwatch.Start();


            var mainPageParser = new MainPageParser();
            var itemPageParser = new ItemPageParser();
            string csvOutput = "";
            var mainPageAddress = $"https://www.toy.ru/catalog/boy_transport/?filterseccode%5B0%5D=transport&PAGEN_5"; // ссылка, где можно изменять номер страницы
            int pageNumber = mainPageParser.FindPageNumber($"{mainPageAddress}=1"); //кол-во всех страниц

            for (int i = 1; i <= pageNumber; i++)
            {
                var mainPageDict = mainPageParser.OwnParse($"{mainPageAddress}={i}");
                var mainPageCollection = mainPageDict["Items"]; // все элементы (товары на i-ой странице)

                var htmlCollection = new List<Dictionary<string, IHtmlCollection<IElement>>>();
                var referenceList = new List<string>();

                Task[] tasks = new Task[mainPageCollection.Length]; //массив тасков
                int index = 0;
                foreach (var item in mainPageCollection)
                {
                    tasks[index] = new Task(() =>
                    {
                        string address = mainPageParser.DefaultAddress + item.Attributes["href"].Value; //ссылка на конкретный товар
                        htmlCollection.Add(itemPageParser.OwnParse(address)); //список из словарей элементов для товара
                        referenceList.Add(address); //список ссылок на товар 
                    });
                    tasks[index].Start();
                    index++;
                }
                Task.WaitAll(tasks);

                foreach (var element in htmlCollection)
                {
                    Item item = new Item(); //Заполняем нужные совйства для каждого товара
                    item.Region = element["Region"][0].TextContent;
                    var breadList = from p in element["Breadcrumbs"]
                                    select p.TextContent;
                    item.Breadcrumbs = breadList.ToList<string>();
                    item.Name = element["Name"][0].TextContent;
                    item.Price = element["Price"][0].TextContent;
                    item.OldPrice = element["OldPrice"].Length > 0 ? element["OldPrice"][0].TextContent : item.Price;
                    item.IsInStock = element["IsInStock"][0].TextContent;
                    var linksToPictureList = (from p in element["LinksToPicture"]
                                              where p.Attributes["src"] is not null
                                              select p.Attributes["src"].Value);
                    item.LinksToPicture = linksToPictureList.ToList<string>();
                    item.LinkTo = referenceList[0];
                    referenceList.Remove(referenceList[0]);

                    csvOutput += $"{item}\n";
                }
            }
            var csv = new CsvCreator("toy.csv", csvOutput);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds.ToString()); //Время выполнения
        }
    }
}