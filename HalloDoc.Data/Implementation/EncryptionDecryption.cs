using System.Security.Cryptography;
using System.Text;

namespace Services.Implementation
{
    public static class EncryptionDecryption
    {


        public const string secretKey = "ORM@714rGK#zO>6H61Sdt<ST[yx\\38!@GW£|Vx>lg";

        public static string EncryptStringToBase64_Aes(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return string.Empty;
            }

            try
            {
                // Get the bytes of the string
                byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(secretKey);

                // Hash the password with SHA256
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);

                return GetHexString(bytesEncrypted);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string DecryptStringFromBase64_Aes(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                return string.Empty;
            }

            try
            {
                // Get the bytes of the string
                byte[] bytesToBeDecrypted = GetBytes(cipherText);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(secretKey);

                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);

                if (bytesDecrypted == null)
                {
                    return string.Empty;
                }

                return Encoding.UTF8.GetString(bytesDecrypted);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[]? encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (MemoryStream ms = new())
            {
                using (RijndaelManaged aes = new())
                {
                    Rfc2898DeriveBytes key = new(passwordBytes, saltBytes, 1000);
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);
                    aes.Mode = CipherMode.CBC;

                    using (CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            try
            {
                byte[]? decryptedBytes = null;

                // Set your salt here, change it to meet your flavor:
                // The salt bytes must be at least 8 bytes.
                byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
                using (MemoryStream ms = new())
                {
                    using (RijndaelManaged aes = new())
                    {
                        Rfc2898DeriveBytes key = new(passwordBytes, saltBytes, 1000);
                        aes.KeySize = 256;
                        aes.BlockSize = 128;
                        aes.Key = key.GetBytes(aes.KeySize / 8);
                        aes.IV = key.GetBytes(aes.BlockSize / 8);
                        aes.Mode = CipherMode.CBC;

                        using (CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                            cs.Close();
                        }

                        decryptedBytes = ms.ToArray();
                    }
                }

                return decryptedBytes;
            }
            catch (Exception ex)
            {
                return null!;
            }
        }

        private static string GetHexString(byte[] bytes)
        {
            StringBuilder sb = new();
            foreach (byte b in bytes)
            {
                string hex = b.ToString("x2");
                sb.Append(hex);
            }

            return sb.ToString();
        }

        private static byte[] GetBytes(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }
    }

}


//private static readonly char[] Digits = "0123456789".ToCharArray();
//private static readonly char[] Alphabets = "ABCDEFGHIJ".ToCharArray();

//public static string EncryptStringToBase64_Aes(string plainText)
//{
//    return new string(plainText.Select(ch => EncryptCharacter(ch)).ToArray());
//}

//public static string DecryptStringFromBase64_Aes(string cipherText)
//{
//    return new string(cipherText.Select(ch => DecryptCharacter(ch)).ToArray());
//}

//private static char EncryptCharacter(char ch)
//{
//    int index = Array.IndexOf(Digits, ch);
//    return index >= 0 ? Alphabets[index] : ch;
//}

//private static char DecryptCharacter(char ch)
//{
//    int index = Array.IndexOf(Alphabets, ch);
//    return index >= 0 ? Digits[index] : ch;
//}
//public static string EncryptStringToBase64_Aes(string plainText)
//{
//    byte[] encrypted;

//    using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
//    {
//        aes.Key = Key;
//        aes.IV = IV;

//        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

//        using (MemoryStream ms = new MemoryStream())
//        {
//            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
//            {
//                using (StreamWriter sw = new StreamWriter(cs))
//                {
//                    sw.Write(plainText);
//                }
//                encrypted = ms.ToArray();
//            }
//        }
//    }
//    return Convert.ToBase64String(encrypted).Replace('+', '-').Replace('/', '_').TrimEnd('=');

//    //return Convert.ToBase64String(encrypted);
//}

//public static string DecryptStringFromBase64_Aes(string cipherText)
//{
//    cipherText = cipherText.Replace('-', '+').Replace('_', '/') + new string('=', (4 - cipherText.Length % 4) % 4);

//    string plaintext = null;

//    using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
//    {
//        aes.Key = Key;
//        aes.IV = IV;

//        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

//        using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
//        {
//            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
//            {
//                using (StreamReader sr = new StreamReader(cs))
//                {
//                    plaintext = sr.ReadToEnd();
//                }
//            }
//        }
//    }

//    return plaintext;
//}








//public static string DecryptStringFromBase64_Aes(string cipherTextBase64, byte[] key, byte[] iv)
//{
//    byte[] cipherText = Convert.FromBase64String(cipherTextBase64);

//    // Declare the string used to hold
//    // the decrypted text.
//    string plaintext = null;

//    // Create an Aes object
//    // with the specified key and IV.
//    using (Aes aesAlg = Aes.Create())
//    {
//        aesAlg.Key = key;
//        aesAlg.IV = iv;

//        // Create a decryptor to perform the stream transform.
//        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

//        // Create the streams used for decryption.
//        using (MemoryStream msDecrypt = new MemoryStream(cipherText))
//        {
//            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
//            {
//                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
//                {
//                    // Read the decrypted bytes from the decrypting stream
//                    // and place them in a string.
//                    plaintext = srDecrypt.ReadToEnd();
//                }
//            }
//        }
//    }

//    return plaintext;
//}

//public static string EncryptStringToBase64_Aes(string plainText, byte[] key, byte[] iv)
//{
//    byte[] encrypted;

//    // Create an Aes object
//    // with the specified key and IV.
//    using (Aes aesAlg = Aes.Create())
//    {
//        aesAlg.Key = key;
//        aesAlg.IV = iv;

//        // Create an encryptor to perform the stream transform.
//        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

//        // Create the streams used for encryption.
//        using (MemoryStream msEncrypt = new MemoryStream())
//        {
//            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
//            {
//                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
//                {
//                    //Write all data to the stream.
//                    swEncrypt.Write(plainText);
//                }
//                encrypted = msEncrypt.ToArray();
//            }
//        }
//    }

//    // Return the encrypted bytes as Base64 string.
//    return Convert.ToBase64String(encrypted);
//}

