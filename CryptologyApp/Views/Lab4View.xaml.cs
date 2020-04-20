using CryptologyApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CryptologyApp.Views
{
    /// <summary>
    /// Interaction logic for Lab4View.xaml
    /// </summary>
    public partial class Lab4View : Window
    {
        public Lab4View()
        {
            InitializeComponent();
        }

        private void ShowAuthor(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Consts.author);
        }
    }
}
