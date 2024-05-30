using DurakGame.Memento;
using DurakGame.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DurakGame.ViewHandler;

namespace DurakGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainGamePage : Page
    {
        public GameManager Game;
        public GameCaretaker Caretaker = new GameCaretaker();
        public UIBackgroundManager UIBackgroundManager;
        public UIDeckManager UIDeckManager;
        public UIPlayerManager UIPlayerManager;
        public UIManager UIManager;
        public UIBotManager UIBotManager;
        public UIButtonHandler UIButtonHandler;

        public MainGamePage()
        {
            InitializeComponent();
            UIBackgroundManager = new UIBackgroundManager(this);
            UIDeckManager = new UIDeckManager(this);
            UIPlayerManager = new UIPlayerManager(this);
            UIManager = new UIManager(this);
            UIBotManager = new UIBotManager(this);
            UIButtonHandler = new UIButtonHandler(this);
            InitializeGame();
        }

        public void InitializeGameManager()
        {
            Game = new GameManager();
            Game.GameChanged += OnGameStateChanged;
        }

        public void InitializeGame()
        {
            new GameInitializer(this).GameInitialize();
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            UIButtonHandler.StartGameButton_Click(sender, e);
        }

        private void TakeButton_Click(object sender, RoutedEventArgs e)
        {
            UIButtonHandler.TakeButton_Click(sender, e);
        }

        private void BeatButton_Click(object sender, RoutedEventArgs e)
        {
            UIButtonHandler.BeatButton_Click(sender, e);
        }

        private void OnGameStateChanged()
        {
            UIManager.UpdateUI();
        }

        public void CardControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UIPlayerManager.CardControl_MouseLeftButtonDown(sender, e);
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            UIButtonHandler.UndoButton_Click(sender, e);
        }

        private void HintButton_Click(object sender, RoutedEventArgs e)
        {
            UIButtonHandler.HintButton_Click(sender, e);
        }
    }
}
