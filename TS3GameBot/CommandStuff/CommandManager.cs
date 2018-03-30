using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TS3GameBot.CommandStuff.Commands;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff
{
	static class Responses
	{
		public static String NotRegistered { get; } = "\nYou are not registered yet!\nUse " + CommandManager.CmdIndicator + CommandManager.Commands["register"].Label + " to register yourshelf.";
		public static String NotEnoughPoints { get; } = $"{Utils.Utils.ApplyColor(Color.Red)}\nNot enough Points in your Wallet![S](get fucked)[/S][/COLOR]";
		public static String NegativeNumber { get; } = $"{Utils.Utils.ApplyColor(Color.Red)}\nNumber must be positive![S](smartass, huh?)[/S][/COLOR]";
	}

	static class CommandManager
	{
		static public String CmdIndicator { get; set; } = "!";

		static public Dictionary<String, CommandBase> Commands { get; } = new Dictionary<String, CommandBase>();

		public static void RegisterCommands()
		{
			RegisterCommand(new CommandHelp("help", "Displays a list of available commands."));
			RegisterCommand(new CommandRegister("register", "Register yourshelf, fool"));
			RegisterCommand(new CommandHighscore("highscore", "List the Highscores"));
			RegisterCommand(new CommandDaily("daily", "Get your frickinnn Daily reeeeeword"));
			RegisterCommand(new CommandWallet("wallet", "Get the amount of cash you have"));
			RegisterCommand(new CommandTransfer("transfer", "Transfer your money 2 peepz"));
			RegisterCommand(new CommandSpin("spin", "Get your game on"));
			RegisterCommand(new CommandSpin("flip", "Navia aut caput"));
		}

		private static void RegisterCommand(CommandBase command)
		{
			Commands.Add(command.Label.ToLower(), command);
		}

		public static void ExecuteCommand(IReadOnlyCollection<TextMessage> data)
		{
			foreach (TextMessage msg in data) // Sry und so D:
			{				
				if (!msg.Message.StartsWith(CmdIndicator) || msg.InvokerId == GameBot.Instance.Who.ClientId) // Checking if its a command or the bot himself
				{
					return; // Fricking off
				}
				msg.Message = msg.Message.Substring(CmdIndicator.Length, msg.Message.Length - 1); // Removing the CmdIndicator

				String[] parts = msg.Message.Split(" ");
				String label = parts[0];
				List<String> args = new List<string>();

				if (parts.Length > 1)
				{
					for (int i = 1; i < parts.Length; i++)
					{
						args.Add(parts[i]); // Putin all the args in a List 
					}					
				}
				CommandBase cmd;
				try
				{
					cmd = Commands[label.ToLower()]; // Getting the Command associated to the given Command
				}
				catch (KeyNotFoundException)
				{
					AnswerCall(msg, $"\nUnknown Command '{CmdIndicator}{label}'");
					return;
				}

				if (!cmd.Enabled)
				{
					AnswerCall(msg, "\nThis Command is currently disabled!");
					return;
				}

				using (PersonDb db = new PersonDb())
				{
					if(cmd.NeedsRegister && !DbInterface.IsRegistered(msg.InvokerUid, db).GetAwaiter().GetResult())
					{
						AnswerCall(msg, Responses.NotRegistered);
						return;
					}
					cmd.Execute(args, msg, db);
					return;
				}			

			}
		}

		public static bool AnswerCall(TextMessage msg, String message)
		{
			StringBuilder endMessage = new StringBuilder();
				
			endMessage.Append("\n!!! MESSAGE DOES NOT FIT !!!");
			if (message.Length >= 1024)
			{
				message = message.Substring(0, 1024 - endMessage.Length) + endMessage;
			}
			if(message == null || message == "")
			{
				return false;
			}

			GameBot.Instance.TSClient.SendMessage(message, msg.TargetMode, 0);
			return true;

		}

		public static String ClientUrl(String uid, String name)
		{
			return $"[URL=client://0/{uid}]{name}[/URL]";
		}

	}
}
