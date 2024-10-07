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

            public static readonly char[] BoxTemplate = { '─', '│', '┌', '┐', '┘', '└' };

            public const ConsoleColor TextColor = ConsoleColor.Gray;
            public const ConsoleColor BoxColor = ConsoleColor.White;
        
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
