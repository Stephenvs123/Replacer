using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Replacer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Replace()
        {
            List<string> strlist = new List<string>();
            string param = textBox1.Text;

            string richTextList = new TextRange(rtbList.Document.ContentStart, rtbList.Document.ContentEnd).Text;
            string[] listLines = richTextList.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            );
            string richTextQry = new TextRange(rtbQuery.Document.ContentStart, rtbQuery.Document.ContentEnd).Text;
            string[] qryLines = richTextQry.Split(
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
                    if ((bool)checkBox1.IsChecked)
                    {
                        string str = item.Replace(@"[]", @"[" + strName + @"]");
                        rtbResult.Document.Blocks.Add(new Paragraph(new Run(str + Environment.NewLine)));
                    }
                    else
                    {
                        string str = item.Replace(param, strName);
                        rtbResult.Document.Blocks.Add(new Paragraph(new Run(str)));
                    }

                }
            }
        }

        private void Commit_Click(object sender, RoutedEventArgs e)
        {
            rtbResult.Document.Blocks.Clear();
            Replace();
            if (cbPerformLine.IsChecked == true)
            {
                SetText();
                LineItem();
                tbcMain.SelectedIndex++;
                rtcLineOut.Focus();
            }
            
        }

        private void Clear_Results_Click(object sender, RoutedEventArgs e)
        {
            rtbResult.Document.Blocks.Clear();
        }

        private void Clear_Query_Click(object sender, RoutedEventArgs e)
        {
            rtbQuery.Document.Blocks.Clear();
        }

        private void Clear_QRY_Results_Click(object sender, RoutedEventArgs e)
        {
            rtbResult.Document.Blocks.Clear();
        }

        private void Clear_Object_Click(object sender, RoutedEventArgs e)
        {
            rtbList.Document.Blocks.Clear();
        }

        private void Clear_All_Click(object sender, RoutedEventArgs e)
        {
            rtbList.Document.Blocks.Clear();
            rtbResult.Document.Blocks.Clear();
            rtbQuery.Document.Blocks.Clear();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LineItem();
        }

        private void SetText()
        {
            rtcLineOut.Document.Blocks.Clear();
            rtcLineIn.Document.Blocks.Clear();
            string input = new TextRange(rtbResult.Document.ContentStart, rtbResult.Document.ContentEnd).Text;
            rtcLineIn.Document.Blocks.Add(new Paragraph(new Run(input)));
        }

        private void LineItem()
        {
            int lineCount = 0, count = 0;
            string input = string.Empty, Output = string.Empty, temp = string.Empty;

            if (!int.TryParse(txtLineCount.Text, out lineCount))
            {
                MessageBox.Show("Please enter a number for the line count");
                return;
            }

            input = new TextRange(rtcLineIn.Document.ContentStart, rtcLineIn.Document.ContentEnd).Text;
            input = input.Replace("\n", string.Empty);
            input = input.Replace("\r", string.Empty);
            var list = input.Split(',');

            if (cbDefaultBig.IsChecked == true && lineCount != 4)
            {
                int c = list.Count();
                if (c <= 20)
                {
                    lineCount = 4;
                }
                if (c > 20 && c <= 50 )
                {
                    lineCount = 10;
                }
                if (c > 50)
                {
                    lineCount = 20;
                }
            }

            foreach (var item in list)
            {
                if (item == "")
                {
                    break;
                }
                if (count == lineCount)
                {
                    Output += temp + '\n';
                    temp = string.Empty;
                    count = 0;
                }
                temp += item + ",";
                count++;
            }
            Output += temp + '\n';

            if (cbRemoveLast.IsChecked == true)
            {
                Output = Output.Remove(Output.Count() - 2);
            }

            rtcLineOut.Document.Blocks.Clear();

            rtcLineOut.Document.Blocks.Add(new Paragraph(new Run(Output)));
        }
    }
}
