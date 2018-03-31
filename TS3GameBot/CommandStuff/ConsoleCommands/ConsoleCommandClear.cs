using System;
using System.Collections.Generic;
using System.Text;
using TS3GameBot.CommandStuff.Commands;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.ConsoleCommands
{
	class ConsoleCommandClear : ConsoleCommandBase
	{
		public ConsoleCommandClear(string label, string description) : base(label, description)
		{
		}

		internal override CCR Execute(List<string> args, PersonDb db)
		{
			Console.Clear();
			return CCR.OK;
		}
	}
}
