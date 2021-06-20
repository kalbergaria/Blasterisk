using System;
using System.Threading;
using System.Diagnostics;
using System.Timers;

namespace Blasterisk
{
    class BlasteriskUI
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        // Console Window
        ///////////////////////////////////////////////////////////////////////////////////////////
        private static readonly int CONSOLE_WIDTH   = 106;
        private static readonly int CONSOLE_HEIGHT  = 25;

        ///////////////////////////////////////////////////////////////////////////////////////////
        // UI State Management
        ///////////////////////////////////////////////////////////////////////////////////////////
        public enum UIState
        {
            DEFAULT = 0,
            TITLE,
            MENU,
            PLAY,
            SEED,
            LEADERBOARD,
            EXIT,
            ERROR
        }
        private UIState uiState = UIState.DEFAULT;

        ///////////////////////////////////////////////////////////////////////////////////////////
        // Title Screen
        ///////////////////////////////////////////////////////////////////////////////////////////
        //                                                                                                        1
        //              1         2         3         4         5         6         7         8         9         0
        //    0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345
        //   +----------------------------------------------------++----------------------------------------------------+
        //  0|                                                                                                          |
        //  1|                                                                                                          |
        //  2|                                                                                                          |
        //  3|                                                                                                          |
        //  4|                                                                                                          |
        //  5|                                                                                                          |
        //  6|                                                                                                          |
        //  7|      _     ______   _        _______   ______  _______  _______  ______   _   ______  _     _     _      |
        //  8|   _ | | _ (____  \ (_)      (_______) / _____)(_______)(_______)(_____ \ | | / _____)(_)   | | _ | | _   |
        //  9|  ( \| |/ ) ____)  ) _        _______ ( (____      _     _____    _____) )| |( (____   _____| |( \| |/ )  |
        // 10|   )     ( |  __  ( | |      |  ___  | \____ \    | |   |  ___)  |  __  / | | \____ \ |  _   _) )     (   |
        // 11|  (_/| |\_)| |__)  )| |_____ | |   | | _____) )   | |   | |_____ | |  \ \ | | _____) )| |  \ \ (_/| |\_)  |
        // 12+     |_|   |______/ |_______)|_|   |_|(______/    |_|   |_______)|_|   |_||_|(______/ |_|   \_)   |_|     +
        // 13|                                                                                                          |
        // 14|                                                                                                          |
        // 15|                                                                                                          |
        // 16|                                                                                                          |
        // 17|                                                                                                          |
        // 18|                                          PRESS ANY KEY TO PLAY!                                          |
        // 19|                                                                                                          |
        // 20|                                                                                                          |
        // 21|                                                                                                          |
        // 22|                                                                                                          |
        // 23|                                                                                                          |
        // 24|                                                                                                          |
        //   +----------------------------------------------------++----------------------------------------------------+

        private static readonly string PRESS_ANY_KEY_PROMPT              = "PRESS ANY KEY TO PLAY!";
        private static readonly int PRESS_ANY_KEY_PROMPT_COL             = 42;
        private static readonly int PRESS_ANY_KEY_PROMPT_ROW             = 17;
        private static readonly int PRESS_ANY_KEY_PROMPT_FLASH_INTERVAL  = 600; // ms

        // Upon expiration of this timer, the prompt will either be erased or printed
        // to emulate the prompt flashing to the user.
        private static FlashTimer pressAnyKeyPromptTimer;

        ///////////////////////////////////////////////////////////////////////////////////////////
        // Menu Screen
        ///////////////////////////////////////////////////////////////////////////////////////////      
        //                                                                                                        1
        //              1         2         3         4         5         6         7         8         9         0
        //    0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345
        //   +----------------------------------------------------++----------------------------------------------------+
        //  0|                                                                                                          |
        //  1|                                                                                                          |
        //  2|                                                                                                          |
        //  3|                                                                                                          |
        //  4|                                                                                                          |
        //  5|                                               _     _                                                    |
        //  6|                                              |_)|  |_|\ / |                                              |
        //  7|                                              |  |__| | |  o                                              |
        //  8|                                                                                                          |
        //  9|                                                                                                          |
        // 10|                                  ___ __         __ _  _  __ _  _  _  _  _  _                             |
        // 11|                              \ /  | |_ | |  |  |_ |_|| \|_ |_)|_)/ \|_||_)| \                            |
        // 12+                               V  _|_|__|^|  |__|__| ||_/|__| \|_)\_/| || \|_/                            |
        // 13|                                                                                                          |
        // 14|                                                                                                          |
        // 15|                                                __   ______                                               |
        // 16|                                               |_ \ / |  |                                                |
        // 17|                                               |__/ \_|_ |                                                |
        // 18|                                                                                                          |
        // 19|                                                                                                          |
        // 20|                                                                                                          |
        // 21|                                                                                                          |
        // 22|                                                                                                          |
        // 23|                                                                                                          |
        // 24|                                                                                                          |
        //   +----------------------------------------------------++----------------------------------------------------+

