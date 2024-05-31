using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ConfigLibrary
{
    public static class EncryptionUtility
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("0123456789ABCDEF0123456789ABCDEF");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("ABCDEF1234567890");

        public static string Encrypt(string plainText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(cs))
                    {
                        writer.Write(plainText);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string encryptedText)
        {
            try
            {
                using (var aes = Aes.Create())
                {
                    aes.Key = Key;
                    aes.IV = IV;

                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    using (var ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var reader = new StreamReader(cs))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
                // If decryption fails, return the original text
                return encryptedText;
            }
        }
    }
}






private void LoadConfiguration(string filePath)
{
    ConfigElements.Clear();
    var map = new ExeConfigurationFileMap { ExeConfigFilename = filePath };
    var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
    var configSection = (ConnectionConfigSection)config.GetSection("connectionConfig");

    if (configSection != null)
    {
        foreach (ConnectionConfigElement encryptedElement in configSection.Connections)
        {
            var element = new ConnectionConfigElement
            {
                EnvName = EncryptionUtility.Decrypt(encryptedElement.EnvName),
                ConnectionString = EncryptionUtility.Decrypt(encryptedElement.ConnectionString),
                EchoRoutingKey = EncryptionUtility.Decrypt(encryptedElement.EchoRoutingKey),
                EchoServerUrl = EncryptionUtility.Decrypt(encryptedElement.EchoServerUrl)
            };

            ConfigElements.Add(element);
        }
    }
    else
    {
        MessageBox.Show("Configuration section not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}








private void SaveButton_Click(object sender, RoutedEventArgs e)
{
    try
    {
        var map = new ExeConfigurationFileMap { ExeConfigFilename = ConfigFilePathTextBox.Text };
        var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        var configSection = (ConnectionConfigSection)config.GetSection("connectionConfig");

        if (configSection != null)
        {
            configSection.Connections.Clear();
            foreach (var element in ConfigElements)
            {
                var encryptedElement = new ConnectionConfigElement
                {
                    EnvName = EncryptionUtility.Encrypt(element.EnvName),
                    ConnectionString = EncryptionUtility.Encrypt(element.ConnectionString),
                    EchoRoutingKey = EncryptionUtility.Encrypt(element.EchoRoutingKey),
                    EchoServerUrl = EncryptionUtility.Encrypt(element.EchoServerUrl)
                };

                configSection.Connections.Add(encryptedElement);
            }

            config.Save(ConfigurationSaveMode.Modified);
            MessageBox.Show("Configuration saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show("Configuration section not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error saving configuration: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
