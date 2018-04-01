using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TS3GameBot.CommandStuff.Commands;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.ConsoleCommands
{
	class ConsoleCommandFind : ConsoleCommandBase
	{
		public ConsoleCommandFind(string label, string description) : base(label, description)
		{
			this.Usage = "[name | uid]";
		}

		internal override CCR Execute(List<string> args, PersonDb db)
		{
			List<CasinoPlayer> playerList = new List<CasinoPlayer>();
			if (args.Count != 1 || args[0] == "")
			{
				return CCR.INVALIDPARAM;
			}

			if (args[0].EndsWith("="))
			{
				playerList.Add(DbInterface.GetPlayer(args[0], db));
			}
			else
			{
				playerList = DbInterface.GetPlayerList(name: args[0], fuzzy: true);
			}
			if (playerList.Count == 0 || playerList[0] == null)
			{
				return CCR.PLAYERNOTFOUND;
			}
			StringBuilder outMessage = new StringBuilder();

			outMessage.
				AppendFormat("{0, -15} | {1, -15} | {2, -28} | {3, -15}\n", "Name", "Points", "uid", "SteamID64");
				//AppendFormat("{0, -15} | {0, -15} | {0, -28} | {0, -15}\n", "");

			foreach (CasinoPlayer player in playerList)
			{			
				outMessage.
					AppendFormat("{0, -15} | {1, -15} | {2, -28} | {3, -15}\n", player.Name, player.Points, player.Id, player.SteamID64);
			}

			Console.WriteLine(outMessage);

			return CCR.OK;
		}
	}
}
