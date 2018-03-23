using System;
using System.Collections.Generic;
using System.Text;

namespace TS3GameBot.Utils
{
	[Serializable]
    class Creds
    {	

		public String DBName { get; set; }
		public String DBUser { get; set; }

		public String DBPass { get; set; }

		public String TS3User { get; set; }

		public String TS3Pass { get; set; }

		public Creds() { }

		public Creds(Dictionary<string, string> creds)
		{
			try
			{
				DBName = creds["DBName"];
				DBUser = creds["DBUser"];
				DBPass = creds["DBPass"];

				TS3User = creds["TS3User"];
				TS3Pass = creds["TS3Pass"];
			}
			catch (Exception)
			{
				throw;
			}
			
		}

		public override string ToString()
		{
			StringBuilder msg = new StringBuilder();

			msg.
				Append("DBUser: " + DBUser).
				Append("\nDBPass: " + DBPass).
				Append("\nTS3User: " + TS3User).
				Append("\nTS3User: " + TS3Pass);

			return msg.ToString();
		}
	}
}