        public enum MenuOptions
        {
            PLAY = 0,
            VIEW_LEADERBOARD,
            EXIT
        }
        private MenuOptions selectedOption = MenuOptions.PLAY;

        private static readonly string[] MENU_PLAY_STRINGS =
        {@"  _     _       ",
         @" |_)|  |_|\ / | ",
         @" |  |__| | |  o ",
         @"                "};
        private static readonly int MENU_PLAY_START_COL = 45;
        private static readonly int MENU_PLAY_START_ROW = 5;
        private static readonly ConsoleColor MENU_PLAY_COLOR = ConsoleColor.Magenta;

        private static readonly string[] VIEW_LEADERBOARD_STRINGS = 
        {@"     ___ __         __ _  _  __ _  _  _  _  _  _  ",                                                                    
         @" \ /  | |_ | |  |  |_ |_|| \|_ |_)|_)/ \|_||_)| \ ",
         @"  V  _|_|__|^|  |__|__| ||_/|__| \|_)\_/| || \|_/ ",
         @"                                                  "};
        private static readonly int MENU_VIEW_LEADBOARD_START_COL = 29;
        private static readonly int MENU_VIEW_LEADBOARD_START_ROW = 10;
        private static readonly ConsoleColor MENU_VIEW_LEADBOARD_COLOR = ConsoleColor.DarkYellow;

        private static readonly string[] EXIT_STRINGS =
        {@"  __   ______ ",
         @" |_ \ / |  |  ",
         @" |__/ \_|_ |  ",
         @"              "};
        private static readonly int MENU_EXIT_START_COL = 46;
        private static readonly int MENU_EXIT_START_ROW = 15;
        private static readonly ConsoleColor MENU_EXIT_COLOR = ConsoleColor.Cyan;

        private static readonly string[][] MENU_OPTION_STRINGS =
        {MENU_PLAY_STRINGS,
         VIEW_LEADERBOARD_STRINGS,
         EXIT_STRINGS};
        private static readonly int[] MENU_OPTION_START_COLS =
        {MENU_PLAY_START_COL,
         MENU_VIEW_LEADBOARD_START_COL,
         MENU_EXIT_START_COL};
        private static readonly int[] MENU_OPTION_START_ROWS =
        {MENU_PLAY_START_ROW,
         MENU_VIEW_LEADBOARD_START_ROW,
         MENU_EXIT_START_ROW};
        private static readonly ConsoleColor[] MENU_OPTION_COLORS =
        {MENU_PLAY_COLOR,
         MENU_VIEW_LEADBOARD_COLOR,
         MENU_EXIT_COLOR};

