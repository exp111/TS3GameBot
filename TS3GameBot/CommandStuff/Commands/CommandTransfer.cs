using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamSpeak3QueryApi.Net.Specialized;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TS3GameBot.DBStuff;
using TS3GameBot.Utils;

namespace TS3GameBot.CommandStuff.Commands
{
	class CommandTransfer : CommandBase
	{
		public CommandTransfer(string label, string description) : base(label, description)
		{
			this.Usage = "<target> <amount>";
		}

		internal override bool Execute(List<string> args, TextMessage message, PersonDb db)
		{
			if(args.Count < 2) //Not enough parameters => Show Usage
			{
				CommandManager.AnswerCall(message, "\nUsage:\n" + CommandManager.CmdIndicator + this.Label + " " + this.Usage);
				return false;
			}

			if (!Int32.TryParse(args[1], out int amount)) //Can't parse number => NaN
			{
				CommandManager.AnswerCall(message, Utils.Utils.ApplyColor(Color.Red) + "\n" + args[1] + " is not a number![/COLOR]");
				return false;
			}
			if(amount <= 0) //Number not positive => frick off
			{
				CommandManager.AnswerCall(message, Utils.Utils.ApplyColor(Color.Red) + "\nNumber must be positive![S](smartass, huh?)[/S][/COLOR]");
				return false;
			}

			//Get Invoker & Target Player
			CasinoPlayer invoker = DbInterface.GetPlayer(message.InvokerUid, db);
			List<CasinoPlayer> targets = DbInterface.GetPlayerList(name: args[0]);	
			
			if(invoker == null) //Invoker not registered => tell 'em boi
			{
				CommandManager.AnswerCall(message, Responses.NotRegistered);
				return false;
			}

			if(targets.Count != 1) //Target not found/too many targets => same procedure as every year
			{
				CommandManager.AnswerCall(message, Utils.Utils.ApplyColor(Color.Red) + "\nTarget not Found or not Registered![/COLOR]");
				return false;
			}

			if (invoker == targets[0]) //Wanna give money to yourself? Why tho
			{
				CommandManager.AnswerCall(message, Utils.Utils.ApplyColor(Color.Red) + "\nJust no![/COLOR]");
				return false;
			}

			if (invoker.Points < amount) //Not enough points
			{
				CommandManager.AnswerCall(message, Utils.Utils.ApplyColor(Color.Red) + "\nNot enough Points in your Wallet![S](get fucked)[/S][/COLOR]");
				return false;
			}

			//Change the points nauw
			db.Players.Find(invoker.Id).Points -= amount;
			db.Players.Find(targets[0].Id).Points += amount;

			DbInterface.SaveChanges(db);

			//Tell the peepz about the transfer
			CommandManager.AnswerCall(message, Utils.Utils.ApplyColor(Color.DarkGreen) + "\nTransfer done![/COLOR]\n" + CommandManager.ClientUrl(invoker.Id, invoker.Name) + ": " + invoker.Points + " Points\n" + CommandManager.ClientUrl(targets[0].Id, targets[0].Name) + ": " + db.Players.Find(targets[0].Id).Points + " Points");

			//Private messages
			StringBuilder privateMessage = new StringBuilder();
			privateMessage.Clear().
				Append("\nYou send " + amount + " Points to " + CommandManager.ClientUrl(targets[0].Id, targets[0].Name) + "!").
				Append("\nYou now have " + invoker.Points + " Points!");

			GameBot.Instance.TSClient.SendMessage(privateMessage.ToString(), MessageTarget.Private, message.InvokerId);

			privateMessage.Clear().
				Append("You received " + amount + " Points from " + CommandManager.ClientUrl(invoker.Id, invoker.Name) + "!").
				Append("\nYou now have " + targets[0].Points + " Points!");

			var shit = Program.CurrentClients.Where(c => c.NickName == targets[0].Name);

			if(shit == null || shit.Count() == 0) //Player not online => send offline msg
			{
				GameBot.Instance.TSClient.SendOfflineMessage(targets[0].Id, privateMessage.ToString(), "You received Points!");

				return false;
			}

			privateMessage.Insert(0, "\n");

			GameBot.Instance.TSClient.SendMessage(privateMessage.ToString(), MessageTarget.Private, shit.FirstOrDefault().Id);

			return true;
		}
	}
}
