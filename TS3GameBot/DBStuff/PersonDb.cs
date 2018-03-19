using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TS3GameBot.DBStuff
{
    class PersonDb : DbContext
    {
		public static PersonDb Instance { get; } = new PersonDb();
		public DbSet<CasinoPlayer> Players { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseMySql(@"Server=localhost;Database=csharp;User=csharp;Password=csharp");
		}

		private PersonDb() { }
	}
}
