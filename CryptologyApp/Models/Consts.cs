using System;
using System.Collections.Generic;
using System.Text;

namespace CryptologyApp.Models
{
    public class Consts
    {
        public static string author = "Developed by Ribii Vitalii \n\t 2020";
        public static string alphabetEng = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLowerInvariant() + "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string alphabetUkr = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ".ToLowerInvariant() + "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";
        public static string byteCharacteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        public static char[] specialSymbols = {'\n',' '};
        public enum CryptionOption
        {
            Encrypt = 0,
            Decrypt = 1
        }

        public enum Languages
        {
            English = 0,
            Ukrainian = 1,
        }

        public enum EncryptType
        {
            Characters = 0,
            Bytes = 1
        }
    }
}
