using System;
using System.Collections.Generic;
using System.Text;

namespace TS3GameBot.DBStuff
{
    class CasinoPlayer
    {
		public String Id { get; set; }

		public String Name { get; set; }

		public int Points { get; set; }

		public String SteamID64 { get; set; }

		public DateTime LastDaily { get; set; }
	}
}
