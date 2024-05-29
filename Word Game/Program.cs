using System.Numerics;
using System.Linq;
using System.Threading;
using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;

// start game
welcomeText();
play(prepareGameParams());
// finish game

// method for printing messages into console
void printMessage(string mes) => Console.WriteLine(mes);

// welcome text
void welcomeText()
{
    printMessage("Welcome to \"Word\" game!!!");
}

// method prepare some params for future game: a word, char's restrictions.
string prepareGameParams()
{
    // consts for min and max chars for new word
    const int minNumber = 8;    //min chars for a word
    const int maxNumber = 30;   //max chars for a word

    // regex for main word
    Regex wordRegex = new Regex("^[а-яёА-ЯЁa-zA-Z]{" + minNumber + "," + maxNumber + "}$");

    string? mainWord;   // main word

    // enter new word
    printMessage($"Please enter a word. You can only use letters of the Latin alphabet and Cyrillic alphabet. A word can contain from {minNumber} to {maxNumber} letters: ");
    mainWord = Console.ReadLine();

    // check a word correctness
    bool isWordValid = false;
    do
    {
        if (!wordRegex.IsMatch(mainWord))
        {
            printMessage("Something is wrong in your word. Please try again:"); // check if a word doesn't fit the parameters
            mainWord = Console.ReadLine();          // enter a word
        }
        else
        {
            isWordValid = true;
        }
    } while (!isWordValid);

    return mainWord;
}

// main method for the game
// get words from players and check if everything is correct
// othewise return the number of winner and statistics
void play(string word)
{

    int turnTime = 10000;    // turn time for each player

    printMessage($"Excellent. Let's play with a word '{word}'. You'll have only {turnTime / 1000} seconds for your try.");

    string[] playerNames = { "Player 1", "Player 2" };  // player names array
    string? playerWord;                                 // a word from players
    bool turnResult = true;                             // turn result: true is a word is correct, false otherwise
    int turn = 1;                                       // turn number
    string lowerWord = word.ToLower();                  // main word in lower case
    List<string> wordList = new List<string>();         // list of correct player word
    string tmpPlayerName;                               // temp player name

    while (turnResult)
    {
        tmpPlayerName = (turn % 2 == 1) ? playerNames[0] : playerNames[1];
        /*
        var timer = new System.Timers.Timer(turnTime);  // Tick every second, var timer
        timer.Elapsed += Count;
        timer.Enabled = true;
        */
        playerTurn(tmpPlayerName, lowerWord, wordList, turn);
        // timer
        /*
        int num = 0;
        object gameParams = new gameParams["111", "222"];
        TimerCallback tm = new TimerCallback(Test);
        Timer timer = new Timer(tm, name, 0, turnTime);

        //Console.ReadLine();
        */
        if (wordList.Count != turn)
        {
            printMessage("Game is over. Player " + (turn % 2 + 1) + " is a winner!");
            printMessage($"Please check game statistics with a word '{word}': ");
            string[] arr = wordList.ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                tmpPlayerName = (i % 2 == 1) ? playerNames[1] : playerNames[0];
                printMessage("Turn " + (i + 1) + ". " + tmpPlayerName + ". " + arr[i]);
            }
            turnResult = false;
        }

        turn++;
    }
}

void playerTurn(string playerName, string word, List<string> wordList, int turn)
{
    printMessage($"Turn {turn}. " + playerName + ", your try: ");
    string playerWord = Console.ReadLine();
    bool turnResult = true;
    List<char> mainWordList = word.ToList();

    // check if player word is correct
    foreach (char ch in playerWord)
    {
        if (mainWordList.IndexOf(Char.ToLower(ch)) < 0)
        {
            printMessage($"There isn't '{ch}' char in main word.");
            turnResult = false;
        }
        mainWordList.Remove(ch);
    }

    if (turnResult)
    {
        printMessage($"Word '{playerWord}' is correct. Good job!");
        wordList.Add(playerWord);
    }
}

void Test(object obj)
{
    printMessage("Your time's up." + obj);
}

/*
5. Добавить таймер 10 секунд.
*/
