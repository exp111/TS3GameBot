using System;
using System.Collections.Generic;
using System.Text;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TS3GameBot.CommandStuff.Commands;

namespace TS3GameBot.CommandStuff.ConsoleCommands
{
	class ConsoleCommandStop : ConsoleCommandBase
	{
		public ConsoleCommandStop(string label, string description) : base(label, description)
		{
			this.Usage = "";
		}

		public override CCR Execute(List<string> args)
		{
			Console.WriteLine("Stopping Bot");
			Program.Running = false;
			return CCR.OK;
		}
	}
}
