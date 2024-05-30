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

- **KISS (Keep It Simple, Stupid)**: The design and implementation of the game were kept as simple as possible, avoiding unnecessary complexity. For example, the `GameManager.cs` class handles the core game logic in a straightforward manner.

- **SOLID**:
   - **Single Responsibility Principle (SRP)**: Each class in the project has a single responsibility. For example, `GameManager.cs` handles the overall game logic, while `Card.cs` represents a single card.
   - **Open/Closed Principle (OCP)**: The game is designed to be easily extendable. For example, new strategies for bots can be added without modifying existing code.
   - **Liskov Substitution Principle (LSP)**: Objects of a superclass should be replaceable with objects of a subclass without affecting the functionality. For example, `HumanPlayer` and `BotPlayer` can be used interchangeably as `Player`.
   - **Interface Segregation Principle (ISP)**: Interfaces are designed to be client-specific rather than general-purpose. For example, `IHintHandler.cs` and `IBotStrategy.cs` are specific to their respective functionalities.
   - **Dependency Inversion Principle (DIP)**: High-level modules depend on abstractions rather than concrete classes. This is seen in the use of interfaces like `IHintHandler.cs` and `IBotStrategy.cs`.

- **YAGNI (You Aren't Gonna Need It)**: Features were not added unless they were necessary. For example, only essential game features were implemented initially, and additional features were considered based on necessity.

- **Composition Over Inheritance**: Composition was preferred over inheritance to achieve code reuse. For example, `Player` class uses composition to include strategies for playing cards rather than inheriting from multiple classes.

- **Program to Interfaces, not Implementations**: The game logic relies on interfaces rather than concrete implementations. For example, the `GameManager` class interacts with `IHintHandler` and `IBotStrategy` interfaces.

- **Fail Fast**: The game logic is designed to fail early if there are errors. For example, validation checks are performed at the beginning of methods to catch errors early, such as in `Validation` folder.
