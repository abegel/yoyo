using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace YoYoPC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PCListener.PreviewKeyDown += PCListener_PreviewKeyDown;
        }

        private void PCListener_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // get text
                TextBox tb = e.Source as TextBox;
                int caret = tb.SelectionStart;
                int currentLine = tb.GetLineIndexFromCharacterIndex(caret);
                string lineText = tb.GetLineText(currentLine);
                if (lineText.Trim().Length == 0)
                {
                    e.Handled = true;
                }
                if (currentLine < tb.LineCount - 1)
                {
                    tb.SelectionStart++;
                    e.Handled = true;
                }
                
            }
        }


    }
}
