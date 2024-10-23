using Client.Pages;
using ConsoleUI.Elements;
using Logger;
using System.Net;
using Server.Postman;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Client client = new Client(new LoggerPage());
            ConnectPage connectPage = new ConnectPage();
            ChatPage chatPage = new ChatPage();

            client.ConnectionEvent += chatPage.ConnectionAction;
            client.PacketReceivedEvent += chatPage.PrintReceivedPacket;

            connectPage.ConnectEvent += client.ConnectToServerAsync;
            
            chatPage._connectButton.Action += () => BasePage.CurrentPage = connectPage;
            chatPage._textBox.Action += async (string text) => await client.SendPacketAsync(new Packet("", Server.Postman.Label.Message, text));

            BasePage.CurrentPage = chatPage;


            Task.Delay(9999999).Wait();
        }       
    }
}
