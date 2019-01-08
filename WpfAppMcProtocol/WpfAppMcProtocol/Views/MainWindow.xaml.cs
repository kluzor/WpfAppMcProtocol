using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using Mitsubishi;


namespace WpfAppMcProtocol
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Plc FApp;

        public MainWindow()
        {
            FApp = new McProtocolTcp("10.1.13.90", 0x7D0); // 0x7D0 = Port: 2000 HEX
            //FApp.Open();
            //ConnectToPlc();
            InitializeComponent();
        }

        private void UstawieniaButton_Click(object sender, RoutedEventArgs e)
        {
            //ints[0] = 5;
        }
        private void Error_Click(object sender, RoutedEventArgs e)
        {
            //ReadData();
        }
        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            //WriteData();
        }
    }
}