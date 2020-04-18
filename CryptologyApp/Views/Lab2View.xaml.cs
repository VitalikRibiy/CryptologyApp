using CryptologyApp.Models;
using CryptologyApp.ViewModels;
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
    /// Interaction logic for Lab2View.xaml
    /// </summary>
    public partial class Lab2View : Window
    {

        public Lab2View()
        {
            InitializeComponent();
        }

        private void ShowAuthor(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Consts.author);
        }

        private void Open_FindKeyWindow(object sender, RoutedEventArgs e)
        {
            MessageBox.Show((DataContext as Lab2ViewModel).Attack());
        }
    }
}
