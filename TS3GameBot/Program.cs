using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamSpeak3QueryApi.Net.Specialized;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TeamSpeak3QueryApi.Net.Specialized.Responses;
using TS3GameBot.CommandStuff;
using TS3GameBot.CommandStuff.Commands;
using TS3GameBot.Utils;

namespace TS3GameBot
{
	class Program
	{
		public static IReadOnlyList<GetClientsInfo> CurrentClients { get; private set; }

		public Queue<String> ConsoleQueue { get; } = new Queue<string>();

		public static bool Running = true;

		static void Main(string[] args)
		{
			Console.Title = "Shit 2k18";
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.Green;

			ConsoleCommandManager.RegisterCommands();


			Thread botThread = new Thread(RunBot);
			botThread.Start(1);

			List<String> commandArgs = new List<string>();
			Console.WriteLine("Welcome to Dj's GameBot!");

			while (Running)
			{								
				Console.Write("> ");
				String shit = Console.ReadLine();
				ConsoleCommandBase cmd;

				String[] parts = shit.Split(" ");

				for (int i = 1; i < parts.Length; i++)
				{
					commandArgs.Add(parts[i]); // Putin all the args in a List 
				}
				try
				{
					cmd = ConsoleCommandManager.Commands[parts[0].ToLower()]; // Getting the Command Assoscccscsiated to the give Command			

					cmd.Execute(commandArgs);

				}
				catch (KeyNotFoundException)
				{
					Console.Clear();
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Unknown Command '" + shit.Split(" ")[0] + "'");
					Console.ForegroundColor = ConsoleColor.Green;
					Console.Beep();
				}
			}
		}

		public static void RunBot(object sid)
		{
			GameBot myBot = GameBot.Instance;

			myBot.Login().Wait();
			myBot.StartBot((int)sid).Wait();

			myBot.EventShit();
			CommandStuff.CommandManager.RegisterCommands();

			CurrentClients = myBot.TSClient.GetClients().GetAwaiter().GetResult();

			ulong i = 0;
			ulong refreshRate = 1000 / 20;

			while (Running)
			{

				if (i++ > refreshRate * 60)
				{
					CurrentClients = myBot.TSClient.GetClients().GetAwaiter().GetResult();
					i = 0;
				}			

				Thread.Sleep(20);
			}
			myBot.TSClient.Quit();
			Console.WriteLine("Bot Closed");
			Console.Read();
		}
	}	
	
}
