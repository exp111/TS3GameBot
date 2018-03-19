using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TS3GameBot.Utils
{
    class Utils
    {
		public static String MsToM(double miliseconds)
		{
			StringBuilder message = new StringBuilder();

			int years = (int)(miliseconds / 1000 / 60 / 60 / 24 / 365);
			int days = (int)(miliseconds / 1000 / 60 / 60 / 24 % 365);
			int hours = (int)(miliseconds / 1000 / 60 / 60 % 24);
			int minutes = (int)(miliseconds / 1000 / 60 % 60);
			int seconds = (int)(miliseconds / 1000 % 60);

			message.Append(years > 0 ? years + "a " : "");
			message.Append(days > 0 ? days + "d " : "");
			message.Append(hours > 0 ? hours + "h " : "");
			message.Append(minutes > 0 ? minutes + "m " : "");
			message.Append(seconds + "s");

			return message.ToString();
		}

		public static StringBuilder ApplyColor(Color clr)
		{
			StringBuilder msg = new StringBuilder();

			msg.Append("[COLOR=#").
				Append(clr.ToHex()).
				Append("]");

			return msg;
		}

	}
}
