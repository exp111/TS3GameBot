using System;
using System.Collections.Generic;
using System.Text;
using TS3GameBot.CommandStuff.Commands;

namespace TS3GameBot.CommandStuff.ConsoleCommands
{
	class ConsoleCommandHelp : ConsoleCommandBase
	{
		public ConsoleCommandHelp(string label, string description) : base(label, description)
		{
			this.Usage = "[command]";
		}

		public override CCR Execute(List<string> args)
		{
			if(args.Count > 1)
			{
				return CCR.WRONGPARAM;
			}

			StringBuilder outputMessage = new StringBuilder();

			if (args.Count != 0)
			{
				foreach (KeyValuePair<String, ConsoleCommandBase> cmd in ConsoleCommandManager.Commands)
				{
					if (args[0] == cmd.Key)
					{
						outputMessage.Append(cmd.Value.GetUsage() + "\n");
						Console.Write(outputMessage.ToString());
						return CCR.OK;
					}
				}
				return CCR.INVALIDPARAM;
			}
			else
			{
				foreach (KeyValuePair<String, ConsoleCommandBase> cmd in ConsoleCommandManager.Commands)
				{
					outputMessage.
						Append(cmd.Key + "").
						Append(": ").
						Append(cmd.Value.Description + "\n");

				}
				Console.Write(outputMessage.Append("\n").ToString());
				return CCR.OK;
			}
		}
	}
}
