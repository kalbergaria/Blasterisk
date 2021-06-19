using System;
using System.Collections.Generic;
using System.Text;

namespace Blasterisk
{
    class BlasteriskUI
    {
        // Console Window Constants
        const int CONSOLE_WIDTH         = 106;
        const int CONSOLE_HEIGHT        = 25;
        const int GAME_STATUS_COL_NUM   = 0;
        const int GAME_STATUS_ROW_NUM   = 24;

        public BlasteriskUI()
        {
            Console.SetWindowSize(CONSOLE_WIDTH, CONSOLE_HEIGHT);
        }

        public void updateGameStatus()
        {

        }
    }
}
