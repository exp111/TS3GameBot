using System;
using System.Collections.Generic;
using System.Text;
using TS3GameBot.CommandStuff.Commands;

namespace TS3GameBot.CommandStuff.ConsoleCommands
{
	class ConsoleCommandClear : ConsoleCommandBase
	{
		public ConsoleCommandClear(string label, string description) : base(label, description)
		{
		}

		public override CCR Execute(List<string> args)
		{
			Console.Clear();
			return CCR.OK;
		}
	}
}
