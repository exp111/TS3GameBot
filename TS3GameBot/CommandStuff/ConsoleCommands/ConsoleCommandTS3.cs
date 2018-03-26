using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamSpeak3QueryApi.Net.Specialized.Responses;
using TS3GameBot.CommandStuff.Commands;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.ConsoleCommands
{
	class ConsoleCommandTS3 : ConsoleCommandBase
	{
		public ConsoleCommandTS3(string label, string description) : base(label, description)
		{
			this.Usage = "[Users per Page] [Page]";
		}

		internal override CCR Execute(List<string> args, PersonDb db)
		{
			int page = 1;
			int perPage = 10;
			if (args.Count != 0)
			{
				if (!Int32.TryParse(args[0], out perPage))
				{
					return CCR.NOTANUMBER;
				}
				if (!Int32.TryParse((args.Count == 2 ? args[1] : "1"), out page))
				{
					return CCR.NOTANUMBER;
				}
			}

			StringBuilder outMessage = new StringBuilder();

			int endIndex = (page * perPage);
			int index = (endIndex - perPage);
			index = index <= 0 ? 0 : index;

			int playerCount = Program.CurrentClients.Count;
			int pageCount = (int)Math.Ceiling((double)playerCount / perPage);
			pageCount = pageCount == 0 ? 1 : pageCount;

			if (page > pageCount)
			{
				outMessage.Append($"There are only {pageCount} pages!");
			}
			else
			{
				outMessage.Append($"Total Players: {playerCount}\nPage ({page}/{pageCount})\n");

				List<GetClientsInfo> shit = Program.CurrentClients.ToList();
				outMessage.AppendFormat("{0, -15} | {1, -15} | {2, -28} | {3, -15}\n\n", "Name", "DdId", "uid", "SteamID64");

				foreach (GetClientsInfo player in shit)
				{
					outMessage.
						AppendFormat("{0, -15} | {1, -15} | {2, -28} | {3, -15}\n", player.NickName, player.DatabaseId, player.Id, player.Id);
				}
			}
			Console.WriteLine(outMessage);
			return CCR.OK;
		}
	}
}
