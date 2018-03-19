using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS3GameBot.DBStuff
{
	enum Error
	{
		UNKNOWN = -1,
		OK = 0,
		DUPLICATE = 1,
		SAVEERROR = 2,
		READERROR = 3
	}

    class DbInterface
    {
		public DbInterface Instance { get; } = new DbInterface();

		private DbInterface() { }

		public static Error UpdateDaily(String uid, int amount)
		{
			PersonDb db = PersonDb.Instance;
			db.Players.Find(uid).Points += amount;
			db.Players.Find(uid).LastDaily = DateTime.Now;

			if (db.SaveChanges() < 1)
			{
				return Error.SAVEERROR;
			}

			return Error.OK;
		}

		public static Error AddPlayer(String uid, String name, String steamID = "", int points = 0)
		{
			PersonDb db = PersonDb.Instance;

			CasinoPlayer tempPlayer = new CasinoPlayer { Id = uid, Name = name, Points = points, SteamID64 = steamID };

			if (IsAlive(tempPlayer).GetAwaiter().GetResult())
			{
				return Error.DUPLICATE;
			}

			db.Players.Add(tempPlayer);
			int savedCount = db.SaveChanges();
			if(savedCount < 1)
			{
				return Error.SAVEERROR;
			}

			Console.WriteLine("{0} records saved to database", savedCount);

			return Error.OK;
		}

		public static CasinoPlayer GetPlayer(String uid)
		{
			PersonDb db = PersonDb.Instance;
			return db.Players.Find(uid);
		}

		public static List<CasinoPlayer> GetPlayerList(String name = "N/A")
		{
			List<CasinoPlayer> playerList = new List<CasinoPlayer>();
			if (name == "N/A")
			{
				PersonDb db = PersonDb.Instance;

				foreach (CasinoPlayer player in db.Players)
				{
					playerList.Add(player);
				}

				return playerList;
			}
			else
			{
				PersonDb db = PersonDb.Instance;

				return db.Players.Where(p => p.Name.Contains(name)).ToList();
			}
		}

		public static async Task<bool> IsAlive(CasinoPlayer myPlayer)
		{
			PersonDb db = PersonDb.Instance;

			return (await db.Players.FindAsync(myPlayer.Id)) != null;
		}

		public static async Task<bool> IsAlive(String uid)
		{
			PersonDb db = PersonDb.Instance;

			return (await db.Players.FindAsync(uid)) != null;
		}

		public static Error SaveChanges()
		{
			PersonDb db = PersonDb.Instance;
			if (db.SaveChanges() < 1)
			{
				return Error.SAVEERROR;
			}
			return Error.OK;
		}

		public static bool AlterPoints(CasinoPlayer player, int amount)
		{
			PersonDb db = PersonDb.Instance;
			db.Players.Find(player.Id).Points += amount;

			return true;

		}
	}
}
