using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static CryptologyApp.Models.Consts;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using Microsoft.Win32;

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

        private static int Mod(int dividend, int divisor)
        {
            var result = dividend % divisor;

            if (result >= 0)
            {
                return result;
            }
            else
            {
                return Math.Abs(result + divisor);
            }
        }

        public string EncryptTrithemius(string input)
        {
            string exp = string.Empty;
            var count = 0;
            foreach(var x in input)
            {
                if (alphabet.Contains(x))
                {
                    exp += alphabet[Mod(((alphabet.IndexOf(x)) + CalculateK(count)), alphabet.Length)];
                }
                else
                {
                    exp += x;
                }
                count++;
            }
            return exp;
        }

        public string DecryptTrithemius(string input)
        {
            string exp = string.Empty;
            var count = 0;
            foreach (var y in input)
            {
                if(alphabet.Contains(y))
                {
                    exp += alphabet[Mod(((alphabet.IndexOf(y)) + alphabet.Length - (CalculateK(count) % alphabet.Length)), alphabet.Length)];
                }
                else
                {
                    exp += y;
                }
                count++;
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
                    if (Motto == null || Motto.Length == 0) return 0;
                    while(Motto.Length < i)
                    {
                        Motto += Motto;
                    }
                    return alphabet.IndexOf(Motto[i]);
                default:
                    return 0;
            }
        }

        public string FindKey(string inputDecoded, string inputEncoded)
        {
            if (keyType == KeyTypesTrithemius.Lineal && inputDecoded.Length >= 2)
            {
                var n = alphabet.Length;

                var b = Mod(alphabet.IndexOf(inputEncoded[0]) -
                        alphabet.IndexOf(inputDecoded[0]), n);

                var a = Mod(alphabet.IndexOf(inputEncoded[1]) -
                        alphabet.IndexOf(inputDecoded[1]) - b, n);

                return $"a={a}, b={b}";
            }
            else if (keyType == KeyTypesTrithemius.Squared && inputDecoded.Length >= 3)
            {
                var n = alphabet.Length;

                var c = Mod(alphabet.IndexOf(inputEncoded[0]) -
                        alphabet.IndexOf(inputDecoded[0]), n);

                var ab = alphabet.IndexOf(inputEncoded[1]) -
                        alphabet.IndexOf(inputDecoded[1]) - c;

                var a = Mod((alphabet.IndexOf(inputEncoded[2]) -
                        alphabet.IndexOf(inputDecoded[2]) - c - 2 * ab) / 2, n);

                var b = Mod(alphabet.IndexOf(inputEncoded[1]) -
                        alphabet.IndexOf(inputDecoded[1]) - c - a, n);

                return $"a={a}, b={b}, c={c}";
            }
            else
            {
                return ":(";
            }
        }

        #endregion

        #region XOR

        private string _xorKey;
        public string XORKey
        {
            get { return _xorKey; }
            set
            {
                _xorKey = value;
                OnPropertyChanged();
            }
        }
        
        public string EncryptDecryptXOR(string input)
        {
            //StratchKey(input);
            var first = Encoding.ASCII.GetBytes(input);
            var second = Encoding.ASCII.GetBytes(XORKey);
            byte[] output = new byte[first.Length];
            
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = (byte)(input[i] ^ second[i % second.Length]);
            }
            //SaveFileDialog openFileDialog = new SaveFileDialog();
            //if (openFileDialog.ShowDialog() == true)
            //{
            //    File.WriteAllBytes(openFileDialog.FileName, output);
            //}
            return Encoding.ASCII.GetString(output);
        }

        public void StratchKey(string input)
        {
            if (XORKey == null || XORKey.Length == 0) return;
            while (XORKey.Length < input.Length)
            {
                XORKey += XORKey;
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
