using System;
using System.Collections.Generic;
using System.Text;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.Commands
{
	class CommandRegister : CommandBase
	{
		public CommandRegister(string label, string description) : base(label, description)
		{
			this.Usage = "[ SteamID64 ]";
			this.NeedsRegister = false;
		}

		internal override bool Execute(List<string> args, TextMessage message, PersonDb db)
		{
			StringBuilder outMessage = new StringBuilder();
			Error result = DbInterface.AddPlayer(message.InvokerUid, message.InvokerName, db, args.Count > 0 ? args[0] : "");

			outMessage.Append("\n");

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
					outMessage.Append("An Unknown Error Occured");
					break;
			}
			CommandManager.AnswerCall(message, outMessage.ToString());

			return false;
		}
	}
}
