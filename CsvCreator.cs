using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyParser
{
    internal class CsvCreator
    {
        internal CsvCreator(string path, string csvOutput, bool IsConcatNeeded = false)
        {
            if (File.Exists(path) && IsConcatNeeded)
            {
                string inputText = "";
                using (FileStream fstreamRead = File.OpenRead(path))
                {
                    byte[] buffer = new byte[fstreamRead.Length];
                    fstreamRead.Read(buffer, 0, buffer.Length);
                    inputText = Encoding.Default.GetString(buffer);
                    csvOutput = inputText + csvOutput;
                    fstreamRead.Close();
                }
             }
            using (FileStream fstreamWrite = new FileStream(path, FileMode.OpenOrCreate)) // создание csv-файла
            {
                csvOutput = "Город;Хлеб;Название;Цена, руб;Старая цена, руб;Наличие;Картинки;Ссылка\n" + csvOutput; //columns for csv
                byte[] input = Encoding.Default.GetBytes(csvOutput + "\n");
                fstreamWrite.Write(input, 0, input.Length);
                fstreamWrite.Close();
            }
        }

    }
}