        ///////////////////////////////////////////////////////////////////////////////////////////
        // Play Screen
        ///////////////////////////////////////////////////////////////////////////////////////////      
        //                                                                                                        1
        //              1         2         3         4         5         6         7         8         9         0
        //    0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345
        //   +----------------------------------------------------++----------------------------------------------------+
        //  0|                                                                 ||              __ __ __ _               |
        //  1|                                                                 ||             (_ |_ |_ | \              |
        //  2|                                                                 ||             __)|__|__|_/              |
        //  3|                                                                 ||                   123456              |
        //  4|                                                                 ||                                       |
        //  5|                                                                 ||             __ __ _  _  __            |
        //  6|                                                                 ||            (_ /  / \|_)|_             |
        //  7|                                                                 ||            __)\__\_/| \|__            |
        //  8|                                                                 ||                     123456            |
        //  9|                                                                 ||                                       |
        // 10|                                                                 ||                __    __               |
        // 11|                                                                 ||            |  |_ \ /|_ |              |
        // 12+                                                                 ||            |__|__ V |__|__            +
        // 13|                                                                 ||                     123456            |
        // 14|                                                                 ||                                       |
        // 15|                                                                 ||             ______    __              |
        // 16|                                                                 ||              |  | |V||_               |
        // 17|                                                                 ||              | _|_| ||__              |
        // 18|                                                                 ||                 HH:MM:SS              |
        // 19|                                                                 ||                                       |
        // 20|     *                                                           ||         ___ __    __ __ _  _  __      |
        // 21|                                                                 ||     |_| | /__|_| (_ /  / \|_)|_       |
        // 22|   =====                                                         ||     | |_|_\_|| | __)\__\_/| \|__      |
        // 23|                                                                 ||                           123456      |
        // 24|                                                                 ||                                       |
        //   +----------------------------------------------------++----------------------------------------------------+

        public enum PlayScreenFields
        {
            SEED = 0,
            SCORE,
            LEVEL,
            TIME,
            HIGH_SCORE
        }

        private static readonly int PLAY_DIVIDER_START_COL = 65;
        private static readonly string PLAY_DIVIDER_STRING = "||";
        private static readonly ConsoleColor PLAY_DIVIDER_COLOR = ConsoleColor.White;

        private static readonly string[] PLAY_SEED_STRINGS =
        {@" __ __ __ _ ",
         @"(_ |_ |_ | \",
         @"__)|__|__|_/"};
        private static readonly int PLAY_SEED_START_COL = 80;
        private static readonly int PLAY_SEED_START_ROW = 0;
        private static readonly ConsoleColor PLAY_SEED_COLOR = ConsoleColor.Red;
        private static readonly int PLAY_SEED_DATA_START_COL = 86;
        private static readonly int PLAY_SEED_DATA_START_ROW = 3;
        private static readonly int PLAY_SEED_DATA_MAX_DIGITS = 6;
        private static readonly ConsoleColor PLAY_SEED_DATA_COLOR = ConsoleColor.White;
        private int seed = 0;

        private static readonly string[] PLAY_SCORE_STRINGS =
        {@" __ __ _  _  __",
         @"(_ /  / \|_)|_ ",
         @"__)\__\_/| \|__"};
        private static readonly int PLAY_SCORE_START_COL = 79;
        private static readonly int PLAY_SCORE_START_ROW = 5;
        private static readonly ConsoleColor PLAY_SCORE_COLOR = ConsoleColor.DarkYellow;
        private static readonly int PLAY_SCORE_DATA_START_COL = 88;
        private static readonly int PLAY_SCORE_DATA_START_ROW = 8;
        private static readonly int PLAY_SCORE_DATA_MAX_DIGITS = 6;
        private static readonly ConsoleColor PLAY_SCORE_DATA_COLOR = ConsoleColor.White;
        private int score = 0;

        private static readonly string[] PLAY_LEVEL_STRINGS =
        {@"    __    __   ",
         @"|  |_ \ /|_ |  ",
         @"|__|__ V |__|__"};
        private static readonly int PLAY_LEVEL_START_COL = 79;
        private static readonly int PLAY_LEVEL_START_ROW = 10;
        private static readonly ConsoleColor PLAY_LEVEL_COLOR = ConsoleColor.Green;
        private static readonly int PLAY_LEVEL_DATA_START_COL = 88;
        private static readonly int PLAY_LEVEL_DATA_START_ROW = 13;
        private static readonly int PLAY_LEVEL_DATA_MAX_DIGITS = 6;
        private static readonly ConsoleColor PLAY_LEVEL_DATA_COLOR = ConsoleColor.White;
        private int level = 0;

        private static readonly string[] PLAY_TIME_STRINGS =
        {@"______    __",
         @" |  | |V||_ ",
         @" | _|_| ||__"};
        private static readonly int PLAY_TIME_START_COL = 80;
        private static readonly int PLAY_TIME_START_ROW = 15;
        private static readonly ConsoleColor PLAY_TIME_COLOR = ConsoleColor.Cyan;
        private static readonly int PLAY_TIME_DATA_START_COL = 84;
        private static readonly int PLAY_TIME_DATA_START_ROW = 18;
        private static readonly ConsoleColor PLAY_TIME_DATA_COLOR = ConsoleColor.White;
        private Stopwatch levelStopWatch = new Stopwatch();

