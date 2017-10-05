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
	public class Instance
	{
		public Instance()
		{
			CorrectAnswers = new List<CorrectAnswer>();
			AnswerSheets = new List<AnswerSheet>();
		}

		[Key]
		public int InstanceId { get; set; }
		public int Year { get; set; }

		[MaxLength(10)]
		public string Category { get; set; }

		public virtual ICollection<CorrectAnswer> CorrectAnswers { get; set; }
		public virtual ICollection<AnswerSheet> AnswerSheets { get; set; }
	}

	public class AnswerSheet
	{
		public AnswerSheet()
		{
			ChosenAnswers = new List<ChosenAnswer>();
		}

		[Key]
		public int AnswerSheetId { get; set; }
		// TODO: implement this
		public int StudentNumber { get; set; }
		public int Points { get; set; }
		public byte[] Scan { get; set; }

		[Required]
		public int InstanceId { get; set; }

		[ForeignKey("InstanceId"), Required]
		public virtual Instance Instance { get; set; }

		public virtual ICollection<ChosenAnswer> ChosenAnswers { get; set; }
	}

	public abstract class Answer
	{
		[Key]
		public int AnswerId { get; set; }
		public int QuestionNumber { get; set; }
		public string Value { get; set; }
	}

	public class CorrectAnswer : Answer
	{
		[Required]
		public int InstanceId { get; set; }

		[ForeignKey("InstanceId"), Required]
		public virtual Instance Instance { get; set; }
	}

	public class ChosenAnswer : Answer
	{
		[Required]
		public int AnswerSheetId { get; set; }

		[ForeignKey("AnswerSheetId"), Required]
		public virtual AnswerSheet AnswerSheet { get; set; }
	}

	public class KlokanDBContext : DbContext
	{
		public KlokanDBContext() : base("KlokanDBContext") { }

		public KlokanDBContext(SQLiteConnection connection) : base(connection, true) { }

		public DbSet<Instance> Instances { get; set; }
		public DbSet<AnswerSheet> AnswerSheets { get; set; }
		public DbSet<CorrectAnswer> CorrectAnswers { get; set; }
		public DbSet<ChosenAnswer> ChosenAnswers { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<KlokanDBContext>(modelBuilder);
			Database.SetInitializer(sqliteConnectionInitializer);
		}
	}
}
