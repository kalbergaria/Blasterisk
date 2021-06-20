using System;
using System.Threading;
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
            INITIAL = 0,
            TITLE,
            MENU,
            PLAY,
            SEED,
            LEADERBOARD
        }
        private UIState uiState = UIState.INITIAL;

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
        private static Mutex pressAnyKeyPromptMutex = new Mutex();

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
        public void titleScreen()
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
        public void menu()
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
                // Print the logo
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
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        //
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void setUiState(UIState state)
        {
            pressAnyKeyPromptMutex.WaitOne();

            // Stop the PRESS_ANY_KEY_PROMPT from flashing if leaving the TITLE state
            if (uiState == UIState.TITLE)
            {
                pressAnyKeyPromptTimer.Stop();
            }

            // Set the new state
            uiState = state;
            pressAnyKeyPromptMutex.ReleaseMutex();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        //
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void updateGameStatus()
        {

        }
    }
}
