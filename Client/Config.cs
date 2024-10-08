using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class Config
    {
        public static class Chat
        {
            public static readonly char[] BorderTemplate = { '─', '│', '┌', '┐', '┘', '└' };

            public const int MessageBoxMinWidth = 20;
            public const int MessageBoxMaxWidth = 40;

            public const int MessageBoxMinHeight = 1;
            public const int MessageBoxMaxHeight = 40;

            public const ConsoleColor SendedTextColor = ConsoleColor.Gray;
            public const ConsoleColor SendedTextBorderColor = ConsoleColor.Magenta;

            public const ConsoleColor ReceivedTextColor = ConsoleColor.Gray;
            public const ConsoleColor ReceivedTextBorderColor = ConsoleColor.Blue;

            

            public const bool ActivateRandomColor = true;

            public const int RightLimit = 0;
            public const int LeftLimit = 100;

            public const string Logo = @"
     _____                                        
    / ___/____  ____ _______________ _      __    
    \__ \/ __ \/ __ `/ ___/ ___/ __ \ | /| / /    
   ___/ / /_/ / /_/ / /  / /  / /_/ / |/ |/ /     
  /____/ .___/\__,_/_/  /_/   \____/|__/|__/ \      
   _  /_/  _________________________________\ \     
  /_________________________________________/ /
                                           /_/
";
        }
    }
}
