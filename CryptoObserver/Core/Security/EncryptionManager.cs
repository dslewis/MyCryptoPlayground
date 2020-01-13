using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Core.Security
{
  public class EncryptionManager
  {
    private const int SaltSize = 64;

    public static string Decrypt(string cipherText, string theKey)
    {
      if (string.IsNullOrWhiteSpace(cipherText) || string.IsNullOrWhiteSpace(theKey))
      {
        throw new ArgumentNullException();
      }
      var allTheBytes = Convert.FromBase64String(cipherText);
      var saltBytes = allTheBytes.Take(SaltSize).ToArray();
      var ciphertextBytes = allTheBytes.Skip(SaltSize).Take(allTheBytes.Length - SaltSize).ToArray();

      using (var keyDerivationFunction = new Rfc2898DeriveBytes(theKey, saltBytes))
      {
        var keyBytes = keyDerivationFunction.GetBytes(32);
        var ivBytes = keyDerivationFunction.GetBytes(16);

        return DecryptWithAes(ciphertextBytes, keyBytes, ivBytes);
      }
    }

    public static string Encrypt(string plainText, string key)
    {
      if (string.IsNullOrEmpty(plainText) || string.IsNullOrEmpty(key))
      {
        throw new ArgumentNullException();
      }
      using (var keyDerivationFunction = new Rfc2898DeriveBytes(key, SaltSize))
      {
        var saltBytes = keyDerivationFunction.Salt;
        var keyBytes = keyDerivationFunction.GetBytes(32);
        var ivBytes = keyDerivationFunction.GetBytes(16);

        using (var aesManaged = new AesManaged())
        {
          aesManaged.KeySize = 256;

          using (var encryptor = aesManaged.CreateEncryptor(keyBytes, ivBytes))
          {
            MemoryStream memoryStream = null;
            CryptoStream cryptoStream = null;

            return WriteMemoryStream(plainText, ref saltBytes, encryptor, ref memoryStream, ref cryptoStream);
          }
        }
      }
    }

    private static string DecryptWithAes(byte[] ciphertextBytes, byte[] keyBytes, byte[] ivBytes)
    {
      using (var aesManaged = new AesManaged())
      {
        using (var decryptor = aesManaged.CreateDecryptor(keyBytes, ivBytes))
        {
          MemoryStream memoryStream = null;

          try
          {
            memoryStream = new MemoryStream(ciphertextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var streamReader = new StreamReader(cryptoStream);

            return streamReader.ReadToEnd();
          }
          finally
          {
            if (memoryStream != null)
            {
              memoryStream.Dispose();
            }
          }
        }
      }
    }

    private static string WriteMemoryStream(string plainText, ref byte[] saltBytes, ICryptoTransform encryptor,
      ref MemoryStream memoryStream, ref CryptoStream cryptoStream)
    {
      try
      {
        memoryStream = new MemoryStream();

        try
        {
          cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

          using (var streamWriter = new StreamWriter(cryptoStream))
          {
            streamWriter.Write(plainText);
          }
        }
        finally
        {
          if (cryptoStream != null)
          {
            cryptoStream.Dispose();
          }
        }

        var cipherTextBytes = memoryStream.ToArray();
        Array.Resize(ref saltBytes, saltBytes.Length + cipherTextBytes.Length);
        Array.Copy(cipherTextBytes, 0, saltBytes, SaltSize, cipherTextBytes.Length);

        return Convert.ToBase64String(saltBytes);
      }
      finally
      {
        if (memoryStream != null)
        {
          memoryStream.Dispose();
        }
      }
    }
  }
}