        private static readonly string[] PLAY_HIGH_SCORE_STRINGS =
        {@"    ___ __    __ __ _  _  __",
         @"|_| | /__|_| (_ /  / \|_)|_ ",
         @"| |_|_\_|| | __)\__\_/| \|__"};
        private static readonly int PLAY_HIGH_SCORE_START_COL = 72;
        private static readonly int PLAY_HIGH_SCORE_START_ROW = 20;
        private static readonly ConsoleColor PLAY_HIGH_SCORE_COLOR = ConsoleColor.Blue;
        private static readonly int PLAY_HIGH_SCORE_DATA_START_COL = 94;
        private static readonly int PLAY_HIGH_SCORE_DATA_START_ROW = 23;
        private static readonly int PLAY_HIGH_SCORE_DATA_MAX_DIGITS = 6;
        private static readonly ConsoleColor PLAY_HIGH_SCORE_DATA_COLOR = ConsoleColor.White;
        private int highScore = 0;

        private static readonly string[][] PLAY_FIELDS_STRINGS =
        {PLAY_SEED_STRINGS,
         PLAY_SCORE_STRINGS,
         PLAY_LEVEL_STRINGS,
         PLAY_TIME_STRINGS,
         PLAY_HIGH_SCORE_STRINGS};
        private static readonly int[] PLAY_FIELDS_START_COLS =
        {PLAY_SEED_START_COL,
         PLAY_SCORE_START_COL,
         PLAY_LEVEL_START_COL,
         PLAY_TIME_START_COL,
         PLAY_HIGH_SCORE_START_COL};
        private static readonly int[] PLAY_FIELDS_START_ROWS =
        {PLAY_SEED_START_ROW,
         PLAY_SCORE_START_ROW,
         PLAY_LEVEL_START_ROW,
         PLAY_TIME_START_ROW,
         PLAY_HIGH_SCORE_START_ROW};
        private static readonly ConsoleColor[] PLAY_FIELDS_COLORS =
        {PLAY_SEED_COLOR,
         PLAY_SCORE_COLOR,
         PLAY_LEVEL_COLOR,
         PLAY_TIME_COLOR,
         PLAY_HIGH_SCORE_COLOR};

        // Update the Time and Score based every time this timer expires
        private static System.Timers.Timer scoreAndTimeUpdateTimer;
        private static readonly int SCORE_TIME_UPDATE_INTERVAL = 100; // ms
        private static Mutex scoreMutex = new Mutex();
        private static Mutex timeMutex = new Mutex();

