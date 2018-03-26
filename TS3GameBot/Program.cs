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
using TS3GameBot.DBStuff;
using TS3GameBot.Utils;
using Newtonsoft.Json;
using TS3GameBot.Utils.Settings;

namespace TS3GameBot
{
	class Program
	{
		public enum ConnectionResult
		{
			UNKNOWN = -1,
			OK = 0,
			SOCKET = 1,
			QUERY = 2,
			SQLERROR = 3
		}

		public static IReadOnlyList<GetClientsInfo> CurrentClients { get; private set; }

		public Queue<String> ConsoleQueue { get; } = new Queue<string>();

		public static bool Running = true;
		
		private static String CredPathJson { get; } = "Credentials.json";

		public static Creds MyCreds { get; private set; }

		static void Main(string[] args)
		{
			Console.Title = "Shit 2k18";
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.Green;

			if (!CredManager.CredCheck(CredPathJson))
			{
				MyCreds = CredManager.CreateCreds();
				JsonSerialization.WriteToJsonFile<Creds>(CredPathJson, MyCreds);
			}
			else
			{
				MyCreds = JsonSerialization.ReadFromJsonFile<Creds>(CredPathJson);
				if(MyCreds == null)
				{
					throw new Exception("shit");
				}
			}

			Console.Clear();
			Console.WriteLine("Connecting to TeamSpeak Server...");

			TS3QueryInfo ts3ServerInfo;
			if (MyCreds.TS3InfoList.Count > 1)
			{
				Console.WriteLine("You have more than 1 TS3 Server configured!\nPlease choose 1 of the following:\n");
				foreach (var item in MyCreds.TS3InfoList)
				{
					Console.WriteLine("[" + item.Key + "] on " + item.Value.ServerAddress);
				}
				Console.Write("> ");
				String input = Console.ReadLine();
				ts3ServerInfo = MyCreds.TS3InfoList.Where(e => e.Key.ToLower().Equals(input.ToLower())).FirstOrDefault().Value;
				while (ts3ServerInfo == null)
				{
					Console.WriteLine("Not found. Try Again!");
					Console.Write("> ");
					ts3ServerInfo = MyCreds.TS3InfoList.Where(e => e.Key.ToLower().Equals(Console.ReadLine().ToLower())).First().Value;
				}
			}
			else
			{
				ts3ServerInfo = MyCreds.TS3InfoList.First().Value;
			}

			switch (GameBot.Instance.Login(ts3ServerInfo).GetAwaiter().GetResult())
			{						
				case ConnectionResult.OK:
					Console.WriteLine("Connected!");
					break;
				case ConnectionResult.SOCKET:
					Console.BackgroundColor = ConsoleColor.Red;
					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine("\nCould not connect to TeamSpeak Server! Is the Server offline?\n\nPress Enter to exit");
					while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
					return;
				//	break;
				case ConnectionResult.QUERY:
					Console.BackgroundColor = ConsoleColor.Red;
					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine("\nPlease Enter the correct Login Credentials! (" + CredPathJson + ")\n\nPress Enter to exit");
					while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
					return;
				//	break;

				case ConnectionResult.UNKNOWN:
				default:
					Console.BackgroundColor = ConsoleColor.Red;
					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine("\nCould not connect to TeamSpeak Server!\n\nPress Enter to exit");
					while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
					return;
				//	break;
			}

			Console.WriteLine("Connecting to Database...");
			ConnectionResult CResult = DbInterface.CheckConnection();
			switch (CResult)
			{
				case ConnectionResult.OK:
					Console.WriteLine("Connected!");
					break;
				case ConnectionResult.SQLERROR:
					Console.BackgroundColor = ConsoleColor.Red;
					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine("\nCould not connect to Database! Error: " + CResult + " Check your Settings! (" + CredPathJson + ")\n\nPress Enter to exit");
					while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
					Console.BackgroundColor = ConsoleColor.Black;
					Console.ForegroundColor = ConsoleColor.Green;
					return;
				//	break;

				case ConnectionResult.UNKNOWN:
				default:
					Console.BackgroundColor = ConsoleColor.Red;
					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine("\nCould not connect to Database! Error: " + CResult + "\n\nPress Enter to exit");
					while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
					Console.BackgroundColor = ConsoleColor.Black;
					Console.ForegroundColor = ConsoleColor.Green;
					return;
					//	break;
			}
			Console.WriteLine(DbInterface.GetPlayerCount() + " Players found!");// Making an Initial DB call, to get rid of the Delay on the first Commnand

			ConsoleCommandManager.RegisterCommands();
			Thread botThread = new Thread(RunBot);
			botThread.Start();

			List<String> commandArgs = new List<string>();
			Console.WriteLine("Welcome to Dj's GameBot!\nUse 'help' for a list of commands.\nUse 'help <command>' for Usage of given Command.");

			while (Running)
			{								
				Console.Write("> ");
				commandArgs.Clear();
				String shit = Console.ReadLine().Trim();
				ConsoleCommandBase cmd;

				String[] parts = shit.Split(" ");

				for (int i = 1; i < parts.Length; i++)
				{
					commandArgs.Add(parts[i]); // Putin all the args in a List 
				}
				try
				{
					cmd = ConsoleCommandManager.Commands[parts[0].ToLower()]; // Getting the Command Assoscccscsiated to the give Command			
					
					CCR result = ConsoleCommandManager.ExecuteCommand(cmd, commandArgs);					
					switch (result)
					{						
						case CCR.OK:
							break;
						case CCR.WRONGPARAM:
							Console.WriteLine(cmd.GetUsage());
							break;
						case CCR.INVALIDPARAM:
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Invalid Parameter! ");
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine(cmd.GetUsage());
							break;
						case CCR.PLAYERNOTFOUND:
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Player not Found!");
							Console.ForegroundColor = ConsoleColor.Green;
							break;
						case CCR.BELOWZERO:
						case CCR.NOTENOUGHPOINTS:
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Points can't go below 0!");
							Console.ForegroundColor = ConsoleColor.Green;
							break;

						case CCR.UNKNOWN:
						default:
							Console.BackgroundColor = ConsoleColor.Red;
							Console.ForegroundColor = ConsoleColor.White;
							Console.WriteLine("\nUnknown Error appeared! Code: " + result + "\n");
							Console.BackgroundColor = ConsoleColor.Black;
							Console.ForegroundColor = ConsoleColor.Green;
							break;
					}

				}
				catch (KeyNotFoundException)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Unknown Command '" + shit.Split(" ")[0] + "'!");
					Console.ForegroundColor = ConsoleColor.Green;
					Console.Beep();
				}
			}
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Clear();
			while (!botThread.IsAlive)
			{
				Console.WriteLine("Waiting for Bot Thread to finish!");
			}
		}

		public static void RunBot()
		{
			GameBot myBot = GameBot.Instance;
					
			myBot.StartBot(myBot.VServerID).Wait();

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
			Console.Clear();
			Console.WriteLine("Saving to Database...");
			switch (DbInterface.SaveChanges())
			{
				case Error.OK:
					Console.WriteLine("Saving Successful!\n");

					myBot.TSClient.Quit();
					Console.WriteLine("Bot Closed");
					return;
				//	break;
				case Error.SAVEERROR:
					Console.Beep();
					Console.BackgroundColor = ConsoleColor.Red;
					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine("\nNothing was written to the DataBase! \n");
					Console.BackgroundColor = ConsoleColor.Black;
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Hit Enter to exit");
					while (Console.ReadKey(true).Key != ConsoleKey.Enter)
					{
						Console.Beep();
					}
					myBot.TSClient.Quit();
					break;

				case Error.UNKNOWN:
				default:
					break;
			}			
		}
	}	
	
}
