using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.Commands
{
    class CommandSpin : CommandBase
    {
		public int Cost { get; private set; }
		public int MagicNumber { get; private set; }


		public CommandSpin(string label, string description) : base(label, description)
		{
			this.Usage = "";
			this.NeedsRegister = true;

			this.Cost = 25;
			this.MagicNumber = 6;
		}

		public override bool Execute(List<string> args, TextMessage message)
		{
			CasinoPlayer myPlayer = DbInterface.GetPlayer(message.InvokerUid);
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
				CommandManager.AnswerCall(message, Utils.Utils.ApplyColor(Color.Red) + "\nNot enough Points in your Wallet![S](get fucked)[/S][/COLOR]");
				return false;
			}

			//Generate Numbers
			Random run = new Random();
			int firstNum = run.Next(1, 9);
			int secondNum = run.Next(1, 9);
			int thirdNum = run.Next(1, 9);

			int price = 0;

			//Create Message
			outMessage.Append(CommandManager.ClientUrl(myPlayer.Id, myPlayer.Name) + " has paid " + this.Cost + " to spin the Slot Machine!");
			outMessage.Append("#############\n");
			outMessage.Append("# " + firstNum + " | " + secondNum + " | " + thirdNum + " #\n");
			outMessage.Append("#############\n");

			//Did you win something? prolly not
			if (firstNum == MagicNumber && secondNum == MagicNumber && thirdNum == MagicNumber)
			{//lucky bastard
				outMessage.Append("\nYou've won something really good!\n");
				price = Cost * 10;
			}
			else if ((firstNum == MagicNumber && secondNum == MagicNumber) ||
					 (firstNum == MagicNumber && thirdNum == MagicNumber) ||
					 (secondNum == MagicNumber && thirdNum == MagicNumber)) //medium 
			{
				outMessage.Append("\nYou've won a neato price!\n");
				price = Cost * 5;
			}
			else if (firstNum == MagicNumber || secondNum == MagicNumber || thirdNum == MagicNumber)
			{//better than nothing amirite
				outMessage.Append("\nYou've won a meh price!\n");
				price = Cost * 3;
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

			if (!DbInterface.AlterPoints(myPlayer, price - this.Cost)) //Give/Take money
			{ //Can't change points for some reason
				CommandManager.AnswerCall(message, "Shitty mcshitfuck");
				throw new Exception("Shitty mcschit fuck again");
			}
			DbInterface.SaveChanges();
			outMessage.Append("You now have " + myPlayer.Points + " Points!");

			CommandManager.AnswerCall(message, outMessage.ToString());
			return true;
		}
	}
}
