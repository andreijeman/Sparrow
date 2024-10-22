using Client.Pages;
using ConsoleUI.Elements;
using Logger;
using System.Net;
using Server.Postman;

namespace Client
{
    public class Program
    {
        ILogger logger = new ConsoleLogger();
        
        public static void Main(string[] args)
        {
            Client client = new Client(new LoggerPage());

            ConnectPage connectPage = new ConnectPage();
            connectPage.ConnectEvent += client.ConnectToServerAsync;
            BasePage.CurrentPage = connectPage;
            



            Thread.Sleep(10000000);

        }       
    }
}
