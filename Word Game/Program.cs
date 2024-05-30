using System.Text.RegularExpressions;

// init varialbles
string? mainWord;                           // main word
int turnTime = 10;                          // turn time for each player in cesonds
int tmpTurnTime = turnTime;                 // temp value for timer
var timer = new System.Timers.Timer();      // timer
List<string> statList = new List<string>(); // list of correct player word
bool isGameFinished = false;                // flag for game ending
string[] playerNames;       // player names array

// start game
welcomeText();
prepareGameParams();
play();
// finish game

// method for printing messages into console
void printMessage(string mes) => Console.WriteLine(mes);

// method for reading messages from console
string readMessage() => Console.ReadLine();

// welcome text
void welcomeText()
{
    printMessage("Welcome to \"Word\" game!!!");
}

// method prepare some params for future game: a word, char's restrictions.
void prepareGameParams()
{
    const int minNumber = 8;    //min chars for a word
    const int maxNumber = 30;   //max chars for a word
    playerNames = ["Player 1", "Player 2"];

    // enter new word
    printMessage($"Please enter a word. You can only use letters of the Latin alphabet and Cyrillic alphabet. A word can contain from {minNumber} to {maxNumber} letters: ");
    mainWord = readMessage();

    // check a word correctness
    bool isWordValid = false;
    // regex for main word
    Regex wordRegex = new Regex("^[а-яёА-ЯЁa-zA-Z]{" + minNumber + "," + maxNumber + "}$");

    do
    {
        if (!validateMainWord(mainWord, wordRegex))
        {
            printMessage($"Use letters of the Latin alphabet and Cyrillic alphabet. A word can contain from {minNumber} to {maxNumber} letters. PLease try again."); // check if a word doesn't fit the parameters
            mainWord = readMessage();   // enter a word
        }
        else
        {
            isWordValid = true;
        }
    } while (!isWordValid);
}

// word validation through regex
bool validateMainWord(string word, Regex regex)
{
    return regex.IsMatch(word);
}

// main method for the game
// get words from players and check if everything is correct
// othewise return the number of winner and statistics
void play()
{
    printMessage($"Excellent. Let's play with a word '{mainWord}'. You'll have only {turnTime} seconds for your try.");

    int turn = 1;           // turn number
    string tmpPlayerName;   // temp player name

    while (!isGameFinished)
    {
        tmpPlayerName = (turn % 2 == 1) ? playerNames[0] : playerNames[1];

        setTimer();

        playerTurn(tmpPlayerName, turn);
        timer.Stop();
        tmpTurnTime = turnTime;

        turn++;
    }

    // finish game
    finishGame(statList);
}

void setTimer()
{
    var timer = new System.Timers.Timer(1000);
    timer.Elapsed += Timer_Elapsed;
    timer.Start();
}

void Timer_Elapsed(Object source, System.Timers.ElapsedEventArgs e)
{
    if (tmpTurnTime > 0)
    {
        printMessage(tmpTurnTime-- + " seconds remaining");
    }
    else
    {
        printMessage("Your time's up.");
        timer.Stop(); // Stop the timer
        tmpTurnTime = turnTime;
        isGameFinished = true;
    } 
}

// method show stat at the end of game
void finishGame(List<string> statList)
{
    string tmp = statList.ToArray()[statList.Count - 1].Split("|")[0];
    printMessage("Game is over. " + (tmp) + " is a winner!");
    if (statList.Count > 1)
    {
        printMessage($"Please check game statistics with a word '{mainWord}': ");
        string[] arr = statList.ToArray();
        for (int i = 0; i < arr.Length; i++)
        {
            string[] tmpArr = arr[i].Split('|');
            printMessage("Turn " + (i + 1) + ". " + tmpArr[0] + ". Word: " + tmpArr[1] + ".");
        }
    }
}

void playerTurn(string playerName, int turn)
{
    string lowerWord = mainWord.ToLower();     // main word in lower case
    string playerWord;

    printMessage($"Turn {turn}. " + playerName + ", your try: ");
    
    // player word shouldn't be empty
    playerWord = readMessage();
    while (playerWord == "")
    {
        printMessage("Empty word is not allowed. Please try again."); // check if a word doesn't fit the parameters
        playerWord = readMessage();          // enter a word
    }

    // check if a word is correct
    if (checkWord(mainWord, playerWord))
    {
        printMessage($"Word '{playerWord}' is correct. Good job!");
        statList.Add(playerName + "|" +playerWord);
        timer.Stop();
    }
    else isGameFinished = true;
}

// method for checking if a word is a part of a main word
bool checkWord(string mainWord, string word)
{
    bool result = true;
    List<char> mainWordList = mainWord.ToList();
    foreach (char ch in word)
    {
        if (mainWordList.IndexOf(Char.ToLower(ch)) < 0)
        {
            printMessage($"There isn't '{ch}' char in main word.");
            result = false;
        }
        mainWordList.Remove(ch);
    }

    return result;
}