namespace KlokanUI
{
	/// <summary>
	/// Every form that performs evaluation needs to implement this interface, 
	/// so that the evaluation process can correctly communicate with the form.
	/// </summary>
	interface IEvaluationForm
	{
		void EnableGoButton();
		void ShowMessageBoxInfo(int failedSheets, double evaluationTime, double databaseTime);
	}
}
