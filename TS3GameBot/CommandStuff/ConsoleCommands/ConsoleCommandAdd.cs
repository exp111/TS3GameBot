using System;
using System.Collections.Generic;
using System.Text;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.Commands
{
	class ConsoleCommandAdd : ConsoleCommandBase
	{
		public ConsoleCommandAdd(string label, string description) : base(label, description)
		{
			this.Usage = "<uid> <name> [steamid] [points]";
		}
		internal override CCR Execute(List<string> args, PersonDb db)
		{
			if (args.Count < 2)
			{
				return CCR.INVALIDPARAM;
			}

			int points = 0;
			if (args.Count > 3)
			{
				if (!Int32.TryParse(args[3], out points))
				{
					return CCR.NOTANUMBER;
				}
			}

			//Most checks are in addplayer function
			Error result = DbInterface.AddPlayer(args[0], args[1], db, args.Count > 2 ? args[2] : "", points);

			StringBuilder outMessage = new StringBuilder();
			switch (result)
			{
				case Error.OK:
					outMessage.Append("Player added successfully!");
					break;
				case Error.DUPLICATE:
					outMessage.Append("Player already exists in the Database");
					break;
				case Error.INVALIDNAME:
					outMessage.Append("Name already exists within the Database");
					break;
				case Error.SAVEERROR:
					outMessage.Append("Player could not be saved to the Database!");
					break;
				case Error.UNKNOWN:
				default:
					outMessage.Append("An Unknown Error Occured!");
					break;
			}
			Console.Write(outMessage.Append("\n").ToString());

			return CCR.OK;
		}
	}
}
