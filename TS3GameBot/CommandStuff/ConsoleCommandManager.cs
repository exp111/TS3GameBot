using System;
using System.Collections.Generic;
using System.Text;
using TS3GameBot.CommandStuff.Commands;
using TS3GameBot.CommandStuff.ConsoleCommands;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff
{
	class ConsoleCommandManager
	{
		static public Dictionary<String, ConsoleCommandBase> Commands { get; } = new Dictionary<String, ConsoleCommandBase>();

		public static void RegisterCommands()
		{
			RegisterCommand(new ConsoleCommandStop("stop", "Stop the Bot"));
			RegisterCommand(new ConsoleCommandHelp("help", "Displays a List of commands / Displays usage of given Command"));
			RegisterCommand(new ConsoleCommandPoints("points", "Edits the Points of a given Player"));
			RegisterCommand(new ConsoleCommandList("list", "List all registered Players"));
			RegisterCommand(new ConsoleCommandClear("clear", "Clears the Console Window"));
			RegisterCommand(new ConsoleCommandFind("find", "Finds a given Player"));
			RegisterCommand(new ConsoleCommandTS3("ts3", "Lists all Online users on the TS3"));
			RegisterCommand(new ConsoleCommandAdd("add", "Add a new Player to the Database"));
			RegisterCommand(new ConsoleCommandDelete("delete", "Delete a Player from the Database"));
			RegisterCommand(new ConsoleCommandChange("edit", "Change the Name and SteamID of a Player"));
		}
		private static void RegisterCommand(ConsoleCommandBase command)
		{
			Commands.Add(command.Label.ToLower(), command);
		}

		internal static CCR ExecuteCommand(ConsoleCommandBase cmd, List<String> args)
		{
			using (PersonDb db = new PersonDb())
			{
				return cmd.Execute(args, db);
			}			
		}
	}
}
