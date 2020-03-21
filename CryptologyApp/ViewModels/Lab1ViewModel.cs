using CryptologyApp.Models;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using static CryptologyApp.Models.Consts;

namespace CryptologyApp.ViewModels
{
    public class Lab1ViewModel : INotifyPropertyChanged
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

        private Languages _language;
        public Languages Language
        {
            get { return _language; }
            set
            {
                _language = value;
                EncryptionMachine.setLanguage(_language);
                Act();
            }
        }

        private EncryptType encryptType;
        public EncryptType EncryptType
        {
            get { return encryptType; }
            set
            {
                encryptType = value;
                //(encryptType);
                Act();
            }
        }

        public EncryptionMachine EncryptionMachine { get; }

        private string _inputString;
        public string InputString {
            get { return _inputString; }
            set
            {
                _inputString = value;
                OnPropertyChanged();
                Act();
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

        public Lab1ViewModel()
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

        public ICommand PreviousCommand
        {
            get
            {
                return new DelegateCommand(ExecutePreviousCommand, CanExecutePreviousCommand);
            }
        }

        public ICommand NextCommand
        {
            get
            {
                return new DelegateCommand(ExecuteNextCommand, CanExecuteNextCommand);
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
                if (EncryptType == EncryptType.Characters)
                    InputString = File.ReadAllText(openFileDialog.FileName);
                if (EncryptType == EncryptType.Bytes)
                    InputString = GetBase64String(openFileDialog.FileName);
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
                if (EncryptType == EncryptType.Characters)
                    File.WriteAllText(openFileDialog.FileName, ExportString);
                if (EncryptType == EncryptType.Bytes)
                    File.WriteAllBytes(openFileDialog.FileName, GetFromBase64String(ExportString));
            }
               
        }

        public bool CanExecutePreviousCommand()
        {
            return true;
        }

        public void ExecutePreviousCommand()
        {
            EncryptionMachine.Key--;
            Act();
        }

        public bool CanExecuteNextCommand()
        {
            return true;
        }

        public void ExecuteNextCommand()
        {
            EncryptionMachine.Key++;
            Act();
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
                if (InputString == null) return;
                if (EncryptType == EncryptType.Bytes)
                {
                    if (SelectedOption == CryptionOption.Encrypt)
                    {
                        ExportString = EncryptionMachine.EncryptBytes(InputString);
                    }
                    else if (SelectedOption == CryptionOption.Decrypt)
                    {
                        ExportString = EncryptionMachine.DecryptBytes(InputString);
                    }
                }
                else
                {
                    if (SelectedOption == CryptionOption.Encrypt)
                    {
                        ExportString = EncryptionMachine.Encrypt(InputString);
                    }
                    else if (SelectedOption == CryptionOption.Decrypt)
                    {
                        ExportString = EncryptionMachine.Decrypt(InputString);
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Invalid input data,please try again");
            }

        }

        public string GetBase64String(string PathToFile)
        {
            try
            {
                byte[] binData = File.ReadAllBytes(PathToFile);
                return Convert.ToBase64String(binData);
            }
            catch
            {
                return "";
            }

        }

        public byte[] GetFromBase64String(string base64string)
        {
            try
            {
                return Convert.FromBase64String(base64string);
            }
            catch
            {
                return (byte[])null;
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
