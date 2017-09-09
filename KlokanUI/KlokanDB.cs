using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using SQLite.CodeFirst;

namespace KlokanUI
{
	class AnswerSheet
	{
		public int AnswerSheetId { get; set; }
		public string Data { get; set; }
	}

	class KlokanDBContext : DbContext
	{
		public KlokanDBContext() : base("KlokanDBContext")
		{

		}

		public DbSet<AnswerSheet> AnswerSheets { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<KlokanDBContext>(modelBuilder);
			Database.SetInitializer(sqliteConnectionInitializer);
		}
	}
}
