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
		}

		public override bool Execute(List<string> args, TextMessage message)
		{
			StringBuilder outMessage = new StringBuilder();
			Error result = DbInterface.AddPlayer(message.InvokerUid, message.InvokerName, args.Count > 0 ? args[0] : "");

			outMessage.Append("\n");

			switch (result)
			{
				case Error.UNKNOWN:
					outMessage.Append("An Unknown Error Occured");
					break;
				case Error.OK:
					outMessage.Append("Player added Successfullllllyyyie!");
					break;
				case Error.DUPLICATE:
					outMessage.Append("Player already exists in the Database");
					break;
				case Error.SAVEERROR:
					outMessage.Append("Player could not be saved to the Database!");
					break;
				default:
					outMessage.Append("No glue");
					break;
			}
			CommandManager.AnswerCall(message, outMessage.ToString());
			Console.WriteLine(outMessage);

			return false;
		}
	}
}
