using System;

namespace Blasterisk
{
    class Program
    {
        static void Main(string[] args)
        {
            BlasteriskUI blasteriskUi = new BlasteriskUI();
            blasteriskUi.titleScreen();
            blasteriskUi.menu();
		}
    }
}
