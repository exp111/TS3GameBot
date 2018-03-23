using System;
using System.Collections.Generic;
using System.Text;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.Commands
{
	class CommandStore : CommandBase
	{
		public CommandStore(string label, string description) : base(label, description)
		{
			this.Usage = "<list | buy> [item]";
		}

		public override bool Execute(List<string> args, TextMessage message)
		{
			if (args.Count < 1) //Not enough parameters => Show Usage
			{
				CommandManager.AnswerCall(message, "\nUsage:\n" + CommandManager.CmdIndicator + this.Label + " " + this.Usage);
				return false;
			}

			if (args[0].ToLower() == "buy") //buy item
			{
				if (args.Count == 2) //if we have no item to buy -> show list
				{
					//Get Invoker
					CasinoPlayer invoker = DbInterface.GetPlayer(message.InvokerUid);

					if (invoker == null) //Invoker not registered => tell 'em boi
					{
						CommandManager.AnswerCall(message, Responses.NotRegistered);
						return false;
					}

					/*if (invoker.Points < amount) //Not enough points
					{
						CommandManager.AnswerCall(message, Utils.Utils.ApplyColor(Color.Red) + "\nNot enough Points in your Wallet![S](get fucked)[/S][/COLOR]");
						return false;
					}

					//Change the points nauw
					if (!DbInterface.AlterPoints(invoker, -amount)) //Can't change points for some reason
					{
						CommandManager.AnswerCall(message, "Shitty mcshitfuck");
						throw new Exception("Shitty mcschit fuck again");
						//return false;
					}
					DbInterface.AlterPoints(targets[0], +amount);
					DbInterface.SaveChanges();
					
					//Tell the peepz about their item
					CommandManager.AnswerCall(message, Utils.Utils.ApplyColor(Color.DarkGreen) + "\nTransfer done![/COLOR]\n" + CommandManager.ClientUrl(invoker.Id, invoker.Name) + ": " + invoker.Points + " Points\n" + CommandManager.ClientUrl(targets[0].Id, targets[0].Name) + ": " + targets[0].Points + " Points");
					*/
				}
			}

			if (args[0].ToLower() == "list") //List items
			{
				//List
				CommandManager.AnswerCall(message, "\nItems:");
				return true;
			}

			return true;
		}
	}
}