using System;
using System.Collections.Generic;
using System.Text;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;
using TeamSpeak3QueryApi.Net.Specialized.Responses;
using TS3GameBot.DBStuff;

namespace TS3GameBot.CommandStuff
{
	public abstract class CommandBase
    {
		public String Label { get; set; }

		public String Description { get; set; } = "No Description";

		public String Usage { get; set; } = "";

		public bool WIP { get; set; } = true;

		public bool Enabled { get; set; } = true;

		public bool Hidden { get; set; } = false;

		public bool Restricted { get; set; } = false;

		public CommandBase(String label, String description)
		{
			Label = label;
			this.Description = description;
		}

		public String GetUsage()
		{
			return "\n[Usage]\n " + CommandManager.CmdIndicator + Label + " " + this.Usage;
		}

		//public abstract bool Execute(List<String> args, TextMessage message);
		internal abstract bool Execute(List<string> args, TextMessage msg, PersonDb db);
	}
}
