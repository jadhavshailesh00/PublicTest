using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;


namespace ConfigEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ConnectionConfigElement> ConfigElements { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ConfigElements = new ObservableCollection<ConnectionConfigElement>();
            ConfigDataGrid.ItemsSource = ConfigElements;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Config files (*.config)|*.config|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                ConfigFilePathTextBox.Text = openFileDialog.FileName;
                LoadConfiguration(openFileDialog.FileName);
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
                foreach (ConnectionConfigElement connection in configSection.Connections)
                {
                    ConfigElements.Add(connection);
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
                        configSection.Connections.Add(element);
                    }

                    // Encrypt the connectionConfig section
                    configSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                   // configSection.SectionInformation.
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
    }

    public class ConnectionConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ConnectionConfigElementCollection Connections
        {
            get { return (ConnectionConfigElementCollection)this[""]; }
        }
    }

    [ConfigurationCollection(typeof(ConnectionConfigElement), AddItemName = "add")]
    public class ConnectionConfigElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ConnectionConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ConnectionConfigElement)element).EnvName;
        }

        public void Add(ConnectionConfigElement element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }
    }

    public class ConnectionConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("env_name", IsRequired = true)]
        public string EnvName
        {
            get { return (string)this["env_name"]; }
            set { this["env_name"] = value; }
        }

        [ConfigurationProperty("connectionString", IsRequired = true)]
        public string ConnectionString
        {
            get { return (string)this["connectionString"]; }
            set { this["connectionString"] = value; }
        }

        [ConfigurationProperty("echoRoutingKey", IsRequired = true)]
        public string EchoRoutingKey
        {
            get { return (string)this["echoRoutingKey"]; }
            set { this["echoRoutingKey"] = value; }
        }

        [ConfigurationProperty("echoServerUrl", IsRequired = true)]
        public string EchoServerUrl
        {
            get { return (string)this["echoServerUrl"]; }
            set { this["echoServerUrl"] = value; }
        }
    }
}


<Window x:Class="ConfigEditor.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Configuration Editor" Height="450" Width="800">
    <Window.Resources>
        <!-- Style for DataGridTextColumn elements -->
        <Style x:Key="DataGridTextColumnElementStyle" TargetType="{x:Type TextBlock}">
            <Setter Property="HorizontalAlignment" Value="Center"/>
            <Setter Property="VerticalAlignment" Value="Center"/>
            <Setter Property="Margin" Value="5"/>
            <Setter Property="TextWrapping" Value="Wrap"/>
        </Style>

        <!-- Style for DataGridTextColumn editing elements -->
        <Style x:Key="DataGridTextColumnEditingElementStyle" TargetType="{x:Type TextBox}">
            <Setter Property="HorizontalAlignment" Value="Stretch"/>
            <Setter Property="VerticalAlignment" Value="Center"/>
            <Setter Property="Margin" Value="5"/>
            <Setter Property="Padding" Value="5"/>
        </Style>
    </Window.Resources>
    <Grid Background="#F0F0F0">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="Auto"/>
            <ColumnDefinition Width="*"/>
            <ColumnDefinition Width="Auto"/>
        </Grid.ColumnDefinitions>

        <!-- Config File Path Section -->
        <StackPanel Grid.Row="0" Grid.Column="0" Grid.ColumnSpan="3" Orientation="Horizontal" Margin="10">
            <TextBlock Text="Config File Path:" VerticalAlignment="Center" Margin="0,0,10,0" Foreground="#333" FontWeight="Bold"/>
            <TextBox Name="ConfigFilePathTextBox" VerticalAlignment="Center" MinWidth="300"
                     BorderThickness="1" BorderBrush="#AAA" Background="WhiteSmoke" Padding="5"/>
            <Button Content="Browse" Click="BrowseButton_Click" Margin="10,0" Padding="10"
                    Background="#007ACC" Foreground="White" BorderThickness="1" BorderBrush="#007ACC"/>
        </StackPanel>

        <!-- Configuration Settings Grid -->
        <DataGrid Grid.Row="1" Grid.Column="0" Grid.ColumnSpan="3" Name="ConfigDataGrid" AutoGenerateColumns="False" Margin="10"
                  BorderThickness="1" BorderBrush="#AAA" Background="White" SelectionMode="Extended" 
                  CanUserAddRows="True" CanUserDeleteRows="True" RowHeaderWidth="0" HorizontalScrollBarVisibility="Auto">
            <DataGrid.Columns>
                <DataGridTextColumn Header="Environment Name" Binding="{Binding EnvName}" Width="*" 
                                    ElementStyle="{StaticResource DataGridTextColumnElementStyle}" 
                                    EditingElementStyle="{StaticResource DataGridTextColumnEditingElementStyle}"/>
                <DataGridTextColumn Header="Connection String" Binding="{Binding ConnectionString}" Width="*" 
                                    ElementStyle="{StaticResource DataGridTextColumnElementStyle}" 
                                    EditingElementStyle="{StaticResource DataGridTextColumnEditingElementStyle}"/>
                <DataGridTextColumn Header="Echo Routing Key" Binding="{Binding EchoRoutingKey}" Width="*" 
                                    ElementStyle="{StaticResource DataGridTextColumnElementStyle}" 
                                    EditingElementStyle="{StaticResource DataGridTextColumnEditingElementStyle}"/>
                <DataGridTextColumn Header="Echo Server URL" Binding="{Binding EchoServerUrl}" Width="*" 
                                    ElementStyle="{StaticResource DataGridTextColumnElementStyle}" 
                                    EditingElementStyle="{StaticResource DataGridTextColumnEditingElementStyle}"/>
            </DataGrid.Columns>
        </DataGrid>

        <!-- Save Button Section -->
        <Button Grid.Row="2" Grid.Column="2" Content="Save" Click="SaveButton_Click" HorizontalAlignment="Right" Margin="10"
                Background="#007ACC" Foreground="White" Padding="10" BorderThickness="1" BorderBrush="#007ACC"/>
    </Grid>
