using System;
using System.Collections.Generic;
using System.Text;
using TS3GameBot.CommandStuff.Commands;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.ConsoleCommands
{
    class ConsoleCommandChange : ConsoleCommandBase
    {
		public ConsoleCommandChange(string label, string description) : base(label, description)
		{
			this.Usage = "<uid> <name> [steamid]";
		}
		internal override CCR Execute(List<string> args, PersonDb db)
		{
			if (args.Count < 2)
			{
				return CCR.INVALIDPARAM;
			}

			Error result = DbInterface.ChangePlayer(args[0], db, args[1], args.Count > 2 ? args[2] : "");

			StringBuilder outMessage = new StringBuilder();
			switch (result)
			{
				case Error.OK:
					outMessage.Append("Player changed successfully!");
					break;
				case Error.SAVEERROR:
					outMessage.Append("Player could not be saved to the Database!");
					break;
				case Error.NOTFOUND:
					outMessage.Append("Could not find a player with the given UID!");
					break;
				case Error.UNKNOWN:
				default:
					outMessage.Append("An Unknown Error Occured");
					break;
			}
			Console.Write(outMessage);

			return CCR.OK;
		}
	}
}
