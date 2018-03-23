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
			this.NeedsRegister = true;
		}

		public override bool Execute(List<string> args, TextMessage message)
		{
			StringBuilder outMessage = new StringBuilder();

			CasinoPlayer player = DbInterface.GetPlayer(message.InvokerUid);

			outMessage.Append("\n").
				Append(CommandManager.ClientUrl(player.Id, player.Name) + ": You have " + player.Points + " in your Wallet!");

			return CommandManager.AnswerCall(message, outMessage.ToString());
		}
	}
}
