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
    public class Lab2ViewModel : INotifyPropertyChanged
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

        private KeyTypesTrithemius _keyType = KeyTypesTrithemius.Lineal;
        public KeyTypesTrithemius KeyType 
        { 
            get { return _keyType; }
            set
            {
                _keyType = value;
                EncryptionMachine.keyType = _keyType;
                switch (_keyType)
                {
                    case KeyTypesTrithemius.Lineal:
                        LinealKeyVisibility = Visibility.Visible;
                        SquadKeyVisibility = Visibility.Collapsed;
                        MottoKeyVisibility = Visibility.Collapsed;
                        break;
                    case KeyTypesTrithemius.Squared:
                        LinealKeyVisibility = Visibility.Collapsed;
                        SquadKeyVisibility = Visibility.Visible;
                        MottoKeyVisibility = Visibility.Collapsed;
                        break;
                    case KeyTypesTrithemius.Motto:
                        LinealKeyVisibility = Visibility.Collapsed;
                        SquadKeyVisibility = Visibility.Collapsed;
                        MottoKeyVisibility = Visibility.Visible;
                        break;
                }
                OnPropertyChanged();
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

        private Visibility _linealKeyVisibility = Visibility.Visible;
        public Visibility LinealKeyVisibility
        {
            get { return _linealKeyVisibility; }
            set
            {
                _linealKeyVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _squadKeyVisibility = Visibility.Collapsed;
        public Visibility SquadKeyVisibility
        {
            get { return _squadKeyVisibility; }
            set
            {
                _squadKeyVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _mottoKeyVisibility = Visibility.Collapsed;
        public Visibility MottoKeyVisibility
        {
            get { return _mottoKeyVisibility; }
            set
            {
                _mottoKeyVisibility = value;
                OnPropertyChanged();
            }
        }

        #region Lineal
        private int _linealA;
        public int LinealA
        {
            get { return _linealA; }
            set
            {
                _linealA = value;
                EncryptionMachine.lineaKey[0] = _linealA;
                Act();
            }
        }

        private int _linealB;
        public int LinealB
        {
            get { return _linealB; }
            set
            {
                _linealB = value;
                EncryptionMachine.lineaKey[1] = _linealB;
                Act();
            }
        }
        #endregion

        #region Squad
        private int _squadA;
        public int SquadA
        {
            get { return _squadA; }
            set
            {
                _squadA = value;
                EncryptionMachine.squadKey[0] = _squadA;
                Act();
            }
        }

        private int _squadB;
        public int SquadB
        {
            get { return _squadB; }
            set
            {
                _squadB = value;
                EncryptionMachine.squadKey[1] = _squadB;
                Act();
            }
        }

        private int _squadC;
        public int SquadC
        {
            get { return _squadC; }
            set
            {
                _squadC = value;
                EncryptionMachine.squadKey[2] = _squadC;
                Act();
            }
        }
        #endregion

        public Lab2ViewModel()
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
                switch(SelectedOption)
                {
                    case CryptionOption.Encrypt:
                        ExportString = EncryptionMachine.EncryptTrithemius(InputString);
                        break;
                    case CryptionOption.Decrypt:
                        ExportString = EncryptionMachine.DecryptTrithemius(InputString);
                        break;
                }
            }
            catch (Exception e)
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
