using AngleSharp.Text;

namespace ToyParser
{
    internal class Item
    {
        private string region = "";
        private string price = "";
        private string oldPrice = "";
        private string isInStock = "";

        //Свойства, которую содержат требуемую информацию
        public string Region //Регион
        { 
            get
            {
                return region;
            }
            set
            {
                string substr = new string(value.Where(x => (!x.IsWhiteSpaceCharacter())).ToArray());
                region = substr.Split(':')[1];
            } }
        public List<string> Breadcrumbs { get; set; } = new List<string>(); // Хлебные крошки
        public string Name { get; set; } = ""; //Имя товара
        public string Price //Цена товара
        {
            get
            {
                return price;
            }
            set
            {
                price = ClearMoney(value);
            }
        }

        public string OldPrice //Старая цена товара (если ее нет, то старая цена = текущая, то есть не изменилась)
        {
            get
            {
                return oldPrice;
            }
            set
            {
                oldPrice = ClearMoney(value);
            }
        }
        public string IsInStock // Наличие товара
        {
            get { return isInStock; }
            set
            { 
                isInStock = value == " Товар есть в наличии" ? "Да" : "Нет"; }
        }
        public List<string> LinksToPicture { get; set; } = new List<string>(); //Список ссылок на картинки
        public string LinkTo { get; set; } = ""; //Ссылка на товар


        private string ClearMoney(string money) => new string(money.Where(x => (x.IsDigit())).ToArray()); //Отчистка от лишних символов

        public override string ToString() => $"{Region};{ToString(Breadcrumbs, '>')};{Name};{Price};" +
            $"{OldPrice};{IsInStock};{ToString(LinksToPicture, '|')};{LinkTo}"; //Строка для формата csv, delimetr = ';'

        public string ToString(List<string> list, char delimetr) //Перевод спика в строку 
        {
            string outStr = "";
            foreach (var item in list)
            {
                outStr += item + delimetr.ToString();
            }
            outStr = outStr.Remove(outStr.Length - 1);
            return outStr;
        }

    }
}
