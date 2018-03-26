using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.Commands
{
	class CommandWallet : CommandBase
	{
		public CommandWallet(string label, string description) : base(label, description)
		{
			this.Usage = "";
			this.WIP = false;
		}

		internal override bool Execute(List<string> args, TextMessage message, PersonDb db)
		{
			StringBuilder outMessage = new StringBuilder();

			CasinoPlayer tempPlayer = DbInterface.GetPlayer(message.InvokerUid, db);

			outMessage.Append("\n").
				Append(CommandManager.ClientUrl(tempPlayer.Id, tempPlayer.Name) + ": You have " + tempPlayer.Points + " in your Wallet!");

			return CommandManager.AnswerCall(message, outMessage.ToString());
		}
	}
}
