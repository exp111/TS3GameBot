using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using TeamSpeak3QueryApi.Net.Specialized;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TeamSpeak3QueryApi.Net.Specialized.Responses;
using TS3GameBot.Utils;

namespace TS3GameBot
{
    class Program
    {
		public static IReadOnlyList<GetClientsInfo> CurrentClients { get; private set; }

		async static Task Main(string[] args)
		{
			Console.Title = "Shit 2k18";
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.Green;

			GameBot myBot = GameBot.Instance;

			await myBot.Login();
			myBot.EventShit();
			CommandStuff.CommandManager.RegisterCommands();

			 CurrentClients = await myBot.TSClient.GetClients();
			// var targets = currentClients.Where(c => c.NickName.StartsWith("M"));

			
			while (true)
			{
				await Task.Delay(1000);
				CurrentClients = await myBot.TSClient.GetClients();
			}

			//Console.ReadLine();
		}
    }
}
