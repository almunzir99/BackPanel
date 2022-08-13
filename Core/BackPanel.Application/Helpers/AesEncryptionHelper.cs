using System.Security.Cryptography;
using System.Text;

namespace BackPanel.Shared.Helpers;

public class AesEncryptionHelper
{
    public static string Encrypt(string key,string text)
    {
        var iv = new byte[16];
        byte[] array;
        using(var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key,aes.IV);
            using(var ms = new MemoryStream())
            {
                using(var cryptoStream = new CryptoStream((Stream)ms,encryptor,CryptoStreamMode.Write)){
                    using(var writer = new StreamWriter(cryptoStream))
                    {
                        writer.Write(text);
                    }
                    array = ms.ToArray();
                }
                
            }
        }
        return Convert.ToBase64String(array);
    }
    public static string Decrypt(string key,string cipherText){
        var iv = new byte[16];
        var buffer = Convert.FromBase64String(cipherText);
        using(var aes = Aes.Create())
        {
            aes.IV = iv;
            aes.Key = Encoding.UTF8.GetBytes(key);
            var decryptor = aes.CreateDecryptor(aes.Key,aes.IV);
            using(var ms = new MemoryStream(buffer))
            {
                using(var cryptoStream = new CryptoStream((Stream)ms,decryptor,CryptoStreamMode.Read))
                {
                    using(var reader = new StreamReader(cryptoStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

    }
}