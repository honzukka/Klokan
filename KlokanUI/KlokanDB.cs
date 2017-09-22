using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
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

	public class CorrectAnswer
	{
		[Key]
		public int CorrectAnswerId { get; set; }
		public int QuestionNumber { get; set; }

		[MaxLength(1)]
		public string Value { get; set; }

		public int InstanceId { get; set; }

		[ForeignKey("InstanceId")]
		public virtual Instance Instance { get; set; }
	}

	public class AnswerSheet
	{
		public AnswerSheet()
		{
			Answers = new List<Answer>();
		}

		[Key]
		public int AnswerSheetId { get; set; }
		// TODO: implement this
		public int StudentNumber { get; set; }
		public int Points { get; set; }
		public byte[] Scan { get; set; }

		public int InstanceId { get; set; }

		[ForeignKey("InstanceId")]
		public virtual Instance Instance { get; set; }

		public virtual ICollection<Answer> Answers { get; set; }
	}

	public class Answer
	{
		[Key]
		public int AnswerId { get; set; }
		public int QuestionNumber { get; set; }

		[MaxLength(1)]
		public string Value { get; set; }

		public int AnswerSheetId { get; set; }

		[ForeignKey("AnswerSheetId")]
		public virtual AnswerSheet AnswerSheet { get; set; }
	}

	public class KlokanDBContext : DbContext
	{
		public KlokanDBContext() : base("KlokanDBContext") { }

		public DbSet<Instance> Instances { get; set; }
		public DbSet<CorrectAnswer> CorrectAnswers { get; set; }
		public DbSet<AnswerSheet> AnswerSheets { get; set; }
		public DbSet<Answer> Answers { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			var sqliteConnectionInitializer = new SqliteDropCreateDatabaseAlways<KlokanDBContext>(modelBuilder);
			Database.SetInitializer(sqliteConnectionInitializer);
		}
	}
}
