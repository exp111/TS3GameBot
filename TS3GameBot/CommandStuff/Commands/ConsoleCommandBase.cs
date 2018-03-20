using System;
using System.Collections.Generic;
using System.Text;

namespace TS3GameBot.CommandStuff.Commands
{
	public abstract class ConsoleCommandBase
    {
		public String Label { get; set; }

		public String Description { get; set; } = "No Description";

		public String Usage { get; set; } = "";

		public ConsoleCommandBase(String label, String description)
		{
			Label = label;
			this.Description = description;
		}

		public String GetUsage()
		{
			return "\n[Usage]\n " + Label + " " + this.Usage;
		}

		public abstract bool Execute(List<String> args);
	}
}
