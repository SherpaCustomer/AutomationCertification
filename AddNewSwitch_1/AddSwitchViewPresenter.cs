using System;
using System.Linq;

namespace AddNewSwitch_1
{
	public class AddSwitchViewPresenter
	{
		private readonly AddSwitchView view;
		private readonly Model model;

		public AddSwitchViewPresenter(AddSwitchView view, Model model)
		{
			if (view == null) throw new ArgumentNullException("view");
			if (model == null) throw new ArgumentNullException("model");

			this.view = view;
			this.model = model;

			view.CreateButton.Pressed += OnCreateButtonPressed;
		}

		public event EventHandler<EventArgs> Create;

		public void LoadFromModel()
		{
			view.NameTextBox.Text = model.Name;
			view.IpAddressTextBox.Text = model.IpAddress;
			view.LocationDropDown.SetOptions(model.Locations);
			view.LocationDropDown.Selected = model.SelectedLocation;
			view.ActivateImmediatelyCheckBox.IsChecked = model.ActivateImmediately;
		}

		private void StoreToModel()
		{
			model.Name = view.NameTextBox.Text;
			model.IpAddress = view.IpAddressTextBox.Text;
			model.SelectedLocation = view.LocationDropDown.Selected;
			model.ActivateImmediately = view.ActivateImmediatelyCheckBox.IsChecked;
		}

		private void OnCreateButtonPressed(object sender, EventArgs e)
		{
			StoreToModel();

			var nameValidationResult = model.ValidateName();
			var ipAddressValidationResult = model.ValidateIpAddress();
			var isValid = nameValidationResult.IsValid && ipAddressValidationResult.IsValid;
			if (!isValid)
			{
				view.UpdateNameTextBoxValidation(nameValidationResult.IsValid, nameValidationResult.ValidationMessage);
				view.UpdateIpAddressTextBoxValidation(ipAddressValidationResult.IsValid, ipAddressValidationResult.ValidationMessage);
				view.UpdateValidationLabel("Input is not valid!");

				return;
			}

			if (Create != null)
			{
				Create(this, EventArgs.Empty);
			}
		}
	}
}
