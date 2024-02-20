using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Replacer
{
    public class ReplaceDto
    {
        public string param { get; set; }
        public string richTextList { get; set; }
        public string richTextQry { get; set; }
        public bool sql { get; set; }
    }

    public class LineItemDto
    {
        public string input { get; set; }
        public string lineCount { get; set; }
        public bool bigList { get; set; }
        public bool removeLast { get; set; }
    }
    public static class Helpers
    {
        public static Paragraph Replace(ReplaceDto replace)
        {
            Paragraph result = new Paragraph();
            List<string> strlist = new List<string>();
            string Output = string.Empty;

            string[] listLines = replace.richTextList.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            );

            string[] qryLines = replace.richTextQry.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            );

            foreach (var item in listLines)
            {
                if (item != "")
                {
                    strlist.Add(item);
                }
            }

            foreach (var strName in strlist)
            {
                foreach (var item in qryLines)
                {
                    if (item == "")
                    {
                        continue;
                    }
                    if (replace.sql)
                    {
                        string str = item.Replace(@"[]", @"[" + strName + @"]");
                        Output += str + Environment.NewLine;
                    }
                    else
                    {
                        string str = item.Replace(replace.param, strName);
                        Output += str + Environment.NewLine;
                    }

                }
            }
            result = new Paragraph(new Run(Output));
            return result;
        }

        public static Paragraph LineItem(LineItemDto line)
        {
            Paragraph result = new Paragraph();
            int lineCount = 0, count = 0;
            string input = string.Empty, Output = string.Empty, temp = string.Empty;

            if (!int.TryParse(line.lineCount, out lineCount))
            {
                MessageBox.Show("Please enter a number for the line count");
                return result;
            }

            input = line.input;
            input = input.Replace("\n", string.Empty);
            input = input.Replace("\r", string.Empty);
            var list = input.Split(',');

            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Please enter a text to be formatted");
                return result;
            }

            if (line.bigList)
            {
                int c = list.Count();
                if (c <= 20)
                {
                    lineCount = 5;
                }
                if (c > 20 && c <= 50)
                {
                    lineCount = 10;
                }
                if (c > 50)
                {
                    lineCount = 20;
                }
                MessageBox.Show($"Large list selected, formatting for {lineCount} items per line");
            }

            foreach (var item in list)
            {
                if (item == "")
                {
                    break;
                }
                if (count == lineCount)
                {
                    Output += temp + Environment.NewLine;
                    temp = string.Empty;
                    count = 0;
                }
                temp += item + ",";
                count++;
            }
            Output += temp + Environment.NewLine;

            if (line.removeLast)
            {
                Output = Output.Remove(Output.Count() - 2);
            }

            result = new Paragraph(new Run(Output));
            return result;
        }
    }
}
