using Skyline.DataMiner.Automation;
using Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit;

namespace AddNewSwitch_1
{
	public class AddSwitchView : Dialog
	{
		public AddSwitchView(IEngine engine) : base(engine)
		{
			Title = "Add New Leaf";

			NameTextBox = new TextBox();
			IpAddressTextBox = new TextBox();
			LocationDropDown = new DropDown();
			ActivateImmediatelyCheckBox = new CheckBox("Activate Immediately");

			CreateButton = new Button("Create");
			ValidationErrorLabel = new Label(string.Empty);

			AddWidget(new Label("Name"), 0, 0);
			AddWidget(NameTextBox, 0, 1, 1, 2);

			AddWidget(new Label("IP Address"), 1, 0);
			AddWidget(IpAddressTextBox, 1, 1, 1, 2);

			AddWidget(new Label("Location"), 2, 0);
			AddWidget(LocationDropDown, 2, 1, 1, 2);

			AddWidget(ActivateImmediatelyCheckBox, 3, 1, 1, 2);

			AddWidget(CreateButton, 4, 1);

			AddWidget(ValidationErrorLabel, 4, 2);
		}

		public TextBox NameTextBox { get; private set; }

		public TextBox IpAddressTextBox { get; private set; }

		public DropDown LocationDropDown { get; private set; }

		public CheckBox ActivateImmediatelyCheckBox { get; private set; }

		public Button CreateButton { get; private set; }

		public Label ValidationErrorLabel { get; private set; }

		public void UpdateNameTextBoxValidation(bool isValid, string message)
		{
			NameTextBox.ValidationState = isValid ? UIValidationState.Valid : UIValidationState.Invalid;
			NameTextBox.ValidationText = message;
		}

		public void UpdateIpAddressTextBoxValidation(bool isValid, string message)
		{
			IpAddressTextBox.ValidationState = isValid ? UIValidationState.Valid : UIValidationState.Invalid;
			IpAddressTextBox.ValidationText = message;
		}

		public void UpdateValidationLabel(string message)
		{
			ValidationErrorLabel.Text = message;
		}
	}
}