using System.Text.RegularExpressions;

DateTime localDate = DateTime.Now;
// init varialbles
string? mainWord;                                   // main word
const int turnTime = 10;                            // turn time for each player in cesonds
int tmpTurnTime = turnTime;                         // temp value for timer
var timer = new System.Timers.Timer();              // timer
List<string> statList = new List<string>();         // list of correct player word
bool isGameFinished = false;                        // flag for game ending
string[] playerNames;                               // player names array
string[] languages = ["English", "Русский"];        // languages array
int choosedLanguage;                                // user language choice
const int minNumber = 8;                            // min chars for a word
const int maxNumber = 30;                           // max chars for a word
string[] arrRegex = ["^[a-zA-Z]", "^[а-яёА-ЯЁ]"];   // regex array
                                                    // different text depends on language
string[] arrWordText =
    ["Please enter a word. You can only use letters of the Latin alphabet. The size of the word: ",
    "Пожалуйста введите слово. Разрешено использовать только буквы русского алфавита. Размер слова: "];
string[] arrTurn = 
    ["Excellent! Let's play! A word: ",
    "Отлично! Играем! Слово: "];
string[] arrSecRem = 
    [" seconds remaining...",
    " секунд осталось..."];
string[] arrTimeSUp = 
    ["Your time's up.",
    "Время вышло."];
string[] arrGameOverText = 
    ["Game is over. The winner is ",
    "Игра закончена. Победитель "];
string[] arrGameOverDrowText =
    ["Game is over. It's draw.",
    "Игра закончена. Ничья "];
string[] arrGameStatText = 
    ["Please check game statistics with a word ",
    "Пожалуйста проверьте статистику игры со словом "];
string[] arrCorrectWord = 
    [" is correct. Good job!",
    " подходит. Отличная работа!"];
string[] arrEmptyWord = 
    ["Empty word is not allowed. Please try again.",
    "Пустое слово недопустимо. Попробуйте еще раз."];
string[] arrNotChar = 
    [" char isn't in main word.",
    " нет в главном слове."];
string[] arrTurnPlayerText = 
    ["Turn of ",
    "Ход игрока "];

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
    playerNames = ["Player 1", "Player 2"];

    chooseLanguage();

    //clearConsole();

    // enter new word
    printMessage(arrWordText[choosedLanguage - 1] + $"{minNumber} - {maxNumber}");
    mainWord = readMessage();

    // check a word correctness
    bool isWordValid = false;
    // regex for main word
    Regex wordRegex = new Regex(arrRegex[choosedLanguage - 1] + "{" + minNumber + "," + maxNumber + "}$");

    do
    {
        if (!validateMainWord(mainWord, wordRegex))
        {
            //clearConsole();
            printMessage(arrWordText[choosedLanguage - 1] + $"{minNumber} - {maxNumber}"); // check if a word doesn't fit the parameters
            mainWord = readMessage();   // enter a word
        }
        else
        {
            isWordValid = true;
        }
    } while (!isWordValid);
}

void chooseLanguage()
{
    printMessage("Please choose the language:");
    for (int i = 0; i < languages.Length; i++)
    {
        printMessage((i + 1) + ". " + languages[i]);
    }

    choosedLanguage = int.Parse(readMessage());
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
    printMessage(arrTurn[choosedLanguage - 1] + $"{mainWord}");

    int turn = 1;           // turn number
    string tmpPlayerName;   // temp player name

    while (!isGameFinished)
    {
        tmpPlayerName = (turn % 2 == 1) ? playerNames[0] : playerNames[1];
        printMessage("start");
        setTimer();
        printMessage("settimer");
        playerTurn(tmpPlayerName, turn);
        printMessage("playerturn");
        timer.Stop();
        printMessage("stoptimer");
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
    printMessage("starttimer " + localDate);
    timer.Start();
}

void Timer_Elapsed(Object source, System.Timers.ElapsedEventArgs e)
{
    if (tmpTurnTime > 0)
    {
        printMessage(tmpTurnTime-- + arrSecRem[choosedLanguage - 1]);
    }
    else
    {
        printMessage(arrTimeSUp[choosedLanguage - 1]);
        timer.Stop(); // Stop the timer
        printMessage("Stoptimer");
        tmpTurnTime = turnTime;
        isGameFinished = true;
    } 
}

// method show stat at the end of game
void finishGame(List<string> statList)
{
    if (statList.Count == 0)
    {
        printMessage(arrGameOverDrowText[choosedLanguage - 1]);
    }
    else
    {
        string tmp = statList.ToArray()[statList.Count - 1].Split("|")[0];
        printMessage(arrGameOverText[choosedLanguage - 1] + $"{tmp}");
    }
    
    if (statList.Count > 1)
    {
        printMessage(arrGameStatText[choosedLanguage - 1] + $"{mainWord}");
        string[] arr = statList.ToArray();
        for (int i = 0; i < arr.Length; i++)
        {
            string[] tmpArr = arr[i].Split('|');
            printMessage((i + 1) + ". " + tmpArr[0] + ". " + tmpArr[1] + ".");
        }
    }
}

// clear console
void clearConsole()
{
    Console.Clear();
}

void playerTurn(string playerName, int turn)
{
    //clearConsole();

    string lowerWord = mainWord.ToLower();     // main word in lower case
    string playerWord;

    printMessage($"{turn}. " + playerName + ", your try: ");
    
    // player word shouldn't be empty
    playerWord = readMessage();
    while (playerWord == "")
    {
        printMessage(arrEmptyWord[choosedLanguage - 1]); // check if a word doesn't fit the parameters
        playerWord = readMessage();                      
    }

    // check if a word is correct
    if (checkWord(lowerWord, playerWord))
    {
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
            printMessage($"'{ch}'" + arrNotChar[choosedLanguage - 1]);
            result = false;
        }
        mainWordList.Remove(ch);
    }

    return result;
}