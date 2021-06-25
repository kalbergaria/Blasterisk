using System;
using System.Collections.Generic;
using System.Text;

namespace Blasterisk
{
    class HorizontalPaddle
    {
        private int row;
        private int col; // The left most point of the paddle
        private string paddleString = "";
        private string paddleEraseString = "";

        ///////////////////////////////////////////////////////////////////////////////////////////
        // Constructor
        ///////////////////////////////////////////////////////////////////////////////////////////
        public HorizontalPaddle(int row, string paddleString)
        {
            this.row = row;
            this.paddleString = paddleString;

            // Make a paddle erase string a string comprised of spaces matching the length of the
            // paddle string
            for(int i = 0; i < paddleString.Length; i++)
            {
                this.paddleEraseString += " ";
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        //
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void updatePaddle(int col)
        {
            // Erase the previous paddle
            Console.SetCursorPosition(this.col, row);
            Console.Write(paddleEraseString);

            // Draw the new paddle
            this.col = col;
            Console.SetCursorPosition(this.col, row);
            Console.Write(paddleString);
        }
    }
}
