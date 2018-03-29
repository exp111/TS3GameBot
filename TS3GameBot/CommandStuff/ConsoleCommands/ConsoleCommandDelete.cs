using System;
using System.Collections.Generic;
using System.Text;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.Commands
{
	class ConsoleCommandDelete : ConsoleCommandBase
	{
		public ConsoleCommandDelete(string label, string description) : base(label, description)
		{
			this.Usage = "<uid>";
		}
		internal override CCR Execute(List<string> args, PersonDb db)
		{
			if (args.Count != 1)
			{
				return CCR.INVALIDPARAM;
			}

			Error result = DbInterface.DeletePlayer(args[0], db);

			StringBuilder outMessage = new StringBuilder();
			switch (result)
			{
				case Error.OK:
					outMessage.Append("Player deleted successfully!");
					break;
				case Error.SAVEERROR:
					outMessage.Append("Player could not be saved to the Database!");
					break;
				case Error.NOTFOUND:
					outMessage.Append("Could not find a player with the given UID!");
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
