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
    /// Interaction logic for CardControl.xaml
    /// </summary>
    public partial class CardControl : UserControl
    {
        public static readonly DependencyProperty CardImageProperty = DependencyProperty.Register(
         "CardImage", typeof(ImageSource), typeof(CardControl), new PropertyMetadata(null));

        public static readonly DependencyProperty CardProperty = DependencyProperty.Register(
        "Card", typeof(Card), typeof(CardControl), new PropertyMetadata(null, OnCardChanged));

        public static readonly RoutedEvent CardClickedEvent = EventManager.RegisterRoutedEvent(
        "CardClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CardControl));

        public event RoutedEventHandler CardClicked
        {
            add { AddHandler(CardClickedEvent, value); }
            remove { RemoveHandler(CardClickedEvent, value); }
        }
        public Card Card
        {
            get { return (Card)GetValue(CardProperty); }
            set { SetValue(CardProperty, value); }
        }
        public CardControl()
        {
            InitializeComponent();
            MouseEnter += CardControl_MouseEnter;
            MouseLeave += CardControl_MouseLeave;
        }
        private static void OnCardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CardControl cardControl = (CardControl)d;
            Card card = (Card)e.NewValue;
            string imagePath = $"/Resources/{card.Rank.ToString().ToLowerInvariant()}_of_{card.Suit.ToString().ToLowerInvariant()}.png";
            BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            cardControl.CardImage.Source = bitmapImage;
        }
        private void RaiseCardClickedEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(CardClickedEvent);
            RaiseEvent(args);
        }
        private void CardControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                RaiseCardClickedEvent();
            }
        }
        private void CardControl_MouseEnter(object sender, MouseEventArgs e)
        {
            CardAnimation.Y = -20;
        }
        private void CardControl_MouseLeave(object sender, MouseEventArgs e)
        {
            CardAnimation.Y = 0;
        }
    }
}
