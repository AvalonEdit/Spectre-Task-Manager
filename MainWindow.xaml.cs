using System;
using System.Diagnostics;
using System.Management;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Spectre_Task_Manger
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PopulateProcesses();
            Processes.Visibility = Visibility.Collapsed;
            Performence.Visibility = Visibility.Visible;

            cpuCounter = new PerformanceCounter("Processer", "% Processer Time", "_Total");
            memoryCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        private PerformanceCounter cpuCounter;
        private PerformanceCounter memoryCounter;

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) { DragMove(); }

        private void Close_Click(object sender, RoutedEventArgs e) { this.Close(); }

        private void Maximize_Click(object sender, RoutedEventArgs e) { if (WindowState != WindowState.Maximized) { WindowState = WindowState.Maximized; } else { WindowState = WindowState.Normal; } }

        private void Minimize_Click(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized; }

        private void PopulateProcesses()
        {
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes) 
            { 
                ProcessBox.Items.Add(process.ProcessName);
            }
        }

        private void KillProcess_Click(object sender, RoutedEventArgs e)
        {
            string selectedProcessName = ProcessBox.SelectedItem as string;
            if (! string.IsNullOrEmpty(selectedProcessName)) 
            {
                Process[] processes = Process.GetProcessesByName(selectedProcessName);
                foreach (Process process in processes)
                {
                    process.Kill();
                }
                MessageBox.Show($"{selectedProcessName} killed successfully");

                ProcessBox.Items.Remove(selectedProcessName);
            }
            else
            {
                MessageBox.Show("Please select a process to kill");
            }
        }

        private void StartNewProcess_Click(object sender, RoutedEventArgs e)
        {
            string executablePath = "C:";

            try
            {
                Process.Start(executablePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting process: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
