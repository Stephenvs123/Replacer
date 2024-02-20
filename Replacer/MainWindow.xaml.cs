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

        private void Commit_Click(object sender, RoutedEventArgs e)
        {
            rtbResult.Document.Blocks.Clear();
            rtbResult.Document.Blocks.Add(Helpers.Replace(
                new ReplaceDto()
                {
                    param = textBox1.Text,
                    richTextList = new TextRange(rtbList.Document.ContentStart, rtbList.Document.ContentEnd).Text,
                    richTextQry = new TextRange(rtbQuery.Document.ContentStart, rtbQuery.Document.ContentEnd).Text,
                    sql = (bool)checkBox1.IsChecked
                }));

            if (cbPerformLine.IsChecked == true)
            {
                SetText();
                ProcLineItemMain();
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

        private void ProcLineItem_Click(object sender, RoutedEventArgs e)
        {
            ProcLineItemMain();
        }

        private void ProcLineItemMain()
        {
            rtcLineOut.Document.Blocks.Clear();
            rtcLineOut.Document.Blocks.Add(Helpers.LineItem(
                new LineItemDto()
                {
                    input = new TextRange(rtcLineIn.Document.ContentStart, rtcLineIn.Document.ContentEnd).Text,
                    bigList = cbDefaultBig.IsChecked == true,
                    removeLast = cbRemoveLast.IsChecked == true,
                    lineCount = txtLineCount.Text
                }));
        }

        private void SetText()
        {
            rtcLineOut.Document.Blocks.Clear();
            rtcLineIn.Document.Blocks.Clear();
            string input = new TextRange(rtbResult.Document.ContentStart, rtbResult.Document.ContentEnd).Text;
            rtcLineIn.Document.Blocks.Add(new Paragraph(new Run(input)));
        }
    }
}
