using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Skyline.DataMiner.Automation;
using Skyline.DataMiner.Library.Common;
using Skyline.DataMiner.Library.Common.Properties;

namespace AddNewSwitch_1
{
	public class Model
	{
		private const string LeafSwitchProtocolName = "Arista Switch";
		private const string LocationsRootViewName = "Locations";

		private readonly IDms dms;

		private string[] locations;
		private string selectedLocation;

		public Model(IDms dms)
		{
			if (dms == null) throw new ArgumentNullException("dms");

			this.dms = dms;
		}

		public string Name { get; set; }

		public string IpAddress { get; set; }

		public string[] Locations
		{
			get
			{
				if (locations == null)
				{
					InitializeLocations();
				}

				return locations;
			}
		}

		public string SelectedLocation
		{
			get
			{
				if (selectedLocation == null)
				{
					selectedLocation = locations.First();
				}

				return selectedLocation;
			}

			set
			{
				if (value == SelectedLocation) return;

				selectedLocation = value;
			}
		}

		public bool ActivateImmediately { get; set; }

		public ValidationResult ValidateName()
		{
			var expectedPrefix = $"{SelectedLocation}-LEAF_";
			if (String.IsNullOrEmpty(Name) || !Name.StartsWith(expectedPrefix)) return new ValidationResult(false, $"Name should start with '{expectedPrefix}'");

			var allElements = dms.GetElements();
			if (allElements.Any(e => e.Name == Name)) return new ValidationResult(false, "Name is not unique");

			return new ValidationResult(true);
		}

		public ValidationResult ValidateIpAddress()
		{
			if (!IPAddress.TryParse(IpAddress, out var address)) return new ValidationResult(false, "Invalid IP address");

			return new ValidationResult(true);
		}

		public IDmsElement CreateSwitchElement(IEngine engine)
		{
			var protocol = dms.GetProtocol(LeafSwitchProtocolName, "Production");

			var connection = new SnmpV2Connection(new Udp(IpAddress, 161));
			var elementConfiguration = new ElementConfiguration(dms, Name, protocol, new[] { connection });

			var view = dms.GetView(SelectedLocation);
			elementConfiguration.Views.Add(view);

			var agent = dms.GetAgents().FirstOrDefault();

			var id = agent.CreateElement(elementConfiguration);
			if (!WaitUntilElementIsAvailable(id, TimeSpan.FromSeconds(10))) return null;

			var element = agent.GetElement(id);
			ProvisionSwitchElement(element);

			return element;
		}

		private void InitializeLocations()
		{
			var locationsRootView = dms.GetView(LocationsRootViewName);
			locations = locationsRootView.ChildViews.Select(v => v.Name).ToArray();
		}

		private void ProvisionSwitchElement(IDmsElement element)
		{
			var systemContactParameter = element.GetStandaloneParameter<string>(10502);
			systemContactParameter.SetValue("it@skyline.be");

			var systemLocationParameter = element.GetStandaloneParameter<string>(10504);
			systemLocationParameter.SetValue(SelectedLocation);

			var systemNameConfigurationParameter = element.GetStandaloneParameter<string>(10111);
			systemNameConfigurationParameter.SetValue(Name);

			var systemDescriptionConfigurationParameter = element.GetStandaloneParameter<string>(10113);
			systemDescriptionConfigurationParameter.SetValue("Arista Networks EOS version 4.15.2F running on an Arista Networks DCS-7010T-48");

			var modelConfigurationParameter = element.GetStandaloneParameter<string>(10115);
			modelConfigurationParameter.SetValue("7050X3");

			var interfacesConfigurationModeParameter = element.GetStandaloneParameter<double?>(10232);
			interfacesConfigurationModeParameter.SetValue(1);

			var deployConfigButtonParameter = element.GetStandaloneParameter<double?>(10120);
			deployConfigButtonParameter.SetValue(1);

			var interfaceTable = element.GetTable(11000);
			var physicalConnectionColumn = interfaceTable.GetColumn<int?>(11134);
			physicalConnectionColumn.SetValue("1", 1);
			physicalConnectionColumn.SetValue("2", 1);
			physicalConnectionColumn.SetValue("3", 1);
			physicalConnectionColumn.SetValue("49", 1);
			physicalConnectionColumn.SetValue("50", 1);

			var locationProperty = (IWritableProperty)element.Properties["Location"];
			if (locationProperty != null) locationProperty.Value = SelectedLocation;

			var typeProperty = (IWritableProperty)element.Properties["Type"];
			if (typeProperty != null) typeProperty.Value = "Leaf";

			var isActiveProperty = (IWritableProperty)element.Properties["IsActive"];
			if (isActiveProperty != null) isActiveProperty.Value = "False";

			element.Update();
		}

		private bool WaitUntilElementIsAvailable(DmsElementId id, TimeSpan timeout)
		{
			var success = false;

			var sw = new Stopwatch();
			sw.Start();

			do
			{
				success = dms.ElementExists(id);
				if (!success)
				{
					Thread.Sleep(1000);
				}
			}
			while (!success && sw.Elapsed <= timeout);

			return success;
		}
	}
}