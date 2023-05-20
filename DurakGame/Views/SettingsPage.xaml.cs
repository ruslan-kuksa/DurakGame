using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DurakGame.Views
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            if (App.Current.MainWindow.WindowStyle == WindowStyle.None)
            {
                FullScreen.IsChecked = true;
            }
            switch (App.BackgroundImagePath.ToString())
            {
                case "pack://application:,,,/Resources/green_background.png":
                    GreenSBorder.BorderThickness = new Thickness(3);
                    break;
                case "pack://application:,,,/Resources/poker_red_background.jpg":
                    RedDBorder.BorderThickness = new Thickness(3);
                    break;
                case "pack://application:,,,/Resources/poker_black_background.jpg":
                    BlackDBorder.BorderThickness = new Thickness(3);
                    break;
                case "pack://application:,,,/Resources/poker_green_background.jpg":
                    GreenDBorder.BorderThickness = new Thickness(3);
                    break;
            }
        }

        private void FullScreen_Checked(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Maximized;
            App.Current.MainWindow.WindowStyle = WindowStyle.None;
        }

        private void FullScreen_Unchecked(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Normal;
            App.Current.MainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MenuPage());
        }

        private void RedDBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RedDBorder.BorderThickness = new Thickness(3);
            BlackDBorder.BorderThickness = new Thickness(0);
            GreenDBorder.BorderThickness = new Thickness(0);
            GreenSBorder.BorderThickness = new Thickness(0);
            App.BackgroundImagePath = "pack://application:,,,/Resources/poker_red_background.jpg";
        }

        private void BlackDBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RedDBorder.BorderThickness = new Thickness(0);
            BlackDBorder.BorderThickness = new Thickness(3);
            GreenDBorder.BorderThickness = new Thickness(0);
            GreenSBorder.BorderThickness = new Thickness(0);
            App.BackgroundImagePath = "pack://application:,,,/Resources/poker_black_background.jpg";
        }

        private void GreenDBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RedDBorder.BorderThickness = new Thickness(0);
            BlackDBorder.BorderThickness = new Thickness(0);
            GreenDBorder.BorderThickness = new Thickness(3);
            GreenSBorder.BorderThickness = new Thickness(0);
            App.BackgroundImagePath = "pack://application:,,,/Resources/poker_green_background.jpg";
        }

        private void GreenSBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RedDBorder.BorderThickness = new Thickness(0);
            BlackDBorder.BorderThickness = new Thickness(0);
            GreenDBorder.BorderThickness = new Thickness(0);
            GreenSBorder.BorderThickness = new Thickness(3);
            App.BackgroundImagePath = "pack://application:,,,/Resources/green_background.png";
        }
    }
}
