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
            public const int MessageBoxMinWidth = 10;
            public const int MessageBoxMaxWidth = 40;

            public const int MessageBoxMinHeight = 1;
            public const int MessageBoxMaxHeight = 40;

            public static readonly char[] BorderTemplate = { '─', '│', '┌', '┐', '┘', '└' };

            public const ConsoleColor MyMessageTextColor = ConsoleColor.Gray;
            public const ConsoleColor MyMessageBoxColor = ConsoleColor.Magenta;

            public const ConsoleColor ReceivedMessageTextColor = ConsoleColor.Gray;
            public const ConsoleColor ReceivedMessageBoxColor = ConsoleColor.Blue;

            public const bool MessageRandomColor = false;

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
