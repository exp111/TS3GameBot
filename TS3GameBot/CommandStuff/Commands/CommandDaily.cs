using System;
using System.Collections.Generic;
using System.Text;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TS3GameBot.DBStuff;
using TS3GameBot.Utils;

namespace TS3GameBot.CommandStuff.Commands
{
	class CommandDaily : CommandBase
	{
		private int DailyReward { get; } = 50;

		public CommandDaily(string label, string description) : base(label, description)
		{
			this.Usage = "";
			this.NeedsRegister = true;
		}

		public override bool Execute(List<string> args, TextMessage message)
		{
			CasinoPlayer myPlayer = DbInterface.GetPlayer(message.InvokerUid);
			StringBuilder outMessage = new StringBuilder();

			outMessage.Append("\n");

			//myPlayer.LastDaily <= DateTime.Now.AddDays(-1)
			if (myPlayer.LastDaily.Year < DateTime.Now.Year ||
				myPlayer.LastDaily.Month < DateTime.Now.Month ||
				myPlayer.LastDaily.Day < DateTime.Now.Day)
			{
				Error result = DbInterface.UpdateDaily(myPlayer.Id, this.DailyReward);
				switch (result)
				{
					case Error.OK:
						outMessage.Append(this.DailyReward + " Points have been added to your Wallet!\nYou now have " + myPlayer.Points + " Points in your Wallet!");
						break;
					case Error.SAVEERROR:
						outMessage.Append("Changes could not be saved to the Database!");
						break;
					case Error.UNKNOWN:
					default:
						outMessage.Append("An Unknown Error Occured");
						break;
				}
			}
			else
			{
				outMessage.Append(CommandManager.ClientUrl(myPlayer.Id, myPlayer.Name) + ": You already received your Daily reward " + Utils.Utils.MsToM((DateTime.Now - myPlayer.LastDaily).TotalMilliseconds) + " ago!\nCome back in " + Utils.Utils.MsToM((DateTime.Now.AddDays(1).Date - DateTime.Now).TotalMilliseconds) + ".");
				CommandManager.AnswerCall(message, outMessage.ToString());
				return false;
			}
			CommandManager.AnswerCall(message, outMessage.ToString());
			return true;
		}
	}
}
