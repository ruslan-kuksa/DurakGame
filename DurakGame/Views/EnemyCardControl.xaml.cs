using DurakGame.Models;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DurakGame.Views
{
    /// <summary>
    /// Interaction logic for EnemyCardControl.xaml
    /// </summary>
    public partial class EnemyCardControl : UserControl
    {
        public static readonly DependencyProperty CardProperty = DependencyProperty.Register(
        "Card", typeof(Card), typeof(EnemyCardControl), new PropertyMetadata(null));


        public Card Card
        {
            get { return (Card)GetValue(CardProperty); }
            set { SetValue(CardProperty, value); }
        }
        public EnemyCardControl()
        {
            InitializeComponent();
        }
        /*private void EnemyCardControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            *//*ToggleCardVisibility();*//*
        }*/

        /*private void ToggleCardVisibility()
        {
            if (CardImage.Source == CardBackImage.Source)
            {
                string imagePath = $"/Resources/{Card.Rank.ToString().ToLowerInvariant()}_of_{Card.Suit.ToString().ToLowerInvariant()}.png";
                BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                CardImage.Source = bitmapImage;
            }
            else
            {
                CardImage.Source = CardBackImage.Source;
            }
        }*/
    }
}
