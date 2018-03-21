using System;
using System.Collections.Generic;
using System.Text;
using TS3GameBot.CommandStuff.Commands;
using TS3GameBot.CommandStuff.ConsoleCommands;

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
		}
		private static void RegisterCommand(ConsoleCommandBase command)
		{
			Commands.Add(command.Label.ToLower(), command);
		}
	}
}
