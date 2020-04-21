using CryptologyApp.Models;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using static CryptologyApp.Models.Consts;

namespace CryptologyApp.ViewModels
{
    public class Lab3ViewModel: INotifyPropertyChanged
    {
        private CryptionOption _selectedOption;
        public CryptionOption SelectedOption
        {
            get { return _selectedOption; }
            set
            {
                _selectedOption = value;
                Act();
            }
        }

        public EncryptionMachine EncryptionMachine { get; }

        private string _inputString;
        public string InputString
        {
            get { return _inputString; }
            set
            {
                _inputString = value;
                OnPropertyChanged();
                //Act();
            }
        }

        private string _exprotString { get; set; }
        public string ExportString
        {
            get { return _exprotString; }
            set
            {
                _exprotString = value;
                OnPropertyChanged();
            }
        }

        public Lab3ViewModel()
        {
            EncryptionMachine = new EncryptionMachine();
        }

        #region Commands
        public ICommand OpenFileCommand
        {
            get
            {
                return new DelegateCommand(ExecuteOpenFileCommand, CanExecuteOpenFileCommand);
            }
        }

        public ICommand SaveFileCommand
        {
            get
            {
                return new DelegateCommand(ExecuteSaveFileCommand, CanExecuteSaveFileCommand);
            }
        }

        public ICommand ActCommand
        {
            get
            {
                return new DelegateCommand(ExecuteActCommand, CanExecuteActCommand);
            }
        }

        public bool CanExecuteOpenFileCommand()
        {
            return true;
        }

        public void ExecuteOpenFileCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var inp = File.ReadAllText(openFileDialog.FileName);
                InputString = inp;
                var bytes = File.ReadAllBytes(openFileDialog.FileName);
                File.WriteAllText(@"C:\Users\Vitalii Ribii\Desktop\KeyText.txt", EncryptionMachine.XORKey.ToUpperInvariant());
                var key = File.ReadAllBytes(@"C:\Users\Vitalii Ribii\Desktop\KeyText.txt");
                if (CryptionOption.Encrypt == SelectedOption)
                {
                    File.WriteAllBytes(@"C:\Users\Vitalii Ribii\Desktop\Output.txt", EncryptionMachine.Encrypt(bytes,key));
                    ExportString = File.ReadAllText(@"C:\Users\Vitalii Ribii\Desktop\Output.txt");
                }
                else
                {
                    File.WriteAllBytes(@"C:\Users\Vitalii Ribii\Desktop\Output.txt", EncryptionMachine.Decrypt(bytes, key));
                    ExportString = File.ReadAllText(@"C:\Users\Vitalii Ribii\Desktop\Output.txt");
                }
            }
        }

        public bool CanExecuteSaveFileCommand()
        {
            return true;
        }

        public void ExecuteSaveFileCommand()
        {
            SaveFileDialog openFileDialog = new SaveFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(openFileDialog.FileName, ExportString);
            }

        }

        public bool CanExecuteActCommand()
        {
            return true;
        }

        public void ExecuteActCommand()
        {
            Act();
        }

        #endregion

        #region Methods
        private void Act()
        {
            try
            {
                if (InputString == null || string.IsNullOrWhiteSpace(EncryptionMachine.XORKey) ) return;
            }
            catch (Exception e)
            {
                MessageBox.Show("Invalid input data,please try again");
            }

        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
