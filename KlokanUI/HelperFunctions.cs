using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;

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
		/// <param name="tableIndex">An index of the table represented by the picture box in the correctAnswers array.</param>
		/// <param name="answers">A multi-dimensional array of answers (yes/no) that are displayed in the table image.</param>
		public static void HandleTableImageClicks(MouseEventArgs mouseEvent, PictureBox pictureBox, int tableIndex, bool[,,] answers)
		{
			int cellHeight = pictureBox.Height / 9;
			int cellWidth = pictureBox.Width / 6;
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
						for (int i = 0; i < 5; i++)
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
			Point upperLeft = new Point(column * cellWidth, row * cellHeight);
			Point upperRight = new Point(upperLeft.X + cellWidth, upperLeft.Y);
			Point lowerLeft = new Point(upperLeft.X, upperLeft.Y + cellHeight);
			Point lowerRight = new Point(upperLeft.X + cellWidth, upperLeft.Y + cellHeight);

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
			Rectangle cell = new Rectangle((column * cellWidth) + 1, (row * cellHeight) + 1, cellWidth - 2, cellHeight - 2);
			graphics.FillRectangle(Brushes.White, cell);
		}
	}
}
