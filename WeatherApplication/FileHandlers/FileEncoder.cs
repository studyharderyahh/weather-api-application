using System;
using System.IO; // Provides functionalities for file handling.
using System.Security.Cryptography; // Provides encryption and decryption functionalities.
using System.Text;


namespace WeatherApplication.FileHandlers
{
    public class FileEncoder
    {
        // Lazy initialization of the singleton instance
        private static Lazy<FileEncoder> lazyInstance;
        // File path to store encrypted data.
        private readonly string filePath;
        // Encryption key (base64 string).
        private readonly byte[] encryptionKey = Convert.FromBase64String("C/+YjsuTzXJzop3TX46d2WATe1qZ/PiNT/mCRxrSw1o=");
        // Initialization vector for encryption.
        private readonly byte[] iv;

        private readonly Logger logger = Logger.Instance();

        private FileEncoder(string filePath)
        {
            logger.LogInfo("File Encoding started...");

            this.filePath = filePath;
            using (Aes aes = Aes.Create())
            {
                iv = aes.IV;
            }
        }
        // Static method to initialize the singleton instance with a file path
        public static void Initialize(string path)
        {
            if (lazyInstance == null)
            {
                lazyInstance = new Lazy<FileEncoder>(() => new FileEncoder(path));
            }
            else
            {
                throw new InvalidOperationException("FileEncoder has already been initialized.");
            }
        }
        
        // Property to get the singleton instance
        public static FileEncoder Instance
        {
            get
            {
                if (lazyInstance == null)
                {
                    throw new InvalidOperationException("FileEncoder is not initialized. Call Initialize() first.");
                }
                return lazyInstance.Value;
            }
        }

        // Method to write encrypted key-value pairs to file.
        public void Write(string apiKey, string value)
        {
            try
            {
                // Encrypt the key-value pair.
                string encryptedPair = EncryptString($"{apiKey}={value}", encryptionKey, iv);

                // Write the encrypted pair to the file.
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(encryptedPair);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error writing apiKey and its value to file: {ex.Message}");
                // Handle any errors that occur during writing.
                Console.WriteLine($"Error writing apiKey and its value to file: {ex.Message}");
            }
        }

        // Method to read the decrypted value for a given key.
        public string Read(string apiKey)
        {
            try
            {
                // Read all lines from the file.
                string[] lines = File.ReadAllLines(filePath);
                // Search for the specified key.
                foreach (string line in lines)
                {
                    // Decrypt each line and split into key-value pairs.
                    string decryptedLine = DecryptString(line, encryptionKey, iv);
                    string[] parts = decryptedLine.Split('=');
                    // Return the value if the key matches.
                    if (parts.Length == 2 && parts[0] == apiKey)
                    {
                        return parts[1];
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error reading apiKey and its value from file: {ex.Message}");
                // Handle any errors that occur during reading.
                Console.WriteLine($"Error reading apiKey and its value from file: {ex.Message}");
            }
            // Return null if key is not found.
            return null;
        }

        // Private method to encrypt a string using AES.
        private string EncryptString(string input, byte[] encryptionKey, byte[] iv)
        {
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = encryptionKey;
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
                                // Write all data to the stream
                                swEncrypt.Write(input);
                            }
                        }
                        // Return the encrypted bytes as a base64 string.
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Encryption error: {ex.Message}");
                // Handle any errors that occur during encryption.
                Console.WriteLine($"Encryption error: {ex.Message}");
                throw;
            }
        }

        // Private method to decrypt a string using AES.
        private string DecryptString(string input, byte[] encryptionKey, byte[] iv)
        {
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = encryptionKey;
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
                                // Read the decrypted bytes from the decrypting stream.
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {   
                logger.LogError($"Decryption error: {ex.Message}");
                // Handle any errors that occur during decryption.
                Console.WriteLine($"Decryption error: {ex.Message}");
                throw;
            }
        }
    }
}
