using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace UIF
{
    public enum EncodingType { ASCII = 0, UTF_7 = 1, UTF_8 = 2, UTF_32 = 3, UNICODE = 4, BIG_ENDIAN = 5 }

    //Src: https://www.c-sharpcorner.com/UploadFile/a85b23/text-encrypt-and-decrypt-with-a-specified-key/
    internal static class Crypto
    {
        static byte[] GetSalt(string saltString, EncodingType encodingType)
        {
            byte[] salt = null;
            if (encodingType == EncodingType.ASCII)
            {
                salt = Encoding.ASCII.GetBytes(saltString);
            }
            else if (encodingType == EncodingType.BIG_ENDIAN)
            {
                salt = Encoding.BigEndianUnicode.GetBytes(saltString);
            }
            else if (encodingType == EncodingType.UNICODE)
            {
                salt = Encoding.Unicode.GetBytes(saltString);
            }
            else if (encodingType == EncodingType.UTF_32)
            {
                salt = Encoding.UTF32.GetBytes(saltString);
            }
            else if (encodingType == EncodingType.UTF_7)
            {
                salt = Encoding.UTF7.GetBytes(saltString);
            }
            else if (encodingType == EncodingType.UTF_8)
            {
                salt = Encoding.UTF8.GetBytes(saltString);
            }
            return salt;
        }

        static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                var msg = "Stream did not contain properly formatted byte array";
                var ex = new SystemException(msg);
                ULog.PrintException(ex);
                ULog.PrintError(msg);
                throw ex;
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                var msg = "Did not read byte array properly";
                var ex = new SystemException(msg);
                ULog.PrintException(ex);
                ULog.PrintError(msg);
                throw ex;
            }
            return buffer;
        }
        
        /// <summary>  
        /// Encrypt the given string using AES.  The string can be decrypted using  
        /// DecryptStringAES().  The sharedSecret parameters must match.  
        /// </summary>  
        /// <param name="plainText">The text to encrypt.</param>  
        /// <param name="sharedSecret">A password used to generate a key for encryption.</param>  
        internal static string EncryptStringAES(this string plainText, EncodingType encodingType, string sharedSecret, string salt)
        {
            var _salt = GetSalt(salt, encodingType);
            if (string.IsNullOrEmpty(plainText))
            {
                var ex = new ArgumentNullException("plainText");
                ULog.PrintException(ex);
                ULog.PrintError("Invalid input data to crypt");
                throw ex;
            }

            if (string.IsNullOrEmpty(sharedSecret))
            {
                var ex = new ArgumentNullException("sharedSecret");
                ULog.PrintException(ex);
                ULog.PrintError("Invalid shared secret. Did you properly use the UIF setting editor? ");
                throw ex;
            }

            string outStr = null;                 // Encrypted string to return  
            RijndaelManaged aesAlg = null;        // RijndaelManaged object used to encrypt the data.  
            try
            {
                // generate the key from the shared secret and the salt  
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create a RijndaelManaged object  
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                // Create a decryptor to perform the stream transform.  
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.  
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // prepend the IV  
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt =
                       new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.  
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            catch (Exception ex)
            {
                var msg = "could not encrypt per data save. Exception msg: " + ex.Message;
                ULog.PrintException(ex);
                ULog.PrintError(msg);
                throw ex;
            }
            finally
            {
                // Clear the RijndaelManaged object.  
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            // Return the encrypted bytes from the memory stream.  
            return outStr;   
        }

        /// <summary>  
        /// Decrypt the given string.  Assumes the string was encrypted using  
        /// EncryptStringAES(), using an identical sharedSecret.  
        /// </summary>  
        /// <param name="cipherText">The text to decrypt.</param>  
        /// <param name="sharedSecret">A password used to generate a key for decryption.</param>  
        internal static string DecryptStringAES(this string cipherText, EncodingType encodingType, string sharedSecret, string salt)
        {
            var _salt = GetSalt(salt, encodingType);
            if (string.IsNullOrEmpty(cipherText))
            {
                var msg = "Invalid encrypted data to decrypt.";
                var ex = new ArgumentNullException("cipherText");
                ULog.PrintException(ex);
                ULog.PrintError(msg);
                throw ex;
            }

            if (string.IsNullOrEmpty(sharedSecret))
            {
                var msg = "Invalid shared secret. Did you properly use the UIF setting editor? ";
                var ex = new ArgumentNullException("sharedSecret");
                ULog.PrintException(ex);
                ULog.PrintError(msg);
                throw ex;
            }

            // Declare the RijndaelManaged object  
            // used to decrypt the data.  
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            try
            {
                // generate the key from the shared secret and the salt  
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create the streams used for decryption.  
                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    // Create a RijndaelManaged object  
                    // with the specified key and IV.  
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    // Get the initialization vector from the encrypted stream  
                    aesAlg.IV = ReadByteArray(msDecrypt);
                    // Create a decrytor to perform the stream transform.  
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt =
                        new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream  
                            // and place them in a string.  
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = "could not decrypt per data save. Exception msg: " + ex.Message;
                ULog.PrintException(ex);
                ULog.PrintError(msg);
                throw ex;
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            return plaintext;
        }
    }
}