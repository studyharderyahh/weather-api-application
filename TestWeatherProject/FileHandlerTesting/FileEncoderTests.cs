using global::WeatherApplication.FileHandlers;
using NUnit.Framework;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using WeatherApplication.FileHandlers;

namespace TestWeatherProject.FileHandlerTesting
{
    [TestFixture]
    public class FileEncoderTests
    {
        private const string TestFilePath = "testfile.txt";
        private const string EncryptionKey = "P@ssw0rd123"; // Example encryption key (base64 string).

        [SetUp]
        public void Setup()
        {
            // Initialize FileEncoder singleton instance before each test
            FileEncoder.Initialize(TestFilePath, EncryptionKey);
        }

        [TearDown]
        public void Teardown()
        {
            // Clean up the test file after each test
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        [Test]
        public void WriteAndRead_ValidData_EncryptsAndDecryptsSuccessfully()
        {
            // Arrange
            string apiKey = "API_KEY";
            string value = "SECRET_VALUE";

            // Act
            FileEncoder.Instance.Write(apiKey, value);
            string decryptedValue = FileEncoder.Instance.Read(apiKey);

            // Assert
            Assert.AreEqual(value, decryptedValue);
        }

        [Test]
        public void Read_NonExistentApiKey_ReturnsNull()
        {
            // Arrange
            string nonExistentApiKey = "NON_EXISTENT_KEY";

            // Act
            string decryptedValue = FileEncoder.Instance.Read(nonExistentApiKey);

            // Assert
            Assert.IsNull(decryptedValue);
        }

        [Test]
        public void Write_ExceptionThrown_LogsError()
        {
            // Arrange
            string invalidFilePath = ""; // Invalid file path to trigger exception

            // Act
            TestDelegate action = () => FileEncoder.Initialize(invalidFilePath, EncryptionKey);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }
    }
}