using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static CryptologyApp.Models.Consts;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CryptologyApp.Models
{
    public class EncryptionMachine : INotifyPropertyChanged
    {
        private int _key;
        public int Key 
        { 
            get { return _key; }
            set
            {
                _key = value;
                OnPropertyChanged();
            }
        }
        private string alphabet = alphabetEng;

        #region Cesar
        public string EncryptCesar(string message)
        {
            var encoded = string.Empty;
            foreach(char el in message)
            {
                if (!alphabet.Contains(el))
                {
                    encoded += el;
                }
                else
                {
                    encoded += GetEncrCesarChar(el, Key);
                }
            }
            return encoded;
        }

        public string DecryptCesar(string message)
        {
            var decoded = string.Empty;
            foreach (char el in message)
            {
                if (!alphabet.Contains(el))
                {
                    decoded += el;
                }
                else
                {
                    decoded += GetEncrCesarChar(el, alphabet.Length - Key);
                }
            }
            return decoded;
        }

        public string EncryptCesarBytes(string message)
        {
            var encoded = string.Empty;
            foreach (char el in message)
            {
                if (!byteCharacteres.Contains(el))
                {
                    encoded += el;
                }
                else
                {
                    encoded += GetEncrCesarByte(el, Key);
                }
            }
            return encoded;
        }

        public string DecryptCesarBytes(string message)
        {
            var decoded = string.Empty;
            foreach (char el in message)
            {
                if (!byteCharacteres.Contains(el))
                {
                    decoded += el;
                }
                else
                {
                    decoded += GetEncrCesarByte(el, byteCharacteres.Length - Key);
                }
            }
            return decoded;
        }

        public void setLanguage(Languages language)
        {
            if (language == Languages.English)
            {
                alphabet = alphabetEng;
            }
            else if (language == Languages.Ukrainian)
            {
                alphabet = alphabetUkr;
            }
        }

        public char GetEncrCesarByte(char el,int key)
        {
            return byteCharacteres[(byteCharacteres.IndexOf(el) + key) % byteCharacteres.Length];
        }

        public char GetEncrCesarChar(char el,int key)
        {
            return alphabet[(alphabet.IndexOf(el) + key) % alphabet.Length];
        }

        #endregion

        #region Trithemius

        public KeyTypesTrithemius keyType;

        public int[] lineaKey = new int[2];

        public int[] squadKey = new int[3];

        public string Motto { get; set; }

        public string EncryptTrithemius(string input)
        {
            string exp = string.Empty;
            var count = 1;
            foreach(var x in input)
            {
                if (alphabet.Contains(x))
                {
                    exp += alphabet[((alphabet.IndexOf(x)) + CalculateK(count)) % alphabet.Length];
                }
                else
                {
                    exp += x;
                }
            }
            return exp;
        }

        public string DecryptTrithemius(string input)
        {
            string exp = string.Empty;
            var count = 1;
            foreach (var y in input)
            {
                if(alphabet.Contains(y))
                {
                    exp += alphabet[((alphabet.IndexOf(y)) + alphabet.Length - (CalculateK(count) % alphabet.Length)) % alphabet.Length];
                }
                else
                {
                    exp += y;
                }
            }
            return exp;
        }

        public int CalculateK(int i)
        {
            switch(keyType)
            {
                case KeyTypesTrithemius.Lineal:
                    return i * lineaKey[0] + lineaKey[1];
                case KeyTypesTrithemius.Squared:
                    return squadKey[0] * squadKey[0] + squadKey[1] * i + squadKey[2];
                case KeyTypesTrithemius.Motto:
                    while(Motto.Length < i)
                    {
                        Motto += Motto;
                    }
                    return alphabet.IndexOf(Motto[i-1]);
                default:
                    return 0;
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
