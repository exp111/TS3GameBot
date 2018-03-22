using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TS3GameBot.CommandStuff.Commands;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.ConsoleCommands
{
	class ConsoleCommandList : ConsoleCommandBase
	{
		public ConsoleCommandList(string label, string description) : base(label, description)
		{
			this.Usage = "[Players per Page] [Page]";
		}

		public override CCR Execute(List<string> args)
		{
			int page = 1;
			int perPage = 10;
			if (args.Count != 0) //We have parameter
			{
				if (!Int32.TryParse(args[0], out perPage)) //Parameter 1 (playersperpage) NaN => return
				{
					return CCR.NOTANUMBER;
				}
				if (!Int32.TryParse((args.Count == 2 ? args[1] : "1"), out page)) //Parameter 2 (page) NaN =>return
				{
					return CCR.NOTANUMBER;
				}
			}

			StringBuilder outMessage = new StringBuilder();

			//Calculate the index, endindex & page count
			int endIndex = (page * perPage);
			int index = (endIndex - perPage);
			index = index <= 0 ? 0 : index;

			int playerCount = DbInterface.GetPlayerCount();
			int pageCount = (int)Math.Ceiling((double)playerCount / perPage);
			pageCount = pageCount == 0 ? 1 : pageCount;

			if (page > pageCount) //Page is higher than the max page count
			{
				outMessage.Append("There are only " + pageCount + " pages!");
			}
			else
			{
				outMessage.Append("Total Players: " + playerCount + "\nPage (" + page + "/" + pageCount + ")\n");

				List<CasinoPlayer> shit = DbInterface.GetPlayerList(index, endIndex).OrderByDescending(p => p.Points).ToList();
				outMessage.AppendFormat("{0, -15} | {1, -15} | {2, -28} | {3, -15}\n\n", "Name", "Points", "uid", "SteamID64");

				foreach (CasinoPlayer player in shit)
				{
					outMessage.
						AppendFormat("{0, -15} | {1, -15} | {2, -28} | {3, -15}\n", player.Name, player.Points, player.Id, player.SteamID64);
				}
			}
			Console.WriteLine(outMessage);
			return CCR.OK;
		}
	}
}
