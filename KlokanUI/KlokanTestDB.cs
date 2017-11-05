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
			ExpectedValues = new List<KlokanTestDBExpectedValue>();
			ComputedValues = new List<KlokanTestDBComputedValue>();
		}

		[Key]
		public int ScanId { get; set; }
		public byte[] Image { get; set; }
		public float Correctness { get; set; }

		public virtual ICollection<KlokanTestDBExpectedValue> ExpectedValues { get; set; }
		public virtual ICollection<KlokanTestDBComputedValue> ComputedValues { get; set; }
	}

	public abstract class KlokanTestDBValue
	{
		[Key]
		public int ValueId { get; set; }
		public int QuestionIdentifier { get; set; }
		public string Value { get; set; }
	}

	public class KlokanTestDBExpectedValue
	{
		[Required]
		public int ScanId { get; set; }

		[ForeignKey("ScanId"), Required]
		public virtual KlokanTestDBScan Scan { get; set; }
	}

	public class KlokanTestDBComputedValue
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
		public DbSet<KlokanTestDBExpectedValue> ExpectedValues { get; set; }
		public DbSet<KlokanTestDBComputedValue> ComputedValues { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<KlokanTestDBContext>(modelBuilder);
			Database.SetInitializer(sqliteConnectionInitializer);
		}
	}
}
