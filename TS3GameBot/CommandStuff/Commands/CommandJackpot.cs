using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.Commands
{
    class CommandJackpot : CommandBase
    {
		public String JackpotID { get; } = "Jackpot=";

		public CommandJackpot(string label, string description) : base(label, description)
		{
			this.Usage = "";
		}

		internal override bool Execute(List<string> args, TextMessage message, PersonDb db)
		{
			//GetJackPot player
			CasinoPlayer jackpotPlayer = DbInterface.GetPlayer(this.JackpotID, db);
			if (jackpotPlayer == null)
			{
				CommandManager.AnswerCall(message, $"{Utils.Utils.ApplyColor(Color.Red)}\nNo Jackpot found! Please contact your admin![/COLOR]");
				return false;
			}

			StringBuilder outMessage = new StringBuilder();

			outMessage.Append("\n");

			outMessage.Append($"The Jackpot is currently at {jackpotPlayer.Points} Points!");
			
			CommandManager.AnswerCall(message, outMessage.ToString());
			return true;
		}
	}
}
