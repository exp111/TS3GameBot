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
		}

		public override bool Execute(List<string> args, TextMessage message)
		{
			StringBuilder outMessage = new StringBuilder();

			CasinoPlayer tempPlayer = DbInterface.GetPlayer(message.InvokerUid);
			if(tempPlayer == null)
			{
				CommandManager.AnswerCall(message, Responses.NotRegistered);
				return false;
			}

			outMessage.Append("\n").
				Append(CommandManager.ClientUrl(tempPlayer.Id, tempPlayer.Name) + ": You have " + tempPlayer.Points + " in your Wallet!");

			Console.WriteLine(outMessage);
			return CommandManager.AnswerCall(message, outMessage.ToString());
		}
	}
}
