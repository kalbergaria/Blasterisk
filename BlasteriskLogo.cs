using System;
using System.Collections.Generic;
using System.Text;

namespace Blasterisk
{
    class BlasteriskLogo
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        // Logo
        ///////////////////////////////////////////////////////////////////////////////////////////
        //      _     ______   _        _______   ______  _______  _______  ______   _   ______  _     _     _    
        //   _ | | _ (____  \ (_)      (_______) / _____)(_______)(_______)(_____ \ | | / _____)(_)   | | _ | | _ 
        //  ( \| |/ ) ____)  ) _        _______ ( (____      _     _____    _____) )| |( (____   _____| |( \| |/ )
        //   )     ( |  __  ( | |      |  ___  | \____ \    | |   |  ___)  |  __  / | | \____ \ |  _   _) )     ( 
        //  (_/| |\_)| |__)  )| |_____ | |   | | _____) )   | |   | |_____ | |  \ \ | | _____) )| |  \ \ (_/| |\_)
        //     |_|   |______/ |_______)|_|   |_|(______/    |_|   |_______)|_|   |_||_|(______/ |_|   \_)   |_|   

        private static readonly int[] LOGO_ROWS = { 5, 6, 7, 8, 9, 10 };
        private static readonly ConsoleColor[] LOGO_COLORS = {ConsoleColor.DarkRed,
                                                              ConsoleColor.Red,
                                                              ConsoleColor.DarkYellow,
                                                              ConsoleColor.Green,
                                                              ConsoleColor.Cyan,
                                                              ConsoleColor.Blue,
                                                              ConsoleColor.DarkBlue,
                                                              ConsoleColor.DarkMagenta,
                                                              ConsoleColor.DarkRed,
                                                              ConsoleColor.Red,
                                                              ConsoleColor.DarkYellow,
                                                              ConsoleColor.Green};

        private static readonly string[] LOGO_ASTERISK = {@"    _    ",
                                                          @" _ | | _ ",
                                                          @"( \| |/ )",
                                                          @" )     ( ",
                                                          @"(_/| |\_)",
                                                          @"   |_|   "};
        private static readonly int LOGO_ASTERISK1_START_COL = 2;
        private static readonly int LOGO_ASTERISK2_START_COL = 95;


        private static readonly string[] LOGO_LETTER_B = {@" ______  ",
                                                          @"(____  \ ",
                                                          @" ____)  )",
                                                          @"|  __  ( ",
                                                          @"| |__)  )",
                                                          @"|______/ "};
        private static readonly int LOGO_LETTER_B_START_COL = 11;

        private static readonly string[] LOGO_LETTER_L = {@" _       ",
                                                          @"(_)      ",
                                                          @" _       ",
                                                          @"| |      ",
                                                          @"| |_____ ",
                                                          @"|_______)"};
        private static readonly int LOGO_LETTER_L_START_COL = 20;

        private static readonly string[] LOGO_LETTER_A = {@" _______ ",
                                                          @"(_______)",
                                                          @" _______ ",
                                                          @"|  ___  |",
                                                          @"| |   | |",
                                                          @"|_|   |_|"};
        private static readonly int LOGO_LETTER_A_START_COL = 29;

        private static readonly string[] LOGO_LETTER_S = {@"  ______ ",
                                                          @" / _____)",
                                                          @"( (____  ",
                                                          @" \____ \ ",
                                                          @" _____) )",
                                                          @"(______/ "};
        private static readonly int LOGO_LETTER_S1_START_COL = 38;
        private static readonly int LOGO_LETTER_S2_START_COL = 77;

        private static readonly string[] LOGO_LETTER_T = {@" _______ ",
                                                          @"(_______)",
                                                          @"    _    ",
                                                          @"   | |   ",
                                                          @"   | |   ",
                                                          @"   |_|   "};
        private static readonly int LOGO_LETTER_T_START_COL = 47;

        private static readonly string[] LOGO_LETTER_E = {@" _______ ",
                                                          @"(_______)",
                                                          @" _____   ",
                                                          @"|  ___)  ",
                                                          @"| |_____ ",
                                                          @"|_______)"};
        private static readonly int LOGO_LETTER_E_START_COL = 56;

        private static readonly string[] LOGO_LETTER_R = 
        {@" ______  ",
         @"(_____ \ ",
         @" _____) )",
         @"|  __  / ",
         @"| |  \ \ ",
         @"|_|   |_|"};
        private static readonly int LOGO_LETTER_R_START_COL = 65;

        private static readonly string[] LOGO_LETTER_I = {@" _ ",
                                                          @"| |",
                                                          @"| |",
                                                          @"| |",
                                                          @"| |",
                                                          @"|_|"};
        private static readonly int LOGO_LETTER_I_START_COL = 74;

        private static readonly string[] LOGO_LETTER_K = {@" _     _ ",
                                                          @"(_)   | |",
                                                          @" _____| |",
                                                          @"|  _   _)",
                                                          @"| |  \ \ ",
                                                          @"|_|   \_)"};
        private static readonly int LOGO_LETTER_K_START_COL = 86;

        private static readonly string[][] LOGO_CHARACTERS = {LOGO_ASTERISK,
                                                              LOGO_LETTER_B,
                                                              LOGO_LETTER_L,
                                                              LOGO_LETTER_A,
                                                              LOGO_LETTER_S,
                                                              LOGO_LETTER_T,
                                                              LOGO_LETTER_E,
                                                              LOGO_LETTER_R,
                                                              LOGO_LETTER_I,
                                                              LOGO_LETTER_S,
                                                              LOGO_LETTER_K,
                                                              LOGO_ASTERISK};
        private static readonly int CHARACTER_DIMENSION = 0;

        private static readonly int[] LOGO_CHARACTER_START_COLS = {LOGO_ASTERISK1_START_COL,
                                                                   LOGO_LETTER_B_START_COL,
                                                                   LOGO_LETTER_L_START_COL,
                                                                   LOGO_LETTER_A_START_COL,
                                                                   LOGO_LETTER_S1_START_COL,
                                                                   LOGO_LETTER_T_START_COL,
                                                                   LOGO_LETTER_E_START_COL,
                                                                   LOGO_LETTER_R_START_COL,
                                                                   LOGO_LETTER_I_START_COL,
                                                                   LOGO_LETTER_S2_START_COL,
                                                                   LOGO_LETTER_K_START_COL,
                                                                   LOGO_ASTERISK2_START_COL};

        public static void printBlasteriskLogo()
        {
            // Save the console color
            ConsoleColor colorToRestore = Console.ForegroundColor;
            
            // Print the logo
            for (int charNum = 0; charNum < 
                LOGO_CHARACTERS.GetLength(CHARACTER_DIMENSION); charNum++)
            {
                Console.ForegroundColor = LOGO_COLORS[charNum];
                for (int row = 0; row < LOGO_ROWS.Length; row++)
                {
                    Console.SetCursorPosition(LOGO_CHARACTER_START_COLS[charNum], LOGO_ROWS[row]);
                    Console.Write(LOGO_CHARACTERS[charNum][row]);
                }
            }

            // Restore the console color
            Console.ForegroundColor = colorToRestore;

        }
    }
}
