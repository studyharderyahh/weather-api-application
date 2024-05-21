using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WeatherApplication
{
    public class FileEncoder
    {
        private static readonly object lockObject = new object();
        private static FileEncoder instance; // Initialize instance here
        private readonly string filePath;
        private readonly byte[] key = Convert.FromBase64String("C/+YjsuTzXJzop3TX46d2WATe1qZ/PiNT/mCRxrSw1o=");
        private readonly byte[] iv; // I'm good hiiiiii knuuugvghv vhgcfivfgffffffffffffffffffffffffffffffffffffffffffffffffff

        private FileEncoder(string filePath)
        {
            this.filePath = filePath;
            using (Aes aes = Aes.Create())
            {
                iv = aes.IV;
            }
        }

        public static FileEncoder GetInstance(string filePath)
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new FileEncoder(filePath);
                    }
                }
            }
            return instance;
        }

        public void Write(string key, string value)
        {
            // Encrypt the key-value pair
            string encryptedPair = EncryptString($"{key}={value}", this.key, iv);

            // Write to file
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(encryptedPair);
            }
        }
        public string Read(string key)
        {
            // Read lines from the file
            string[] lines = File.ReadAllLines(filePath);

            // Find the line corresponding to the key
            foreach (string line in lines)
            {
                string decryptedLine = DecryptString(line, this.key, iv);
                string[] parts = decryptedLine.Split('=');
                if (parts.Length == 2 && parts[0] == key)
                {
                    // Return the decrypted value
                    return parts[1];
                }
            }

            // Key not found
            return null;
        }
        private string EncryptString(string input, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Write all data to the stream.
                            swEncrypt.Write(input);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
        private string DecryptString(string input, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(input)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
