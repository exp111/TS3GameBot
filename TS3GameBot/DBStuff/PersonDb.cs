﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TS3GameBot.DBStuff
{
    class PersonDb : DbContext
    {
		public DbSet<CasinoPlayer> Players { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseMySql(@"Server=localhost;Database=" + Program.MyCreds.DBName + ";User=" + Program.MyCreds.DBUsername + ";Password=" + Program.MyCreds.DBLoginpass);
		}

		public PersonDb()
		{
			Database.EnsureCreated();
		}
	}
}
