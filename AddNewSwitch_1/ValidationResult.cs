namespace AddNewSwitch_1
{
	public class ValidationResult
	{
		public ValidationResult(bool isValid, string validationMessage = "")
		{
			IsValid = isValid;
			ValidationMessage = validationMessage;
		}

		public bool IsValid { get; private set; }

		public string ValidationMessage { get; private set; }
	}
}