using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;

namespace KlokanUI
{
	static class AnswerDrawing
	{
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
