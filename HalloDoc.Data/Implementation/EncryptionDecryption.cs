namespace Services.Implementation
{
    public static class EncryptionDecryption
    {
        private static readonly byte[] Key = { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
        private static readonly byte[] IV = { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

        private static readonly char[] Digits = "0123456789".ToCharArray();
        private static readonly char[] Alphabets = "ABCDEFGHIJ".ToCharArray();

        public static string EncryptStringToBase64_Aes(string plainText)
        {
            return new string(plainText.Select(ch => EncryptCharacter(ch)).ToArray());
        }

        public static string DecryptStringFromBase64_Aes(string cipherText)
        {
            return new string(cipherText.Select(ch => DecryptCharacter(ch)).ToArray());
        }

        private static char EncryptCharacter(char ch)
        {
            int index = Array.IndexOf(Digits, ch);
            return index >= 0 ? Alphabets[index] : ch;
        }

        private static char DecryptCharacter(char ch)
        {
            int index = Array.IndexOf(Alphabets, ch);
            return index >= 0 ? Digits[index] : ch;
        }
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
    }
}