</Window>




<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<configSections>
		<section name="connectionConfig" type="ConfigEditor.ConnectionConfigSection, ConfigEditor" />
	</configSections>

	<startup>
		<supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
	</startup>

	<connectionConfig configProtectionProvider="DataProtectionConfigurationProvider">
  <EncryptedData>
   <CipherData>
    <CipherValue>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAALuQOXe7cy0eZJKTivAkUyQQAAAACAAAAAAAQZgAAAAEAACAAAAD4kdssujkQIgIncsWNq7UJ0q5doTu931DXIjCameITxwAAAAAOgAAAAAIAACAAAACetoGNof66VVcLVxk/FDLGR35GMisUBPJ6A8JVpjqzRJAEAAA3jMH18Cn04MfZ8GVi9MdV+1LfJZhq9pmixAlXz4Kk5qv2gbvsM8pBgXoCVDFW2O3WG6q5tESd4c3EpGT067rW4rEblBbyH7etseXWML7BnV9mPyycCmpMO5wdUQYdczn+8GsO+XQrbnRIj0eiIXD00s5muOvJwaxyRhNuXekX/kKkM4XzEm17qOKH73Qa89+EIxQTP70epm0Me1JcnPZPexkF8IvGitUYvbiBSyeep94QnvXctOrKjgBDDr4cvcmP6nDkmwfTKAuCDxHXrYxxHD8NnKDuM8u71Nc1cLW6udv0BwDZ/2izzdZgZGXp7zzaYW5WEUk/jGmAu4rZMoJygzOnUo8u+gu6B1wIJx0vnz6PPIgJTERBgf8aE9Me5IjNBMP68xdz4usbYDS8rszuP12xq7n2YhOfkxU1C+mQGGfBZuPFSe/+PcQbcCAw5AvNN2p1cafq0/y3yecgjx1slcIzfwFJ6jT2GLvxfGk2P/RyOv0JslgTUQJI/4eaEwGSrDWS1B7VjuGj2TMCdiefuTx4lGxysOt68Xy3dU2H8YgQ7MQz4qySGLs8YPGutavkJ8pFL9XG5Kc8sg/++08pkk94hCBc2lg5dnZivZ0Wf4CTx92a8VJG0k6BLVDjkckazq5SQgDtLwXEDLxzWjztqQOHVGMyWyzUMs2Zk4iv1rHvia3xXbx1dAbqG9JKRVEaGXVGdTlTNKRmgkW8ahy4j8aOIL0lgi9Gq81MHiZeEXNbnkBFhv0wiPzYs5z+c2A2ptl9N5kh2jJxcay3+UxVfrz+6TLDLww6maSsphnqgu6ZE0taSS81tu7gDhlfb9KZ8vK3NxU1hrbZBE1BZYGB0e+QmwI9kZAzVxoVkaDK1JPv4cqhNawnGZidYBpgNFdxNVwIMthK/l8I85+05iVQ9uAb9Aao3ET4ZpjoY00/ThSllUsypPvNOXagu1YmelkUq99dclXdqog5Brzqw3oo+CeZTBLjiWijSeJE6vzyCqrAdh0k0svLmQAmWU+ZcJei22HEQ8+ZzPhocd686k8v9aqqJPnCTqOIp/B523tKcS76NY0WwXU3nCdWpekANKPGoEpdVwQO91+hXeouuJQi+NZ9GiugzHOCbXlo/yfIA3MIEBFzoX5HvRbcBIEqqQzqfV64L8/dv5Nr3Sz83mRrqrH1MOjyJAKeK19Bn3I2f6wnk7kb2PYOqTPPbBZdSRBSd1srJZOB/kwvsIx+V9ZNsLRmSgujUEahPXwEl7F6VXSZVTNFT2GoKWeqUtG5oDnodn2cf+K8bul4wuREPxrxsyN4ie+2GnSbFrMarioB4itcPjF4m5LKxucQ5qLO4OdND1LigX8iPcm3lv1xB/+ppCtZ0bMF3Z064s0QXo+PpELB2R5WHEMSHYthVJwebWl6YKbiRtFN98zJLQo5WTLBJ86dZHvORneXP1Eq7axgc2eAIkVGhSNTQRLDnit73fCjMR4rFFKeKGt7WNAyom7QmZvfBZhDyXON0pn4OKrpOHQAUeswz7vEPGw0qiVLZk5mRw/6ZTnYKCcHCDHg/YCpQAAAAFIBGAxG8opxI3qcEoWCV3/S/YluZhKnhorI2NFq90+9t/wW73yM178M2QHbgEZHZwDxVsqP1uTBOT8PHW/PZPE=</CipherValue>
   </CipherData>
  </EncryptedData>
 </connectionConfig>
</configuration>





