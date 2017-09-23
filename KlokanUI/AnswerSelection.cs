using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;

namespace KlokanUI
{
	class AnswerSelection
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
						AnswerDrawing.RemoveCellContent(rowClicked, columnClicked, cellWidth, cellHeight, graphics);
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
								AnswerDrawing.RemoveCellContent(rowClicked, i + 1, cellWidth, cellHeight, graphics);
							}
						}

						answers[tableIndex, rowClicked - 1, columnClicked - 1] = true;
						AnswerDrawing.DrawCross(rowClicked, columnClicked, cellWidth, cellHeight, graphics, blackPen);
					}
				}

				pictureBox.Refresh();
			}
		}
	}
}
