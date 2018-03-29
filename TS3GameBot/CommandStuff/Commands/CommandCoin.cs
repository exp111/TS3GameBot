using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.Commands
{
    class CommandCoin : CommandBase
    {
		public CommandCoin(string label, string description) : base(label, description)
		{
			this.Usage = "<heads | tails> <wager = 5>";
			this.NeedsRegister = true;
		}

		internal override bool Execute(List<string> args, TextMessage msg, PersonDb db)
		{
			if (args.Count < 1)
			{
				CommandManager.AnswerCall(msg, $"\nUsage:\n{CommandManager.CmdIndicator}{this.Label} {this.Usage}");
				return false;
			}

			String bet = args[0].ToLower();
			if (bet != "heads" && bet != "tails")
			{
				CommandManager.AnswerCall(msg, $"\nUsage:\n{CommandManager.CmdIndicator}{this.Label} {this.Usage}");
				return false;
			}

			int wager = 5;
			if (args.Count > 1)
			{
				if (!Int32.TryParse(args[1], out wager))
				{
					CommandManager.AnswerCall(msg, $"{Utils.Utils.ApplyColor(Color.Red)}\n{args[1]} is not a number![/COLOR]");
					return false;
				}
			}

			CasinoPlayer myPlayer = DbInterface.GetPlayer(msg.InvokerUid, db);
			
			if (myPlayer.Points < wager) //Not enough points
			{
				CommandManager.AnswerCall(msg, Responses.NotEnoughPoints);
				return false;
			}

			StringBuilder outMessage = new StringBuilder();
			outMessage.Append("\n");

			//Generate Numbers
			Random run = new Random();
			int edge = run.Next(0, 6000);
			int flip = 0;

			int price = 0;

			if (edge != 1)  //1 in 6k
			{
				flip = run.Next(0, 1);
				if (flip == 0)
				{
					outMessage.Append("The Coin shows Heads!\n");
					if (bet.Equals("heads"))
					{
						price = wager * 2;
					}
				}
				else
				{
					outMessage.Append("The Coin shows Tails!\n");
					if (bet.Equals("tails"))
					{
						price = wager * 2;
					}
				}
			}
			else
			{
				outMessage.Append("The Coin landed on its edge! What a crooked game!\n");
			}

			int add = price - wager;
			if (!DbInterface.AlterPoints(myPlayer, add)) //Give/Take money
			{ //Can't change points for some reason
				CommandManager.AnswerCall(msg, "An Unknown Error Occured! \nGive this to your Admin: 'Alter Points failed in CommandCoin.cs'");
				throw new Exception("Alter Points failed in CommandCoin.cs");
			}
			DbInterface.SaveChanges(db);

			outMessage.Append(add >= 0 ? $"You have won {add}" : $"You have lost {-add}");
			outMessage.Append($" Points. You now have {myPlayer.Points} Points!");

			CommandManager.AnswerCall(msg, outMessage.ToString());
			return true;
		}
	}
}