        ///////////////////////////////////////////////////////////////////////////////////////////
        // Constructor
        ///////////////////////////////////////////////////////////////////////////////////////////
        public BlasteriskUI()
        {
            // Setup the console
            Console.SetWindowSize(CONSOLE_WIDTH, CONSOLE_HEIGHT);
            Console.CursorVisible = false;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // 
        ///////////////////////////////////////////////////////////////////////////////////////////
        public UIState titleScreen()
        {
            setUiState(UIState.TITLE);

            // Print logo
            BlasteriskLogo.printBlasteriskLogo();

            // Start the timer to periodically print the prompt
            pressAnyKeyPromptTimer = new FlashTimer
            {
                Interval = PRESS_ANY_KEY_PROMPT_FLASH_INTERVAL,
                Showing = false,
            };
            pressAnyKeyPromptTimer.Elapsed += onPressAnyKeyPromptTimerExpiration;
            pressAnyKeyPromptTimer.Start();

            // Wait for a key press
            // NOTE: The "true" prevents the key press from being writtern to the screen
            Console.ReadKey(true);

            // Next UIState is the menu
            return UIState.MENU;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // 
        ///////////////////////////////////////////////////////////////////////////////////////////
        private static void onPressAnyKeyPromptTimerExpiration(Object source, ElapsedEventArgs e)
        {
            // Get if the prompt is currently showing or not
            bool showing = ((FlashTimer)source).Showing;

            // Move the cursor to position and either print or erase the prompt
            Console.SetCursorPosition(PRESS_ANY_KEY_PROMPT_COL, PRESS_ANY_KEY_PROMPT_ROW);
            if (showing == false)
            {
                Console.Write(PRESS_ANY_KEY_PROMPT);
            }
            else
            {                
                // Write spaces the length of the prompt to erase the prompt
                foreach(char c in PRESS_ANY_KEY_PROMPT)
                {
                    Console.Write(' ');
                }
            }

            // Toggle the showing flag
            ((FlashTimer)source).Showing = !showing;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        //
        ///////////////////////////////////////////////////////////////////////////////////////////
        public UIState menu()
        {
            // Update the UI state
            setUiState(UIState.MENU);

            // Save the console color
            ConsoleColor foreColorToRestore = Console.ForegroundColor;
            ConsoleColor backColorToRestore = Console.BackgroundColor;

            // Clear the console
            Console.Clear();

            bool validKey = true;
            bool selectionMade = false;
            do
            {
                // Print each menu option
                foreach (int option in Enum.GetValues(typeof(MenuOptions)))
                {
                    // For the selected option, swap the fore and back colors
                    if (option == ((int)selectedOption))
                    {
                        Console.BackgroundColor = MENU_OPTION_COLORS[option];
                        Console.ForegroundColor = backColorToRestore;
                    }
                    else
                    {
                        Console.BackgroundColor = backColorToRestore;
                        Console.ForegroundColor = MENU_OPTION_COLORS[option];
                    }

                    // Print the strings for the option
                    int row = MENU_OPTION_START_ROWS[option];
                    foreach (string s in MENU_OPTION_STRINGS[option])
                    {
                        Console.SetCursorPosition(MENU_OPTION_START_COLS[option], row++);
                        Console.Write(s);
                    }
                }

                // Read the input from the user
                do
                {
                    validKey = true;
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.UpArrow:
                            // If the current option isn't the first, move to the option above
                            if (selectedOption != MenuOptions.PLAY)
                            {
                                // Move to the option above
                                int intOption = ((int)selectedOption) - 1;
                                if (Enum.IsDefined(typeof(MenuOptions), intOption))
                                {
                                    selectedOption = (MenuOptions)intOption;
                                }
                            }
                            break;

                        case ConsoleKey.DownArrow:
                            // If the current option isn't the last, move to the option below
                            if (selectedOption != MenuOptions.EXIT)
                            {
                                int intOption = ((int)selectedOption) + 1;
                                if (Enum.IsDefined(typeof(MenuOptions), intOption))
                                {
                                    selectedOption = (MenuOptions)intOption;
                                }
                            }
                            break;

                        case ConsoleKey.Enter:
                            selectionMade = true;
                            break;

                        default:
                            validKey = false;
                            break;
                    }
                } while (!validKey);

            } while (!selectionMade);

            // Restore the console color
            Console.ForegroundColor = foreColorToRestore;
            Console.BackgroundColor = backColorToRestore;

            // Determine the next state for the UI based on the user selection
            UIState nextUiState = UIState.DEFAULT;
            switch(selectedOption)
            {
                case MenuOptions.PLAY:
                    nextUiState = UIState.PLAY;
                    break;

                case MenuOptions.VIEW_LEADERBOARD:
                    nextUiState = UIState.LEADERBOARD;
                    break;

                case MenuOptions.EXIT:
                    nextUiState = UIState.EXIT;
                    break;

                default:
                    nextUiState = UIState.ERROR;
                    break;
            }
            return nextUiState;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        //
        ///////////////////////////////////////////////////////////////////////////////////////////
        public UIState play()
        {
            // Update the UI state
            setUiState(UIState.MENU);

            // Save the console color
            ConsoleColor foreColorToRestore = Console.ForegroundColor;
            ConsoleColor backColorToRestore = Console.BackgroundColor;

            // Clear the console
            Console.Clear();

            // Draw the play screen divider
            Console.ForegroundColor = PLAY_DIVIDER_COLOR;
            for(int i = 0; i < CONSOLE_HEIGHT; i++)
            {
                Console.SetCursorPosition(PLAY_DIVIDER_START_COL, i);
                Console.Write(PLAY_DIVIDER_STRING);
            }

            // Print each play screen field
            foreach (int field in Enum.GetValues(typeof(PlayScreenFields)))
            {
                // Set the color for the field
                Console.ForegroundColor = PLAY_FIELDS_COLORS[field];

                // Print the strings for the field
                int row = PLAY_FIELDS_START_ROWS[field];
                foreach (string s in PLAY_FIELDS_STRINGS[field])
                {
                    Console.SetCursorPosition(PLAY_FIELDS_START_COLS[field], row++);
                    Console.Write(s);
                }
            }

            // Print the seed
            // NOTE: This only has to be done once since the seed doesn't change throughout
            // the course of the game
            Console.ForegroundColor = PLAY_SEED_DATA_COLOR;
            Console.SetCursorPosition(PLAY_SEED_DATA_START_COL, PLAY_SEED_DATA_START_ROW);
            Console.Write(seed.ToString("D" + PLAY_SEED_DATA_MAX_DIGITS));

            // Print the high score for the current seed
            // NOTE: This only has to be done once since the high score doesn't change throughout
            // the course of the game
            Console.ForegroundColor = PLAY_HIGH_SCORE_DATA_COLOR;
            Console.SetCursorPosition(PLAY_HIGH_SCORE_DATA_START_COL, 
                PLAY_HIGH_SCORE_DATA_START_ROW);
            Console.Write(highScore.ToString("D" + PLAY_HIGH_SCORE_DATA_MAX_DIGITS));

            // Update the level
            level++;
            Console.ForegroundColor = PLAY_LEVEL_DATA_COLOR;
            Console.SetCursorPosition(PLAY_LEVEL_DATA_START_COL,
                PLAY_LEVEL_DATA_START_ROW);
            Console.Write(level.ToString("D" + PLAY_LEVEL_DATA_MAX_DIGITS));

            // Start the level stopwatch
            levelStopWatch.Start();

            // Start the timer to periodically print the prompt
            scoreAndTimeUpdateTimer = new System.Timers.Timer();
            scoreAndTimeUpdateTimer.Elapsed += (sender, e) =>
                onScoreTimeUpdateTimerExpiration(sender, e, this);
            scoreAndTimeUpdateTimer.Start();

            // TEST SCORE
            int q = 0;
            while (true)
            {
                setScore(q++);
                Thread.Sleep(2000);
            }

            // Restore the console color
            Console.ForegroundColor = foreColorToRestore;
            Console.BackgroundColor = backColorToRestore;

            return UIState.EXIT;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // 
        ///////////////////////////////////////////////////////////////////////////////////////////
        private static void onScoreTimeUpdateTimerExpiration(Object source, ElapsedEventArgs e,
            BlasteriskUI bui)
        {
            // Update the score
            Console.SetCursorPosition(PLAY_SCORE_DATA_START_COL, PLAY_SCORE_DATA_START_ROW);
            Console.Write(bui.getScore().ToString("D" + PLAY_SCORE_DATA_MAX_DIGITS));

            // Update the time
            Console.SetCursorPosition(PLAY_TIME_DATA_START_COL, PLAY_TIME_DATA_START_ROW);
            TimeSpan levelElapsedTime = bui.getLevelElapsedTime();
            Console.Write(String.Format("{0:00}:{1:00}:{2:00}",
                levelElapsedTime.Hours, levelElapsedTime.Minutes, levelElapsedTime.Seconds));
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        //
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void setUiState(UIState state)
        {
            // Stop the PRESS_ANY_KEY_PROMPT from flashing if leaving the TITLE state
            if (uiState == UIState.TITLE)
            {
                pressAnyKeyPromptTimer.Stop();
            }

            // Set the new state
            uiState = state;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        //
        ///////////////////////////////////////////////////////////////////////////////////////////
        public int getScore()
        {
            int tempScore = 0;

            scoreMutex.WaitOne();
            tempScore = score;
            scoreMutex.ReleaseMutex();

            return tempScore;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        //
        ///////////////////////////////////////////////////////////////////////////////////////////
        public TimeSpan getLevelElapsedTime()
        {
            TimeSpan levelElapsedTime;

            scoreMutex.WaitOne();
            levelElapsedTime = levelStopWatch.Elapsed;
            scoreMutex.ReleaseMutex();

            return levelElapsedTime;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        //
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void setScore(int score)
        {
            scoreMutex.WaitOne();
            this.score = score;
            scoreMutex.ReleaseMutex();
        }
    }
}
