## Installation

To run the Durak game locally, follow these steps:

1. Clone the repository:
    ```sh
    https://github.com/ruslan-kuksa/DurakGame
    ```
2. Open Visual Studio
3. Open the solution DurakGame.sln:
4. Run the project, click F5 or green button on panel in VS:

## Usage

After starting the application, you can start a new game by clicking the "Start Game" button. 
The game will initialize with one human player and one bot player. The game determines the player(player or bot) who play cards first
You can play cards by clicking on them, and the bot will defense or take cards automatically. 
Bot select low rank card and non-trump card for his round to attack player.
Player can defense from bot cards or take the cards from the table

## Programming Principles

The following programming principles were adhered to during the development of this project:

- **DRY (Don't Repeat Yourself)**: This principle was followed by extracting common code into reusable methods and classes. For example, card-related logic is encapsulated within the `Card.cs` class to avoid duplication.

- **KISS (Keep It Simple, Stupid)**: The design and implementation of the game were kept as simple as possible, avoiding unnecessary complexity.

- **SOLID**:
   - **Single Responsibility Principle (SRP)**: Each class in the project has a single responsibility. For example,`Card.cs` represents a single card.
   - **Open/Closed Principle (OCP)**: The game is designed to be easily extendable. For example, new strategies for bots can be added without modifying existing code.
   - **Liskov Substitution Principle (LSP)**: Objects of a superclass should be replaceable with objects of a subclass without affecting the functionality. For example, `HumanPlayer` and `BotPlayer` can be used interchangeably as `Player`. Same with `HintHandler`
   - **Interface Segregation Principle (ISP)**: Interfaces are designed to be client-specific rather than general-purpose. For example, `IHintHandler.cs` and `IBotStrategy.cs` are specific to their respective functionalities.
   - **Dependency Inversion Principle (DIP)**: High-level modules depend on abstractions rather than concrete classes. This is seen in the use of interfaces like `IHintHandler.cs` and `IBotStrategy.cs`.

- **YAGNI (You Aren't Gonna Need It)**: Features were not added unless they were necessary. For example, only essential game features were implemented initially, and additional features were considered based on necessity.

- **Composition Over Inheritance**: Composition was preferred over inheritance to achieve code reuse. For example, `BotPlayer` class uses composition to include strategies for playing cards.

- **Program to Interfaces, not Implementations**: The game logic relies on interfaces rather than concrete implementations. For example, the `GameManager` class interacts with `IHintHandler` and `IBotStrategy` interfaces.

- **Fail Fast**: The game logic is designed to fail early if there are errors. For example, validation checks are performed at the beginning of methods to catch errors early, such as in `Validation` folder.
## Design Patterns

### Strategy Pattern

The Strategy Pattern is used to define different algorithms for attacking and defending. This allows for flexibility and extensibility in how players (both human and bot) play their turns.

#### Code Example
- `IHumanStrategy.cs`
  ```csharp
    public interface IHumanStrategy
    {
        bool PlayCardStrategy(Player player, Card card, Table table, Card trumpCard, out string errorMessage);
    }
  ```
  
- `HumanDefenseStrategy.cs`
  ```csharp
    public class HumanDefenseStrategy : IHumanStrategy
    {
        private readonly BaseValidator _validator;

        public HumanDefenseStrategy(BaseValidator validator)
        {
            _validator = validator;
        }

        public bool PlayCardStrategy(Player player, Card card, Table table, Card trumpCard, out string errorMessage)
        {
            try
            {
                _validator.SetValidationStrategy(new DefenseCardValidation());
                _validator.Validate(player, card, table, trumpCard);
                player.RemoveCardFromHand(card);
                table.AddDefenseCard(card);
                errorMessage = string.Empty;
                return true;
            }
            catch (GameValidationException ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
  ```
- `HumanAttackStrategy.cs`
  ```csharp
    public class HumanAttackStrategy : IHumanStrategy
    {
        private readonly BaseValidator _validator;

        public HumanAttackStrategy(BaseValidator validator)
        {
            _validator = validator;
        }

        public bool PlayCardStrategy(Player player, Card card, Table table, Card trumpCard, out string errorMessage)
        {
            try
            {
                _validator.SetValidationStrategy(new AttackCardValidation());
                _validator.Validate(player, card, table, trumpCard);
                player.RemoveCardFromHand(card);
                table.AddAttackCard(card);
                errorMessage = string.Empty;
                return true;
            }
            catch (GameValidationException ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
  ```

### Memento Pattern

The Memento Pattern implements undo functionality, allowing the game to be restored to a previous state.

#### Code

- `GameCaretaker.cs`
  ```csharp
    public class GameCaretaker
    {
        private Stack<GameMemento> _mementos = new Stack<GameMemento>();

        public void Save(GameMemento memento)
        {
            _mementos.Push(memento);
        }

        public GameMemento Undo()
        {
            if (_mementos.Count > 0)
            {
                return _mementos.Pop();
            }
            return null;
        }
    }
  ```
- `GameMemento.cs`
  ```csharp
    public class GameMemento
    {
        public List<Card> PlayerHand { get; private set; }
        public List<Card> TableAttackCards { get; private set; }
        public List<Card> TableDefenseCards { get; private set; }
        public Player ActivePlayer { get; private set; }

        public GameMemento(List<Card> playerHand, List<Card> tableAttackCards, List<Card> tableDefenseCards, Player activePlayer)
        {
            PlayerHand = new List<Card>(playerHand);
            TableAttackCards = new List<Card>(tableAttackCards);
            TableDefenseCards = new List<Card>(tableDefenseCards);
            ActivePlayer = activePlayer;
        }
    }
  ```
- `SaveState.cs`
  ```csharp
        public GameMemento SaveState()
        {
            return new GameMemento(
                new List<Card>(Players[0].Hand),
                new List<Card>(Table.AttackCards),
                new List<Card>(Table.DefenseCards),
                ActivePlayer
            );
        }
  ```
- `RestoreState.cs`
  ```csharp
        public void RestoreState(GameMemento memento)
        {
            Players[0].SetHand(new List<Card>(memento.PlayerHand));
            Table.SetAttackCards(new List<Card>(memento.TableAttackCards));
            Table.SetDefenseCards(new List<Card>(memento.TableDefenseCards));
            ActivePlayer = memento.ActivePlayer;
            OnGameChanged();
        }
  ```
### Observer Pattern

The Observer Pattern notifies the UI of changes in the game state. 
When the game state changes, all subscribed observers are notified and can update accordingly.

#### Code

- `GameManager.cs`
  ```csharp
    public class GameManager
    {
        public event Action GameChanged;

        public void OnGameChanged() => GameChanged?.Invoke();
    }
  ```
- `MainGamePage.cs`
  ```csharp
    public partial class MainGamePage : Page
    {
        public void InitializeGameManager()
        {
            Game = new GameManager();
            Game.GameChanged += OnGameStateChanged;
        }
        private void OnGameStateChanged()
        {
            UIManager.UpdateUI();
        }
    }
  ```

### Chain of Responsibility Pattern

The Chain of Responsibility Pattern handles hints, where multiple handlers process a request.

#### Code

- `IHintHandler.cs`
  ```csharp
    public interface IHintHandler
    {
        IHintHandler SetNext(IHintHandler handler);

        string Handle(Player player, Table table, Card trumpCard);
    }
  ```
- `HintHandler.cs`
  ```csharp
    public abstract class HintHandler : IHintHandler
    {
        private IHintHandler? _nextHandler;

        public IHintHandler SetNext(IHintHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual string Handle(Player player, Table table, Card trumpCard)
        {
            if (_nextHandler != null)
            {
                return _nextHandler.Handle(player, table, trumpCard);
            }
            else
            {
                return GameNotification.NoAvailableHintsMessage;
            }
        }
    }
  ```
- `AttackHintHandler.cs`
  ```csharp
    public class AttackHintHandler : HintHandler
    {
        public override string Handle(Player player, Table table, Card trumpCard)
        {
            if (table.AttackCards.Count <= table.DefenseCards.Count)
            {
                List<Card> availableCards = player.Hand
                    .Where(table.CanAddAttackCard)
                    .OrderBy(card => card.Rank)
                    .ToList();

                if (availableCards.Any())
                {
                    Card bestCard = availableCards.First();
                    return string.Format(GameNotification.RecommendAttackMessage, bestCard.Rank, bestCard.Suit);
                }
            }

            return base.Handle(player, table, trumpCard);
        }
    }
  ```
- `DefenseHintHandler.cs`
  ```csharp
    public class DefenseHintHandler : HintHandler
    {
        public override string Handle(Player player, Table table, Card trumpCard)
        {
            if (table.AttackCards.Count > table.DefenseCards.Count)
            {
                Card cardToBeat = table.AttackCards.LastOrDefault();
                if (cardToBeat == null)
                    return GameNotification.NoDefenseCardsMessage;

                List<Card> availableCards = player.Hand
                    .Where(card => card.CanBeat(cardToBeat, trumpCard.Suit))
                    .OrderBy(card => card.Rank)
                    .ToList();

                if (availableCards.Any())
                {
                    Card bestCard = availableCards.First();
                    return string.Format(GameNotification.RecommendDefenseMessage, bestCard.Rank, bestCard.Suit);
                }
            }

            return base.Handle(player, table, trumpCard);
        }
    }
  ```

## Refactoring Techniques

Refactoring techniques employed:

- **Extract Method**: Complex methods are broken into smaller methods, e.g., `StartGame` in `GameManager.cs` is split into `DealInitialCards` and `SetTrumpCard`.
- **Replace Magic Number with Symbolic Constant**: Magic numbers are replaced with constants in `GameConstants.cs`.
- **Introduce Parameter Object**: Related parameters are grouped into objects, e.g., game state parameters in `GameMemento.cs`.
- **Replace Nested Conditional with Guard Clauses**: Isolate all special checks and edge cases into separate clauses, e.g.,`UIBotManager.cs`
- **Move Method/Field**: Create a new method in the class that uses the method the most, then move code from the old method to there, e.g.,`UIBotManager.cs`, `UIHandler.cs` etc.
- **Consolidate Duplicate Conditional Fragments**: Combining duplicate conditional fragments, e.g., method `SetDeckVisibility`
