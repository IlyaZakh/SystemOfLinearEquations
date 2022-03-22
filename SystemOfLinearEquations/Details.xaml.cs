using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace SystemOfLinearEquations
{
    /// <summary>
    /// Логика взаимодействия для Details.xaml
    /// </summary>
    public partial class Details : Window
    {
        public bool closing = false;
        public Details()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!closing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "RichText Files (*.rtf)|*.rtf|Текстовый файл (*.txt)|*.txt|XAML Files (*.xaml)|*.xaml|All files (*.*)|*.*";
            if (sfd.ShowDialog() == true)
            {
                TextRange doc = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                using (FileStream fs = File.Create(sfd.FileName))
                {
                    if (System.IO.Path.GetExtension(sfd.FileName).ToLower() == ".rtf")
                        doc.Save(fs, DataFormats.Rtf);
                    else if (System.IO.Path.GetExtension(sfd.FileName).ToLower() == ".txt")
                        doc.Save(fs, DataFormats.Text);
                    else if ((System.IO.Path.GetExtension(sfd.FileName).ToLower() == ".xaml"))
                        doc.Save(fs, DataFormats.Xaml);
                    else
                        doc.Save(fs, DataFormats.Text);
                }
            }
        }
    }
}
