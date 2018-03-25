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

			switch (GameBot.Instance.Login(MyCreds.TS3User, MyCreds.TS3Pass, MyCreds.TS3Server).GetAwaiter().GetResult())
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
					return;
				//	break;

				case ConnectionResult.UNKNOWN:
				default:
					Console.BackgroundColor = ConsoleColor.Red;
					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine("\nCould not connect to Database! Error: " + CResult + "\n\nPress Enter to exit");
					while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
					return;
					//	break;
			}
			Console.WriteLine(DbInterface.GetPlayerCount() + " Players found!");// Making an Initial DB call, to get rid of the Delay on the first Commnand

			ConsoleCommandManager.RegisterCommands();
			Thread botThread = new Thread(RunBot);
			botThread.Start(1);

			List<String> commandArgs = new List<string>();
			Console.WriteLine("Welcome to Dj's GameBot!\nUse 'help' for a list of commands.\nUse 'help <command>' for Usage of given Command.");

			while (Running)
			{								
				Console.Write("> ");
				commandArgs.Clear();
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
		}

		public static void RunBot(object sid)
		{
			GameBot myBot = GameBot.Instance;
					
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
