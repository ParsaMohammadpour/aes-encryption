using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Encoding_Decoding
{
    class Program
    {
        static void Main(string[] args)
        {
            string myPlainText = "123456";

            string key = "!Hs$Fsu%";

            string cipherText = CryptoGraphy.Encrypt(myPlainText, key);
            
            Console.WriteLine("Cipher Text :" + cipherText);

            string newPlainText = CryptoGraphy.Decrypt(cipherText, key);
            
            Console.WriteLine("Plain Text: " + newPlainText);
            
            Console.WriteLine(newPlainText.Equals(myPlainText));
        }
    }

    static class CryptoGraphy
    {
        public static string Encrypt(string plainText, string key)
        {
            byte[] cipherTextByteArray;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.Unicode.GetBytes(key);
                aes.IV = new byte[16];
                aes.Mode = CipherMode.CBC;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        cipherTextByteArray = memoryStream.ToArray();
                    }
                }
            }
            string cipherText = Convert.ToBase64String(cipherTextByteArray);
            
            return cipherText;
        }

        public static string Decrypt(string cipherText, string key)
        {
            byte[] cipherTextArray = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.Unicode.GetBytes(key);
                aes.IV = new byte[16];
                aes.Mode = CipherMode.CBC;
                
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(cipherTextArray))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            string PlainText = streamReader.ReadToEnd();

                            return PlainText;
                        }
                    }
                }
            }
        }
    }
}