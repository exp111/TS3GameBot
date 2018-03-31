using System;
using System.Collections.Generic;
using System.Text;

namespace TS3GameBot.Utils.Settings
{
	class Creds
	{
		public Dictionary<String, TS3QueryInfo> TS3InfoList { get; set; } = new Dictionary<string, TS3QueryInfo>();

		public String DBName { get; set; }

		public String DBUsername { get; set; }

		public String DBLoginpass { get; set; }

		public Creds()
		{
		}

		public Creds(Dictionary<String, String> creds)
		{
			try
			{
				DBName = creds["DBName"];
				DBUsername = creds["DBUser"];
				DBLoginpass = creds["DBPass"];

				TS3InfoList.
					Add(creds["TS3CustomName"], new TS3QueryInfo() { ServerAddress = creds["TS3Server"], TS3LoginName = creds["TS3User"], TS3LoginPass = creds["TS3Pass"] });
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
