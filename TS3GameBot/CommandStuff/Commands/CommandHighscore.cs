﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.Commands
{
	class CommandHighscore : CommandBase
	{
		public CommandHighscore(string label, string description) : base(label, description)
		{
			this.Usage = "[ Name ]";
			this.NeedsRegister = false;
		}

		internal override bool Execute(List<string> args, TextMessage message, PersonDb db)
		{
			StringBuilder outMessage = new StringBuilder();
			int i = 1;
			if (args.Count == 0)
			{
				List<CasinoPlayer> playerList = DbInterface.GetPlayerList().OrderByDescending(p => p.Points).ToList();

				foreach (CasinoPlayer player in playerList)
				{
					outMessage.
						Append($"\n{i}. Player {CommandManager.ClientUrl(player.Id, player.Name)} has a Total of {player.Points:n0}!");
					i++;
				}
			}
			else if(args.Count == 1)
			{
				List<CasinoPlayer> playerList = DbInterface.GetPlayerList(name: args[0]).OrderByDescending(p => p.Points).ToList();

				foreach (CasinoPlayer player in playerList)
				{
					outMessage.
						Append($"\n{i}. Player {CommandManager.ClientUrl(player.Id, player.Name)} has a Total of {player.Points}!");
					i++;
				}
			}
			else
			{
				CommandManager.AnswerCall(message, $"\nUnknown usage!\n{this.Usage}");
			}
			
			CommandManager.AnswerCall(message, outMessage.ToString());

			return true;
		}
	}
}
