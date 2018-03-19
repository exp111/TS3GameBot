using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using TeamSpeak3QueryApi.Net.Specialized;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TS3GameBot.Utils;

namespace TS3GameBot
{
    class Program
    {
		async static Task Main(string[] args)
		{
			GameBot myBot = GameBot.Instance;

			await myBot.Login();
			myBot.EventShit();
			CommandStuff.CommandManager.RegisterCommands();

			var currentClients = await myBot.TSClient.GetClients();
			// var targets = currentClients.Where(c => c.NickName.StartsWith("M"));

			
			while (true)
			{
				await Task.Delay(1000);
			}

			//Console.ReadLine();
		}
    }
}
