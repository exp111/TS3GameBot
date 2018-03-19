﻿using System;
using System.Collections.Generic;
using System.Text;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TeamSpeak3QueryApi.Net.Specialized.Responses;

namespace TS3GameBot.CommandStuff.Commands
{
	class CommandHelp : CommandBase
	{
		public CommandHelp(string label, string description) : base(label, description)
		{
			this.Usage = "< command >";
		}

		public override bool Execute(List<String> args, TextMessage message)
		{
			StringBuilder outputMessage = new StringBuilder();

			if(args.Count != 0)
			{
				foreach (KeyValuePair<String, CommandBase> cmd in CommandManager.Commands)
				{
					if(args[0] == cmd.Key)
					{
						outputMessage.Append(cmd.Value.GetUsage());
						CommandManager.AnswerCall(message, outputMessage.ToString());
						Console.WriteLine(outputMessage);
						return true;
					}
				}
			}
			else
			{
				foreach (KeyValuePair<String, CommandBase> cmd in CommandManager.Commands)
				{
					if (cmd.Value.Hidden)
					{
						continue;
					}

					outputMessage.
						Append(cmd.Value.Enabled ? "" : "[S]" ).
						Append(cmd.Value.WIP ? "[COLOR=#ff0000]" : "[COLOR=#006f00]" ).
						Append("\n" + CommandManager.CmdIndicator + cmd.Key + "[/COLOR]").
						Append(": ").
						Append(cmd.Value.Description + (cmd.Value.WIP ? " (WIP)" : "" )).
						Append(cmd.Value.Enabled ? "" : "[/S]");				

				}
				CommandManager.AnswerCall(message, outputMessage.ToString());
				Console.WriteLine(outputMessage);
				return true;
			}



			return false;

		}
	}
}
