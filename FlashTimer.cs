using System;
using System.Collections.Generic;
using System.Text;

namespace Blasterisk
{
    class FlashTimer : System.Timers.Timer
    {
        private bool showing;
        public bool Showing
        {
            get { return showing;  }
            set { showing = value;  }
        }
    }
}
