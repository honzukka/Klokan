# Klokan
Mathematical Kangaroo is an international competition for students of primary and secondary schools. Each student answers 24 questions by choosing one out of five possible answers. Answers sheets then have to be manually evaluated and the final score is assigned to each student. This process, however, is tedious and prone to human error.

Klokan is an automated evaluation tool which processes images of scanned answer sheets and saves the results into a database where they can be queried or exported into Excel. It can therefore help make the evaluation of answer sheets easier in the meantime before the whole competition is digitized.

## What format should the answer sheets be in?
The answers sheet form to be printed out and filled in by students is part of this project and can be found [here](forms/new_form2.docx). This form should then be scanned. The supported image file formats are:
  - JPEG
  - PNG
  
## How do I evaluate the answer sheets?
After choosing *Evaluation* in the main menu, you will be presented with the following screen:

<img src="https://user-images.githubusercontent.com/24512922/37081844-0844095e-21eb-11e8-818b-c56863d1e094.PNG" width="400">

Here is where you can set up a batch to be evaluated. Notice that all answer sheets in the batch need to have the same year. They can have various categories, though, and you can configure these by ticking one of the checkboxes. That, and also clicking *Edit* when the category is already configured, will take you to this screen:

![Evaluation Category Configuration](https://user-images.githubusercontent.com/24512922/37082316-8e96e7c8-21ec-11e8-8a75-438b7a1cb66e.PNG)

For each category, you can manually input the correct answers by clicking on the table images and then you can select all answer sheet images available for that category in your computer by clicking *Add*. *Save Category* will save your configuration and take you back to the previous screen.

Once at least one category is configured, you can start the evaluation by clicking *Evaluate*. Ticked category configurations will be added to a batch and evaluated in a single run.

The parameters can be set to *Default* for now. You can find out more about them in the [last section of this readme](#parameters-explained).

## Where can I find my results?
This is what the *Database* module is for. This is what it looks like when there are some results available:

<img src="https://user-images.githubusercontent.com/24512922/37083191-1b32784e-21ef-11e8-91ca-156eb5e90f46.PNG" width="700">

#### To view specific results:

The two dropdowns at the top can help you find the specific results you're interested in. Once you click *View Selection*, all available results corresponding to the chosen year and category will be displayed.

#### To export the selection:

Simply click *Export Selection* to generate a .csv file.

#### To examine the results further:

Clicking the description at the top of each column will sort the results according to that column. Clicking the first cell on each line will then select a specific result. If this result has the best score, for example, you might want to check if the answer sheet has been evaluated correctly before awarding its author. You can do that by clicking *View Detail*. Here is what that looks like:

![Database Item Detail](https://user-images.githubusercontent.com/24512922/37083623-565e3420-21f0-11e8-8c59-0d84f8f04905.PNG)

Crosses represent answers read by the computer and red circles represent correct answers for the given category. There is also the original image of the answer sheet available for reference.

#### To import results from a different computer:

The database consists of two files located in the folder with your Klokan executable. One is called *KlokanDB.sqlite* and the other one is *KlokanTestDB.sqlite*. The first one contains your results and the second one contains test items from the [Test module](#to-change-the-parameters-of-the-evaluation).

You can copy these files and transfer them to a Klokan folder on a different computer. When you lauch Klokan on that computer, you will be able to see your results.

Since the database files need to have names mentioned above, **do not forget** to make a backup of your current database files before you replace them with different ones!

## My results are inaccurate. How to improve them?
There are two ways of improving your results.

#### To manually correct the results:

Click *Edit Entry* on the *Database Detail* screen shown above to enter the edit mode and then click on the table images to select the actual answers you see in scan. Don't forget to apply changes, re-evaluate the answer sheet and update the database if you want your changes to be saved!

#### To change the parameters of the evaluation:

Go to the *Test* module:

<img src="https://user-images.githubusercontent.com/24512922/37083984-80eb1bda-21f1-11e8-8ff6-5516a67ce1a5.PNG" width="400">

Here, you can add test items, which are simply images of answer sheets along with manually entered answers which are present in the sheet. These will serve as a reference. Once you add your test items, you can click *Evaluate*. This will read answers in the images and compare them with your reference. The correctness will then be displayed. 100% means that all read answers correspond to your manually edited ones and therefore the program works perfectly well. In case the percentage isn't as high as you would want it to be, you can click *Edit Parameters* and once you input new values, you can try the evaluation again.

<img src="https://user-images.githubusercontent.com/24512922/37084255-486f9064-21f2-11e8-948f-d0d479757906.PNG" width="400">

#### Parameters explained:

- **Black and White Threshold** - This directly tells you, how bright a shade of grey can be (from 0 - pure black to 255 - pure white) to still be considered as black. In case your students write their answers very lightly with a pencil, setting this value higher might help improve the correctness. On the other hand, it might also make the program too sensitive to certain artifacts in the image.
- **Table Line Eccentricity Limit** - Setting this value higher might help read values from an answer sheet image which isn't upright but slightly rotated. Please note that the value cannot be set too high because then the notion of a horizontal and vertical lines would mix up.
- **Table Line Curvature Limit** - Setting this value higher might help for answer sheets where the table lines are straight. However, setting it too high will also easily introduce artifacts.
- **Resized Cell Width & Height** - These parameters tell you how big table cells will be (in pixels) when the image is resized to be 1700 pixels wide. This is important for extracting tables from the image but should be left default when using the provided [form](forms/new_form2.docx).
- **Default Cell Width & Height** - These are the dimensions (in pixels) to which the extracted cells will be resized. Setting it higher might help improve the accuracy of the algorithm but will also make it run longer.
- **Cell Evaluation Type** - There are two types of cell evaluation: **Shape Recognition** and **Pixel Ratio**. **Shape Recognition** corresponds to *True* and **Pixel Ratio** to *False*.
- **Shape Recognition** - This method recognizes lines that could form a cross in a table cell and based on that either says, that the cross is there (answers was chosen) or not. The length is tied to the **Default Cell Width & Height** and says how long a line should at least be (in pixels) to be recognized as a line. **Curvature** has already been described above and **Rubbish Lines Limit** is how many lines not forming a cross can be ignored while evaluating a cell. Setting this too low will hurt crosses which are not neat and setting it too high will take corrected answers as the true answers.
- **Pixel Ratio** - This method simply counts the number of black pixels (see **Black and White Threshold** above) and compares it to the total number of pixels in a cell. If the ratio falls between the two thresholds, the cell contains a selected answer.
