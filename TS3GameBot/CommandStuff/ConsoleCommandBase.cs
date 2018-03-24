using System;
using System.Collections.Generic;
using System.Text;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff.Commands
{
	/// <summary>
	///		"ConsoleCommandResult"
	///		Has Result codes for ConsoleCommands
	/// </summary>
	public enum CCR 
	{
		UNKNOWN = -1,
		OK = 0,
		WRONGPARAM = 1,
		INVALIDPARAM = 2,
		PLAYERNOTFOUND = 3,
		NOTANUMBER = 4,
		NOTENOUGHPOINTS = 5,
		DBWRITEFAILED = 6,
		BELOWZERO = 7,
		HANDLED = 8
	}

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
			return "[Usage]\n " + Label + " " + this.Usage;
		}

		internal abstract CCR Execute(List<String> args, PersonDb db);
	}
}
