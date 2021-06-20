using System;

namespace Blasterisk
{
    class Program
    {
        static void Main(string[] args)
        {
            BlasteriskUI blasteriskUi = new BlasteriskUI();
            BlasteriskUI.UIState nextUiState = blasteriskUi.titleScreen();

            bool exit = false;
            while(!exit)
            {
                switch (nextUiState)
                {
                    case BlasteriskUI.UIState.MENU:
                        nextUiState = blasteriskUi.menu();
                        break;

                    case BlasteriskUI.UIState.PLAY:
                        nextUiState = blasteriskUi.play();
                        break;

                    case BlasteriskUI.UIState.SEED:
                        break;

                    case BlasteriskUI.UIState.LEADERBOARD:
                        break;

                    case BlasteriskUI.UIState.EXIT:
                        exit = true;
                        break;

                    default: // DEFAULT and ERROR go here
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine("FATAL ERROR: Invalid UI state... EXITING!");
                        break;
                }
            }
		}
    }
}
