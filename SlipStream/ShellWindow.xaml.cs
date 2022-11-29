using OfficeOpenXml;
using SlipStream.Core;
using SlipStream.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SlipStream
{
    /// <summary>
    /// Interaction logic for ShellWindow.xaml
    /// </summary>
    public partial class ShellWindow : Window
    {
        /// <summary>
        /// Main entry point
        /// </summary>
        // https://stackoverflow.com/questions/1025843/merging-dlls-into-a-single-exe-with-wpf?fbclid=IwAR2vdTCV2W3k9I-p4kJkqBMMbhfw4vfKIoTFEJUBcXzBgcbkRUWOgzt2Ipw
        //[STAThreadAttribute]
        //public static void Mailn()
        //{
            //AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
            //App.Main();
        //}


        /// <summary>
        /// Combines the .dll files to become one exe
        /// </summary>
        // This is also for exe stuff
        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = new AssemblyName(args.Name);

            var path = assemblyName.Name + ".dll";

            if (assemblyName.CultureInfo == null) return null;

            if (assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture) == false) path = String.Format(@"{0}\{1}", assemblyName.CultureInfo, path);

            using (Stream stream = executingAssembly.GetManifestResourceStream(path))
            {
                if (stream == null) return null;

                var assemblyRawBytes = new byte[stream.Length];
                stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
                return Assembly.Load(assemblyRawBytes);
            }
        }

        public MainViewModel _MainViewModel { get; set; }

        public ShellWindow()
        {
            _MainViewModel = new MainViewModel();
            DataContext = _MainViewModel;
            InitializeComponent();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void FullscreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                MinMaxIcon.Text = "2";
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                MinMaxIcon.Text = "1";
                this.WindowState = WindowState.Normal;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DiscordButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://discord.gg/ZJsq4GtS",
                UseShellExecute = true
            });
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
