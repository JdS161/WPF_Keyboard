using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPF_Keyboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string strSource = "QWERTYUIOPASDFGHJKLZXCVBNM~!@#$%^&*()_+{}|:\"<>?1234567890[],./\\`-=;'qwertyuiopasdfghjklzxcvbnm";
        bool flagCaseSensitive = false;
        Random rand = new Random();
        KeyConverter keyconv = new KeyConverter();
        DispatcherTimer timer = null;
        int tmpTimer = 0;
        bool flagBackspace = true;
        int fails = 0;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);

            foreach (Button item in this.keys.Children)
            {
                item.Content = item.Content.ToString().ToLower();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            tmpTimer++;
            Speed();
        }

        private void Speed()
        {
            speed.Content = Math.Round(((double)tb2.Text.Length / tmpTimer) * 60).ToString();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (Button item in this.keys.Children)
            {
                if (e.Key == Key.LeftShift || e.Key == Key.RightShift || e.Key == Key.Capital)
                {
                    if (item.Content.ToString().Length == 1)
                        item.Content = item.Content.ToString().ToUpper();
                }
                if (keyconv.ConvertToString(e.Key) == item.Content.ToString().ToUpper())
                {
                    item.Opacity = 0.5;
                }
                if (e.Key == Key.Back)
                {
                    flagBackspace = true;
                }
                else flagBackspace = false;
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (Button item in this.keys.Children)
            {
                if (e.Key == Key.LeftShift || e.Key == Key.RightShift || e.Key == Key.Capital)
                    item.Content = item.Content.ToString().ToLower();
                if (keyconv.ConvertToString(e.Key) == item.Content.ToString().ToUpper())
                {
                    item.Opacity = 100;
                }
            }
        }
        private void Start()
        {
            tb1.Clear();
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
            tb1.IsEnabled = true;
            tb2.IsEnabled = true;
            tb2.Focus();
            GenerateString(Convert.ToInt32(dif.Content));
            tmpTimer = 0;
            timer.Start();
        }
        private void Stop()
        {
            btnStop.IsEnabled = false;
            btnStart.IsEnabled = true;
            tb2.Clear();
            tb2.IsEnabled = false;
            tb1.IsEnabled = false;
        }

        private void GenerateString(int _dif)
        {
            string tmp = "";
            int tmpCaseSens = (flagCaseSensitive) ? 0 : 47;
            for (int i = 0; i < _dif * 100; i++)
            {
                tmp += strSource[rand.Next(tmpCaseSens, strSource.Length)];
            }
            tmp += " ";
            int countString = _dif * 10;
            for (int i = 0; i < countString; i++)
            {
                tb1.Text += tmp[rand.Next(0, tmp.Length)];
                if (i % _dif == 0) tb1.Text += "";
            }
        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Start();
        }
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            flagCaseSensitive = false;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            flagCaseSensitive = true;
        }

        private void tb2_TextChanged(object sender, TextChangedEventArgs e)
        {
            string tmp;
            tmp = tb1.Text.Substring(0, tb2.Text.Length);
            if (tb2.Text.Equals(tmp))
            {
                tb2.Foreground = new SolidColorBrush(Colors.DarkGreen);
            }
            else
            {
                fails++;
                tb2.Foreground = new SolidColorBrush(Colors.Red);
                cntrFail.Content = fails;                
            }
            if (tb1.Text.Length == tb2.Text.Length)
            {
                timer.Stop();
                MessageBox.Show($"Task complete!\nSymbols: {tb1.Text.Length}.\nFails: {cntrFail.Content}.\nType speed {speed.Content} symb/min", "Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                Stop();
            }
        }
    }
}
