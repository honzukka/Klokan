using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace KlokanUI
{
	static class HelperFunctions
	{
		/// <summary>
		/// Handles clicks for table images, stores chosen answers in a data structure 
		/// and ensures only one answer is selected for each question.
		/// </summary>
		/// <param name="mouseEvent">The click event.</param>
		/// <param name="pictureBox">A reference to the picture box that was clicked.</param>
		/// <param name="tableIndex">The index of the table that was clicked.</param>
		/// <param name="answers">A three-dimensional array of answers (yes/no) that are displayed in the table image.</param>
		public static void HandleTableImageClicks(MouseEventArgs mouseEvent, PictureBox pictureBox, int tableIndex, bool[,,] answers)
		{
			// the tables have one extra column and one extra row that doesn't contain answers but annotations
			int tableRows = answers.GetUpperBound(1) + 2;
			int tableColumns = answers.GetUpperBound(2) + 2;

			int cellHeight = pictureBox.Height / tableRows;
			int cellWidth = pictureBox.Width / tableColumns;
			int rowClicked = mouseEvent.Y / cellHeight;
			int columnClicked = mouseEvent.X / cellWidth;

			// if the area designated for answers was clicked
			if (rowClicked != 0 && columnClicked != 0)
			{
				Image tableImage = pictureBox.Image;

				using (var graphics = Graphics.FromImage(tableImage))
				using (var blackPen = new Pen(Color.Black, 2))
				{
					// if this answer has already been selected, unselect it
					if (answers[tableIndex, rowClicked - 1, columnClicked - 1] == true)
					{
						answers[tableIndex, rowClicked - 1, columnClicked - 1] = false;
						HelperFunctions.RemoveCellContent(rowClicked, columnClicked, cellWidth, cellHeight, graphics);
					}
					// else unselect all other answers in this row and select this one instead
					else
					{
						// remove any crosses that are already in the row
						for (int i = 0; i < tableColumns - 1; i++)
						{
							if (answers[tableIndex, rowClicked - 1, i] == true)
							{
								answers[tableIndex, rowClicked - 1, i] = false;
								HelperFunctions.RemoveCellContent(rowClicked, i + 1, cellWidth, cellHeight, graphics);
							}
						}

						answers[tableIndex, rowClicked - 1, columnClicked - 1] = true;
						HelperFunctions.DrawCross(rowClicked, columnClicked, cellWidth, cellHeight, graphics, blackPen);
					}
				}

				pictureBox.Refresh();
			}
		}

		/// <summary>
		/// Represents a function which draws a shape into a table cell.
		/// </summary>
		/// <param name="row">Table row of the cell. (Corresponds to question number, the first row cannot be drawn into.)</param>
		/// <param name="column">Table column of the cell. (Corresponds to answer option number, the first column cannot be drawn into.)</param>
		/// <param name="cellWidth">The width of the cell.</param>
		/// <param name="cellHeight">The height of the cell.</param>
		/// <param name="graphics">Graphics representing the table image to draw into.</param>
		/// <param name="pen">Pen to be used for drawing.</param>
		public delegate void DrawSomething(int row, int column, int cellWidth, int cellHeight, Graphics graphics, Pen pen);

		/// <summary>
		/// Draws the contents of the answers array into table images of all the picture boxes.
		/// </summary>
		/// <param name="table1PictureBox">Picture box contaning the first table image.</param>
		/// <param name="table2PictureBox">Picture box contaning the second table image.</param>
		/// <param name="table3PictureBox">Picture box contaning the third table image.</param>
		/// <param name="answers">A multi-dimensional array of yes/no answers.</param>
		/// <param name="drawSomething">A delegate which will draw into the cells.</param>
		/// <param name="penColor">The color of the pen to be used for drawing.</param>
		public static void DrawAnswers(PictureBox table1PictureBox, PictureBox table2PictureBox, PictureBox table3PictureBox, bool[,,] answers, DrawSomething drawSomething, Color penColor)
		{
			PictureBox[] pictureBoxes = new PictureBox[] { table1PictureBox, table2PictureBox, table3PictureBox };

			for (int i = 0; i < pictureBoxes.Length; i++)
			{
				int cellHeight = pictureBoxes[i].Height / 9;
				int cellWidth = pictureBoxes[i].Width / 6;

				Image tableImage = pictureBoxes[i].Image;

				using (var graphics = Graphics.FromImage(tableImage))
				using (var blackPen = new Pen(penColor, 2))
				{
					for (int row = 0; row < 8; row++)
					{
						for (int col = 0; col < 5; col++)
						{
							if (answers[i, row, col] == true)
							{
								drawSomething(row + 1, col + 1, cellWidth, cellHeight, graphics, blackPen);
							}
						}
					}
				}

				pictureBoxes[i].Refresh();
			}
		}

		/// <summary>
		/// Draws a cross into a cell.
		/// </summary>
		/// <param name="row">Table row of the cell. (Corresponds to question number, the first row cannot be drawn into.)</param>
		/// <param name="column">Table column of the cell. (Corresponds to answer option number, the first column cannot be drawn into.)</param>
		/// <param name="cellWidth">The width of the cell.</param>
		/// <param name="cellHeight">The height of the cell.</param>
		/// <param name="graphics">Graphics representing the table image to draw into.</param>
		/// <param name="pen">Pen to be used for drawing.</param>
		public static void DrawCross(int row, int column, int cellWidth, int cellHeight, Graphics graphics, Pen pen)
		{
			int crossOffset = 5;

			Point upperLeft = new Point(column * cellWidth, row * cellHeight);
			Point upperRight = new Point(upperLeft.X + cellWidth, upperLeft.Y);
			Point lowerLeft = new Point(upperLeft.X, upperLeft.Y + cellHeight);
			Point lowerRight = new Point(upperLeft.X + cellWidth, upperLeft.Y + cellHeight);

			upperLeft.X += crossOffset;		upperLeft.Y += crossOffset;
			upperRight.X -= crossOffset;	upperRight.Y += crossOffset;
			lowerLeft.X += crossOffset;		lowerLeft.Y -= crossOffset;
			lowerRight.X -= crossOffset;	lowerRight.Y -= crossOffset;

			graphics.DrawLine(pen, upperLeft, lowerRight);
			graphics.DrawLine(pen, upperRight, lowerLeft);
		}

		/// <summary>
		/// Draws a circle (ellipse) into a cell.
		/// </summary>
		/// <param name="row">Table row of the cell. (Corresponds to question number, the first row cannot be drawn into.)</param>
		/// <param name="column">Table column of the cell. (Corresponds to answer option number, the first column cannot be drawn into.)</param>
		/// <param name="cellWidth">The width of the cell.</param>
		/// <param name="cellHeight">The height of the cell.</param>
		/// <param name="graphics">Graphics representing the table image to draw into.</param>
		/// <param name="pen">Pen to be used for drawing.</param>
		public static void DrawCircle(int row, int column, int cellWidth, int cellHeight, Graphics graphics, Pen pen)
		{
			float originX = column * cellWidth;
			float originY = row * cellHeight;

			graphics.DrawEllipse(pen, originX, originY, cellWidth, cellHeight);
		}

		/// <summary>
		/// Fills the cell area with white.
		/// </summary>
		/// <param name="row">Table row of the cell. (Corresponds to question number, the first row cannot be drawn into.)</param>
		/// <param name="column">Table column of the cell. (Corresponds to answer option number, the first column cannot be drawn into.)</param>
		/// <param name="cellWidth">The width of the cell.</param>
		/// <param name="cellHeight">The height of the cell.</param>
		/// <param name="graphics">Graphics representing the table image to draw into.</param>
		/// <param name="pen">Pen to be used for drawing.</param>
		public static void RemoveCellContent(int row, int column, int cellWidth, int cellHeight, Graphics graphics)
		{
			int contentOffset = 4;

			Rectangle cell = new Rectangle(column * cellWidth, row * cellHeight, cellWidth, cellHeight);

			cell.X += contentOffset;			cell.Y += contentOffset;
			cell.Width -= (contentOffset * 2);	cell.Height -= (contentOffset * 2);

			graphics.FillRectangle(Brushes.White, cell);
		}

		/// <summary>
		/// Transforms an image into an array of bytes in a specified format.
		/// </summary>
		/// <param name="imageFilename">Path to the image that will be transformed.</param>
		/// <param name="imageFormat">The format in which the image will be stored into the array.</param>
		public static byte[] GetImageBytes(string imageFilename, ImageFormat imageFormat)
		{
			using (var memoryStream = new MemoryStream())
			using (var sheetImage = Image.FromFile(imageFilename))
			{
				sheetImage.Save(memoryStream, imageFormat);
				return memoryStream.ToArray();
			}
		}

		/// <summary>
		/// Checks whether an answer is selected in each row.
		/// (there can't be two or more answers selected thanks to the implementation of HandlePictureBoxClicks() )
		/// </summary>
		/// <param name="answers">A three-dimensional array containing the answers. (=Array of tables)</param>
		/// <param name="tableIndex">The index of the table to check.</param>
		/// <returns></returns>
		public static bool CheckAnswers(bool[,,] answers, int tableIndex)
		{
			int tableRows = answers.GetUpperBound(1) + 1;
			int tableColumns = answers.GetUpperBound(2) + 1;

			for (int row = 0; row < tableRows; row++)
			{
				bool rowContainsAnswer = false;

				for (int col = 0; col < tableColumns; col++)
				{
					if (answers[tableIndex, row, col] == true)
					{
						rowContainsAnswer = true;
						break;
					}
				}

				if (!rowContainsAnswer)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// This function converts answers from a table (answer table or student table) into DbSets that inherit from KlokanDBAnswer.
		/// Uses reflection and therefore is not suitable for automated batch processing.
		/// </summary>
		/// <typeparam name="T">The specific DbSet type that inherits from KlokanDBAnswer.</typeparam>
		/// <param name="answers">A three-dimensional table of answers.</param>
		/// <param name="tableIndex">The index of a table which is to be converted.</param>
		/// <param name="isStudentTable">A flag that tells whether the table is a student table and therefore should be treated slightly differently.</param>
		public static List<T> AnswersToDbSet<T>(bool[,,] answers, int tableIndex, bool isStudentTable) where T : KlokanDBAnswer
		{
			int tableRows = answers.GetUpperBound(1) + 1;
			int tableColumns = answers.GetUpperBound(2) + 1;

			List<T> chosenAnswersDB = new List<T>();

			for (int row = 0; row < tableRows; row++)
			{
				char enteredValue = '\0';

				// find out the entered value (entered value can stay '\0' in case the question wasn't answered)
				for (int col = 0; col < tableColumns; col++)
				{
					int numberOfSelectedAnswers = 0;

					if (answers[tableIndex, row, col] == true)
					{
						enteredValue = (char)('a' + col);
						numberOfSelectedAnswers++;
					}

					// keep in mind that more answers can be selected
					if (numberOfSelectedAnswers > 1)
					{
						enteredValue = 'x';
					}
				}

				// reflection is fine to use here because this function is never called in automated batch processing
				// it is always called in forms which allow the user to edit 1 sheet
				var answerInstance = Activator.CreateInstance(typeof(T)) as T;

				if (isStudentTable)
				{
					// student table answers are assigned to questions with negative numbers
					answerInstance.QuestionNumber = row - 5;
				}
				else
				{
					answerInstance.QuestionNumber = (row + 1) + (tableIndex * 8);
				}

				answerInstance.Value = new String(enteredValue, 1);

				chosenAnswersDB.Add(answerInstance);
			}

			return chosenAnswersDB;
		}
	}
}
