using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TS3GameBot.Utils
{
    public static class ColorExtension
    {
		public static String ToHex(this Color clr)
		{
			StringBuilder msg = new StringBuilder();
			msg.
				AppendFormat(clr.R == 0 ? "0{0:X}" : "{0:X}", clr.R).
				AppendFormat(clr.G == 0 ? "0{0:X}" : "{0:X}", clr.G).
				AppendFormat(clr.B == 0 ? "0{0:X}" : "{0:X}", clr.B);

			return msg.ToString();
		}
    }
}
