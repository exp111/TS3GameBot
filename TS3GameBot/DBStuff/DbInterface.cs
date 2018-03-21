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
		READERROR = 3,
		INVALIDNAME = 4
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
			if(db.Players.Where(p => p.Name.ToLower() == tempPlayer.Name.ToLower() ) == null)
			{
				return Error.INVALIDNAME;
			}

			db.Players.Add(tempPlayer);
			int savedCount = db.SaveChanges();
			if(savedCount < 1)
			{
				return Error.SAVEERROR;
			}

			//Console.WriteLine("{0} records saved to database", savedCount);

			return Error.OK;
		}

		public static CasinoPlayer GetPlayer(String uid) 
		{
			PersonDb db = PersonDb.Instance;
			return db.Players.Find(uid);
			// TODO: Catch "MySql.Data.MySqlClient.MySqlException" and make it known in shit'n'stuff
		}

		public static List<CasinoPlayer> GetPlayerList(int index = 0, int endIndex = 0, String name = "N/A")
		{
			List<CasinoPlayer> playerList = new List<CasinoPlayer>();
			if (name == "N/A")
			{
				PersonDb db = PersonDb.Instance;

				List<CasinoPlayer> shittyList = db.Players.ToList().OrderByDescending(p => p.Points).ToList();

				endIndex = endIndex > db.Players.Count() - 1 ? db.Players.Count() : endIndex;

				for (int i = index; i < (endIndex >= 1 ? endIndex : db.Players.Count()); i++)
				{
					playerList.Add(shittyList[i]);
				}

				return playerList;
			}
			else
			{
				PersonDb db = PersonDb.Instance;

				return db.Players.Where(p => p.Name.Equals(name)).ToList();
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
			if (player.Points + amount < 0)
			{
				return false;
			}

			player.Points += amount;

			return true;
		}

		public static bool SetPoints(CasinoPlayer player, int amount)
		{
			if(amount < 0)
			{
				return false;
			}
			player.Points = amount;
			return true;
		}
	}
}
