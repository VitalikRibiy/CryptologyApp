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

        public string Encrypt(string message)
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
                    encoded += GetEncrChar(el, Key);
                }
            }
            return encoded;
        }

        public string Decrypt(string message)
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
                    decoded += GetEncrChar(el, alphabet.Length - Key);
                }
            }
            return decoded;
        }

        public string EncryptBytes(string message)
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
                    encoded += GetEncrByte(el, Key);
                }
            }
            return encoded;
        }

        public string DecryptBytes(string message)
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
                    decoded += GetEncrByte(el, byteCharacteres.Length - Key);
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

        public char GetEncrByte(char el,int key)
        {
            return byteCharacteres[(byteCharacteres.IndexOf(el) + key) % byteCharacteres.Length];
        }

        public char GetEncrChar(char el,int key)
        {
            return alphabet[(alphabet.IndexOf(el) + key) % alphabet.Length];
        }

        public string GetByteMessage(string message64,int key)
        {
            var bytes = Convert.FromBase64String(message64);
            byte[] result = new byte[bytes.Length];
            foreach(var b in bytes)
            {
                result[result.Length] = (byte)((int)b + 1);
            }
            return Convert.ToBase64String(result);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
