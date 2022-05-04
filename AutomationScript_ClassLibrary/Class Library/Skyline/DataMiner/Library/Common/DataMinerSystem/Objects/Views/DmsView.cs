namespace Skyline.DataMiner.Library.Common
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Globalization;
	using System.Linq;
	using Net.Exceptions;
	using Net.Messages;
	using Net.Messages.Advanced;
	using Properties;

	/// <summary>
	/// Represents a DataMiner view.
	/// </summary>
	internal class DmsView : DmsObject, IDmsView
	{
		/// <summary>
		/// The child views.
		/// </summary>
		private readonly List<IDmsView> childViews = new List<IDmsView>();

		/// <summary>
		/// The elements that are part of this view.
		/// </summary>
		private readonly List<IDmsElement> elements = new List<IDmsElement>();

		/// <summary>
		/// The properties.
		/// </summary>
		private readonly IDictionary<string, DmsViewProperty> properties = new Dictionary<string, DmsViewProperty>();

		/// <summary>
		/// The names of updated properties.
		/// </summary>
		private readonly HashSet<string> updatedProperties = new HashSet<string>();

		/// <summary>
		/// The display string.
		/// </summary>
		private string display = String.Empty;

		/// <summary>
		/// ID of the view.
		/// </summary>
		private int id = -1;

		/// <summary>
		/// The parent view.
		/// </summary>
		private IDmsView parentView;

		/// <summary>
		/// The name of the view.
		/// </summary>
		private string name;

		private bool isNameLoaded;

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsView"/> class.
		/// </summary>
		/// <param name="dms">Object implementing the <see cref="IDms"/> interface.</param>
		/// <param name="viewId">The ID of the view.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dms"/> is <see langword="null"/>.</exception>
		internal DmsView(IDms dms, int viewId)
			: base(dms)
		{
			id = viewId;
			IsLoaded = false;
			isNameLoaded = false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsView"/> class.
		/// </summary>
		/// <param name="dms">Object implementing the <see cref="IDms"/> interface.</param>
		/// <param name="id">The ID of the view.</param>
		/// <param name="name">The name of the view.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dms"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name">is empty or white space.</paramref></exception>
		internal DmsView(IDms dms, int id, string name)
			 : base(dms)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}

			if (String.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("Provided name must not be empty or white space", "name");
			}

			this.id = id;
			this.name = name;
			IsLoaded = false;
			isNameLoaded = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsView"/> class.
		/// </summary>
		/// <param name="dms">Instance of the DataMinerSystem class.</param>
		/// <param name="viewInfo">The view info.</param>
		internal DmsView(Dms dms, ViewInfoEventMessage viewInfo)
			: base(dms)
		{
			Parse(viewInfo);

			// Remove the properties that are added to the change list because of initialization.
			ClearChangeList();
		}

		/// <summary>
		/// Gets all child views.
		/// </summary>
		/// <value>The child views.</value>
		public IList<IDmsView> ChildViews
		{
			get
			{
				LoadOnDemand();
				return childViews.AsReadOnly();
			}
		}

		/// <summary>
		/// Gets the display string.
		/// </summary>
		/// <value>The display string.</value>
		public string Display
		{
			get
			{
				LoadOnDemand();
				return display;
			}
		}

		/// <summary>
		/// Gets all elements contained in this view.
		/// </summary>
		/// <value>The elements contained in this view.</value>
		public IList<IDmsElement> Elements
		{
			get
			{
				LoadOnDemand();
				return elements.AsReadOnly();
			}
		}

		/// <summary>
		/// Gets the ID of this view.
		/// </summary>
		/// <value>The view ID.</value>
		public int Id
		{
			get
			{
				return id;
			}
		}

		/// <summary>
		/// Gets or sets the name of the view.
		/// </summary>
		/// <value>The view name.</value>
		/// <exception cref="ArgumentNullException">The value of a set operation is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">The value of a set operation is invalid.</exception>
		/// <remarks>
		/// <para>The following restrictions apply to view names:</para>
		/// <list type="bullet">
		/// <item><para>Must not be empty ("") or white space.</para></item>
		/// <item><para>Must not exceed 200 characters.</para></item>
		/// <item><para>Names may not start or end with the following characters: '.' (dot), ' ' (space).</para></item>
		/// <item><para>Names may not contain the following character: '|' (pipe).</para></item>
		/// <item><para>The following characters may not occur more than once within a name: '%' (percentage).</para></item>
		/// </list>
		/// </remarks>
		public string Name
		{
			get
			{
				if (!isNameLoaded)
				{
					LoadOnDemand();
				}

				return name;
			}

			set
			{
				string validatedViewName = InputValidator.ValidateViewName(value, "value");

				if (!isNameLoaded)
				{
					LoadOnDemand();
				}

				if (!name.Equals(validatedViewName, StringComparison.Ordinal))
				{
					ChangedPropertyList.Add("Name");
					name = validatedViewName;
				}
			}
		}

		/// <summary>
		/// Gets or sets the parent view.
		/// </summary>
		/// <value>The parent view.</value>
		/// <exception cref="ArgumentNullException">The value of a set operation is <see langword="null"/>.</exception>
		/// <exception cref="NotSupportedException">The root view cannot be assigned a parent view.</exception>
		/// <exception cref="NotSupportedException">The parent of a view must not be a self-reference.</exception>
		public IDmsView Parent
		{
			get
			{
				LoadOnDemand();
				return parentView;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				if (Id == -1)
				{
					throw new NotSupportedException("The root view cannot be assigned a parent view.");
				}

				LoadOnDemand();

				if (value.Id == this.Id)
				{
					throw new NotSupportedException("The parent of a view must not be a self-reference.");
				}

				if (parentView != value)
				{
					ChangedPropertyList.Add("ParentView");
					parentView = value;
				}
			}
		}

		/// <summary>
		/// Gets the properties of this view.
		/// </summary>
		/// <value>The view properties.</value>
		public IPropertyCollection<IDmsViewProperty, IDmsViewPropertyDefinition> Properties
		{
			get
			{
				LoadOnDemand();

				IDictionary<string, IDmsViewProperty> copy = new Dictionary<string, IDmsViewProperty>(properties.Count);
				foreach (KeyValuePair<string, DmsViewProperty> kvp in properties)
				{
					copy.Add(kvp.Key, kvp.Value);
				}

				return new PropertyCollection<IDmsViewProperty, IDmsViewPropertyDefinition>(copy);
			}
		}

		/// <summary>
		/// Removes the view from the DataMiner System.
		/// </summary>
		public void Delete()
		{
			SetDataMinerInfoMessage message = new SetDataMinerInfoMessage
			{
				DataMinerID = -1,
				ElementID = -1,
				IInfo1 = id,
				IInfo2 = -1,
				What = 4
			};

			Communication.SendMessage(message);
		}

		/// <summary>
		/// Checks if the view exists in the DataMiner System.
		/// </summary>
		/// <returns><c>true</c> if this view exists in the DataMiner System; otherwise, <c>false</c>.</returns>
		public override bool Exists()
		{
			return Dms.ViewExists(id);
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "View name: {0}, ID: {1}", Name, Id);
		}

		/// <summary>
		/// Updates the view.
		/// </summary>
		public void Update()
		{
			// If any op the properties where updated, add "Properties" to the property changed list.
			CheckIfPropertiesEdited();

			if (ChangedPropertyList.Any())
			{
				try
				{
					ExecuteUpdates();

					// Reset this flag because we want to reload the latest values in the object.
					IsLoaded = false;
					isNameLoaded = false;
					ClearChangeList();
				}
				catch (Exception)
				{
					IsLoaded = false;
					isNameLoaded = false;
					throw;
				}
			}
		}

		/// <summary>
		/// Loads the content of the view.
		/// All of the properties.
		/// All of the elements in the view.
		/// </summary>
		/// <exception cref="ViewNotFoundException">No view with the specified ID exists in the DataMiner System.</exception>
		internal override void Load()
		{
			try
			{
				IsLoaded = true;
				isNameLoaded = true;

				ViewInfoEventMessage infoEvent = null;
				GetInfoMessage message = new GetInfoMessage
				{
					Type = InfoType.ViewInfo
				};

				DMSMessage[] responses = Communication.SendMessage(message);
				foreach (DMSMessage response in responses)
				{
					ViewInfoEventMessage viewInfo = (ViewInfoEventMessage)response;

					if (viewInfo.ID.Equals(Id))
					{
						infoEvent = viewInfo;
						break;
					}
				}

				if (infoEvent != null)
				{
					Parse(infoEvent);
				}
				else
				{
					throw new ViewNotFoundException(id);
				}
			}
			catch (DataMinerException)
			{
				IsLoaded = false;
				isNameLoaded = false;
				throw;
			}
		}

		internal void PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			updatedProperties.Add(e.PropertyName);
		}

		/// <summary>
		/// Parses the view info event message.
		/// </summary>
		/// <param name="viewInfo">The view info event message.</param>
		internal void Parse(ViewInfoEventMessage viewInfo)
		{
			IsLoaded = true;
			isNameLoaded = true;

			try
			{
				ParseView(viewInfo);
				ParseChildViews(viewInfo);
				ParseChildElements(viewInfo);
				ParseProperties(viewInfo);
			}
			catch
			{
				IsLoaded = false;
				isNameLoaded = false;
				throw;
			}
		}

		/// <summary>
		/// Clears the property changed list.
		/// </summary>
		private void ClearChangeList()
		{
			updatedProperties.Clear();
		}

		/// <summary>
		/// Parses the view.
		/// </summary>
		/// <param name="viewInfo">The view information.</param>
		private void ParseView(ViewInfoEventMessage viewInfo)
		{
			name = viewInfo.Name;
			id = viewInfo.ID;
			display = viewInfo.DisplayName;
			parentView = Id == -1 ? null : new DmsView(dms, viewInfo.ParentId);
		}

		/// <summary>
		/// Parses the child view.
		/// </summary>
		/// <param name="viewInfo">The view information.</param>
		private void ParseChildViews(ViewInfoEventMessage viewInfo)
		{
			if (viewInfo.DirectChildViews != null)
			{
				foreach (int viewID in viewInfo.DirectChildViews)
				{
					DmsView childView = new DmsView(dms, viewID);
					childViews.Add(childView);
				}
			}
		}

		/// <summary>
		/// Parses the child view.
		/// </summary>
		/// <param name="viewInfo">The view information.</param>
		private void ParseChildElements(ViewInfoEventMessage viewInfo)
		{
			if (viewInfo.Elements != null)
			{
				foreach (string identifier in viewInfo.Elements)
				{
					DmsElementId dmaEid = new DmsElementId(identifier);
					DmsElement element = new DmsElement(dms, dmaEid);
					elements.Add(element);
				}
			}
		}

		/// <summary>
		/// Parses the view properties.
		/// </summary>
		/// <param name="viewInfo">The view information.</param>
		private void ParseProperties(ViewInfoEventMessage viewInfo)
		{
			properties.Clear();

			foreach (IDmsViewPropertyDefinition definition in Dms.ViewPropertyDefinitions)
			{
				PropertyInfo info = null;
				if (viewInfo.Properties != null)
				{
					info = viewInfo.Properties.FirstOrDefault(p => p.Name.Equals(definition.Name, StringComparison.OrdinalIgnoreCase));

					List<String> duplicates = viewInfo.Properties.GroupBy(p => p.Name)
						.Where(g => g.Count() > 1)
						.Select(g => g.Key)
						.ToList();

					if (duplicates.Any())
					{
						string message = "Duplicate view properties detected. View \"" + viewInfo.Name + "\" (" + viewInfo.ID + "), duplicate properties: " + String.Join(", ", duplicates) + ".";
						Logger.Log(message);
					}
				}

				string propertyValue = info != null ? info.Value : String.Empty;

				if (definition.IsReadOnly)
				{
					properties.Add(definition.Name, new DmsViewProperty(this, definition, propertyValue));
				}
				else
				{
					var property = new DmsWritableViewProperty(this, definition, propertyValue);
					properties.Add(definition.Name, property);

					property.PropertyChanged += this.PropertyChanged;
				}
			}
		}

		/// <summary>
		/// Performs the correct operations based on which property of the view was changed.
		/// </summary>
		private void ExecuteUpdates()
		{
			foreach (string operation in ChangedPropertyList)
			{
				switch (operation)
				{
					case "Name":
						UpdateName();
						break;
					case "ParentView":
						UpdateParent();
						break;
					case "Properties":
						UpdateProperties();
						break;
					default:
						continue;
				}
			}
		}

		/// <summary>
		/// Checks of the properties where changed.
		/// </summary>
		private void CheckIfPropertiesEdited()
		{
			if (updatedProperties.Any())
			{
				ChangedPropertyList.Add("Properties");
			}
		}

		/// <summary>
		/// Performs an update of the properties.
		/// </summary>
		private void UpdateProperties()
		{
			PSA propertyArray = new PSA
			{
				Psa = new SA[] { }
			};

			List<SA> changedProperties = new List<SA>();
			foreach (string update in updatedProperties)
			{
				DmsViewProperty property = properties[update];
				SA sa = new SA
				{
					Sa = new string[] { property.Definition.Name, "read-write", property.Value }
				};

				changedProperties.Add(sa);
			}

			propertyArray.Psa = changedProperties.ToArray();

			SetDataMinerInfoMessage infoMessage = new SetDataMinerInfoMessage
			{
				bInfo1 = Int32.MaxValue,
				bInfo2 = Int32.MaxValue,
				DataMinerID = -1,
				ElementID = -1,
				IInfo1 = Int32.MaxValue,
				IInfo2 = Int32.MaxValue,
				Psa2 = propertyArray,
				What = 62,
				StrInfo1 = "view:" + Id
			};

			Communication.SendSingleResponseMessage(infoMessage);
		}

		/// <summary>
		/// Renames the view.
		/// </summary>
		private void UpdateName()
		{
			SetDataMinerInfoMessage infoMessage = new SetDataMinerInfoMessage
			{
				bInfo1 = Int32.MaxValue,
				bInfo2 = Int32.MaxValue,
				DataMinerID = -1,
				ElementID = -1,
				IInfo1 = id,
				IInfo2 = Int32.MaxValue,
				StrInfo2 = Name,
				What = 6
			};

			Communication.SendSingleResponseMessage(infoMessage);
		}

		/// <summary>
		/// Changes the current view to a new parent.
		/// </summary>
		private void UpdateParent()
		{
			SetDataMinerInfoMessage infoMessage = new SetDataMinerInfoMessage
			{
				IInfo1 = Id,
				IInfo2 = parentView.Id,
				What = 118
			};

			Communication.SendSingleResponseMessage(infoMessage);
		}
	}
}