using CryptologyApp.Views;
using Microsoft.VisualStudio.PlatformUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using CryptologyApp.Models;
using System.Windows;

namespace CryptologyApp.ViewModels
{
    public class MainViewModel
    {
        private Lab1View _lab1View;
        private Lab1View Lab1View
        { 
            get
            {
                if (_lab1View != null) return _lab1View;
                _lab1View = new Lab1View();
                _lab1View.Closed += (object o, EventArgs e) => { _lab1View = null; };
                return _lab1View;
            }
        }

        private Lab2View _lab2View;
        private Lab2View Lab2View
        {
            get
            {
                if (_lab2View != null) return _lab2View;
                _lab2View = new Lab2View();
                _lab2View.Closed += (object o, EventArgs e) => { _lab2View = null; };
                return _lab2View;
            }
        }

        public ICommand OpenNewWindowCommand 
        {
            get
            {
                return new DelegateCommand(ExecuteOpenNewWindowCommand,CanExecuteOpenNewWindowCommand);
            }
        }

        public bool CanExecuteOpenNewWindowCommand(object lab)
        {
            switch (Enum.Parse(typeof(Consts.Labs), lab.ToString().Split()[0]))
            {
                case Consts.Labs.Cesars:
                    return Lab1View.Visibility != System.Windows.Visibility.Visible;
                case Consts.Labs.Trithemius:
                    return Lab2View.Visibility != System.Windows.Visibility.Visible;
                default:
                    return false;
            }
            
        }

        public void ExecuteOpenNewWindowCommand(object lab)
        {
            switch(Enum.Parse(typeof(Consts.Labs), lab.ToString().Split()[0]))
            {
                case Consts.Labs.Cesars:
                    Lab1View.Show();
                    break;
                case Consts.Labs.Trithemius:
                    Lab2View.Show();
                    break;
                default:
                    MessageBox.Show("Wrong lab name");
                    break;
            }
        }
    }
}
