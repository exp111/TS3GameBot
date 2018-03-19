using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.Commands
{
	class CommandTransfer : CommandBase
	{
		public CommandTransfer(string label, string description) : base(label, description)
		{
			this.Usage = "<target> <amount>";
		}

		public override bool Execute(List<string> args, TextMessage message)
		{
			//TODO: COMMENT ALL OF IT @EXP

			if(args.Count < 2)
			{
				CommandManager.AnswerCall(message, "\nUsage:\n" + CommandManager.CmdIndicator + this.Label + " " + this.Usage);
				return false;
			}
			if (!Int32.TryParse(args[1], out int amount))
			{
				CommandManager.AnswerCall(message, Utils.Utils.ApplyColor(Color.Red) + "\n" + args[1] + " is not a number![/COLOR]");
				return false;
			}
			if(amount <= 0)
			{
				CommandManager.AnswerCall(message, Utils.Utils.ApplyColor(Color.Red) + "\nNumber must be positive![S](smartass, huh?)[/S][/COLOR]");
				return false;
			}

			CasinoPlayer invoker = DbInterface.GetPlayer(message.InvokerUid);
			List<CasinoPlayer> targets = DbInterface.GetPlayerList(name: args[0]);					

			if(targets.Count != 1)
			{
				CommandManager.AnswerCall(message, Utils.Utils.ApplyColor(Color.Red) + "\nTarget not Found or not Registered![/COLOR]");
				return false;
			}

			if (invoker == targets[0])
			{
				CommandManager.AnswerCall(message, Utils.Utils.ApplyColor(Color.Red) + "\nJust no![/COLOR]");
				return false;
			}

			if (invoker.Points < amount)
			{
				CommandManager.AnswerCall(message, Utils.Utils.ApplyColor(Color.Red) + "\nNot enough Points in your Wallet![S](get fucked)[/S][/COLOR]");
				return false;
			}

			DbInterface.AlterPoints(invoker, -amount);
			DbInterface.AlterPoints(targets[0], +amount);
			DbInterface.SaveChanges();

			// TODO: Tell peepz their accounts have been temperatured with

			return false;
		}
	}
}
