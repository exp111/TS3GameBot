using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.Commands
{
	class CommandSpin : CommandBase
	{
		public int Cost { get; } = 25;
		public int MagicNumber { get; } = 6;


		public CommandSpin(string label, string description) : base(label, description)
		{
			this.Usage = "";
			this.NeedsRegister = true;
		}

		internal override bool Execute(List<string> args, TextMessage msg, PersonDb db)
		{
			CasinoPlayer myPlayer = DbInterface.GetPlayer(msg.InvokerUid, db);
			StringBuilder outMessage = new StringBuilder();

			outMessage.Append("\n");
			/*
			 The user Exp has pulled the SLOTMACHINE!
				#############
				# 5 | 6 | 5 #
				#############
				And they won a meh Price...
			  */

			if (myPlayer.Points < this.Cost) //Not enough points
			{
				CommandManager.AnswerCall(msg, $"{Utils.Utils.ApplyColor(Color.Red)}\nNot enough Points in your Wallet![S](get fucked)[/S][/COLOR]");
				return false;
			}

			//Generate Numbers
			Random run = new Random();
			int firstNum = run.Next(1, 9);
			int secondNum = run.Next(1, 9);
			int thirdNum = run.Next(1, 9);

			int price = 0;

			//Create Message
			outMessage.Append($"{CommandManager.ClientUrl(myPlayer.Id, myPlayer.Name)} has paid {this.Cost} to spin the Slot Machine!\n");
			outMessage.Append("########\n");
			outMessage.Append($"# {firstNum} |  {secondNum}  |  {thirdNum} #\n");
			outMessage.Append("########\n");

			//Did you win something? prolly not
			if (firstNum == MagicNumber && secondNum == MagicNumber && thirdNum == MagicNumber)
			{//lucky bastard
				outMessage.Append("\nYou've won something really good!\n");
				price = Cost * 2;
			}
			else if ((firstNum == MagicNumber && secondNum == MagicNumber) ||
					 (firstNum == MagicNumber && thirdNum == MagicNumber) ||
					 (secondNum == MagicNumber && thirdNum == MagicNumber)) //medium 
			{
				outMessage.Append("\nYou've won a neato price!\n");
				price = Cost + 10;
			}
			else if (firstNum == MagicNumber || secondNum == MagicNumber || thirdNum == MagicNumber)
			{//better than nothing amirite
				outMessage.Append("\nYou've won a meh price!\n");
				price = Cost - 10;
			}
			else if (firstNum == 4 && secondNum == 0 && thirdNum == 4)
			{
				outMessage.Append("\nPrice not found!\n");
				price = Cost;
			}
			else //Nothing
			{
				outMessage.Append("\nYou've won [S]a price[/S] [B]nothing[/B]!\n");
				price = 0;
			}

			int add = price - this.Cost;
			if (!DbInterface.AlterPoints(myPlayer, add)) //Give/Take money
			{ //Can't change points for some reason
				CommandManager.AnswerCall(msg, "An Unknown Error Occured! \nGive this to your Admin: 'Alter Points failed in CommandSpin.cs'");
				throw new Exception("Alter Points failed in CommandSpin.cs");
			}
			DbInterface.SaveChanges(db);

			outMessage.Append(add >= 0 ? "You have won " : "You have lost ");
			outMessage.Append($"{add} Points. You now have {myPlayer.Points} Points!");

			CommandManager.AnswerCall(msg, outMessage.ToString());
			return true;
		}
	}
}
