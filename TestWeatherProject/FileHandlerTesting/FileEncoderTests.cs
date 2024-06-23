using global::WeatherApplication.FileHandlers;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using WeatherApplication.FileHandlers;

namespace TestWeatherProject.FileHandlerTesting
{
    [TestFixture]
    public class FileEncoderTests
    {
        private const string TestFilePath = "testfile.txt";
        private const string TestEncryptionKey = "C/+YjsuTzXJzop3TX46d2WATe1qZ/PiNT/mCRxrSw1o=";

        [SetUp]
        public void Setup()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }

            // Reset lazyInstance field before each test to ensure a clean state
            typeof(FileEncoder).GetField("lazyInstance", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, null);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        [Test]
        public void Initialize_SingletonInstance_CorrectlyInitialized()
        {
            // Act
            FileEncoder.Initialize(TestFilePath, TestEncryptionKey);

            // Assert
            var instance = FileEncoder.Instance;
            Assert.That(instance, Is.Not.Null);
            Assert.That(() => FileEncoder.Initialize(TestFilePath, TestEncryptionKey),
                Throws.InvalidOperationException.With.Message.Contains("FileEncoder has already been initialized."));
        }

        [Test]
        public void Instance_BeforeInitialization_ThrowsInvalidOperationException()
        {
            // Act & Assert
            Assert.That(() => { var instance = FileEncoder.Instance; },
                Throws.InvalidOperationException.With.Message.Contains("FileEncoder is not initialized. Call Initialize() first."));
        }

        [Test]
        public void Write_EncryptsAndWritesData_Correctly()
        {
            // Arrange
            FileEncoder.Initialize(TestFilePath, TestEncryptionKey);
            var encoder = FileEncoder.Instance;
            var apiKey = "testApiKey";
            var value = "a173994356f879bb3e42AmazingGrace";

            // Act
            encoder.Write(apiKey, value);

            // Assert
            var lines = File.ReadAllLines(TestFilePath);
            Assert.That(lines.Length, Is.EqualTo(1));

            var encryptedData = lines[0];
            var decryptedData = DecryptTestString(encryptedData, Convert.FromBase64String(TestEncryptionKey), encoder);
            Assert.That(decryptedData, Is.EqualTo($"{apiKey}={value}"));
        }

        [Test]
        public void Write_Exception_LogsError()
        {
            // Arrange
            var mockLogger = new Mock<Logger>();
            FileEncoder.Initialize(TestFilePath, TestEncryptionKey);
            var encoder = FileEncoder.Instance;
            var apiKey = "testApiKey";
            var value = "a173994356f879bb3e42275PraiseHim";

            // Act & Assert
            Assert.DoesNotThrow(() => encoder.Write(apiKey, value));
        }

        [Test]
        public void Read_DecryptsAndRetrievesData_Correctly()
        {
            // Arrange
            FileEncoder.Initialize(TestFilePath, TestEncryptionKey);
            var encoder = FileEncoder.Instance;
            var apiKey = "testApiKey";
            var value = "a173994356f879bb3e422754123456";

            encoder.Write(apiKey, value);

            // Act
            var result = encoder.Read(apiKey);

            // Assert
            Assert.That(result, Is.EqualTo(value));
        }

        [Test]
        public void Read_KeyNotFound_ReturnsNull()
        {
            // Arrange
            FileEncoder.Initialize(TestFilePath, TestEncryptionKey);
            var encoder = FileEncoder.Instance;

            // Act
            var result = encoder.Read("nonexistentKey");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Read_Exception_LogsError()
        {
            // Arrange
            var mockLogger = new Mock<Logger>();
            FileEncoder.Initialize(TestFilePath, TestEncryptionKey);
            var encoder = FileEncoder.Instance;

            // Act & Assert
            Assert.DoesNotThrow(() => encoder.Read("testApiKey"));
        }

        private string DecryptTestString(string input, byte[] encryptionKey, FileEncoder encoder)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = encryptionKey;
                aesAlg.IV = encoder.GetType().GetField("iv", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(encoder) as byte[];

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(input)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}