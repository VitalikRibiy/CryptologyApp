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
        #region Cesar

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
        private string cesarAlphabet = alphabetEng;
        public string EncryptCesar(string message)
        {
            var encoded = string.Empty;
            foreach(char el in message)
            {
                if (!cesarAlphabet.Contains(el))
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
                if (!cesarAlphabet.Contains(el))
                {
                    decoded += el;
                }
                else
                {
                    decoded += GetEncrCesarChar(el, cesarAlphabet.Length - Key);
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

        public void setCesarLanguage(Languages language)
        {
            if (language == Languages.English)
            {
                cesarAlphabet = alphabetEng;
            }
            else if (language == Languages.Ukrainian)
            {
                cesarAlphabet = alphabetUkr;
            }
        }

        public char GetEncrCesarByte(char el,int key)
        {
            return byteCharacteres[(byteCharacteres.IndexOf(el) + key) % byteCharacteres.Length];
        }

        public char GetEncrCesarChar(char el,int key)
        {
            return cesarAlphabet[(cesarAlphabet.IndexOf(el) + key) % cesarAlphabet.Length];
        }

        #endregion

        #region Trithemius

        private int _trithemiusKey;
        public int TrithemiusKey
        {
            get { return _trithemiusKey; }
            set
            {
                _trithemiusKey = value;
                OnPropertyChanged();
            }
        }
        private string trithemiusAlphabet = alphabetEng;

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
                if (trithemiusAlphabet.Contains(x))
                {
                    exp += trithemiusAlphabet[Mod(((trithemiusAlphabet.IndexOf(x)) + CalculateK(count)), trithemiusAlphabet.Length)];
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
                if(trithemiusAlphabet.Contains(y))
                {
                    exp += trithemiusAlphabet[Mod(((trithemiusAlphabet.IndexOf(y)) + trithemiusAlphabet.Length - (CalculateK(count) % trithemiusAlphabet.Length)), trithemiusAlphabet.Length)];
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
                    return trithemiusAlphabet.IndexOf(Motto[i]);
                default:
                    return 0;
            }
        }

        public string FindKey(string inputDecoded, string inputEncoded)
        {
            if (keyType == KeyTypesTrithemius.Lineal && inputDecoded.Length >= 2)
            {
                var n = trithemiusAlphabet.Length;

                var b = Mod(trithemiusAlphabet.IndexOf(inputEncoded[0]) -
                        trithemiusAlphabet.IndexOf(inputDecoded[0]), n);

                var a = Mod(trithemiusAlphabet.IndexOf(inputEncoded[1]) -
                        trithemiusAlphabet.IndexOf(inputDecoded[1]) - b, n);

                return $"a={a}, b={b}";
            }
            else if (keyType == KeyTypesTrithemius.Squared && inputDecoded.Length >= 3)
            {
                var n = trithemiusAlphabet.Length;

                var c = Mod(trithemiusAlphabet.IndexOf(inputEncoded[0]) -
                        trithemiusAlphabet.IndexOf(inputDecoded[0]), n);

                var ab = trithemiusAlphabet.IndexOf(inputEncoded[1]) -
                        trithemiusAlphabet.IndexOf(inputDecoded[1]) - c;

                var a = Mod((trithemiusAlphabet.IndexOf(inputEncoded[2]) -
                        trithemiusAlphabet.IndexOf(inputDecoded[2]) - c - 2 * ab) / 2, n);

                var b = Mod(trithemiusAlphabet.IndexOf(inputEncoded[1]) -
                        trithemiusAlphabet.IndexOf(inputDecoded[1]) - c - a, n);

                return $"a={a}, b={b}, c={c}";
            }
            else
            {
                return ":(";
            }
        }

        public void setTrithemiusLanguage(Languages language)
        {
            if (language == Languages.English)
            {
                trithemiusAlphabet = alphabetEng;
            }
            else if (language == Languages.Ukrainian)
            {
                trithemiusAlphabet = alphabetUkr;
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
            var first = Convert.FromBase64String(input);
            var second = Convert.FromBase64String(XORKey);
            byte[] output = Encrypt(first,second);

            //for (int i = 0; i < first.Length; i++)
            //{
            //    output[i] = (byte)(first[i] ^ second[i % second.Length]);
            //}
            //var s = Encoding.ASCII.GetString(output);
            File.WriteAllBytes("CashText.txt", output);
            return File.ReadAllText("CashText.txt");
        }

        public byte[] Encrypt(byte[] text, byte[] key)
        {
            if (text.Length <= key.Length)
            {
                var result = new byte[text.Length];
                for (int i = 0; i < text.Length; i++)
                {
                    result[i] = (byte)(text[i] ^ key[i]);
                }
                return result;
            }
            else
            {
                var newKeyList = key.ToList();
                while (newKeyList.Count < text.Length)
                {
                    newKeyList.InsertRange(0, key.ToList());
                }

                var result = new byte[text.Length];
                for (int i = 0; i < text.Length; i++)
                {
                    result[i] = (byte)(text[i] ^ newKeyList[i]);
                }
                return result;
            }
        }

        public byte[] Decrypt(byte[] text, byte[] key)
        {
            if (text.Length <= key.Length)
            {
                var result = new byte[text.Length];
                for (int i = 0; i < text.Length; i++)
                {
                    result[i] = (byte)(text[i] ^ key[i]);
                }
                return result;
            }
            else
            {
                var newKeyList = key.ToList();
                while (newKeyList.Count < text.Length)
                {
                    newKeyList.InsertRange(0, key.ToList());
                }

                var result = new byte[text.Length];
                for (int i = 0; i < text.Length; i++)
                {
                    result[i] = (byte)(text[i] ^ newKeyList[i]);
                }
                return result;
            }
        }

        //public string EncryptDecryptXOR(string input)
        //{
        //    File.WriteAllText("CashFile.txt", input);
        //    var first = File.ReadAllBytes("CashFile.txt");
        //    File.WriteAllText("KeyFile.txt", XORKey);
        //    var second = File.ReadAllBytes("KeyFile.txt");
        //    byte[] output = new byte[first.Length];

        //    for (int i = 0; i < input.Length; i++)
        //    {
        //        output[i] = (byte)(input[i] ^ second[i % second.Length]);
        //    }
        //    File.WriteAllBytes("ResultFile.txt", output);
        //    return File.ReadAllText("ResultFile.txt");
        //}

        public void StratchKey(string input)
        {
            if (XORKey == null || XORKey.Length == 0) return;
            while (XORKey.Length < input.Length)
            {
                XORKey += XORKey;
            }
        }

        #endregion

        #region Vigenere

        private string _vigenereKey;
        public string VigenereKey
        {
            get { return _vigenereKey; }
            set
            {
                _vigenereKey = value;
                OnPropertyChanged();
            }
        }

        private static int VMod(int a, int b)
        {
            return (a % b + b) % b;
        }

        public string VigenereCipher(string input, bool encipher)
        {
            for (int i = 0; i < VigenereKey.Length; ++i)
                if (!char.IsLetter(VigenereKey[i]))
                    return null; // Error

            string output = string.Empty;
            int nonAlphaCharCount = 0;

            for (int i = 0; i < input.Length; ++i)
            {
                if (char.IsLetter(input[i]))
                {
                    bool cIsUpper = char.IsUpper(input[i]);
                    char offset = cIsUpper ? 'A' : 'a';
                    int keyIndex = (i - nonAlphaCharCount) % VigenereKey.Length;
                    int k = (cIsUpper ? char.ToUpper(VigenereKey[keyIndex]) : char.ToLower(VigenereKey[keyIndex])) - offset;
                    k = encipher ? k : -k;
                    char ch = vigenereAlphabet[(VMod(((input[i] + k) - offset), vigenereAlphabet.Length)) + offset];
                    output += ch;
                }
                else
                {
                    output += input[i];
                    ++nonAlphaCharCount;
                }
            }

            return output;
        }

        private string vigenereAlphabet = alphabetEng;

        public void setVigenereLanguage(Languages language)
        {
            if (language == Languages.English)
            {
                vigenereAlphabet = alphabetEng;
            }
            else if (language == Languages.Ukrainian)
            {
                vigenereAlphabet = alphabetUkr;
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
