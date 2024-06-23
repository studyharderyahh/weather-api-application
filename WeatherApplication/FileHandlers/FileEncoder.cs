using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WeatherApplication.FileHandlers
{
    /// <summary>
    /// Provides functionalities for encoding and decoding files with encryption.
    /// </summary>
    public class FileEncoder
    {
        // Lazy initialization of the singleton instance.
        private static Lazy<FileEncoder> lazyInstance;

        // File path to store encrypted data.
        private readonly string filePath;

        // Encryption key (base64 string).
        private readonly byte[] encryptionKey;

        // Initialization vector for encryption.
        private readonly byte[] iv;

        // Logger instance for logging operations.
        private readonly Logger logger = Logger.Instance();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileEncoder"/> class.
        /// </summary>
        /// <param name="filePath">The file path where encrypted data will be stored.</param>
        /// <param name="encryptionKey">The encryption key as a base64 string.</param>
        private FileEncoder(string filePath, string encryptionKey)
        {
            logger.LogInfo("File Encoding started...");

            this.filePath = filePath;
            this.encryptionKey = Convert.FromBase64String(encryptionKey);

            using (Aes aes = Aes.Create())
            {
                iv = aes.IV;
            }
        }

        /// <summary>
        /// Initializes the singleton instance with the specified file path and encryption key.
        /// </summary>
        /// <param name="path">The file path where encrypted data will be stored.</param>
        /// <param name="encryptionKey">The encryption key as a base64 string.</param>
        public static void Initialize(string path, string encryptionKey)
        {
            if (lazyInstance == null)
            {
                lazyInstance = new Lazy<FileEncoder>(() => new FileEncoder(path, encryptionKey));
            }
            else
            {
                throw new InvalidOperationException("FileEncoder has already been initialized.");
            }
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="FileEncoder"/> class.
        /// </summary>
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

        /// <summary>
        /// Writes an encrypted key-value pair to the file.
        /// </summary>
        /// <param name="apiKey">The key to be encrypted.</param>
        /// <param name="value">The value to be encrypted.</param>
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
                Console.WriteLine($"Error writing apiKey and its value to file: {ex.Message}");
                throw; // Rethrow the exception to propagate it up the call stack.
            }
        }

        /// <summary>
        /// Reads the decrypted value for a given key from the file.
        /// </summary>
        /// <param name="apiKey">The key whose value needs to be decrypted.</param>
        /// <returns>The decrypted value associated with the key.</returns>
        public string Read(string apiKey)
        {
            try
            {
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
                Console.WriteLine($"Error reading apiKey and its value from file: {ex.Message}");
                throw; // Rethrow the exception to propagate it up the call stack.
            }
            // Return null if key is not found.
            return null;
        }

        /// <summary>
        /// Encrypts a string using AES encryption.
        /// </summary>
        /// <param name="input">The input string to encrypt.</param>
        /// <param name="encryptionKey">The encryption key as a byte array.</param>
        /// <param name="iv">The initialization vector as a byte array.</param>
        /// <returns>The encrypted string in base64 format.</returns>
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
                Console.WriteLine($"Encryption error: {ex.Message}");
                throw; // Rethrow the exception to propagate it up the call stack.
            }
        }

        /// <summary>
        /// Decrypts a string using AES encryption.
        /// </summary>
        /// <param name="input">The input string to decrypt.</param>
        /// <param name="encryptionKey">The encryption key as a byte array.</param>
        /// <param name="iv">The initialization vector as a byte array.</param>
        /// <returns>The decrypted string.</returns>
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
                Console.WriteLine($"Decryption error: {ex.Message}");
                throw; // Rethrow the exception to propagate it up the call stack.
            }
        }
    }
}
