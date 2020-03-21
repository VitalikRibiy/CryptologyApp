using CryptologyApp.Views;
using Microsoft.VisualStudio.PlatformUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

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

        public ICommand OpenNewWindowCommand 
        {
            get
            {
                return new DelegateCommand(ExecuteOpenNewWindowCommand,CanExecuteOpenNewWindowCommand);
            }
        }

        public bool CanExecuteOpenNewWindowCommand()
        {
            return  Lab1View.Visibility != System.Windows.Visibility.Visible;
        }

        public void ExecuteOpenNewWindowCommand()
        {
            Lab1View.Show();
        }
    }
}
