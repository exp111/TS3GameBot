using System;
using System.Collections.Generic;
using System.Text;
using TS3GameBot.CommandStuff.Commands;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.ConsoleCommands
{
	class ConsoleCommandPoints : ConsoleCommandBase
	{
		public ConsoleCommandPoints(string label, string description) : base(label, description)
		{
			this.Usage = "<uid> [change | set] [<value>]";
		}

		public override CCR Execute(List<string> args)
		{
			#region argCompute
			if (args.Count == 0 || args.Count == 2 || args.Count > 3)
			{
				return CCR.WRONGPARAM;
			}
			CasinoPlayer player = DbInterface.GetPlayer(args[0]);
			if(player == null)
			{
				return CCR.PLAYERNOTFOUND;
			}
			#endregion
			// show case
			if ( args.Count == 1 )
			{
				Console.WriteLine("Name: " + player.Name + "\nPoints: " + player.Points + "\nSteam: " + player.SteamID64);
				return CCR.OK;
			}
			// change case
			if (args[1].ToLower() == "change")
			{
				if(!Int32.TryParse(args[2], out int amount))
				{
					return CCR.NOTANUMBER;
				}
				if(!DbInterface.AlterPoints(player, amount))
				{
					return CCR.NOTENOUGHPOINTS;
				}
				if (!DbInterface.AlterPoints(player, amount))
				{
					return CCR.NOTENOUGHPOINTS;
				}


				switch (DbInterface.SaveChanges())
				{
					case Error.OK:
						Console.WriteLine("Points altered from " + (player.Points - amount) + " to " + player.Points);
						return CCR.OK;
					//	break;
					case Error.SAVEERROR:
						DbInterface.AlterPoints(player, -amount);
						return CCR.DBWRITEFAILED;
					//	break;

					case Error.UNKNOWN:
					default:
						return CCR.UNKNOWN;
					//	break;
				}			

			}
			// set case
			if(args[1].ToLower() == "set")
			{
				if (!Int32.TryParse(args[2], out int amount))
				{
					return CCR.NOTANUMBER;
				}
				int oldAmount = player.Points;
				if (!DbInterface.SetPoints(player, amount))
				{
					return CCR.BELOWZERO;
				}			

				switch (DbInterface.SaveChanges())
				{
					case Error.OK:
						Console.WriteLine(player.Name + "'s Points were altered from " + oldAmount + " to " + player.Points);
						return CCR.OK;
					//	break;
					case Error.SAVEERROR:
						DbInterface.SetPoints(player, oldAmount);
						return CCR.DBWRITEFAILED;
					//	break;

					case Error.UNKNOWN:
					default:
						return CCR.UNKNOWN;
						//	break;
				}
			}
			return CCR.UNKNOWN;
		}
	}
}
