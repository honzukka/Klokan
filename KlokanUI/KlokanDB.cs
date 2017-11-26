using System.Collections.Generic;

using System.Data.Entity;
using System.Data.SQLite;
using SQLite.CodeFirst;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlokanUI
{
	public class KlokanDBInstance
	{
		public KlokanDBInstance()
		{
			CorrectAnswers = new List<KlokanDBCorrectAnswer>();
			AnswerSheets = new List<KlokanDBAnswerSheet>();
		}

		[Key]
		public int InstanceId { get; set; }
		public int Year { get; set; }

		[MaxLength(10)]
		public string Category { get; set; }

		public virtual ICollection<KlokanDBCorrectAnswer> CorrectAnswers { get; set; }
		public virtual ICollection<KlokanDBAnswerSheet> AnswerSheets { get; set; }
	}

	public class KlokanDBAnswerSheet
	{
		public KlokanDBAnswerSheet()
		{
			ChosenAnswers = new List<KlokanDBChosenAnswer>();
		}

		[Key]
		public int AnswerSheetId { get; set; }
		public int StudentNumber { get; set; }
		public int Points { get; set; }
		public byte[] Scan { get; set; }

		[Required]
		public int InstanceId { get; set; }

		[ForeignKey("InstanceId"), Required]
		public virtual KlokanDBInstance Instance { get; set; }

		public virtual ICollection<KlokanDBChosenAnswer> ChosenAnswers { get; set; }
	}

	public abstract class KlokanDBAnswer
	{
		[Key]
		public int AnswerId { get; set; }
		public int QuestionNumber { get; set; }
		public string Value { get; set; }
	}

	public class KlokanDBCorrectAnswer : KlokanDBAnswer
	{
		[Required]
		public int InstanceId { get; set; }

		[ForeignKey("InstanceId"), Required]
		public virtual KlokanDBInstance Instance { get; set; }
	}

	public class KlokanDBChosenAnswer : KlokanDBAnswer
	{
		[Required]
		public int AnswerSheetId { get; set; }

		[ForeignKey("AnswerSheetId"), Required]
		public virtual KlokanDBAnswerSheet AnswerSheet { get; set; }
	}

	public class KlokanDBContext : DbContext
	{
		public KlokanDBContext() : base("KlokanDBContext") { }

		public KlokanDBContext(SQLiteConnection connection) : base(connection, true) { }

		public DbSet<KlokanDBInstance> Instances { get; set; }
		public DbSet<KlokanDBAnswerSheet> AnswerSheets { get; set; }
		public DbSet<KlokanDBCorrectAnswer> CorrectAnswers { get; set; }
		public DbSet<KlokanDBChosenAnswer> ChosenAnswers { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<KlokanDBContext>(modelBuilder);
			Database.SetInitializer(sqliteConnectionInitializer);
		}
	}
}
