using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using System.Data.SQLite;
using SQLite.CodeFirst;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlokanUI
{
	public class KlokanTestDBScan
	{
		public KlokanTestDBScan()
		{
			ExpectedValues = new List<KlokanTestDBExpectedAnswer>();
			ComputedValues = new List<KlokanTestDBComputedAnswer>();
		}

		[Key]
		public int ScanId { get; set; }
		public byte[] Image { get; set; }
		public float Correctness { get; set; }

		public virtual ICollection<KlokanTestDBExpectedAnswer> ExpectedValues { get; set; }
		public virtual ICollection<KlokanTestDBComputedAnswer> ComputedValues { get; set; }
	}

	public class KlokanTestDBExpectedAnswer : KlokanDBAnswer
	{
		[Required]
		public int ScanId { get; set; }

		[ForeignKey("ScanId"), Required]
		public virtual KlokanTestDBScan Scan { get; set; }
	}

	public class KlokanTestDBComputedAnswer : KlokanDBAnswer
	{
		[Required]
		public int ScanId { get; set; }

		[ForeignKey("ScanId"), Required]
		public virtual KlokanTestDBScan Scan { get; set; }
	}

	public class KlokanTestDBContext : DbContext
	{
		public KlokanTestDBContext() : base("KlokanTestDBContext") { }

		public KlokanTestDBContext(SQLiteConnection connection) : base(connection, true) { }

		public DbSet<KlokanTestDBScan> Scans { get; set; }
		public DbSet<KlokanTestDBExpectedAnswer> ExpectedValues { get; set; }
		public DbSet<KlokanTestDBComputedAnswer> ComputedValues { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			var sqliteConnectionInitializer = new SqliteDropCreateDatabaseAlways<KlokanTestDBContext>(modelBuilder);
			Database.SetInitializer(sqliteConnectionInitializer);
		}
	}
}
