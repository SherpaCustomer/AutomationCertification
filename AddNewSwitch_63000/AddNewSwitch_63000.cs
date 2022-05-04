// --- auto-generated code --- do not modify ---

/*
{{StartPackageInfo}}
<PackageInfo xmlns="http://www.skyline.be/ClassLibrary">
	<BasePackage>
		<Identity>
			<Name>Class Library</Name>
			<Version>1.2.0.12</Version>
		</Identity>
	</BasePackage>
	<CustomPackages>
		<Package>
			<Identity>
				<Name>InteractiveAutomationToolkit</Name>
				<Version>1.0.6.4</Version>
			</Identity>
		</Package>
	</CustomPackages>
</PackageInfo>
{{EndPackageInfo}}
*/

namespace Skyline.DataMiner
{
    namespace Library
    {
        namespace Automation
        {
            /// <summary>
            /// Defines extension methods on the <see cref = "IEngine"/> interface.
            /// </summary>
            [Skyline.DataMiner.Library.Common.Attributes.DllImport("SLManagedAutomation.dll")]
            [Skyline.DataMiner.Library.Common.Attributes.DllImport("SLNetTypes.dll")]
            public static class IEngineExtensions
            {
#pragma warning disable S1104 // Fields should not have public accessibility

#pragma warning disable S2223 // Non-constant static fields should not be visible

                /// <summary>
                /// Allows an override of the behavior of GetDms to return a Fake or Mock of <see cref = "IDms"/>.
                /// Important: When this is used, unit tests should never be run in parallel.
                /// </summary>
                public static System.Func<Skyline.DataMiner.Automation.IEngine, Skyline.DataMiner.Library.Common.IDms> OverrideGetDms = engine =>
                {
                    return new Skyline.DataMiner.Library.Common.Dms(new Skyline.DataMiner.Library.Common.ConnectionCommunication(Skyline.DataMiner.Automation.Engine.SLNetRaw));
                }

                ;
#pragma warning restore S2223 // Non-constant static fields should not be visible

#pragma warning restore S1104 // Fields should not have public accessibility

                /// <summary>
                /// Retrieves an object implementing the <see cref = "IDms"/> interface.
                /// </summary>
                /// <param name = "engine">The <see cref = "IEngine"/> implementation.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "engine"/> is <see langword = "null"/>.</exception>
                /// <returns>The <see cref = "IDms"/> object.</returns>
                public static Skyline.DataMiner.Library.Common.IDms GetDms(this Skyline.DataMiner.Automation.IEngine engine)
                {
                    if (engine == null)
                    {
                        throw new System.ArgumentNullException("engine");
                    }

                    return OverrideGetDms(engine);
                }
            }
        }

        namespace Common
        {
            namespace Attributes
            {
                /// <summary>
                /// This attribute indicates a DLL is required.
                /// </summary>
                [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = true)]
                public sealed class DllImportAttribute : System.Attribute
                {
                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DllImportAttribute"/> class.
                    /// </summary>
                    /// <param name = "dllImport">The name of the DLL to be imported.</param>
                    public DllImportAttribute(string dllImport)
                    {
                        DllImport = dllImport;
                    }

                    /// <summary>
                    /// Gets the name of the DLL to be imported.
                    /// </summary>
                    public string DllImport
                    {
                        get;
                        private set;
                    }
                }
            }

            /// <summary>
            /// Represents a system-wide element ID.
            /// </summary>
            /// <remarks>This is a combination of a DataMiner Agent ID (the ID of the Agent on which the element was created) and an element ID.</remarks>
            [System.Serializable]
            [Skyline.DataMiner.Library.Common.Attributes.DllImport("Newtonsoft.Json.dll")]
            public struct DmsElementId : System.IEquatable<Skyline.DataMiner.Library.Common.DmsElementId>, System.IComparable, System.IComparable<Skyline.DataMiner.Library.Common.DmsElementId>
            {
                /// <summary>
                /// The DataMiner Agent ID.
                /// </summary>
                private int agentId;
                /// <summary>
                /// The element ID.
                /// </summary>
                private int elementId;
                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsElementId"/> structure using the specified string.
                /// </summary>
                /// <param name = "id">String representing the system-wide element ID.</param>
                /// <remarks>The provided string must be formatted as follows: "DataMiner Agent ID/element ID (e.g. 400/201)".</remarks>
                /// <exception cref = "ArgumentNullException"><paramref name = "id"/> is <see langword = "null"/> .</exception>
                /// <exception cref = "ArgumentException"><paramref name = "id"/> is the empty string ("") or white space.</exception>
                /// <exception cref = "ArgumentException">The ID does not match the mandatory format.</exception>
                /// <exception cref = "ArgumentException">The DataMiner Agent ID is not an integer.</exception>
                /// <exception cref = "ArgumentException">The element ID is not an integer.</exception>
                /// <exception cref = "ArgumentException">Invalid DataMiner Agent ID.</exception>
                /// <exception cref = "ArgumentException">Invalid element ID.</exception>
                public DmsElementId(string id)
                {
                    if (id == null)
                    {
                        throw new System.ArgumentNullException("id");
                    }

                    if (System.String.IsNullOrWhiteSpace(id))
                    {
                        throw new System.ArgumentException("The provided ID must not be empty.", "id");
                    }

                    string[] idParts = id.Split('/');
                    if (idParts.Length != 2)
                    {
                        string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid ID. Value: {0}. The string must be formatted as follows: \"agent ID/element ID\".", id);
                        throw new System.ArgumentException(message, "id");
                    }

                    if (!System.Int32.TryParse(idParts[0], System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out agentId))
                    {
                        string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid DataMiner agent ID. \"{0}\" is not an integer value", id);
                        throw new System.ArgumentException(message, "id");
                    }

                    if (!System.Int32.TryParse(idParts[1], System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out elementId))
                    {
                        string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid Element ID. \"{0}\" is not an integer value", id);
                        throw new System.ArgumentException(message, "id");
                    }

                    if (!IsValidAgentId())
                    {
                        string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid agent ID. Value: {0}.", agentId);
                        throw new System.ArgumentException(message, "id");
                    }

                    if (!IsValidElementId())
                    {
                        string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid element ID. Value: {0}.", elementId);
                        throw new System.ArgumentException(message, "id");
                    }
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsElementId"/> structure using the specified element ID and DataMiner Agent ID.
                /// </summary>
                /// <param name = "agentId">The DataMiner Agent ID.</param>
                /// <param name = "elementId">The element ID.</param>
                /// <remarks>The hosting DataMiner Agent ID value will be set to the same value as the specified DataMiner Agent ID.</remarks>
                /// <exception cref = "ArgumentException"><paramref name = "agentId"/> is invalid.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "elementId"/> is invalid.</exception>
                public DmsElementId(int agentId, int elementId)
                {
                    if ((elementId == -1 && agentId != -1) || agentId < -1)
                    {
                        string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid agent ID. Value: {0}.", agentId);
                        throw new System.ArgumentException(message, "agentId");
                    }

                    if ((agentId == -1 && elementId != -1) || elementId < -1)
                    {
                        string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid element ID. Value: {0}.", elementId);
                        throw new System.ArgumentException(message, "elementId");
                    }

                    this.elementId = elementId;
                    this.agentId = agentId;
                }

                /// <summary>
                /// Gets the DataMiner Agent ID.
                /// </summary>
                /// <remarks>The DataMiner Agent ID is the ID of the DataMiner Agent this element has been created on.</remarks>
                public int AgentId
                {
                    get
                    {
                        return agentId;
                    }

                    private set
                    {
                        // setter for serialization.
                        agentId = value;
                    }
                }

                /// <summary>
                /// Gets the element ID.
                /// </summary>
                public int ElementId
                {
                    get
                    {
                        return elementId;
                    }

                    private set
                    {
                        // setter for serialization.
                        elementId = value;
                    }
                }

                /// <summary>
                /// Gets the DataMiner Agent ID/element ID string representation.
                /// </summary>
                [Newtonsoft.Json.JsonIgnore]
                public string Value
                {
                    get
                    {
                        return agentId + "/" + elementId;
                    }
                }

                /// <summary>
                /// Compares the current instance with another object of the same type and returns an integer that indicates whether the
                /// current instance precedes, follows, or occurs in the same position in the sort order as the other object.
                /// </summary>
                /// <param name = "other">An object to compare with this instance.</param>
                /// <returns>A value that indicates the relative order of the objects being compared.
                /// The return value has these meanings: Less than zero means this instance precedes <paramref name = "other"/> in the sort order.
                /// Zero means this instance occurs in the same position in the sort order as <paramref name = "other"/>.
                /// Greater than zero means this instance follows <paramref name = "other"/> in the sort order.</returns>
                /// <remarks>The order of the comparison is as follows: DataMiner Agent ID, element ID.</remarks>
                public int CompareTo(Skyline.DataMiner.Library.Common.DmsElementId other)
                {
                    int result = agentId.CompareTo(other.AgentId);
                    if (result == 0)
                    {
                        result = elementId.CompareTo(other.ElementId);
                    }

                    return result;
                }

                /// <summary>
                /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
                /// </summary>
                /// <param name = "obj">An object to compare with this instance.</param>
                /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Less than zero means this instance precedes <paramref name = "obj"/> in the sort order. Zero means this instance occurs in the same position in the sort order as <paramref name = "obj"/>. Greater than zero means this instance follows <paramref name = "obj"/> in the sort order.</returns>
                /// <remarks>The order of the comparison is as follows: DataMiner Agent ID, element ID.</remarks>
                /// <exception cref = "ArgumentException">The obj is not of type <see cref = "DmsElementId"/></exception>
                public int CompareTo(object obj)
                {
                    if (obj == null)
                    {
                        return 1;
                    }

                    if (!(obj is Skyline.DataMiner.Library.Common.DmsElementId))
                    {
                        throw new System.ArgumentException("The provided object must be of type DmsElementId.", "obj");
                    }

                    return CompareTo((Skyline.DataMiner.Library.Common.DmsElementId)obj);
                }

                /// <summary>
                /// Compares the object to another object.
                /// </summary>
                /// <param name = "obj">The object to compare against.</param>
                /// <returns><c>true</c> if the elements are equal; otherwise, <c>false</c>.</returns>
                public override bool Equals(object obj)
                {
                    if (!(obj is Skyline.DataMiner.Library.Common.DmsElementId))
                    {
                        return false;
                    }

                    return Equals((Skyline.DataMiner.Library.Common.DmsElementId)obj);
                }

                /// <summary>
                /// Indicates whether the current object is equal to another object of the same type.
                /// </summary>
                /// <param name = "other">An object to compare with this object.</param>
                /// <returns><c>true</c> if the elements are equal; otherwise, <c>false</c>.</returns>
                public bool Equals(Skyline.DataMiner.Library.Common.DmsElementId other)
                {
                    if (elementId == other.elementId && agentId == other.agentId)
                    {
                        return true;
                    }

                    return false;
                }

                /// <summary>
                /// Returns the hash code.
                /// </summary>
                /// <returns>The hash code.</returns>
                public override int GetHashCode()
                {
                    return elementId ^ agentId;
                }

                /// <summary>
                /// Returns a string that represents the current object.
                /// </summary>
                /// <returns>A string that represents the current object.</returns>
                public override string ToString()
                {
                    return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "agent ID: {0}, element ID: {1}", agentId, elementId);
                }

                /// <summary>
                /// Returns a value determining whether the agent ID is valid.
                /// </summary>
                /// <returns><c>true</c> if the agent ID is valid; otherwise, <c>false</c>.</returns>
                private bool IsValidAgentId()
                {
                    bool isValid = true;
                    if ((elementId == -1 && agentId != -1) || agentId < -1)
                    {
                        isValid = false;
                    }

                    return isValid;
                }

                /// <summary>
                /// Returns a value determining whether the element ID is valid.
                /// </summary>
                /// <returns><c>true</c> if the element ID is valid; otherwise, <c>false</c>.</returns>
                private bool IsValidElementId()
                {
                    bool isValid = true;
                    if ((agentId == -1 && elementId != -1) || elementId < -1)
                    {
                        isValid = false;
                    }

                    return isValid;
                }
            }

            /// <summary>
            /// Represents a system-wide element ID.
            /// </summary>
            /// <remarks>This is a combination of a DataMiner Agent ID (the ID of the Agent on which the element was created) and an element ID.</remarks>
            [System.Serializable]
            [Skyline.DataMiner.Library.Common.Attributes.DllImport("Newtonsoft.Json.dll")]
            public struct DmsServiceId : System.IEquatable<Skyline.DataMiner.Library.Common.DmsServiceId>, System.IComparable, System.IComparable<Skyline.DataMiner.Library.Common.DmsServiceId>
            {
                /// <summary>
                /// The DataMiner Agent ID.
                /// </summary>
                private int agentId;
                /// <summary>
                /// The service ID.
                /// </summary>
                private int serviceId;
                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsServiceId"/> structure using the specified string.
                /// </summary>
                /// <param name = "id">String representing the system-wide service ID.</param>
                /// <remarks>The provided string must be formatted as follows: "DataMiner Agent ID/service ID (e.g. 400/201)".</remarks>
                /// <exception cref = "ArgumentNullException"><paramref name = "id"/> is <see langword = "null"/> .</exception>
                /// <exception cref = "ArgumentException"><paramref name = "id"/> is the empty string ("") or white space.</exception>
                /// <exception cref = "ArgumentException">The ID does not match the mandatory format.</exception>
                /// <exception cref = "ArgumentException">The DataMiner Agent ID is not an integer.</exception>
                /// <exception cref = "ArgumentException">The service ID is not an integer.</exception>
                /// <exception cref = "ArgumentException">Invalid DataMiner Agent ID.</exception>
                /// <exception cref = "ArgumentException">Invalid service ID.</exception>
                public DmsServiceId(string id)
                {
                    if (id == null)
                    {
                        throw new System.ArgumentNullException("id");
                    }

                    if (System.String.IsNullOrWhiteSpace(id))
                    {
                        throw new System.ArgumentException("The provided ID must not be empty.", "id");
                    }

                    string[] identifierParts = id.Split('/');
                    if (identifierParts.Length != 2)
                    {
                        string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid ID. Value: {0}. The string must be formatted as follows: \"agent ID/service ID\".", id);
                        throw new System.ArgumentException(message, "id");
                    }

                    if (!System.Int32.TryParse(identifierParts[0], System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out agentId))
                    {
                        string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid DataMiner agent ID. \"{0}\" is not an integer value", id);
                        throw new System.ArgumentException(message, "id");
                    }

                    if (!System.Int32.TryParse(identifierParts[1], System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out serviceId))
                    {
                        string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid Service ID. \"{0}\" is not an integer value", id);
                        throw new System.ArgumentException(message, "id");
                    }

                    if (!IsValidAgentId())
                    {
                        string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid agent ID. Value: {0}.", agentId);
                        throw new System.ArgumentException(message, "id");
                    }

                    if (!IsValidServiceId())
                    {
                        string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid element ID. Value: {0}.", serviceId);
                        throw new System.ArgumentException(message, "id");
                    }
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsServiceId"/> structure using the specified service ID and DataMiner Agent ID.
                /// </summary>
                /// <param name = "agentId">The DataMiner Agent ID.</param>
                /// <param name = "serviceId">The service ID.</param>
                /// <remarks>The hosting DataMiner Agent ID value will be set to the same value as the specified DataMiner Agent ID.</remarks>
                /// <exception cref = "ArgumentException"><paramref name = "agentId"/> is invalid.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "serviceId"/> is invalid.</exception>
                public DmsServiceId(int agentId, int serviceId)
                {
                    this.serviceId = serviceId;
                    this.agentId = agentId;
                    if (!IsValidAgentId())
                    {
                        string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid agent ID. Value: {0}.", agentId);
                        throw new System.ArgumentException(message, "agentId");
                    }

                    if (!IsValidServiceId())
                    {
                        string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid element ID. Value: {0}.", serviceId);
                        throw new System.ArgumentException(message, "serviceId");
                    }
                }

                /// <summary>
                /// Gets the DataMiner Agent ID.
                /// </summary>
                /// <remarks>The DataMiner Agent ID is the ID of the DataMiner Agent this service has been created on.</remarks>
                public int AgentId
                {
                    get
                    {
                        return agentId;
                    }

                    private set
                    {
                        // setter for serialization.
                        agentId = value;
                    }
                }

                /// <summary>
                /// Gets the service ID.
                /// </summary>
                public int ServiceId
                {
                    get
                    {
                        return serviceId;
                    }

                    private set
                    {
                        // setter for serialization.
                        serviceId = value;
                    }
                }

                /// <summary>
                /// Gets the DataMiner Agent ID/service ID string representation.
                /// </summary>
                [Newtonsoft.Json.JsonIgnore]
                public string Value
                {
                    get
                    {
                        return agentId + "/" + serviceId;
                    }
                }

                /// <summary>
                /// Compares the current instance with another object of the same type and returns an integer that indicates whether the
                /// current instance precedes, follows, or occurs in the same position in the sort order as the other object.
                /// </summary>
                /// <param name = "other">An object to compare with this instance.</param>
                /// <returns>A value that indicates the relative order of the objects being compared.
                /// The return value has these meanings: Less than zero means this instance precedes <paramref name = "other"/> in the sort order.
                /// Zero means this instance occurs in the same position in the sort order as <paramref name = "other"/>.
                /// Greater than zero means this instance follows <paramref name = "other"/> in the sort order.</returns>
                /// <remarks>The order of the comparison is as follows: DataMiner Agent ID, service ID.</remarks>
                public int CompareTo(Skyline.DataMiner.Library.Common.DmsServiceId other)
                {
                    int result = agentId.CompareTo(other.AgentId);
                    if (result == 0)
                    {
                        result = serviceId.CompareTo(other.ServiceId);
                    }

                    return result;
                }

                /// <summary>
                /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
                /// </summary>
                /// <param name = "obj">An object to compare with this instance.</param>
                /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Less than zero means this instance precedes <paramref name = "obj"/> in the sort order. Zero means this instance occurs in the same position in the sort order as <paramref name = "obj"/>. Greater than zero means this instance follows <paramref name = "obj"/> in the sort order.</returns>
                /// <remarks>The order of the comparison is as follows: DataMiner Agent ID, service ID.</remarks>
                /// <exception cref = "ArgumentException">The obj is not of type <see cref = "DmsServiceId"/></exception>
                public int CompareTo(object obj)
                {
                    if (obj == null)
                    {
                        return 1;
                    }

                    if (!(obj is Skyline.DataMiner.Library.Common.DmsServiceId))
                    {
                        throw new System.ArgumentException("The provided object must be of type DmsServiceId.", "obj");
                    }

                    return CompareTo((Skyline.DataMiner.Library.Common.DmsServiceId)obj);
                }

                /// <summary>
                /// Compares the object to another object.
                /// </summary>
                /// <param name = "obj">The object to compare against.</param>
                /// <returns><c>true</c> if the elements are equal; otherwise, <c>false</c>.</returns>
                public override bool Equals(object obj)
                {
                    if (!(obj is Skyline.DataMiner.Library.Common.DmsServiceId))
                    {
                        return false;
                    }

                    return Equals((Skyline.DataMiner.Library.Common.DmsServiceId)obj);
                }

                /// <summary>
                /// Indicates whether the current object is equal to another object of the same type.
                /// </summary>
                /// <param name = "other">An object to compare with this object.</param>
                /// <returns><c>true</c> if the services are equal; otherwise, <c>false</c>.</returns>
                public bool Equals(Skyline.DataMiner.Library.Common.DmsServiceId other)
                {
                    if (serviceId == other.serviceId && agentId == other.agentId)
                    {
                        return true;
                    }

                    return false;
                }

                /// <summary>
                /// Returns the hash code.
                /// </summary>
                /// <returns>The hash code.</returns>
                public override int GetHashCode()
                {
                    return serviceId ^ agentId;
                }

                /// <summary>
                /// Returns a string that represents the current object.
                /// </summary>
                /// <returns>A string that represents the current object.</returns>
                public override string ToString()
                {
                    return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "agent ID: {0}, service ID: {1}", agentId, serviceId);
                }

                /// <summary>
                /// Returns a value determining whether the agent ID is valid.
                /// </summary>
                /// <returns><c>true</c> if the agent ID is valid; otherwise, <c>false</c>.</returns>
                private bool IsValidAgentId()
                {
                    bool isValid = true;
                    if ((serviceId == -1 && agentId != -1) || agentId < -1)
                    {
                        isValid = false;
                    }

                    return isValid;
                }

                /// <summary>
                /// Returns a value determining whether the element ID is valid.
                /// </summary>
                /// <returns><c>true</c> if the element ID is valid; otherwise, <c>false</c>.</returns>
                private bool IsValidServiceId()
                {
                    bool isValid = true;
                    if ((agentId == -1 && serviceId != -1) || serviceId < -1)
                    {
                        isValid = false;
                    }

                    return isValid;
                }
            }

            /// <summary>
            /// Represents a DataMiner System.
            /// </summary>
            internal class Dms : Skyline.DataMiner.Library.Common.IDms
            {
                /// <summary>
                /// Dictionary for all of the element properties found on the DataMiner System.
                /// </summary>
                private readonly System.Collections.Generic.IDictionary<System.String, Skyline.DataMiner.Library.Common.Properties.IDmsElementPropertyDefinition> elementProperties = new System.Collections.Generic.Dictionary<System.String, Skyline.DataMiner.Library.Common.Properties.IDmsElementPropertyDefinition>();
                /// <summary>
                /// Dictionary for all of the service properties found on the DataMiner System.
                /// </summary>
                private readonly System.Collections.Generic.IDictionary<System.String, Skyline.DataMiner.Library.Common.Properties.IDmsServicePropertyDefinition> serviceProperties = new System.Collections.Generic.Dictionary<System.String, Skyline.DataMiner.Library.Common.Properties.IDmsServicePropertyDefinition>();
                /// <summary>
                /// Dictionary for all of the view properties found on the DataMiner System.
                /// </summary>
                private readonly System.Collections.Generic.IDictionary<System.String, Skyline.DataMiner.Library.Common.Properties.IDmsViewPropertyDefinition> viewProperties = new System.Collections.Generic.Dictionary<System.String, Skyline.DataMiner.Library.Common.Properties.IDmsViewPropertyDefinition>();
                /// <summary>
                /// Specifies is the DataMiner System object has been loaded.
                /// </summary>
                private bool isLoaded;
                /// <summary>
                /// Cached element information message.
                /// </summary>
                private Skyline.DataMiner.Net.Messages.ElementInfoEventMessage cachedElementInfoMessage;
                /// <summary>
                /// Cached view information message.
                /// </summary>
                private Skyline.DataMiner.Net.Messages.ViewInfoEventMessage cachedViewInfoMessage;
                /// <summary>
                /// Cached DataMiner information message.
                /// </summary>
                private Skyline.DataMiner.Net.Messages.GetDataMinerInfoResponseMessage cachedDataMinerAgentMessage;
                /// <summary>
                /// Cached protocol information message.
                /// </summary>
                private Skyline.DataMiner.Net.Messages.GetProtocolInfoResponseMessage cachedProtocolMessage;
                /// <summary>
                /// Cached protocol requested version.
                /// </summary>
                private string cachedProtocolRequestedVersion;
                /// <summary>
                /// The object used for DMS communication.
                /// </summary>
                private Skyline.DataMiner.Library.Common.ICommunication communication;
                /// <summary>
                /// Initializes a new instance of the <see cref = "Dms"/> class.
                /// </summary>
                /// <param name = "communication">An object implementing the ICommunication interface.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "communication"/> is <see langword = "null"/>.</exception>
                internal Dms(Skyline.DataMiner.Library.Common.ICommunication communication)
                {
                    if (communication == null)
                    {
                        throw new System.ArgumentNullException("communication");
                    }

                    this.communication = communication;
                }

                /// <summary>
                /// Gets the communication interface.
                /// </summary>
                /// <value>The communication interface.</value>
                public Skyline.DataMiner.Library.Common.ICommunication Communication
                {
                    get
                    {
                        return communication;
                    }
                }

                /// <summary>
                /// Gets the element property definitions available in the DataMiner System.
                /// </summary>
                /// <value>The element property definitions available in the DataMiner System.</value>
                public Skyline.DataMiner.Library.Common.IPropertyDefinitionCollection<Skyline.DataMiner.Library.Common.Properties.IDmsElementPropertyDefinition> ElementPropertyDefinitions
                {
                    get
                    {
                        if (!isLoaded)
                        {
                            LoadDmsProperties();
                        }

                        return new Skyline.DataMiner.Library.Common.PropertyDefinitionCollection<Skyline.DataMiner.Library.Common.Properties.IDmsElementPropertyDefinition>(elementProperties);
                    }
                }

                /// <summary>
                /// Gets the view property definitions available in the DataMiner System.
                /// </summary>
                /// <value>The view property definitions available in the DataMiner System.</value>
                public Skyline.DataMiner.Library.Common.IPropertyDefinitionCollection<Skyline.DataMiner.Library.Common.Properties.IDmsViewPropertyDefinition> ViewPropertyDefinitions
                {
                    get
                    {
                        if (!isLoaded)
                        {
                            LoadDmsProperties();
                        }

                        return new Skyline.DataMiner.Library.Common.PropertyDefinitionCollection<Skyline.DataMiner.Library.Common.Properties.IDmsViewPropertyDefinition>(viewProperties);
                    }
                }

                /// <summary>
                ///     Gets the service property definitions available in the DataMiner System.
                /// </summary>
                /// <value>The service property definitions available in the DataMiner System.</value>
                public Skyline.DataMiner.Library.Common.IPropertyDefinitionCollection<Skyline.DataMiner.Library.Common.Properties.IDmsServicePropertyDefinition> ServicePropertyDefinitions
                {
                    get
                    {
                        if (!this.isLoaded)
                        {
                            this.LoadDmsProperties();
                        }

                        return new Skyline.DataMiner.Library.Common.PropertyDefinitionCollection<Skyline.DataMiner.Library.Common.Properties.IDmsServicePropertyDefinition>(this.serviceProperties);
                    }
                }

                /// <summary>
                /// Determines whether a DataMiner Agent with the specified ID is present in the DataMiner System.
                /// </summary>
                /// <param name = "agentId">The DataMiner Agent ID.</param>
                /// <exception cref = "ArgumentException"><paramref name = "agentId"/> is invalid.</exception>
                /// <returns><c>true</c> if the DataMiner Agent ID is valid; otherwise, <c>false</c>.</returns>
                public bool AgentExists(int agentId)
                {
                    if (agentId < 1)
                    {
                        throw new System.ArgumentException(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "DataMiner agent ID: {0} is invalid", agentId), "agentId");
                    }

                    try
                    {
                        Skyline.DataMiner.Net.Messages.GetDataMinerByIDMessage message = new Skyline.DataMiner.Net.Messages.GetDataMinerByIDMessage(agentId);
                        cachedDataMinerAgentMessage = communication.SendSingleResponseMessage(message) as Skyline.DataMiner.Net.Messages.GetDataMinerInfoResponseMessage;
                        return cachedDataMinerAgentMessage != null;
                    }
                    catch (Skyline.DataMiner.Net.Exceptions.DataMinerException e)
                    {
                        if (e.ErrorCode == -2146233088)
                        {
                            // 0x80131500, No agent available with ID.
                            return false;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                /// <summary>
                /// Determines whether an element with the specified Agent ID/element ID exists in the DataMiner System.
                /// </summary>
                /// <param name = "dmsElementId">The DataMiner Agent ID/element ID of the element.</param>
                /// <returns><c>true</c> if the element exists; otherwise, <c>false</c>.</returns>
                /// <exception cref = "ArgumentException"><paramref name = "dmsElementId"/> is invalid.</exception>
                public bool ElementExists(Skyline.DataMiner.Library.Common.DmsElementId dmsElementId)
                {
                    int dmaId = dmsElementId.AgentId;
                    int elementId = dmsElementId.ElementId;
                    if (dmaId < 1)
                    {
                        throw new System.ArgumentException(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid DataMiner agent ID: {0}", dmaId), "dmsElementId");
                    }

                    if (elementId < 1)
                    {
                        throw new System.ArgumentException(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid DataMiner element ID: {0}", elementId), "dmsElementId");
                    }

                    try
                    {
                        Skyline.DataMiner.Net.Messages.GetElementByIDMessage message = new Skyline.DataMiner.Net.Messages.GetElementByIDMessage(dmaId, elementId);
                        Skyline.DataMiner.Net.Messages.ElementInfoEventMessage response = (Skyline.DataMiner.Net.Messages.ElementInfoEventMessage)Communication.SendSingleResponseMessage(message);
                        // Cache the response of SLNet.
                        // Could be useful when this call is used within a "GetElement" this makes sure that we do not double call SLNet.
                        if (response != null)
                        {
                            cachedElementInfoMessage = response;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Skyline.DataMiner.Net.Exceptions.DataMinerException e)
                    {
                        if (e.ErrorCode == -2146233088)
                        {
                            // 0x80131500, Element "[element name]" is unavailable.
                            return false;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                /// <summary>
                /// Gets the DataMiner Agents found on the DataMiner System.
                /// </summary>
                /// <returns>The DataMiner Agents in the DataMiner System.</returns>
                public System.Collections.Generic.ICollection<Skyline.DataMiner.Library.Common.IDma> GetAgents()
                {
                    System.Collections.Generic.List<Skyline.DataMiner.Library.Common.IDma> dataMinerAgents = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.IDma>();
                    Skyline.DataMiner.Net.Messages.GetInfoMessage message = new Skyline.DataMiner.Net.Messages.GetInfoMessage{Type = Skyline.DataMiner.Net.Messages.InfoType.DataMinerInfo};
                    Skyline.DataMiner.Net.Messages.DMSMessage[] responses = communication.SendMessage(message);
                    foreach (Skyline.DataMiner.Net.Messages.DMSMessage response in responses)
                    {
                        Skyline.DataMiner.Net.Messages.GetDataMinerInfoResponseMessage info = (Skyline.DataMiner.Net.Messages.GetDataMinerInfoResponseMessage)response;
                        if (info.ID > 0)
                        {
                            dataMinerAgents.Add(new Skyline.DataMiner.Library.Common.Dma(this, info.ID));
                        }
                    }

                    return dataMinerAgents;
                }

                /// <summary>
                /// Retrieves the element with the specified ID.
                /// </summary>
                /// <param name = "dmsElementId">The DataMiner Agent ID/element ID of the element.</param>
                /// <exception cref = "ArgumentException"><paramref name = "dmsElementId"/> is invalid.</exception>
                /// <exception cref = "ElementNotFoundException">The element with the specified ID was not found in the DataMiner System.</exception>
                /// <returns>The element with the specified ID.</returns>
                public Skyline.DataMiner.Library.Common.IDmsElement GetElement(Skyline.DataMiner.Library.Common.DmsElementId dmsElementId)
                {
                    if (!ElementExists(dmsElementId))
                    {
                        throw new Skyline.DataMiner.Library.Common.ElementNotFoundException(dmsElementId);
                    }

                    return new Skyline.DataMiner.Library.Common.DmsElement(this, cachedElementInfoMessage);
                }

                /// <summary>
                /// Retrieves all elements from the DataMiner System.
                /// </summary>
                /// <returns>The elements present on the DataMiner System.</returns>
                public System.Collections.Generic.ICollection<Skyline.DataMiner.Library.Common.IDmsElement> GetElements()
                {
                    Skyline.DataMiner.Net.Messages.GetInfoMessage message = new Skyline.DataMiner.Net.Messages.GetInfoMessage{Type = Skyline.DataMiner.Net.Messages.InfoType.ElementInfo};
                    Skyline.DataMiner.Net.Messages.DMSMessage[] responses = communication.SendMessage(message);
                    System.Collections.Generic.List<Skyline.DataMiner.Library.Common.IDmsElement> elements = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.IDmsElement>();
                    foreach (Skyline.DataMiner.Net.Messages.DMSMessage response in responses)
                    {
                        Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo = (Skyline.DataMiner.Net.Messages.ElementInfoEventMessage)response;
                        if (elementInfo.DataMinerID == -1 || elementInfo.ElementID == -1)
                        {
                            continue;
                        }

                        try
                        {
                            Skyline.DataMiner.Library.Common.DmsElement element = new Skyline.DataMiner.Library.Common.DmsElement(this, elementInfo);
                            elements.Add(element);
                        }
                        catch (System.Exception ex)
                        {
                            string logMessage = "Failed parsing element info for element " + System.Convert.ToString(elementInfo.Name) + " (" + System.Convert.ToString(elementInfo.DataMinerID) + "/" + System.Convert.ToString(elementInfo.ElementID) + ")." + System.Environment.NewLine + ex;
                            Skyline.DataMiner.Library.Common.Logger.Log(logMessage);
                        }
                    }

                    return elements;
                }

                /// <summary>
                /// Retrieves the protocol with the given name and version.
                /// </summary>
                /// <param name = "name">The name of the protocol.</param>
                /// <param name = "version">The version of the protocol.</param>
                /// <returns>An instance of the protocol.</returns>
                /// <exception cref = "ArgumentNullException"><paramref name = "name"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentNullException"><paramref name = "version"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "version"/> is the empty string ("") or white space.</exception>
                /// <exception cref = "ProtocolNotFoundException">No protocol with the specified name and version exists in the DataMiner System.</exception>
                public Skyline.DataMiner.Library.Common.IDmsProtocol GetProtocol(string name, string version)
                {
                    if (name == null)
                    {
                        throw new System.ArgumentNullException("name");
                    }

                    if (version == null)
                    {
                        throw new System.ArgumentNullException("version");
                    }

                    if (System.String.IsNullOrWhiteSpace(name))
                    {
                        throw new System.ArgumentException("The name of the protocol is the empty string (\"\") or white space.", "name");
                    }

                    if (System.String.IsNullOrWhiteSpace(version))
                    {
                        throw new System.ArgumentException("The name of the version is the empty string (\"\") or white space.", "version");
                    }

                    if (!ProtocolExists(name, version))
                    {
                        throw new Skyline.DataMiner.Library.Common.ProtocolNotFoundException(name, version);
                    }

                    return new Skyline.DataMiner.Library.Common.DmsProtocol(this, cachedProtocolMessage, Skyline.DataMiner.Library.Common.DmsProtocol.CheckIsProduction(cachedProtocolRequestedVersion));
                }

                /// <summary>
                /// Retrieves the view with the specified name.
                /// </summary>
                /// <param name = "viewName">The view name.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "viewName"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "viewName"/> is the empty string ("") or white space.</exception>
                /// <exception cref = "ViewNotFoundException">No view with the specified name exists in the DataMiner System.</exception>
                /// <returns>The view with the specified name.</returns>
                public Skyline.DataMiner.Library.Common.IDmsView GetView(string viewName)
                {
                    if (viewName == null)
                    {
                        throw new System.ArgumentNullException("viewName");
                    }

                    if (System.String.IsNullOrWhiteSpace(viewName))
                    {
                        throw new System.ArgumentException("The name of the view is the empty string (\"\") or white space.", "viewName");
                    }

                    if (!ViewExists(viewName))
                    {
                        throw new Skyline.DataMiner.Library.Common.ViewNotFoundException(viewName);
                    }

                    return new Skyline.DataMiner.Library.Common.DmsView(this, cachedViewInfoMessage);
                }

                /// <summary>
                /// Determines whether the specified version of the specified protocol exists.
                /// </summary>
                /// <param name = "protocolName">The protocol name.</param>
                /// <param name = "protocolVersion">The protocol version.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "protocolName"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentNullException"><paramref name = "protocolVersion"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "protocolName"/> is the empty string ("") or white space.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "protocolVersion"/> is the empty string ("") or white space.</exception>
                /// <returns><c>true</c> if the protocol is valid; otherwise, <c>false</c>.</returns>
                public bool ProtocolExists(string protocolName, string protocolVersion)
                {
                    if (protocolName == null)
                    {
                        throw new System.ArgumentNullException("protocolName");
                    }

                    if (protocolVersion == null)
                    {
                        throw new System.ArgumentNullException("protocolVersion");
                    }

                    if (System.String.IsNullOrWhiteSpace(protocolName))
                    {
                        throw new System.ArgumentException("The name of the protocol is the empty string (\"\") or white space.", "protocolName");
                    }

                    if (System.String.IsNullOrWhiteSpace(protocolVersion))
                    {
                        throw new System.ArgumentException("The version of the protocol is the empty string (\"\") or white space.", "protocolVersion");
                    }

                    cachedProtocolRequestedVersion = protocolVersion;
                    Skyline.DataMiner.Net.Messages.GetProtocolMessage message = new Skyline.DataMiner.Net.Messages.GetProtocolMessage{Protocol = protocolName, Version = cachedProtocolRequestedVersion};
                    cachedProtocolMessage = (Skyline.DataMiner.Net.Messages.GetProtocolInfoResponseMessage)communication.SendSingleResponseMessage(message);
                    return cachedProtocolMessage != null;
                }

                /// <summary>
                /// Determines whether the specified property exists in the DataMiner System.
                /// </summary>
                /// <param name = "name">The name of the property.</param>
                /// <param name = "type">Specifies the property type.</param>
                /// <returns>Value indicating whether the specified property exists in the DataMiner System.</returns>
                /// <exception cref = "ArgumentNullException"><paramref name = "name"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "type"/> is invalid.</exception>
                public bool PropertyExists(string name, Skyline.DataMiner.Library.Common.PropertyType type)
                {
                    if (name == null)
                    {
                        throw new System.ArgumentNullException("name");
                    }

                    if (System.String.IsNullOrWhiteSpace(name))
                    {
                        throw new System.ArgumentException("The name of the property is the empty string (\"\") or white space.", "name");
                    }

                    if (type != Skyline.DataMiner.Library.Common.PropertyType.Element && type != Skyline.DataMiner.Library.Common.PropertyType.View && type != Skyline.DataMiner.Library.Common.PropertyType.Service)
                    {
                        throw new System.ArgumentException("Invalid property type specified.", "type");
                    }

                    LoadDmsProperties();
                    switch (type)
                    {
                        case Skyline.DataMiner.Library.Common.PropertyType.Element:
                            try
                            {
                                Skyline.DataMiner.Library.Common.Properties.IDmsPropertyDefinition def = ElementPropertyDefinitions[name];
                                return def != null;
                            }
                            catch (System.ArgumentOutOfRangeException)
                            {
                                return false;
                            }

                        case Skyline.DataMiner.Library.Common.PropertyType.View:
                            try
                            {
                                Skyline.DataMiner.Library.Common.Properties.IDmsViewPropertyDefinition def = ViewPropertyDefinitions[name];
                                return def != null;
                            }
                            catch (System.ArgumentOutOfRangeException)
                            {
                                return false;
                            }

                        case Skyline.DataMiner.Library.Common.PropertyType.Service:
                            try
                            {
                                Skyline.DataMiner.Library.Common.Properties.IDmsServicePropertyDefinition def = ServicePropertyDefinitions[name];
                                return def != null;
                            }
                            catch (System.ArgumentOutOfRangeException)
                            {
                                return false;
                            }

                        default:
                            return false;
                    }
                }

                /// <summary>
                /// Returns a string that represents the current object.
                /// </summary>
                /// <returns>A string that represents the current object.</returns>
                public override string ToString()
                {
                    return "DataMiner System";
                }

                /// <summary>
                /// Determines whether the view with the specified ID exists.
                /// </summary>
                /// <param name = "viewId">The view ID.</param>
                /// <exception cref = "ArgumentException"><paramref name = "viewId"/> is invalid.</exception>
                /// <returns><c>true</c> if the view exists; otherwise, <c>false</c>.</returns>
                public bool ViewExists(int viewId)
                {
                    if (viewId < -1 || viewId == 0)
                    {
                        throw new System.ArgumentException(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid view ID: {0}", viewId), "viewId");
                    }

                    Skyline.DataMiner.Net.Messages.GetInfoMessage message = new Skyline.DataMiner.Net.Messages.GetInfoMessage{Type = Skyline.DataMiner.Net.Messages.InfoType.ViewInfo};
                    Skyline.DataMiner.Net.Messages.DMSMessage[] responses = communication.SendMessage(message);
                    foreach (Skyline.DataMiner.Net.Messages.DMSMessage response in responses)
                    {
                        Skyline.DataMiner.Net.Messages.ViewInfoEventMessage viewInfo = (Skyline.DataMiner.Net.Messages.ViewInfoEventMessage)response;
                        if (viewInfo.ID.Equals(viewId))
                        {
                            cachedViewInfoMessage = viewInfo;
                            return true;
                        }
                    }

                    return false;
                }

                /// <summary>
                /// Determines whether the view with the specified name exists.
                /// </summary>
                /// <param name = "viewName">The view name.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "viewName"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "viewName"/> is the empty string ("") or white space.</exception>
                /// <returns><c>true</c> if the view exists; otherwise, <c>false</c>.</returns>
                public bool ViewExists(string viewName)
                {
                    if (viewName == null)
                    {
                        throw new System.ArgumentNullException("viewName");
                    }

                    if (System.String.IsNullOrWhiteSpace(viewName))
                    {
                        throw new System.ArgumentException("The name of the view is the empty string (\"\") or white space.", "viewName");
                    }

                    Skyline.DataMiner.Net.Messages.GetInfoMessage message = new Skyline.DataMiner.Net.Messages.GetInfoMessage{Type = Skyline.DataMiner.Net.Messages.InfoType.ViewInfo};
                    Skyline.DataMiner.Net.Messages.DMSMessage[] responses = communication.SendMessage(message);
                    foreach (Skyline.DataMiner.Net.Messages.DMSMessage response in responses)
                    {
                        Skyline.DataMiner.Net.Messages.ViewInfoEventMessage viewInfo = (Skyline.DataMiner.Net.Messages.ViewInfoEventMessage)response;
                        if (viewInfo.Name.Equals(viewName, System.StringComparison.OrdinalIgnoreCase))
                        {
                            cachedViewInfoMessage = viewInfo;
                            return true;
                        }
                    }

                    return false;
                }

                /// <summary>
                /// Loads all the properties found on the DataMiner system.
                /// </summary>
                internal void LoadDmsProperties()
                {
                    isLoaded = true;
                    Skyline.DataMiner.Net.Messages.GetInfoMessage message = new Skyline.DataMiner.Net.Messages.GetInfoMessage{Type = Skyline.DataMiner.Net.Messages.InfoType.PropertyConfiguration};
                    Skyline.DataMiner.Net.Messages.GetPropertyConfigurationResponse response = (Skyline.DataMiner.Net.Messages.GetPropertyConfigurationResponse)communication.SendSingleResponseMessage(message);
                    foreach (Skyline.DataMiner.Net.Messages.PropertyConfig property in response.Properties)
                    {
                        switch (property.Type)
                        {
                            case "Element":
                                elementProperties[property.Name] = new Skyline.DataMiner.Library.Common.Properties.DmsElementPropertyDefinition(this, property);
                                break;
                            case "View":
                                viewProperties[property.Name] = new Skyline.DataMiner.Library.Common.Properties.DmsViewPropertyDefinition(this, property);
                                break;
                            case "Service":
                                serviceProperties[property.Name] = new Skyline.DataMiner.Library.Common.Properties.DmsServicePropertyDefinition(this, property);
                                break;
                            default:
                                continue;
                        }
                    }
                }
            }

            /// <summary>
            /// Helper class to convert from enumeration value to string or vice versa.
            /// </summary>
            internal static class EnumMapper
            {
                /// <summary>
                /// The connection type map.
                /// </summary>
                private static readonly System.Collections.Generic.Dictionary<System.String, Skyline.DataMiner.Library.Common.ConnectionType> ConnectionTypeMapping = new System.Collections.Generic.Dictionary<System.String, Skyline.DataMiner.Library.Common.ConnectionType>{{"SNMP", Skyline.DataMiner.Library.Common.ConnectionType.SnmpV1}, {"SNMPV1", Skyline.DataMiner.Library.Common.ConnectionType.SnmpV1}, {"SNMPV2", Skyline.DataMiner.Library.Common.ConnectionType.SnmpV2}, {"SNMPV3", Skyline.DataMiner.Library.Common.ConnectionType.SnmpV3}, {"SERIAL", Skyline.DataMiner.Library.Common.ConnectionType.Serial}, {"SERIAL SINGLE", Skyline.DataMiner.Library.Common.ConnectionType.SerialSingle}, {"SMART-SERIAL", Skyline.DataMiner.Library.Common.ConnectionType.SmartSerial}, {"SMART-SERIAL SINGLE", Skyline.DataMiner.Library.Common.ConnectionType.SmartSerialSingle}, {"HTTP", Skyline.DataMiner.Library.Common.ConnectionType.Http}, {"GPIB", Skyline.DataMiner.Library.Common.ConnectionType.Gpib}, {"VIRTUAL", Skyline.DataMiner.Library.Common.ConnectionType.Virtual}, {"OPC", Skyline.DataMiner.Library.Common.ConnectionType.Opc}, {"SLA", Skyline.DataMiner.Library.Common.ConnectionType.Sla}, {"WEBSOCKET", Skyline.DataMiner.Library.Common.ConnectionType.WebSocket}};
                /// <summary>
                /// Converts a string denoting a connection type to the corresponding value of the <see cref = "ConnectionType"/> enumeration.
                /// </summary>
                /// <param name = "type">The connection type.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "type"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "type"/> is the empty string ("") or white space</exception>
                /// <exception cref = "KeyNotFoundException"></exception>
                /// <returns>The corresponding <see cref = "ConnectionType"/> value.</returns>
                internal static Skyline.DataMiner.Library.Common.ConnectionType ConvertStringToConnectionType(string type)
                {
                    if (type == null)
                    {
                        throw new System.ArgumentNullException("type");
                    }

                    if (System.String.IsNullOrWhiteSpace(type))
                    {
                        throw new System.ArgumentException("The type must not be empty.", "type");
                    }

                    string valueLower = type.ToUpperInvariant();
                    Skyline.DataMiner.Library.Common.ConnectionType result;
                    if (!ConnectionTypeMapping.TryGetValue(valueLower, out result))
                    {
                        throw new System.Collections.Generic.KeyNotFoundException(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "The key {0} could not be found.", valueLower));
                    }

                    return result;
                }
            }

            /// <summary>
            /// Class containing helper methods.
            /// </summary>
            internal static class HelperClass
            {
                /// <summary>
                /// Checks the element state and throws an exception if no operation is possible due to the current element state.
                /// </summary>
                /// <param name = "element">The element on which to check the state.</param>
                /// <exception cref = "ElementNotFoundException">The element was not found in the DataMiner system.</exception>
                /// <exception cref = "ElementStoppedException">The element is stopped.</exception>
                internal static void CheckElementState(Skyline.DataMiner.Library.Common.IDmsElement element)
                {
                    if (element.State == Skyline.DataMiner.Library.Common.ElementState.Deleted)
                    {
                        throw new Skyline.DataMiner.Library.Common.ElementNotFoundException(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Failed to perform an operation on the element: {0} because it has been deleted.", element.Name));
                    }

                    if (element.State == Skyline.DataMiner.Library.Common.ElementState.Stopped)
                    {
                        throw new Skyline.DataMiner.Library.Common.ElementStoppedException(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Failed to perform an operation on the element: {0} because it has been stopped.", element.Name));
                    }
                }

                /// <summary>
                /// Creates an SLNet AddElementMessage.
                /// </summary>
                /// <param name = "elementConfiguration">The element configuration.</param>
                /// <returns>The <see cref = "AddElementMessage"/> message instance.</returns>
                internal static Skyline.DataMiner.Net.Messages.AddElementMessage CreateAddElementMessage(Skyline.DataMiner.Library.Common.ElementConfiguration elementConfiguration, bool isCompatibilityIssueDetected)
                {
                    Skyline.DataMiner.Net.Messages.AddElementMessage addMessage = new Skyline.DataMiner.Net.Messages.AddElementMessage{AlarmTemplate = elementConfiguration.AlarmTemplate != null ? elementConfiguration.AlarmTemplate.Name : System.String.Empty, CreateDVEs = elementConfiguration.DveSettings.IsDveCreationEnabled, Description = elementConfiguration.Description, ElementID = -1, ElementName = elementConfiguration.Name, //ForceAgent = elementConfiguration.FailoverSettings.ForceAgent,
                    IsHidden = elementConfiguration.AdvancedSettings.IsHidden, IsReadOnly = elementConfiguration.AdvancedSettings.IsReadOnly, //KeepOnline = elementConfiguration.FailoverSettings.KeepOnline,
                    ProtocolName = elementConfiguration.Protocol.Name, ProtocolVersion = elementConfiguration.Protocol.Version, State = GetSLNetEnumValue(elementConfiguration.State), TimeoutTime = (int)elementConfiguration.AdvancedSettings.Timeout.TotalMilliseconds, TrendTemplate = elementConfiguration.TrendTemplate != null ? elementConfiguration.TrendTemplate.Name : System.String.Empty, Type = elementConfiguration.Type, };
                    //todo: should be in model
                    addMessage.SlowPollBase = "NO";
                    addMessage.PingInterval = 30000;
                    addMessage.KeepOnline = false;
                    // Build array with view IDs.
                    int viewCount = elementConfiguration.Views.Count;
                    if (elementConfiguration.Views != null && viewCount > 0)
                    {
                        int[] viewIds = new int[viewCount];
                        for (int i = 0; i < viewCount; i++)
                        {
                            viewIds[i] = System.Linq.Enumerable.ElementAt(elementConfiguration.Views, i).Id;
                        }

                        addMessage.ViewIDs = viewIds;
                    }

                    System.Collections.Generic.ICollection<Skyline.DataMiner.Net.Messages.PropertyInfo> propertyInfo = new System.Collections.Generic.List<Skyline.DataMiner.Net.Messages.PropertyInfo>();
                    if (System.Linq.Enumerable.Count(elementConfiguration.UpdatedProperties) != 0)
                    {
                        foreach (string updatedProperty in elementConfiguration.UpdatedProperties)
                        {
                            Skyline.DataMiner.Library.Common.PropertyConfiguration propertyConfiguration = elementConfiguration.Properties[updatedProperty];
                            propertyInfo.Add(new Skyline.DataMiner.Net.Messages.PropertyInfo{DataType = "Element", Name = propertyConfiguration.Definition.Name, Value = propertyConfiguration.Value});
                        }

                        addMessage.Properties = System.Linq.Enumerable.ToArray(propertyInfo);
                    }

                    System.Collections.Generic.IEnumerable<Skyline.DataMiner.Net.Messages.ElementPortInfo> portInfoMessages = elementConfiguration.Connections.CreatePortInfo(isCompatibilityIssueDetected);
                    addMessage.Ports = System.Linq.Enumerable.ToArray(portInfoMessages);
                    return addMessage;
                }

                /// <summary>
                /// Helper method to extract the ElementPortInfo information from an ElementInfoEventMessage object.
                /// </summary>
                /// <param name = "message">The ElementInfoEventMessage</param>
                /// <returns>List of <see cref = "ElementPortInfo"/></returns>
                internal static Skyline.DataMiner.Net.Messages.ElementPortInfo[] ObtainElementPortInfos(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage message)
                {
                    int count = 1;
                    if (message.ExtraPorts != null)
                    {
                        count += message.ExtraPorts.Length;
                    }

                    Skyline.DataMiner.Net.Messages.ElementPortInfo[] allPortInfo = new Skyline.DataMiner.Net.Messages.ElementPortInfo[count];
                    var mainPortInfo = new Skyline.DataMiner.Net.Messages.ElementPortInfo();
                    if (message.MainPort != null)
                    {
                        mainPortInfo.IsSslTlsEnabled = message.MainPort.IsSslTlsEnabled;
                    }

                    allPortInfo[0] = mainPortInfo;
                    if (message.ExtraPorts != null)
                    {
                        for (int i = 0; i < message.ExtraPorts.Length; i++)
                        {
                            var extraPort = new Skyline.DataMiner.Net.Messages.ElementPortInfo();
                            if (message.ExtraPorts[i] != null)
                            {
                                extraPort.IsSslTlsEnabled = message.ExtraPorts[i].IsSslTlsEnabled;
                            }

                            allPortInfo[i + 1] = extraPort;
                        }
                    }

                    return allPortInfo;
                }

                /// <summary>
                /// Gets the corresponding SLNet element state enumeration value.
                /// </summary>
                /// <param name = "state">The element state.</param>
                /// <returns>The corresponding SLNet element state enumeration value.</returns>
                private static Skyline.DataMiner.Net.Messages.ElementState GetSLNetEnumValue(Skyline.DataMiner.Library.Common.ConfigurationElementState state)
                {
                    switch (state)
                    {
                        case Skyline.DataMiner.Library.Common.ConfigurationElementState.Active:
                            return Skyline.DataMiner.Net.Messages.ElementState.Active;
                        case Skyline.DataMiner.Library.Common.ConfigurationElementState.Paused:
                            return Skyline.DataMiner.Net.Messages.ElementState.Paused;
                        case Skyline.DataMiner.Library.Common.ConfigurationElementState.Stopped:
                            return Skyline.DataMiner.Net.Messages.ElementState.Stopped;
                        default:
                            return Skyline.DataMiner.Net.Messages.ElementState.Undefined;
                    }
                }

                /// <summary>
                /// Determines if a connection is using a dedicated connection or not (e.g serial single, smart serial single).
                /// </summary>
                /// <param name = "info">ElementPortInfo</param>
                /// <returns>Whether a connection is marked as single or not.</returns>
                internal static bool IsDedicatedConnection(Skyline.DataMiner.Net.Messages.ElementPortInfo info)
                {
                    bool isDedicatedConnection = false;
                    switch (info.ProtocolType)
                    {
                        case Skyline.DataMiner.Net.Messages.ProtocolType.SerialSingle:
                        case Skyline.DataMiner.Net.Messages.ProtocolType.SmartSerialRawSingle:
                        case Skyline.DataMiner.Net.Messages.ProtocolType.SmartSerialSingle:
                            isDedicatedConnection = true;
                            break;
                        default:
                            isDedicatedConnection = false;
                            break;
                    }

                    return isDedicatedConnection;
                }
            }

            /// <summary>
            ///     DataMiner System interface.
            /// </summary>
            public interface IDms
            {
                /// <summary>
                ///     Gets the communication interface.
                /// </summary>
                /// <value>The communication interface.</value>
                Skyline.DataMiner.Library.Common.ICommunication Communication
                {
                    get;
                }

                /// <summary>
                ///     Gets the element property definitions available in the DataMiner System.
                /// </summary>
                /// <value>The element property definitions available in the DataMiner System.</value>
                Skyline.DataMiner.Library.Common.IPropertyDefinitionCollection<Skyline.DataMiner.Library.Common.Properties.IDmsElementPropertyDefinition> ElementPropertyDefinitions
                {
                    get;
                }

                /// <summary>
                ///     Gets the service property definitions available in the DataMiner System.
                /// </summary>
                /// <value>The service property definitions available in the DataMiner System.</value>
                Skyline.DataMiner.Library.Common.IPropertyDefinitionCollection<Skyline.DataMiner.Library.Common.Properties.IDmsServicePropertyDefinition> ServicePropertyDefinitions
                {
                    get;
                }

                /// <summary>
                ///     Gets the view property definitions available in the DataMiner System.
                /// </summary>
                /// <value>The view property definitions available in the DataMiner System.</value>
                Skyline.DataMiner.Library.Common.IPropertyDefinitionCollection<Skyline.DataMiner.Library.Common.Properties.IDmsViewPropertyDefinition> ViewPropertyDefinitions
                {
                    get;
                }

                /// <summary>
                ///     Determines whether a DataMiner Agent with the specified ID is present in the DataMiner System.
                /// </summary>
                /// <param name = "agentId">The DataMiner Agent ID.</param>
                /// <exception cref = "ArgumentException">The DataMiner Agent ID is negative.</exception>
                /// <returns><c>true</c> if the DataMiner Agent ID is valid; otherwise, <c>false</c>.</returns>
                bool AgentExists(int agentId);
                /// <summary>
                ///     Determines whether an element with the specified DataMiner Agent ID/element ID exists in the DataMiner System.
                /// </summary>
                /// <param name = "dmsElementId">The DataMiner Agent ID/element ID of the element.</param>
                /// <returns><c>true</c> if the element exists; otherwise, <c>false</c>.</returns>
                bool ElementExists(Skyline.DataMiner.Library.Common.DmsElementId dmsElementId);
                /// <summary>
                ///     Gets the DataMiner Agents found in the DataMiner System.
                /// </summary>
                /// <returns>The DataMiner Agents in the DataMiner System.</returns>
                System.Collections.Generic.ICollection<Skyline.DataMiner.Library.Common.IDma> GetAgents();
                /// <summary>
                ///     Retrieves the element with the specified element ID.
                /// </summary>
                /// <param name = "dmsElementId">The DataMiner Agent ID/element ID of the element.</param>
                /// <exception cref = "ArgumentException"><paramref name = "dmsElementId"/> is empty.</exception>
                /// <exception cref = "ElementNotFoundException">No element with the specified ID exists in the DataMiner System.</exception>
                /// <returns>The element with the specified ID.</returns>
                Skyline.DataMiner.Library.Common.IDmsElement GetElement(Skyline.DataMiner.Library.Common.DmsElementId dmsElementId);
                /// <summary>
                ///     Retrieves all elements from the DataMiner System.
                /// </summary>
                /// <returns>The elements present on the DataMiner System.</returns>
                System.Collections.Generic.ICollection<Skyline.DataMiner.Library.Common.IDmsElement> GetElements();
                /// <summary>
                ///     Retrieves the protocol with the given protocol name and version.
                /// </summary>
                /// <param name = "name">The name of the protocol.</param>
                /// <param name = "version">The version of the protocol.</param>
                /// <returns>An instance of the protocol.</returns>
                /// <exception cref = "ArgumentNullException"><paramref name = "name"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentNullException"><paramref name = "version"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "version"/> is the empty string ("") or white space.</exception>
                /// <exception cref = "ProtocolNotFoundException">
                ///     No protocol with the specified name and version exists in the DataMiner
                ///     System.
                /// </exception>
                Skyline.DataMiner.Library.Common.IDmsProtocol GetProtocol(string name, string version);
                /// <summary>
                ///     Retrieves the view with the specified name.
                /// </summary>
                /// <param name = "viewName">The view name.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "viewName"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "viewName"/> is the empty string ("") or white space.</exception>
                /// <exception cref = "ViewNotFoundException">No view with the specified name exists in the DataMiner System.</exception>
                /// <returns>The view with the specified name.</returns>
                Skyline.DataMiner.Library.Common.IDmsView GetView(string viewName);
                /// <summary>
                ///     Determines whether the specified property exists in the DataMiner System.
                /// </summary>
                /// <param name = "name">The name of the property.</param>
                /// <param name = "type">Specifies the property type.</param>
                /// <returns>Value indicating whether the specified property exists in the DataMiner System.</returns>
                /// <exception cref = "ArgumentNullException"><paramref name = "name"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
                bool PropertyExists(string name, Skyline.DataMiner.Library.Common.PropertyType type);
                /// <summary>
                ///     Determines whether the specified version of the specified protocol exists.
                /// </summary>
                /// <param name = "protocolName">The protocol name.</param>
                /// <param name = "protocolVersion">The protocol version.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "protocolName"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentNullException"><paramref name = "protocolVersion"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "protocolName"/> is the empty string ("") or white space.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "protocolVersion"/> is the empty string ("") or white space.</exception>
                /// <returns><c>true</c> if the protocol is valid; otherwise, <c>false</c>.</returns>
                bool ProtocolExists(string protocolName, string protocolVersion);
                /// <summary>
                ///     Determines whether the view with the specified ID exists.
                /// </summary>
                /// <param name = "viewId">The view ID.</param>
                /// <exception cref = "ArgumentException"><paramref name = "viewId"/> is invalid.</exception>
                /// <returns><c>true</c> if the view exists; otherwise, <c>false</c>.</returns>
                bool ViewExists(int viewId);
                /// <summary>
                ///     Determines whether the view with the specified name exists.
                /// </summary>
                /// <param name = "viewName">The view name.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "viewName"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "viewName"/> is the empty string ("") or white space.</exception>
                /// <returns><c>true</c> if the view exists; otherwise, <c>false</c>.</returns>
                bool ViewExists(string viewName);
            }

            /// <summary>
            /// Contains methods for input validation.
            /// </summary>
            internal static class InputValidator
            {
                /// <summary>
                /// Validates the name of an element, service, redundancy group, template or folder.
                /// </summary>
                /// <param name = "name">The element name.</param>
                /// <param name = "parameterName">The name of the parameter that is passing the name.</param>
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation is empty or white space.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation exceeds 200 characters.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation contains a forbidden character.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation contains more than one '%' character.</exception>
                /// <returns><c>true</c> if the name is valid; otherwise, <c>false</c>.</returns>
                /// <remarks>Forbidden characters: '\', '/', ':', '*', '?', '"', '&lt;', '&gt;', '|', '°', ';'.</remarks>
                public static string ValidateName(string name, string parameterName)
                {
                    if (name == null)
                    {
                        throw new System.ArgumentNullException("name");
                    }

                    if (parameterName == null)
                    {
                        throw new System.ArgumentNullException("parameterName");
                    }

                    if (System.String.IsNullOrWhiteSpace(name))
                    {
                        throw new System.ArgumentException("The name must not be null or white space.", parameterName);
                    }

                    string trimmedName = name.Trim();
                    if (trimmedName.Length > 200)
                    {
                        throw new System.ArgumentException("The name must not exceed 200 characters.", parameterName);
                    }

                    // White space is trimmed.
                    if (trimmedName[0].Equals('.'))
                    {
                        throw new System.ArgumentException("The name must not start with a dot ('.').", parameterName);
                    }

                    if (trimmedName[trimmedName.Length - 1].Equals('.'))
                    {
                        throw new System.ArgumentException("The name must not end with a dot ('.').", parameterName);
                    }

                    if (!System.Text.RegularExpressions.Regex.IsMatch(trimmedName, @"^[^/\\:;\*\?<>\|°""]+$"))
                    {
                        throw new System.ArgumentException("The name contains a forbidden character.", parameterName);
                    }

                    if (System.Linq.Enumerable.Count(trimmedName, x => x == '%') > 1)
                    {
                        throw new System.ArgumentException("The name must not contain more than one '%' characters.", parameterName);
                    }

                    return trimmedName;
                }

                /// <summary>
                /// Validates the specified name for a view.
                /// </summary>
                /// <param name = "name">The view name.</param>
                /// <param name = "parameterName">The name of the parameter to which the view name is passed.</param>
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "name"/> is invalid.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation exceeds 200 characters.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation contains a forbidden character.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation contains more than one '%' character.</exception>
                /// <returns>The validated view name.</returns>
                public static string ValidateViewName(string name, string parameterName)
                {
                    if (name == null)
                    {
                        throw new System.ArgumentNullException(parameterName);
                    }

                    if (System.String.IsNullOrWhiteSpace(name))
                    {
                        throw new System.ArgumentException("The name must not be null or white space.", parameterName);
                    }

                    string trimmedName = name.Trim();
                    if (trimmedName.Length > 0)
                    {
                        if (trimmedName.Length > 200)
                        {
                            throw new System.ArgumentException("The name must not exceed 200 characters.", parameterName);
                        }

                        ValidateViewNameForbiddenCharacters(trimmedName, parameterName);
                    }

                    return trimmedName;
                }

                /// <summary>
                /// Determines whether the specified template is compatible with the specified protocol.
                /// </summary>
                /// <param name = "template">The template.</param>
                /// <param name = "protocol">The protocol.</param>
                /// <returns><c>true</c> if the template is compatible with the protocol; otherwise, <c>false</c>.</returns>
                public static bool IsCompatibleTemplate(Skyline.DataMiner.Library.Common.Templates.IDmsTemplate template, Skyline.DataMiner.Library.Common.IDmsProtocol protocol)
                {
                    bool isCompatible = true;
                    if (template != null && (!template.Protocol.Name.Equals(protocol.Name, System.StringComparison.OrdinalIgnoreCase) || !template.Protocol.Version.Equals(protocol.Version, System.StringComparison.OrdinalIgnoreCase)))
                    {
                        isCompatible = false;
                    }

                    return isCompatible;
                }

                /// <summary>
                /// Validates the specified name for a view for forbidden characters.
                /// </summary>
                /// <param name = "viewName">The view name.</param>
                /// <param name = "parameterName">The name of the parameter to which the view name is passed.</param>
                /// <exception cref = "ArgumentException"><paramref name = "viewName"/> is invalid.</exception>
                private static void ValidateViewNameForbiddenCharacters(string viewName, string parameterName)
                {
                    if (viewName[0].Equals('.'))
                    {
                        throw new System.ArgumentException("The name must not start with a dot ('.').", parameterName);
                    }

                    if (viewName[viewName.Length - 1].Equals('.'))
                    {
                        throw new System.ArgumentException("The name must not end with a dot ('.').", parameterName);
                    }

                    if (System.Linq.Enumerable.Contains(viewName, '|'))
                    {
                        throw new System.ArgumentException("The name contains a forbidden character. (Forbidden characters: '|')", parameterName);
                    }

                    if (System.Linq.Enumerable.Count(viewName, x => x == '%') > 1)
                    {
                        throw new System.ArgumentException("The name must not contain more than one '%' characters.", parameterName);
                    }
                }
            }

            /// <summary>
            /// Updateable interface.
            /// </summary>
            public interface IUpdateable
            {
                /// <summary>
                /// Executes the update.
                /// </summary>
                void Update();
            }

            /// <summary>
            /// Represents a DataMiner Agent.
            /// </summary>
            internal class Dma : Skyline.DataMiner.Library.Common.DmsObject, Skyline.DataMiner.Library.Common.IDma
            {
                // Not applicable in current build (kept to not differ a lot from 9.6.3 build and can be used whenever a new compatibility issue needs resolving)
                internal const string SnmpV3AuthenticationChangeDMAVersion = "10.0.3.0";
                /// <summary>
                /// The object used for DMS communication.
                /// </summary>
                private new readonly Skyline.DataMiner.Library.Common.IDms dms;
                /// <summary>
                /// The DataMiner Agent ID.
                /// </summary>
                private readonly int id;
                private string hostName;
                private string name;
                private Skyline.DataMiner.Library.Common.IDmsScheduler scheduler;
                private Skyline.DataMiner.Library.Common.AgentState state;
                private bool stateRetrieved;
                private string versionInfo;
                /// <summary>
                /// Initializes a new instance of the <see cref = "Dma"/> class.
                /// </summary>
                /// <param name = "dms">The DataMiner System.</param>
                /// <param name = "id">The ID of the DataMiner Agent.</param>
                /// <exception cref = "ArgumentNullException">The <see cref = "IDms"/> reference is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException">The DataMiner Agent ID is negative.</exception>
                internal Dma(Skyline.DataMiner.Library.Common.IDms dms, int id): base(dms)
                {
                    if (id < 1)
                    {
                        throw new System.ArgumentException(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid DataMiner agent ID: {0}", id), "id");
                    }

                    this.dms = dms;
                    this.id = id;
                }

                internal Dma(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Net.Messages.GetDataMinerInfoResponseMessage infoMessage): base(dms)
                {
                    if (infoMessage == null)
                    {
                        throw new System.ArgumentNullException("infoMessage");
                    }

                    Parse(infoMessage);
                }

                /// <summary>
                /// Gets the name of the host that is hosting this DataMiner Agent.
                /// </summary>
                /// <value>The name of the host.</value>
                /// <exception cref = "AgentNotFoundException">The Agent was not found in the DataMiner System.</exception>
                public string HostName
                {
                    get
                    {
                        LoadOnDemand();
                        return hostName;
                    }
                }

                /// <summary>
                /// Gets the ID of this DataMiner Agent.
                /// </summary>
                /// <value>The ID of this DataMiner Agent.</value>
                public int Id
                {
                    get
                    {
                        return id;
                    }
                }

                /// <summary>
                /// Gets the name of this DataMiner Agent.
                /// </summary>
                /// <value>The name of this DataMiner Agent.</value>
                /// <exception cref = "AgentNotFoundException">The Agent was not found in the DataMiner System.</exception>
                public string Name
                {
                    get
                    {
                        LoadOnDemand();
                        return name;
                    }
                }

                /// <summary>
                /// Gets the Scheduler component of the DataMiner System.
                /// </summary>
                public Skyline.DataMiner.Library.Common.IDmsScheduler Scheduler
                {
                    get
                    {
                        LoadOnDemand();
                        return scheduler;
                    }
                }

                /// <summary>
                /// Gets the state of this DataMiner Agent.
                /// </summary>
                /// <value>The state of this DataMiner Agent.</value>
                /// <exception cref = "AgentNotFoundException">The Agent was not found in the DataMiner System.</exception>
                public Skyline.DataMiner.Library.Common.AgentState State
                {
                    get
                    {
                        if (!stateRetrieved)
                        {
                            try
                            {
                                Skyline.DataMiner.Net.Messages.GetDataMinerStateMessage message = new Skyline.DataMiner.Net.Messages.GetDataMinerStateMessage(id);
                                var response = dms.Communication.SendSingleResponseMessage(message) as Skyline.DataMiner.Net.Messages.GetDataMinerStateResponseMessage;
                                if (response != null)
                                {
                                    stateRetrieved = true;
                                    state = (Skyline.DataMiner.Library.Common.AgentState)response.State;
                                }
                            }
                            catch (Skyline.DataMiner.Net.Exceptions.DataMinerCommunicationException e)
                            {
                                if (e.ErrorCode == -2147220787)
                                {
                                    // 0x800402CD No connection.
                                    throw new Skyline.DataMiner.Library.Common.AgentNotFoundException(id);
                                }
                            }
                        }

                        return state;
                    }
                }

                /// <summary>
                /// Gets the version information of the DataMiner Agent.
                /// </summary>
                public string VersionInfo
                {
                    get
                    {
                        LoadOnDemand();
                        return versionInfo;
                    }
                }

                /// <summary>
                /// Creates a new element with the specified configuration.
                /// </summary>
                /// <param name = "configuration">The configuration of the element to be created.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "configuration"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "IncorrectDataException">The provided configuration is invalid.</exception>
                /// <returns>The ID of the created element.</returns>
                public Skyline.DataMiner.Library.Common.DmsElementId CreateElement(Skyline.DataMiner.Library.Common.ElementConfiguration configuration)
                {
                    if (configuration == null)
                    {
                        throw new System.ArgumentNullException("configuration");
                    }

                    bool valid = configuration.Connections.ValidateConnectionTypes();
                    if (valid)
                    {
                        try
                        {
                            bool isCompatibilityIssueDetected = IsVersionHigher(SnmpV3AuthenticationChangeDMAVersion);
                            Skyline.DataMiner.Net.Messages.AddElementMessage createMessage = Skyline.DataMiner.Library.Common.HelperClass.CreateAddElementMessage(configuration, isCompatibilityIssueDetected);
                            createMessage.DataMinerID = id;
                            Skyline.DataMiner.Net.Messages.AddElementResponseMessage createResponse = (Skyline.DataMiner.Net.Messages.AddElementResponseMessage)dms.Communication.SendSingleResponseMessage(createMessage);
                            int elementId = createResponse.NewID;
                            return new Skyline.DataMiner.Library.Common.DmsElementId(id, elementId);
                        }
                        catch (Skyline.DataMiner.Net.Exceptions.DataMinerException e)
                        {
                            if (e.ErrorCode == -2147220959)
                            {
                                // 0x80040221, SL_INVALID_DATA, "Invalid data".
                                string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid data: '{0}'", configuration);
                                throw new Skyline.DataMiner.Library.Common.IncorrectDataException(message);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                    else
                    {
                        throw new Skyline.DataMiner.Library.Common.IncorrectDataException("Invalid Element Connections provided in ElementConfiguration object.");
                    }
                }

                /// <summary>
                /// Determines whether this DataMiner Agent exists in the DataMiner System.
                /// </summary>
                /// <returns><c>true</c> if the DataMiner Agent exists in the DataMiner System; otherwise, <c>false</c>.</returns>
                public override bool Exists()
                {
                    return dms.AgentExists(id);
                }

                /// <summary>
                /// Retrieves the element with the specified the DataMiner Agent ID and element ID from this DataMiner Agent.
                /// </summary>
                /// <param name = "dmsElementId">The DataMiner Agent ID/element ID of the element.</param>
                /// <exception cref = "ArgumentException"><paramref name = "dmsElementId"/> is invalid.</exception>
                /// <exception cref = "ElementNotFoundException">The element was not found on this DataMiner Agent.</exception>
                /// <returns>The element with the specified DataMiner Agent ID and element ID.</returns>
                public Skyline.DataMiner.Library.Common.IDmsElement GetElement(Skyline.DataMiner.Library.Common.DmsElementId dmsElementId)
                {
                    Skyline.DataMiner.Library.Common.IDmsElement element = dms.GetElement(dmsElementId);
                    if (element.Host.Id != id)
                    {
                        throw new Skyline.DataMiner.Library.Common.ElementNotFoundException(dmsElementId);
                    }

                    return element;
                }

                /// <summary>
                /// Verifies if the provided version number is higher then the DataMiner Agent version.
                /// </summary>
                /// <param name = "versionNumber">The version number to compare against the version of the DMA.</param>
                /// <returns><c>true</c> if the DataMiner Agent is higher than the specified version number; otherwise, <c>false</c>.</returns>
                public bool IsVersionHigher(string versionNumber)
                {
                    bool isHigher = false;
                    LoadOnDemand();
                    System.Int32[] dmaVersionParts = ParseVersionNumbers(versionInfo);
                    System.Int32[] versionParts = ParseVersionNumbers(versionNumber);
                    for (int i = 0; i < 4; i++)
                    {
                        int dmaNumber = dmaVersionParts[i];
                        int inputNumber = versionParts[i];
                        if (inputNumber > dmaNumber)
                        {
                            isHigher = true;
                            return isHigher;
                        }
                    }

                    return isHigher;
                }

                /// <summary>
                /// Returns a string that represents the current object.
                /// </summary>
                /// <returns>A string that represents the current object.</returns>
                public override string ToString()
                {
                    return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "DataMiner agent ID: {0}", id);
                }

                internal override void Load()
                {
                    try
                    {
                        Skyline.DataMiner.Net.Messages.GetDataMinerByIDMessage message = new Skyline.DataMiner.Net.Messages.GetDataMinerByIDMessage(id);
                        Skyline.DataMiner.Net.Messages.GetDataMinerInfoResponseMessage infoResponseMessage = Dms.Communication.SendSingleResponseMessage(message) as Skyline.DataMiner.Net.Messages.GetDataMinerInfoResponseMessage;
                        if (infoResponseMessage != null)
                        {
                            Parse(infoResponseMessage);
                        }
                        else
                        {
                            throw new Skyline.DataMiner.Library.Common.AgentNotFoundException(id);
                        }

                        Skyline.DataMiner.Net.Messages.GetAgentBuildInfo buildInfoMessage = new Skyline.DataMiner.Net.Messages.GetAgentBuildInfo(id);
                        Skyline.DataMiner.Net.Messages.BuildInfoResponse buildInfoResponse = (Skyline.DataMiner.Net.Messages.BuildInfoResponse)Dms.Communication.SendSingleResponseMessage(buildInfoMessage);
                        if (buildInfoResponse != null)
                        {
                            Parse(buildInfoResponse);
                        }

                        Skyline.DataMiner.Net.Messages.RSAPublicKeyRequest rsapkr;
                        rsapkr = new Skyline.DataMiner.Net.Messages.RSAPublicKeyRequest(id)
                        {HostingDataMinerID = id};
                        Skyline.DataMiner.Net.Messages.RSAPublicKeyResponse resp = Dms.Communication.SendSingleResponseMessage(rsapkr) as Skyline.DataMiner.Net.Messages.RSAPublicKeyResponse;
                        Skyline.DataMiner.Library.Common.RSA.PublicKey = new System.Security.Cryptography.RSAParameters{Modulus = resp.Modulus, Exponent = resp.Exponent};
                        scheduler = new Skyline.DataMiner.Library.Common.DmsScheduler(this);
                        IsLoaded = true;
                    }
                    catch (Skyline.DataMiner.Net.Exceptions.DataMinerException e)
                    {
                        if (e.ErrorCode == -2146233088)
                        {
                            // 0x80131500, No agent available with ID.
                            throw new Skyline.DataMiner.Library.Common.AgentNotFoundException(id);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                /// <summary>
                /// Parses the version number string into a string array.
                /// </summary>
                /// <param name = "versionNumber">The version number.</param>
                /// <returns>String array containing the parsed version number.</returns>
                /// <exception cref = "ArgumentException">When the version number is not in the expected format of a.b.c.d where a,b,c and d are integers.</exception>
                private static System.Int32[] ParseVersionNumbers(string versionNumber)
                {
                    string[] splitDot = new[]{"."};
                    string[] versionParts = versionNumber.Split(splitDot, System.StringSplitOptions.None);
                    if (versionParts.Length != 4)
                    {
                        throw new System.ArgumentException("versionNumber is not in expected format.");
                    }

                    int versionPartMajor = 0;
                    int versionPartMinor = 0;
                    int versionPartMonth = 0;
                    int versionPartWeek = 0;
                    if (!System.Int32.TryParse(versionParts[0], out versionPartMajor) || !System.Int32.TryParse(versionParts[2], out versionPartMonth) || !System.Int32.TryParse(versionParts[1], out versionPartMinor) || !System.Int32.TryParse(versionParts[3], out versionPartWeek))
                    {
                        throw new System.ArgumentException("versionNumber is not in expected format.");
                    }

                    System.Int32[] versionPartNumbers = new System.Int32[4];
                    versionPartNumbers[0] = versionPartMajor;
                    versionPartNumbers[1] = versionPartMinor;
                    versionPartNumbers[2] = versionPartMonth;
                    versionPartNumbers[3] = versionPartWeek;
                    return versionPartNumbers;
                }

                private void Parse(Skyline.DataMiner.Net.Messages.GetDataMinerInfoResponseMessage infoMessage)
                {
                    name = infoMessage.AgentName;
                    hostName = infoMessage.ComputerName;
                }

                /// <summary>
                /// Parses the version information of the DataMiner Agent.
                /// </summary>
                /// <param name = "response">The response message.</param>
                private void Parse(Skyline.DataMiner.Net.Messages.BuildInfoResponse response)
                {
                    if (response == null || response.Agents == null || response.Agents.Length == 0)
                    {
                        throw new System.ArgumentException("Agent build information cannot be null or empty");
                    }

                    string rawVersion = response.Agents[0].RawVersion;
                    this.versionInfo = rawVersion;
                }
            }

            /// <summary>
            /// DataMiner Agent interface.
            /// </summary>
            public interface IDma
            {
                /// <summary>
                /// Gets the DataMiner System this Agent is part of.
                /// </summary>
                /// <value>The DataMiner system this Agent is part of.</value>
                Skyline.DataMiner.Library.Common.IDms Dms
                {
                    get;
                }

                /// <summary>
                /// Gets the name of the host of this DataMiner Agent.
                /// </summary>
                /// <value>The name of the host of this DataMiner Agent.</value>
                /// <exception cref = "AgentNotFoundException">The Agent was not found in the DataMiner System.</exception>
                string HostName
                {
                    get;
                }

                /// <summary>
                /// Gets the ID of this DataMiner Agent.
                /// </summary>
                /// <value>The ID of this DataMiner Agent.</value>
                int Id
                {
                    get;
                }

                /// <summary>
                /// Gets the name of this DataMiner Agent.
                /// </summary>
                /// <value>The name of this DataMiner Agent.</value>
                /// <exception cref = "AgentNotFoundException">The Agent was not found in the DataMiner System.</exception>
                string Name
                {
                    get;
                }

                /// <summary>
                /// Gets the Scheduler component of the DataMiner System.
                /// </summary>
                Skyline.DataMiner.Library.Common.IDmsScheduler Scheduler
                {
                    get;
                }

                /// <summary>
                /// Gets the state of this DataMiner Agent.
                /// </summary>
                /// <value>The state of this DataMiner Agent.</value>
                /// <exception cref = "AgentNotFoundException">The Agent was not found in the DataMiner System.</exception>
                Skyline.DataMiner.Library.Common.AgentState State
                {
                    get;
                }

                /// <summary>
                /// Gets the version info of this DataMiner Agent.
                /// </summary>
                string VersionInfo
                {
                    get;
                }

                /// <summary>
                /// Creates a new element with the specified configuration.
                /// </summary>
                /// <param name = "configuration">The configuration of the element to be created.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "configuration"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "IncorrectDataException">The provided configuration is invalid.</exception>
                /// <returns>The ID of the created element.</returns>
                /// <remarks>Currently, this method only supports creating virtual elements.</remarks>
                Skyline.DataMiner.Library.Common.DmsElementId CreateElement(Skyline.DataMiner.Library.Common.ElementConfiguration configuration);
                /// <summary>
                /// Determines whether this DataMiner Agent exists in the DataMiner System.
                /// </summary>
                /// <returns><c>true</c> if the DataMiner Agent exists in the DataMiner System; otherwise, <c>false</c>.</returns>
                bool Exists();
                /// <summary>
                /// Retrieves the element with the specified the DataMiner Agent ID and element ID from this DataMiner Agent.
                /// </summary>
                /// <param name = "dmsElementId">The Agent ID/element ID of the element.</param>
                /// <exception cref = "ArgumentException"><paramref name = "dmsElementId"/> is invalid.</exception>
                /// <exception cref = "ElementNotFoundException">The element was not found on this DataMiner Agent.</exception>
                /// <returns>The element with the specified DataMiner Agent ID and element ID.</returns>
                Skyline.DataMiner.Library.Common.IDmsElement GetElement(Skyline.DataMiner.Library.Common.DmsElementId dmsElementId);
                /// <summary>
                /// Verifies if the provided version number is higher then the DataMiner Agent version.
                /// </summary>
                /// <param name = "versionNumber">The version number to compare against the version of the DMA.</param>
                /// <returns><c>true</c> if the version of the DMA is higher than wat is ; otherwise, <c>false</c>.</returns>
                bool IsVersionHigher(string versionNumber);
            }

            /// <summary>
            /// Represents a communication interface implementation using the <see cref = "IConnection"/> interface.
            /// </summary>
            internal class ConnectionCommunication : Skyline.DataMiner.Library.Common.ICommunication
            {
                /// <summary>
                /// The SLNet connection.
                /// </summary>
                private readonly Skyline.DataMiner.Net.IConnection connection;
                /// <summary>
                /// Initializes a new instance of the <see cref = "ConnectionCommunication"/> class using an instance of the <see cref = "IConnection"/> class.
                /// </summary>
                /// <param name = "connection">The connection.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "connection"/> is <see langword = "null"/>.</exception>
                public ConnectionCommunication(Skyline.DataMiner.Net.IConnection connection)
                {
                    if (connection == null)
                    {
                        throw new System.ArgumentNullException("connection");
                    }

                    this.connection = connection;
                }

                /// <summary>
                /// Sends a message to the SLNet process.
                /// </summary>
                /// <param name = "message">The message to be sent.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "message"/> is <see langword = "null"/>.</exception>
                /// <returns>The message responses.</returns>
                public Skyline.DataMiner.Net.Messages.DMSMessage[] SendMessage(Skyline.DataMiner.Net.Messages.DMSMessage message)
                {
                    if (message == null)
                    {
                        throw new System.ArgumentNullException("message");
                    }

                    return connection.HandleMessage(message);
                }

                /// <summary>
                /// Sends a message to the SLNet process.
                /// </summary>
                /// <param name = "message">The message to be sent.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "message"/> is <see langword = "null"/>.</exception>
                /// <returns>The message response.</returns>
                public Skyline.DataMiner.Net.Messages.DMSMessage SendSingleResponseMessage(Skyline.DataMiner.Net.Messages.DMSMessage message)
                {
                    if (message == null)
                    {
                        throw new System.ArgumentNullException("message");
                    }

                    return connection.HandleSingleResponseMessage(message);
                }
            }

            /// <summary>
            /// Defines methods to send messages to a DataMiner System.
            /// </summary>
            public interface ICommunication
            {
                /// <summary>
                /// Sends a message to the SLNet process that can have multiple responses.
                /// </summary>
                /// <param name = "message">The message to be sent.</param>
                /// <exception cref = "ArgumentNullException">The message cannot be null.</exception>
                /// <returns>The message responses.</returns>
                Skyline.DataMiner.Net.Messages.DMSMessage[] SendMessage(Skyline.DataMiner.Net.Messages.DMSMessage message);
                /// <summary>
                /// Sends a message to the SLNet process that returns a single response.
                /// </summary>
                /// <param name = "message">The message to be sent.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "message"/> is <see langword = "null"/>.</exception>
                /// <returns>The message response.</returns>
                Skyline.DataMiner.Net.Messages.DMSMessage SendSingleResponseMessage(Skyline.DataMiner.Net.Messages.DMSMessage message);
            }

            /// <summary>
            /// Defines methods to support the comparison of DataMiner views for equality.
            /// </summary>
            public class DmsViewEqualityComparer : System.Collections.Generic.EqualityComparer<Skyline.DataMiner.Library.Common.IDmsView>
            {
                /// <summary>
                /// Determines whether the specified view objects are equal.
                /// </summary>
                /// <param name = "x">The first object to compare.</param>
                /// <param name = "y">The second object to compare.</param>
                /// <returns><c>true</c> if the specified views have the same ID; otherwise, <c>false</c>.</returns>
                public override bool Equals(Skyline.DataMiner.Library.Common.IDmsView x, Skyline.DataMiner.Library.Common.IDmsView y)
                {
                    if (x == null && y == null)
                    {
                        return true;
                    }

                    if (x == null || y == null)
                    {
                        return false;
                    }

                    return x.Id == y.Id;
                }

                /// <summary>
                /// Returns a hash code for the specified object.
                /// </summary>
                /// <param name = "obj">The object for which to get a hash code.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "obj"/> is <see langword = "null"/>.</exception>
                /// <returns>A hash code for the specified object.</returns>
                public override int GetHashCode(Skyline.DataMiner.Library.Common.IDmsView obj)
                {
                    if (obj == null)
                    {
                        throw new System.ArgumentNullException("obj");
                    }

                    return obj.Id.GetHashCode();
                }
            }

            /// <summary>
            /// Represents an element configuration.
            /// </summary>
            public class AdvancedElementConfiguration
            {
                /// <summary>
                /// The default timeout time of an element.
                /// </summary>
                private System.TimeSpan timespan = new System.TimeSpan(0, 0, 30);
                /// <summary>
                /// Gets or sets a value indicating whether the element is hidden.
                /// </summary>
                public bool IsHidden
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets a value indicating whether the element is read-only.
                /// </summary>
                public bool IsReadOnly
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the timeout.
                /// </summary>
                /// <exception cref = "ArgumentOutOfRangeException">The value of a set operation is not within the range of [0,120] s.</exception>
                public System.TimeSpan Timeout
                {
                    get
                    {
                        return timespan;
                    }

                    set
                    {
                        int timeoutInSeconds = (int)value.TotalSeconds;
                        if (timeoutInSeconds < 0 || timeoutInSeconds > 120)
                        {
                            throw new System.ArgumentOutOfRangeException("value", "The timeout value must be in the range of [0,120] s.");
                        }

                        timespan = value;
                    }
                }

                /// <summary>
                /// Returns a string that represents the current object.
                /// </summary>
                /// <returns>A string that represents the current object.</returns>
                public override string ToString()
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "IsHidden: {0}{1}", IsHidden, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "IsReadOnly: {0}{1}", IsReadOnly, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Timeout: {0}{1}", Timeout, System.Environment.NewLine);
                    return sb.ToString();
                }
            }

            /// <summary>
            /// Represents a service configuration.
            /// </summary>
            public class AdvancedServiceConfiguration
            {
                /// <summary>
                /// Instance of the protocol for enhanced services.
                /// </summary>
                private Skyline.DataMiner.Library.Common.IDmsProtocol protocol;
                /// <summary>
                /// The alarm template assigned to the element for enhanced services.
                /// </summary>
                private Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate alarmTemplate;
                /// <summary>
                /// The trend template assigned to the element for enhanced services.
                /// </summary>
                private Skyline.DataMiner.Library.Common.Templates.IDmsTrendTemplate trendTemplate;
                /// <summary>
                /// Gets or sets the protocol for enhanced services.
                /// </summary>
                public Skyline.DataMiner.Library.Common.IDmsProtocol Protocol
                {
                    get
                    {
                        return this.protocol;
                    }

                    set
                    {
                        if (alarmTemplate != null && !Skyline.DataMiner.Library.Common.InputValidator.IsCompatibleTemplate(alarmTemplate, value))
                        {
                            alarmTemplate = null;
                        }

                        if (trendTemplate != null && !Skyline.DataMiner.Library.Common.InputValidator.IsCompatibleTemplate(trendTemplate, value))
                        {
                            trendTemplate = null;
                        }

                        if (value.Type != Skyline.DataMiner.Library.Common.ProtocolType.Service)
                        {
                            throw new System.ArgumentException("The specified protocol is not compatible with services.", "value");
                        }

                        this.protocol = value;
                    }
                }

                /// <summary>
                /// Gets or sets a value indicating whether the timeouts of the linked element will be included in the service.
                /// </summary>
                public bool IgnoreTimeouts
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the alarm template for enhanced service.
                /// </summary>
                /// <exception cref = "ArgumentException">The value of a set operation is an alarm template that is not compatible with the protocol of the enhanced service.</exception>
                public Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate AlarmTemplate
                {
                    get
                    {
                        return alarmTemplate;
                    }

                    set
                    {
                        if (!Skyline.DataMiner.Library.Common.InputValidator.IsCompatibleTemplate(value, this.Protocol))
                        {
                            throw new System.ArgumentException("The specified alarm template is not compatible with the protocol this enhnaced service executes.", "value");
                        }
                        else
                        {
                            this.alarmTemplate = value;
                        }
                    }
                }

                /// <summary>
                /// Gets or sets the trend template for the enhanced service.
                /// </summary>
                /// <exception cref = "ArgumentException">The value of a set operation is a trend template that is not compatible with the protocol of the enhanced service.</exception>
                public Skyline.DataMiner.Library.Common.Templates.IDmsTrendTemplate TrendTemplate
                {
                    get
                    {
                        return trendTemplate;
                    }

                    set
                    {
                        if (!Skyline.DataMiner.Library.Common.InputValidator.IsCompatibleTemplate(value, this.Protocol))
                        {
                            throw new System.ArgumentException("The specified trend template is not compatible with the protocol this enhanced service executes.", "value");
                        }
                        else
                        {
                            trendTemplate = value;
                        }
                    }
                }

                /// <summary>
                /// Returns a string that represents the current object.
                /// </summary>
                /// <returns>A string that represents the current object.</returns>
                public override string ToString()
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "IncludesTimeout: {0}{1}", IgnoreTimeouts, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Protocol: {0}{1}", protocol == null ? "<<NULL>>" : protocol.Name, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Version: {0}{1}", protocol == null ? "<<NULL>>" : protocol.Version, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "AlarmTemplate: {0}{1}", alarmTemplate == null ? "<<NULL>>" : alarmTemplate.Name, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "TrendTemplate: {0}{1}", trendTemplate == null ? "<<NULL>>" : trendTemplate.Name, System.Environment.NewLine);
                    return sb.ToString();
                }
            }

            /// <summary>
            /// Represents an element configuration.
            /// </summary>
            public class DveElementConfiguration
            {
                /// <summary>
                /// Initializes a new instance of the <see cref = "DveElementConfiguration"/> class.
                /// </summary>
                public DveElementConfiguration()
                {
                    IsDveCreationEnabled = true;
                }

                /// <summary>
                /// Gets or sets a value indicating whether the creation of DVEs is enabled.
                /// </summary>
                public bool IsDveCreationEnabled
                {
                    get;
                    set;
                }

                /// <summary>
                /// Returns a string that represents the current object.
                /// </summary>
                /// <returns>A string that represents the current object.</returns>
                public override string ToString()
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "IsDveCreationEnabled: {0}{1}", IsDveCreationEnabled, System.Environment.NewLine);
                    return sb.ToString();
                }
            }

            /// <summary>
            /// Represents an element configuration.
            /// </summary>
            public class ElementConfiguration
            {
                /// <summary>
                /// The advanced configuration options.
                /// </summary>
                private readonly Skyline.DataMiner.Library.Common.AdvancedElementConfiguration advancedConfiguration = new Skyline.DataMiner.Library.Common.AdvancedElementConfiguration();
                /// <summary>
                /// The IDms class.
                /// </summary>
                private readonly Skyline.DataMiner.Library.Common.IDms dms;
                /// <summary>
                /// The DVE configuration options.
                /// </summary>
                private readonly Skyline.DataMiner.Library.Common.DveElementConfiguration dveConfiguration = new Skyline.DataMiner.Library.Common.DveElementConfiguration();
                /// <summary>
                /// The properties to be added to the element.
                /// </summary>
                private readonly System.Collections.Generic.IDictionary<System.String, Skyline.DataMiner.Library.Common.PropertyConfiguration> properties = new System.Collections.Generic.Dictionary<System.String, Skyline.DataMiner.Library.Common.PropertyConfiguration>();
                /// <summary>
                /// Instance of the protocol this element executes.
                /// </summary>
                private readonly Skyline.DataMiner.Library.Common.IDmsProtocol protocol;
                /// <summary>
                /// Keeps track of which properties where updated.
                /// </summary>
                private readonly System.Collections.Generic.HashSet<System.String> updatedProperties = new System.Collections.Generic.HashSet<System.String>();
                /// <summary>
                /// The view containing the element.
                /// </summary>
                private readonly System.Collections.Generic.ICollection<Skyline.DataMiner.Library.Common.IDmsView> views = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.IDmsView>();
                /// <summary>
                /// The alarm template assigned to this element.
                /// </summary>
                private Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate alarmTemplate;
                /// <summary>
                /// Element description.
                /// </summary>
                private string description = System.String.Empty;
                /// <summary>
                /// The name of the element.
                /// </summary>
                private string name;
                /// <summary>
                /// Specific whether or not the properties where loaded.
                /// </summary>
                private bool propertiesLoaded;
                /// <summary>
                /// The element state.
                /// </summary>
                private Skyline.DataMiner.Library.Common.ConfigurationElementState state = Skyline.DataMiner.Library.Common.ConfigurationElementState.Active;
                /// <summary>
                /// The trend template assigned to this element.
                /// </summary>
                private Skyline.DataMiner.Library.Common.Templates.IDmsTrendTemplate trendTemplate;
                /// <summary>
                /// The type of the element.
                /// </summary>
                private string type = System.String.Empty;
                /// <summary>
                /// The connections available to this element.
                /// </summary>
                private Skyline.DataMiner.Library.Common.ElementConnectionCollection connections;
                /// <summary>
                /// Initializes a new instance of the <see cref = "ElementConfiguration"/> class and defines by default a virtual connection.
                /// </summary>
                /// <param name = "dms">The <see cref = "IDms"/> interface.</param>
                /// <param name = "elementName">The element name.</param>
                /// <param name = "protocol">The protocol.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "elementName"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentNullException"><paramref name = "protocol"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "elementName"/> is empty or white space.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "elementName"/> exceeds 200 characters.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "elementName"/> contains a forbidden character.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "elementName"/> contains more than one '%' character.</exception>
                /// <remarks>Forbidden characters: '\', '/', ':', '*', '?', '"', '&lt;', '&gt;', '|', '°', ';'.</remarks>
                public ElementConfiguration(Skyline.DataMiner.Library.Common.IDms dms, string elementName, Skyline.DataMiner.Library.Common.IDmsProtocol protocol)
                {
                    if (dms == null)
                    {
                        throw new System.ArgumentNullException("dms");
                    }

                    if (protocol == null)
                    {
                        throw new System.ArgumentNullException("protocol");
                    }

                    Name = elementName;
                    this.dms = dms;
                    this.protocol = protocol;
                    this.connections = new Skyline.DataMiner.Library.Common.ElementConnectionCollection(protocol.ConnectionInfo);
                    Skyline.DataMiner.Library.Common.IVirtualConnection myVirtualConnection = new Skyline.DataMiner.Library.Common.VirtualConnection();
                    Connections[0] = myVirtualConnection;
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ElementConfiguration"/> class.
                /// </summary>
                /// <param name = "dms">The <see cref = "IDms"/> interface.</param>
                /// <param name = "elementName">The element name.</param>
                /// <param name = "protocol">The protocol.</param>
                /// <param name = "connectionInfo">The connections that will be used to create the element.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "elementName"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentNullException"><paramref name = "protocol"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "elementName"/> is empty or white space.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "elementName"/> exceeds 200 characters.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "elementName"/> contains a forbidden character.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "elementName"/> contains more than one '%' character.</exception>
                /// <remarks>Forbidden characters: '\', '/', ':', '*', '?', '"', '&lt;', '&gt;', '|', '°', ';'.</remarks>
                public ElementConfiguration(Skyline.DataMiner.Library.Common.IDms dms, string elementName, Skyline.DataMiner.Library.Common.IDmsProtocol protocol, System.Collections.Generic.IEnumerable<Skyline.DataMiner.Library.Common.IElementConnection> connectionInfo)
                {
                    if (dms == null)
                    {
                        throw new System.ArgumentNullException("dms");
                    }

                    if (protocol == null)
                    {
                        throw new System.ArgumentNullException("protocol");
                    }

                    Name = elementName;
                    this.dms = dms;
                    this.protocol = protocol;
                    this.connections = new Skyline.DataMiner.Library.Common.ElementConnectionCollection(protocol.ConnectionInfo);
                    int i = 0;
                    foreach (Skyline.DataMiner.Library.Common.IElementConnection conn in connectionInfo)
                    {
                        this.connections[i] = conn;
                        i++;
                    }
                }

                /// <summary>
                /// Gets the advanced settings.
                /// </summary>
                public Skyline.DataMiner.Library.Common.AdvancedElementConfiguration AdvancedSettings
                {
                    get
                    {
                        return advancedConfiguration;
                    }
                }

                /// <summary>
                /// Gets or sets the alarm template.
                /// </summary>
                /// <exception cref = "ArgumentException">The value of a set operation is an alarm template that is not compatible with the protocol of the element.</exception>
                public Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate AlarmTemplate
                {
                    get
                    {
                        return alarmTemplate;
                    }

                    set
                    {
                        if (!Skyline.DataMiner.Library.Common.InputValidator.IsCompatibleTemplate(value, this.Protocol))
                        {
                            throw new System.ArgumentException("The specified alarm template is not compatible with the protocol this element executes.", "value");
                        }
                        else
                        {
                            this.alarmTemplate = value;
                        }
                    }
                }

                /// <summary>
                /// Gets or sets the description.
                /// </summary>
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                public string Description
                {
                    get
                    {
                        return description;
                    }

                    set
                    {
                        if (value == null)
                        {
                            throw new System.ArgumentNullException("value");
                        }

                        description = value;
                    }
                }

                /// <summary>
                /// Gets the DVE settings.
                /// </summary>
                public Skyline.DataMiner.Library.Common.DveElementConfiguration DveSettings
                {
                    get
                    {
                        return dveConfiguration;
                    }
                }

                /// <summary>
                /// Gets or sets the element name.
                /// </summary>
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation is empty or white space.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation exceeds 200 characters.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation contains a forbidden character.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation contains more than one '%' character.</exception>
                /// <remarks>Forbidden characters: '\', '/', ':', '*', '?', '"', '&lt;', '&gt;', '|', '°', ';'.</remarks>
                public string Name
                {
                    get
                    {
                        return name;
                    }

                    set
                    {
                        name = Skyline.DataMiner.Library.Common.InputValidator.ValidateName(value, "value");
                    }
                }

                /// <summary>
                /// Gets the writable properties of the elements.
                /// </summary>
                public Skyline.DataMiner.Library.Common.IPropertConfigurationCollection Properties
                {
                    get
                    {
                        LoadPropertyDefinitions();
                        return new Skyline.DataMiner.Library.Common.PropertyConfigurationCollection(properties);
                    }
                }

                /// <summary>
                /// Gets the protocol.
                /// </summary>
                public Skyline.DataMiner.Library.Common.IDmsProtocol Protocol
                {
                    get
                    {
                        return protocol;
                    }
                }

                /// <summary>
                /// Gets or sets the element state.
                /// </summary>
                /// <exception cref = "ArgumentException">The value of a set operation is invalid.</exception>
                public Skyline.DataMiner.Library.Common.ConfigurationElementState State
                {
                    get
                    {
                        return state;
                    }

                    set
                    {
                        if (value == Skyline.DataMiner.Library.Common.ConfigurationElementState.Active || value == Skyline.DataMiner.Library.Common.ConfigurationElementState.Paused || value == Skyline.DataMiner.Library.Common.ConfigurationElementState.Stopped)
                        {
                            state = value;
                        }
                        else
                        {
                            throw new System.ArgumentException("Invalid state specified.", "value");
                        }
                    }
                }

                /// <summary>
                /// Gets or sets the trend template.
                /// </summary>
                /// <exception cref = "ArgumentException">The value of a set operation is a trend template that is not compatible with the protocol of the element.</exception>
                public Skyline.DataMiner.Library.Common.Templates.IDmsTrendTemplate TrendTemplate
                {
                    get
                    {
                        return trendTemplate;
                    }

                    set
                    {
                        if (!Skyline.DataMiner.Library.Common.InputValidator.IsCompatibleTemplate(value, this.Protocol))
                        {
                            throw new System.ArgumentException("The specified trend template is not compatible with the protocol this element executes.", "value");
                        }
                        else
                        {
                            trendTemplate = value;
                        }
                    }
                }

                /// <summary>
                /// Gets or sets the type of the element.
                /// </summary>
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                public string Type
                {
                    get
                    {
                        return type;
                    }

                    set
                    {
                        if (value == null)
                        {
                            throw new System.ArgumentNullException("value");
                        }

                        type = value;
                    }
                }

                /// <summary>
                /// Gets the views that include the element.
                /// </summary>
                public System.Collections.Generic.ICollection<Skyline.DataMiner.Library.Common.IDmsView> Views
                {
                    get
                    {
                        return views;
                    }
                }

                /// <summary>
                /// Gets or sets the list of connections on the element.
                /// </summary>
                public Skyline.DataMiner.Library.Common.ElementConnectionCollection Connections
                {
                    get
                    {
                        return connections;
                    }

                    set
                    {
                        if (value == null)
                        {
                            throw new System.ArgumentNullException("value");
                        }

                        if (value.Length != connections.Length)
                        {
                            throw new Skyline.DataMiner.Library.Common.IncorrectDataException("IElementConnection array length does not match expected value");
                        }
                        else
                        {
                            bool valid = value.ValidateConnectionTypes();
                            if (!valid)
                            {
                                throw new Skyline.DataMiner.Library.Common.IncorrectDataException("Invalid Connection Type provided.");
                            }

                            connections = value;
                        }
                    }
                }

                /// <summary>
                /// Gets the list of property names that where updated.
                /// </summary>
                internal System.Collections.Generic.IEnumerable<System.String> UpdatedProperties
                {
                    get
                    {
                        return updatedProperties;
                    }
                }

                /// <summary>
                /// Loads the writable property definitions when required.
                /// </summary>
                internal void LoadPropertyDefinitions()
                {
                    if (!this.propertiesLoaded)
                    {
                        this.propertiesLoaded = true;
                        foreach (Skyline.DataMiner.Library.Common.Properties.IDmsElementPropertyDefinition def in this.dms.ElementPropertyDefinitions)
                        {
                            if (!def.IsReadOnly)
                            {
                                properties.Add(def.Name, new Skyline.DataMiner.Library.Common.PropertyConfiguration(this, def, System.String.Empty));
                            }
                        }
                    }
                }

                /// <summary>
                /// Adds the name of the property that was updated by the user.
                /// </summary>
                /// <param name = "sender">The sender of the event.</param>
                /// <param name = "e">The event arguments.</param>
                internal void PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
                {
                    updatedProperties.Add(e.PropertyName);
                }
            }

            /// <summary>
            /// A collection of IElementConnection objects.
            /// </summary>
            public class ElementConnectionCollection : Skyline.DataMiner.Library.Common.IElementConnectionCollection
            {
                private readonly Skyline.DataMiner.Library.Common.IElementConnection[] connections;
                private readonly bool canBeValidated;
                private readonly System.Collections.Generic.IList<Skyline.DataMiner.Library.Common.IDmsConnectionInfo> protocolConnectionInfo;
                /// <summary>
                /// initiates a new instance.
                /// </summary>
                /// <param name = "protocolConnectionInfo"></param>
                internal ElementConnectionCollection(System.Collections.Generic.IList<Skyline.DataMiner.Library.Common.IDmsConnectionInfo> protocolConnectionInfo)
                {
                    int amountOfConnections = protocolConnectionInfo.Count;
                    this.connections = new Skyline.DataMiner.Library.Common.IElementConnection[amountOfConnections];
                    this.protocolConnectionInfo = protocolConnectionInfo;
                    canBeValidated = true;
                }

                /// <summary>
                /// Initiates a new instance.
                /// </summary>
                /// <param name = "message"></param>
                internal ElementConnectionCollection(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage message)
                {
                    int amountOfConnections = 1;
                    if (message != null && message.ExtraPorts != null)
                    {
                        amountOfConnections += message.ExtraPorts.Length;
                    }

                    this.connections = new Skyline.DataMiner.Library.Common.IElementConnection[amountOfConnections];
                    canBeValidated = false;
                }

                /// <summary>
                /// Gets the total amount of elements in the collection.
                /// </summary>
                public int Length
                {
                    get
                    {
                        return connections.Length;
                    }
                }

                /// <summary>
                /// Returns the collection as a IElementConnection array.
                /// </summary>
                public System.Collections.Generic.IEnumerable<Skyline.DataMiner.Library.Common.IElementConnection> Enumerator
                {
                    get
                    {
                        return connections;
                    }
                }

                /// <summary>
                /// Returns an enumerator that iterates through the collection.
                /// </summary>
                /// <returns>An enumerator that can be used to iterate through the collection.</returns>
                public System.Collections.Generic.IEnumerator<Skyline.DataMiner.Library.Common.IElementConnection> GetEnumerator()
                {
                    return ((System.Collections.Generic.IEnumerable<Skyline.DataMiner.Library.Common.IElementConnection>)connections).GetEnumerator();
                }

                /// <summary>
                /// Returns an enumerator that iterates through a collection.
                /// </summary>
                /// <returns>An <see cref = "IEnumerator"/> object that can be used to iterate through the collection.</returns>
                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
                {
                    return this.GetEnumerator();
                }

                /// <summary>
                /// Gets or sets an entry in the collection.
                /// </summary>
                /// <param name = "index"></param>
                /// <returns></returns>
                public Skyline.DataMiner.Library.Common.IElementConnection this[int index]
                {
                    get
                    {
                        return connections[index];
                    }

                    set
                    {
                        bool valid = ValidateConnectionTypeAtPos(index, value);
                        if (valid)
                        {
                            connections[index] = value;
                        }
                        else
                        {
                            throw new Skyline.DataMiner.Library.Common.IncorrectDataException("Invalid connection type provided at index " + index);
                        }
                    }
                }

                /// <summary>
                /// Validates all connections in the collection. 
                /// </summary>
                /// <returns>returns true when connection is valid, false when invalid.</returns>
                internal bool ValidateConnectionTypes()
                {
                    bool valid = true;
                    for (int i = 0; i < protocolConnectionInfo.Count; i++)
                    {
                        Skyline.DataMiner.Library.Common.IDmsConnectionInfo connectionInfo = protocolConnectionInfo[i];
                        Skyline.DataMiner.Library.Common.IElementConnection conn = this.connections[i];
                        valid = ValidateConnectionInfo(conn, connectionInfo);
                        if (!valid)
                            break;
                    }

                    return valid;
                }

                /// <summary>
                /// Validates the provided <see cref = "IElementConnection"/> against the provided Protocol.
                /// </summary>
                /// <param name = "index">The index position of the connection to validate.</param>
                /// <param name = "conn">The IElementConnection connection.</param>
                /// <exception cref = "ArgumentOutOfRangeException"><paramref name = "index"/> is out of range.</exception>
                /// <returns></returns>
                private bool ValidateConnectionTypeAtPos(int index, Skyline.DataMiner.Library.Common.IElementConnection conn)
                {
                    if (!canBeValidated)
                    {
                        return true;
                    }

                    if (index < 0 || ((index + 1) > protocolConnectionInfo.Count))
                    {
                        throw new System.ArgumentOutOfRangeException("index", "Index needs to be between 0 and the amount of connections in the protocol minus 1.");
                    }

                    return ValidateConnectionInfo(conn, protocolConnectionInfo[index]);
                }

                /// <summary>
                /// Validates a single connection.
                /// </summary>
                /// <param name = "conn"><see cref = "IElementConnection"/> object.</param>
                /// <param name = "connectionInfo"><see cref = "IDmsConnectionInfo"/> object.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "conn"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentNullException"><paramref name = "connectionInfo"/> is <see langword = "null"/>.</exception>
                /// <returns></returns>
                private static bool ValidateConnectionInfo(Skyline.DataMiner.Library.Common.IElementConnection conn, Skyline.DataMiner.Library.Common.IDmsConnectionInfo connectionInfo)
                {
                    if (conn == null)
                    {
                        throw new Skyline.DataMiner.Library.Common.IncorrectDataException("conn: Invalid data , ElementConfiguration does not contain connection info");
                    }

                    if (connectionInfo == null)
                    {
                        throw new Skyline.DataMiner.Library.Common.IncorrectDataException("connectionInfo: Invalid data , Protocol does not contain connection info");
                    }

                    switch (connectionInfo.Type)
                    {
                        case Skyline.DataMiner.Library.Common.ConnectionType.SnmpV1:
                            return ValidateAsSnmpV1(conn);
                        case Skyline.DataMiner.Library.Common.ConnectionType.SnmpV2:
                            return ValidateAsSnmpV2(conn);
                        case Skyline.DataMiner.Library.Common.ConnectionType.SnmpV3:
                            return ValidateAsSnmpV3(conn);
                        case Skyline.DataMiner.Library.Common.ConnectionType.Virtual:
                            return conn is Skyline.DataMiner.Library.Common.IVirtualConnection;
                        case Skyline.DataMiner.Library.Common.ConnectionType.Http:
                            return conn is Skyline.DataMiner.Library.Common.IHttpConnection;
                        default:
                            return false;
                    }
                }

                /// <summary>
                /// Validate a connection for SNMPv1
                /// </summary>
                /// <param name = "conn">object of type <see cref = "IElementConnection"/> to validate.</param>
                /// <returns></returns>
                private static bool ValidateAsSnmpV1(Skyline.DataMiner.Library.Common.IElementConnection conn)
                {
                    return conn is Skyline.DataMiner.Library.Common.ISnmpV1Connection || conn is Skyline.DataMiner.Library.Common.ISnmpV2Connection || conn is Skyline.DataMiner.Library.Common.ISnmpV3Connection;
                }

                /// <summary>
                /// Validate a connection for SNMPv2
                /// </summary>
                /// <param name = "conn">object of type <see cref = "IElementConnection"/> to validate.</param>
                /// <returns></returns>
                private static bool ValidateAsSnmpV2(Skyline.DataMiner.Library.Common.IElementConnection conn)
                {
                    return conn is Skyline.DataMiner.Library.Common.ISnmpV2Connection || conn is Skyline.DataMiner.Library.Common.ISnmpV3Connection;
                }

                /// <summary>
                /// Validate a connection for SNMPv3
                /// </summary>
                /// <param name = "conn">object of type <see cref = "IElementConnection"/> to validate.</param>
                /// <returns></returns>
                private static bool ValidateAsSnmpV3(Skyline.DataMiner.Library.Common.IElementConnection conn)
                {
                    return conn is Skyline.DataMiner.Library.Common.ISnmpV3Connection || conn is Skyline.DataMiner.Library.Common.ISnmpV2Connection;
                }

                /// <summary>
                /// Clear any update flags for all the provided connections.
                /// </summary>
                public void Clear()
                {
                    foreach (Skyline.DataMiner.Library.Common.IElementConnection connection in connections)
                    {
                        Skyline.DataMiner.Library.Common.ConnectionSettings connectionSetting = connection as Skyline.DataMiner.Library.Common.ConnectionSettings;
                        if (connectionSetting != null)
                        {
                            connectionSetting.ClearUpdates();
                        }
                    }
                }

                /// <summary>
                /// Indicates if changes occurred on IElementCommunicationConnection objects requiring an update of the SLNET Message.
                /// </summary>
                /// <returns>A boolean indicating an update is required or not. </returns>
                public bool IsUpdateRequired()
                {
                    bool isUpdate = false;
                    foreach (Skyline.DataMiner.Library.Common.IElementConnection connection in connections)
                    {
                        Skyline.DataMiner.Library.Common.ConnectionSettings connectionSetting = connection as Skyline.DataMiner.Library.Common.ConnectionSettings;
                        if (connectionSetting != null)
                        {
                            isUpdate = connectionSetting.IsUpdated;
                            if (isUpdate)
                                break;
                        }
                    }

                    return isUpdate;
                }

                /// <summary>
                /// Updates the provide ElementPortInfo list based on changes performed on provided IElementCommunicationConnection implementations.
                /// </summary>
                /// <param name = "portInfos">List of <see cref = "ElementPortInfo"/></param>
                /// <param name = "isCompatibilityIssueDetected"></param>
                internal void UpdatePortInfo(Skyline.DataMiner.Net.Messages.ElementPortInfo[] portInfos, bool isCompatibilityIssueDetected)
                {
                    for (int i = 0; i < connections.Length; i++)
                    {
                        Skyline.DataMiner.Library.Common.ConnectionSettings settings = connections[i] as Skyline.DataMiner.Library.Common.ConnectionSettings;
                        if (settings != null)
                        {
                            Skyline.DataMiner.Net.Messages.ElementPortInfo info = portInfos[i];
                            settings.UpdateElementPortInfo(info, isCompatibilityIssueDetected);
                        }
                    }
                }

                /// <summary>
                /// Creates a list of ElementPortInfo objects based on the list of IElementCommunicationConnection objects.
                /// </summary>
                /// <returns>List of <see cref = "ElementPortInfo"/></returns>
                internal System.Collections.Generic.IEnumerable<Skyline.DataMiner.Net.Messages.ElementPortInfo> CreatePortInfo(bool isCompatibilityIssueDetected)
                {
                    Skyline.DataMiner.Net.Messages.ElementPortInfo[] portInfoMessages = new Skyline.DataMiner.Net.Messages.ElementPortInfo[connections.Length];
                    for (int i = 0; i < connections.Length; i++)
                    {
                        Skyline.DataMiner.Library.Common.ConnectionSettings connection = connections[i] as Skyline.DataMiner.Library.Common.ConnectionSettings;
                        if (connection != null)
                        {
                            portInfoMessages[i] = connection.CreateElementPortInfo(i, isCompatibilityIssueDetected);
                        }
                    }

                    return portInfoMessages;
                }
            }

            /// <summary>
            /// Represents a configuration class for elements that are included in services.
            /// </summary>
            public class ElementParamConfiguration : Skyline.DataMiner.Library.Common.ParamConfiguration
            {
                private System.Collections.Generic.List<Skyline.DataMiner.Library.Common.ElementParamFilterConfiguration> parameterFilters;
                /// <summary>
                /// Initializes a new instance of the <see cref = "ElementParamConfiguration"/> class.
                /// </summary>
                /// <param name = "paramsSettingsConfiguration">The parameter settings of the service.</param>
                /// <param name = "infoParams">The ServiceInfoParams object.</param>
                /// <param name = "index">The unique index of the item included in the service.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "infoParams"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "infoParams"/> has an alias that already exists in the service configuration.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "infoParams"/> is a service.</exception>
                internal ElementParamConfiguration(Skyline.DataMiner.Library.Common.ServiceParamsConfiguration paramsSettingsConfiguration, Skyline.DataMiner.Net.Messages.ServiceInfoParams infoParams, int index)
                {
                    this.ParamsSettingsConfiguration = paramsSettingsConfiguration;
                    if (infoParams == null)
                    {
                        throw new System.ArgumentNullException("infoParams");
                    }

                    if (infoParams.IsService)
                    {
                        throw new System.ArgumentException("The serviceinfoparams are for a service instance.", "infoParams");
                    }

                    if (paramsSettingsConfiguration.ContainsAlias(infoParams.Alias))
                    {
                        throw new System.ArgumentException("The alias already exists in the service configuration.", "infoParams");
                    }

                    IncludedElement = infoParams;
                    parameterFilters = Skyline.DataMiner.Library.Common.ElementParamFilterConfiguration.GetParameterFilters(infoParams);
                    Index = index;
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ElementParamConfiguration"/> class.
                /// </summary>
                /// <param name = "paramsSettingsConfiguration">The parameter settings of the service.</param>
                /// <param name = "elementId">The DataMiner/element ID of the element to include.</param>
                /// <param name = "parameters">The parameters that need to be included in the service.</param>
                /// <param name = "index">The unique index of the item included in the service.</param>
                internal ElementParamConfiguration(Skyline.DataMiner.Library.Common.ServiceParamsConfiguration paramsSettingsConfiguration, Skyline.DataMiner.Library.Common.DmsElementId elementId, System.Collections.Generic.List<Skyline.DataMiner.Library.Common.ElementParamFilterConfiguration> parameters, int index)
                {
                    this.ParamsSettingsConfiguration = paramsSettingsConfiguration;
                    IncludedElement = new Skyline.DataMiner.Net.Messages.ServiceInfoParams(elementId.AgentId, elementId.ElementId, false);
                    parameterFilters = parameters;
                    Index = index;
                }

                /// <summary>
                /// Gets or sets the DataMiner/element ID of the included element.
                /// </summary>
                public Skyline.DataMiner.Library.Common.DmsElementId ElementId
                {
                    get
                    {
                        return new Skyline.DataMiner.Library.Common.DmsElementId(IncludedElement.DataMinerID, IncludedElement.ElementID);
                    }

                    set
                    {
                        IncludedElement.DataMinerID = value.AgentId;
                        IncludedElement.ElementID = value.ElementId;
                    }
                }

                /// <summary>
                /// Gets the parameters included in the service for this element.
                /// </summary>
                /// <returns>The </returns>
                public System.Collections.Generic.List<Skyline.DataMiner.Library.Common.ElementParamFilterConfiguration> ParameterFilters
                {
                    get
                    {
                        return parameterFilters;
                    }
                }
            }

            /// <summary>
            /// Represents a class for the element parameters included in the service.
            /// </summary>
            public class ElementParamFilterConfiguration
            {
                private readonly Skyline.DataMiner.Net.Messages.ServiceParamFilter serviceParamFilter;
                /// <summary>
                /// Initializes a new instance of the <see cref = "ElementParamFilterConfiguration"/> class.
                /// </summary>
                /// <param name = "pid">The parameter ID.</param>
                /// <param name = "filter">The filter in case of a table parameter.</param>
                /// <param name = "included">Indicates if the parameter is included.</param>
                /// <exception cref = "ArgumentException"><paramref name = "pid"/> is invalid.</exception>
                public ElementParamFilterConfiguration(int pid, string filter, bool included)
                {
                    if (pid < 0)
                    {
                        throw new System.ArgumentException("The parameter ID should be greater than 0.", "pid");
                    }

                    serviceParamFilter = new Skyline.DataMiner.Net.Messages.ServiceParamFilter(pid, filter, included);
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ElementParamFilterConfiguration"/> class.
                /// </summary>
                /// <param name = "pid">The parameter ID.</param>
                /// <param name = "filter">The filter in case of a table parameter.</param>
                /// <param name = "included">Indicates if the parameter is included.</param>
                /// <param name = "type">The type of the filter.</param>
                /// <exception cref = "ArgumentException"><paramref name = "pid"/> is invalid.</exception>
                public ElementParamFilterConfiguration(int pid, string filter, bool included, Skyline.DataMiner.Library.Common.FilterType type)
                {
                    if (pid < 0)
                    {
                        throw new System.ArgumentException("The parameter ID should be greater than 0.", "pid");
                    }

                    serviceParamFilter = new Skyline.DataMiner.Net.Messages.ServiceParamFilter(pid, filter, included, (Skyline.DataMiner.Net.Messages.FilterType)type);
                }

                internal ElementParamFilterConfiguration(Skyline.DataMiner.Net.Messages.ServiceParamFilter serviceParamFilter)
                {
                    this.serviceParamFilter = serviceParamFilter;
                }

                /// <summary>
                /// Gets or sets the filter for the parameter.
                /// </summary>
                public string Filter
                {
                    get
                    {
                        return serviceParamFilter.Filter;
                    }

                    set
                    {
                        serviceParamFilter.Filter = value;
                    }
                }

                /// <summary>
                /// Gets the filter value for the parameter.
                /// </summary>
                public string FilterValue
                {
                    get
                    {
                        return serviceParamFilter.FilterValue;
                    }
                }

                /// <summary>
                /// Gets or sets the filter type for the parameter.
                /// </summary>
                public Skyline.DataMiner.Library.Common.FilterType FilterType
                {
                    get
                    {
                        return (Skyline.DataMiner.Library.Common.FilterType)serviceParamFilter.FilterType;
                    }

                    set
                    {
                        serviceParamFilter.FilterType = (Skyline.DataMiner.Net.Messages.FilterType)value;
                    }
                }

                /// <summary>
                /// Gets or sets a value indicating whether the parameter is included.
                /// </summary>
                public bool IsIncluded
                {
                    get
                    {
                        return serviceParamFilter.IsIncluded;
                    }

                    set
                    {
                        serviceParamFilter.IsIncluded = value;
                    }
                }

                /// <summary>
                /// Gets or sets the matrix port for the parameter.
                /// </summary>
                public int MatrixPort
                {
                    get
                    {
                        return serviceParamFilter.MatrixPort;
                    }

                    set
                    {
                        serviceParamFilter.MatrixPort = value;
                    }
                }

                /// <summary>
                /// Gets or sets the ID of the parameter.
                /// </summary>
                public int ParameterID
                {
                    get
                    {
                        return serviceParamFilter.ParameterID;
                    }

                    set
                    {
                        serviceParamFilter.ParameterID = value;
                    }
                }

                internal static System.Collections.Generic.List<Skyline.DataMiner.Library.Common.ElementParamFilterConfiguration> GetParameterFilters(Skyline.DataMiner.Net.Messages.ServiceInfoParams infoParams)
                {
                    System.Collections.Generic.List<Skyline.DataMiner.Library.Common.ElementParamFilterConfiguration> lParameters = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.ElementParamFilterConfiguration>();
                    foreach (var parameter in infoParams.ParameterFilters)
                    {
                        lParameters.Add(new Skyline.DataMiner.Library.Common.ElementParamFilterConfiguration(parameter));
                    }

                    return lParameters;
                }
            }

            /// <summary>
            /// A collection of IElementConnection objects.
            /// </summary>
            public interface IElementConnectionCollection : System.Collections.Generic.IEnumerable<Skyline.DataMiner.Library.Common.IElementConnection>
            {
                /// <summary>
                /// Gets or sets an entry in the collection.
                /// </summary>
                Skyline.DataMiner.Library.Common.IElementConnection this[int index]
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets the total amount of elements in the collection.
                /// </summary>
                int Length
                {
                    get;
                }

                /// <summary>
                /// Returns the collection as a IElementConnection array.
                /// </summary>
                System.Collections.Generic.IEnumerable<Skyline.DataMiner.Library.Common.IElementConnection> Enumerator
                {
                    get;
                }

                /// <summary>
                /// Clear any update flags for all the provided connections.
                /// </summary>
                void Clear();
                /// <summary>
                /// Indicates if changes occurred on IElementCommunicationConnection objects requiring an update of the SLNET Message.
                /// </summary>
                /// <returns>A boolean indicating an update is required or not. </returns>
                bool IsUpdateRequired();
            }

            /// <summary>
            /// DataMiner service configuration interface for included elements or services.
            /// </summary>
            public interface IServiceParamConfiguration
            {
                /// <summary>
                /// Gets or sets the Alias of the service.
                /// </summary>
                string Alias
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the included capped alarm level of the element or service.
                /// </summary>
                Skyline.DataMiner.Library.Common.AlarmLevel IncludedCapped
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets the index of the element or service.
                /// </summary>
                int Index
                {
                    get;
                }

                /// <summary>
                /// Gets or sets a value indicating whether the element is excluded.
                /// </summary>
                bool IsExcluded
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets a value indicating whether the element is a service.
                /// </summary>
                bool IsService
                {
                    get;
                }

                /// <summary>
                /// Gets or sets the not used capped alarm level of the element.
                /// </summary>
                Skyline.DataMiner.Library.Common.AlarmLevel NotUsedCapped
                {
                    get;
                    set;
                }
            }

            /// <summary>
            /// Represents an abstract configuration class for elements or services that are included in a service.
            /// </summary>
            public abstract class ParamConfiguration : Skyline.DataMiner.Library.Common.IServiceParamConfiguration
            {
                /// <summary>
                /// Gets or sets the Alias of the element.
                /// </summary>
                /// <exception cref = "ArgumentException">The value has an alias that already exists in the service configuration.</exception>
                public string Alias
                {
                    get
                    {
                        return IncludedElement.Alias;
                    }

                    set
                    {
                        if (ParamsSettingsConfiguration.ContainsAlias(value))
                        {
                            throw new System.ArgumentException("The alias already exists in the service configuration.", "value");
                        }

                        IncludedElement.Alias = value;
                    }
                }

                /// <summary>
                /// Gets or sets the included capped alarm level of the element.
                /// </summary>
                public Skyline.DataMiner.Library.Common.AlarmLevel IncludedCapped
                {
                    get
                    {
                        return (Skyline.DataMiner.Library.Common.AlarmLevel)System.Enum.Parse(typeof(Skyline.DataMiner.Library.Common.AlarmLevel), IncludedElement.IncludedCapped, true);
                    }

                    set
                    {
                        IncludedElement.IncludedCapped = value.ToString();
                    }
                }

                /// <summary>
                /// Gets or sets the index of the element.
                /// </summary>
                public int Index
                {
                    get
                    {
                        return IncludedElement.Index;
                    }

                    protected set
                    {
                        IncludedElement.Index = value;
                    }
                }

                /// <summary>
                /// Gets or sets a value indicating whether the element is excluded.
                /// </summary>
                public bool IsExcluded
                {
                    get
                    {
                        return IncludedElement.IsExcluded;
                    }

                    set
                    {
                        IncludedElement.IsExcluded = value;
                    }
                }

                /// <summary>
                /// Gets a value indicating whether the element is a service.
                /// </summary>
                public bool IsService
                {
                    get
                    {
                        return false;
                    }
                }

                /// <summary>
                /// Gets or sets the not used capped alarm level of the element.
                /// </summary>
                public Skyline.DataMiner.Library.Common.AlarmLevel NotUsedCapped
                {
                    get
                    {
                        return (Skyline.DataMiner.Library.Common.AlarmLevel)System.Enum.Parse(typeof(Skyline.DataMiner.Library.Common.AlarmLevel), IncludedElement.NotUsedCapped, true);
                    }

                    set
                    {
                        IncludedElement.NotUsedCapped = value.ToString();
                    }
                }

                /// <summary>
                /// Gets or sets the paramsSettingsConfiguration class.
                /// </summary>
                internal Skyline.DataMiner.Library.Common.ServiceParamsConfiguration ParamsSettingsConfiguration
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the <see cref = "ServiceInfoParams"/> object of the included element.
                /// </summary>
                protected Skyline.DataMiner.Net.Messages.ServiceInfoParams IncludedElement
                {
                    get;
                    set;
                }
            }

            /// <summary>
            /// Represents a property configuration.
            /// </summary>
            public class PropertyConfiguration : System.ComponentModel.INotifyPropertyChanged
            {
                private readonly Skyline.DataMiner.Library.Common.Properties.IDmsPropertyDefinition definition;
                private string value;
                /// <summary>
                /// Initializes a new instance of the <see cref = "PropertyConfiguration"/> class.
                /// </summary>
                /// <param name = "config">The configuration of the element.</param>
                /// <param name = "definition">The definition of the property.</param>
                /// <param name = "value">The value that should be assigned to the property.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "definition"/> or <paramref name = "config"/> is <see langword = "null"/>.</exception>
                internal PropertyConfiguration(Skyline.DataMiner.Library.Common.ElementConfiguration config, Skyline.DataMiner.Library.Common.Properties.IDmsPropertyDefinition definition, string value)
                {
                    if (definition == null)
                    {
                        throw new System.ArgumentNullException("definition");
                    }

                    if (config == null)
                    {
                        throw new System.ArgumentNullException("config");
                    }

                    this.value = value;
                    this.definition = definition;
                    PropertyChanged += config.PropertyChanged;
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "PropertyConfiguration"/> class.
                /// </summary>
                /// <param name = "config">The configuration of the service.</param>
                /// <param name = "definition">The definition of the property.</param>
                /// <param name = "value">The value that should be assigned to the property.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "definition"/> or <paramref name = "config"/> is <see langword = "null"/>.</exception>
                internal PropertyConfiguration(Skyline.DataMiner.Library.Common.ServiceConfiguration config, Skyline.DataMiner.Library.Common.Properties.IDmsPropertyDefinition definition, string value)
                {
                    if (definition == null)
                    {
                        throw new System.ArgumentNullException("definition");
                    }

                    if (config == null)
                    {
                        throw new System.ArgumentNullException("config");
                    }

                    this.value = value;
                    this.definition = definition;
                    PropertyChanged += config.PropertyChanged;
                }

                /// <summary>
                /// Occurs when the value of a property changes.
                /// </summary>
                public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
                /// <summary>
                /// Gets the property definition.
                /// </summary>
                public Skyline.DataMiner.Library.Common.Properties.IDmsPropertyDefinition Definition
                {
                    get
                    {
                        return definition;
                    }
                }

                /// <summary>
                /// Gets or sets the property value.
                /// </summary>
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation is invalid.</exception>
                public string Value
                {
                    get
                    {
                        return value;
                    }

                    set
                    {
                        if (value == null)
                        {
                            throw new System.ArgumentNullException("value");
                        }

                        if (!definition.IsValidInput(value))
                        {
                            throw new System.ArgumentException("value", System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "The value:'{0}' cannot be assigned to the property.", value));
                        }

                        this.value = value;
                        NotifyPropertyChanged();
                    }
                }

                /// <summary>
                /// Notifies when a property was changed.
                /// </summary>
                private void NotifyPropertyChanged()
                {
                    System.ComponentModel.PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null)
                    {
                        PropertyChanged.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(definition.Name));
                    }
                }
            }

            /// <summary>
            /// Represents a service configuration.
            /// </summary>
            public class ServiceConfiguration
            {
                /// <summary>
                /// The parameter settings.
                /// </summary>
                private readonly Skyline.DataMiner.Library.Common.ServiceParamsConfiguration parameterSettings;
                /// <summary>
                /// The advanced configuration options.
                /// </summary>
                private readonly Skyline.DataMiner.Library.Common.AdvancedServiceConfiguration advancedConfiguration = new Skyline.DataMiner.Library.Common.AdvancedServiceConfiguration();
                /// <summary>
                /// The IDms class.
                /// </summary>
                private readonly Skyline.DataMiner.Library.Common.IDms dms;
                /// <summary>
                /// The properties to be added to the service.
                /// </summary>
                private readonly System.Collections.Generic.IDictionary<System.String, Skyline.DataMiner.Library.Common.PropertyConfiguration> properties = new System.Collections.Generic.Dictionary<System.String, Skyline.DataMiner.Library.Common.PropertyConfiguration>();
                /// <summary>
                /// Keeps track of which properties where updated.
                /// </summary>
                private readonly System.Collections.Generic.HashSet<System.String> updatedProperties = new System.Collections.Generic.HashSet<System.String>();
                /// <summary>
                /// The views containing the service.
                /// </summary>
                private readonly System.Collections.Generic.ICollection<Skyline.DataMiner.Library.Common.IDmsView> views = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.IDmsView>();
                /// <summary>
                /// Service description.
                /// </summary>
                private string description = System.String.Empty;
                /// <summary>
                /// The name of the service.
                /// </summary>
                private string name;
                /// <summary>
                /// Specific whether or not the properties where loaded.
                /// </summary>
                private bool propertiesLoaded;
                /// <summary>
                /// Initializes a new instance of the <see cref = "ServiceConfiguration"/> class. The parameter settings need to be added to create the service.
                /// </summary>
                /// <param name = "dms">The <see cref = "IDms"/> interface.</param>
                /// <param name = "serviceName">The service name.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "serviceName"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "serviceName"/> is empty or white space.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "serviceName"/> exceeds 200 characters.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "serviceName"/> contains a forbidden character.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "serviceName"/> contains more than one '%' character.</exception>
                /// <remarks>Forbidden characters: '\', '/', ':', '*', '?', '"', '&lt;', '&gt;', '|', '°', ';'.</remarks>
                public ServiceConfiguration(Skyline.DataMiner.Library.Common.IDms dms, string serviceName)
                {
                    if (dms == null)
                    {
                        throw new System.ArgumentNullException("dms");
                    }

                    Name = serviceName;
                    this.dms = dms;
                    this.parameterSettings = new Skyline.DataMiner.Library.Common.ServiceParamsConfiguration();
                }

                /// <summary>
                /// Gets the advanced settings.
                /// </summary>
                public Skyline.DataMiner.Library.Common.AdvancedServiceConfiguration AdvancedSettings
                {
                    get
                    {
                        return advancedConfiguration;
                    }
                }

                /// <summary>
                /// Gets or sets the description.
                /// </summary>
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                public string Description
                {
                    get
                    {
                        return description;
                    }

                    set
                    {
                        if (value == null)
                        {
                            throw new System.ArgumentNullException("value");
                        }

                        description = value;
                    }
                }

                /// <summary>
                /// Gets or sets the service name.
                /// </summary>
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation is empty or white space.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation exceeds 200 characters.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation contains a forbidden character.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation contains more than one '%' character.</exception>
                /// <remarks>Forbidden characters: '\', '/', ':', '*', '?', '"', '&lt;', '&gt;', '|', '°', ';'.</remarks>
                public string Name
                {
                    get
                    {
                        return name;
                    }

                    set
                    {
                        name = Skyline.DataMiner.Library.Common.InputValidator.ValidateName(value, "value");
                    }
                }

                /// <summary>
                /// Gets the writable properties of the service.
                /// </summary>
                public Skyline.DataMiner.Library.Common.IPropertConfigurationCollection Properties
                {
                    get
                    {
                        LoadPropertyDefinitions();
                        return new Skyline.DataMiner.Library.Common.PropertyConfigurationCollection(properties);
                    }
                }

                /// <summary>
                /// Gets the views that include the service.
                /// </summary>
                public System.Collections.Generic.ICollection<Skyline.DataMiner.Library.Common.IDmsView> Views
                {
                    get
                    {
                        return views;
                    }
                }

                /// <summary>
                /// Gets the included elements.
                /// </summary>
                public Skyline.DataMiner.Library.Common.ParamConfiguration[] IncludedElements
                {
                    get
                    {
                        return parameterSettings.GetIncludedElements();
                    }
                }

                /// <summary>
                /// Loads the writable property definitions when required.
                /// </summary>
                internal void LoadPropertyDefinitions()
                {
                    if (!this.propertiesLoaded)
                    {
                        this.propertiesLoaded = true;
                        foreach (Skyline.DataMiner.Library.Common.Properties.IDmsServicePropertyDefinition def in this.dms.ServicePropertyDefinitions)
                        {
                            if (!def.IsReadOnly)
                            {
                                properties.Add(def.Name, new Skyline.DataMiner.Library.Common.PropertyConfiguration(this, def, System.String.Empty));
                            }
                        }
                    }
                }

                /// <summary>
                /// Adds the name of the property that was updated by the user.
                /// </summary>
                /// <param name = "sender">The sender of the event.</param>
                /// <param name = "e">The event arguments.</param>
                internal void PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
                {
                    updatedProperties.Add(e.PropertyName);
                }
            }

            /// <summary>
            /// Represents a configuration class for services that are included in services.
            /// </summary>
            public class ServiceParamConfiguration : Skyline.DataMiner.Library.Common.ParamConfiguration
            {
                /// <summary>
                /// Initializes a new instance of the <see cref = "ServiceParamConfiguration"/> class.
                /// </summary>
                /// <param name = "parametersConfiguration">The parameter settings of the service.</param>
                /// <param name = "infoParams">The ServiceInfoParams object.</param>
                /// <param name = "index">The unique index of the item included in the service.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "infoParams"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "infoParams"/> has an alias that already exists in the service configuration.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "infoParams"/> is not a service.</exception>
                internal ServiceParamConfiguration(Skyline.DataMiner.Library.Common.ServiceParamsConfiguration parametersConfiguration, Skyline.DataMiner.Net.Messages.ServiceInfoParams infoParams, int index)
                {
                    this.ParamsSettingsConfiguration = parametersConfiguration;
                    if (infoParams == null)
                    {
                        throw new System.ArgumentNullException("infoParams");
                    }

                    if (!infoParams.IsService)
                    {
                        throw new System.ArgumentException("The serviceinfoparams are not for a service instance.", "infoParams");
                    }

                    if (parametersConfiguration.ContainsAlias(infoParams.Alias))
                    {
                        throw new System.ArgumentException("The alias already exists in the service configuration.", "infoParams");
                    }

                    IncludedElement = infoParams;
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ServiceParamConfiguration"/> class.
                /// </summary>
                /// <param name = "parametersConfiguration">The parameter settings of the service.</param>
                /// <param name = "serviceId">The service ID of the service that needs to be included.</param>
                /// <param name = "index">The unique index of the item included in the service.</param>
                internal ServiceParamConfiguration(Skyline.DataMiner.Library.Common.ServiceParamsConfiguration parametersConfiguration, Skyline.DataMiner.Library.Common.DmsServiceId serviceId, int index)
                {
                    this.ParamsSettingsConfiguration = parametersConfiguration;
                    IncludedElement = new Skyline.DataMiner.Net.Messages.ServiceInfoParams(serviceId.AgentId, serviceId.ServiceId, true);
                }

                /// <summary>
                /// Gets or sets the DataMiner/service ID of the included service.
                /// </summary>
                public Skyline.DataMiner.Library.Common.DmsServiceId ServiceId
                {
                    get
                    {
                        return new Skyline.DataMiner.Library.Common.DmsServiceId(IncludedElement.DataMinerID, IncludedElement.ElementID);
                    }

                    set
                    {
                        IncludedElement.DataMinerID = value.AgentId;
                        IncludedElement.ElementID = value.ServiceId;
                    }
                }
            }

            /// <summary>
            /// Represents a base class for all of the components in a DmsService object.
            /// </summary>
            internal class ServiceParamsConfiguration
            {
                private readonly System.Collections.Generic.Dictionary<System.Int32, Skyline.DataMiner.Library.Common.ParamConfiguration> includedParams;
                /// <summary>
                /// Initializes a new instance of the <see cref = "ServiceParamsConfiguration"/> class. Parameter settings need to be added to create the service.
                /// </summary>
                internal ServiceParamsConfiguration()
                {
                    includedParams = new System.Collections.Generic.Dictionary<System.Int32, Skyline.DataMiner.Library.Common.ParamConfiguration>();
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ServiceParamsConfiguration"/> class.
                /// </summary>
                /// <param name = "includedElements">The elements and services that need to be included in the configuration.</param>
                internal ServiceParamsConfiguration(Skyline.DataMiner.Net.Messages.ServiceInfoParams[] includedElements)
                {
                    includedParams = new System.Collections.Generic.Dictionary<System.Int32, Skyline.DataMiner.Library.Common.ParamConfiguration>();
                    foreach (var item in includedElements)
                    {
                        Skyline.DataMiner.Library.Common.ParamConfiguration includedElement;
                        int index = GetNextIndex(item.Index);
                        if (item.IsService)
                        {
                            includedElement = new Skyline.DataMiner.Library.Common.ServiceParamConfiguration(this, item, index);
                        }
                        else
                        {
                            includedElement = new Skyline.DataMiner.Library.Common.ElementParamConfiguration(this, item, index);
                        }

                        includedParams[index] = includedElement;
                    }
                }

                /// <summary>
                /// Returns the string representation of the object.
                /// </summary>
                /// <returns>String representation of the object.</returns>
                public override string ToString()
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendLine("PARAM SETTINGS:");
                    sb.AppendLine("==========================");
                    foreach (Skyline.DataMiner.Library.Common.ParamConfiguration includedElement in includedParams.Values)
                    {
                        sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Included Element: ({0}){1}{2}", includedElement.Index, includedElement.Alias, System.Environment.NewLine);
                    }

                    return sb.ToString();
                }

                /// <summary>
                /// Gets the included elements and parameters.
                /// If it is a service it can be casted to a <see cref = "ServiceParamConfiguration"/>, if not to a <see cref = "ElementParamConfiguration"/> instance.
                /// </summary>
                /// <returns>The included elements and/or services.</returns>
                internal Skyline.DataMiner.Library.Common.ParamConfiguration[] GetIncludedElements()
                {
                    return System.Linq.Enumerable.ToArray(includedParams.Values);
                }

                /// <summary>
                /// Indicates whether the alias is already in use by an included element or service.
                /// </summary>
                /// <param name = "alias">The alias to check.</param>
                /// <returns><c>true</c> if the alias is being used for an included element or service; otherwise, <c>false</c>.</returns>
                internal bool ContainsAlias(string alias)
                {
                    foreach (var includedParam in includedParams.Values)
                    {
                        if (includedParam.Alias == alias)
                        {
                            return true;
                        }
                    }

                    return false;
                }

                /// <summary>
                /// Uses the suggested entry if not in use or gives the next index that can be used.
                /// </summary>
                /// <param name = "suggested">The suggested index.</param>
                /// <returns>The index that can be used. If the suggested index is available this one will be used.</returns>
                /// <exception cref = "ServiceOverflowException">The service configuration contains too many items.</exception>
                private int GetNextIndex(int suggested)
                {
                    if (includedParams.ContainsKey(suggested))
                    {
                        return GetNextIndex();
                    }

                    return suggested;
                }

                /// <summary>
                /// Gives the next index that can be used.
                /// </summary>
                /// <returns>The next index that is not in use.</returns>
                /// <exception cref = "ServiceOverflowException">The service configuration contains too many items.</exception>
                private int GetNextIndex()
                {
                    // A service should never contain more than 1000 entries.
                    for (int index = 1; index < 1000; index++)
                    {
                        if (!includedParams.ContainsKey(index))
                        {
                            return index;
                        }
                    }

                    throw new Skyline.DataMiner.Library.Common.ServiceOverflowException();
                }
            }

            /// <summary>
            /// Represents information about a connection.
            /// </summary>
            internal class DmsConnectionInfo : Skyline.DataMiner.Library.Common.IDmsConnectionInfo
            {
                /// <summary>
                /// The name of the connection.
                /// </summary>
                private readonly string name;
                /// <summary>
                /// The connection type.
                /// </summary>
                private readonly Skyline.DataMiner.Library.Common.ConnectionType type;
                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsConnectionInfo"/> class.
                /// </summary>
                /// <param name = "name">The connection name.</param>
                /// <param name = "type">The connection type.</param>
                internal DmsConnectionInfo(string name, Skyline.DataMiner.Library.Common.ConnectionType type)
                {
                    this.name = name;
                    this.type = type;
                }

                /// <summary>
                /// Gets the connection name.
                /// </summary>
                /// <value>The connection name.</value>
                public string Name
                {
                    get
                    {
                        return name;
                    }
                }

                /// <summary>
                /// Gets the connection type.
                /// </summary>
                /// <value>The connection type.</value>
                public Skyline.DataMiner.Library.Common.ConnectionType Type
                {
                    get
                    {
                        return type;
                    }
                }

                /// <summary>
                /// Returns a string that represents the current object.
                /// </summary>
                /// <returns>A string that represents the current object.</returns>
                public override string ToString()
                {
                    return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Connection with Name:{0} and Type:{1}.", name, type);
                }
            }

            /// <summary>
            /// DataMiner element connection information interface.
            /// </summary>
            public interface IDmsConnectionInfo
            {
                /// <summary>
                /// Gets the connection name.
                /// </summary>
                /// <value>The connection name.</value>
                string Name
                {
                    get;
                }

                /// <summary>
                /// Gets the connection type.
                /// </summary>
                /// <value>The connection type.</value>
                Skyline.DataMiner.Library.Common.ConnectionType Type
                {
                    get;
                }
            }

            /// <summary>
            /// Specifies the state of the Agent.
            /// </summary>
            public enum AgentState
            {
                /// <summary>
                /// Specifies the not running state.
                /// </summary>
                NotRunning = 0,
                /// <summary>
                /// Specifies the running state.
                /// </summary>
                Running = 1,
                /// <summary>
                /// Specifies the starting state.
                /// </summary>
                Starting = 2,
                /// <summary>
                /// Specifies the unknown state.
                /// </summary>
                Unknown = 3,
                /// <summary>
                /// Specifies the switching state.
                /// </summary>
                Switching = 4
            }

            /// <summary>
            /// Specifies the state of the element.
            /// </summary>
            public enum ConfigurationElementState
            {
                /// <summary>
                /// Specifies the active element state.
                /// </summary>
                Active,
                /// <summary>
                /// Specifies the paused element state.
                /// </summary>
                Paused,
                /// <summary>
                /// Specifies the stopped element state.
                /// </summary>
                Stopped
            }

            /// <summary>
            /// Specifies the connection type.
            /// </summary>
            public enum ConnectionType
            {
                /// <summary>
                /// Undefined connection type.
                /// </summary>
                Undefined = 0,
                /// <summary>
                /// SNMPv1 connection.
                /// </summary>
                SnmpV1 = 1,
                /// <summary>
                /// Serial connection.
                /// </summary>
                Serial = 2,
                /// <summary>
                /// Smart-serial connection.
                /// </summary>
                SmartSerial = 3,
                /// <summary>
                /// Virtual connection.
                /// </summary>
                Virtual = 4,
                /// <summary>
                /// GBIP (General Purpose Interface Bus) connection.
                /// </summary>
                Gpib = 5,
                /// <summary>
                /// OPC (OLE for Process Control) connection.
                /// </summary>
                Opc = 6,
                /// <summary>
                /// SLA (Service Level Agreement).
                /// </summary>
                Sla = 7,
                /// <summary>
                /// SNMPv2 connection.
                /// </summary>
                SnmpV2 = 8,
                /// <summary>
                /// SNMPv3 connection.
                /// </summary>
                SnmpV3 = 9,
                /// <summary>
                /// HTTP connection.
                /// </summary>
                Http = 10,
                /// <summary>
                /// Service.
                /// </summary>
                Service = 11,
                /// <summary>
                /// Serial single connection.
                /// </summary>
                SerialSingle = 12,
                /// <summary>
                /// Smart-serial single connection.
                /// </summary>
                SmartSerialSingle = 13,
                /// <summary>
                /// Web Socket connection.
                /// </summary>
                WebSocket = 14
            }

            /// <summary>
            /// The alarm level of an element, parameter or alarm.
            /// </summary>
            public enum AlarmLevel
            {
                /// <summary>
                /// No alarm
                /// </summary>
                Undefined = 0,
                /// <summary>
                /// Normal
                /// </summary>
                Normal = 1,
                /// <summary>
                /// Warning
                /// </summary>
                Warning = 2,
                /// <summary>
                /// Minor
                /// </summary>
                Minor = 3,
                /// <summary>
                /// Major
                /// </summary>
                Major = 4,
                /// <summary>
                /// Critical
                /// </summary>
                Critical = 5,
                /// <summary>
                /// Information
                /// </summary>
                Information = 6,
                /// <summary>
                /// Timeout
                /// </summary>
                Timeout = 7,
                /// <summary>
                /// Initial
                /// </summary>
                Initial = 8,
                /// <summary>
                /// Masked
                /// </summary>
                Masked = 9,
                /// <summary>
                /// Error
                /// </summary>
                Error = 10,
                /// <summary>
                /// Notice
                /// </summary>
                Notice = 11,
                /// <summary>
                /// Suggestion
                /// </summary>
                Suggestion = 12
            }

            /// <summary>
            /// Specifies the state of the element.
            /// </summary>
            public enum ElementState
            {
                /// <summary>
                /// Specifies the undefined element state.
                /// </summary>
                Undefined = 0,
                /// <summary>
                /// Specifies the active element state.
                /// </summary>
                Active = 1,
                /// <summary>
                /// Specifies the hidden element state.
                /// </summary>
                Hidden = 2,
                /// <summary>
                /// Specifies the paused element state.
                /// </summary>
                Paused = 3,
                /// <summary>
                /// Specifies the stopped element state.
                /// </summary>
                Stopped = 4,
                /// <summary>
                /// Specifies the deleted element state.
                /// </summary>
                Deleted = 6,
                /// <summary>
                /// Specifies the error element state.
                /// </summary>
                Error = 10,
                /// <summary>
                /// Specifies the restart element state.
                /// </summary>
                Restart = 11,
                /// <summary>
                /// Specifies the masked element state.
                /// </summary>
                Masked = 12
            }

            /// <summary>
            /// Specifies the type of the filtering.
            /// </summary>
            public enum FilterType
            {
                /// <summary>
                /// Filtering done on display key.
                /// </summary>
                Display = 0,
                /// <summary>
                /// Filtering done on primary key.
                /// </summary>
                PrimaryKey = 1
            }

            /// <summary>
            /// Specifies the property type.
            /// </summary>
            public enum PropertyType
            {
                /// <summary>
                /// Element property.
                /// </summary>
                Element = 0,
                /// <summary>
                /// View property.
                /// </summary>
                View = 1,
                /// <summary>
                /// Service property.
                /// </summary>
                Service = 2
            }

            /// <summary>
            /// Specifies the protocol type.
            /// </summary>
            public enum ProtocolType
            {
                /// <summary>
                /// Undefined protocol type.
                /// </summary>
                Undefined = 0,
                /// <summary>
                /// The SNMP protocol type.
                /// </summary>
                Snmp = 1,
                /// <summary>
                /// The SNMPv1 protocol type.
                /// </summary>
                SnmpV1 = 1,
                /// <summary>
                /// The serial protocol type.
                /// </summary>
                Serial = 2,
                /// <summary>
                /// The smart serial protocol type.
                /// </summary>
                SmartSerial = 3,
                /// <summary>
                /// The virtual protocol type.
                /// </summary>
                Virtual = 4,
                /// <summary>
                /// The General Purpose Interface Bus (GPIB) protocol type.
                /// </summary>
                Gpib = 5,
                /// <summary>
                /// The OLE Process Controller (OPC) protocol type.
                /// </summary>
                Opc = 6,
                /// <summary>
                /// The Service Level Agreement (SLA) protocol type.
                /// </summary>
                Sla = 7,
                /// <summary>
                /// The SNMPv2 protocol type.
                /// </summary>
                SnmpV2 = 8,
                /// <summary>
                /// The SNMPv3 protocol type.
                /// </summary>
                SnmpV3 = 9,
                /// <summary>
                /// The HTTP protocol type.
                /// </summary>
                Http = 10,
                /// <summary>
                /// The service protocol type.
                /// </summary>
                Service = 11,
                /// <summary>
                /// The serial single protocol type.
                /// </summary>
                SerialSingle = 12,
                /// <summary>
                /// The smart serial single protocol type.
                /// </summary>
                SmartSerialSingle = 13,
                /// <summary>
                /// The smart serial raw protocol type.
                /// </summary>
                SmartSerialRaw = 14,
                /// <summary>
                /// The smart serial raw single protocol type.
                /// </summary>
                SmartSerialRawSingle = 15,
                /// <summary>
                /// The websocket protocol type.
                /// </summary>
                WebSocket = 16
            }

            /// <summary>
            /// The exception that is thrown when an action is performed on a DataMiner Agent that was not found.
            /// </summary>
            [System.Serializable]
            [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.Runtime.Serialization.dll")]
            public class AgentNotFoundException : Skyline.DataMiner.Library.Common.DmsException
            {
                /// <summary>
                /// Initializes a new instance of the <see cref = "AgentNotFoundException"/> class.
                /// </summary>
                public AgentNotFoundException()
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "AgentNotFoundException"/> class with a specified DataMiner Agent ID.
                /// </summary>
                /// <param name = "id">The ID of the DataMiner Agent that was not found.</param>
                public AgentNotFoundException(int id): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "The agent with ID '{0}' was not found.", id))
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "AgentNotFoundException"/> class with a specified error message.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                public AgentNotFoundException(string message): base(message)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "AgentNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
                public AgentNotFoundException(string message, System.Exception innerException): base(message, innerException)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "AgentNotFoundException"/> class with serialized data.
                /// </summary>
                /// <param name = "info">The serialization info.</param>
                /// <param name = "context">The streaming context.</param>
                /// <exception cref = "ArgumentNullException">The <paramref name = "info"/> parameter is <see langword = "null"/>.</exception>
                /// <exception cref = "SerializationException">The class name is <see langword = "null"/> or HResult is zero (0).</exception>
                /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
                protected AgentNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
                {
                }
            }

            /// <summary>
            /// The exception that is thrown when a requested alarm template was not found.
            /// </summary>
            [System.Serializable]
            [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.Runtime.Serialization.dll")]
            public class AlarmTemplateNotFoundException : Skyline.DataMiner.Library.Common.TemplateNotFoundException
            {
                /// <summary>
                /// Initializes a new instance of the <see cref = "AlarmTemplateNotFoundException"/> class.
                /// </summary>
                public AlarmTemplateNotFoundException()
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "AlarmTemplateNotFoundException"/> class.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                public AlarmTemplateNotFoundException(string message): base(message)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "AlarmTemplateNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
                public AlarmTemplateNotFoundException(string message, System.Exception innerException): base(message, innerException)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "AlarmTemplateNotFoundException"/> class.
                /// </summary>
                /// <param name = "templateName">The name of the template.</param>
                /// <param name = "protocol">The protocol this template relates to.</param>
                public AlarmTemplateNotFoundException(string templateName, Skyline.DataMiner.Library.Common.IDmsProtocol protocol): base(templateName, protocol)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "AlarmTemplateNotFoundException"/> class.
                /// </summary>
                /// <param name = "templateName">The name of the template.</param>
                /// <param name = "protocolName">The name of the protocol.</param>
                /// <param name = "protocolVersion">The version of the protocol.</param>
                public AlarmTemplateNotFoundException(string templateName, string protocolName, string protocolVersion): base(templateName, protocolName, protocolVersion)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "AlarmTemplateNotFoundException"/> class with serialized data.
                /// </summary>
                /// <param name = "info">The serialization info.</param>
                /// <param name = "context">The streaming context.</param>
                /// <exception cref = "ArgumentNullException">The <paramref name = "info"/> parameter is <see langword = "null"/>.</exception>
                /// <exception cref = "SerializationException">The class name is <see langword = "null"/> or HResult is zero (0).</exception>
                /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
                protected AlarmTemplateNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
                {
                }
            }

            /// <summary>
            /// The exception that is thrown when an exception occurs in a DataMiner System.
            /// </summary>
            [System.Serializable]
            [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.Runtime.Serialization.dll")]
            public class DmsException : System.Exception
            {
                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsException"/> class.
                /// </summary>
                public DmsException()
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsException"/> class.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                public DmsException(string message): base(message)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
                public DmsException(string message, System.Exception innerException): base(message, innerException)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsException"/> class with serialized data.
                /// </summary>
                /// <param name = "info">The serialization info.</param>
                /// <param name = "context">The streaming context.</param>
                /// <exception cref = "ArgumentNullException">The <paramref name = "info"/> parameter is <see langword = "null"/>.</exception>
                /// <exception cref = "SerializationException">The class name is <see langword = "null"/> or HResult is zero (0).</exception>
                /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
                protected DmsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
                {
                }
            }

            /// <summary>
            /// The exception that is thrown when performing actions on an element that was not found.
            /// </summary>
            [System.Serializable]
            [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.Runtime.Serialization.dll")]
            public class ElementNotFoundException : Skyline.DataMiner.Library.Common.DmsException
            {
                /// <summary>
                /// Initializes a new instance of the <see cref = "ElementNotFoundException"/> class.
                /// </summary>
                public ElementNotFoundException()
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ElementNotFoundException"/> class.
                /// </summary>
                /// <param name = "dmsElementId">The DataMiner Agent ID/element ID of the element that was not found.</param>
                public ElementNotFoundException(Skyline.DataMiner.Library.Common.DmsElementId dmsElementId): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Element with DMA ID '{0}' and element ID '{1}' was not found.", dmsElementId.AgentId, dmsElementId.ElementId))
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ElementNotFoundException"/> class.
                /// </summary>
                /// <param name = "dmaId">The ID of the DataMiner Agent that was not found.</param>
                /// <param name = "elementId">The ID of the element that was not found.</param>
                public ElementNotFoundException(int dmaId, int elementId): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Element with DMA ID '{0}' and element ID '{1}' was not found.", dmaId, elementId))
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ElementNotFoundException"/> class.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                public ElementNotFoundException(string message): base(message)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ElementNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
                public ElementNotFoundException(string message, System.Exception innerException): base(message, innerException)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ElementNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
                /// </summary>
                /// <param name = "dmsElementId">The DataMiner Agent ID/element ID of the element that was not found.</param>
                /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
                public ElementNotFoundException(Skyline.DataMiner.Library.Common.DmsElementId dmsElementId, System.Exception innerException): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Element with DMA ID '{0}' and element ID '{1}' was not found.", dmsElementId.AgentId, dmsElementId.ElementId), innerException)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ElementNotFoundException"/> class with serialized data.
                /// </summary>
                /// <param name = "info">The serialization info.</param>
                /// <param name = "context">The streaming context.</param>
                /// <exception cref = "ArgumentNullException">The <paramref name = "info"/> parameter is <see langword = "null"/>.</exception>
                /// <exception cref = "SerializationException">The class name is <see langword = "null"/> or HResult is zero (0).</exception>
                /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
                protected ElementNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
                {
                }
            }

            /// <summary>
            /// The exception that is thrown when an operation is performed on a stopped element.
            /// </summary>
            [System.Serializable]
            [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.Runtime.Serialization.dll")]
            public class ElementStoppedException : Skyline.DataMiner.Library.Common.DmsException
            {
                /// <summary>
                /// Initializes a new instance of the <see cref = "ElementStoppedException"/> class.
                /// </summary>
                public ElementStoppedException()
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ElementStoppedException"/> class.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                public ElementStoppedException(string message): base(message)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ElementStoppedException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
                /// </summary>
                /// <param name = "dmsElementId">The ID of the element.</param>
                /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
                public ElementStoppedException(Skyline.DataMiner.Library.Common.DmsElementId dmsElementId, System.Exception innerException): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "The element with ID '{0}' is stopped.", dmsElementId.Value), innerException)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ElementStoppedException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
                public ElementStoppedException(string message, System.Exception innerException): base(message, innerException)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ElementStoppedException"/> class with serialized data.
                /// </summary>
                /// <param name = "info">The serialization info.</param>
                /// <param name = "context">The streaming context.</param>
                /// <exception cref = "ArgumentNullException">The <paramref name = "info"/> parameter is <see langword = "null"/>.</exception>
                /// <exception cref = "SerializationException">The class name is <see langword = "null"/> or HResult is zero (0).</exception>
                /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
                protected ElementStoppedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
                {
                }
            }

            /// <summary>
            /// The exception that is thrown when invalid data was provided.
            /// </summary>
            [System.Serializable]
            [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.Runtime.Serialization.dll")]
            public class IncorrectDataException : Skyline.DataMiner.Library.Common.DmsException
            {
                /// <summary>
                /// Initializes a new instance of the <see cref = "IncorrectDataException"/> class.
                /// </summary>
                public IncorrectDataException()
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "IncorrectDataException"/> class.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                public IncorrectDataException(string message): base(message)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "IncorrectDataException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
                public IncorrectDataException(string message, System.Exception innerException): base(message, innerException)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "IncorrectDataException"/> class with serialized data.
                /// </summary>
                /// <param name = "info">The serialization info.</param>
                /// <param name = "context">The streaming context.</param>
                /// <exception cref = "ArgumentNullException">The <paramref name = "info"/> parameter is <see langword = "null"/>.</exception>
                /// <exception cref = "SerializationException">The class name is <see langword = "null"/> or HResult is zero (0).</exception>
                /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
                protected IncorrectDataException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
                {
                }
            }

            /// <summary>
            /// The exception that is thrown when a requested protocol was not found.
            /// </summary>
            [System.Serializable]
            [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.Runtime.Serialization.dll")]
            public class ProtocolNotFoundException : Skyline.DataMiner.Library.Common.DmsException
            {
                /// <summary>
                /// Initializes a new instance of the <see cref = "ProtocolNotFoundException"/> class.
                /// </summary>
                public ProtocolNotFoundException()
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ProtocolNotFoundException"/> class.
                /// </summary>
                /// <param name = "protocolName">The name of the protocol.</param>
                /// <param name = "protocolVersion">The version of the protocol.</param>
                public ProtocolNotFoundException(string protocolName, string protocolVersion): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Protocol with name '{0}' and version '{1}' was not found.", protocolName, protocolVersion))
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ProtocolNotFoundException"/> class.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                public ProtocolNotFoundException(string message): base(message)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ProtocolNotFoundException"/> class.
                /// </summary>
                /// <param name = "protocolName">The name of the protocol.</param>
                /// <param name = "protocolVersion">The version of the protocol.</param>
                /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
                public ProtocolNotFoundException(string protocolName, string protocolVersion, System.Exception innerException): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Protocol with name '{0}' and version '{1}' was not found.", protocolName, protocolVersion), innerException)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ProtocolNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
                public ProtocolNotFoundException(string message, System.Exception innerException): base(message, innerException)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ProtocolNotFoundException"/> class with serialized data.
                /// </summary>
                /// <param name = "info">The serialization info.</param>
                /// <param name = "context">The streaming context.</param>
                /// <exception cref = "ArgumentNullException">The <paramref name = "info"/> parameter is <see langword = "null"/>.</exception>
                /// <exception cref = "SerializationException">The class name is <see langword = "null"/> or HResult is zero (0).</exception>
                /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
                protected ProtocolNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
                {
                }
            }

            /// <summary>
            /// The exception that is thrown when performing actions on an service that has too many elements or services included.
            /// </summary>
            [System.Serializable]
            [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.Runtime.Serialization.dll")]
            public class ServiceOverflowException : Skyline.DataMiner.Library.Common.DmsException
            {
                /// <summary>
                /// Initializes a new instance of the <see cref = "ServiceOverflowException"/> class.
                /// </summary>
                public ServiceOverflowException()
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ServiceOverflowException"/> class.
                /// </summary>
                /// <param name = "dmsServiceId">The DataMiner Agent ID/service ID of the service that has too many elements or services included.</param>
                public ServiceOverflowException(Skyline.DataMiner.Library.Common.DmsServiceId dmsServiceId): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Service with DMA ID '{0}' and service ID '{1}' has too many elements or services included.", dmsServiceId.AgentId, dmsServiceId.ServiceId))
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ServiceOverflowException"/> class.
                /// </summary>
                /// <param name = "dmaId">The ID of the DataMiner Agent that has too many elements or services included.</param>
                /// <param name = "serviceId">The ID of the service that has too many elements or services included.</param>
                public ServiceOverflowException(int dmaId, int serviceId): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Service with DMA ID '{0}' and service ID '{1}' has too many elements or services included.", dmaId, serviceId))
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ServiceOverflowException"/> class.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                public ServiceOverflowException(string message): base(message)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ServiceOverflowException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
                public ServiceOverflowException(string message, System.Exception innerException): base(message, innerException)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ServiceOverflowException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
                /// </summary>
                /// <param name = "dmsServiceId">The DataMiner Agent ID/service ID of the service that has too many elements or services included.</param>
                /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
                public ServiceOverflowException(Skyline.DataMiner.Library.Common.DmsServiceId dmsServiceId, System.Exception innerException): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Service with DMA ID '{0}' and service ID '{1}' has too many elements or services included.", dmsServiceId.AgentId, dmsServiceId.ServiceId), innerException)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ServiceOverflowException"/> class with serialized data.
                /// </summary>
                /// <param name = "info">The serialization info.</param>
                /// <param name = "context">The streaming context.</param>
                /// <exception cref = "ArgumentNullException">The <paramref name = "info"/> parameter is <see langword = "null"/>.</exception>
                /// <exception cref = "SerializationException">The class name is <see langword = "null"/> or HResult is zero (0).</exception>
                /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
                protected ServiceOverflowException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
                {
                }
            }

            /// <summary>
            /// The exception that is thrown when a requested template was not found.
            /// </summary>
            [System.Serializable]
            [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.Runtime.Serialization.dll")]
            public class TemplateNotFoundException : Skyline.DataMiner.Library.Common.DmsException
            {
                /// <summary>
                /// Initializes a new instance of the <see cref = "TemplateNotFoundException"/> class.
                /// </summary>
                public TemplateNotFoundException()
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "TemplateNotFoundException"/> class.
                /// </summary>
                /// <param name = "templateName">The name of the template.</param>
                /// <param name = "protocol">The protocol this template relates to.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "protocol"/> is <see langword = "null"/>.</exception>
                public TemplateNotFoundException(string templateName, Skyline.DataMiner.Library.Common.IDmsProtocol protocol): base(BuildMessageString(templateName, protocol))
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "TemplateNotFoundException"/> class.
                /// </summary>
                /// <param name = "templateName">The name of the template.</param>
                /// <param name = "protocolName">The name of the protocol.</param>
                /// <param name = "protocolVersion">The version of the protocol.</param>
                public TemplateNotFoundException(string templateName, string protocolName, string protocolVersion): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Template \"{0}\" for protocol \"{1}\" version \"{2}\" was not found.", templateName, protocolName, protocolVersion))
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "TemplateNotFoundException"/> class.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                public TemplateNotFoundException(string message): base(message)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "TemplateNotFoundException"/> class.
                /// </summary>
                /// <param name = "templateName">The name of the template.</param>
                /// <param name = "protocolName">The name of the protocol.</param>
                /// <param name = "protocolVersion">The version of the protocol.</param>
                /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
                public TemplateNotFoundException(string templateName, string protocolName, string protocolVersion, System.Exception innerException): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Template \"{0}\" for protocol \"{1}\" version \"{2}\" was not found.", templateName, protocolName, protocolVersion), innerException)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "TemplateNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
                public TemplateNotFoundException(string message, System.Exception innerException): base(message, innerException)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "TemplateNotFoundException"/> class with serialized data.
                /// </summary>
                /// <param name = "info">The serialization info.</param>
                /// <param name = "context">The streaming context.</param>
                /// <exception cref = "ArgumentNullException">The <paramref name = "info"/> parameter is <see langword = "null"/>.</exception>
                /// <exception cref = "SerializationException">The class name is <see langword = "null"/> or HResult is zero (0).</exception>
                /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
                protected TemplateNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
                {
                }

                private static string BuildMessageString(string templateName, Skyline.DataMiner.Library.Common.IDmsProtocol protocol)
                {
                    if (protocol == null)
                    {
                        throw new System.ArgumentNullException("protocol");
                    }

                    return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Template \"{0}\" for protocol \"{1}\" version \"{2}\" was not found.", templateName, protocol.Name, protocol.Version);
                }
            }

            /// <summary>
            /// The exception that is thrown when performing actions on a view that was not found.
            /// </summary>
            [System.Serializable]
            [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.Runtime.Serialization.dll")]
            public class ViewNotFoundException : Skyline.DataMiner.Library.Common.DmsException
            {
                /// <summary>
                /// Initializes a new instance of the <see cref = "ViewNotFoundException"/> class.
                /// </summary>
                public ViewNotFoundException()
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ViewNotFoundException"/> class.
                /// </summary>
                /// <param name = "viewId">The ID of the view that was not found.</param>
                public ViewNotFoundException(int viewId): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "View with ID '{0}' was not found.", viewId))
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ViewNotFoundException"/> class.
                /// </summary>
                /// <param name = "viewId">The ID of the view that was not found.</param>
                /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
                public ViewNotFoundException(int viewId, System.Exception innerException): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "View with ID '{0}' was not found.", viewId), innerException)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ViewNotFoundException"/> class.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                public ViewNotFoundException(string message): base(message)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ViewNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
                /// </summary>
                /// <param name = "message">The error message that explains the reason for the exception.</param>
                /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
                public ViewNotFoundException(string message, System.Exception innerException): base(message, innerException)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "ViewNotFoundException"/> class with serialized data.
                /// </summary>
                /// <param name = "info">The serialization info.</param>
                /// <param name = "context">The streaming context.</param>
                /// <exception cref = "ArgumentNullException">The <paramref name = "info"/> parameter is <see langword = "null"/>.</exception>
                /// <exception cref = "SerializationException">The class name is <see langword = "null"/> or HResult is zero (0).</exception>
                /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
                protected ViewNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
                {
                }
            }

            /// <summary>
            /// Represents the parent for every type of object that can be present on a DataMiner system.
            /// </summary>
            internal abstract class DmsObject
            {
                /// <summary>
                /// The DataMiner system the object belongs to.
                /// </summary>
                protected readonly Skyline.DataMiner.Library.Common.IDms dms;
                /// <summary>
                /// List containing all of the properties that were changed.
                /// </summary>
                private readonly System.Collections.Generic.List<System.String> changedPropertyList = new System.Collections.Generic.List<System.String>();
                /// <summary>
                /// Flag stating whether the DataMiner system object has been loaded.
                /// </summary>
                private bool isLoaded;
                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsObject"/> class.
                /// </summary>
                /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
                protected DmsObject(Skyline.DataMiner.Library.Common.IDms dms)
                {
                    if (dms == null)
                    {
                        throw new System.ArgumentNullException("dms");
                    }

                    this.dms = dms;
                }

                /// <summary>
                /// Gets the DataMiner system this object belongs to.
                /// </summary>
                public Skyline.DataMiner.Library.Common.IDms Dms
                {
                    get
                    {
                        return dms;
                    }
                }

                /// <summary>
                /// Gets the list containing all of the names of the properties that are changed.
                /// </summary>
                internal System.Collections.Generic.List<System.String> ChangedPropertyList
                {
                    get
                    {
                        return changedPropertyList;
                    }
                }

                /// <summary>
                /// Gets the communication object.
                /// </summary>
                internal Skyline.DataMiner.Library.Common.ICommunication Communication
                {
                    get
                    {
                        return dms.Communication;
                    }
                }

                /// <summary>
                /// Gets or sets a value indicating whether or not the DMS object has been loaded.
                /// </summary>
                internal bool IsLoaded
                {
                    get
                    {
                        return isLoaded;
                    }

                    set
                    {
                        isLoaded = value;
                    }
                }

                /// <summary>
                /// Returns a value indicating whether the object exists in the DataMiner System.
                /// </summary>
                /// <returns><c>true</c> if the object exists in the DataMiner System; otherwise, <c>false</c>.</returns>
                public abstract bool Exists();
                /// <summary>
                /// Loads DMS object data in case the object exists and is not already loaded.
                /// </summary>
                internal void LoadOnDemand()
                {
                    if (!IsLoaded)
                    {
                        Load();
                    }
                }

                /// <summary>
                /// Loads the object.
                /// </summary>
                internal abstract void Load();
            }

            /// <summary>
            /// DataMiner object interface.
            /// </summary>
            public interface IDmsObject
            {
                /// <summary>
                /// Returns a value indicating whether the object exists in the DataMiner System.
                /// </summary>
                /// <returns><c>true</c> if the object exists in the DataMiner System; otherwise, <c>false</c>.</returns>
                bool Exists();
            }

            /// <summary>
            /// Represents a DataMiner element.
            /// </summary>
            internal class DmsElement : Skyline.DataMiner.Library.Common.DmsObject, Skyline.DataMiner.Library.Common.IDmsElement
            {
                /// <summary>
                ///     Contains the properties for the element.
                /// </summary>
                private readonly System.Collections.Generic.IDictionary<System.String, Skyline.DataMiner.Library.Common.Properties.DmsElementProperty> properties = new System.Collections.Generic.Dictionary<System.String, Skyline.DataMiner.Library.Common.Properties.DmsElementProperty>();
                /// <summary>
                ///     This list will be used to keep track of which views were assigned / removed during the life time of the element.
                /// </summary>
                private readonly System.Collections.Generic.List<System.Int32> registeredViewIds = new System.Collections.Generic.List<System.Int32>();
                /// <summary>
                ///     A set of all updated properties.
                /// </summary>
                private readonly System.Collections.Generic.HashSet<System.String> updatedProperties = new System.Collections.Generic.HashSet<System.String>();
                /// <summary>
                ///     Array of views where the element is contained in.
                /// </summary>
                private readonly System.Collections.Generic.ISet<Skyline.DataMiner.Library.Common.IDmsView> views = new Skyline.DataMiner.Library.Common.DmsViewSet();
                /// <summary>
                ///     The advanced settings.
                /// </summary>
                private Skyline.DataMiner.Library.Common.AdvancedSettings advancedSettings;
                /// <summary>
                ///     The device settings.
                /// </summary>
                private Skyline.DataMiner.Library.Common.DeviceSettings deviceSettings;
                /// <summary>
                ///     The DVE settings.
                /// </summary>
                private Skyline.DataMiner.Library.Common.DveSettings dveSettings;
                /// <summary>
                ///     Collection of connections available on the element.
                /// </summary>
                private Skyline.DataMiner.Library.Common.IElementConnectionCollection elementCommunicationConnections;
                // Keep this message in case we need to parse the element properties when the user wants to use these.
                private Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo;
                /// <summary>
                ///     The failover settings.
                /// </summary>
                private Skyline.DataMiner.Library.Common.FailoverSettings failoverSettings;
                /// <summary>
                ///     The general settings.
                /// </summary>
                private Skyline.DataMiner.Library.Common.GeneralSettings generalSettings;
                /// <summary>
                ///     Specifies whether the properties of the elementInfo object have been parsed into dedicated objects.
                /// </summary>
                private bool propertiesLoaded;
                /// <summary>
                ///     The redundancy settings.
                /// </summary>
                private Skyline.DataMiner.Library.Common.RedundancySettings redundancySettings;
                /// <summary>
                ///     The replication settings.
                /// </summary>
                private Skyline.DataMiner.Library.Common.ReplicationSettings replicationSettings;
                /// <summary>
                ///     The element components.
                /// </summary>
                private System.Collections.Generic.IList<Skyline.DataMiner.Library.Common.ElementSettings> settings;
                /// <summary>
                ///     Specifies whether the views have been loaded.
                /// </summary>
                private bool viewsLoaded;
                /// <summary>
                ///     Initializes a new instance of the <see cref = "DmsElement"/> class.
                /// </summary>
                /// <param name = "dms">Object implementing <see cref = "IDms"/> interface.</param>
                /// <param name = "dmsElementId">The system-wide element ID.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
                internal DmsElement(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Library.Common.DmsElementId dmsElementId): base(dms)
                {
                    this.Initialize();
                    this.generalSettings.DmsElementId = dmsElementId;
                }

                /// <summary>
                ///     Initializes a new instance of the <see cref = "DmsElement"/> class.
                /// </summary>
                /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
                /// <param name = "elementInfo">The element information.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentNullException"><paramref name = "elementInfo"/> is <see langword = "null"/>.</exception>
                internal DmsElement(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo): base(dms)
                {
                    if (elementInfo == null)
                    {
                        throw new System.ArgumentNullException("elementInfo");
                    }

                    this.Initialize(elementInfo);
                    this.Parse(elementInfo);
                }

                /// <summary>
                ///     Gets the advanced settings of this element.
                /// </summary>
                /// <value>The advanced settings of this element.</value>
                public Skyline.DataMiner.Library.Common.IAdvancedSettings AdvancedSettings
                {
                    get
                    {
                        return this.advancedSettings;
                    }
                }

                /// <summary>
                ///     Gets the DataMiner Agent ID.
                /// </summary>
                /// <value>The DataMiner Agent ID.</value>
                public int AgentId
                {
                    get
                    {
                        return this.generalSettings.DmsElementId.AgentId;
                    }
                }

                /// <summary>
                ///     Gets or sets the alarm template assigned to this element.
                /// </summary>
                /// <value>The alarm template assigned to this element.</value>
                /// <exception cref = "ArgumentException">
                ///     The specified alarm template is not compatible with the protocol this element
                ///     executes.
                /// </exception>
                public Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate AlarmTemplate
                {
                    get
                    {
                        return this.generalSettings.AlarmTemplate;
                    }

                    set
                    {
                        if (!Skyline.DataMiner.Library.Common.InputValidator.IsCompatibleTemplate(value, this.Protocol))
                        {
                            throw new System.ArgumentException("The specified alarm template is not compatible with the protocol this element executes.", "value");
                        }

                        this.generalSettings.AlarmTemplate = value;
                    }
                }

                /// <summary>
                ///     Gets or sets the available connections on the element.
                /// </summary>
                public Skyline.DataMiner.Library.Common.IElementConnectionCollection Connections
                {
                    get
                    {
                        return this.elementCommunicationConnections;
                    }

                    set
                    {
                        this.elementCommunicationConnections = value;
                    }
                }

                /// <summary>
                ///     Gets or sets the element description.
                /// </summary>
                /// <value>The element description.</value>
                public string Description
                {
                    get
                    {
                        return this.GeneralSettings.Description;
                    }

                    set
                    {
                        this.GeneralSettings.Description = value;
                    }
                }

                /// <summary>
                ///     Gets the system-wide element ID of the element.
                /// </summary>
                /// <value>The system-wide element ID of the element.</value>
                public Skyline.DataMiner.Library.Common.DmsElementId DmsElementId
                {
                    get
                    {
                        return this.generalSettings.DmsElementId;
                    }
                }

                /// <summary>
                ///     Gets the DVE settings of this element.
                /// </summary>
                /// <value>The DVE settings of this element.</value>
                public Skyline.DataMiner.Library.Common.IDveSettings DveSettings
                {
                    get
                    {
                        return this.dveSettings;
                    }
                }

                /// <summary>
                ///     Gets the failover settings of this element.
                /// </summary>
                /// <value>The failover settings of this element.</value>
                public Skyline.DataMiner.Library.Common.IFailoverSettings FailoverSettings
                {
                    get
                    {
                        return this.failoverSettings;
                    }
                }

                /// <summary>
                ///     Gets the DataMiner Agent that hosts this element.
                /// </summary>
                /// <value>The DataMiner Agent that hosts this element.</value>
                public Skyline.DataMiner.Library.Common.IDma Host
                {
                    get
                    {
                        return this.generalSettings.Host;
                    }
                }

                /// <summary>
                ///     Gets the element ID.
                /// </summary>
                /// <value>The element ID.</value>
                public int Id
                {
                    get
                    {
                        return this.generalSettings.DmsElementId.ElementId;
                    }
                }

                /// <summary>
                ///     Gets or sets the element name.
                /// </summary>
                /// <value>The element name.</value>
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation is empty or white space.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation exceeds 200 characters.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation contains a forbidden character.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation contains more than one '%' character.</exception>
                /// <exception cref = "NotSupportedException">A set operation is not supported on a DVE child or a derived element.</exception>
                /// <remarks>
                ///     <para>The following restrictions apply to element names:</para>
                ///     <list type = "bullet">
                ///         <item>
                ///             <para>Names may not start or end with the following characters: '.' (dot), ' ' (space).</para>
                ///         </item>
                ///         <item>
                ///             <para>
                ///                 Names may not contain the following characters: '\', '/', ':', '*', '?', '"', '&lt;', '&gt;', '|',
                ///                 '°', ';'.
                ///             </para>
                ///         </item>
                ///         <item>
                ///             <para>The following characters may not occur more than once within a name: '%' (percentage).</para>
                ///         </item>
                ///     </list>
                /// </remarks>
                public string Name
                {
                    get
                    {
                        return this.generalSettings.Name;
                    }

                    set
                    {
                        this.generalSettings.Name = Skyline.DataMiner.Library.Common.InputValidator.ValidateName(value, "value");
                    }
                }

                /// <summary>
                ///     Gets the properties of this element.
                /// </summary>
                /// <value>The element properties.</value>
                public Skyline.DataMiner.Library.Common.IPropertyCollection<Skyline.DataMiner.Library.Common.Properties.IDmsElementProperty, Skyline.DataMiner.Library.Common.Properties.IDmsElementPropertyDefinition> Properties
                {
                    get
                    {
                        this.LoadOnDemand();
                        // Parse properties using definitions from Dms.
                        if (!this.propertiesLoaded)
                        {
                            this.ParseElementProperties();
                        }

                        System.Collections.Generic.IDictionary<System.String, Skyline.DataMiner.Library.Common.Properties.IDmsElementProperty> copy = new System.Collections.Generic.Dictionary<System.String, Skyline.DataMiner.Library.Common.Properties.IDmsElementProperty>(this.properties.Count);
                        foreach (var kvp in this.properties)
                        {
                            copy.Add(kvp.Key, kvp.Value);
                        }

                        return new Skyline.DataMiner.Library.Common.PropertyCollection<Skyline.DataMiner.Library.Common.Properties.IDmsElementProperty, Skyline.DataMiner.Library.Common.Properties.IDmsElementPropertyDefinition>(copy);
                    }
                }

                /// <summary>
                ///     Gets the protocol executed by this element.
                /// </summary>
                /// <value>The protocol executed by this element.</value>
                public Skyline.DataMiner.Library.Common.IDmsProtocol Protocol
                {
                    get
                    {
                        return this.generalSettings.Protocol;
                    }
                }

                /// <summary>
                ///     Gets the redundancy settings.
                /// </summary>
                /// <value>The redundancy settings.</value>
                public Skyline.DataMiner.Library.Common.IRedundancySettings RedundancySettings
                {
                    get
                    {
                        return this.redundancySettings;
                    }
                }

                /// <summary>
                ///     Gets the replication settings.
                /// </summary>
                /// <value>The replication settings.</value>
                public Skyline.DataMiner.Library.Common.IReplicationSettings ReplicationSettings
                {
                    get
                    {
                        return this.replicationSettings;
                    }
                }

                /// <summary>
                /// Gets the spectrum component of this element.
                /// </summary>
                public Skyline.DataMiner.Library.Common.IDmsSpectrumAnalyzer SpectrumAnalyzer
                {
                    get
                    {
                        return new Skyline.DataMiner.Library.Common.DmsSpectrumAnalyzer(this);
                    }
                }

                /// <summary>
                ///     Gets the element state.
                /// </summary>
                /// <value>The element state.</value>
                public Skyline.DataMiner.Library.Common.ElementState State
                {
                    get
                    {
                        return this.GeneralSettings.State;
                    }

                    internal set
                    {
                        this.GeneralSettings.State = value;
                    }
                }

                /// <summary>
                ///     Gets or sets the trend template that is assigned to this element.
                /// </summary>
                /// <value>The trend template that is assigned to this element.</value>
                /// <exception cref = "ArgumentException">
                ///     The specified trend template is not compatible with the protocol this element
                ///     executes.
                /// </exception>
                public Skyline.DataMiner.Library.Common.Templates.IDmsTrendTemplate TrendTemplate
                {
                    get
                    {
                        return this.generalSettings.TrendTemplate;
                    }

                    set
                    {
                        if (!Skyline.DataMiner.Library.Common.InputValidator.IsCompatibleTemplate(value, this.Protocol))
                        {
                            throw new System.ArgumentException("The specified trend template is not compatible with the protocol this element executes.", "value");
                        }

                        this.generalSettings.TrendTemplate = value;
                    }
                }

                /// <summary>
                ///     Gets the type of the element.
                /// </summary>
                /// <value>The element type.</value>
                public string Type
                {
                    get
                    {
                        return this.deviceSettings.Type;
                    }
                }

                /// <summary>
                ///     Gets the views the element is part of.
                /// </summary>
                /// <value>The views the element is part of.</value>
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation is an empty collection.</exception>
                public System.Collections.Generic.ISet<Skyline.DataMiner.Library.Common.IDmsView> Views
                {
                    get
                    {
                        if (!this.viewsLoaded)
                        {
                            this.LoadViews();
                        }

                        return this.views;
                    }
                }

                /// <summary>
                ///     Gets the general settings of the element.
                /// </summary>
                internal Skyline.DataMiner.Library.Common.GeneralSettings GeneralSettings
                {
                    get
                    {
                        return this.generalSettings;
                    }
                }

                /// <summary>
                ///     Determines whether this DataMiner element exists in the DataMiner System.
                /// </summary>
                /// <returns><c>true</c> if the DataMiner element exists in the DataMiner System; otherwise, <c>false</c>.</returns>
                public override bool Exists()
                {
                    return this.Dms.ElementExists(this.DmsElementId);
                }

                /// <summary>
                ///     Gets the specified standalone parameter.
                /// </summary>
                /// <typeparam name = "T">The type of the parameter. Currently supported types: int?, double?, DateTime? and string.</typeparam>
                /// <param name = "parameterId">The parameter ID.</param>
                /// <exception cref = "ArgumentException"><paramref name = "parameterId"/> is invalid.</exception>
                /// <exception cref = "ElementNotFoundException">The element was not found in the DataMiner System.</exception>
                /// <exception cref = "ElementStoppedException">The element is stopped.</exception>
                /// <exception cref = "NotSupportedException">A type other than string, int?, double? or DateTime? was provided.</exception>
                /// <returns>The standalone parameter that corresponds with the specified ID.</returns>
                public Skyline.DataMiner.Library.Common.IDmsStandaloneParameter<T> GetStandaloneParameter<T>(int parameterId)
                {
                    if (parameterId < 1)
                    {
                        throw new System.ArgumentException("Invalid parameter ID.", "parameterId");
                    }

                    System.Type type = typeof(T);
                    if (type != typeof(string) && type != typeof(int? ) && type != typeof(double? ) && type != typeof(System.DateTime? ))
                    {
                        throw new System.NotSupportedException("Only one of the following types is supported: string, int?, double? or DateTime?.");
                    }

                    Skyline.DataMiner.Library.Common.HelperClass.CheckElementState(this);
                    return new Skyline.DataMiner.Library.Common.DmsStandaloneParameter<T>(this, parameterId);
                }

                /// <summary>
                ///     Gets the specified table.
                /// </summary>
                /// <param name = "tableId">The table parameter ID.</param>
                /// <exception cref = "ArgumentException"><paramref name = "tableId"/> is invalid.</exception>
                /// <exception cref = "ElementNotFoundException">The element was not found in the DataMiner System.</exception>
                /// <exception cref = "ElementStoppedException">The element is stopped.</exception>
                /// <returns>The table that corresponds with the specified ID.</returns>
                public Skyline.DataMiner.Library.Common.IDmsTable GetTable(int tableId)
                {
                    Skyline.DataMiner.Library.Common.HelperClass.CheckElementState(this);
                    if (tableId < 1)
                    {
                        throw new System.ArgumentException("Invalid table ID.", "tableId");
                    }

                    return new Skyline.DataMiner.Library.Common.DmsTable(this, tableId);
                }

                /// <summary>
                ///     Returns a string that represents the current object.
                /// </summary>
                /// <returns>A string that represents the current object.</returns>
                public override string ToString()
                {
                    var sb = new System.Text.StringBuilder();
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Name: {0}{1}", this.Name, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "agent ID/element ID: {0}{1}", this.DmsElementId.Value, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Description: {0}{1}", this.Description, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Protocol name: {0}{1}", this.Protocol.Name, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Protocol version: {0}{1}", this.Protocol.Version, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Hosting agent ID: {0}{1}", this.Host.Id, System.Environment.NewLine);
                    return sb.ToString();
                }

                /// <summary>
                ///     Updates the element.
                /// </summary>
                /// <exception cref = "IncorrectDataException">Invalid data was set.</exception>
                /// <exception cref = "ElementNotFoundException">The element was not found in the DataMiner system.</exception>
                public void Update()
                {
                    if (this.generalSettings.State == Skyline.DataMiner.Library.Common.ElementState.Deleted)
                    {
                        throw new Skyline.DataMiner.Library.Common.ElementNotFoundException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "The element with name {0} was not found on this DataMiner agent.", this.Name));
                    }

                    try
                    {
                        // Use this flag to see if we actually have to perform an update on the element.
                        if (this.UpdateRequired())
                        {
                            if (this.ViewsRequireUpdate() && this.views.Count == 0)
                            {
                                throw new Skyline.DataMiner.Library.Common.IncorrectDataException("Views must not be empty; an element must belong to at least one view.");
                            }

                            Skyline.DataMiner.Library.Common.IDma targetDma = this.Host;
                            bool isCompatibilityIssueDetected = targetDma.IsVersionHigher(Skyline.DataMiner.Library.Common.Dma.SnmpV3AuthenticationChangeDMAVersion);
                            Skyline.DataMiner.Net.Messages.AddElementMessage message = this.CreateUpdateMessage(isCompatibilityIssueDetected);
                            this.Communication.SendSingleResponseMessage(message);
                            this.ClearChangeList();
                            // Performed the update, so tell the element to refresh.
                            this.IsLoaded = false;
                            this.viewsLoaded = false;
                            this.propertiesLoaded = false;
                        }
                    }
                    catch (Skyline.DataMiner.Net.Exceptions.DataMinerException e)
                    {
                        this.IsLoaded = false;
                        this.viewsLoaded = false;
                        this.propertiesLoaded = false;
                        if (!this.Exists())
                        {
                            this.generalSettings.State = Skyline.DataMiner.Library.Common.ElementState.Deleted;
                            throw new Skyline.DataMiner.Library.Common.ElementNotFoundException(this.DmsElementId, e);
                        }

                        throw;
                    }
                }

                /// <summary>
                ///     Loads all the data and properties found related to the element.
                /// </summary>
                /// <exception cref = "ElementNotFoundException">The element was not found in the DataMiner system.</exception>
                internal override void Load()
                {
                    try
                    {
                        this.IsLoaded = true;
                        var message = new Skyline.DataMiner.Net.Messages.GetElementByIDMessage(this.generalSettings.DmsElementId.AgentId, this.generalSettings.DmsElementId.ElementId);
                        var response = (Skyline.DataMiner.Net.Messages.ElementInfoEventMessage)this.Communication.SendSingleResponseMessage(message);
                        this.elementCommunicationConnections = new Skyline.DataMiner.Library.Common.ElementConnectionCollection(response);
                        this.Parse(response);
                    }
                    catch (Skyline.DataMiner.Net.Exceptions.DataMinerException e)
                    {
                        if (e.ErrorCode == -2146233088)
                        {
                            // 0x80131500, Element "[element ID]" is unavailable.
                            throw new Skyline.DataMiner.Library.Common.ElementNotFoundException(this.DmsElementId, e);
                        }

                        throw;
                    }
                }

                /// <summary>
                ///     Loads all the views where this element is included.
                /// </summary>
                internal void LoadViews()
                {
                    var message = new Skyline.DataMiner.Net.Messages.GetViewsForElementMessage{DataMinerID = this.generalSettings.DmsElementId.AgentId, ElementID = this.generalSettings.DmsElementId.ElementId};
                    var response = (Skyline.DataMiner.Net.Messages.GetViewsForElementResponse)this.Communication.SendSingleResponseMessage(message);
                    this.views.Clear();
                    this.registeredViewIds.Clear();
                    foreach (Skyline.DataMiner.Net.Messages.DataMinerObjectInfo info in response.Views)
                    {
                        var view = new Skyline.DataMiner.Library.Common.DmsView(this.dms, info.ID, info.Name);
                        this.registeredViewIds.Add(info.ID);
                        this.views.Add(view);
                    }

                    this.viewsLoaded = true;
                }

                /// <summary>
                ///     Parses all of the element info.
                /// </summary>
                /// <param name = "elementInfo">The element info message.</param>
                internal void Parse(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
                {
                    this.IsLoaded = true;
                    try
                    {
                        this.ParseElementInfo(elementInfo);
                    }
                    catch
                    {
                        this.IsLoaded = false;
                        throw;
                    }
                }

                /// <summary>
                ///     Update the updataProperties HashSet with a change event.
                /// </summary>
                /// <param name = "sender"></param>
                /// <param name = "e"></param>
                internal void PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
                {
                    this.updatedProperties.Add(e.PropertyName);
                }

                /// <summary>
                ///     Clears all of the queued updated properties.
                /// </summary>
                private void ClearChangeList()
                {
                    this.ChangedPropertyList.Clear();
                    foreach (Skyline.DataMiner.Library.Common.ElementSettings setting in this.settings)
                    {
                        setting.ClearUpdates();
                    }

                    this.updatedProperties.Clear();
                    this.elementCommunicationConnections.Clear();
                    // If the update passes, update the list of registered views for the element.
                    this.registeredViewIds.Clear();
                    this.registeredViewIds.AddRange(System.Linq.Enumerable.Select(this.views, v => v.Id));
                }

                /// <summary>
                ///     Creates the AddElementMessage based on the current state of the object.
                /// </summary>
                /// <returns>The AddElementMessage.</returns>
                private Skyline.DataMiner.Net.Messages.AddElementMessage CreateUpdateMessage(bool isCompatibilityIssueDetected)
                {
                    var message = new Skyline.DataMiner.Net.Messages.AddElementMessage{ElementID = this.DmsElementId.ElementId, DataMinerID = this.DmsElementId.AgentId};
                    if (!this.dveSettings.IsChild)
                    {
                        message.ProtocolName = this.Protocol.Name;
                        message.ProtocolVersion = this.Protocol.Version;
                    }

                    foreach (Skyline.DataMiner.Library.Common.ElementSettings setting in this.settings)
                    {
                        if (setting.Updated)
                        {
                            setting.FillUpdate(message);
                        }
                    }

                    // Update connection info if change has been detected.
                    var connections = (Skyline.DataMiner.Library.Common.ElementConnectionCollection)this.elementCommunicationConnections;
                    if (connections.IsUpdateRequired())
                    {
                        Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elemInfo;
                        if (this.elementInfo == null)
                        {
                            var msg = new Skyline.DataMiner.Net.Messages.GetElementByIDMessage(this.DmsElementId.AgentId, this.DmsElementId.ElementId);
                            elemInfo = (Skyline.DataMiner.Net.Messages.ElementInfoEventMessage)this.Communication.SendSingleResponseMessage(msg);
                        }
                        else
                        {
                            elemInfo = this.elementInfo;
                        }

                        var elementPortInfos = Skyline.DataMiner.Library.Common.HelperClass.ObtainElementPortInfos(elemInfo);
                        connections.UpdatePortInfo(elementPortInfos, isCompatibilityIssueDetected);
                        message.Ports = elementPortInfos;
                    }

                    if (this.ViewsRequireUpdate())
                    {
                        message.ViewIDs = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Select(this.views, v => v.Id));
                    }

                    var newPropertyValues = new System.Collections.Generic.List<Skyline.DataMiner.Net.Messages.PropertyInfo>();
                    foreach (string propertyName in this.updatedProperties)
                    {
                        Skyline.DataMiner.Library.Common.Properties.DmsElementProperty property = this.properties[propertyName];
                        newPropertyValues.Add(new Skyline.DataMiner.Net.Messages.PropertyInfo{DataType = "Element", Value = property.Value, Name = property.Definition.Name, AccessType = Skyline.DataMiner.Net.Messages.PropertyAccessType.ReadWrite});
                    }

                    if (System.Linq.Enumerable.Any(newPropertyValues))
                    {
                        message.Properties = newPropertyValues.ToArray();
                    }

                    return message;
                }

                /// <summary>
                ///     Initializes the element.
                /// </summary>
                private void Initialize(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
                {
                    this.elementInfo = elementInfo;
                    this.Initialize();
                    this.elementCommunicationConnections = new Skyline.DataMiner.Library.Common.ElementConnectionCollection(this.elementInfo);
                }

                /// <summary>
                ///     Initializes the element.
                /// </summary>
                private void Initialize()
                {
                    this.generalSettings = new Skyline.DataMiner.Library.Common.GeneralSettings(this);
                    this.deviceSettings = new Skyline.DataMiner.Library.Common.DeviceSettings(this);
                    this.replicationSettings = new Skyline.DataMiner.Library.Common.ReplicationSettings(this);
                    this.advancedSettings = new Skyline.DataMiner.Library.Common.AdvancedSettings(this);
                    this.failoverSettings = new Skyline.DataMiner.Library.Common.FailoverSettings(this);
                    this.redundancySettings = new Skyline.DataMiner.Library.Common.RedundancySettings(this);
                    this.dveSettings = new Skyline.DataMiner.Library.Common.DveSettings(this);
                    this.settings = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.ElementSettings>{this.generalSettings, this.deviceSettings, this.replicationSettings, this.advancedSettings, this.failoverSettings, this.redundancySettings, this.dveSettings};
                }

                /// <summary>
                ///     Parse an ElementPortInfo object in order to add IElementConnection objects to the ElementConnectionCollection.
                /// </summary>
                /// <param name = "info">The ElementPortInfo object.</param>
                private void ParseConnection(Skyline.DataMiner.Net.Messages.ElementPortInfo info)
                {
                    switch (info.ProtocolType)
                    {
                        case Skyline.DataMiner.Net.Messages.ProtocolType.Virtual:
                            var myVirtualConnection = new Skyline.DataMiner.Library.Common.VirtualConnection(info);
                            this.elementCommunicationConnections[info.PortID] = myVirtualConnection;
                            break;
                        case Skyline.DataMiner.Net.Messages.ProtocolType.SnmpV1:
                            var mySnmpV1Connection = new Skyline.DataMiner.Library.Common.SnmpV1Connection(info);
                            this.elementCommunicationConnections[info.PortID] = mySnmpV1Connection;
                            break;
                        case Skyline.DataMiner.Net.Messages.ProtocolType.SnmpV2:
                            var mySnmpv2Connection = new Skyline.DataMiner.Library.Common.SnmpV2Connection(info);
                            this.elementCommunicationConnections[info.PortID] = mySnmpv2Connection;
                            break;
                        case Skyline.DataMiner.Net.Messages.ProtocolType.SnmpV3:
                            var mySnmpV3Connection = new Skyline.DataMiner.Library.Common.SnmpV3Connection(info);
                            this.elementCommunicationConnections[info.PortID] = mySnmpV3Connection;
                            break;
                        case Skyline.DataMiner.Net.Messages.ProtocolType.Http:
                            var myHttpConnection = new Skyline.DataMiner.Library.Common.HttpConnection(info);
                            this.elementCommunicationConnections[info.PortID] = myHttpConnection;
                            break;
                        default:
                            var myConnection = new Skyline.DataMiner.Library.Common.RealConnection(info);
                            this.elementCommunicationConnections[info.PortID] = myConnection;
                            break;
                    }
                }

                /// <summary>
                ///     Parse an ElementInfoEventMessage object.
                /// </summary>
                /// <param name = "elementInfo"></param>
                private void ParseConnections(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
                {
                    // Keep this object in case properties are accessed.
                    this.elementInfo = elementInfo;
                    this.ParseConnection(elementInfo.MainPort);
                    if (elementInfo.ExtraPorts != null)
                    {
                        foreach (Skyline.DataMiner.Net.Messages.ElementPortInfo info in elementInfo.ExtraPorts)
                        {
                            this.ParseConnection(info);
                        }
                    }
                }

                /// <summary>
                ///     Parses the element info.
                /// </summary>
                /// <param name = "elementInfo">The element info.</param>
                private void ParseElementInfo(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
                {
                    // Keep this object in case properties are accessed.
                    this.elementInfo = elementInfo;
                    foreach (Skyline.DataMiner.Library.Common.ElementSettings component in this.settings)
                    {
                        component.Load(elementInfo);
                    }

                    this.ParseConnections(elementInfo);
                }

                /// <summary>
                ///     Parses the element properties.
                /// </summary>
                private void ParseElementProperties()
                {
                    this.properties.Clear();
                    foreach (Skyline.DataMiner.Library.Common.Properties.IDmsElementPropertyDefinition definition in this.Dms.ElementPropertyDefinitions)
                    {
                        Skyline.DataMiner.Net.Messages.PropertyInfo info = null;
                        if (this.elementInfo.Properties != null)
                        {
                            info = System.Linq.Enumerable.FirstOrDefault(this.elementInfo.Properties, p => p.Name.Equals(definition.Name, System.StringComparison.OrdinalIgnoreCase));
                            var duplicates = System.Linq.Enumerable.ToList(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(System.Linq.Enumerable.GroupBy(this.elementInfo.Properties, p => p.Name), g => System.Linq.Enumerable.Count(g) > 1), g => g.Key));
                            if (System.Linq.Enumerable.Any(duplicates))
                            {
                                string message = "Duplicate element properties detected. Element \"" + this.elementInfo.Name + "\" (" + this.elementInfo.DataMinerID + "/" + this.elementInfo.ElementID + "), duplicate properties: " + string.Join(", ", duplicates) + ".";
                                Skyline.DataMiner.Library.Common.Logger.Log(message);
                            }
                        }

                        string propertyValue = info != null ? info.Value : System.String.Empty;
                        if (definition.IsReadOnly)
                        {
                            this.properties.Add(definition.Name, new Skyline.DataMiner.Library.Common.Properties.DmsElementProperty(this, definition, propertyValue));
                        }
                        else
                        {
                            var property = new Skyline.DataMiner.Library.Common.Properties.DmsWritableElementProperty(this, definition, propertyValue);
                            this.properties.Add(definition.Name, property);
                            property.PropertyChanged += this.PropertyChanged;
                        }
                    }

                    this.propertiesLoaded = true;
                }

                /// <summary>
                ///     Specifies if the element requires an update or not.
                /// </summary>
                /// <returns><c>true</c> if an update is required; otherwise, <c>false</c>.</returns>
                private bool UpdateRequired()
                {
                    bool settingsChanged = System.Linq.Enumerable.Any(this.settings, s => s.Updated) || this.updatedProperties.Count != 0 || this.ChangedPropertyList.Count != 0 || this.ViewsRequireUpdate();
                    bool connectionInfoChanged = this.elementCommunicationConnections.IsUpdateRequired();
                    return settingsChanged || connectionInfoChanged;
                }

                /// <summary>
                ///     Specifies if the views of the element have been updated.
                /// </summary>
                /// <returns><c>true</c> if the views have been updated; otherwise, <c>false</c>.</returns>
                private bool ViewsRequireUpdate()
                {
                    if (this.views.Count != this.registeredViewIds.Count)
                    {
                        return true;
                    }

                    var ids = System.Linq.Enumerable.ToList(System.Linq.Enumerable.Select(this.views, t => t.Id));
                    var distinctOne = System.Linq.Enumerable.Except(ids, this.registeredViewIds);
                    var distinctTwo = System.Linq.Enumerable.Except(this.registeredViewIds, ids);
                    return System.Linq.Enumerable.Any(distinctOne) || System.Linq.Enumerable.Any(distinctTwo);
                }
            }

            /// <summary>
            /// Represents a set of <see cref = "IDmsView"/> items.
            /// </summary>
            [System.Serializable]
            public sealed class DmsViewSet : System.Collections.Generic.ISet<Skyline.DataMiner.Library.Common.IDmsView>
            {
                /// <summary>
                /// The views in the set.
                /// </summary>
                private readonly System.Collections.Generic.HashSet<Skyline.DataMiner.Library.Common.IDmsView> views = new System.Collections.Generic.HashSet<Skyline.DataMiner.Library.Common.IDmsView>(new Skyline.DataMiner.Library.Common.DmsViewEqualityComparer());
                /// <summary>
                /// Gets the number of views that are contained in a set.
                /// </summary>
                /// <value>The number of views that are contained in the set.</value>
                public int Count
                {
                    get
                    {
                        return views.Count;
                    }
                }

                /// <summary>
                /// Gets a value indicating whether a collection is read-only.
                /// </summary>
                /// <value><c>true</c> if the collection is read-only; otherwise, <c>false</c>.</value>
                bool System.Collections.Generic.ICollection<Skyline.DataMiner.Library.Common.IDmsView>.IsReadOnly
                {
                    get
                    {
                        return ((System.Collections.Generic.ICollection<Skyline.DataMiner.Library.Common.IDmsView>)views).IsReadOnly;
                    }
                }

                /// <summary>
                /// Adds the specified item to a set.
                /// </summary>
                /// <param name = "item">The item to add to the set.</param>
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                /// <returns><c>true</c> if the item is added to the set; <c>false</c> if the item is already present.</returns>
                public bool Add(Skyline.DataMiner.Library.Common.IDmsView item)
                {
                    if (item == null)
                    {
                        throw new System.ArgumentNullException("item");
                    }

                    return views.Add(item);
                }

                /// <summary>
                /// Adds the specified item to a set.
                /// </summary>
                /// <param name = "item">The item to add to the set.</param>
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                void System.Collections.Generic.ICollection<Skyline.DataMiner.Library.Common.IDmsView>.Add(Skyline.DataMiner.Library.Common.IDmsView item)
                {
                    if (item == null)
                    {
                        throw new System.ArgumentNullException("item");
                    }

                    views.Add(item);
                }

                /// <summary>
                /// Removes all items from the collection.
                /// </summary>
                /// <remarks>
                /// <para>This method is an O(n) operation, where <c>n</c> is Count.</para>
                /// </remarks>
                public void Clear()
                {
                    views.Clear();
                }

                /// <summary>
                /// Determines whether the collection contains the specified item.
                /// </summary>
                /// <param name = "item">The item to locate in the set.</param>
                /// <returns><c>true</c> if the collection contains the specified item; otherwise, <c>false</c>.</returns>
                /// <remarks>This method is an O(1) operation.</remarks>
                public bool Contains(Skyline.DataMiner.Library.Common.IDmsView item)
                {
                    return views.Contains(item);
                }

                /// <summary>
                /// Copies the items of a ICollection&lt;IDmsView&gt; object to an array, starting at the specified array index.
                /// </summary>
                /// <param name = "array">The one-dimensional array that is the destination of the items copied from the object. The array must have zero-based indexing.</param>
                /// <param name = "arrayIndex">The zero-based index in array at which copying begins.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "array"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentOutOfRangeException"><paramref name = "arrayIndex"/> is less than 0.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "arrayIndex"/> is greater than the length of the destination <paramref name = "array"/>.</exception>
                public void CopyTo(Skyline.DataMiner.Library.Common.IDmsView[] array, int arrayIndex)
                {
                    views.CopyTo(array, arrayIndex);
                }

                /// <summary>
                /// Returns an enumerator that iterates through the collection object.
                /// </summary>
                /// <returns>A enumerator object for the object.</returns>
                public System.Collections.Generic.IEnumerator<Skyline.DataMiner.Library.Common.IDmsView> GetEnumerator()
                {
                    return views.GetEnumerator();
                }

                /// <summary>
                /// Removes the specified item from the collection.
                /// </summary>
                /// <param name = "item">The item to remove.</param>
                /// <returns><c>true</c> if the item is successfully found and removed; otherwise, <c>false</c>. This method returns <c>false</c> if the item is not found in the collection.</returns>
                public bool Remove(Skyline.DataMiner.Library.Common.IDmsView item)
                {
                    return views.Remove(item);
                }

                /// <summary>
                /// Returns an enumerator that iterates through a collection.
                /// </summary>
                /// <returns>An <see cref = "IEnumerator&lt;T&gt;"/> object that can be used to iterate through the collection.</returns>
                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
                {
                    return ((System.Collections.IEnumerable)views).GetEnumerator();
                }

                /// <summary>
                /// Modifies the current set to contain all items that are present in itself, the specified collection, or both.
                /// </summary>
                /// <param name = "other">The collection to compare to the current set.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "other"/> is <see langword = "null"/>.</exception>
                public void UnionWith(System.Collections.Generic.IEnumerable<Skyline.DataMiner.Library.Common.IDmsView> other)
                {
                    views.UnionWith(other);
                }

                /// <summary>
                /// Modifies the current set to contain only items that are present in that object and in the specified collection.
                /// </summary>
                /// <param name = "other">The collection to compare to the current set.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "other"/> is <see langword = "null"/>.</exception>
                public void IntersectWith(System.Collections.Generic.IEnumerable<Skyline.DataMiner.Library.Common.IDmsView> other)
                {
                    views.IntersectWith(other);
                }

                /// <summary>
                /// Removes all items in the specified collection from the current set.
                /// </summary>
                /// <param name = "other">The collection of items to remove from the set.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "other"/> is <see langword = "null"/>.</exception>
                /// <remarks>
                /// <para>The ExceptWith method is the equivalent of mathematical set subtraction.</para>
                /// <para>This method is an O(<c>n</c>) operation, where <c>n</c> is the number of elements in the <c>other</c> parameter.</para>
                /// </remarks>
                public void ExceptWith(System.Collections.Generic.IEnumerable<Skyline.DataMiner.Library.Common.IDmsView> other)
                {
                    views.ExceptWith(other);
                }

                /// <summary>
                /// Modifies the current set to contain only items that are present either in this object or in the specified collection, but not both.
                /// </summary>
                /// <param name = "other">The collection to compare to the current object.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "other"/> is <see langword = "null"/>.</exception>
                /// <remarks>
                /// If the other parameter is a collection with the same equality comparer as the current object, this method is an O(n) operation.
                ///  Otherwise, this method is an O(n + m) operation, where n is the number of items in <paramref name = "other"/> and m is Count.
                /// </remarks>
                public void SymmetricExceptWith(System.Collections.Generic.IEnumerable<Skyline.DataMiner.Library.Common.IDmsView> other)
                {
                    views.SymmetricExceptWith(other);
                }

                /// <summary>
                /// Determines whether this set is a subset of the specified collection.
                /// </summary>
                /// <param name = "other">The collection to compare to the current object.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "other"/> is <see langword = "null"/>.</exception>
                /// <returns><c>true</c> if this object is a subset of other; otherwise, <c>false</c>.</returns>
                /// <remarks>
                /// <para>An empty set is a subset of any other collection, including an empty set.
                /// Therefore, this method returns true if the collection represented by the current object is empty, even if the other parameter is an empty set.</para>
                /// <para>This method always returns false if Count is greater than the number of items in <paramref name = "other"/>.</para>
                /// <para>If the collection represented by other is a collection with the same equality comparer as the current object, this method is an O(n) operation.
                /// Otherwise, this method is an O(n + m) operation, where n is Count and m is the number of items in <paramref name = "other"/>.</para>
                /// </remarks>
                public bool IsSubsetOf(System.Collections.Generic.IEnumerable<Skyline.DataMiner.Library.Common.IDmsView> other)
                {
                    return views.IsSubsetOf(other);
                }

                /// <summary>
                /// Determines whether this object is a superset of the specified collection.
                /// </summary>
                /// <param name = "other">The collection to compare to the current object.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "other"/> is <see langword = "null"/>.</exception>
                /// <returns><c>true</c> if the object is a superset of other; otherwise, <c>false</c>.</returns>
                /// <remarks>
                /// <para>All collections, including the empty set, are supersets of the empty set.
                /// Therefore, this method returns true if the collection represented by the other parameter is empty, even if the current object is empty.</para>
                /// <para>This method always returns false if Count is less than the number of items in <paramref name = "other"/>.</para>
                /// <para>If the collection represented by other is a collection with the same equality comparer as the current object, this method is an O(n) operation.
                ///  Otherwise, this method is an O(n + m) operation, where n is the number of items in <paramref name = "other"/> and m is Count.</para>
                /// </remarks>
                public bool IsSupersetOf(System.Collections.Generic.IEnumerable<Skyline.DataMiner.Library.Common.IDmsView> other)
                {
                    return views.IsSupersetOf(other);
                }

                /// <summary>
                /// Determines whether the object is a proper superset of the specified collection.
                /// </summary>
                /// <param name = "other">The collection to compare to the current object.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "other"/> is <see langword = "null"/>.</exception>
                /// <returns><c>true</c> if this object is a proper superset of other; otherwise, <c>false</c>.</returns>
                /// <remarks>
                /// <para>An empty set is a proper superset of any other collection.
                /// Therefore, this method returns true if the collection represented by the other parameter is empty unless the current collection is also empty.</para>
                /// <para>This method always returns <c>false</c> if Count is less than or equal to the number of elements in other.</para>
                /// <para>If the collection represented by other is a collection with the same equality comparer as the current object, this method is an O(n) operation.
                ///  Otherwise, this method is an O(n + m) operation, where n is the number of elements in other and m is Count.</para>
                /// </remarks>
                public bool IsProperSupersetOf(System.Collections.Generic.IEnumerable<Skyline.DataMiner.Library.Common.IDmsView> other)
                {
                    return views.IsProperSupersetOf(other);
                }

                /// <summary>
                /// Determines whether this object is a proper subset of the specified collection.
                /// </summary>
                /// <param name = "other">The collection to compare to the current object.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "other"/> is <see langword = "null"/>.</exception>
                /// <returns><c>true</c> if this object is a proper subset of other; otherwise, <c>false</c>.</returns>
                /// <remarks>
                /// <para>An empty set is a proper subset of any other collection.
                /// Therefore, this method returns <c>true</c> if the collection represented by the current object is empty unless the other parameter is also an empty set.</para>
                /// <para>This method always returns <c>false</c> if Count is greater than or equal to the number of items in other.</para>
                /// <para>If the collection represented by other is a collection with the same equality comparer as the current object, then this method is an O(n) operation.
                ///  Otherwise, this method is an O(n + m) operation, where n is Count and m is the number of items in <paramref name = "other"/>.</para>
                /// </remarks>
                public bool IsProperSubsetOf(System.Collections.Generic.IEnumerable<Skyline.DataMiner.Library.Common.IDmsView> other)
                {
                    return views.IsProperSubsetOf(other);
                }

                /// <summary>
                /// Determines whether the current object and a specified collection share common items.
                /// </summary>
                /// <param name = "other">The collection to compare to the current object.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "other"/> is <see langword = "null"/>.</exception>
                /// <returns><c>true</c> if this object and other share at least one common element; otherwise, <c>false</c>.</returns>
                /// <remarks>
                /// This method is an O(<c>n</c>) operation, where <c>n</c> is the number of items in <paramref name = "other"/>.
                /// </remarks>
                public bool Overlaps(System.Collections.Generic.IEnumerable<Skyline.DataMiner.Library.Common.IDmsView> other)
                {
                    return views.Overlaps(other);
                }

                /// <summary>
                /// Determines whether this object and the specified collection contain the same items.
                /// </summary>
                /// <param name = "other">The collection to compare to the current object.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "other"/> is <see langword = "null"/>.</exception>
                /// <returns>true if this set is equal to <paramref name = "other"/>; otherwise, false.</returns>
                /// <remarks>
                /// <para>The SetEquals method ignores duplicate entries and the order of items in the <paramref name = "other"/> parameter.</para>
                /// <para>If the collection represented by other is a collection with the same equality comparer as the current object, this method is an O(n) operation.
                /// Otherwise, this method is an O(n + m) operation, where n is the number of items in <paramref name = "other"/> and m is Count.</para>
                /// </remarks>
                public bool SetEquals(System.Collections.Generic.IEnumerable<Skyline.DataMiner.Library.Common.IDmsView> other)
                {
                    return views.SetEquals(other);
                }
            }

            /// <summary>
            /// DataMiner element interface.
            /// </summary>
            public interface IDmsElement : Skyline.DataMiner.Library.Common.IDmsObject, Skyline.DataMiner.Library.Common.IUpdateable
            {
                /// <summary>
                /// Gets the advanced settings of this element.
                /// </summary>
                /// <value>The advanced settings of this element.</value>
                Skyline.DataMiner.Library.Common.IAdvancedSettings AdvancedSettings
                {
                    get;
                }

                /// <summary>
                /// Gets the DataMiner Agent ID.
                /// </summary>
                /// <value>The DataMiner Agent ID.</value>
                int AgentId
                {
                    get;
                }

                /// <summary>
                /// Gets or sets the alarm template assigned to this element.
                /// </summary>
                /// <value>The alarm template assigned to this element.</value>
                /// <exception cref = "ArgumentException">The specified alarm template is not compatible with the protocol this element executes.</exception>
                Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate AlarmTemplate
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or Sets the collection of IElementConnection objects.
                /// </summary>
                Skyline.DataMiner.Library.Common.IElementConnectionCollection Connections
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the element description.
                /// </summary>
                /// <value>The element description.</value>
                string Description
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets the system-wide element ID of the element.
                /// </summary>
                /// <value>The system-wide element ID of the element.</value>
                Skyline.DataMiner.Library.Common.DmsElementId DmsElementId
                {
                    get;
                }

                /// <summary>
                /// Gets the DVE settings of this element.
                /// </summary>
                /// <value>The DVE settings of this element.</value>
                Skyline.DataMiner.Library.Common.IDveSettings DveSettings
                {
                    get;
                }

                /// <summary>
                /// Gets the DataMiner Agent that hosts this element.
                /// </summary>
                /// <value>The DataMiner Agent that hosts this element.</value>
                Skyline.DataMiner.Library.Common.IDma Host
                {
                    get;
                }

                ///// <summary>
                ///// Gets the failover settings of this element.
                ///// </summary>
                ///// <value>The failover settings of this element.</value>
                //IFailoverSettings FailoverSettings { get; }
                /// <summary>
                /// Gets the element ID.
                /// </summary>
                /// <value>The element ID.</value>
                int Id
                {
                    get;
                }

                /// <summary>
                /// Gets or sets the element name.
                /// </summary>
                /// <value>The element name.</value>
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation is empty or white space.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation exceeds 200 characters.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation contains a forbidden character.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation contains more than one '%' character.</exception>
                /// <exception cref = "NotSupportedException">A set operation is not supported on a DVE child or a derived element.</exception>
                /// <remarks>
                /// <para>The following restrictions apply to element names:</para>
                /// <list type = "bullet">
                ///		<item><para>Names may not start or end with the following characters: '.' (dot), ' ' (space).</para></item>
                ///		<item><para>Names may not contain the following characters: '\', '/', ':', '*', '?', '"', '&lt;', '&gt;', '|', '°', ';'.</para></item>
                ///		<item><para>The following characters may not occur more than once within a name: '%' (percentage).</para></item>
                /// </list>
                /// </remarks>
                string Name
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets the properties of this element.
                /// </summary>
                /// <value>The element properties.</value>
                Skyline.DataMiner.Library.Common.IPropertyCollection<Skyline.DataMiner.Library.Common.Properties.IDmsElementProperty, Skyline.DataMiner.Library.Common.Properties.IDmsElementPropertyDefinition> Properties
                {
                    get;
                }

                /// <summary>
                /// Gets the protocol executed by this element.
                /// </summary>
                /// <value>The protocol executed by this element.</value>
                Skyline.DataMiner.Library.Common.IDmsProtocol Protocol
                {
                    get;
                }

                /// <summary>
                /// Gets the redundancy settings.
                /// </summary>
                /// <value>The redundancy settings.</value>
                Skyline.DataMiner.Library.Common.IRedundancySettings RedundancySettings
                {
                    get;
                }

                /// <summary>
                /// Gets the replication settings.
                /// </summary>
                /// <value>The replication settings.</value>
                Skyline.DataMiner.Library.Common.IReplicationSettings ReplicationSettings
                {
                    get;
                }

                /// <summary>
                /// Gets the spectrum analyzer component of this element.
                /// </summary>
                /// <value>The spectrum analyzer component.</value>
                /// <remarks>This is only applicable for spectrum analyzer elements.</remarks>
                Skyline.DataMiner.Library.Common.IDmsSpectrumAnalyzer SpectrumAnalyzer
                {
                    get;
                }

                /// <summary>
                /// Gets the element state.
                /// </summary>
                /// <value>The element state.</value>
                Skyline.DataMiner.Library.Common.ElementState State
                {
                    get;
                }

                /// <summary>
                /// Gets or sets the trend template that is assigned to this element.
                /// </summary>
                /// <value>The trend template that is assigned to this element.</value>
                /// <exception cref = "ArgumentException">The specified trend template is not compatible with the protocol this element executes.</exception>
                Skyline.DataMiner.Library.Common.Templates.IDmsTrendTemplate TrendTemplate
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets the type of the element.
                /// </summary>
                /// <value>The element type.</value>
                string Type
                {
                    get;
                }

                /// <summary>
                /// Gets the views the element is part of.
                /// </summary>
                /// <value>The views the element is part of.</value>
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation is an empty collection.</exception>
                System.Collections.Generic.ISet<Skyline.DataMiner.Library.Common.IDmsView> Views
                {
                    get;
                }

                /// <summary>
                /// Gets the specified standalone parameter.
                /// </summary>
                /// <typeparam name = "T">The type of the parameter. Currently supported types: int?, double?, DateTime? and string.</typeparam>
                /// <param name = "parameterId">The parameter ID.</param>
                /// <exception cref = "ArgumentException"><paramref name = "parameterId"/> is invalid.</exception>
                /// <exception cref = "ElementNotFoundException">The element was not found in the DataMiner System.</exception>
                /// <exception cref = "ElementStoppedException">The element is not active.</exception>
                /// <exception cref = "NotSupportedException">A type other than string, int?, double? or DateTime? was provided.</exception>
                /// <returns>The standalone parameter that corresponds with the specified ID.</returns>
                Skyline.DataMiner.Library.Common.IDmsStandaloneParameter<T> GetStandaloneParameter<T>(int parameterId);
                /// <summary>
                /// Gets the specified table.
                /// </summary>
                /// <param name = "tableId">The table parameter ID.</param>
                /// <exception cref = "ArgumentException"><paramref name = "tableId"/> is invalid.</exception>
                /// <exception cref = "ElementNotFoundException">The element was not found in the DataMiner System.</exception>
                /// <exception cref = "ElementStoppedException">The element is not active.</exception>
                /// <returns>The table that corresponds with the specified ID.</returns>
                Skyline.DataMiner.Library.Common.IDmsTable GetTable(int tableId);
            }

            /// <summary>
            /// Base class for all connection related objects.
            /// </summary>
            public abstract class ConnectionSettings
            {
                /// <summary>
                /// Enum used to track changes on properties of classes implementing this abstract class.
                /// </summary>
                protected enum ConnectionSetting
                {
                    /// <summary>
                    /// GetCommunityString
                    /// </summary>
                    GetCommunityString = 0,
                    /// <summary>
                    /// SetCommunityString
                    /// </summary>
                    SetCommunityString = 1,
                    /// <summary>
                    /// DeviceAddress
                    /// </summary>
                    DeviceAddress = 2,
                    /// <summary>
                    /// Timeout
                    /// </summary>
                    Timeout = 3,
                    /// <summary>
                    /// Retries
                    /// </summary>
                    Retries = 4,
                    /// <summary>
                    /// ElementTimeout
                    /// </summary>
                    ElementTimeout = 5,
                    /// <summary>
                    /// PortConnection (e.g.Udp , Tcp)
                    /// </summary>
                    PortConnection = 6,
                    /// <summary>
                    /// SecurityConfiguration
                    /// </summary>
                    SecurityConfig = 7,
                    /// <summary>
                    /// SNMPv3 Encryption Algorithm
                    /// </summary>
                    EncryptionAlgorithm = 8,
                    /// <summary>
                    /// SNMPv3 AuthenticationProtocol
                    /// </summary>
                    AuthenticationProtocol = 9,
                    /// <summary>
                    /// SNMPv3 EncryptionKey
                    /// </summary>
                    EncryptionKey = 10,
                    /// <summary>
                    /// SNMPv3 AuthenticationKey
                    /// </summary>
                    AuthenticationKey = 11,
                    /// <summary>
                    /// SNMPv3 Username
                    /// </summary>
                    Username = 12,
                    /// <summary>
                    /// SNMPv3 Security Level and Protocol
                    /// </summary>
                    SecurityLevelAndProtocol = 13,
                    /// <summary>
                    /// Local port
                    /// </summary>
                    LocalPort = 14,
                    /// <summary>
                    /// Remote port
                    /// </summary>
                    RemotePort = 15,
                    /// <summary>
                    /// Is SSL/TLS enabled
                    /// </summary>
                    IsSslTlsEnabled = 16,
                    /// <summary>
                    /// Remote host
                    /// </summary>
                    RemoteHost = 17,
                    /// <summary>
                    /// Network interface card
                    /// </summary>
                    NetworkInterfaceCard = 18,
                    /// <summary>
                    /// Bus address
                    /// </summary>
                    BusAddress = 19,
                    /// <summary>
                    /// Is BypassProxy enabled.
                    /// </summary>
                    IsByPassProxyEnabled
                }

                /// <summary>
                /// The list of changed properties.
                /// </summary>
                private readonly System.Collections.Generic.List<Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting> changedPropertyList = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting>();
                /// <summary>
                /// Gets a value indicating whether one or more properties have been updated.
                /// </summary>
                internal abstract bool IsUpdated
                {
                    get;
                }

                /// <summary>
                /// Gets the list of updated properties.
                /// </summary>
                protected System.Collections.Generic.List<Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting> ChangedPropertyList
                {
                    get
                    {
                        return changedPropertyList;
                    }
                }

                /// <summary>
                /// Creates an ElementPortPortInfo object based on the field contents.
                /// </summary>
                /// <param name = "portPosition">The corresponding port number.</param>
                /// <param name = "isCompatibilityIssueDetected">Indicates if compatibility changes need to be taken into account.</param>
                /// <returns>ElementPortInfo object.</returns>
                internal abstract Skyline.DataMiner.Net.Messages.ElementPortInfo CreateElementPortInfo(int portPosition, bool isCompatibilityIssueDetected);
                /// <summary>
                /// Updates the provided ElementPortInfo object with any performed changes on the object.
                /// </summary>
                /// <param name = "portInfo"></param>
                /// <param name = "isCompatibilityIssueDetected"></param>
                internal abstract void UpdateElementPortInfo(Skyline.DataMiner.Net.Messages.ElementPortInfo portInfo, bool isCompatibilityIssueDetected);
                /// <summary>
                /// Clears the entries update dictionary.
                /// </summary>
                internal abstract void ClearUpdates();
            }

            /// <summary>
            /// Class representing an HTTP Connection.
            /// </summary>
            public class HttpConnection : Skyline.DataMiner.Library.Common.ConnectionSettings, Skyline.DataMiner.Library.Common.IHttpConnection
            {
                private string busAddress;
                private readonly int id;
                private System.TimeSpan? elementTimeout;
                private bool isBypassProxyEnabled;
                private int retries;
                private Skyline.DataMiner.Library.Common.ITcp tcpConfiguration;
                private System.TimeSpan timeout;
                private const string BypassProxyValue = "bypassProxy";
                /// <summary>
                /// Initializes a new instance of the <see cref = "HttpConnection"/> class with default settings for Timeout (1500), Retries (3), Element Timeout (30),
                /// </summary>
                /// <param name = "tcpConfiguration">The TCP Connection.</param>
                /// <param name = "isByPassProxyEnabled">Allows you to enable the ByPassProxy setting. Default true.</param>
                /// <remarks>In case HTTPS needs to be used. TCP port needs to be 443 or the PollingIP needs to start with https:// . e.g. https://192.168.0.1</remarks>
                public HttpConnection(Skyline.DataMiner.Library.Common.ITcp tcpConfiguration, bool isByPassProxyEnabled = true)
                {
                    if (tcpConfiguration == null)
                        throw new System.ArgumentNullException("tcpConfiguration");
                    this.tcpConfiguration = tcpConfiguration;
                    this.busAddress = isByPassProxyEnabled ? BypassProxyValue : System.String.Empty;
                    this.IsBypassProxyEnabled = isByPassProxyEnabled;
                    this.id = -1;
                    this.timeout = new System.TimeSpan(0, 0, 0, 0, 1500);
                    this.retries = 3;
                    this.elementTimeout = new System.TimeSpan(0, 0, 0, 30);
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "HttpConnection"/> class using the specified <see cref = "ElementPortInfo"/>.
                /// </summary>
                /// <param name = "info">Instance of <see cref = "ElementPortInfo"/> to parse the contents of.</param>
                internal HttpConnection(Skyline.DataMiner.Net.Messages.ElementPortInfo info)
                {
                    this.busAddress = info.BusAddress;
                    this.isBypassProxyEnabled = info.ByPassProxy;
                    this.retries = info.Retries;
                    this.timeout = new System.TimeSpan(0, 0, 0, 0, info.TimeoutTime);
                    this.id = info.PortID;
                    this.elementTimeout = new System.TimeSpan(0, 0, 0, 0, info.ElementTimeoutTime);
                    this.tcpConfiguration = new Skyline.DataMiner.Library.Common.Tcp(info);
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "HttpConnection"/> class.
                /// </summary>
                public HttpConnection()
                {
                }

                /// <summary>
                /// Gets the bus address.
                /// </summary>
                /// <value>The buss address.</value>
                public string BusAddress
                {
                    get
                    {
                        return this.busAddress;
                    }
                }

                /// <summary>
                /// Gets or sets the element timeout.
                /// </summary>
                /// <value>The element timeout.</value>
                /// <remarks>When <see langword = "null"/>, this connection will not be taken into account for the element to go into timeout.</remarks>
                public System.TimeSpan? ElementTimeout
                {
                    get
                    {
                        return this.elementTimeout;
                    }

                    set
                    {
                        if (this.elementTimeout != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.ElementTimeout);
                            this.elementTimeout = value;
                        }
                    }
                }

                /// <summary>
                /// Gets the connection ID.
                /// </summary>
                /// <value>The connection ID.</value>
                public int Id
                {
                    get
                    {
                        return this.id;
                    }
                }

                /// <summary>
                /// Gets a value indicating whether to bypass the proxy.
                /// </summary>
                /// <value><c>true</c> if the proxy needs to be bypassed; otherwise, <c>false</c>.</value>
                public bool IsBypassProxyEnabled
                {
                    get
                    {
                        return this.isBypassProxyEnabled;
                    }

                    set
                    {
                        if (this.isBypassProxyEnabled != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.IsByPassProxyEnabled);
                            this.isBypassProxyEnabled = value;
                            this.busAddress = this.isBypassProxyEnabled ? BypassProxyValue : System.String.Empty;
                        }
                    }
                }

                /// <summary>
                /// Gets or set the number of retries.
                /// </summary>
                /// <value>The number of retries.</value>
                public int Retries
                {
                    get
                    {
                        return this.retries;
                    }

                    set
                    {
                        if (this.retries != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.Retries);
                            this.retries = value;
                        }
                    }
                }

                /// <summary>
                /// Gets or sets the TCP connection configuration.
                /// </summary>
                /// <value>The TCP connection configuration.</value>
                public Skyline.DataMiner.Library.Common.ITcp TcpConfiguration
                {
                    get
                    {
                        return this.tcpConfiguration;
                    }

                    set
                    {
                        if (this.tcpConfiguration != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.PortConnection);
                            this.tcpConfiguration = value;
                        }
                    }
                }

                /// <summary>
                /// Gets or sets the timeout.
                /// </summary>
                /// <value>The timeout.</value>
                public System.TimeSpan Timeout
                {
                    get
                    {
                        return this.timeout;
                    }

                    set
                    {
                        if (this.timeout != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.Timeout);
                            this.timeout = value;
                        }
                    }
                }

                /// <summary>
                /// Gets a value indicating whether one or more properties have been updated.
                /// </summary>
                internal override bool IsUpdated
                {
                    get
                    {
                        var tcpSettings = (Skyline.DataMiner.Library.Common.ConnectionSettings)this.tcpConfiguration;
                        return System.Linq.Enumerable.Any(this.ChangedPropertyList) || tcpSettings.IsUpdated;
                    }
                }

                /// <summary>
                /// Clears the entries update dictionary.
                /// </summary>
                internal override void ClearUpdates()
                {
                    this.ChangedPropertyList.Clear();
                    var tcpSettings = (Skyline.DataMiner.Library.Common.ConnectionSettings)this.tcpConfiguration;
                    tcpSettings.ClearUpdates();
                }

                /// <summary>
                /// Creates an ElementPortPortInfo object based on the field contents.
                /// </summary>
                /// <param name = "portPosition">The corresponding port number.</param>
                /// <param name = "isCompatibilityIssueDetected">Indicates if compatibility changes need to be taken into account.</param>
                /// <returns>ElementPortInfo object.</returns>
                internal override Skyline.DataMiner.Net.Messages.ElementPortInfo CreateElementPortInfo(int portPosition, bool isCompatibilityIssueDetected)
                {
                    var portInfo = new Skyline.DataMiner.Net.Messages.ElementPortInfo{BusAddress = this.busAddress, ByPassProxy = this.isBypassProxyEnabled, Retries = this.retries, TimeoutTime = System.Convert.ToInt32(this.timeout.TotalMilliseconds), ElementTimeoutTime = this.elementTimeout.HasValue ? System.Convert.ToInt32(this.elementTimeout.Value.TotalMilliseconds) : -1, LibraryCredential = System.Guid.Empty, PollingIPPort = System.Convert.ToString(this.tcpConfiguration.RemotePort), IsSslTlsEnabled = this.tcpConfiguration.IsSslTlsEnabled, PollingIPAddress = this.tcpConfiguration.RemoteHost, LocalIPPort = this.tcpConfiguration.LocalPort.ToString(), Number = this.tcpConfiguration.NetworkInterfaceCard.ToString(), PortID = portPosition, ProtocolType = Skyline.DataMiner.Net.Messages.ProtocolType.Http, Type = "ip", Baudrate = System.String.Empty, DataBits = System.String.Empty, FlowControl = System.String.Empty, Parity = System.String.Empty};
                    return portInfo;
                }

                /// <summary>
                /// Updates the provided ElementPortInfo object with any performed changes on the object.
                /// </summary>
                /// <param name = "portInfo">The port info.</param>
                /// <param name = "isCompatibilityIssueDetected"></param>
                internal override void UpdateElementPortInfo(Skyline.DataMiner.Net.Messages.ElementPortInfo portInfo, bool isCompatibilityIssueDetected)
                {
                    foreach (Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting property in this.ChangedPropertyList)
                    {
                        switch (property)
                        {
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.IsByPassProxyEnabled:
                                portInfo.BusAddress = this.busAddress;
                                portInfo.ByPassProxy = this.isBypassProxyEnabled;
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.Timeout:
                                portInfo.TimeoutTime = System.Convert.ToInt32(this.timeout.TotalMilliseconds);
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.Retries:
                                portInfo.Retries = this.retries;
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.PortConnection:
                                portInfo.PollingIPPort = System.Convert.ToString(this.tcpConfiguration.RemotePort);
                                portInfo.IsSslTlsEnabled = this.tcpConfiguration.IsSslTlsEnabled;
                                portInfo.PollingIPAddress = this.tcpConfiguration.RemoteHost;
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.ElementTimeout:
                                portInfo.ElementTimeoutTime = System.Convert.ToInt32(this.elementTimeout.Value.TotalMilliseconds);
                                break;
                            default:
                                continue;
                        }
                    }

                    var tcpSettings = (Skyline.DataMiner.Library.Common.ConnectionSettings)this.tcpConfiguration;
                    tcpSettings.UpdateElementPortInfo(portInfo, isCompatibilityIssueDetected);
                    portInfo.ProtocolType = Skyline.DataMiner.Net.Messages.ProtocolType.Http;
                }
            }

            /// <summary>
            /// Represents a connection of a DataMiner element.
            /// </summary>
            public interface IElementConnection
            {
                /// <summary>
                /// Gets the value indicating the connection number or sets which connection id should be used during creation.
                /// </summary>
                /// <value>The identifier of the connection.</value>
                int Id
                {
                    get;
                }
            }

            /// <summary>
            /// Represents an HTTP Connection
            /// </summary>
            public interface IHttpConnection : Skyline.DataMiner.Library.Common.IRealConnection
            {
                /// <summary>
                /// Gets or sets the TCP connection configuration.
                /// </summary>
                /// <value>The TCP connection configuration.</value>
                Skyline.DataMiner.Library.Common.ITcp TcpConfiguration
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets the bus address.
                /// </summary>
                /// <value>The buss address.</value>
                string BusAddress
                {
                    get;
                }

                /// <summary>
                /// Gets a value indicating whether to bypass the proxy.
                /// </summary>
                /// <value><c>true</c> if the proxy needs to be bypassed; otherwise, <c>false</c>.</value>
                bool IsBypassProxyEnabled
                {
                    get;
                    set;
                }
            }

            /// <summary>
            /// Defines a non-virtual interface.
            /// </summary>
            public interface IRealConnection : Skyline.DataMiner.Library.Common.IElementConnection
            {
                // The following properties are added to each connection although it only works for the main connection.
                // The reason for this is that it could be supported in the future, and it's also designed like this in the web api: http://localhost/API/v1/soap.asmx?op=CreateElement
                /// <summary>
                /// Gets or sets the timeout of a single command or request.
                /// </summary>
                /// <value>The timeout of a single command or request.</value>
                System.TimeSpan Timeout
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the number of retries.
                /// </summary>
                /// <value>The number of retries.</value>
                int Retries
                {
                    get;
                    set;
                }

                ///<summary>
                /// Gets or sets a value indicating after how long the element will go into timeout when it is not responding for.
                ///</summary>
                /// <value>The timespan to be set, when set to <see langword = "null"/>, the connection does not have an impact on the element timeout./></value>
                System.TimeSpan? ElementTimeout
                {
                    get;
                    set;
                }
            }

            /// <summary>
            /// Represents a serial connection.
            /// </summary>
            public interface ISerialConnection : Skyline.DataMiner.Library.Common.IRealConnection
            {
                // TODO: Model serial single.
                // bool IsDedicatedConnection { get; } or make subclass?
                /// <summary>
                /// Gets or sets the port connection.
                /// </summary>
                /// <value>The port connection.</value>
                Skyline.DataMiner.Library.Common.IPortConnection Connection
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the bus address.
                /// </summary>
                string BusAddress
                {
                    get;
                    set;
                }
            //bool IsSecure { get; set; }
            }

            /// <summary>
            /// Defines an SNMP connection.
            /// </summary>
            public interface ISnmpConnection : Skyline.DataMiner.Library.Common.IRealConnection
            {
                /// <summary>
                /// Gets or sets the underlying connection.
                /// </summary>
                /// <value>The underlying connection.</value>
                Skyline.DataMiner.Library.Common.IUdp UdpConfiguration
                {
                    get;
                    set;
                }

                // Credentials can currently be used with SNMP connections only.
                // When credentials are provided, then no community strings (snmpv1/snmpv2) or user name,level,authentication protocol,authentication key,encryption protocol, encryption key can be provided.
                // See http://devcore3/documentation/server/RC/html/e7dbdb35-9528-5b65-8436-6b3242a8076f.htm
                // Currently only Get implemented in order to detect if credentials are used or not because then the other fields should be empty and not be settable.
                /// <summary>
                /// Gets the library credentials Guid. When empty guid, the credentials are not used from the library.
                /// </summary>
                System.Guid LibraryCredentials
                {
                    get;
                }

                /// <summary>
                /// Gets or sets the device address.
                /// </summary>
                /// <value>The device address.</value>
                string DeviceAddress
                {
                    get;
                    set;
                }
            }

            /// <summary>
            /// Defines an SNMPv1 Connection
            /// </summary>
            public interface ISnmpV1Connection : Skyline.DataMiner.Library.Common.ISnmpConnection
            {
                /// <summary>
                /// Gets or sets the get community string.
                /// </summary>
                /// <value>The get community string.</value>
                string GetCommunityString
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the set community string.
                /// </summary>
                /// <value>The set community string.</value>
                string SetCommunityString
                {
                    get;
                    set;
                }
            }

            /// <summary>
            /// Defines an SNMPv2 Connection.
            /// </summary>
            public interface ISnmpV2Connection : Skyline.DataMiner.Library.Common.ISnmpConnection
            {
                /// <summary>
                /// Gets or sets the get community string.
                /// </summary>
                /// <value>The get community string.</value>
                string GetCommunityString
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the set community string.
                /// </summary>
                /// <value>The set community string.</value>
                string SetCommunityString
                {
                    get;
                    set;
                }
            }

            /// <summary>
            /// Defines an SNMPv3 Connection.
            /// </summary>
            public interface ISnmpV3Connection : Skyline.DataMiner.Library.Common.ISnmpConnection
            {
                /// <summary>
                /// Gets or sets the SNMPv3 security configuration.
                /// </summary>
                Skyline.DataMiner.Library.Common.ISnmpV3SecurityConfig SecurityConfig
                {
                    get;
                    set;
                }
            }

            /// <summary>
            /// Interface for SnmpV3 Security configurations.
            /// </summary>
            public interface ISnmpV3SecurityConfig
            {
                /// <summary>
                /// Gets or sets the EncryptionAlgorithm.
                /// </summary>
                Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm EncryptionAlgorithm
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the AuthenticationProtocol.
                /// </summary>
                Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm AuthenticationAlgorithm
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the EncryptionKey.
                /// </summary>
                string EncryptionKey
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the AuthenticationKey.
                /// </summary>
                string AuthenticationKey
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the username.
                /// </summary>
                string Username
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the SecurityLevelAndProtocol.
                /// </summary>
                Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol SecurityLevelAndProtocol
                {
                    get;
                    set;
                }
            }

            /// <summary>
            /// Defines a Virtual Connection
            /// </summary>
            public interface IVirtualConnection : Skyline.DataMiner.Library.Common.IElementConnection
            {
            }

            /// <summary>
            /// Class representing any non-virtual connection.
            /// </summary>
            public class RealConnection : Skyline.DataMiner.Library.Common.ConnectionSettings, Skyline.DataMiner.Library.Common.IRealConnection
            {
                private readonly int id;
                private System.TimeSpan timeout;
                private int retries;
                private System.TimeSpan? elementTimeout;
                /// <summary>
                /// Initiates a new RealConnection class.
                /// </summary>
                /// <param name = "info"></param>
                internal RealConnection(Skyline.DataMiner.Net.Messages.ElementPortInfo info)
                {
                    this.id = info.PortID;
                    this.retries = info.Retries;
                    this.timeout = new System.TimeSpan(0, 0, 0, 0, info.TimeoutTime);
                }

                /// <summary>
                /// Default empty constructor.
                /// </summary>
                public RealConnection()
                {
                }

                /// <summary>
                /// Gets the zero based id of the connection.
                /// </summary>
                public int Id
                {
                    get
                    {
                        return this.id;
                    }
                }

                /// <summary>
                /// Get or Set the timeout value.
                /// </summary>
                public System.TimeSpan Timeout
                {
                    get
                    {
                        return timeout;
                    }

                    set
                    {
                        if (value.TotalMilliseconds >= 10 && value.TotalMilliseconds <= 120000)
                        {
                            timeout = value;
                        }
                        else
                        {
                            throw new Skyline.DataMiner.Library.Common.IncorrectDataException("Timeout value should be between 10 and 120 s.");
                        }
                    }
                }

                /// <summary>
                /// Get or Set the amount of retries.
                /// </summary>
                public int Retries
                {
                    get
                    {
                        return retries;
                    }

                    set
                    {
                        if (value >= 0 && value <= 10)
                        {
                            retries = value;
                        }
                        else
                        {
                            throw new Skyline.DataMiner.Library.Common.IncorrectDataException("Retries value should be between 0 and 10.");
                        }
                    }
                }

                /// <summary>
                /// Get or Set the timespan before the element will go into timeout after not responding.
                /// </summary>
                /// <value>When null, the connection will not be taken into account for the element to go into timeout.</value>
                public System.TimeSpan? ElementTimeout
                {
                    get
                    {
                        return elementTimeout;
                    }

                    set
                    {
                        if (value == null || (value.Value.TotalSeconds >= 1 && value.Value.TotalSeconds <= 120))
                        {
                            elementTimeout = value;
                        }
                        else
                        {
                            throw new Skyline.DataMiner.Library.Common.IncorrectDataException("ElementTimeout value should be between 1 and 120 sec.");
                        }
                    }
                }

                /// <summary>
                /// Returns whether a property has been set or not.
                /// </summary>
                internal override bool IsUpdated
                {
                    get
                    {
                        return (System.Linq.Enumerable.Any(ChangedPropertyList));
                    }
                }

                /// <summary>
                /// Creates an ElementPortInfo object based on the properties.
                /// </summary>
                /// <returns></returns>
                internal override Skyline.DataMiner.Net.Messages.ElementPortInfo CreateElementPortInfo(int portPosition, bool isCompatibilityIssueDetected)
                {
                    throw new System.NotSupportedException("RealConnection is not supported.");
                }

                /// <summary>
                /// Updates an ElementPortInfo object based on changes on properties.
                /// </summary>
                /// <param name = "portInfo"></param>
                /// <param name = "isCompatibilityIssueDetected"></param>
                internal override void UpdateElementPortInfo(Skyline.DataMiner.Net.Messages.ElementPortInfo portInfo, bool isCompatibilityIssueDetected)
                {
                // throw new NotSupportedException("RealConnection is not supported.");
                }

                /// <summary>
                /// Clear the performed update flags of the properties of the object.
                /// </summary>
                internal override void ClearUpdates()
                {
                    this.ChangedPropertyList.Clear();
                }
            }

            /// <summary>
            /// Class used to Encrypt data in DataMiner.
            /// </summary>
            internal class RSA
            {
                private static System.Security.Cryptography.RSAParameters publicKey;
                /// <summary>
                /// Get or Sets the Public Key.
                /// </summary>
                internal static System.Security.Cryptography.RSAParameters PublicKey
                {
                    get
                    {
                        return publicKey;
                    }

                    set
                    {
                        publicKey = value;
                    }
                }

                /// <summary>
                /// Encrypt a string value using the PublicKey.
                /// </summary>
                /// <param name = "plainData">The string to encrypt.</param>
                /// <returns>Encrypted string value.</returns>
                internal static string Encrypt(string plainData)
                {
                    if (plainData == null)
                    {
                        throw new System.ArgumentNullException("plainData");
                    }

                    if (publicKey.Modulus == null)
                    {
                        throw new Skyline.DataMiner.Library.Common.IncorrectDataException("publicKey.Modulus is null");
                    }

                    if (publicKey.Exponent == null)
                    {
                        throw new Skyline.DataMiner.Library.Common.IncorrectDataException("publicKey.Exponent is null");
                    }

                    plainData = plainData ?? "";
                    System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider(); //NOTE: OAEP padding is used.
                    rsa.ImportParameters(publicKey);
                    //Encrypt data
                    byte[] encryptedData = rsa.Encrypt(System.Text.Encoding.UTF8.GetBytes(plainData), true);
                    return System.BitConverter.ToString(encryptedData).Replace("-", "");
                }
            }

            /// <summary>
            /// Class representing a Serial Connection.
            /// </summary>
            public class SerialConnection : Skyline.DataMiner.Library.Common.ISerialConnection
            {
                /// <summary>
                ///	Initiates a new instance with default settings for Timeout (1500), Retries (3), Element Timeout (30),
                ///	</summary>
                /// <param name = "tcpConnection">The TCP Connection.</param>
                public SerialConnection(Skyline.DataMiner.Library.Common.ITcp tcpConnection)
                {
                    if (tcpConnection == null)
                        throw new System.ArgumentNullException("tcpConnection");
                    Connection = tcpConnection;
                    BusAddress = System.String.Empty;
                    Id = -1;
                    Timeout = new System.TimeSpan(0, 0, 0, 0, 1500);
                    Retries = 3;
                    ElementTimeout = new System.TimeSpan(0, 0, 0, 30);
                }

                /// <summary>
                ///	Initiates a new instance with default settings for Timeout (1500), Retries (3), Element Timeout (30),
                ///	</summary>
                /// <param name = "udpConnection">The UDP Connection.</param>
                public SerialConnection(Skyline.DataMiner.Library.Common.IUdp udpConnection)
                {
                    if (udpConnection == null)
                        throw new System.ArgumentNullException("udpConnection");
                    Connection = udpConnection;
                    BusAddress = System.String.Empty;
                    Id = -1;
                    Timeout = new System.TimeSpan(0, 0, 0, 0, 1500);
                    Retries = 3;
                    ElementTimeout = new System.TimeSpan(0, 0, 0, 30);
                }

                /// <summary>
                /// Default empty constructor
                /// </summary>
                public SerialConnection()
                {
                }

                /// <summary>
                /// Get or Sets the connection settings.
                /// </summary>
                public Skyline.DataMiner.Library.Common.IPortConnection Connection
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the bus address.
                /// </summary>
                public string BusAddress
                {
                    get;
                    set;
                }

                /// <summary>
                /// Get or Set the timeout value.
                /// </summary>
                public System.TimeSpan Timeout
                {
                    get;
                    set;
                }

                /// <summary>
                /// Get or Set the amount of retries.
                /// </summary>
                public int Retries
                {
                    get;
                    set;
                }

                /// <summary>
                /// Get or Set the timespan before the element will go into timeout after not responding.
                /// </summary>
                /// <value>When null, the connection will not be taken into account for the element to go into timeout.</value>
                public System.TimeSpan? ElementTimeout
                {
                    get;
                    set;
                }

                /// <summary>
                /// Get or Sets the zero based id of the connection.
                /// </summary>
                public int Id
                {
                    get;
                    private set;
                }
            }

            /// <summary>
            ///     Class representing an SNMPv1 connection.
            /// </summary>
            public class SnmpV1Connection : Skyline.DataMiner.Library.Common.ConnectionSettings, Skyline.DataMiner.Library.Common.ISnmpV1Connection
            {
                private readonly int id;
                private readonly System.Guid libraryCredentials;
                private string deviceAddress;
                private System.TimeSpan? elementTimeout;
                private string getCommunityString;
                private int retries;
                private string setCommunityString;
                private System.TimeSpan timeout;
                private Skyline.DataMiner.Library.Common.IUdp udpIpConfiguration;
                /// <summary>
                ///     /// Initiates a new instance with default settings for Get Community String (public), Set Community String
                ///     (private), Device Address (empty),
                ///     Command Timeout (1500ms), Retries (3) and Element Timeout (30s).
                /// </summary>
                /// <param name = "udpConfiguration">The UDP configuration parameters.</param>
                public SnmpV1Connection(Skyline.DataMiner.Library.Common.IUdp udpConfiguration)
                {
                    if (udpConfiguration == null)
                    {
                        throw new System.ArgumentNullException("udpConfiguration");
                    }

                    this.id = -1;
                    this.udpIpConfiguration = udpConfiguration;
                    this.getCommunityString = "public";
                    this.setCommunityString = "private";
                    this.deviceAddress = System.String.Empty;
                    this.timeout = new System.TimeSpan(0, 0, 0, 0, 1500);
                    this.retries = 3;
                    this.elementTimeout = new System.TimeSpan(0, 0, 0, 30);
                }

                /// <summary>
                ///     Default empty constructor
                /// </summary>
                public SnmpV1Connection()
                {
                }

                /// <summary>
                ///     Initiates an new instance.
                /// </summary>
                internal SnmpV1Connection(Skyline.DataMiner.Net.Messages.ElementPortInfo info)
                {
                    this.deviceAddress = info.BusAddress;
                    this.retries = info.Retries;
                    this.timeout = new System.TimeSpan(0, 0, 0, 0, info.TimeoutTime);
                    this.libraryCredentials = info.LibraryCredential;
                    // this.elementTimeout = new TimeSpan(0, 0, info.ElementTimeoutTime / 1000);
                    if (this.libraryCredentials == System.Guid.Empty)
                    {
                        this.getCommunityString = info.GetCommunity;
                        this.setCommunityString = info.SetCommunity;
                    }
                    else
                    {
                        this.getCommunityString = System.String.Empty;
                        this.setCommunityString = System.String.Empty;
                    }

                    this.id = info.PortID;
                    this.elementTimeout = new System.TimeSpan(0, 0, 0, 0, info.ElementTimeoutTime);
                    this.udpIpConfiguration = new Skyline.DataMiner.Library.Common.Udp(info);
                }

                /// <summary>
                ///     Get or Set the device address.
                /// </summary>
                public string DeviceAddress
                {
                    get
                    {
                        return this.deviceAddress;
                    }

                    set
                    {
                        if (this.deviceAddress != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.DeviceAddress);
                            this.deviceAddress = value;
                        }
                    }
                }

                /// <summary>
                ///     Get or Set the timespan before the element will go into timeout after not responding.
                /// </summary>
                /// <value>When null, the connection will not be taken into account for the element to go into timeout.</value>
                public System.TimeSpan? ElementTimeout
                {
                    get
                    {
                        return this.elementTimeout;
                    }

                    set
                    {
                        if (this.elementTimeout != value)
                        {
                            if (value == null || (value.Value.TotalSeconds >= 1 && value.Value.TotalSeconds <= 120))
                            {
                                this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.ElementTimeout);
                                this.elementTimeout = value;
                            }
                            else
                            {
                                throw new Skyline.DataMiner.Library.Common.IncorrectDataException("ElementTimeout value should be between 1 and 120 sec.");
                            }
                        }
                    }
                }

                /// <summary>
                ///     Get or sets the Get community string.
                /// </summary>
                public string GetCommunityString
                {
                    get
                    {
                        return this.getCommunityString;
                    }

                    set
                    {
                        if (this.getCommunityString != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.GetCommunityString);
                            this.getCommunityString = value;
                        }
                    }
                }

                /// <summary>
                /// Gets the zero based id of the connection.
                /// </summary>
                public int Id
                {
                    get
                    {
                        return this.id;
                    }
                }

                /// <summary>
                ///     Get the libraryCredentials
                /// </summary>
                public System.Guid LibraryCredentials
                {
                    get
                    {
                        return this.libraryCredentials;
                    }
                }

                /// <summary>
                ///     Get or Set the amount of retries.
                /// </summary>
                public int Retries
                {
                    get
                    {
                        return this.retries;
                    }

                    set
                    {
                        if (this.retries != value)
                        {
                            if (value >= 0 && value <= 10)
                            {
                                this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.Retries);
                                this.retries = value;
                            }
                            else
                            {
                                throw new Skyline.DataMiner.Library.Common.IncorrectDataException("Retries value should be between 0 and 10.");
                            }
                        }
                    }
                }

                /// <summary>
                ///     Get or set the Set Community String.
                /// </summary>
                public string SetCommunityString
                {
                    get
                    {
                        return this.setCommunityString;
                    }

                    set
                    {
                        if (this.setCommunityString != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.SetCommunityString);
                            this.setCommunityString = value;
                        }
                    }
                }

                /// <summary>
                ///     Get or Set the timeout value.
                /// </summary>
                public System.TimeSpan Timeout
                {
                    get
                    {
                        return this.timeout;
                    }

                    set
                    {
                        if (this.timeout != value)
                        {
                            if (value.TotalMilliseconds >= 10 && value.TotalMilliseconds <= 120000)
                            {
                                this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.Timeout);
                                this.timeout = value;
                            }
                            else
                            {
                                throw new Skyline.DataMiner.Library.Common.IncorrectDataException("Timeout value should be between 10 and 120 sec.");
                            }
                        }
                    }
                }

                /// <summary>
                ///     Get or Set the UDP Connection settings
                /// </summary>
                public Skyline.DataMiner.Library.Common.IUdp UdpConfiguration
                {
                    get
                    {
                        return this.udpIpConfiguration;
                    }

                    set
                    {
                        if (this.udpIpConfiguration == null || !this.udpIpConfiguration.Equals(value))
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.PortConnection);
                            this.udpIpConfiguration = value;
                        }
                    }
                }

                /// <summary>
                ///     Indicates if updates have been performed on the properties of the object.
                /// </summary>
                internal override bool IsUpdated
                {
                    get
                    {
                        var udpSettings = (Skyline.DataMiner.Library.Common.ConnectionSettings)this.udpIpConfiguration;
                        return System.Linq.Enumerable.Any(this.ChangedPropertyList) || udpSettings.IsUpdated;
                    }
                }

                /// <summary>
                ///     Clear the performed update flags of the properties of the object.
                /// </summary>
                internal override void ClearUpdates()
                {
                    this.ChangedPropertyList.Clear();
                    var udpSettings = (Skyline.DataMiner.Library.Common.ConnectionSettings)this.udpIpConfiguration;
                    udpSettings.ClearUpdates();
                }

                /// <summary>
                ///     Creates an ElementPortPortInfo object based on the field contents.
                /// </summary>
                /// <returns>ElementPortInfo object.</returns>
                internal override Skyline.DataMiner.Net.Messages.ElementPortInfo CreateElementPortInfo(int portPosition, bool isCompatibilityIssueDetected)
                {
                    var portInfo = new Skyline.DataMiner.Net.Messages.ElementPortInfo{BusAddress = this.deviceAddress, Retries = this.retries, TimeoutTime = System.Convert.ToInt32(this.timeout.TotalMilliseconds), LibraryCredential = this.libraryCredentials};
                    if (this.libraryCredentials == System.Guid.Empty)
                    {
                        portInfo.GetCommunity = this.getCommunityString;
                        portInfo.SetCommunity = this.setCommunityString;
                    }
                    else
                    {
                        portInfo.GetCommunity = System.String.Empty;
                        portInfo.SetCommunity = System.String.Empty;
                    }

                    portInfo.PollingIPPort = System.Convert.ToString(this.udpIpConfiguration.RemotePort);
                    portInfo.IsSslTlsEnabled = this.udpIpConfiguration.IsSslTlsEnabled;
                    portInfo.PollingIPAddress = this.udpIpConfiguration.RemoteHost;
                    portInfo.LocalIPPort = this.udpIpConfiguration.LocalPort.ToString();
                    portInfo.Number = this.udpIpConfiguration.NetworkInterfaceCard.ToString();
                    portInfo.PortID = portPosition;
                    portInfo.ProtocolType = Skyline.DataMiner.Net.Messages.ProtocolType.SnmpV1;
                    portInfo.Type = "ip";
                    portInfo.Baudrate = System.String.Empty;
                    portInfo.DataBits = System.String.Empty;
                    portInfo.FlowControl = System.String.Empty;
                    portInfo.Parity = System.String.Empty;
                    return portInfo;
                }

                /// <summary>
                ///     Updates the provided ElementPortInfo object with any performed changes on the object.
                /// </summary>
                /// <param name = "portInfo"></param>
                /// <param name = "isCompatibilityIssueDetected"></param>
                internal override void UpdateElementPortInfo(Skyline.DataMiner.Net.Messages.ElementPortInfo portInfo, bool isCompatibilityIssueDetected)
                {
                    foreach (Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting property in this.ChangedPropertyList)
                    {
                        switch (property)
                        {
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.GetCommunityString:
                                portInfo.GetCommunity = this.getCommunityString;
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.SetCommunityString:
                                portInfo.SetCommunity = this.setCommunityString;
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.DeviceAddress:
                                portInfo.BusAddress = this.deviceAddress;
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.Timeout:
                                portInfo.TimeoutTime = System.Convert.ToInt32(this.timeout.TotalMilliseconds);
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.Retries:
                                portInfo.Retries = this.retries;
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.PortConnection:
                                portInfo.PollingIPPort = System.Convert.ToString(this.udpIpConfiguration.RemotePort);
                                portInfo.IsSslTlsEnabled = this.udpIpConfiguration.IsSslTlsEnabled;
                                portInfo.PollingIPAddress = this.udpIpConfiguration.RemoteHost;
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.ElementTimeout:
                                portInfo.ElementTimeoutTime = System.Convert.ToInt32(this.elementTimeout.Value.TotalMilliseconds);
                                break;
                            // case "Id":
                            // 	portInfo.PortID = this.id;
                            // 	break;
                            default:
                                continue;
                        }
                    }

                    var udpSettings = (Skyline.DataMiner.Library.Common.ConnectionSettings)this.udpIpConfiguration;
                    udpSettings.UpdateElementPortInfo(portInfo, isCompatibilityIssueDetected);
                    portInfo.ProtocolType = Skyline.DataMiner.Net.Messages.ProtocolType.SnmpV1;
                }
            }

            /// <summary>
            ///     Class representing an SnmpV2 Connection.
            /// </summary>
            public class SnmpV2Connection : Skyline.DataMiner.Library.Common.ConnectionSettings, Skyline.DataMiner.Library.Common.ISnmpV2Connection
            {
                private readonly int id;
                private readonly System.Guid libraryCredentials;
                private string deviceAddress;
                private System.TimeSpan? elementTimeout;
                private string getCommunityString;
                private int retries;
                private string setCommunityString;
                private System.TimeSpan timeout;
                private Skyline.DataMiner.Library.Common.IUdp udpIpConfiguration;
                /// <summary>
                ///     Initiates a new instance with default settings for Get Community String (public), Set Community String (private),
                ///     Device Address (empty),
                ///     Command Timeout (1500ms), Retries (3) and Element Timeout (30s).
                /// </summary>
                /// <param name = "udpConfiguration">The UDP Connection settings.</param>
                public SnmpV2Connection(Skyline.DataMiner.Library.Common.IUdp udpConfiguration)
                {
                    if (udpConfiguration == null)
                    {
                        throw new System.ArgumentNullException("udpConfiguration");
                    }

                    this.id = -1;
                    this.udpIpConfiguration = udpConfiguration;
                    // this.udpIpConfiguration = udpIpIpConfiguration;
                    this.deviceAddress = System.String.Empty;
                    this.getCommunityString = "public";
                    this.setCommunityString = "private";
                    this.timeout = new System.TimeSpan(0, 0, 0, 0, 1500);
                    this.retries = 3;
                    this.elementTimeout = new System.TimeSpan(0, 0, 0, 30);
                    this.libraryCredentials = System.Guid.Empty;
                }

                /// <summary>
                ///     Default empty constructor
                /// </summary>
                public SnmpV2Connection()
                {
                }

                /// <summary>
                ///     Initializes a new instance.
                /// </summary>
                internal SnmpV2Connection(Skyline.DataMiner.Net.Messages.ElementPortInfo info)
                {
                    this.deviceAddress = info.BusAddress;
                    this.retries = info.Retries;
                    this.timeout = new System.TimeSpan(0, 0, 0, 0, info.TimeoutTime);
                    this.getCommunityString = info.GetCommunity;
                    this.setCommunityString = info.SetCommunity;
                    this.libraryCredentials = info.LibraryCredential;
                    if (info.LibraryCredential == System.Guid.Empty)
                    {
                        this.getCommunityString = info.GetCommunity;
                        this.setCommunityString = info.SetCommunity;
                    }
                    else
                    {
                        this.getCommunityString = System.String.Empty;
                        this.setCommunityString = System.String.Empty;
                    }

                    this.id = info.PortID;
                    this.elementTimeout = new System.TimeSpan(0, 0, 0, 0, info.ElementTimeoutTime);
                    this.udpIpConfiguration = new Skyline.DataMiner.Library.Common.Udp(info);
                }

                /// <summary>
                ///     Get or Sets the device address.
                /// </summary>
                public string DeviceAddress
                {
                    get
                    {
                        return this.deviceAddress;
                    }

                    set
                    {
                        if (this.deviceAddress != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.DeviceAddress);
                            this.deviceAddress = value;
                        }
                    }
                }

                /// <summary>
                ///     Get or Set the timespan before the element will go into timeout after not responding.
                /// </summary>
                /// <value>When null, the connection will not be taken into account for the element to go into timeout.</value>
                public System.TimeSpan? ElementTimeout
                {
                    get
                    {
                        return this.elementTimeout;
                    }

                    set
                    {
                        if (this.elementTimeout != value)
                        {
                            if (value == null || (value.Value.TotalSeconds >= 1 && value.Value.TotalSeconds <= 120))
                            {
                                this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.ElementTimeout);
                                this.elementTimeout = value;
                            }
                            else
                            {
                                throw new Skyline.DataMiner.Library.Common.IncorrectDataException("ElementTimeout value should be between 1 and 120 sec.");
                            }
                        }
                    }
                }

                /// <summary>
                ///     Get or Sets the Get community string.
                /// </summary>
                public string GetCommunityString
                {
                    get
                    {
                        return this.getCommunityString;
                    }

                    set
                    {
                        if (this.getCommunityString != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.GetCommunityString);
                            this.getCommunityString = value;
                        }
                    }
                }

                /// <summary>
                ///     Gets the zero based id of the connection.
                /// </summary>
                public int Id
                {
                    get
                    {
                        return this.id;
                    }
                }

                /// <summary>
                ///     Gets the Library Credential settings.
                /// </summary>
                public System.Guid LibraryCredentials
                {
                    get
                    {
                        return this.libraryCredentials;
                    }
                }

                /// <summary>
                ///     Get or Set the amount of retries.
                /// </summary>
                public int Retries
                {
                    get
                    {
                        return this.retries;
                    }

                    set
                    {
                        if (this.retries != value)
                        {
                            if (value >= 0 && value <= 10)
                            {
                                this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.Retries);
                                this.retries = value;
                            }
                            else
                            {
                                throw new Skyline.DataMiner.Library.Common.IncorrectDataException("Retries value should be between 0 and 10.");
                            }
                        }
                    }
                }

                /// <summary>
                ///     Get or Sets the Set community string.
                /// </summary>
                public string SetCommunityString
                {
                    get
                    {
                        return this.setCommunityString;
                    }

                    set
                    {
                        if (this.setCommunityString != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.SetCommunityString);
                            this.setCommunityString = value;
                        }
                    }
                }

                /// <summary>
                ///     Get or Set the timeout value.
                /// </summary>
                public System.TimeSpan Timeout
                {
                    get
                    {
                        return this.timeout;
                    }

                    set
                    {
                        if (this.timeout != value)
                        {
                            if (value.TotalMilliseconds >= 10 && value.TotalMilliseconds <= 120000)
                            {
                                this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.Timeout);
                                this.timeout = value;
                            }
                            else
                            {
                                throw new Skyline.DataMiner.Library.Common.IncorrectDataException("Timeout value should be between 10 and 120 s.");
                            }
                        }
                    }
                }

                /// <summary>
                ///     Get or Sets the UDP connection settings.
                /// </summary>
                public Skyline.DataMiner.Library.Common.IUdp UdpConfiguration
                {
                    get
                    {
                        return this.udpIpConfiguration;
                    }

                    set
                    {
                        if (this.udpIpConfiguration == null || !this.udpIpConfiguration.Equals(value))
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.PortConnection);
                            this.udpIpConfiguration = value;
                        }
                    }
                }

                /// <summary>
                ///     Indicates if updates have been performed on the properties of the object.
                /// </summary>
                internal override bool IsUpdated
                {
                    get
                    {
                        var udpSettings = (Skyline.DataMiner.Library.Common.ConnectionSettings)this.udpIpConfiguration;
                        return System.Linq.Enumerable.Any(this.ChangedPropertyList) || udpSettings.IsUpdated;
                    }
                }

                /// <summary>
                ///     Clear the performed update flags of the properties of the object.
                /// </summary>
                internal override void ClearUpdates()
                {
                    this.ChangedPropertyList.Clear();
                    var udpSettings = (Skyline.DataMiner.Library.Common.ConnectionSettings)this.udpIpConfiguration;
                    udpSettings.ClearUpdates();
                }

                /// <summary>
                ///     Creates an ElementPortPortInfo object based on the field contents.
                /// </summary>
                /// <returns>ElementPortInfo object.</returns>
                internal override Skyline.DataMiner.Net.Messages.ElementPortInfo CreateElementPortInfo(int portPosition, bool isCompatibilityIssueDetected)
                {
                    var portInfo = new Skyline.DataMiner.Net.Messages.ElementPortInfo{BusAddress = this.deviceAddress, Retries = this.retries, TimeoutTime = System.Convert.ToInt32(this.timeout.TotalMilliseconds), LibraryCredential = this.libraryCredentials, PollingIPPort = System.Convert.ToString(this.udpIpConfiguration.RemotePort), IsSslTlsEnabled = this.udpIpConfiguration.IsSslTlsEnabled, PollingIPAddress = this.udpIpConfiguration.RemoteHost, PortID = portPosition, ProtocolType = Skyline.DataMiner.Net.Messages.ProtocolType.SnmpV2, LocalIPPort = this.udpIpConfiguration.LocalPort.ToString(), Number = this.udpIpConfiguration.NetworkInterfaceCard.ToString(), Type = "ip", Baudrate = System.String.Empty, DataBits = System.String.Empty, FlowControl = System.String.Empty, Parity = System.String.Empty};
                    if (this.libraryCredentials == System.Guid.Empty)
                    {
                        portInfo.GetCommunity = this.getCommunityString;
                        portInfo.SetCommunity = this.setCommunityString;
                    }
                    else
                    {
                        portInfo.GetCommunity = System.String.Empty;
                        portInfo.SetCommunity = System.String.Empty;
                    }

                    return portInfo;
                }

                /// <summary>
                ///     Updates the provided ElementPortInfo object with any performed changes on the object.
                /// </summary>
                /// <param name = "portInfo"></param>
                /// <param name = "isCompatibilityIssueDetected"></param>
                internal override void UpdateElementPortInfo(Skyline.DataMiner.Net.Messages.ElementPortInfo portInfo, bool isCompatibilityIssueDetected)
                {
                    foreach (Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting property in this.ChangedPropertyList)
                    {
                        switch (property)
                        {
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.GetCommunityString:
                                portInfo.GetCommunity = this.getCommunityString;
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.SetCommunityString:
                                portInfo.SetCommunity = this.setCommunityString;
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.DeviceAddress:
                                portInfo.BusAddress = this.deviceAddress;
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.Timeout:
                                portInfo.TimeoutTime = System.Convert.ToInt32(this.timeout.TotalMilliseconds);
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.Retries:
                                portInfo.Retries = this.retries;
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.PortConnection:
                                portInfo.PollingIPPort = System.Convert.ToString(this.udpIpConfiguration.RemotePort);
                                portInfo.IsSslTlsEnabled = this.udpIpConfiguration.IsSslTlsEnabled;
                                portInfo.PollingIPAddress = this.udpIpConfiguration.RemoteHost;
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.ElementTimeout:
                                portInfo.ElementTimeoutTime = System.Convert.ToInt32(this.elementTimeout.Value.TotalMilliseconds);
                                break;
                            default:
                                continue;
                        }
                    }

                    var udpSettings = (Skyline.DataMiner.Library.Common.ConnectionSettings)this.udpIpConfiguration;
                    udpSettings.UpdateElementPortInfo(portInfo, isCompatibilityIssueDetected);
                    portInfo.ProtocolType = Skyline.DataMiner.Net.Messages.ProtocolType.SnmpV2;
                }
            }

            /// <summary>
            ///     Class representing a SNMPv3 class.
            /// </summary>
            public class SnmpV3Connection : Skyline.DataMiner.Library.Common.ConnectionSettings, Skyline.DataMiner.Library.Common.ISnmpV3Connection
            {
                private readonly int id;
                private readonly System.Guid libraryCredentials;
                private string deviceAddress;
                private System.TimeSpan? elementTimeout;
                private int retries;
                private Skyline.DataMiner.Library.Common.ISnmpV3SecurityConfig securityConfig;
                private System.TimeSpan timeout;
                private Skyline.DataMiner.Library.Common.IUdp udpIpConfiguration;
                /// <summary>
                ///     Initializes a new instance.
                /// </summary>
                /// <param name = "udpConfiguration">The udp configuration settings.</param>
                /// <param name = "securityConfig">The SNMPv3 security configuration.</param>
                public SnmpV3Connection(Skyline.DataMiner.Library.Common.IUdp udpConfiguration, Skyline.DataMiner.Library.Common.SnmpV3SecurityConfig securityConfig)
                {
                    if (udpConfiguration == null)
                    {
                        throw new System.ArgumentNullException("udpConfiguration");
                    }

                    if (securityConfig == null)
                    {
                        throw new System.ArgumentNullException("securityConfig");
                    }

                    this.libraryCredentials = System.Guid.Empty;
                    this.id = -1;
                    this.udpIpConfiguration = udpConfiguration;
                    this.deviceAddress = System.String.Empty;
                    this.securityConfig = securityConfig;
                    this.timeout = new System.TimeSpan(0, 0, 0, 0, 1500);
                    this.retries = 3;
                    this.elementTimeout = new System.TimeSpan(0, 0, 0, 30);
                }

                /// <summary>
                ///     Default empty constructor
                /// </summary>
                public SnmpV3Connection()
                {
                }

                /// <summary>
                ///     Initializes a new instance.
                /// </summary>
                internal SnmpV3Connection(Skyline.DataMiner.Net.Messages.ElementPortInfo info)
                {
                    this.deviceAddress = info.BusAddress;
                    this.retries = info.Retries;
                    this.timeout = new System.TimeSpan(0, 0, 0, 0, info.TimeoutTime);
                    this.elementTimeout = new System.TimeSpan(0, 0, info.ElementTimeoutTime / 1000);
                    if (this.libraryCredentials == System.Guid.Empty)
                    {
                        var securityLevelAndProtocol = Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocolAdapter.FromSLNetStopBits(info.StopBits);
                        var encryptionAlgorithm = Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithmAdapter.FromSLNetFlowControl(info.FlowControl);
                        var authenticationProtocol = Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithmAdapter.FromSLNetParity(info.Parity);
                        string authenticationKey = info.GetCommunity;
                        string encryptionKey = info.SetCommunity;
                        string username = info.DataBits;
                        this.securityConfig = new Skyline.DataMiner.Library.Common.SnmpV3SecurityConfig(securityLevelAndProtocol, username, authenticationKey, encryptionKey, authenticationProtocol, encryptionAlgorithm);
                    }
                    else
                    {
                        this.SecurityConfig = new Skyline.DataMiner.Library.Common.SnmpV3SecurityConfig(Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol.DefinedInCredentialsLibrary, System.String.Empty, System.String.Empty, System.String.Empty, Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.DefinedInCredentialsLibrary, Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.DefinedInCredentialsLibrary);
                    }

                    this.id = info.PortID;
                    this.elementTimeout = new System.TimeSpan(0, 0, 0, 0, info.ElementTimeoutTime);
                    this.udpIpConfiguration = new Skyline.DataMiner.Library.Common.Udp(info);
                }

                /// <summary>
                ///     Gets or Sets the device address.
                /// </summary>
                public string DeviceAddress
                {
                    get
                    {
                        return this.deviceAddress;
                    }

                    set
                    {
                        if (this.deviceAddress != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.DeviceAddress);
                            this.deviceAddress = value;
                        }
                    }
                }

                /// <summary>
                ///     Get or Set the timespan before the element will go into timeout after not responding.
                /// </summary>
                /// <value>When null, the connection will not be taken into account for the element to go into timeout.</value>
                public System.TimeSpan? ElementTimeout
                {
                    get
                    {
                        return this.elementTimeout;
                    }

                    set
                    {
                        if (this.elementTimeout != value)
                        {
                            if (value == null || (value.Value.TotalSeconds >= 1 && value.Value.TotalSeconds <= 120))
                            {
                                this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.ElementTimeout);
                                this.elementTimeout = value;
                            }
                            else
                            {
                                throw new Skyline.DataMiner.Library.Common.IncorrectDataException("ElementTimeout value should be between 1 and 120 sec.");
                            }
                        }
                    }
                }

                /// <summary>
                /// Gets the zero based id of the connection.
                /// </summary>
                public int Id
                {
                    get
                    {
                        return this.id;
                    }
                // set
                // {
                // 	ChangedPropertyList.Add("Id");
                // 	id = value;
                // }
                }

                /// <summary>
                ///     Get the libraryCredentials.
                /// </summary>
                public System.Guid LibraryCredentials
                {
                    get
                    {
                        return this.libraryCredentials;
                    }
                }

                /// <summary>
                ///     Get or Set the amount of retries.
                /// </summary>
                public int Retries
                {
                    get
                    {
                        return this.retries;
                    }

                    set
                    {
                        if (this.retries != value)
                        {
                            if (value >= 0 && value <= 10)
                            {
                                this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.Retries);
                                this.retries = value;
                            }
                            else
                            {
                                throw new Skyline.DataMiner.Library.Common.IncorrectDataException("Retries value should be between 0 and 10.");
                            }
                        }
                    }
                }

                /// <summary>
                ///     Gets or sets the SNMPv3 security configuration.
                /// </summary>
                public Skyline.DataMiner.Library.Common.ISnmpV3SecurityConfig SecurityConfig
                {
                    get
                    {
                        return this.securityConfig;
                    }

                    set
                    {
                        this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.SecurityConfig);
                        this.securityConfig = value;
                    }
                }

                /// <summary>
                ///     Get or Set the timeout value.
                /// </summary>
                public System.TimeSpan Timeout
                {
                    get
                    {
                        return this.timeout;
                    }

                    set
                    {
                        if (this.timeout != value)
                        {
                            if (value.TotalMilliseconds >= 10 && value.TotalMilliseconds <= 120000)
                            {
                                this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.Timeout);
                                this.timeout = value;
                            }
                            else
                            {
                                throw new Skyline.DataMiner.Library.Common.IncorrectDataException("Timeout value should be between 10 and 120 sec.");
                            }
                        }
                    }
                }

                /// <summary>
                ///     Get or Set the UDP Connection settings
                /// </summary>
                public Skyline.DataMiner.Library.Common.IUdp UdpConfiguration
                {
                    get
                    {
                        return this.udpIpConfiguration;
                    }

                    set
                    {
                        if (this.udpIpConfiguration == null || !this.udpIpConfiguration.Equals(value))
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.PortConnection);
                            this.udpIpConfiguration = value;
                        }
                    }
                }

                /// <summary>
                ///     Indicates if updates have been performed on the properties of the object.
                /// </summary>
                internal override bool IsUpdated
                {
                    get
                    {
                        var udpSettings = (Skyline.DataMiner.Library.Common.ConnectionSettings)this.udpIpConfiguration;
                        var mySecurityConfig = (Skyline.DataMiner.Library.Common.ConnectionSettings)this.securityConfig;
                        return System.Linq.Enumerable.Any(this.ChangedPropertyList) || udpSettings.IsUpdated || mySecurityConfig.IsUpdated;
                    }
                }

                /// <summary>
                ///     Clear the performed update flags of the properties of the object.
                /// </summary>
                internal override void ClearUpdates()
                {
                    this.ChangedPropertyList.Clear();
                    var udpSettings = (Skyline.DataMiner.Library.Common.ConnectionSettings)this.udpIpConfiguration;
                    udpSettings.ClearUpdates();
                    var mySecurityConfig = (Skyline.DataMiner.Library.Common.ConnectionSettings)this.securityConfig;
                    mySecurityConfig.ClearUpdates();
                }

                /// <summary>
                ///     Creates an ElementPortPortInfo object based on the field contents.
                /// </summary>
                /// <returns>ElementPortInfo object.</returns>
                internal override Skyline.DataMiner.Net.Messages.ElementPortInfo CreateElementPortInfo(int portPosition, bool isCompatibilityIssueDetected)
                {
                    var portInfo = new Skyline.DataMiner.Net.Messages.ElementPortInfo{BusAddress = this.deviceAddress, Retries = this.retries, TimeoutTime = System.Convert.ToInt32(this.timeout.TotalMilliseconds), LibraryCredential = System.Guid.Empty, StopBits = Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocolAdapter.ToSLNetStopBits(this.SecurityConfig.SecurityLevelAndProtocol), FlowControl = Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithmAdapter.ToSLNetFlowControl(this.SecurityConfig.EncryptionAlgorithm), Parity = Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithmAdapter.ToSLNetParity(this.SecurityConfig.AuthenticationAlgorithm), GetCommunity = Skyline.DataMiner.Library.Common.RSA.Encrypt(this.SecurityConfig.AuthenticationKey), SetCommunity = Skyline.DataMiner.Library.Common.RSA.Encrypt(this.SecurityConfig.EncryptionKey), DataBits = this.SecurityConfig.Username, PollingIPPort = System.Convert.ToString(this.udpIpConfiguration.RemotePort), IsSslTlsEnabled = this.udpIpConfiguration.IsSslTlsEnabled, PollingIPAddress = this.udpIpConfiguration.RemoteHost, PortID = portPosition, ProtocolType = Skyline.DataMiner.Net.Messages.ProtocolType.SnmpV3, Baudrate = System.String.Empty, LocalIPPort = this.udpIpConfiguration.LocalPort.ToString(), Number = this.udpIpConfiguration.NetworkInterfaceCard.ToString(), Type = "ip"};
                    return portInfo;
                }

                /// <summary>
                ///     Updates the provided ElementPortInfo object with any performed changes on the object.
                /// </summary>
                /// <param name = "portInfo"></param>
                /// <param name = "isCompatibilityIssueDetected"></param>
                internal override void UpdateElementPortInfo(Skyline.DataMiner.Net.Messages.ElementPortInfo portInfo, bool isCompatibilityIssueDetected)
                {
                    var mySecurityConfig = (Skyline.DataMiner.Library.Common.ConnectionSettings)this.securityConfig;
                    var udpSettings = (Skyline.DataMiner.Library.Common.ConnectionSettings)this.udpIpConfiguration;
                    foreach (Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting property in this.ChangedPropertyList)
                    {
                        switch (property)
                        {
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.DeviceAddress:
                                portInfo.BusAddress = this.deviceAddress;
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.Timeout:
                                portInfo.TimeoutTime = System.Convert.ToInt32(this.timeout.TotalMilliseconds);
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.ElementTimeout:
                                portInfo.ElementTimeoutTime = System.Convert.ToInt32(this.elementTimeout.Value.TotalMilliseconds);
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.Retries:
                                portInfo.Retries = this.retries;
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.PortConnection:
                                udpSettings.UpdateElementPortInfo(portInfo, isCompatibilityIssueDetected);
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.SecurityConfig:
                                mySecurityConfig.UpdateElementPortInfo(portInfo, isCompatibilityIssueDetected);
                                break;
                            default:
                                continue;
                        }
                    }

                    portInfo.ProtocolType = Skyline.DataMiner.Net.Messages.ProtocolType.SnmpV3;
                    if (mySecurityConfig.IsUpdated)
                    {
                        mySecurityConfig.UpdateElementPortInfo(portInfo, isCompatibilityIssueDetected);
                    }

                    if (udpSettings.IsUpdated)
                    {
                        udpSettings.UpdateElementPortInfo(portInfo, isCompatibilityIssueDetected);
                    }
                }
            }

            /// <summary>
            /// Allows adapting the enum to other library equivalents.
            /// </summary>
            internal static class SnmpV3EncryptionAlgorithmAdapter
            {
                /// <summary>
                /// Converts SLNet flowControl string into the enum.
                /// </summary>
                /// <param name = "flowControl">flowControl string received from SLNet.</param>
                /// <returns>The equivalent enum.</returns>
                public static Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm FromSLNetFlowControl(string flowControl)
                {
                    string noCaseFlowControl = flowControl.ToUpper();
                    switch (noCaseFlowControl)
                    {
                        case "DES":
                            return Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.Des;
                        case "AES128":
                            return Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.Aes128;
                        case "AES192":
                            return Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.Aes192;
                        case "AES256":
                            return Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.Aes256;
                        case "DEFINEDINCREDENTIALSLIBRARY":
                            return Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.DefinedInCredentialsLibrary;
                        case "NONE":
                        default:
                            return Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.None;
                    }
                }

                /// <summary>
                /// Converts the enum into the equivalent SLNet string value.
                /// </summary>
                /// <param name = "value">The enum you wish to convert.</param>
                /// <returns>The equivalent string value.</returns>
                public static string ToSLNetFlowControl(Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm value)
                {
                    switch (value)
                    {
                        case Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.Des:
                            return "DES";
                        case Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.Aes128:
                            return "AES128";
                        case Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.Aes192:
                            return "AES192";
                        case Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.Aes256:
                            return "AES256";
                        case Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.DefinedInCredentialsLibrary:
                        case Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.None:
                        default:
                            return value.ToString();
                    }
                }
            }

            /// <summary>
            ///     Represents the Security settings linked to SNMPv3.
            /// </summary>
            public class SnmpV3SecurityConfig : Skyline.DataMiner.Library.Common.ConnectionSettings, Skyline.DataMiner.Library.Common.ISnmpV3SecurityConfig
            {
                private Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm authenticationAlgorithm;
                private string authenticationKey;
                private Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm encryptionAlgorithm;
                private string encryptionKey;
                private Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol securityLevelAndProtocol;
                private string username;
                /// <summary>
                ///     Initializes a new instance using No Authentication and No Privacy.
                /// </summary>
                /// <param name = "username">The username.</param>
                /// <exception cref = "System.ArgumentNullException">When the username is null.</exception>
                public SnmpV3SecurityConfig(string username)
                {
                    if (username == null)
                    {
                        throw new System.ArgumentNullException("username");
                    }

                    this.securityLevelAndProtocol = Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol.NoAuthenticationNoPrivacy;
                    this.username = username;
                    this.authenticationKey = string.Empty;
                    this.encryptionKey = string.Empty;
                    this.authenticationAlgorithm = Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.None;
                    this.encryptionAlgorithm = Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.None;
                }

                /// <summary>
                ///     Initializes a new instance using Authentication No Privacy.
                /// </summary>
                /// <param name = "username">The username.</param>
                /// <param name = "authenticationKey">The Authentication key.</param>
                /// <param name = "authenticationAlgorithm">The Authentication Algorithm.</param>
                /// <exception cref = "System.ArgumentNullException">When username, authenticationKey is null.</exception>
                /// <exception cref = "IncorrectDataException">
                ///     When None or DefinedInCredentialsLibrary is selected as authentication
                ///     algorithm.
                /// </exception>
                public SnmpV3SecurityConfig(string username, string authenticationKey, Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm authenticationAlgorithm)
                {
                    if (username == null)
                    {
                        throw new System.ArgumentNullException("username");
                    }

                    if (authenticationKey == null)
                    {
                        throw new System.ArgumentNullException("authenticationKey");
                    }

                    if (authenticationAlgorithm == Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.None || authenticationAlgorithm == Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.DefinedInCredentialsLibrary)
                    {
                        throw new Skyline.DataMiner.Library.Common.IncorrectDataException("Authentication Algorithm 'None' and 'DefinedInCredentialsLibrary' is Invalid when choosing 'Authentication No Privacy' as Security Level and Protocol.");
                    }

                    this.securityLevelAndProtocol = Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol.AuthenticationNoPrivacy;
                    this.username = username;
                    this.authenticationKey = authenticationKey;
                    this.encryptionKey = string.Empty;
                    this.authenticationAlgorithm = authenticationAlgorithm;
                    this.encryptionAlgorithm = Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.None;
                }

                /// <summary>
                ///     Initializes a new instance using Authentication and Privacy.
                /// </summary>
                /// <param name = "username">The username.</param>
                /// <param name = "authenticationKey">The authentication key.</param>
                /// <param name = "encryptionKey">The encryptionKey.</param>
                /// <param name = "authenticationProtocol">The authentication algorithm.</param>
                /// <param name = "encryptionAlgorithm">The encryption algorithm.</param>
                /// <exception cref = "System.ArgumentNullException">When username, authenticationKey or encryptionKey is null.</exception>
                /// <exception cref = "IncorrectDataException">
                ///     When None or DefinedInCredentialsLibrary is selected as authentication
                ///     algorithm or encryption algorithm.
                /// </exception>
                public SnmpV3SecurityConfig(string username, string authenticationKey, Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm authenticationProtocol, string encryptionKey, Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm encryptionAlgorithm)
                {
                    if (username == null)
                    {
                        throw new System.ArgumentNullException("username");
                    }

                    if (authenticationKey == null)
                    {
                        throw new System.ArgumentNullException("authenticationKey");
                    }

                    if (encryptionKey == null)
                    {
                        throw new System.ArgumentNullException("encryptionKey");
                    }

                    if (authenticationProtocol == Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.None || authenticationProtocol == Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.DefinedInCredentialsLibrary)
                    {
                        throw new Skyline.DataMiner.Library.Common.IncorrectDataException("Authentication Algorithm 'None' and 'DefinedInCredentialsLibrary' is Invalid when choosing 'Authentication No Privacy' as Security Level and Protocol.");
                    }

                    if (encryptionAlgorithm == Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.None || encryptionAlgorithm == Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.DefinedInCredentialsLibrary)
                    {
                        throw new Skyline.DataMiner.Library.Common.IncorrectDataException("Encryption Algorithm 'None' and 'DefinedInCredentialsLibrary' is Invalid when choosing 'Authentication and Privacy' as Security Level and Protocol.");
                    }

                    this.securityLevelAndProtocol = Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol.AuthenticationPrivacy;
                    this.username = username;
                    this.authenticationKey = authenticationKey;
                    this.encryptionKey = encryptionKey;
                    this.authenticationAlgorithm = authenticationProtocol;
                    this.encryptionAlgorithm = encryptionAlgorithm;
                }

                /// <summary>
                ///     Default empty constructor
                /// </summary>
                public SnmpV3SecurityConfig()
                {
                }

                /// <summary>
                ///     Initializes a new instance.
                /// </summary>
                /// <param name = "securityLevelAndProtocol">The security Level and Protocol.</param>
                /// <param name = "username">The username.</param>
                /// <param name = "authenticationKey">The authenticationKey</param>
                /// <param name = "encryptionKey">The encryptionKey</param>
                /// <param name = "authenticationAlgorithm">The authentication Algorithm.</param>
                /// <param name = "encryptionAlgorithm">The encryption Algorithm.</param>
                /// <exception cref = "System.ArgumentNullException">When username, authenticationKey or encryptionKey is null.</exception>
                internal SnmpV3SecurityConfig(Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol securityLevelAndProtocol, string username, string authenticationKey, string encryptionKey, Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm authenticationAlgorithm, Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm encryptionAlgorithm)
                {
                    if (username == null)
                    {
                        throw new System.ArgumentNullException("username");
                    }

                    if (authenticationKey == null)
                    {
                        throw new System.ArgumentNullException("authenticationKey");
                    }

                    if (encryptionKey == null)
                    {
                        throw new System.ArgumentNullException("encryptionKey");
                    }

                    this.securityLevelAndProtocol = securityLevelAndProtocol;
                    this.username = username;
                    this.authenticationKey = authenticationKey;
                    this.encryptionKey = encryptionKey;
                    this.authenticationAlgorithm = authenticationAlgorithm;
                    this.encryptionAlgorithm = encryptionAlgorithm;
                }

                /// <summary>
                ///     Gets or sets the AuthenticationProtocol.
                /// </summary>
                public Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm AuthenticationAlgorithm
                {
                    get
                    {
                        return this.authenticationAlgorithm;
                    }

                    set
                    {
                        if (this.authenticationAlgorithm != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.AuthenticationProtocol);
                            this.authenticationAlgorithm = value;
                        }
                    }
                }

                /// <summary>
                ///     Gets or sets the AuthenticationKey.
                /// </summary>
                public string AuthenticationKey
                {
                    get
                    {
                        return this.authenticationKey;
                    }

                    set
                    {
                        if (this.AuthenticationKey != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.AuthenticationKey);
                            this.authenticationKey = value;
                        }
                    }
                }

                /// <summary>
                ///     Gets or sets the EncryptionAlgorithm.
                /// </summary>
                public Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm EncryptionAlgorithm
                {
                    get
                    {
                        return this.encryptionAlgorithm;
                    }

                    set
                    {
                        if (this.encryptionAlgorithm != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.EncryptionAlgorithm);
                            this.encryptionAlgorithm = value;
                        }
                    }
                }

                /// <summary>
                ///     Gets or sets the EncryptionKey.
                /// </summary>
                public string EncryptionKey
                {
                    get
                    {
                        return this.encryptionKey;
                    }

                    set
                    {
                        if (this.encryptionKey != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.EncryptionKey);
                            this.encryptionKey = value;
                        }
                    }
                }

                /// <summary>
                ///     Gets or sets the SecurityLevelAndProtocol.
                /// </summary>
                public Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol SecurityLevelAndProtocol
                {
                    get
                    {
                        return this.securityLevelAndProtocol;
                    }

                    set
                    {
                        if (this.securityLevelAndProtocol != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.SecurityLevelAndProtocol);
                            this.securityLevelAndProtocol = value;
                        }
                    }
                }

                /// <summary>
                ///     Gets or sets the username.
                /// </summary>
                public string Username
                {
                    get
                    {
                        return this.username;
                    }

                    set
                    {
                        if (this.username != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.Username);
                            this.username = value;
                        }
                    }
                }

                /// <summary>
                ///     Indicates if a property has been updated or not.
                /// </summary>
                internal override bool IsUpdated
                {
                    get
                    {
                        return System.Linq.Enumerable.Any(this.ChangedPropertyList);
                    }
                }

                /// <summary>
                ///     Clear the performed update flags of the properties of the object.
                /// </summary>
                internal override void ClearUpdates()
                {
                    this.ChangedPropertyList.Clear();
                }

                /// <summary>
                ///     Creates an ElementPortInfo object based on the property values.
                /// </summary>
                /// <returns></returns>
                internal override Skyline.DataMiner.Net.Messages.ElementPortInfo CreateElementPortInfo(int portPosition, bool isCompatibilityIssueDetected)
                {
                    throw new System.NotSupportedException("Method is not supported. ElementPortInfo content is directly created in corresponding connection.");
                }

                /// <summary>
                ///     Update the ElementPortInfo object with the changed properties.
                /// </summary>
                /// <param name = "portInfo"></param>
                /// <param name = "isCompatibilityIssueDetected"></param>
                internal override void UpdateElementPortInfo(Skyline.DataMiner.Net.Messages.ElementPortInfo portInfo, bool isCompatibilityIssueDetected)
                {
                    foreach (Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting property in this.ChangedPropertyList)
                    {
                        switch (property)
                        {
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.SecurityLevelAndProtocol:
                                portInfo.StopBits = Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocolAdapter.ToSLNetStopBits(this.securityLevelAndProtocol);
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.EncryptionAlgorithm:
                                portInfo.FlowControl = Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithmAdapter.ToSLNetFlowControl(this.encryptionAlgorithm);
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.AuthenticationProtocol:
                                portInfo.Parity = Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithmAdapter.ToSLNetParity(this.authenticationAlgorithm);
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.GetCommunityString:
                                portInfo.GetCommunity = Skyline.DataMiner.Library.Common.RSA.Encrypt(this.authenticationKey);
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.EncryptionKey:
                                portInfo.SetCommunity = Skyline.DataMiner.Library.Common.RSA.Encrypt(this.encryptionKey);
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.Username:
                                portInfo.DataBits = this.username;
                                break;
                        }
                    }
                }
            }

            /// <summary>
            /// Allows adapting the enum to other library equivalents.
            /// </summary>
            internal static class SnmpV3SecurityLevelAndProtocolAdapter
            {
                /// <summary>
                /// Converts SLNet stopBits string into the enum.
                /// </summary>
                /// <param name = "stopBits">stopBits string received from SLNet.</param>
                /// <returns>The equivalent enum.</returns>
                public static Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol FromSLNetStopBits(string stopBits)
                {
                    string noCaseStopBits = stopBits.ToUpper();
                    switch (noCaseStopBits)
                    {
                        case "AUTHPRIV":
                        case "AUTHENTICATIONPRIVACY":
                            return Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol.AuthenticationPrivacy;
                        case "AUTHNOPRIV":
                        case "AUTHENTICATIONNOPRIVACY":
                            return Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol.AuthenticationNoPrivacy;
                        case "NOAUTHNOPRIV":
                        case "NOAUTHENTICATIONNOPRIVACY":
                            return Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol.NoAuthenticationNoPrivacy;
                        case "DEFINEDINCREDENTIALSLIBRARY":
                            return Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol.DefinedInCredentialsLibrary;
                        default:
                            return Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol.None;
                    }
                }

                /// <summary>
                /// Converts the enum into the equivalent SLNet string value.
                /// </summary>
                /// <param name = "value">The enum you wish to convert.</param>
                /// <returns>The equivalent string value.</returns>
                public static string ToSLNetStopBits(Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol value)
                {
                    switch (value)
                    {
                        case Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol.AuthenticationPrivacy:
                            return "authPriv";
                        case Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol.AuthenticationNoPrivacy:
                            return "authNoPriv";
                        case Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol.NoAuthenticationNoPrivacy:
                            return "noAuthNoPriv";
                        case Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol.DefinedInCredentialsLibrary:
                            return "DefinedInCredentialsLibrary";
                        case Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol.None:
                        default:
                            return string.Empty;
                    }
                }
            }

            /// <summary>
            /// Class representing a Virtual connection. 
            /// </summary>
            public class VirtualConnection : Skyline.DataMiner.Library.Common.ConnectionSettings, Skyline.DataMiner.Library.Common.IVirtualConnection
            {
                private readonly int id;
                /// <summary>
                /// Initiates a new VirtualConnection class.
                /// </summary>
                /// <param name = "info"></param>
                internal VirtualConnection(Skyline.DataMiner.Net.Messages.ElementPortInfo info)
                {
                    this.id = info.PortID;
                }

                /// <summary>
                /// Initiates a new VirtualConnection class.
                /// </summary>
                public VirtualConnection()
                {
                    this.id = -1;
                }

                /// <summary>
                /// Gets the zero based id of the connection.
                /// </summary>
                public int Id
                {
                    get
                    {
                        return id;
                    }
                }

                /// <summary>
                /// Create an ElementPortInfo object from the properties.
                /// </summary>
                /// <returns></returns>
                internal override Skyline.DataMiner.Net.Messages.ElementPortInfo CreateElementPortInfo(int portPosition, bool isCompatibilityIssueDetected)
                {
                    Skyline.DataMiner.Net.Messages.ElementPortInfo portInfo = new Skyline.DataMiner.Net.Messages.ElementPortInfo{PortID = portPosition, ProtocolType = Skyline.DataMiner.Net.Messages.ProtocolType.Virtual};
                    return portInfo;
                }

                /// <summary>
                /// Update an ElementPortInfo object based on the performed changes.
                /// </summary>
                /// <param name = "portInfo"></param>
                /// <param name = "isCompatibilityIssueDetected"></param>
                internal override void UpdateElementPortInfo(Skyline.DataMiner.Net.Messages.ElementPortInfo portInfo, bool isCompatibilityIssueDetected)
                {
                //nothing to update but no exception can be thrown.
                }

                /// <summary>
                /// Returns whether a property has been set or not.
                /// </summary>
                internal override bool IsUpdated
                {
                    get
                    {
                        return (System.Linq.Enumerable.Any(ChangedPropertyList));
                    }
                }

                /// <summary>
                /// Clear the performed update flags of the properties of the object.
                /// </summary>
                internal override void ClearUpdates()
                {
                    this.ChangedPropertyList.Clear();
                }
            }

            /// <summary>
            /// Specifies the SNMPv3 authentication protocol.
            /// </summary>
            public enum SnmpV3AuthenticationAlgorithm
            {
                /// <summary>
                /// Message Digest 5 (MD5).
                /// </summary>
                Md5 = 0,
                /// <summary>
                /// Secure Hash Algorithm (SHA).
                /// </summary>
                Sha1 = 1,
                /// <summary>
                /// Secure Hash Algorithm (SHA) 224.
                /// </summary>
                Sha224 = 2,
                /// <summary>
                /// Secure Hash Algorithm (SHA) 256.
                /// </summary>
                Sha256 = 3,
                /// <summary>
                /// Secure Hash Algorithm (SHA) 384.
                /// </summary>
                Sha384 = 4,
                /// <summary>
                /// Secure Hash Algorithm (SHA) 512.
                /// </summary>
                Sha512 = 5,
                /// <summary>
                /// Algorithm is defined in the Credential Library.
                /// </summary>
                DefinedInCredentialsLibrary = 6,
                /// <summary>
                /// No algorithm selected.
                /// </summary>
                None = 7
            }

            /// <summary>
            /// Allows adapting the enum to other library equivalents.
            /// </summary>
            public class SnmpV3AuthenticationAlgorithmAdapter
            {
                /// <summary>
                /// Converts SLNet parity string into the enum.
                /// </summary>
                /// <param name = "parity">Parity string received from SLNet.</param>
                /// <returns>The equivalent enum.</returns>
                public static Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm FromSLNetParity(string parity)
                {
                    string noCaseParity = parity.ToUpper();
                    switch (noCaseParity)
                    {
                        case "MD5":
                        case "HMAC-MD5":
                            return Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.Md5;
                        case "SHA":
                        case "SHA1":
                        case "HMAC-SHA":
                            return Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.Sha1;
                        case "SHA224":
                            return Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.Sha224;
                        case "SHA256":
                            return Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.Sha256;
                        case "SHA384":
                            return Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.Sha384;
                        case "SHA512":
                            return Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.Sha512;
                        case "DEFINEDINCREDENTIALSLIBRARY":
                            return Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.DefinedInCredentialsLibrary;
                        case "NONE":
                        default:
                            return Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.None;
                    }
                }

                /// <summary>
                /// Converts the enum into the equivalent SLNet string value.
                /// </summary>
                /// <param name = "value">The enum you wish to convert.</param>
                /// <returns>The equivalent string value.</returns>
                public static string ToSLNetParity(Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm value)
                {
                    switch (value)
                    {
                        case Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.Md5:
                            // Verified with server side: SLSNMPManager can handle both MD5 or HMAC-MD5 as input.
                            return "MD5";
                        case Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.Sha1:
                            return "SHA";
                        case Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.Sha224:
                            return "SHA224";
                        case Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.Sha256:
                            return "SHA256";
                        case Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.Sha384:
                            return "SHA384";
                        case Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.Sha512:
                            return "SHA512";
                        case Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.None:
                            return "None";
                        case Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.DefinedInCredentialsLibrary:
                        default:
                            return value.ToString();
                    }
                }
            }

            /// <summary>
            /// Specifies the SNMPv3 encryption algorithm.
            /// </summary>
            public enum SnmpV3EncryptionAlgorithm
            {
                /// <summary>
                /// Data Encryption Standard (DES).
                /// </summary>
                Des = 0,
                /// <summary>
                /// Advanced Encryption Standard (AES) 128 bit.
                /// </summary>
                Aes128 = 1,
                /// <summary>
                /// Advanced Encryption Standard (AES) 192 bit.
                /// </summary>
                Aes192 = 2,
                /// <summary>
                /// Advanced Encryption Standard (AES) 256 bit.
                /// </summary>
                Aes256 = 3,
                /// <summary>
                /// Advanced Encryption Standard is defined in the Credential Library.
                /// </summary>
                DefinedInCredentialsLibrary = 4,
                /// <summary>
                /// No algorithm selected.
                /// </summary>
                None = 5
            }

            /// <summary>
            /// Specifies the SNMP v3 security level and protocol.
            /// </summary>
            public enum SnmpV3SecurityLevelAndProtocol
            {
                /// <summary>
                /// Authentication and privacy.
                /// </summary>
                AuthenticationPrivacy = 0,
                /// <summary>
                /// Authentication but no privacy.
                /// </summary>
                AuthenticationNoPrivacy = 1,
                /// <summary>
                /// No authentication and no privacy.
                /// </summary>
                NoAuthenticationNoPrivacy = 2,
                /// <summary>
                /// Security Level and Protocol is defined in the Credential library.
                /// </summary>
                DefinedInCredentialsLibrary = 3,
                /// <summary>
                /// No algorithm selected.
                /// </summary>
                None = 4
            }

            /// <summary>
            /// Represents a connection using the Internet Protocol (IP).
            /// </summary>
            public interface IIpBased : Skyline.DataMiner.Library.Common.IPortConnection
            {
                /// <summary>
                /// Gets or sets the host name or IP address of the host to connect to.
                /// </summary>
                /// <value>The host name or IP address of the host to connect to.</value>
                string RemoteHost
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the network interface card (NIC).
                /// </summary>
                /// <value>The network interface card (NIC). A value of 0 means the network interface card will be selected automatically.</value>
                int NetworkInterfaceCard
                {
                    get;
                    set;
                }
            }

            /// <summary>
            /// interface IPortConnection for which all connections will inherit from.
            /// </summary>
            public interface IPortConnection
            {
            }

            /// <summary>
            /// Represents a TCP/IP connection.
            /// </summary>
            public interface ITcp : Skyline.DataMiner.Library.Common.IIpBased
            {
                /// <summary>
                /// Gets or sets the local port number.
                /// </summary>
                /// <value>The local port number.</value>
                /// <remarks>Configuring the local port is only supported for <see cref = "ISerialConnection"/> connections.</remarks>
                int? LocalPort
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or set the remote port number.
                /// </summary>
                /// <value>The remote port number.</value>
                int? RemotePort
                {
                    get;
                    set;
                }

                /// <summary>
                /// Indicates if SSL/TLS is enabled on the connection.
                /// </summary>
                /// <remarks>Can only be set to true on connection for protocol type Serial and port type IP.</remarks>
                bool IsSslTlsEnabled
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets if a dedicated connection is used.
                /// </summary>
                /// <remarks>This is the "single" of <see cref = "ISerialConnection"/> and <see cref = "ISmartSerialConnection"/>. Cannot be configured.</remarks>
                bool IsDedicated
                {
                    get;
                }
            }

            /// <summary>
            /// Represents a UDP/IP connection.
            /// </summary>
            public interface IUdp : Skyline.DataMiner.Library.Common.IIpBased
            {
                /// <summary>
                /// Gets or sets the local port number.
                /// </summary>
                /// <value>The local port number.</value>
                /// <remarks>Configuring the local port is only supported for <see cref = "ISerialConnection"/> connections.</remarks>
                int? LocalPort
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or set the remote port number.
                /// </summary>
                /// <value>The remote port number.</value>
                int? RemotePort
                {
                    get;
                    set;
                }

                /// <summary>
                /// Indicates if SSL/TLS is enabled on the connection.
                /// </summary>
                bool IsSslTlsEnabled
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets if a dedicated connection is used.
                /// </summary>
                bool IsDedicated
                {
                    get;
                }
            }

            /// <summary>
            /// Class representing a TCP connection.
            /// </summary>
            public class Tcp : Skyline.DataMiner.Library.Common.ConnectionSettings, Skyline.DataMiner.Library.Common.ITcp
            {
                private string remoteHost;
                private int networkInterfaceCard;
                private int? localPort;
                private int? remotePort;
                private bool isSslTlsEnabled;
                private readonly bool isDedicated;
                internal Tcp(Skyline.DataMiner.Net.Messages.ElementPortInfo info)
                {
                    this.remoteHost = info.PollingIPAddress;
                    if (!info.PollingIPPort.Equals(System.String.Empty))
                        remotePort = System.Convert.ToInt32(info.PollingIPPort);
                    if (!info.LocalIPPort.Equals(System.String.Empty))
                        localPort = System.Convert.ToInt32(info.LocalIPPort);
                    this.isSslTlsEnabled = info.IsSslTlsEnabled;
                    this.isDedicated = Skyline.DataMiner.Library.Common.HelperClass.IsDedicatedConnection(info);
                    int networkInterfaceId = System.String.IsNullOrWhiteSpace(info.Number) ? 0 : System.Convert.ToInt32(info.Number);
                    this.networkInterfaceCard = networkInterfaceId;
                }

                /// <summary>
                /// Initializes a new instance, using default values for localPort (null=Auto) and NetworkInterfaceCard (0=Auto)
                /// </summary>
                /// <param name = "remoteHost">The IP or name of the remote host.</param>
                /// <param name = "remotePort">The port number of the remote host.</param>
                public Tcp(string remoteHost, int remotePort)
                {
                    this.localPort = null;
                    this.remotePort = remotePort;
                    this.remoteHost = remoteHost;
                    this.networkInterfaceCard = 0;
                    this.isDedicated = false;
                }

                /// <summary>
                /// Default empty constructor.
                /// </summary>
                public Tcp()
                {
                }

                /// <summary>
                /// Gets or sets the IP Address or name of the remote host.
                /// </summary>
                public string RemoteHost
                {
                    get
                    {
                        return this.remoteHost;
                    }

                    set
                    {
                        if (this.remoteHost != value)
                        {
                            ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.RemoteHost);
                            this.remoteHost = value;
                        }
                    }
                }

                /// <summary>
                /// Gets or sets the network interface card number.
                /// </summary>
                public int NetworkInterfaceCard
                {
                    get
                    {
                        return this.networkInterfaceCard;
                    }

                    set
                    {
                        if (this.networkInterfaceCard != value)
                        {
                            ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.NetworkInterfaceCard);
                            networkInterfaceCard = value;
                        }
                    }
                }

                /// <summary>
                /// Gets or sets the local port.
                /// </summary>
                public int? LocalPort
                {
                    get
                    {
                        return localPort;
                    }

                    set
                    {
                        if (this.localPort != value)
                        {
                            ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.LocalPort);
                            this.localPort = value;
                        }
                    }
                }

                /// <summary>
                /// Gets or sets the remote port.
                /// </summary>
                public int? RemotePort
                {
                    get
                    {
                        return remotePort;
                    }

                    set
                    {
                        if (this.remotePort != value)
                        {
                            ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.RemotePort);
                            remotePort = value;
                        }
                    }
                }

                /// <summary>
                /// Indicates if SSL/TLS is enabled on the connection.
                /// </summary>
                /// <remarks>Can only be set to true on connection for protocol type Serial and port type IP.</remarks>
                public bool IsSslTlsEnabled
                {
                    get
                    {
                        return this.isSslTlsEnabled;
                    }

                    set
                    {
                        if (this.isSslTlsEnabled != value)
                        {
                            ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.IsSslTlsEnabled);
                            this.isSslTlsEnabled = value;
                        }
                    }
                }

                /// <summary>
                /// Gets or sets if a dedicated connection is used.
                /// </summary>
                /// <remarks>This is the "single" of <see cref = "ISerialConnection"/> and <see cref = "ISmartSerialConnection"/>. Cannot be configured.</remarks>
                public bool IsDedicated
                {
                    get
                    {
                        return this.isDedicated;
                    }
                }

                internal override bool IsUpdated
                {
                    get
                    {
                        return System.Linq.Enumerable.Any(this.ChangedPropertyList);
                    }
                }

                internal override void ClearUpdates()
                {
                    this.ChangedPropertyList.Clear();
                }

                internal override Skyline.DataMiner.Net.Messages.ElementPortInfo CreateElementPortInfo(int portPosition, bool isCompatibilityIssueDetected)
                {
                    throw new System.NotSupportedException("Method is not supported. ElementPortInfo content is directly created in corresponding connection.");
                }

                internal override void UpdateElementPortInfo(Skyline.DataMiner.Net.Messages.ElementPortInfo portInfo, bool isCompatibilityIssueDetected)
                {
                    foreach (Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting property in this.ChangedPropertyList)
                    {
                        switch (property)
                        {
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.LocalPort:
                                portInfo.LocalIPPort = System.Convert.ToString(this.localPort);
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.RemotePort:
                                portInfo.PollingIPPort = System.Convert.ToString(this.remotePort);
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.IsSslTlsEnabled:
                                portInfo.IsSslTlsEnabled = this.isSslTlsEnabled;
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.RemoteHost:
                                portInfo.PollingIPAddress = this.remoteHost;
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.NetworkInterfaceCard:
                                portInfo.Number = System.Convert.ToString(this.networkInterfaceCard);
                                break;
                            default:
                                continue;
                        }
                    }
                }
            }

            /// <summary>
            ///     Class representing an UDP connection.
            /// </summary>
            public sealed class Udp : Skyline.DataMiner.Library.Common.ConnectionSettings, Skyline.DataMiner.Library.Common.IUdp
            {
                /// <summary>
                ///		Compares two instances of this object by comparing the property fields.
                /// </summary>
                /// <param name = "other">The object to compare to.</param>
                /// <returns>Boolean indicating if object is equal or not.</returns>
                public bool Equals(Skyline.DataMiner.Library.Common.Udp other)
                {
                    return this.isDedicated == other.isDedicated && this.isSslTlsEnabled == other.isSslTlsEnabled && this.localPort == other.localPort && this.networkInterfaceCard == other.networkInterfaceCard && string.Equals(this.remoteHost, other.remoteHost, System.StringComparison.InvariantCulture) && this.remotePort == other.remotePort;
                }

                /// <summary>Determines whether the specified object is equal to the current object.</summary>
                /// <param name = "obj">The object to compare with the current object. </param>
                /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
                public override bool Equals(object obj)
                {
                    if (ReferenceEquals(null, obj))
                        return false;
                    if (ReferenceEquals(this, obj))
                        return true;
                    if (obj.GetType() != this.GetType())
                        return false;
                    return Equals((Skyline.DataMiner.Library.Common.Udp)obj);
                }

                /// <summary>Serves as the default hash function. </summary>
                /// <returns>A hash code for the current object.</returns>
                public override int GetHashCode()
                {
                    unchecked
                    {
                        int hashCode = this.isDedicated.GetHashCode();
                        hashCode = (hashCode * 397) ^ this.isSslTlsEnabled.GetHashCode();
                        hashCode = (hashCode * 397) ^ this.localPort.GetHashCode();
                        hashCode = (hashCode * 397) ^ this.networkInterfaceCard;
                        hashCode = (hashCode * 397) ^ (this.remoteHost != null ? System.StringComparer.InvariantCulture.GetHashCode(this.remoteHost) : 0);
                        hashCode = (hashCode * 397) ^ this.remotePort.GetHashCode();
                        return hashCode;
                    }
                }

                private readonly bool isDedicated;
                private bool isSslTlsEnabled;
                private int? localPort;
                private int networkInterfaceCard;
                private string remoteHost;
                private int? remotePort;
                /// <summary>
                ///     Initializes a new instance, using default values for localPort (null=Auto) SslTlsEnabled (false), IsDedicated
                ///     (false) and NetworkInterfaceCard (0=Auto)
                /// </summary>
                /// <param name = "remoteHost">The IP or name of the remote host.</param>
                /// <param name = "remotePort">The port number of the remote host.</param>
                public Udp(string remoteHost, int remotePort)
                {
                    this.localPort = null;
                    this.remotePort = remotePort;
                    this.isSslTlsEnabled = false;
                    this.isDedicated = false;
                    this.remoteHost = remoteHost;
                    this.networkInterfaceCard = 0;
                }

                /// <summary>
                ///     Default empty constructor
                /// </summary>
                public Udp()
                {
                }

                /// <summary>
                ///     Initializes a new instance using a <see cref = "ElementPortInfo"/> object.
                /// </summary>
                /// <param name = "info"></param>
                internal Udp(Skyline.DataMiner.Net.Messages.ElementPortInfo info)
                {
                    this.remoteHost = info.PollingIPAddress;
                    if (!info.PollingIPPort.Equals(System.String.Empty))
                        remotePort = System.Convert.ToInt32(info.PollingIPPort);
                    if (!info.LocalIPPort.Equals(System.String.Empty))
                        localPort = System.Convert.ToInt32(info.LocalIPPort);
                    this.isSslTlsEnabled = info.IsSslTlsEnabled;
                    this.isDedicated = Skyline.DataMiner.Library.Common.HelperClass.IsDedicatedConnection(info);
                    int networkInterfaceId = string.IsNullOrWhiteSpace(info.Number) ? 0 : System.Convert.ToInt32(info.Number);
                    this.networkInterfaceCard = networkInterfaceId;
                }

                /// <summary>
                ///     Gets or sets if a dedicated connection is used.
                /// </summary>
                public bool IsDedicated
                {
                    get
                    {
                        return this.isDedicated;
                    }
                }

                /// <summary>
                ///     Gets or sets if TLS is enabled on the connection.
                /// </summary>
                public bool IsSslTlsEnabled
                {
                    get
                    {
                        return this.isSslTlsEnabled;
                    }

                    set
                    {
                        if (this.isSslTlsEnabled != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.IsSslTlsEnabled);
                            this.isSslTlsEnabled = value;
                        }
                    }
                }

                /// <summary>
                ///     Gets or sets the local port.
                /// </summary>
                public int? LocalPort
                {
                    get
                    {
                        return this.localPort;
                    }

                    set
                    {
                        if (this.localPort != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.LocalPort);
                            this.localPort = value;
                        }
                    }
                }

                /// <summary>
                ///     Gets or sets the network interface card number.
                /// </summary>
                public int NetworkInterfaceCard
                {
                    get
                    {
                        return this.networkInterfaceCard;
                    }

                    set
                    {
                        if (this.networkInterfaceCard != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.NetworkInterfaceCard);
                            this.networkInterfaceCard = value;
                        }
                    }
                }

                /// <summary>
                ///     Gets or sets the IP Address or name of the remote host.
                /// </summary>
                public string RemoteHost
                {
                    get
                    {
                        return this.remoteHost;
                    }

                    set
                    {
                        if (this.remoteHost != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.RemoteHost);
                            this.remoteHost = value;
                        }
                    }
                }

                /// <summary>
                ///     Gets or sets the remote port.
                /// </summary>
                public int? RemotePort
                {
                    get
                    {
                        return this.remotePort;
                    }

                    set
                    {
                        if (this.remotePort != value)
                        {
                            this.ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.RemotePort);
                            this.remotePort = value;
                        }
                    }
                }

                /// <summary>
                ///     Indicates whether changes have been applied to the properties.
                /// </summary>
                internal override bool IsUpdated
                {
                    get
                    {
                        return System.Linq.Enumerable.Any(this.ChangedPropertyList);
                    }
                }

                /// <summary>
                ///     Clear the list keeping track of all the changes performed on properties.
                /// </summary>
                internal override void ClearUpdates()
                {
                    this.ChangedPropertyList.Clear();
                }

                /// <summary>
                ///     Creates a new <see cref = "ElementPortInfo"/> object.
                /// </summary>
                /// <returns></returns>
                internal override Skyline.DataMiner.Net.Messages.ElementPortInfo CreateElementPortInfo(int portPosition, bool isCompatibilityIssueDetected)
                {
                    throw new System.NotSupportedException("Method is not supported. ElementPortInfo content is directly created in corresponding connection.");
                }

                /// <summary>
                ///     Updates the provided ElementPortInfo object with any performed changes on the object.
                /// </summary>
                /// <param name = "portInfo"></param>
                /// <param name = "isCompatibilityIssueDetected"></param>
                internal override void UpdateElementPortInfo(Skyline.DataMiner.Net.Messages.ElementPortInfo portInfo, bool isCompatibilityIssueDetected)
                {
                    foreach (Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting property in this.ChangedPropertyList)
                    {
                        switch (property)
                        {
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.LocalPort:
                                portInfo.LocalIPPort = System.Convert.ToString(this.localPort);
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.RemotePort:
                                portInfo.PollingIPPort = System.Convert.ToString(this.remotePort);
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.IsSslTlsEnabled:
                                portInfo.IsSslTlsEnabled = this.isSslTlsEnabled;
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.RemoteHost:
                                portInfo.PollingIPAddress = this.remoteHost;
                                break;
                            case Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.NetworkInterfaceCard:
                                portInfo.Number = System.Convert.ToString(this.networkInterfaceCard);
                                break;
                            default:
                                continue;
                        }
                    }
                }
            }

            /// <summary>
            /// Represents the advanced element information.
            /// </summary>
            internal class AdvancedSettings : Skyline.DataMiner.Library.Common.ElementSettings, Skyline.DataMiner.Library.Common.IAdvancedSettings
            {
                /// <summary>
                /// Value indicating whether the element is hidden.
                /// </summary>
                private bool isHidden;
                /// <summary>
                /// Value indicating whether the element is read-only.
                /// </summary>
                private bool isReadOnly;
                /// <summary>
                /// Indicates whether this is a simulated element.
                /// </summary>
                private bool isSimulation;
                /// <summary>
                /// The element timeout value.
                /// </summary>
                private System.TimeSpan timeout = new System.TimeSpan(0, 0, 30);
                /// <summary>
                /// Initializes a new instance of the <see cref = "AdvancedSettings"/> class.
                /// </summary>
                /// <param name = "dmsElement">The reference to the <see cref = "DmsElement"/> instance this object is part of.</param>
                internal AdvancedSettings(Skyline.DataMiner.Library.Common.DmsElement dmsElement): base(dmsElement)
                {
                }

                /// <summary>
                /// Gets or sets a value indicating whether the element is hidden.
                /// </summary>
                /// <value><c>true</c> if the element is hidden; otherwise, <c>false</c>.</value>
                /// <exception cref = "NotSupportedException">A set operation is not supported on a derived element.</exception>
                public bool IsHidden
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return isHidden;
                    }

                    set
                    {
                        DmsElement.LoadOnDemand();
                        if (DmsElement.RedundancySettings.IsDerived)
                        {
                            throw new System.NotSupportedException("This operation is not supported on a derived element.");
                        }

                        if (isHidden != value)
                        {
                            ChangedPropertyList.Add("IsHidden");
                            isHidden = value;
                        }
                    }
                }

                /// <summary>
                /// Gets or sets a value indicating whether the element is read-only.
                /// </summary>
                /// <value><c>true</c> if the element is read-only; otherwise, <c>false</c>.</value>
                /// <exception cref = "NotSupportedException">A set operation is not supported on a DVE or derived element.</exception>
                public bool IsReadOnly
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return isReadOnly;
                    }

                    set
                    {
                        if (DmsElement.DveSettings.IsChild || DmsElement.RedundancySettings.IsDerived)
                        {
                            throw new System.NotSupportedException("This operation is not supported on a DVE child or derived element.");
                        }

                        DmsElement.LoadOnDemand();
                        if (isReadOnly != value)
                        {
                            ChangedPropertyList.Add("IsReadOnly");
                            isReadOnly = value;
                        }
                    }
                }

                /// <summary>
                /// Gets a value indicating whether the element is running a simulation.
                /// </summary>
                /// <value><c>true</c> if the element is running a simulation; otherwise, <c>false</c>.</value>
                public bool IsSimulation
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return isSimulation;
                    }
                }

                /// <summary>
                /// Gets or sets the element timeout value.
                /// </summary>
                /// <value>The timeout value.</value>
                /// <exception cref = "ArgumentOutOfRangeException">The value specified for a set operation is not in the range of [0,120] s.</exception>
                /// <exception cref = "NotSupportedException">A set operation is not supported on a DVE or derived element.</exception>
                /// <remarks>Fractional seconds are ignored. For example, setting the timeout to a value of 3.5s results in setting it to 3s.</remarks>
                public System.TimeSpan Timeout
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return timeout;
                    }

                    set
                    {
                        if (DmsElement.DveSettings.IsChild || DmsElement.RedundancySettings.IsDerived)
                        {
                            throw new System.NotSupportedException("Setting the timeout is not supported on a DVE child or derived element.");
                        }

                        DmsElement.LoadOnDemand();
                        int timeoutInSeconds = (int)value.TotalSeconds;
                        if (timeoutInSeconds < 0 || timeoutInSeconds > 120)
                        {
                            throw new System.ArgumentOutOfRangeException("value", "The timeout value must be in the range of [0,120] s.");
                        }

                        if ((int)timeout.TotalSeconds != (int)value.TotalSeconds)
                        {
                            ChangedPropertyList.Add("Timeout");
                            timeout = value;
                        }
                    }
                }

                /// <summary>
                /// Returns the string representation of the object.
                /// </summary>
                /// <returns>String representation of the object.</returns>
                public override string ToString()
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendLine("ADVANCED SETTINGS:");
                    sb.AppendLine("==========================");
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Timeout: {0}{1}", Timeout, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Hidden: {0}{1}", IsHidden, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Simulation: {0}{1}", IsSimulation, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Read-only: {0}{1}", IsReadOnly, System.Environment.NewLine);
                    return sb.ToString();
                }

                /// <summary>
                /// Loads the information to the component.
                /// </summary>
                /// <param name = "elementInfo">The element information.</param>
                internal override void Load(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
                {
                    timeout = new System.TimeSpan(0, 0, 0, 0, elementInfo.ElementTimeoutTime);
                    isHidden = elementInfo.Hidden;
                    isReadOnly = elementInfo.IsReadOnly;
                    isSimulation = elementInfo.IsSimulated;
                }

                /// <summary>
                /// Fills in the needed properties in the AddElement message.
                /// </summary>
                /// <param name = "message">The AddElement message which will be sent to SLNet.</param>
                internal override void FillUpdate(Skyline.DataMiner.Net.Messages.AddElementMessage message)
                {
                    foreach (string property in ChangedPropertyList)
                    {
                        switch (property)
                        {
                            case "IsHidden":
                                message.IsHidden = isHidden;
                                break;
                            case "IsReadOnly":
                                message.IsReadOnly = isReadOnly;
                                break;
                            case "Timeout":
                                message.TimeoutTime = (int)timeout.TotalMilliseconds;
                                break;
                            default:
                                continue;
                        }
                    }
                }
            }

            /// <summary>
            ///  Represents a class containing the device details of an element.
            /// </summary>
            internal class DeviceSettings : Skyline.DataMiner.Library.Common.ElementSettings
            {
                /// <summary>
                /// The type of the element.
                /// </summary>
                private string type = System.String.Empty;
                /// <summary>
                /// Initializes a new instance of the <see cref = "DeviceSettings"/> class.
                /// </summary>
                /// <param name = "dmsElement">The reference to the DmsElement where this object will be used in.</param>
                internal DeviceSettings(Skyline.DataMiner.Library.Common.DmsElement dmsElement): base(dmsElement)
                {
                }

                /// <summary>
                /// Gets the element type.
                /// </summary>
                internal string Type
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return type;
                    }
                }

                /// <summary>
                /// Returns the string representation of the object.
                /// </summary>
                /// <returns>String representation of the object.</returns>
                public override string ToString()
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendLine("DEVICE SETTINGS:");
                    sb.AppendLine("==========================");
                    sb.AppendLine("Type: " + type);
                    return sb.ToString();
                }

                /// <summary>
                /// Fills in the needed properties in the AddElement message.
                /// </summary>
                /// <param name = "message">The AddElement message that will be sent to SLNet.</param>
                internal override void FillUpdate(Skyline.DataMiner.Net.Messages.AddElementMessage message)
                {
                }

                /// <summary>
                /// Loads the information to the component.
                /// </summary>
                /// <param name = "elementInfo">The element information.</param>
                internal override void Load(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
                {
                    type = elementInfo.Type ?? System.String.Empty;
                }
            }

            /// <summary>
            /// Represents DVE information of an element.
            /// </summary>
            internal class DveSettings : Skyline.DataMiner.Library.Common.ElementSettings, Skyline.DataMiner.Library.Common.IDveSettings
            {
                /// <summary>
                /// Value indicating whether DVE creation is enabled.
                /// </summary>
                private bool isDveCreationEnabled = true;
                /// <summary>
                /// Value indicating whether this element is a parent DVE.
                /// </summary>
                private bool isParent;
                /// <summary>
                /// The parent element.
                /// </summary>
                private Skyline.DataMiner.Library.Common.IDmsElement parent;
                /// <summary>
                /// Initializes a new instance of the <see cref = "DveSettings"/> class.
                /// </summary>
                /// <param name = "dmsElement">The reference to the DmsElement where this object will be used in.</param>
                internal DveSettings(Skyline.DataMiner.Library.Common.DmsElement dmsElement): base(dmsElement)
                {
                }

                /// <summary>
                /// Gets a value indicating whether this element is a DVE child.
                /// </summary>
                /// <value><c>true</c> if this element is a DVE child element; otherwise, <c>false</c>.</value>
                public bool IsChild
                {
                    get
                    {
                        return parent != null;
                    }
                }

                /// <summary>
                /// Gets or sets a value indicating whether DVE creation is enabled for this element.
                /// </summary>
                /// <value><c>true</c> if the element DVE generation is enabled; otherwise, <c>false</c>.</value>
                /// <exception cref = "NotSupportedException">The set operation is not supported: The element is not a DVE parent element.</exception>
                public bool IsDveCreationEnabled
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return isDveCreationEnabled;
                    }

                    set
                    {
                        DmsElement.LoadOnDemand();
                        if (!DmsElement.DveSettings.IsParent)
                        {
                            throw new System.NotSupportedException("This operation is only supported on DVE parent elements.");
                        }

                        if (isDveCreationEnabled != value)
                        {
                            ChangedPropertyList.Add("IsDveCreationEnabled");
                            isDveCreationEnabled = value;
                        }
                    }
                }

                /// <summary>
                /// Gets a value indicating whether this element is a DVE parent.
                /// </summary>
                /// <value><c>true</c> if the element is a DVE parent element; otherwise, <c>false</c>.</value>
                public bool IsParent
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return isParent;
                    }
                }

                /// <summary>
                /// Gets the parent element.
                /// </summary>
                /// <value>The parent element.</value>
                public Skyline.DataMiner.Library.Common.IDmsElement Parent
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return parent;
                    }
                }

                /// <summary>
                /// Returns the string representation of the object.
                /// </summary>
                /// <returns>String representation of the object.</returns>
                public override string ToString()
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendLine("DVE SETTINGS:");
                    sb.AppendLine("==========================");
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "DVE creation enabled: {0}{1}", IsDveCreationEnabled, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Is parent DVE: {0}{1}", IsParent, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Is child DVE: {0}{1}", IsChild, System.Environment.NewLine);
                    if (IsChild)
                    {
                        sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Parent DataMiner agent ID/element ID: {0}{1}", parent.DmsElementId.Value, System.Environment.NewLine);
                    }

                    return sb.ToString();
                }

                /// <summary>
                /// Loads the information to the component.
                /// </summary>
                /// <param name = "elementInfo">The element information.</param>
                internal override void Load(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
                {
                    if (elementInfo.IsDynamicElement && elementInfo.DveParentDmaId != 0 && elementInfo.DveParentElementId != 0)
                    {
                        parent = new Skyline.DataMiner.Library.Common.DmsElement(DmsElement.Dms, new Skyline.DataMiner.Library.Common.DmsElementId(elementInfo.DveParentDmaId, elementInfo.DveParentElementId));
                    }

                    isParent = elementInfo.IsDveMainElement;
                    isDveCreationEnabled = elementInfo.CreateDVEs;
                }

                /// <summary>
                /// Fills in the needed properties in the AddElement message.
                /// </summary>
                /// <param name = "message">The AddElement message that will be sent to SLNet.</param>
                internal override void FillUpdate(Skyline.DataMiner.Net.Messages.AddElementMessage message)
                {
                    foreach (string property in ChangedPropertyList)
                    {
                        if (property.Equals("IsDveCreationEnabled", System.StringComparison.Ordinal))
                        {
                            message.CreateDVEs = isDveCreationEnabled;
                        }
                    }
                }
            }

            /// <summary>
            /// Represents a class containing the failover settings for an element.
            /// </summary>
            internal class FailoverSettings : Skyline.DataMiner.Library.Common.ElementSettings, Skyline.DataMiner.Library.Common.IFailoverSettings
            {
                /// <summary>
                /// In failover configurations, this can be used to force an element to run only on one specific agent.
                /// </summary>
                private string forceAgent = System.String.Empty;
                /// <summary>
                /// Is true when the element is a failover element and is online on the backup agent instead of this agent; otherwise, false.
                /// </summary>
                private bool isOnlineOnBackupAgent;
                /// <summary>
                /// Is true when the element is a failover element that needs to keep running on the same DataMiner agent event after switching; otherwise, false.
                /// </summary>
                private bool keepOnline;
                /// <summary>
                /// Initializes a new instance of the <see cref = "FailoverSettings"/> class.
                /// </summary>
                /// <param name = "dmsElement">The reference to the DmsElement where this object will be used in.</param>
                internal FailoverSettings(Skyline.DataMiner.Library.Common.DmsElement dmsElement): base(dmsElement)
                {
                }

                /// <summary>
                /// Gets or sets a value indicating whether to force agent.
                /// Local IP address of the agent which will be running the element.
                /// </summary>
                /// <value>Value indicating whether to force agent.</value>
                public string ForceAgent
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return forceAgent;
                    }

                    set
                    {
                        DmsElement.LoadOnDemand();
                        var newValue = value == null ? System.String.Empty : value;
                        if (!forceAgent.Equals(newValue, System.StringComparison.Ordinal))
                        {
                            ChangedPropertyList.Add("ForceAgent");
                            forceAgent = newValue;
                        }
                    }
                }

                /// <summary>
                /// Gets a value indicating whether the element is a failover element and is online on the backup agent instead of this agent.
                /// </summary>
                /// <value><c>true</c> if the element is a failover element and is online on the backup agent instead of this agent; otherwise, <c>false</c>.</value>
                public bool IsOnlineOnBackupAgent
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return isOnlineOnBackupAgent;
                    }
                }

                /// <summary>
                /// Gets or sets a value indicating whether the element is a failover element that needs to keep running on the same DataMiner agent event after switching.
                /// keepOnline="true" indicates that the element needs to keep running even when the agent is offline.
                /// </summary>
                /// <value><c>true</c> if the element is a failover element that needs to keep running on the same DataMiner agent event after switching; otherwise, <c>false</c>.</value>
                public bool KeepOnline
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return keepOnline;
                    }

                    set
                    {
                        DmsElement.LoadOnDemand();
                        if (keepOnline != value)
                        {
                            ChangedPropertyList.Add("KeepOnline");
                            keepOnline = value;
                        }
                    }
                }

                /// <summary>
                /// Returns the string representation of the object.
                /// </summary>
                /// <returns>String representation of the object.</returns>
                public override string ToString()
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendLine("FAILOVER SETTINGS:");
                    sb.AppendLine("==========================");
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Keep online: {0}{1}", KeepOnline, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Force agent: {0}{1}", ForceAgent, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Online on backup agent: {0}{1}", IsOnlineOnBackupAgent, System.Environment.NewLine);
                    return sb.ToString();
                }

                /// <summary>
                /// Fills in the needed properties in the AddElement message.
                /// </summary>
                /// <param name = "message">The AddElement message which will be sent to SLNet.</param>
                internal override void FillUpdate(Skyline.DataMiner.Net.Messages.AddElementMessage message)
                {
                    foreach (string property in ChangedPropertyList)
                    {
                        switch (property)
                        {
                            case "ForceAgent":
                                message.ForceAgent = forceAgent;
                                break;
                            case "KeepOnline":
                                message.KeepOnline = keepOnline;
                                break;
                            default:
                                continue;
                        }
                    }
                }

                /// <summary>
                /// Loads the information to the component.
                /// </summary>
                /// <param name = "elementInfo">The element information.</param>
                internal override void Load(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
                {
                    keepOnline = elementInfo.KeepOnline;
                    forceAgent = elementInfo.ForceAgent ?? System.String.Empty;
                    isOnlineOnBackupAgent = elementInfo.IsOnlineOnBackupAgent;
                }
            }

            /// <summary>
            /// Represents general element information.
            /// </summary>
            internal class GeneralSettings : Skyline.DataMiner.Library.Common.ElementSettings
            {
                /// <summary>
                /// The name of the alarm template.
                /// </summary>
                private string alarmTemplateName;
                /// <summary>
                /// The SLNet call that will retrieve the alarm template from the system if needed.
                /// </summary>
                private System.Lazy<Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate> alarmTemplateLoader;
                /// <summary>
                /// The alarm template assigned to this element.
                /// </summary>
                private Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate alarmTemplate;
                /// <summary>
                /// Element description.
                /// </summary>
                private string description = System.String.Empty;
                /// <summary>
                /// The hosting DataMiner agent.
                /// </summary>
                private Skyline.DataMiner.Library.Common.Dma host;
                /// <summary>
                /// The element state.
                /// </summary>
                private Skyline.DataMiner.Library.Common.ElementState state = Skyline.DataMiner.Library.Common.ElementState.Active;
                /// <summary>
                /// Instance of the protocol this element executes.
                /// </summary>
                private Skyline.DataMiner.Library.Common.DmsProtocol protocol;
                /// <summary>
                /// The trend template assigned to this element.
                /// </summary>
                private Skyline.DataMiner.Library.Common.Templates.IDmsTrendTemplate trendTemplate;
                /// <summary>
                /// The name of the element.
                /// </summary>
                private string name;
                /// <summary>
                /// Initializes a new instance of the <see cref = "GeneralSettings"/> class.
                /// </summary>
                /// <param name = "dmsElement">The reference to the DmsElement where this object will be used in.</param>
                internal GeneralSettings(Skyline.DataMiner.Library.Common.DmsElement dmsElement): base(dmsElement)
                {
                }

                /// <summary>
                /// Gets or sets the alarm template definition of the element.
                /// This can either be an alarm template or an alarm template group.
                /// </summary>
                internal Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate AlarmTemplate
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        if (!alarmTemplateLoader.IsValueCreated)
                        {
                            alarmTemplate = alarmTemplateLoader.Value;
                        }

                        return alarmTemplate;
                    }

                    set
                    {
                        DmsElement.LoadOnDemand();
                        string newAlarmTemplateName = value == null ? System.String.Empty : value.Name;
                        bool isCurrentEmpty = System.String.IsNullOrWhiteSpace(alarmTemplateName);
                        bool isNewEmpty = System.String.IsNullOrWhiteSpace(newAlarmTemplateName);
                        bool updateRequired = isCurrentEmpty ? !isNewEmpty : !alarmTemplateName.Equals(newAlarmTemplateName, System.StringComparison.OrdinalIgnoreCase);
                        if (updateRequired)
                        {
                            ChangedPropertyList.Add("AlarmTemplate");
                            alarmTemplateName = newAlarmTemplateName;
                            alarmTemplateLoader = new System.Lazy<Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate>(() => value);
                            alarmTemplate = alarmTemplateLoader.Value;
                        }
                    }
                }

                /// <summary>
                /// Gets or sets the element description.
                /// </summary>
                internal string Description
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return description;
                    }

                    set
                    {
                        DmsElement.LoadOnDemand();
                        string newValue = value == null ? System.String.Empty : value;
                        if (!description.Equals(newValue, System.StringComparison.Ordinal))
                        {
                            ChangedPropertyList.Add("Description");
                            description = newValue;
                        }
                    }
                }

                /// <summary>
                /// Gets or sets the system-wide element ID.
                /// </summary>
                internal Skyline.DataMiner.Library.Common.DmsElementId DmsElementId
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets the DataMiner agent that hosts the element.
                /// </summary>
                internal Skyline.DataMiner.Library.Common.Dma Host
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return host;
                    }
                }

                /// <summary>
                /// Gets or sets the state of the element.
                /// </summary>
                internal Skyline.DataMiner.Library.Common.ElementState State
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return state;
                    }

                    set
                    {
                        DmsElement.LoadOnDemand();
                        state = value;
                    }
                }

                /// <summary>
                /// Gets or sets the trend template assigned to this element.
                /// </summary>
                internal Skyline.DataMiner.Library.Common.Templates.IDmsTrendTemplate TrendTemplate
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return trendTemplate;
                    }

                    set
                    {
                        DmsElement.LoadOnDemand();
                        bool updateRequired = false;
                        if (trendTemplate == null)
                        {
                            if (value != null)
                            {
                                updateRequired = true;
                            }
                        }
                        else
                        {
                            if (value == null || !trendTemplate.Equals(value))
                            {
                                updateRequired = true;
                            }
                        }

                        if (updateRequired)
                        {
                            ChangedPropertyList.Add("TrendTemplate");
                            trendTemplate = value;
                        }
                    }
                }

                /// <summary>
                /// Gets or sets the name of the element.
                /// </summary>
                /// <exception cref = "NotSupportedException">A set operation is not supported on a DVE child or a derived element.</exception>
                internal string Name
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return name;
                    }

                    set
                    {
                        DmsElement.LoadOnDemand();
                        if (DmsElement.DveSettings.IsChild || DmsElement.RedundancySettings.IsDerived)
                        {
                            throw new System.NotSupportedException("Setting the name of a DVE child or a derived element is not supported.");
                        }

                        if (!name.Equals(value, System.StringComparison.Ordinal))
                        {
                            ChangedPropertyList.Add("Name");
                            name = value.Trim();
                        }
                    }
                }

                /// <summary>
                /// Gets or sets the instance of the protocol.
                /// </summary>
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation is empty.</exception>
                internal Skyline.DataMiner.Library.Common.DmsProtocol Protocol
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return protocol;
                    }

                    set
                    {
                        if (value == null)
                        {
                            throw new System.ArgumentNullException("value");
                        }

                        DmsElement.LoadOnDemand();
                        ChangedPropertyList.Add("Protocol");
                        protocol = value;
                    }
                }

                /// <summary>
                /// Returns the string representation of the object.
                /// </summary>
                /// <returns>String representation of the object.</returns>
                public override string ToString()
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendLine("GENERAL SETTINGS:");
                    sb.AppendLine("==========================");
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Name: {0}{1}", DmsElement.Name, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Description: {0}{1}", Description, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Protocol name: {0}{1}", Protocol.Name, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Protocol version: {0}{1}", Protocol.Version, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "DMA ID: {0}{1}", DmsElementId.AgentId, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Element ID: {0}{1}", DmsElementId.ElementId, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Hosting DMA ID: {0}{1}", Host.Id, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Alarm template: {0}{1}", AlarmTemplate, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Trend template: {0}{1}", TrendTemplate, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "State: {0}{1}", State, System.Environment.NewLine);
                    return sb.ToString();
                }

                /// <summary>
                /// Sets the updated fields when an update is performed.
                /// </summary>
                /// <param name = "message">The message to be updated.</param>
                internal override void FillUpdate(Skyline.DataMiner.Net.Messages.AddElementMessage message)
                {
                    foreach (string property in ChangedPropertyList)
                    {
                        switch (property)
                        {
                            case "AlarmTemplate":
                                message.AlarmTemplate = AlarmTemplate == null ? System.String.Empty : AlarmTemplate.Name;
                                break;
                            case "Description":
                                message.Description = description;
                                break;
                            case "TrendTemplate":
                                message.TrendTemplate = trendTemplate == null ? System.String.Empty : trendTemplate.Name;
                                break;
                            case "Name":
                                message.ElementName = name;
                                break;
                            default:
                                throw new System.InvalidOperationException("Unexpected value: " + property);
                        }
                    }
                }

                /// <summary>
                /// Loads the information to the component.
                /// </summary>
                /// <param name = "elementInfo">The element information.</param>
                internal override void Load(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
                {
                    DmsElementId = new Skyline.DataMiner.Library.Common.DmsElementId(elementInfo.DataMinerID, elementInfo.ElementID);
                    description = elementInfo.Description ?? System.String.Empty;
                    protocol = new Skyline.DataMiner.Library.Common.DmsProtocol(DmsElement.Dms, elementInfo.Protocol, elementInfo.ProtocolVersion);
                    alarmTemplateName = elementInfo.ProtocolTemplate;
                    trendTemplate = System.String.IsNullOrWhiteSpace(elementInfo.Trending) ? null : new Skyline.DataMiner.Library.Common.Templates.DmsTrendTemplate(DmsElement.Dms, elementInfo.Trending, protocol);
                    state = (Skyline.DataMiner.Library.Common.ElementState)elementInfo.State;
                    name = elementInfo.Name ?? System.String.Empty;
                    host = new Skyline.DataMiner.Library.Common.Dma(DmsElement.Dms, elementInfo.HostingAgentID);
                    alarmTemplateLoader = new System.Lazy<Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate>(() => LoadAlarmTemplateDefinition());
                }

                /// <summary>
                /// Loads the alarm template definition.
                /// This method checks whether there is a group or a template assigned to the element.
                /// </summary>
                private Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate LoadAlarmTemplateDefinition()
                {
                    Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate innerAlarmTemplate = alarmTemplate; // do not use public property here as it will cause cyclic call
                    if (innerAlarmTemplate == null && !System.String.IsNullOrWhiteSpace(alarmTemplateName))
                    {
                        Skyline.DataMiner.Net.Messages.GetAlarmTemplateMessage message = new Skyline.DataMiner.Net.Messages.GetAlarmTemplateMessage{AsOneObject = true, Protocol = protocol.Name, Version = protocol.Version, Template = alarmTemplateName};
                        Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage response = (Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage)DmsElement.Dms.Communication.SendSingleResponseMessage(message);
                        if (response != null)
                        {
                            switch (response.Type)
                            {
                                case Skyline.DataMiner.Net.Messages.AlarmTemplateType.Template:
                                    innerAlarmTemplate = new Skyline.DataMiner.Library.Common.Templates.DmsStandaloneAlarmTemplate(DmsElement.Dms, response);
                                    break;
                                case Skyline.DataMiner.Net.Messages.AlarmTemplateType.Group:
                                    innerAlarmTemplate = new Skyline.DataMiner.Library.Common.Templates.DmsAlarmTemplateGroup(DmsElement.Dms, response);
                                    break;
                                default:
                                    throw new System.InvalidOperationException("Unexpected value: " + response.Type);
                            }
                        }
                    }

                    return innerAlarmTemplate;
                }
            }

            /// <summary>
            /// DataMiner element advanced settings interface.
            /// </summary>
            public interface IAdvancedSettings
            {
                /// <summary>
                /// Gets or sets a value indicating whether the element is hidden.
                /// </summary>
                /// <value><c>true</c> if the element is hidden; otherwise, <c>false</c>.</value>
                /// <exception cref = "NotSupportedException">A set operation is not supported on a derived element.</exception>
                bool IsHidden
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets a value indicating whether the element is read-only.
                /// </summary>
                /// <value><c>true</c> if the element is read-only; otherwise, <c>false</c>.</value>
                /// <exception cref = "NotSupportedException">A set operation is not supported on a DVE or derived element.</exception>
                bool IsReadOnly
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets a value indicating whether the element is running a simulation.
                /// </summary>
                /// <value><c>true</c> if the element is running a simulation; otherwise, <c>false</c>.</value>
                bool IsSimulation
                {
                    get;
                }

                /// <summary>
                /// Gets or sets the element timeout value.
                /// </summary>
                /// <value>The timeout value.</value>
                /// <exception cref = "NotSupportedException">A set operation is not supported on a DVE or derived element.</exception>
                /// <exception cref = "ArgumentOutOfRangeException">The value specified for a set operation is not in the range of [0,120] s.</exception>
                /// <remarks>Fractional seconds are ignored. For example, setting the timeout to a value of 3.5s results in setting it to 3s.</remarks>
                System.TimeSpan Timeout
                {
                    get;
                    set;
                }
            }

            /// <summary>
            /// DataMiner element DVE settings interface.
            /// </summary>
            public interface IDveSettings
            {
                /// <summary>
                /// Gets a value indicating whether this element is a DVE child.
                /// </summary>
                /// <value><c>true</c> if this element is a DVE child element; otherwise, <c>false</c>.</value>
                bool IsChild
                {
                    get;
                }

                /// <summary>
                /// Gets or sets a value indicating whether DVE creation is enabled for this element.
                /// </summary>
                /// <value><c>true</c> if the element DVE generation is enabled; otherwise, <c>false</c>.</value>
                /// <exception cref = "NotSupportedException">The element is not a DVE parent element.</exception>
                bool IsDveCreationEnabled
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets a value indicating whether this element is a DVE parent.
                /// </summary>
                /// <value><c>true</c> if the element is a DVE parent element; otherwise, <c>false</c>.</value>
                bool IsParent
                {
                    get;
                }

                /// <summary>
                /// Gets the parent element.
                /// </summary>
                /// <value>The parent element.</value>
                Skyline.DataMiner.Library.Common.IDmsElement Parent
                {
                    get;
                }
            }

            /// <summary>
            /// DataMiner element failover settings interface.
            /// </summary>
            internal interface IFailoverSettings
            {
                /// <summary>
                /// Gets or sets a value indicating whether to force agent.
                /// Local IP address of the agent which will be running the element.
                /// </summary>
                /// <value>Value indicating whether to force agent.</value>
                string ForceAgent
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets a value indicating whether the element is a failover element and is online on the backup agent instead of this agent.
                /// </summary>
                /// <value><c>true</c> if the element is a failover element and is online on the backup agent instead of this agent; otherwise, <c>false</c>.</value>
                bool IsOnlineOnBackupAgent
                {
                    get;
                }

                /// <summary>
                /// Gets or sets a value indicating whether the element is a failover element that needs to keep running on the same DataMiner agent event after switching.
                /// </summary>
                /// <value><c>true</c> if the element is a failover element that needs to keep running on the same DataMiner agent event after switching; otherwise, <c>false</c>.</value>
                bool KeepOnline
                {
                    get;
                    set;
                }
            }

            /// <summary>
            /// DataMiner element redundancy settings interface.
            /// </summary>
            public interface IRedundancySettings
            {
                /// <summary>
                /// Gets a value indicating whether the element is derived from another element.
                /// </summary>
                /// <value><c>true</c> if the element is derived from another element; otherwise, <c>false</c>.</value>
                bool IsDerived
                {
                    get;
                }
            }

            /// <summary>
            /// DataMiner element replication settings interface.
            /// </summary>
            public interface IReplicationSettings
            {
                /// <summary>
                /// Gets the domain the user belongs to.
                /// </summary>
                /// <value>The domain the user belongs to.</value>
                string Domain
                {
                    get;
                }

                ///// <summary>
                ///// Gets a value indicating whether it is allowed to perform the logic of a protocol on the replicated element instead of only showing the data received on the original element.
                ///// By Default, some functionality is not allowed on replicated elements (get, set, QAs, triggers etc.).
                ///// </summary>
                ///// <value><c>true</c> if it is allowed to perform the logic of a protocol on the replicated element; otherwise, <c>false</c>.</value>
                //bool ConnectsToExternalProbe { get; }
                /// <summary>
                /// Gets the IP address of the DataMiner Agent from which this element is replicated.
                /// </summary>
                /// <value>The IP address of the DataMiner Agent from which this element is replicated.</value>
                string IPAddressSourceAgent
                {
                    get;
                }

                /// <summary>
                /// Gets a value indicating whether this element is replicated.
                /// </summary>
                /// <value><c>true</c> if this element is replicated; otherwise, <c>false</c>.</value>
                bool IsReplicated
                {
                    get;
                }

                ///// <summary>
                ///// Gets the additional options defined when replicating the element.
                ///// </summary>
                ///// <value>The additional options defined when replicating the element.</value>
                //string Options { get; }
                /// <summary>
                /// Gets the password corresponding with the user name to log in on the source DataMiner Agent.
                /// </summary>
                /// <value>The password corresponding with the user name.</value>
                string Password
                {
                    get;
                }

                /// <summary>
                /// Gets the system-wide element ID of the source element.
                /// </summary>
                /// <value>The system-wide element ID of the source element.</value>
                Skyline.DataMiner.Library.Common.DmsElementId SourceDmsElementId
                {
                    get;
                }

                /// <summary>
                /// Gets the user name used to log in on the source DataMiner Agent.
                /// </summary>
                /// <value>The user name used to log in on the source DataMiner Agent.</value>
                string UserName
                {
                    get;
                }
            }

            /// <summary>
            /// Represents the redundancy settings for a element.
            /// </summary>
            internal class RedundancySettings : Skyline.DataMiner.Library.Common.ElementSettings, Skyline.DataMiner.Library.Common.IRedundancySettings
            {
                /// <summary>
                /// Value indicating whether or not this element is derived from another element.
                /// </summary>
                private bool isDerived;
                /// <summary>
                /// Initializes a new instance of the <see cref = "RedundancySettings"/> class.
                /// </summary>
                /// <param name = "dmsElement">The reference to the <see cref = "DmsElement"/> instance this object is part of.</param>
                internal RedundancySettings(Skyline.DataMiner.Library.Common.DmsElement dmsElement): base(dmsElement)
                {
                }

                /// <summary>
                /// Gets or sets a value indicating whether the element is derived from another element.
                /// </summary>
                /// <value><c>true</c> if the element is derived from another element; otherwise, <c>false</c>.</value>
                public bool IsDerived
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return isDerived;
                    }

                    internal set
                    {
                        isDerived = value;
                    }
                }

                /// <summary>
                /// Returns the string representation of the object.
                /// </summary>
                /// <returns>String representation of the object.</returns>
                public override string ToString()
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendLine("REDUNDANCY SETTINGS:");
                    sb.AppendLine("==========================");
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Derived: {0}{1}", isDerived, System.Environment.NewLine);
                    return sb.ToString();
                }

                /// <summary>
                /// Loads the information to the component.
                /// </summary>
                /// <param name = "elementInfo">The element information.</param>
                internal override void Load(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
                {
                    isDerived = elementInfo.IsDerivedElement;
                }

                /// <summary>
                /// Fills in the needed properties in the AddElement message.
                /// </summary>
                /// <param name = "message">The AddElement message which will be sent to SLNet.</param>
                internal override void FillUpdate(Skyline.DataMiner.Net.Messages.AddElementMessage message)
                {
                }
            }

            /// <summary>
            /// Represents the replication information of an element.
            /// </summary>
            internal class ReplicationSettings : Skyline.DataMiner.Library.Common.ElementSettings, Skyline.DataMiner.Library.Common.IReplicationSettings
            {
                /// <summary>
                /// The domain the specified user belongs to.
                /// </summary>
                private string domain = System.String.Empty;
                /// <summary>
                /// External DMP engine.
                /// </summary>
                private bool connectsToExternalDmp;
                /// <summary>
                /// IP address of the source DataMiner Agent.
                /// </summary>
                private string ipAddressSourceDma = System.String.Empty;
                /// <summary>
                /// Value indicating whether this element is replicated.
                /// </summary>
                private bool isReplicated;
                /// <summary>
                /// The options string.
                /// </summary>
                private string options = System.String.Empty;
                /// <summary>
                /// The password.
                /// </summary>
                private string password = System.String.Empty;
                /// <summary>
                /// The ID of the source element.
                /// </summary>
                private Skyline.DataMiner.Library.Common.DmsElementId sourceDmsElementId = new Skyline.DataMiner.Library.Common.DmsElementId(-1, -1);
                /// <summary>
                /// The user name.
                /// </summary>
                private string userName = System.String.Empty;
                /// <summary>
                /// Initializes a new instance of the <see cref = "ReplicationSettings"/> class.
                /// </summary>
                /// <param name = "dmsElement">The reference to the DmsElement where this object will be used in.</param>
                internal ReplicationSettings(Skyline.DataMiner.Library.Common.DmsElement dmsElement): base(dmsElement)
                {
                }

                /// <summary>
                /// Gets the domain the user belongs to.
                /// </summary>
                /// <value>The domain the user belongs to.</value>
                public string Domain
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return domain;
                    }

                    internal set
                    {
                        domain = value;
                    }
                }

                /// <summary>
                /// Gets a value indicating whether it is allowed to perform logic of a protocol on the replicated element instead of only showing the data received on the original element.
                /// By Default, some functionality is not allowed on replicated elements (get, set, QAs, triggers etc.).
                /// </summary>
                /// <value><c>true</c> if it is allowed to perform the logic of a protocol on the replicated element; otherwise, <c>false</c>.</value>
                public bool ConnectsToExternalProbe
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return connectsToExternalDmp;
                    }
                }

                /// <summary>
                /// Gets the IP address of the DataMiner Agent from which this element is replicated.
                /// </summary>
                /// <value>The IP address of the DataMiner Agent from which this element is replicated</value>
                public string IPAddressSourceAgent
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return ipAddressSourceDma;
                    }

                    internal set
                    {
                        ipAddressSourceDma = value;
                    }
                }

                /// <summary>
                /// Gets a value indicating whether this element is replicated.
                /// </summary>
                /// <value><c>true</c> if this element is replicated; otherwise, <c>false</c>.</value>
                public bool IsReplicated
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return isReplicated;
                    }

                    internal set
                    {
                        isReplicated = value;
                    }
                }

                /// <summary>
                /// Gets the additional options defined when replicating the element.
                /// </summary>
                /// <value>The additional options defined when replicating the element.</value>
                public string Options
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return options;
                    }

                    internal set
                    {
                        options = value;
                    }
                }

                /// <summary>
                /// Gets the password corresponding with the user name to log in on the source DataMiner Agent.
                /// </summary>
                /// <value>The password corresponding with the user name.</value>
                public string Password
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return password;
                    }

                    internal set
                    {
                        password = value;
                    }
                }

                /// <summary>
                /// Gets the system-wide element ID of the source element.
                /// </summary>
                /// <value>The system-wide element ID of the source element.</value>
                public Skyline.DataMiner.Library.Common.DmsElementId SourceDmsElementId
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return sourceDmsElementId;
                    }

                    internal set
                    {
                        sourceDmsElementId = value;
                    }
                }

                /// <summary>
                /// Gets the user name used to log in on the source DataMiner Agent.
                /// </summary>
                /// <value>The user name used to log in on the source DataMiner Agent.</value>
                public string UserName
                {
                    get
                    {
                        DmsElement.LoadOnDemand();
                        return userName;
                    }

                    internal set
                    {
                        userName = value;
                    }
                }

                /// <summary>
                /// Returns the string representation of the object.
                /// </summary>
                /// <returns>String representation of the object.</returns>
                public override string ToString()
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendLine("REPLICATION SETTINGS:");
                    sb.AppendLine("==========================");
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Replicated: {0}{1}", isReplicated, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Source DMA ID: {0}{1}", sourceDmsElementId.AgentId, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Source element ID: {0}{1}", sourceDmsElementId.ElementId, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "IP address source DMA: {0}{1}", ipAddressSourceDma, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Domain: {0}{1}", domain, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "User name: {0}{1}", userName, System.Environment.NewLine);
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Password: {0}{1}", password, System.Environment.NewLine);
                    //sb.AppendFormat(CultureInfo.InvariantCulture, "Options: {0}{1}", options, Environment.NewLine);
                    //sb.AppendFormat(CultureInfo.InvariantCulture, "Replication DMP engine: {0}{1}", connectsToExternalDmp, Environment.NewLine);
                    return sb.ToString();
                }

                /// <summary>
                /// Fills in the needed properties in the AddElement message.
                /// </summary>
                /// <param name = "message">The AddElement message which will be sent to SLNet.</param>
                internal override void FillUpdate(Skyline.DataMiner.Net.Messages.AddElementMessage message)
                {
                }

                /// <summary>
                /// Loads the information to the component.
                /// </summary>
                /// <param name = "elementInfo">The element information.</param>
                internal override void Load(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
                {
                    isReplicated = elementInfo.ReplicationActive;
                    if (!isReplicated)
                    {
                        options = System.String.Empty;
                        ipAddressSourceDma = System.String.Empty;
                        password = System.String.Empty;
                        domain = System.String.Empty;
                        sourceDmsElementId = new Skyline.DataMiner.Library.Common.DmsElementId(-1, -1);
                        userName = System.String.Empty;
                        connectsToExternalDmp = false;
                    }

                    options = elementInfo.ReplicationOptions ?? System.String.Empty;
                    ipAddressSourceDma = elementInfo.ReplicationDmaIP ?? System.String.Empty;
                    password = elementInfo.ReplicationPwd ?? System.String.Empty;
                    domain = elementInfo.ReplicationDomain ?? System.String.Empty;
                    bool isEmpty = System.String.IsNullOrWhiteSpace(elementInfo.ReplicationRemoteElement) || elementInfo.ReplicationRemoteElement.Equals("/", System.StringComparison.Ordinal);
                    if (isEmpty)
                    {
                        sourceDmsElementId = new Skyline.DataMiner.Library.Common.DmsElementId(-1, -1);
                    }
                    else
                    {
                        try
                        {
                            sourceDmsElementId = new Skyline.DataMiner.Library.Common.DmsElementId(elementInfo.ReplicationRemoteElement);
                        }
                        catch (System.Exception ex)
                        {
                            string logMessage = "Failed parsing replication element info for element " + System.Convert.ToString(elementInfo.Name) + " (" + System.Convert.ToString(elementInfo.DataMinerID) + "/" + System.Convert.ToString(elementInfo.ElementID) + "). Replication remote element is: " + System.Convert.ToString(elementInfo.ReplicationRemoteElement) + System.Environment.NewLine + ex;
                            Skyline.DataMiner.Library.Common.Logger.Log(logMessage);
                            sourceDmsElementId = new Skyline.DataMiner.Library.Common.DmsElementId(-1, -1);
                        }
                    }

                    userName = elementInfo.ReplicationUser ?? System.String.Empty;
                    connectsToExternalDmp = elementInfo.ReplicationIsExternalDMP;
                }
            }

            /// <summary>
            /// Represents a base class for all of the components in a DmsElement object.
            /// </summary>
            internal abstract class ElementSettings
            {
                /// <summary>
                /// The list of changed properties.
                /// </summary>
                private readonly System.Collections.Generic.List<System.String> changedPropertyList = new System.Collections.Generic.List<System.String>();
                /// <summary>
                /// Instance of the DmsElement class where these classes will be used for.
                /// </summary>
                private readonly Skyline.DataMiner.Library.Common.DmsElement dmsElement;
                /// <summary>
                /// Initializes a new instance of the <see cref = "ElementSettings"/> class.
                /// </summary>
                /// <param name = "dmsElement">The reference to the <see cref = "DmsElement"/> instance this object is part of.</param>
                protected ElementSettings(Skyline.DataMiner.Library.Common.DmsElement dmsElement)
                {
                    this.dmsElement = dmsElement;
                }

                /// <summary>
                /// Gets the element this object belongs to.
                /// </summary>
                internal Skyline.DataMiner.Library.Common.DmsElement DmsElement
                {
                    get
                    {
                        return dmsElement;
                    }
                }

                /// <summary>
                /// Gets a value indicating whether one or more properties have been updated.
                /// </summary>
                internal bool Updated
                {
                    get
                    {
                        return System.Linq.Enumerable.Any(changedPropertyList);
                    }
                }

                /// <summary>
                /// Gets the list of updated properties.
                /// </summary>
                protected internal System.Collections.Generic.List<System.String> ChangedPropertyList
                {
                    get
                    {
                        return changedPropertyList;
                    }
                }

                /// <summary>
                /// Fills in the needed properties in the AddElement message.
                /// </summary>
                /// <param name = "message">The AddElement message which will be sent to SLNet.</param>
                internal abstract void FillUpdate(Skyline.DataMiner.Net.Messages.AddElementMessage message);
                /// <summary>
                /// Based on the array provided from the DmsNotify call, parse the data to the correct fields.
                /// </summary>
                /// <param name = "elementInfo">Object containing all the required information. Retrieved by DmsClass.</param>
                internal abstract void Load(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo);
                /// <summary>
                /// Clears the entries update dictionary.
                /// </summary>
                protected internal void ClearUpdates()
                {
                    changedPropertyList.Clear();
                }
            }

            /// <summary>
            /// Represents a DataMiner protocol.
            /// </summary>
            internal class DmsProtocol : Skyline.DataMiner.Library.Common.DmsObject, Skyline.DataMiner.Library.Common.IDmsProtocol
            {
                /// <summary>
                /// The constant value 'Production'.
                /// </summary>
                private const string Production = "Production";
                /// <summary>
                /// The protocol name.
                /// </summary>
                private string name;
                /// <summary>
                /// The protocol version.
                /// </summary>
                private string version;
                /// <summary>
                /// The type of the protocol.
                /// </summary>
                private Skyline.DataMiner.Library.Common.ProtocolType type;
                /// <summary>
                /// The protocol referenced version.
                /// </summary>
                private string referencedVersion;
                /// <summary>
                /// Whether the version is 'Production'.
                /// </summary>
                private bool isProduction;
                /// <summary>
                /// The connection info of the protocol.
                /// </summary>
                private System.Collections.Generic.IList<Skyline.DataMiner.Library.Common.IDmsConnectionInfo> connectionInfo = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.IDmsConnectionInfo>();
                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsProtocol"/> class.
                /// </summary>
                /// <param name = "dms">The DataMiner System.</param>
                /// <param name = "name">The protocol name.</param>
                /// <param name = "version">The protocol version.</param>
                /// <param name = "type">The type of the protocol.</param>
                /// <param name = "referencedVersion">The protocol referenced version.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentNullException"><paramref name = "name"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentNullException"><paramref name = "version"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "version"/> is the empty string ("") or white space.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "version"/> is not 'Production' and <paramref name = "referencedVersion"/> is not the empty string ("") or white space.</exception>
                internal DmsProtocol(Skyline.DataMiner.Library.Common.IDms dms, string name, string version, Skyline.DataMiner.Library.Common.ProtocolType type = Skyline.DataMiner.Library.Common.ProtocolType.Undefined, string referencedVersion = ""): base(dms)
                {
                    if (name == null)
                    {
                        throw new System.ArgumentNullException("name");
                    }

                    if (version == null)
                    {
                        throw new System.ArgumentNullException("version");
                    }

                    if (System.String.IsNullOrWhiteSpace(name))
                    {
                        throw new System.ArgumentException("The name of the protocol is the empty string (\"\") or white space.", "name");
                    }

                    if (System.String.IsNullOrWhiteSpace(version))
                    {
                        throw new System.ArgumentException("The version of the protocol is the empty string (\"\") or white space.", "version");
                    }

                    this.name = name;
                    this.version = version;
                    this.type = type;
                    this.isProduction = CheckIsProduction(this.version);
                    if (!this.isProduction && !System.String.IsNullOrWhiteSpace(referencedVersion))
                    {
                        throw new System.ArgumentException("The version of the protocol is not referenced version of the protocol is not the empty string (\"\") or white space.", "referencedVersion");
                    }

                    this.referencedVersion = referencedVersion;
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsProtocol"/> class.
                /// </summary>
                /// <param name = "dms">The DataMiner system.</param>
                /// <param name = "infoMessage">The information message received from SLNet.</param>
                /// <param name = "requestedProduction">The version requested to SLNet.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "infoMessage"/> is <see langword = "null"/>.</exception>
                internal DmsProtocol(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Net.Messages.GetProtocolInfoResponseMessage infoMessage, bool requestedProduction): base(dms)
                {
                    if (infoMessage == null)
                    {
                        throw new System.ArgumentNullException("infoMessage");
                    }

                    this.isProduction = requestedProduction;
                    Parse(infoMessage);
                }

                /// <summary>
                /// Gets the connection information.
                /// </summary>
                /// <value>The connection information.</value>
                public System.Collections.Generic.IList<Skyline.DataMiner.Library.Common.IDmsConnectionInfo> ConnectionInfo
                {
                    get
                    {
                        LoadOnDemand();
                        return new System.Collections.ObjectModel.ReadOnlyCollection<Skyline.DataMiner.Library.Common.IDmsConnectionInfo>(connectionInfo);
                    }
                }

                /// <summary>
                /// Gets the protocol name.
                /// </summary>
                /// <value>The protocol name.</value>
                public string Name
                {
                    get
                    {
                        return name;
                    }
                }

                /// <summary>
                /// Gets the protocol version.
                /// </summary>
                /// <value>The protocol version.</value>
                public string Version
                {
                    get
                    {
                        return version;
                    }
                }

                public Skyline.DataMiner.Library.Common.ProtocolType Type
                {
                    get
                    {
                        return type;
                    }
                }

                /// <summary>
                /// Gets the protocol referenced version.
                /// </summary>
                /// <value>The protocol referenced version.</value>
                public string ReferencedVersion
                {
                    get
                    {
                        if (System.String.IsNullOrEmpty(referencedVersion))
                        {
                            LoadOnDemand();
                        }

                        return referencedVersion == System.String.Empty ? null : referencedVersion;
                    }
                }

                /// <summary>
                /// Gets a value indicating whether the version is 'Production'.
                /// </summary>
                /// <value>Whether the version is 'Production'.</value>
                public bool IsProduction
                {
                    get
                    {
                        return isProduction;
                    }
                }

                /// <summary>
                /// Determines whether this protocol exists in the DataMiner System.
                /// </summary>
                /// <returns><c>true</c> if this protocol exists in the DataMiner System; otherwise, <c>false</c>.</returns>
                public override bool Exists()
                {
                    return Dms.ProtocolExists(name, version);
                }

                /// <summary>
                /// Gets the alarm template with the specified name defined for this protocol.
                /// </summary>
                /// <param name = "templateName">The name of the alarm template.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "templateName"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "templateName"/> is the empty string ("") or white space.</exception>
                /// <exception cref = "AlarmTemplateNotFoundException">No alarm template with the specified name was found.</exception>
                /// <returns>The alarm template with the specified name defined for this protocol.</returns>
                public Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate GetAlarmTemplate(string templateName)
                {
                    Skyline.DataMiner.Net.Messages.GetAlarmTemplateMessage message = new Skyline.DataMiner.Net.Messages.GetAlarmTemplateMessage{AsOneObject = true, Protocol = this.Name, Version = this.Version, Template = templateName};
                    Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage alarmTemplateEventMessage = (Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage)dms.Communication.SendSingleResponseMessage(message);
                    if (alarmTemplateEventMessage == null)
                    {
                        throw new Skyline.DataMiner.Library.Common.AlarmTemplateNotFoundException(templateName, this);
                    }

                    if (alarmTemplateEventMessage.Type == Skyline.DataMiner.Net.Messages.AlarmTemplateType.Template)
                    {
                        return new Skyline.DataMiner.Library.Common.Templates.DmsStandaloneAlarmTemplate(dms, alarmTemplateEventMessage);
                    }
                    else if (alarmTemplateEventMessage.Type == Skyline.DataMiner.Net.Messages.AlarmTemplateType.Group)
                    {
                        return new Skyline.DataMiner.Library.Common.Templates.DmsAlarmTemplateGroup(dms, alarmTemplateEventMessage);
                    }
                    else
                    {
                        throw new System.NotSupportedException("Support for " + alarmTemplateEventMessage.Type + " has not yet been implemented.");
                    }
                }

                /// <summary>
                /// Determines whether a standalone alarm template with the specified name exists for this protocol.
                /// </summary>
                /// <param name = "templateName">Name of the alarm template.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "templateName"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "templateName"/> is the empty string ("") or white space.</exception>
                /// <returns><c>true</c> if a standalone alarm template with the specified name exists; otherwise, <c>false</c>.</returns>
                public bool StandaloneAlarmTemplateExists(string templateName)
                {
                    bool exists = false;
                    Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage template = GetAlarmTemplateSLNet(templateName);
                    if (template != null && template.Type == Skyline.DataMiner.Net.Messages.AlarmTemplateType.Template)
                    {
                        exists = true;
                    }

                    return exists;
                }

                /// <summary>
                /// Returns a string that represents the current object.
                /// </summary>
                /// <returns>A string that represents the current object.</returns>
                public override string ToString()
                {
                    return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Protocol name: {0}, version: {1}", Name, Version);
                }

                /// <summary>
                /// Validate if <paramref name = "version"/> is 'Production'.
                /// </summary>
                /// <param name = "version">The version.</param>
                /// <returns>Whether <paramref name = "version"/> is 'Production'.</returns>
                internal static bool CheckIsProduction(string version)
                {
                    return System.String.Equals(version, Production, System.StringComparison.OrdinalIgnoreCase);
                }

                /// <summary>
                /// Loads the object.
                /// </summary>
                /// <exception cref = "ProtocolNotFoundException">No protocol with the specified name and version exists in the DataMiner system.</exception>
                internal override void Load()
                {
                    isProduction = CheckIsProduction(version);
                    Skyline.DataMiner.Net.Messages.GetProtocolMessage getProtocolMessage = new Skyline.DataMiner.Net.Messages.GetProtocolMessage{Protocol = name, Version = version};
                    Skyline.DataMiner.Net.Messages.GetProtocolInfoResponseMessage protocolInfo = (Skyline.DataMiner.Net.Messages.GetProtocolInfoResponseMessage)Communication.SendSingleResponseMessage(getProtocolMessage);
                    if (protocolInfo != null)
                    {
                        Parse(protocolInfo);
                    }
                    else
                    {
                        throw new Skyline.DataMiner.Library.Common.ProtocolNotFoundException(name, version);
                    }
                }

                /// <summary>
                /// Parses the <see cref = "GetProtocolInfoResponseMessage"/> message.
                /// </summary>
                /// <param name = "protocolInfo">The protocol information.</param>
                private void Parse(Skyline.DataMiner.Net.Messages.GetProtocolInfoResponseMessage protocolInfo)
                {
                    IsLoaded = true;
                    name = protocolInfo.Name;
                    type = (Skyline.DataMiner.Library.Common.ProtocolType)protocolInfo.ProtocolType;
                    if (isProduction)
                    {
                        version = Production;
                        referencedVersion = protocolInfo.Version;
                    }
                    else
                    {
                        version = protocolInfo.Version;
                        referencedVersion = System.String.Empty;
                    }

                    ParseConnectionInfo(protocolInfo);
                }

                /// <summary>
                /// Parses the <see cref = "GetProtocolInfoResponseMessage"/> message.
                /// </summary>
                /// <param name = "protocolInfo">The protocol information.</param>
                private void ParseConnectionInfo(Skyline.DataMiner.Net.Messages.GetProtocolInfoResponseMessage protocolInfo)
                {
                    System.Collections.Generic.List<Skyline.DataMiner.Library.Common.DmsConnectionInfo> info = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.DmsConnectionInfo>();
                    info.Add(new Skyline.DataMiner.Library.Common.DmsConnectionInfo(System.String.Empty, Skyline.DataMiner.Library.Common.EnumMapper.ConvertStringToConnectionType(protocolInfo.Type)));
                    if (protocolInfo.AdvancedTypes != null && protocolInfo.AdvancedTypes.Length > 0 && !System.String.IsNullOrWhiteSpace(protocolInfo.AdvancedTypes))
                    {
                        string[] split = protocolInfo.AdvancedTypes.Split(';');
                        foreach (string part in split)
                        {
                            if (part.Contains(":"))
                            {
                                string[] connectionSplit = part.Split(':');
                                Skyline.DataMiner.Library.Common.ConnectionType connectionType = Skyline.DataMiner.Library.Common.EnumMapper.ConvertStringToConnectionType(connectionSplit[0]);
                                string connectionName = connectionSplit[1];
                                info.Add(new Skyline.DataMiner.Library.Common.DmsConnectionInfo(connectionName, connectionType));
                            }
                            else
                            {
                                Skyline.DataMiner.Library.Common.ConnectionType connectionType = Skyline.DataMiner.Library.Common.EnumMapper.ConvertStringToConnectionType(part);
                                string connectionName = System.String.Empty;
                                info.Add(new Skyline.DataMiner.Library.Common.DmsConnectionInfo(connectionName, connectionType));
                            }
                        }
                    }

                    connectionInfo = info.ToArray();
                }

                /// <summary>
                /// Gets the alarm template via SLNet.
                /// </summary>
                /// <param name = "templateName">The name of the alarm template.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "templateName"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "templateName"/> is the empty string ("") or white space.</exception>
                /// <returns>The AlarmTemplateEventMessage object.</returns>
                private Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage GetAlarmTemplateSLNet(string templateName)
                {
                    if (templateName == null)
                    {
                        throw new System.ArgumentNullException("templateName");
                    }

                    if (System.String.IsNullOrWhiteSpace(templateName))
                    {
                        throw new System.ArgumentException("Provided template name must not be the empty string (\"\") or white space", "templateName");
                    }

                    Skyline.DataMiner.Net.Messages.GetAlarmTemplateMessage message = new Skyline.DataMiner.Net.Messages.GetAlarmTemplateMessage{AsOneObject = true, Protocol = this.Name, Template = templateName, Version = this.Version};
                    return (Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage)Dms.Communication.SendSingleResponseMessage(message);
                }
            }

            /// <summary>
            /// DataMiner protocol interface.
            /// </summary>
            public interface IDmsProtocol : Skyline.DataMiner.Library.Common.IDmsObject
            {
                /// <summary>
                /// Gets the connection information.
                /// </summary>
                /// <value>The connection information.</value>
                System.Collections.Generic.IList<Skyline.DataMiner.Library.Common.IDmsConnectionInfo> ConnectionInfo
                {
                    get;
                }

                /// <summary>
                /// Gets the protocol name.
                /// </summary>
                /// <value>The protocol name.</value>
                string Name
                {
                    get;
                }

                /// <summary>
                /// Gets the protocol version.
                /// </summary>
                /// <value>The protocol version.</value>
                string Version
                {
                    get;
                }

                /// <summary>
                /// Gets the version this production protocol is based on.
                /// </summary>
                /// <value>The referenced version. This is only applicable for production protocols.</value>
                string ReferencedVersion
                {
                    get;
                }

                /// <summary>
                /// Gets the type of the protocol.
                /// </summary>
                /// <value>The type of protocol.</value>
                Skyline.DataMiner.Library.Common.ProtocolType Type
                {
                    get;
                }

                /// <summary>
                /// Gets the alarm template with the specified name defined for this protocol.
                /// </summary>
                /// <param name = "templateName">The name of the alarm template.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "templateName"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "templateName"/> is the empty string ("") or white space.</exception>
                /// <exception cref = "AlarmTemplateNotFoundException">No alarm template with the specified name was found.</exception>
                /// <returns>The alarm template with the specified name defined for this protocol.</returns>
                Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate GetAlarmTemplate(string templateName);
                /// <summary>
                /// Determines whether a standalone alarm template with the specified name exists for this protocol.
                /// </summary>
                /// <param name = "templateName">Name of the alarm template.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "templateName"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "templateName"/> is the empty string ("") or white space.</exception>
                /// <returns><c>true</c> if a standalone alarm template with the specified name exists; otherwise, <c>false</c>.</returns>
                bool StandaloneAlarmTemplateExists(string templateName);
            }

            /// <summary>
            /// Represents the DataMiner Scheduler component.
            /// </summary>
            internal class DmsScheduler : Skyline.DataMiner.Library.Common.IDmsScheduler
            {
                private readonly Skyline.DataMiner.Library.Common.IDma myDma;
                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsScheduler"/> class.
                /// </summary>
                /// <param name = "agent">The agent to which this scheduler component belongs to.</param>
                public DmsScheduler(Skyline.DataMiner.Library.Common.IDma agent)
                {
                    myDma = agent;
                }
            }

            /// <summary>
            /// Represents the DataMiner Scheduler component.
            /// </summary>
            public interface IDmsScheduler
            {
            }

            /// <summary>
            /// DataMiner service interface.
            /// </summary>
            public interface IDmsService : Skyline.DataMiner.Library.Common.IDmsObject, Skyline.DataMiner.Library.Common.IUpdateable
            {
                /// <summary>
                /// Gets the advanced settings of this service.
                /// </summary>
                /// <value>The advanced settings of this service.</value>
                Skyline.DataMiner.Library.Common.IAdvancedServiceSettings AdvancedSettings
                {
                    get;
                }

                /// <summary>
                /// Gets the parameter settings of this service.
                /// </summary>
                /// <value>The parameter settings of this service.</value>
                Skyline.DataMiner.Library.Common.IServiceParamsSettings ParameterSettings
                {
                    get;
                }

                /// <summary>
                /// Gets the replication settings.
                /// </summary>
                /// <value>The replication settings.</value>
                Skyline.DataMiner.Library.Common.IReplicationServiceSettings ReplicationSettings
                {
                    get;
                }

                /// <summary>
                /// Gets the DataMiner Agent ID.
                /// </summary>
                /// <value>The DataMiner Agent ID.</value>
                int AgentId
                {
                    get;
                }

                /// <summary>
                /// Gets or sets the service description.
                /// </summary>
                /// <value>The service description.</value>
                string Description
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets the system-wide service ID of the service.
                /// </summary>
                /// <value>The system-wide service ID of the service.</value>
                Skyline.DataMiner.Library.Common.DmsServiceId DmsServiceId
                {
                    get;
                }

                /// <summary>
                /// Gets the DataMiner Agent that hosts this service.
                /// </summary>
                /// <value>The DataMiner Agent that hosts this service.</value>
                Skyline.DataMiner.Library.Common.IDma Host
                {
                    get;
                }

                /// <summary>
                /// Gets the service ID.
                /// </summary>
                /// <value>The service ID.</value>
                int Id
                {
                    get;
                }

                /// <summary>
                /// Gets or sets the service name.
                /// </summary>
                /// <value>The service name.</value>
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation is empty or white space.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation exceeds 200 characters.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation contains a forbidden character.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation contains more than one '%' character.</exception>
                /// <remarks>
                /// <para>The following restrictions apply to service names:</para>
                /// <list type = "bullet">
                ///		<item><para>Names may not start or end with the following characters: '.' (dot), ' ' (space).</para></item>
                ///		<item><para>Names may not contain the following characters: '\', '/', ':', '*', '?', '"', '&lt;', '&gt;', '|', '°', ';'.</para></item>
                ///		<item><para>The following characters may not occur more than once within a name: '%' (percentage).</para></item>
                /// </list>
                /// </remarks>
                string Name
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets the properties of this service.
                /// </summary>
                /// <value>The service properties.</value>
                Skyline.DataMiner.Library.Common.IPropertyCollection<Skyline.DataMiner.Library.Common.Properties.IDmsServiceProperty, Skyline.DataMiner.Library.Common.Properties.IDmsServicePropertyDefinition> Properties
                {
                    get;
                }

                /// <summary>
                /// Gets the views the service is part of.
                /// </summary>
                /// <value>The views the service is part of.</value>
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation is an empty collection.</exception>
                System.Collections.Generic.ISet<Skyline.DataMiner.Library.Common.IDmsView> Views
                {
                    get;
                }
            }

            /// <summary>
            /// DataMiner service advanced settings interface.
            /// </summary>
            public interface IAdvancedServiceSettings
            {
                /// <summary>
                /// Gets a value indicating whether the service is a service template.
                /// </summary>
                /// <value><c>true</c> if the service is a service template; otherwise, <c>false</c>.</value>
                bool IsTemplate
                {
                    get;
                }

                /// <summary>
                /// Gets the service template from which the service is generated in case the service is generated through a service template.
                /// </summary>
                Skyline.DataMiner.Library.Common.DmsServiceId? ParentTemplate
                {
                    get;
                }

                /// <summary>
                /// Gets the element that is linked to this service in case of an enhanced service.
                /// </summary>
                Skyline.DataMiner.Library.Common.DmsElementId? ServiceElement
                {
                    get;
                }

                /// <summary>
                /// Gets or sets the alarm template of the service element (enhanced service).
                /// </summary>
                Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate ServiceElementAlarmTemplate
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the trend template of the service element (enhanced service).
                /// </summary>
                Skyline.DataMiner.Library.Common.Templates.IDmsTrendTemplate ServiceElementTrendTemplate
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the protocol applied to the service element (enhanced service).
                /// </summary>
                Skyline.DataMiner.Library.Common.IDmsProtocol ServiceElementProtocol
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets a value indicating whether timeouts are being included in the service.
                /// </summary>
                bool IgnoreTimeouts
                {
                    get;
                    set;
                }
            }

            /// <summary>
            /// DataMiner service replication settings interface.
            /// </summary>
            public interface IReplicationServiceSettings
            {
                /// <summary>
                /// Gets the domain the user belongs to.
                /// </summary>
                /// <value>The domain the user belongs to.</value>
                string Domain
                {
                    get;
                }

                /// <summary>
                /// Gets the IP address of the DataMiner Agent from which this service is replicated.
                /// </summary>
                /// <value>The IP address of the DataMiner Agent from which this service is replicated.</value>
                string IPAddressSourceAgent
                {
                    get;
                }

                /// <summary>
                /// Gets a value indicating whether this service is replicated.
                /// </summary>
                /// <value><c>true</c> if this service is replicated; otherwise, <c>false</c>.</value>
                bool IsReplicated
                {
                    get;
                }

                ///// <summary>
                ///// Gets the additional options defined when replicating the service.
                ///// </summary>
                ///// <value>The additional options defined when replicating the service.</value>
                //string Options { get; }
                /// <summary>
                /// Gets the password corresponding with the user name to log in on the source DataMiner Agent.
                /// </summary>
                /// <value>The password corresponding with the user name.</value>
                string Password
                {
                    get;
                }

                /// <summary>
                /// Gets the system-wide service ID of the source service.
                /// </summary>
                /// <value>The system-wide service ID of the source service.</value>
                Skyline.DataMiner.Library.Common.DmsServiceId? SourceDmsServiceId
                {
                    get;
                }

                /// <summary>
                /// Gets the user name used to log in on the source DataMiner Agent.
                /// </summary>
                /// <value>The user name used to log in on the source DataMiner Agent.</value>
                string UserName
                {
                    get;
                }
            }

            /// <summary>
            /// DataMiner service advanced settings interface.
            /// </summary>
            public interface IServiceParamsSettings
            {
                /// <summary>
                /// Gets the included parameters.
                /// </summary>
                Skyline.DataMiner.Library.Common.ServiceParamSettings[] IncludedParameters
                {
                    get;
                }
            }

            /// <summary>
            /// Represents a base class for all of the components in a DmsService object.
            /// </summary>
            public class ServiceParamFilterSettings
            {
                private readonly Skyline.DataMiner.Net.Messages.ServiceParamFilter serviceParamFilter;
                internal ServiceParamFilterSettings(Skyline.DataMiner.Net.Messages.ServiceParamFilter serviceParamFilter)
                {
                    this.serviceParamFilter = serviceParamFilter;
                }

                /// <summary>
                /// Gets the filter for the parameter.
                /// </summary>
                public string Filter
                {
                    get
                    {
                        return serviceParamFilter.Filter;
                    }
                }

                /// <summary>
                /// Gets the filter value for the parameter.
                /// </summary>
                public string FilterValue
                {
                    get
                    {
                        return serviceParamFilter.FilterValue;
                    }
                }

                /// <summary>
                /// Gets the filter type for the parameter.
                /// </summary>
                public Skyline.DataMiner.Library.Common.FilterType FilterType
                {
                    get
                    {
                        return (Skyline.DataMiner.Library.Common.FilterType)serviceParamFilter.FilterType;
                    }
                }

                /// <summary>
                /// Gets a value indicating whether the parameter is included.
                /// </summary>
                public bool IsIncluded
                {
                    get
                    {
                        return serviceParamFilter.IsIncluded;
                    }
                }

                /// <summary>
                /// Gets the matrix port for the parameter.
                /// </summary>
                public int MatrixPort
                {
                    get
                    {
                        return serviceParamFilter.MatrixPort;
                    }
                }

                /// <summary>
                /// Gets the ID of the parameter.
                /// </summary>
                public int ParameterID
                {
                    get
                    {
                        return serviceParamFilter.ParameterID;
                    }
                }

                internal static Skyline.DataMiner.Library.Common.ServiceParamFilterSettings[] GetParamFilters(Skyline.DataMiner.Net.Messages.ServiceInfoParams serviceParams)
                {
                    System.Collections.Generic.List<Skyline.DataMiner.Library.Common.ServiceParamFilterSettings> lParamFilters = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.ServiceParamFilterSettings>();
                    foreach (Skyline.DataMiner.Net.Messages.ServiceParamFilter paramFilter in serviceParams.ParameterFilters)
                    {
                        lParamFilters.Add(new Skyline.DataMiner.Library.Common.ServiceParamFilterSettings(paramFilter));
                    }

                    return lParamFilters.ToArray();
                }
            }

            /// <summary>
            /// Represents a base class for all of the components in a DmsService object.
            /// </summary>
            public class ServiceParamSettings
            {
                private readonly Skyline.DataMiner.Net.Messages.ServiceInfoParams includedElement;
                private Skyline.DataMiner.Library.Common.ServiceParamFilterSettings[] serviceParamFilters;
                private bool isLoaded;
                internal ServiceParamSettings(Skyline.DataMiner.Net.Messages.ServiceInfoParams infoParams)
                {
                    includedElement = infoParams;
                }

                /// <summary>
                /// Gets the Alias of the element.
                /// </summary>
                public string Alias
                {
                    get
                    {
                        return includedElement.Alias;
                    }
                }

                /// <summary>
                /// Gets the DataMiner ID of the element.
                /// </summary>
                public int DataMinerID
                {
                    get
                    {
                        return includedElement.DataMinerID;
                    }
                }

                /// <summary>
                /// Gets the element ID of the element.
                /// </summary>
                public int ElementID
                {
                    get
                    {
                        return includedElement.ElementID;
                    }
                }

                /// <summary>
                /// Gets the group ID to which the element belongs.
                /// </summary>
                public int GroupID
                {
                    get
                    {
                        return includedElement.GroupID;
                    }
                }

                /// <summary>
                /// Gets the included capped alarm level of the element.
                /// </summary>
                public Skyline.DataMiner.Library.Common.AlarmLevel IncludedCapped
                {
                    get
                    {
                        return (Skyline.DataMiner.Library.Common.AlarmLevel)System.Enum.Parse(typeof(Skyline.DataMiner.Library.Common.AlarmLevel), includedElement.IncludedCapped, true);
                    }
                }

                /// <summary>
                /// Gets the index of the element.
                /// </summary>
                public int Index
                {
                    get
                    {
                        return includedElement.Index;
                    }
                }

                /// <summary>
                /// Gets a value indicating whether the element is excluded.
                /// </summary>
                public bool IsExcluded
                {
                    get
                    {
                        return includedElement.IsExcluded;
                    }
                }

                /// <summary>
                /// Gets a value indicating whether the element is a service.
                /// </summary>
                public bool IsService
                {
                    get
                    {
                        return includedElement.IsService;
                    }
                }

                /// <summary>
                /// Gets the not used capped alarm level of the element.
                /// </summary>
                public Skyline.DataMiner.Library.Common.AlarmLevel NotUsedCapped
                {
                    get
                    {
                        return (Skyline.DataMiner.Library.Common.AlarmLevel)System.Enum.Parse(typeof(Skyline.DataMiner.Library.Common.AlarmLevel), includedElement.NotUsedCapped, true);
                    }
                }

                /// <summary>
                /// Gets the parameter filters for the included element.
                /// </summary>
                public Skyline.DataMiner.Library.Common.ServiceParamFilterSettings[] ParameterFilters
                {
                    get
                    {
                        if (!isLoaded)
                        {
                            LoadOnDemand();
                        }

                        return serviceParamFilters;
                    }
                }

                private void LoadOnDemand()
                {
                    isLoaded = true;
                    serviceParamFilters = Skyline.DataMiner.Library.Common.ServiceParamFilterSettings.GetParamFilters(includedElement);
                }
            }

            /// <summary>
            /// Represents the spectrum analyzer component of an element.
            /// </summary>
            internal class DmsSpectrumAnalyzer : Skyline.DataMiner.Library.Common.IDmsSpectrumAnalyzer
            {
                private readonly Skyline.DataMiner.Library.Common.IDmsElement element;
                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsSpectrumAnalyzer"/> class.
                /// </summary>
                /// <param name = "element">The element this spectrum analyzer is part of.</param>
                public DmsSpectrumAnalyzer(Skyline.DataMiner.Library.Common.IDmsElement element)
                {
                    this.element = element;
                    Monitors = new Skyline.DataMiner.Library.Common.DmsSpectrumAnalyzerMonitors(element);
                    Presets = new Skyline.DataMiner.Library.Common.DmsSpectrumAnalyzerPresets(element);
                    Scripts = new Skyline.DataMiner.Library.Common.DmsSpectrumAnalyzerScripts(element);
                }

                /// <summary>
                /// Manipulate the spectrum monitors.
                /// </summary>
                public Skyline.DataMiner.Library.Common.IDmsSpectrumAnalyzerMonitors Monitors
                {
                    get;
                    private set;
                }

                /// <summary>
                /// Manipulate the spectrum presets.
                /// </summary>
                public Skyline.DataMiner.Library.Common.IDmsSpectrumAnalyzerPresets Presets
                {
                    get;
                    private set;
                }

                /// <summary>
                /// Manipulate the spectrum scripts.
                /// </summary>
                public Skyline.DataMiner.Library.Common.IDmsSpectrumAnalyzerScripts Scripts
                {
                    get;
                    private set;
                }
            }

            /// <summary>
            /// Represents the spectrum analyzer monitors.
            /// </summary>
            internal class DmsSpectrumAnalyzerMonitors : Skyline.DataMiner.Library.Common.IDmsSpectrumAnalyzerMonitors
            {
                private readonly Skyline.DataMiner.Library.Common.IDmsElement element;
                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsSpectrumAnalyzerMonitors"/> class.
                /// </summary>
                /// <param name = "element">The element to which this spectrum analyzer component is part of.</param>
                public DmsSpectrumAnalyzerMonitors(Skyline.DataMiner.Library.Common.IDmsElement element)
                {
                    this.element = element;
                }
            }

            /// <summary>
            /// Represents spectrum analyzer presets.
            /// </summary>
            internal class DmsSpectrumAnalyzerPresets : Skyline.DataMiner.Library.Common.IDmsSpectrumAnalyzerPresets
            {
                private readonly Skyline.DataMiner.Library.Common.IDmsElement element;
                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsSpectrumAnalyzerPresets"/> class.
                /// </summary>
                /// <param name = "element">The element to which this spectrum analyzer component belongs.</param>
                public DmsSpectrumAnalyzerPresets(Skyline.DataMiner.Library.Common.IDmsElement element)
                {
                    this.element = element;
                }
            }

            /// <summary>
            /// Represents spectrum analyzer scripts.
            /// </summary>
            internal class DmsSpectrumAnalyzerScripts : Skyline.DataMiner.Library.Common.IDmsSpectrumAnalyzerScripts
            {
                private readonly Skyline.DataMiner.Library.Common.IDmsElement element;
                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsSpectrumAnalyzerScripts"/> class.
                /// </summary>
                /// <param name = "element">The element this spectrum analyzer component is part of.</param>
                public DmsSpectrumAnalyzerScripts(Skyline.DataMiner.Library.Common.IDmsElement element)
                {
                    this.element = element;
                }
            }

            /// <summary>
            /// Represents the spectrum analyzer component of an element.
            /// </summary>
            public interface IDmsSpectrumAnalyzer
            {
                /// <summary>
                /// Gets the spectrum analyzer monitors.
                /// </summary>
                /// <value>The spectrum analyzer monitors.</value>
                Skyline.DataMiner.Library.Common.IDmsSpectrumAnalyzerMonitors Monitors
                {
                    get;
                }

                /// <summary>
                /// Gets the spectrum analyzer presets.
                /// </summary>
                /// <value>The spectrum analyzer presets.</value>
                Skyline.DataMiner.Library.Common.IDmsSpectrumAnalyzerPresets Presets
                {
                    get;
                }

                /// <summary>
                /// Gets the spectrum analyzer scripts.
                /// </summary>
                /// <value>The spectrum analyzer scripts.</value>
                Skyline.DataMiner.Library.Common.IDmsSpectrumAnalyzerScripts Scripts
                {
                    get;
                }
            }

            /// <summary>
            /// Represents the spectrum analyzer monitors.
            /// </summary>
            public interface IDmsSpectrumAnalyzerMonitors
            {
            }

            /// <summary>
            /// Represents the spectrum analyzer presets.
            /// </summary>
            public interface IDmsSpectrumAnalyzerPresets
            {
            }

            /// <summary>
            ///  Represents spectrum analyzer scripts.
            /// </summary>
            public interface IDmsSpectrumAnalyzerScripts
            {
            }

            namespace Templates
            {
                /// <summary>
                /// Base class for standalone alarm templates and alarm template groups.
                /// </summary>
                internal abstract class DmsAlarmTemplate : Skyline.DataMiner.Library.Common.Templates.DmsTemplate, Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate
                {
                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsAlarmTemplate"/> class.
                    /// </summary>
                    /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
                    /// <param name = "name">The name of the alarm template.</param>
                    /// <param name = "protocol">Instance of the protocol.</param>
                    /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "name"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "protocol"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
                    protected DmsAlarmTemplate(Skyline.DataMiner.Library.Common.IDms dms, string name, Skyline.DataMiner.Library.Common.IDmsProtocol protocol): base(dms, name, protocol)
                    {
                    }

                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsAlarmTemplate"/> class.
                    /// </summary>
                    /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
                    /// <param name = "name">The name of the alarm template.</param>
                    /// <param name = "protocolName">The name of the protocol.</param>
                    /// <param name = "protocolVersion">The version of the protocol.</param>
                    /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "name"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "protocolName"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "protocolVersion"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
                    /// <exception cref = "ArgumentException"><paramref name = "protocolName"/> is the empty string ("") or white space.</exception>
                    /// <exception cref = "ArgumentException"><paramref name = "protocolVersion"/> is the empty string ("") or white space.</exception>
                    protected DmsAlarmTemplate(Skyline.DataMiner.Library.Common.IDms dms, string name, string protocolName, string protocolVersion): base(dms, name, protocolName, protocolVersion)
                    {
                    }

                    /// <summary>
                    /// Loads all the data and properties found related to the alarm template.
                    /// </summary>
                    /// <exception cref = "TemplateNotFoundException">The template does not exist in the DataMiner system.</exception>
                    internal override void Load()
                    {
                        Skyline.DataMiner.Net.Messages.GetAlarmTemplateMessage message = new Skyline.DataMiner.Net.Messages.GetAlarmTemplateMessage{AsOneObject = true, Protocol = Protocol.Name, Version = Protocol.Version, Template = Name};
                        Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage response = (Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage)Dms.Communication.SendSingleResponseMessage(message);
                        if (response != null)
                        {
                            Parse(response);
                        }
                        else
                        {
                            throw new Skyline.DataMiner.Library.Common.TemplateNotFoundException(Name, Protocol.Name, Protocol.Version);
                        }
                    }

                    /// <summary>
                    /// Parses the alarm template event message.
                    /// </summary>
                    /// <param name = "message">The message received from SLNet.</param>
                    internal abstract void Parse(Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage message);
                }

                /// <summary>
                /// Represents an alarm template group.
                /// </summary>
                internal class DmsAlarmTemplateGroup : Skyline.DataMiner.Library.Common.Templates.DmsAlarmTemplate, Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplateGroup
                {
                    /// <summary>
                    /// The entries of the alarm group.
                    /// </summary>
                    private readonly System.Collections.Generic.List<Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplateGroupEntry> entries = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplateGroupEntry>();
                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsAlarmTemplateGroup"/> class.
                    /// </summary>
                    /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
                    /// <param name = "name">The name of the alarm template.</param>
                    /// <param name = "protocol">The protocol this alarm template group corresponds with.</param>
                    /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "name"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "protocol"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
                    internal DmsAlarmTemplateGroup(Skyline.DataMiner.Library.Common.IDms dms, string name, Skyline.DataMiner.Library.Common.IDmsProtocol protocol): base(dms, name, protocol)
                    {
                        IsLoaded = false;
                    }

                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsAlarmTemplateGroup"/> class.
                    /// </summary>
                    /// <param name = "dms">Instance of <see cref = "Dms"/>.</param>
                    /// <param name = "alarmTemplateEventMessage">An instance of AlarmTemplateEventMessage.</param>
                    /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentException"><paramref name = "alarmTemplateEventMessage"/> is invalid.</exception>
                    internal DmsAlarmTemplateGroup(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage alarmTemplateEventMessage): base(dms, alarmTemplateEventMessage.Name, alarmTemplateEventMessage.Protocol, alarmTemplateEventMessage.Version)
                    {
                        IsLoaded = true;
                        foreach (Skyline.DataMiner.Net.Messages.AlarmTemplateGroupEntry entry in alarmTemplateEventMessage.GroupEntries)
                        {
                            Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate template = Protocol.GetAlarmTemplate(entry.Name);
                            entries.Add(new Skyline.DataMiner.Library.Common.Templates.DmsAlarmTemplateGroupEntry(template, entry.IsEnabled, entry.IsScheduled));
                        }
                    }

                    /// <summary>
                    /// Gets the entries of the alarm template group.
                    /// </summary>
                    public System.Collections.ObjectModel.ReadOnlyCollection<Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplateGroupEntry> Entries
                    {
                        get
                        {
                            LoadOnDemand();
                            return entries.AsReadOnly();
                        }
                    }

                    /// <summary>
                    /// Determines whether this alarm template exists in the DataMiner System.
                    /// </summary>
                    /// <returns><c>true</c> if the alarm template exists in the DataMiner System; otherwise, <c>false</c>.</returns>
                    public override bool Exists()
                    {
                        bool exists = false;
                        Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage template = GetAlarmTemplate();
                        if (template != null && template.Type == Skyline.DataMiner.Net.Messages.AlarmTemplateType.Group)
                        {
                            exists = true;
                        }

                        return exists;
                    }

                    /// <summary>
                    /// Returns a string that represents the current object.
                    /// </summary>
                    /// <returns>A string that represents the current object.</returns>
                    public override string ToString()
                    {
                        return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Template Group Name: {0}, Protocol Name: {1}, Protocol Version: {2}", Name, Protocol.Name, Protocol.Version);
                    }

                    /// <summary>
                    /// Parses the alarm template event message.
                    /// </summary>
                    /// <param name = "message">The message received from the SLNet process.</param>
                    internal override void Parse(Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage message)
                    {
                        IsLoaded = true;
                        entries.Clear();
                        foreach (Skyline.DataMiner.Net.Messages.AlarmTemplateGroupEntry entry in message.GroupEntries)
                        {
                            Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate template = Protocol.GetAlarmTemplate(entry.Name);
                            entries.Add(new Skyline.DataMiner.Library.Common.Templates.DmsAlarmTemplateGroupEntry(template, entry.IsEnabled, entry.IsScheduled));
                        }
                    }

                    /// <summary>
                    /// Gets the alarm template from the SLNet process.
                    /// </summary>
                    /// <returns>The alarm template.</returns>
                    private Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage GetAlarmTemplate()
                    {
                        Skyline.DataMiner.Net.Messages.GetAlarmTemplateMessage message = new Skyline.DataMiner.Net.Messages.GetAlarmTemplateMessage{AsOneObject = true, Protocol = Protocol.Name, Version = Protocol.Version, Template = Name};
                        Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage cachedAlarmTemplateMessage = (Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage)Dms.Communication.SendSingleResponseMessage(message);
                        return cachedAlarmTemplateMessage;
                    }
                }

                /// <summary>
                /// Represents an alarm group entry.
                /// </summary>
                internal class DmsAlarmTemplateGroupEntry : Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplateGroupEntry
                {
                    /// <summary>
                    /// The template which is an entry of the alarm group.
                    /// </summary>
                    private readonly Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate template;
                    /// <summary>
                    /// Specifies whether this entry is enabled.
                    /// </summary>
                    private readonly bool isEnabled;
                    /// <summary>
                    /// Specifies whether this entry is scheduled.
                    /// </summary>
                    private readonly bool isScheduled;
                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsAlarmTemplateGroupEntry"/> class.
                    /// </summary>
                    /// <param name = "template">The alarm template.</param>
                    /// <param name = "isEnabled">Specifies if the entry is enabled.</param>
                    /// <param name = "isScheduled">Specifies if the entry is scheduled.</param>
                    internal DmsAlarmTemplateGroupEntry(Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate template, bool isEnabled, bool isScheduled)
                    {
                        if (template == null)
                        {
                            throw new System.ArgumentNullException("template");
                        }

                        this.template = template;
                        this.isEnabled = isEnabled;
                        this.isScheduled = isScheduled;
                    }

                    /// <summary>
                    /// Gets the alarm template.
                    /// </summary>
                    public Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate AlarmTemplate
                    {
                        get
                        {
                            return template;
                        }
                    }

                    /// <summary>
                    /// Gets a value indicating whether the entry is enabled.
                    /// </summary>
                    public bool IsEnabled
                    {
                        get
                        {
                            return isEnabled;
                        }
                    }

                    /// <summary>
                    /// Gets a value indicating whether the entry is scheduled.
                    /// </summary>
                    public bool IsScheduled
                    {
                        get
                        {
                            return isScheduled;
                        }
                    }

                    /// <summary>
                    /// Returns a string that represents the current object.
                    /// </summary>
                    /// <returns>A string that represents the current object.</returns>
                    public override string ToString()
                    {
                        return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Alarm template group entry:{0}", template.Name);
                    }
                }

                /// <summary>
                /// Represents a standalone alarm template.
                /// </summary>
                internal class DmsStandaloneAlarmTemplate : Skyline.DataMiner.Library.Common.Templates.DmsAlarmTemplate, Skyline.DataMiner.Library.Common.Templates.IDmsStandaloneAlarmTemplate
                {
                    /// <summary>
                    /// The description of the alarm definition.
                    /// </summary>
                    private string description;
                    /// <summary>
                    /// Indicates whether this alarm template is used in a group.
                    /// </summary>
                    private bool isUsedInGroup;
                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsStandaloneAlarmTemplate"/> class.
                    /// </summary>
                    /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
                    /// <param name = "name">The name of the alarm template.</param>
                    /// <param name = "protocol">The protocol this standalone alarm template corresponds with.</param>
                    /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "name"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "protocol"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
                    internal DmsStandaloneAlarmTemplate(Skyline.DataMiner.Library.Common.IDms dms, string name, Skyline.DataMiner.Library.Common.IDmsProtocol protocol): base(dms, name, protocol)
                    {
                        IsLoaded = false;
                    }

                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsStandaloneAlarmTemplate"/> class.
                    /// </summary>
                    /// <param name = "dms">The DataMiner system reference.</param>
                    /// <param name = "alarmTemplateEventMessage">An instance of AlarmTemplateEventMessage.</param>
                    /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentException"><paramref name = "dms"/> is invalid.</exception>
                    internal DmsStandaloneAlarmTemplate(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage alarmTemplateEventMessage): base(dms, alarmTemplateEventMessage.Name, alarmTemplateEventMessage.Protocol, alarmTemplateEventMessage.Version)
                    {
                        IsLoaded = true;
                        description = alarmTemplateEventMessage.Description;
                        isUsedInGroup = alarmTemplateEventMessage.IsUsedInGroup;
                    }

                    /// <summary>
                    /// Gets or sets the alarm template description.
                    /// </summary>
                    public string Description
                    {
                        get
                        {
                            LoadOnDemand();
                            return description;
                        }

                        set
                        {
                            LoadOnDemand();
                            ChangedPropertyList.Add("Description");
                            description = value;
                        }
                    }

                    /// <summary>
                    /// Gets a value indicating whether the alarm template is used in a group.
                    /// </summary>
                    public bool IsUsedInGroup
                    {
                        get
                        {
                            LoadOnDemand();
                            return isUsedInGroup;
                        }
                    }

                    /// <summary>
                    /// Determines whether this alarm template exists in the DataMiner System.
                    /// </summary>
                    /// <returns><c>true</c> if the alarm template exists in the DataMiner System; otherwise, <c>false</c>.</returns>
                    public override bool Exists()
                    {
                        return Protocol.StandaloneAlarmTemplateExists(Name);
                    }

                    /// <summary>
                    /// Returns a string that represents the current object.
                    /// </summary>
                    /// <returns>A string that represents the current object.</returns>
                    public override string ToString()
                    {
                        return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Alarm Template Name: {0}, Protocol Name: {1}, Protocol Version: {2}", Name, Protocol.Name, Protocol.Version);
                    }

                    /// <summary>
                    /// Parses the alarm template event message.
                    /// </summary>
                    /// <param name = "message">The message received from SLNet.</param>
                    internal override void Parse(Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage message)
                    {
                        IsLoaded = true;
                        description = message.Description;
                        isUsedInGroup = message.IsUsedInGroup;
                    }
                }

                /// <summary>
                /// Represents an alarm template.
                /// </summary>
                internal abstract class DmsTemplate : Skyline.DataMiner.Library.Common.DmsObject
                {
                    /// <summary>
                    /// Alarm template name.
                    /// </summary>
                    private readonly string name;
                    /// <summary>
                    /// The protocol this alarm template corresponds with.
                    /// </summary>
                    private readonly Skyline.DataMiner.Library.Common.IDmsProtocol protocol;
                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsTemplate"/> class.
                    /// </summary>
                    /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
                    /// <param name = "name">The name of the alarm template.</param>
                    /// <param name = "protocol">Instance of the protocol.</param>
                    /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "name"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "protocol"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
                    protected DmsTemplate(Skyline.DataMiner.Library.Common.IDms dms, string name, Skyline.DataMiner.Library.Common.IDmsProtocol protocol): base(dms)
                    {
                        if (name == null)
                        {
                            throw new System.ArgumentNullException("name");
                        }

                        if (protocol == null)
                        {
                            throw new System.ArgumentNullException("protocol");
                        }

                        if (System.String.IsNullOrWhiteSpace(name))
                        {
                            throw new System.ArgumentException("The name of the template is the empty string (\"\") or white space.");
                        }

                        this.name = name;
                        this.protocol = protocol;
                    }

                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsTemplate"/> class.
                    /// </summary>
                    /// <param name = "dms">The DataMiner System reference.</param>
                    /// <param name = "name">The template name.</param>
                    /// <param name = "protocolName">The name of the protocol.</param>
                    /// <param name = "protocolVersion">The version of the protocol.</param>
                    /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "name"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "protocolName"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "protocolVersion"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
                    /// <exception cref = "ArgumentException"><paramref name = "protocolName"/> is the empty string ("") or white space.</exception>
                    /// <exception cref = "ArgumentException"><paramref name = "protocolVersion"/> is the empty string ("") or white space.</exception>
                    protected DmsTemplate(Skyline.DataMiner.Library.Common.IDms dms, string name, string protocolName, string protocolVersion): base(dms)
                    {
                        if (name == null)
                        {
                            throw new System.ArgumentNullException("name");
                        }

                        if (protocolName == null)
                        {
                            throw new System.ArgumentNullException("protocolName");
                        }

                        if (protocolVersion == null)
                        {
                            throw new System.ArgumentNullException("protocolVersion");
                        }

                        if (System.String.IsNullOrWhiteSpace(name))
                        {
                            throw new System.ArgumentException("The name of the template is the empty string(\"\") or white space.", "name");
                        }

                        if (System.String.IsNullOrWhiteSpace(protocolName))
                        {
                            throw new System.ArgumentException("The name of the protocol is the empty string (\"\") or white space.", "protocolName");
                        }

                        if (System.String.IsNullOrWhiteSpace(protocolVersion))
                        {
                            throw new System.ArgumentException("The version of the protocol is the empty string (\"\") or white space.", "protocolVersion");
                        }

                        this.name = name;
                        protocol = new Skyline.DataMiner.Library.Common.DmsProtocol(dms, protocolName, protocolVersion);
                    }

                    /// <summary>
                    /// Gets the template name.
                    /// </summary>
                    public string Name
                    {
                        get
                        {
                            return name;
                        }
                    }

                    /// <summary>
                    /// Gets the protocol this template corresponds with.
                    /// </summary>
                    public Skyline.DataMiner.Library.Common.IDmsProtocol Protocol
                    {
                        get
                        {
                            return protocol;
                        }
                    }
                }

                /// <summary>
                /// Represents a trend template.
                /// </summary>
                internal class DmsTrendTemplate : Skyline.DataMiner.Library.Common.Templates.DmsTemplate, Skyline.DataMiner.Library.Common.Templates.IDmsTrendTemplate
                {
                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsTrendTemplate"/> class.
                    /// </summary>
                    /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
                    /// <param name = "name">The name of the alarm template.</param>
                    /// <param name = "protocol">The instance of the protocol.</param>
                    /// <exception cref = "ArgumentNullException">Dms is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException">Name is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException">Protocol is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
                    internal DmsTrendTemplate(Skyline.DataMiner.Library.Common.IDms dms, string name, Skyline.DataMiner.Library.Common.IDmsProtocol protocol): base(dms, name, protocol)
                    {
                        IsLoaded = true;
                    }

                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsTrendTemplate"/> class.
                    /// </summary>
                    /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
                    /// <param name = "templateInfo">The template info received by SLNet.</param>
                    /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException">name is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException">protocolName is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException">protocolVersion is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentException">name is the empty string ("") or white space.</exception>
                    /// <exception cref = "ArgumentException">ProtocolName is the empty string ("") or white space.</exception>
                    /// <exception cref = "ArgumentException">ProtocolVersion is the empty string ("") or white space.</exception>
                    internal DmsTrendTemplate(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Net.Messages.GetTrendingTemplateInfoResponseMessage templateInfo): base(dms, templateInfo.Name, templateInfo.Protocol, templateInfo.Version)
                    {
                        IsLoaded = true;
                    }

                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsTrendTemplate"/> class.
                    /// </summary>
                    /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
                    /// <param name = "templateInfo">The template info received by SLNet.</param>
                    /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException">Name is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException">ProtocolName is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException">ProtocolVersion is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentException">Name is the empty string ("") or white space.</exception>
                    /// <exception cref = "ArgumentException">ProtocolName is the empty string ("") or white space.</exception>
                    /// <exception cref = "ArgumentException">ProtocolVersion is the empty string ("") or white space.</exception>
                    internal DmsTrendTemplate(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Net.Messages.TrendTemplateMetaInfo templateInfo): base(dms, templateInfo.Name, templateInfo.ProtocolName, templateInfo.ProtocolVersion)
                    {
                        IsLoaded = true;
                    }

                    /// <summary>
                    /// Determines whether this trend template exists in the DataMiner System.
                    /// </summary>
                    /// <returns><c>true</c> if the trend template exists in the DataMiner System; otherwise, <c>false</c>.</returns>
                    public override bool Exists()
                    {
                        Skyline.DataMiner.Net.Messages.GetTrendingTemplateInfoMessage message = new Skyline.DataMiner.Net.Messages.GetTrendingTemplateInfoMessage{Protocol = Protocol.Name, Version = Protocol.Version, Template = Name};
                        Skyline.DataMiner.Net.Messages.GetTrendingTemplateInfoResponseMessage response = (Skyline.DataMiner.Net.Messages.GetTrendingTemplateInfoResponseMessage)Dms.Communication.SendSingleResponseMessage(message);
                        return response != null;
                    }

                    /// <summary>
                    /// Returns a string that represents the current object.
                    /// </summary>
                    /// <returns>A string that represents the current object.</returns>
                    public override string ToString()
                    {
                        return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Trend Template Name: {0}, Protocol Name: {1}, Protocol Version: {2}", Name, Protocol.Name, Protocol.Version);
                    }

                    /// <summary>
                    /// Loads this object.
                    /// </summary>
                    internal override void Load()
                    {
                    }
                }

                /// <summary>
                /// DataMiner alarm template interface.
                /// </summary>
                public interface IDmsAlarmTemplate : Skyline.DataMiner.Library.Common.Templates.IDmsTemplate
                {
                }

                /// <summary>
                /// DataMiner alarm template group interface.
                /// </summary>
                public interface IDmsAlarmTemplateGroup : Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate
                {
                    /// <summary>
                    /// Gets the entries of the alarm template group.
                    /// </summary>
                    System.Collections.ObjectModel.ReadOnlyCollection<Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplateGroupEntry> Entries
                    {
                        get;
                    }
                }

                /// <summary>
                /// DataMiner alarm template group entry interface.
                /// </summary>
                public interface IDmsAlarmTemplateGroupEntry
                {
                    /// <summary>
                    /// Gets a value indicating whether the entry is enabled.
                    /// </summary>
                    bool IsEnabled
                    {
                        get;
                    }

                    /// <summary>
                    /// Gets a value indicating whether the entry is scheduled.
                    /// </summary>
                    bool IsScheduled
                    {
                        get;
                    }

                    /// <summary>
                    /// Gets the alarm template.
                    /// </summary>
                    Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate AlarmTemplate
                    {
                        get;
                    }
                }

                /// <summary>
                /// DataMiner standalone alarm template interface.
                /// </summary>
                public interface IDmsStandaloneAlarmTemplate : Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate
                {
                    /// <summary>
                    /// Gets or sets the alarm template description.
                    /// </summary>
                    string Description
                    {
                        get;
                        set;
                    }

                    /// <summary>
                    /// Gets a value indicating whether the alarm template is used in a group.
                    /// </summary>
                    bool IsUsedInGroup
                    {
                        get;
                    }
                }

                /// <summary>
                /// DataMiner template interface.
                /// </summary>
                public interface IDmsTemplate : Skyline.DataMiner.Library.Common.IDmsObject
                {
                    /// <summary>
                    /// Gets the template name.
                    /// </summary>
                    string Name
                    {
                        get;
                    }

                    /// <summary>
                    /// Gets the protocol this template corresponds with.
                    /// </summary>
                    Skyline.DataMiner.Library.Common.IDmsProtocol Protocol
                    {
                        get;
                    }
                }

                /// <summary>
                /// DataMiner trend template interface.
                /// </summary>
                public interface IDmsTrendTemplate : Skyline.DataMiner.Library.Common.Templates.IDmsTemplate
                {
                }
            }

            /// <summary>
            /// Represents a DataMiner view.
            /// </summary>
            internal class DmsView : Skyline.DataMiner.Library.Common.DmsObject, Skyline.DataMiner.Library.Common.IDmsView
            {
                /// <summary>
                /// The child views.
                /// </summary>
                private readonly System.Collections.Generic.List<Skyline.DataMiner.Library.Common.IDmsView> childViews = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.IDmsView>();
                /// <summary>
                /// The elements that are part of this view.
                /// </summary>
                private readonly System.Collections.Generic.List<Skyline.DataMiner.Library.Common.IDmsElement> elements = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.IDmsElement>();
                /// <summary>
                /// The properties.
                /// </summary>
                private readonly System.Collections.Generic.IDictionary<System.String, Skyline.DataMiner.Library.Common.Properties.DmsViewProperty> properties = new System.Collections.Generic.Dictionary<System.String, Skyline.DataMiner.Library.Common.Properties.DmsViewProperty>();
                /// <summary>
                /// The names of updated properties.
                /// </summary>
                private readonly System.Collections.Generic.HashSet<System.String> updatedProperties = new System.Collections.Generic.HashSet<System.String>();
                /// <summary>
                /// The display string.
                /// </summary>
                private string display = System.String.Empty;
                /// <summary>
                /// ID of the view.
                /// </summary>
                private int id = -1;
                /// <summary>
                /// The parent view.
                /// </summary>
                private Skyline.DataMiner.Library.Common.IDmsView parentView;
                /// <summary>
                /// The name of the view.
                /// </summary>
                private string name;
                private bool isNameLoaded;
                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsView"/> class.
                /// </summary>
                /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
                /// <param name = "viewId">The ID of the view.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
                internal DmsView(Skyline.DataMiner.Library.Common.IDms dms, int viewId): base(dms)
                {
                    id = viewId;
                    IsLoaded = false;
                    isNameLoaded = false;
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsView"/> class.
                /// </summary>
                /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
                /// <param name = "id">The ID of the view.</param>
                /// <param name = "name">The name of the view.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentNullException"><paramref name = "name"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "name">is empty or white space.</paramref></exception>
                internal DmsView(Skyline.DataMiner.Library.Common.IDms dms, int id, string name): base(dms)
                {
                    if (name == null)
                    {
                        throw new System.ArgumentNullException("name");
                    }

                    if (System.String.IsNullOrWhiteSpace(name))
                    {
                        throw new System.ArgumentException("Provided name must not be empty or white space", "name");
                    }

                    this.id = id;
                    this.name = name;
                    IsLoaded = false;
                    isNameLoaded = true;
                }

                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsView"/> class.
                /// </summary>
                /// <param name = "dms">Instance of the DataMinerSystem class.</param>
                /// <param name = "viewInfo">The view info.</param>
                internal DmsView(Skyline.DataMiner.Library.Common.Dms dms, Skyline.DataMiner.Net.Messages.ViewInfoEventMessage viewInfo): base(dms)
                {
                    Parse(viewInfo);
                    // Remove the properties that are added to the change list because of initialization.
                    ClearChangeList();
                }

                /// <summary>
                /// Gets all child views.
                /// </summary>
                /// <value>The child views.</value>
                public System.Collections.Generic.IList<Skyline.DataMiner.Library.Common.IDmsView> ChildViews
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
                public System.Collections.Generic.IList<Skyline.DataMiner.Library.Common.IDmsElement> Elements
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
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation is invalid.</exception>
                /// <remarks>
                /// <para>The following restrictions apply to view names:</para>
                /// <list type = "bullet">
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
                        string validatedViewName = Skyline.DataMiner.Library.Common.InputValidator.ValidateViewName(value, "value");
                        if (!isNameLoaded)
                        {
                            LoadOnDemand();
                        }

                        if (!name.Equals(validatedViewName, System.StringComparison.Ordinal))
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
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                /// <exception cref = "NotSupportedException">The root view cannot be assigned a parent view.</exception>
                /// <exception cref = "NotSupportedException">The parent of a view must not be a self-reference.</exception>
                public Skyline.DataMiner.Library.Common.IDmsView Parent
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
                            throw new System.ArgumentNullException("value");
                        }

                        if (Id == -1)
                        {
                            throw new System.NotSupportedException("The root view cannot be assigned a parent view.");
                        }

                        LoadOnDemand();
                        if (value.Id == this.Id)
                        {
                            throw new System.NotSupportedException("The parent of a view must not be a self-reference.");
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
                public Skyline.DataMiner.Library.Common.IPropertyCollection<Skyline.DataMiner.Library.Common.Properties.IDmsViewProperty, Skyline.DataMiner.Library.Common.Properties.IDmsViewPropertyDefinition> Properties
                {
                    get
                    {
                        LoadOnDemand();
                        System.Collections.Generic.IDictionary<System.String, Skyline.DataMiner.Library.Common.Properties.IDmsViewProperty> copy = new System.Collections.Generic.Dictionary<System.String, Skyline.DataMiner.Library.Common.Properties.IDmsViewProperty>(properties.Count);
                        foreach (System.Collections.Generic.KeyValuePair<System.String, Skyline.DataMiner.Library.Common.Properties.DmsViewProperty> kvp in properties)
                        {
                            copy.Add(kvp.Key, kvp.Value);
                        }

                        return new Skyline.DataMiner.Library.Common.PropertyCollection<Skyline.DataMiner.Library.Common.Properties.IDmsViewProperty, Skyline.DataMiner.Library.Common.Properties.IDmsViewPropertyDefinition>(copy);
                    }
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
                    return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "View name: {0}, ID: {1}", Name, Id);
                }

                /// <summary>
                /// Updates the view.
                /// </summary>
                public void Update()
                {
                    // If any op the properties where updated, add "Properties" to the property changed list.
                    CheckIfPropertiesEdited();
                    if (System.Linq.Enumerable.Any(ChangedPropertyList))
                    {
                        try
                        {
                            ExecuteUpdates();
                            // Reset this flag because we want to reload the latest values in the object.
                            IsLoaded = false;
                            isNameLoaded = false;
                            ClearChangeList();
                        }
                        catch (System.Exception)
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
                /// <exception cref = "ViewNotFoundException">No view with the specified ID exists in the DataMiner System.</exception>
                internal override void Load()
                {
                    try
                    {
                        IsLoaded = true;
                        isNameLoaded = true;
                        Skyline.DataMiner.Net.Messages.ViewInfoEventMessage infoEvent = null;
                        Skyline.DataMiner.Net.Messages.GetInfoMessage message = new Skyline.DataMiner.Net.Messages.GetInfoMessage{Type = Skyline.DataMiner.Net.Messages.InfoType.ViewInfo};
                        Skyline.DataMiner.Net.Messages.DMSMessage[] responses = Communication.SendMessage(message);
                        foreach (Skyline.DataMiner.Net.Messages.DMSMessage response in responses)
                        {
                            Skyline.DataMiner.Net.Messages.ViewInfoEventMessage viewInfo = (Skyline.DataMiner.Net.Messages.ViewInfoEventMessage)response;
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
                            throw new Skyline.DataMiner.Library.Common.ViewNotFoundException(id);
                        }
                    }
                    catch (Skyline.DataMiner.Net.Exceptions.DataMinerException)
                    {
                        IsLoaded = false;
                        isNameLoaded = false;
                        throw;
                    }
                }

                internal void PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
                {
                    updatedProperties.Add(e.PropertyName);
                }

                /// <summary>
                /// Parses the view info event message.
                /// </summary>
                /// <param name = "viewInfo">The view info event message.</param>
                internal void Parse(Skyline.DataMiner.Net.Messages.ViewInfoEventMessage viewInfo)
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
                /// <param name = "viewInfo">The view information.</param>
                private void ParseView(Skyline.DataMiner.Net.Messages.ViewInfoEventMessage viewInfo)
                {
                    name = viewInfo.Name;
                    id = viewInfo.ID;
                    display = viewInfo.DisplayName;
                    parentView = Id == -1 ? null : new Skyline.DataMiner.Library.Common.DmsView(dms, viewInfo.ParentId);
                }

                /// <summary>
                /// Parses the child view.
                /// </summary>
                /// <param name = "viewInfo">The view information.</param>
                private void ParseChildViews(Skyline.DataMiner.Net.Messages.ViewInfoEventMessage viewInfo)
                {
                    if (viewInfo.DirectChildViews != null)
                    {
                        foreach (int viewID in viewInfo.DirectChildViews)
                        {
                            Skyline.DataMiner.Library.Common.DmsView childView = new Skyline.DataMiner.Library.Common.DmsView(dms, viewID);
                            childViews.Add(childView);
                        }
                    }
                }

                /// <summary>
                /// Parses the child view.
                /// </summary>
                /// <param name = "viewInfo">The view information.</param>
                private void ParseChildElements(Skyline.DataMiner.Net.Messages.ViewInfoEventMessage viewInfo)
                {
                    if (viewInfo.Elements != null)
                    {
                        foreach (string identifier in viewInfo.Elements)
                        {
                            Skyline.DataMiner.Library.Common.DmsElementId dmaEid = new Skyline.DataMiner.Library.Common.DmsElementId(identifier);
                            Skyline.DataMiner.Library.Common.DmsElement element = new Skyline.DataMiner.Library.Common.DmsElement(dms, dmaEid);
                            elements.Add(element);
                        }
                    }
                }

                /// <summary>
                /// Parses the view properties.
                /// </summary>
                /// <param name = "viewInfo">The view information.</param>
                private void ParseProperties(Skyline.DataMiner.Net.Messages.ViewInfoEventMessage viewInfo)
                {
                    properties.Clear();
                    foreach (Skyline.DataMiner.Library.Common.Properties.IDmsViewPropertyDefinition definition in Dms.ViewPropertyDefinitions)
                    {
                        Skyline.DataMiner.Net.Messages.PropertyInfo info = null;
                        if (viewInfo.Properties != null)
                        {
                            info = System.Linq.Enumerable.FirstOrDefault(viewInfo.Properties, p => p.Name.Equals(definition.Name, System.StringComparison.OrdinalIgnoreCase));
                            System.Collections.Generic.List<System.String> duplicates = System.Linq.Enumerable.ToList(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(System.Linq.Enumerable.GroupBy(viewInfo.Properties, p => p.Name), g => System.Linq.Enumerable.Count(g) > 1), g => g.Key));
                            if (System.Linq.Enumerable.Any(duplicates))
                            {
                                string message = "Duplicate view properties detected. View \"" + viewInfo.Name + "\" (" + viewInfo.ID + "), duplicate properties: " + System.String.Join(", ", duplicates) + ".";
                                Skyline.DataMiner.Library.Common.Logger.Log(message);
                            }
                        }

                        string propertyValue = info != null ? info.Value : System.String.Empty;
                        if (definition.IsReadOnly)
                        {
                            properties.Add(definition.Name, new Skyline.DataMiner.Library.Common.Properties.DmsViewProperty(this, definition, propertyValue));
                        }
                        else
                        {
                            var property = new Skyline.DataMiner.Library.Common.Properties.DmsWritableViewProperty(this, definition, propertyValue);
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
                    if (System.Linq.Enumerable.Any(updatedProperties))
                    {
                        ChangedPropertyList.Add("Properties");
                    }
                }

                /// <summary>
                /// Performs an update of the properties.
                /// </summary>
                private void UpdateProperties()
                {
                    Skyline.DataMiner.Net.Messages.PSA propertyArray = new Skyline.DataMiner.Net.Messages.PSA{Psa = new Skyline.DataMiner.Net.Messages.SA[]{}};
                    System.Collections.Generic.List<Skyline.DataMiner.Net.Messages.SA> changedProperties = new System.Collections.Generic.List<Skyline.DataMiner.Net.Messages.SA>();
                    foreach (string update in updatedProperties)
                    {
                        Skyline.DataMiner.Library.Common.Properties.DmsViewProperty property = properties[update];
                        Skyline.DataMiner.Net.Messages.SA sa = new Skyline.DataMiner.Net.Messages.SA{Sa = new string[]{property.Definition.Name, "read-write", property.Value}};
                        changedProperties.Add(sa);
                    }

                    propertyArray.Psa = changedProperties.ToArray();
                    Skyline.DataMiner.Net.Messages.Advanced.SetDataMinerInfoMessage infoMessage = new Skyline.DataMiner.Net.Messages.Advanced.SetDataMinerInfoMessage{bInfo1 = System.Int32.MaxValue, bInfo2 = System.Int32.MaxValue, DataMinerID = -1, ElementID = -1, IInfo1 = System.Int32.MaxValue, IInfo2 = System.Int32.MaxValue, Psa2 = propertyArray, What = 62, StrInfo1 = "view:" + Id};
                    Communication.SendSingleResponseMessage(infoMessage);
                }

                /// <summary>
                /// Renames the view.
                /// </summary>
                private void UpdateName()
                {
                    Skyline.DataMiner.Net.Messages.Advanced.SetDataMinerInfoMessage infoMessage = new Skyline.DataMiner.Net.Messages.Advanced.SetDataMinerInfoMessage{bInfo1 = System.Int32.MaxValue, bInfo2 = System.Int32.MaxValue, DataMinerID = -1, ElementID = -1, IInfo1 = id, IInfo2 = System.Int32.MaxValue, StrInfo2 = Name, What = 6};
                    Communication.SendSingleResponseMessage(infoMessage);
                }

                /// <summary>
                /// Changes the current view to a new parent.
                /// </summary>
                private void UpdateParent()
                {
                    Skyline.DataMiner.Net.Messages.Advanced.SetDataMinerInfoMessage infoMessage = new Skyline.DataMiner.Net.Messages.Advanced.SetDataMinerInfoMessage{IInfo1 = Id, IInfo2 = parentView.Id, What = 118};
                    Communication.SendSingleResponseMessage(infoMessage);
                }
            }

            /// <summary>
            /// DataMiner view interface.
            /// </summary>
            public interface IDmsView : Skyline.DataMiner.Library.Common.IDmsObject, Skyline.DataMiner.Library.Common.IUpdateable
            {
                /// <summary>
                /// Gets all child views.
                /// </summary>
                /// <value>The child views.</value>
                System.Collections.Generic.IList<Skyline.DataMiner.Library.Common.IDmsView> ChildViews
                {
                    get;
                }

                /// <summary>
                /// Gets the display string.
                /// </summary>
                /// <value>The display string.</value>
                string Display
                {
                    get;
                }

                /// <summary>
                /// Gets all elements that are immediate children of this view.
                /// </summary>
                /// <value>The elements that are immediate children of this view.</value>
                System.Collections.Generic.IList<Skyline.DataMiner.Library.Common.IDmsElement> Elements
                {
                    get;
                }

                /// <summary>
                /// Gets the ID of this view.
                /// </summary>
                /// <value>The view ID.</value>
                int Id
                {
                    get;
                }

                /// <summary>
                /// Gets or sets the name of the view.
                /// </summary>
                /// <value>The view name.</value>
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException">The value of a set operation is invalid.</exception>
                /// <remarks>
                /// <para>The following restrictions apply to view names:</para>
                /// <list type = "bullet">
                /// <item><para>Must not be empty ("") or white space.</para></item>
                /// <item><para>Must not exceed 200 characters.</para></item>
                /// <item><para>Names may not start or end with the following characters: '.' (dot), ' ' (space).</para></item>
                /// <item><para>Names may not contain the following character: '|' (pipe).</para></item>
                /// <item><para>The following characters may not occur more than once within a name: '%' (percentage).</para></item>
                /// </list>
                /// </remarks>
                string Name
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the parent view.
                /// </summary>
                /// <value>The parent view.</value>
                /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
                /// <exception cref = "NotSupportedException">The root view is assigned a parent view.</exception>
                /// <exception cref = "NotSupportedException">The parent view is a self-reference.</exception>
                Skyline.DataMiner.Library.Common.IDmsView Parent
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets the properties of this view.
                /// </summary>
                /// <value>The view properties.</value>
                Skyline.DataMiner.Library.Common.IPropertyCollection<Skyline.DataMiner.Library.Common.Properties.IDmsViewProperty, Skyline.DataMiner.Library.Common.Properties.IDmsViewPropertyDefinition> Properties
                {
                    get;
                }
            }

            /// <summary>
            /// Represents a table column.
            /// </summary>
            /// <typeparam name = "T">The type of the values this column holds.</typeparam>
            internal class DmsColumn<T> : Skyline.DataMiner.Library.Common.DmsParameter<T>, Skyline.DataMiner.Library.Common.IDmsColumn<T>
            {
                /// <summary>
                /// The table this column belongs to.
                /// </summary>
                private readonly Skyline.DataMiner.Library.Common.IDmsTable table;
                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsColumn{T}"/> class.
                /// </summary>
                /// <param name = "table">The table this column belongs to.</param>
                /// <param name = "id">The column parameter ID.</param>
                internal DmsColumn(Skyline.DataMiner.Library.Common.IDmsTable table, int id): base(id)
                {
                    if (table == null)
                    {
                        throw new System.ArgumentNullException("table");
                    }

                    this.table = table;
                }

                /// <summary>
                /// Gets the table this column is part of.
                /// </summary>
                /// <value>The table this column is part of.</value>
                public Skyline.DataMiner.Library.Common.IDmsTable Table
                {
                    get
                    {
                        return table;
                    }
                }

                /// <summary>
                /// Sets the value of a cell in a table.
                /// </summary>
                /// <param name = "primaryKey">The primary key of the row.</param>
                /// <param name = "value">The value to set.</param>
                /// <exception cref = "ArgumentNullException">
                /// <paramref name = "primaryKey"/> or <paramref name = "value"/> is <see langword = "null"/>.
                /// </exception>
                /// <exception cref = "ElementNotFoundException">
                /// The element was not found in the DataMiner System.
                /// </exception>
                /// <exception cref = "ElementStoppedException">The element is stopped.</exception>
                public void SetValue(string primaryKey, T value)
                {
                    if (primaryKey == null)
                    {
                        throw new System.ArgumentNullException("primaryKey");
                    }

                    if (System.String.IsNullOrWhiteSpace(primaryKey))
                    {
                        throw new System.ArgumentException("The primary key must not be the empty string (\"\") or white space.", "primaryKey");
                    }

                    Skyline.DataMiner.Library.Common.IDmsElement element = table.Element;
                    Skyline.DataMiner.Library.Common.HelperClass.CheckElementState(element);
                    Skyline.DataMiner.Net.Messages.SetParameterMessage message = new Skyline.DataMiner.Net.Messages.SetParameterMessage{DataMinerID = element.DmsElementId.AgentId, ElId = element.DmsElementId.ElementId, ParameterId = Id, TableIndex = primaryKey, TableIndexPreference = Skyline.DataMiner.Net.Messages.SetParameterTableIndexPreference.ByPrimaryKey, DisableInformationEventMessage = true};
                    if (AddValueToSetParameterMessage(message, value))
                    {
                        element.Host.Dms.Communication.SendMessage(message);
                    }
                }
            }

            /// <summary>
            /// Base class for parameters.
            /// </summary>
            /// <typeparam name = "T">The parameter type.</typeparam>
            internal class DmsParameter<T>
            {
                /// <summary>
                /// Setter delegates.
                /// </summary>
                private static readonly System.Collections.Generic.Dictionary<System.Type, System.Func<Skyline.DataMiner.Net.Messages.SetParameterMessage, T, System.Boolean>> Setters = new System.Collections.Generic.Dictionary<System.Type, System.Func<Skyline.DataMiner.Net.Messages.SetParameterMessage, T, System.Boolean>>{{typeof(string), new System.Func<Skyline.DataMiner.Net.Messages.SetParameterMessage, T, System.Boolean>(AddStringValueToSetParameterMessage)}, {typeof(int? ), new System.Func<Skyline.DataMiner.Net.Messages.SetParameterMessage, T, System.Boolean>(AddNullableIntValueToSetParameterMessage)}, {typeof(double? ), new System.Func<Skyline.DataMiner.Net.Messages.SetParameterMessage, T, System.Boolean>(AddNullableDoubleValueToSetParameterMessage)}, {typeof(System.DateTime? ), new System.Func<Skyline.DataMiner.Net.Messages.SetParameterMessage, T, System.Boolean>(AddNullableDateTimeValueToSetParameterMessage)}};
                /// <summary>
                /// The parameter ID.
                /// </summary>
                private readonly int id;
                /// <summary>
                /// The type of the parameter.
                /// </summary>
                /// <remarks>Currently supported types: int?, double?, string, DateTime?.</remarks>
                private readonly System.Type type;
                /// <summary>
                /// The underlying type (in case of Nullable&lt;T&gt;).
                /// </summary>
                private readonly System.Type underlyingType;
                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsParameter{T}"/> class.
                /// </summary>
                /// <param name = "id">The parameter ID.</param>
                /// <exception cref = "ArgumentException"><paramref name = "id"/> is invalid.</exception>
                protected DmsParameter(int id)
                {
                    if (id < 0)
                    {
                        throw new System.ArgumentException("Invalid parameter ID", "id");
                    }

                    this.id = id;
                    type = typeof(T);
                    underlyingType = System.Nullable.GetUnderlyingType(type);
                }

                /// <summary>
                /// Gets the parameter ID.
                /// </summary>
                /// <value>The parameter ID.</value>
                public int Id
                {
                    get
                    {
                        return id;
                    }
                }

                /// <summary>
                /// Adds the value to set to the SetParameterMessage.
                /// </summary>
                /// <param name = "message">The message to update with the parameter value to set.</param>
                /// <param name = "value">The parameter value to set.</param>
                /// <returns>Whether the SetParameterMessage needs to be sent.</returns>
                protected bool AddValueToSetParameterMessage(Skyline.DataMiner.Net.Messages.SetParameterMessage message, T value)
                {
                    System.Func<Skyline.DataMiner.Net.Messages.SetParameterMessage, T, System.Boolean> setter;
                    if (Setters.TryGetValue(type, out setter))
                    {
                        return setter(message, value);
                    }
                    else
                    {
                        throw new System.NotSupportedException("Type " + typeof(T) + " is not supported.");
                    }
                }

                /// <summary>
                /// Adds a nullable DateTime value to the message.
                /// </summary>
                /// <param name = "message">The message.</param>
                /// <param name = "value">The value.</param>
                /// <returns><c>true</c> if the set message needs to be executed; otherwise, <c>false</c>.</returns>
                private static bool AddNullableDateTimeValueToSetParameterMessage(Skyline.DataMiner.Net.Messages.SetParameterMessage message, T value)
                {
                    bool executeSet = true;
                    if (!value.Equals(default(T)))
                    {
                        System.DateTime valueToSet = (System.DateTime)System.Convert.ChangeType(value, typeof(System.DateTime), System.Globalization.CultureInfo.CurrentCulture);
                        message.Value = new Skyline.DataMiner.Net.Messages.ParameterValue(valueToSet);
                    }
                    else
                    {
                        executeSet = false;
                    }

                    return executeSet;
                }

                /// <summary>
                /// Adds a nullable double value to the message.
                /// </summary>
                /// <param name = "message">The message.</param>
                /// <param name = "value">The value.</param>
                /// <returns><c>true</c> if the set message needs to be executed; otherwise, <c>false</c>.</returns>
                private static bool AddNullableDoubleValueToSetParameterMessage(Skyline.DataMiner.Net.Messages.SetParameterMessage message, T value)
                {
                    bool executeSet = true;
                    if (!value.Equals(default(T)))
                    {
                        double valueToSet = (double)System.Convert.ChangeType(value, typeof(double), System.Globalization.CultureInfo.CurrentCulture);
                        message.Value = new Skyline.DataMiner.Net.Messages.ParameterValue(valueToSet);
                    }
                    else
                    {
                        executeSet = false;
                    }

                    return executeSet;
                }

                /// <summary>
                /// Adds a nullable int value to the message.
                /// </summary>
                /// <param name = "message">The message.</param>
                /// <param name = "value">The string value.</param>
                /// <returns><c>true</c> if the set message needs to be executed; otherwise, <c>false</c>.</returns>
                private static bool AddNullableIntValueToSetParameterMessage(Skyline.DataMiner.Net.Messages.SetParameterMessage message, T value)
                {
                    bool executeSet = true;
                    if (!value.Equals(default(T)))
                    {
                        int valueToSet = (int)System.Convert.ChangeType(value, typeof(int), System.Globalization.CultureInfo.CurrentCulture);
                        message.Value = new Skyline.DataMiner.Net.Messages.ParameterValue(valueToSet);
                    }
                    else
                    {
                        executeSet = false;
                    }

                    return executeSet;
                }

                /// <summary>
                /// Adds a string value to the message.
                /// </summary>
                /// <param name = "message">The message.</param>
                /// <param name = "value">The string value.</param>
                /// <returns><c>true</c> if the set message needs to be executed; otherwise, <c>false</c>.</returns>
                private static bool AddStringValueToSetParameterMessage(Skyline.DataMiner.Net.Messages.SetParameterMessage message, T value)
                {
                    message.Value = new Skyline.DataMiner.Net.Messages.ParameterValue((string)System.Convert.ChangeType(value, typeof(string), System.Globalization.CultureInfo.CurrentCulture));
                    return true;
                }
            }

            /// <summary>
            /// Represents a standalone parameter.
            /// </summary>
            /// <typeparam name = "T">The type of the standalone parameter.</typeparam>
            /// <remarks>
            /// In case T equals int?, double? or DateTime?, extension methods are available. Refer to <see 
            ///cref = "ExtensionsIDmsStandaloneParameter"/> for more information.
            /// </remarks>
            internal class DmsStandaloneParameter<T> : Skyline.DataMiner.Library.Common.DmsParameter<T>, Skyline.DataMiner.Library.Common.IDmsStandaloneParameter<T>
            {
                /// <summary>
                /// The element this parameter is part of.
                /// </summary>
                private readonly Skyline.DataMiner.Library.Common.IDmsElement element;
                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsStandaloneParameter{T}"/> class.
                /// </summary>
                /// <param name = "element">The element that the parameter belongs to.</param>
                /// <param name = "id">The ID of the parameter.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "element"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentException"><paramref name = "id"/> is invalid.</exception>
                internal DmsStandaloneParameter(Skyline.DataMiner.Library.Common.IDmsElement element, int id): base(id)
                {
                    if (element == null)
                    {
                        throw new System.ArgumentNullException("element");
                    }

                    this.element = element;
                }

                /// <summary>
                /// Gets the element this parameter is part of.
                /// </summary>
                /// <value>The element this parameter is part of.</value>
                public Skyline.DataMiner.Library.Common.IDmsElement Element
                {
                    get
                    {
                        return element;
                    }
                }

                /// <summary>
                /// Sets the value of this parameter.
                /// </summary>
                /// <param name = "value">The value to set.</param>
                /// <exception cref = "ElementStoppedException">The element is stopped.</exception>
                /// <exception cref = "ElementNotFoundException">
                /// The element was not found in the DataMiner System.
                /// </exception>
                public void SetValue(T value)
                {
                    Skyline.DataMiner.Library.Common.HelperClass.CheckElementState(element);
                    Skyline.DataMiner.Net.Messages.SetParameterMessage message = new Skyline.DataMiner.Net.Messages.SetParameterMessage{DataMinerID = element.DmsElementId.AgentId, ElId = element.DmsElementId.ElementId, ParameterId = Id, DisableInformationEventMessage = true};
                    if (AddValueToSetParameterMessage(message, value))
                    {
                        element.Host.Dms.Communication.SendMessage(message);
                    }
                }

                /// <summary>
                /// Returns a string that represents the current object.
                /// </summary>
                /// <returns>A string that represents the current object.</returns>
                public override string ToString()
                {
                    return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Standalone Parameter:{0}", Id);
                }
            }

            /// <summary>
            /// Represents a table.
            /// </summary>
            internal class DmsTable : Skyline.DataMiner.Library.Common.IDmsTable
            {
                /// <summary>
                /// The element this table belongs to.
                /// </summary>
                private readonly Skyline.DataMiner.Library.Common.IDmsElement element;
                /// <summary>
                /// The table parameter ID.
                /// </summary>
                private readonly int id;
                /// <summary>
                /// Initializes a new instance of the <see cref = "DmsTable"/> class.
                /// </summary>
                /// <param name = "element">The element this table belongs to.</param>
                /// <param name = "id">The table parameter ID.</param>
                internal DmsTable(Skyline.DataMiner.Library.Common.IDmsElement element, int id)
                {
                    this.element = element;
                    this.id = id;
                }

                /// <summary>
                /// Gets the element this table is part of.
                /// </summary>
                /// <value>The element this table is part of.</value>
                public Skyline.DataMiner.Library.Common.IDmsElement Element
                {
                    get
                    {
                        return element;
                    }
                }

                /// <summary>
                /// Gets the table parameter ID.
                /// </summary>
                /// <value>The table parameter ID.</value>
                public int Id
                {
                    get
                    {
                        return id;
                    }
                }

                /// <summary>
                /// Gets the specified column.
                /// </summary>
                /// <param name = "parameterId">The parameter ID.</param>
                /// <typeparam name = "T">The type of the column.</typeparam>
                /// <exception cref = "ArgumentException"><paramref name = "parameterId"/> is invalid.</exception>
                /// <exception cref = "NotSupportedException">A type other than string, int?, double? or DateTime? was provided.</exception>
                /// <returns>The standalone parameter that corresponds with the specified ID.</returns>
                public Skyline.DataMiner.Library.Common.IDmsColumn<T> GetColumn<T>(int parameterId)
                {
                    if (parameterId < 1)
                    {
                        throw new System.ArgumentException("Invalid parameter ID.", "parameterId");
                    }

                    System.Type type = typeof(T);
                    if (type != typeof(string) && type != typeof(int? ) && type != typeof(double? ) && type != typeof(System.DateTime? ))
                    {
                        throw new System.NotSupportedException("Only one of the following types is supported: string, int?, double? or DateTime?.");
                    }

                    return new Skyline.DataMiner.Library.Common.DmsColumn<T>(this, parameterId);
                }

                /// <summary>
                /// Returns a string that represents the current object.
                /// </summary>
                /// <returns>A string that represents the current object.</returns>
                public override string ToString()
                {
                    return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Table Parameter:{0}", id);
                }
            }

            /// <summary>
            /// DataMiner table column interface.
            /// </summary>
            public interface IDmsColumn
            {
                /// <summary>
                /// Gets the column parameter ID.
                /// </summary>
                /// <value>The column parameter ID.</value>
                int Id
                {
                    get;
                }

                /// <summary>
                /// Gets the table this column is part of.
                /// </summary>
                /// <value>The table this column is part of.</value>
                Skyline.DataMiner.Library.Common.IDmsTable Table
                {
                    get;
                }
            }

            /// <summary>
            /// DataMiner table column interface of a specific type.
            /// </summary>
            /// <typeparam name = "T">The type of the column.</typeparam>
            public interface IDmsColumn<T> : Skyline.DataMiner.Library.Common.IDmsColumn
            {
                /// <summary>
                /// Sets the value of a cell in a table.
                /// </summary>
                /// <param name = "primaryKey">The primary key of the row.</param>
                /// <param name = "value">The value to set.</param>
                /// <exception cref = "ArgumentNullException">
                /// <paramref name = "primaryKey"/> or <paramref name = "value"/> is <see langword = "null"/>.
                /// </exception>
                /// <exception cref = "ElementNotFoundException">
                /// The element was not found in the DataMiner System.
                /// </exception>
                /// <exception cref = "ElementStoppedException">The element is stopped.</exception>
                void SetValue(string primaryKey, T value);
            }

            /// <summary>
            /// DataMiner standalone parameter interface.
            /// </summary>
            public interface IDmsStandaloneParameter
            {
                /// <summary>
                /// Gets the element this parameter is part of.
                /// </summary>
                /// <value>The element this parameter is part of.</value>
                Skyline.DataMiner.Library.Common.IDmsElement Element
                {
                    get;
                }

                /// <summary>
                /// Gets the ID of this parameter.
                /// </summary>
                /// <value>The ID of this parameter.</value>
                int Id
                {
                    get;
                }
            }

            /// <summary>
            /// DataMiner standalone parameter interface for a parameter of a specific type.
            /// </summary>
            /// <typeparam name = "T">The type of the standalone parameter.</typeparam>
            public interface IDmsStandaloneParameter<T> : Skyline.DataMiner.Library.Common.IDmsStandaloneParameter
            {
                /// <summary>
                /// Sets the value of this parameter.
                /// </summary>
                /// <param name = "value">The value to set.</param>
                /// <exception cref = "ElementStoppedException">The element is stopped.</exception>
                /// <exception cref = "ElementNotFoundException">The element was not found in the DataMiner System.</exception>
                void SetValue(T value);
            }

            /// <summary>
            /// DataMiner table interface.
            /// </summary>
            public interface IDmsTable
            {
                /// <summary>
                /// Gets the element this table is part of.
                /// </summary>
                /// <value>The element this table is part of.</value>
                Skyline.DataMiner.Library.Common.IDmsElement Element
                {
                    get;
                }

                /// <summary>
                /// Gets the table parameter ID.
                /// </summary>
                /// <value>The table parameter ID.</value>
                int Id
                {
                    get;
                }

                /// <summary>
                /// Gets the specified column.
                /// </summary>
                /// <param name = "parameterId">The parameter ID.</param>
                /// <typeparam name = "T">The type of the column.</typeparam>
                /// <exception cref = "ArgumentException"><paramref name = "parameterId"/> is invalid.</exception>
                /// <exception cref = "NotSupportedException">A type other than string, int?, double? or DateTime? was provided.</exception>
                /// <returns>The standalone parameter that corresponds with the specified ID.</returns>
                Skyline.DataMiner.Library.Common.IDmsColumn<T> GetColumn<T>(int parameterId);
            }

            namespace Properties
            {
                /// <summary>
                /// Represents a DMS element property.
                /// </summary>
                internal class DmsElementProperty : Skyline.DataMiner.Library.Common.Properties.DmsProperty<Skyline.DataMiner.Library.Common.Properties.IDmsElementPropertyDefinition>, Skyline.DataMiner.Library.Common.Properties.IDmsElementProperty
                {
                    /// <summary>
                    /// The element to which the property belongs.
                    /// </summary>
                    private readonly Skyline.DataMiner.Library.Common.IDmsElement element;
                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsElementProperty"/> class.
                    /// </summary>
                    /// <param name = "element">The element to which the property is assigned.</param>
                    /// <param name = "definition">The definition of the property.</param>
                    /// <param name = "value">The current value of the property.</param>
                    /// <exception cref = "ArgumentNullException"><paramref name = "element"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "definition"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "value"/> is <see langword = "null"/>.</exception>
                    public DmsElementProperty(Skyline.DataMiner.Library.Common.IDmsElement element, Skyline.DataMiner.Library.Common.Properties.IDmsElementPropertyDefinition definition, string value): base(definition, value)
                    {
                        if (element == null)
                        {
                            throw new System.ArgumentNullException("element");
                        }

                        this.element = element;
                    }

                    /// <summary>
                    /// Gets the element to which the property is assigned.
                    /// </summary>
                    public Skyline.DataMiner.Library.Common.IDmsElement Element
                    {
                        get
                        {
                            return element;
                        }
                    }
                }

                /// <summary>
                /// Represents a DMS property.
                /// </summary>
                internal class DmsProperty<T> : Skyline.DataMiner.Library.Common.Properties.IDmsProperty<T> where T : Skyline.DataMiner.Library.Common.Properties.IDmsPropertyDefinition
                {
                    /// <summary>
                    /// The definition of the property.
                    /// </summary>
                    protected readonly T definition;
                    /// <summary>
                    /// The value of the property.
                    /// </summary>
                    protected string value;
                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsProperty{T}"/> class.
                    /// </summary>
                    /// <param name = "definition">The definition of the property.</param>
                    /// <param name = "value">The current value of the property.</param>
                    /// <exception cref = "ArgumentNullException"><paramref name = "definition"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "value"/> is <see langword = "null"/>.</exception>
                    protected DmsProperty(T definition, string value)
                    {
                        if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(definition, default(T)))
                        {
                            throw new System.ArgumentNullException("definition");
                        }

                        if (value == null)
                        {
                            throw new System.ArgumentNullException("value");
                        }

                        this.definition = definition;
                        this.value = value;
                    }

                    /// <summary>
                    /// Gets the definition of the property.
                    /// </summary>
                    public T Definition
                    {
                        get
                        {
                            return definition;
                        }
                    }

                    /// <summary>
                    /// Gets the value of the property.
                    /// </summary>
                    public string Value
                    {
                        get
                        {
                            return value;
                        }
                    }
                }

                /// <summary>
                /// Entry class for the discrete entries associated with a property definition.
                /// </summary>
                internal class DmsPropertyEntry : Skyline.DataMiner.Library.Common.Properties.IDmsPropertyEntry
                {
                    /// <summary>
                    /// The value of the property.
                    /// </summary>
                    private string value;
                    /// <summary>
                    /// The numeric value attached with this discrete.
                    /// </summary>
                    private int metric;
                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsPropertyEntry"/> class.
                    /// </summary>
                    /// <param name = "value">The display value of the entry.</param>
                    /// <param name = "metric">The internal value of the entry.</param>
                    internal DmsPropertyEntry(string value, int metric)
                    {
                        Value = value;
                        Metric = metric;
                    }

                    /// <summary>
                    /// Gets the value of the property.
                    /// </summary>
                    public string Value
                    {
                        get
                        {
                            return value;
                        }

                        internal set
                        {
                            this.value = value;
                        }
                    }

                    /// <summary>
                    /// Gets the numeric value attached with the discrete.
                    /// </summary>
                    public int Metric
                    {
                        get
                        {
                            return metric;
                        }

                        internal set
                        {
                            metric = value;
                        }
                    }

                    /// <summary>
                    /// Returns a string that represents the current object.
                    /// </summary>
                    /// <returns>A string that represents the current object.</returns>
                    public override string ToString()
                    {
                        return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Property Entry:<{0};{1}>", value, metric);
                    }
                }

                /// <summary>
                /// Represents a DMS property.
                /// </summary>
                internal class DmsViewProperty : Skyline.DataMiner.Library.Common.Properties.DmsProperty<Skyline.DataMiner.Library.Common.Properties.IDmsViewPropertyDefinition>, Skyline.DataMiner.Library.Common.Properties.IDmsViewProperty
                {
                    /// <summary>
                    /// The view to which the property is assigned.
                    /// </summary>
                    private readonly Skyline.DataMiner.Library.Common.IDmsView view;
                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsViewProperty"/> class.
                    /// </summary>
                    /// <param name = "view">The view to which the property is assigned.</param>
                    /// <param name = "definition">The definition of the property.</param>
                    /// <param name = "value">The current value of the property.</param>
                    /// <exception cref = "ArgumentNullException"><paramref name = "definition"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "value"/> is <see langword = "null"/>.</exception>
                    internal DmsViewProperty(Skyline.DataMiner.Library.Common.IDmsView view, Skyline.DataMiner.Library.Common.Properties.IDmsViewPropertyDefinition definition, string value): base(definition, value)
                    {
                        if (view == null)
                        {
                            throw new System.ArgumentNullException("view");
                        }

                        this.view = view;
                    }

                    /// <summary>
                    /// Gets the view to which the property is assigned.
                    /// </summary>
                    public Skyline.DataMiner.Library.Common.IDmsView View
                    {
                        get
                        {
                            return view;
                        }
                    }
                }

                /// <summary>
                /// Represents a writable DataMiner system element property.
                /// </summary>
                internal class DmsWritableElementProperty : Skyline.DataMiner.Library.Common.Properties.DmsElementProperty, Skyline.DataMiner.Library.Common.Properties.IWritableProperty
                {
                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsWritableElementProperty"/> class.
                    /// </summary>
                    /// <param name = "element">The element to which the property is assigned.</param>
                    /// <param name = "definition">The definition of the property.</param>
                    /// <param name = "value">The current value of the property.</param>
                    /// <exception cref = "ArgumentNullException"><paramref name = "element"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "definition"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "value"/> is <see langword = "null"/>.</exception>
                    public DmsWritableElementProperty(Skyline.DataMiner.Library.Common.IDmsElement element, Skyline.DataMiner.Library.Common.Properties.IDmsElementPropertyDefinition definition, string value): base(element, definition, value)
                    {
                    }

                    /// <summary>
                    /// Occurs when a property value changes.
                    /// </summary>
                    public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
                    /// <summary>
                    /// Gets or sets the value of the property.
                    /// </summary>
                    /// <exception cref = "ArgumentException">Thrown when the value can not be added to the property.</exception>
                    public new string Value
                    {
                        get
                        {
                            return value;
                        }

                        set
                        {
                            if (!definition.IsValidInput(value))
                            {
                                throw new System.ArgumentException(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "The value:'{0}' is not valid for the property", value));
                            }

                            this.value = value;
                            NotifyPropertyChanged();
                        }
                    }

                    private void NotifyPropertyChanged()
                    {
                        System.ComponentModel.PropertyChangedEventHandler handler = PropertyChanged;
                        if (handler != null)
                        {
                            handler.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(definition.Name));
                        }
                    }
                }

                /// <summary>
                /// Represents a DMS property.
                /// </summary>
                internal class DmsWritableViewProperty : Skyline.DataMiner.Library.Common.Properties.DmsViewProperty, Skyline.DataMiner.Library.Common.Properties.IWritableProperty
                {
                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsWritableViewProperty"/> class.
                    /// </summary>
                    /// <param name = "view">The view to which this property belongs.</param>
                    /// <param name = "definition">The definition of the property.</param>
                    /// <param name = "value">The current value of the property.</param>
                    /// <exception cref = "ArgumentNullException"><paramref name = "view"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "definition"/> is <see langword = "null"/>.</exception>
                    /// <exception cref = "ArgumentNullException"><paramref name = "value"/> is <see langword = "null"/>.</exception>
                    public DmsWritableViewProperty(Skyline.DataMiner.Library.Common.IDmsView view, Skyline.DataMiner.Library.Common.Properties.IDmsViewPropertyDefinition definition, string value): base(view, definition, value)
                    {
                    }

                    /// <summary>
                    /// Occurs when the value of a property changes.
                    /// </summary>
                    public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
                    /// <summary>
                    /// Gets or sets the value of the property.
                    /// </summary>
                    /// <exception cref = "ArgumentException">Thrown when the value can not be added to the property.</exception>
                    public new string Value
                    {
                        get
                        {
                            return value;
                        }

                        set
                        {
                            if (!definition.IsValidInput(value))
                            {
                                throw new System.ArgumentException(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "The value:'{0}' is not valid for the property", value));
                            }

                            this.value = value;
                            NotifyPropertyChanged();
                        }
                    }

                    private void NotifyPropertyChanged()
                    {
                        System.ComponentModel.PropertyChangedEventHandler handler = PropertyChanged;
                        if (handler != null)
                        {
                            handler.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(definition.Name));
                        }
                    }
                }

                /// <summary>
                /// DataMiner element property interface.
                /// </summary>
                public interface IDmsElementProperty : Skyline.DataMiner.Library.Common.Properties.IDmsProperty<Skyline.DataMiner.Library.Common.Properties.IDmsElementPropertyDefinition>
                {
                    /// <summary>
                    /// Gets the element this property belongs to.
                    /// </summary>
                    Skyline.DataMiner.Library.Common.IDmsElement Element
                    {
                        get;
                    }
                }

                /// <summary>
                /// DataMiner property interface.
                /// </summary>
                /// <typeparam name = "T">The property type.</typeparam>
                public interface IDmsProperty<out T>
                    where T : Skyline.DataMiner.Library.Common.Properties.IDmsPropertyDefinition
                {
                    /// <summary>
                    /// Gets the property value.
                    /// </summary>
                    string Value
                    {
                        get;
                    }

                    /// <summary>
                    /// Gets the property definition.
                    /// </summary>
                    T Definition
                    {
                        get;
                    }
                }

                /// <summary>
                /// DataMiner property entry interface.
                /// </summary>
                public interface IDmsPropertyEntry
                {
                    /// <summary>
                    /// Gets the internal value.
                    /// </summary>
                    int Metric
                    {
                        get;
                    }

                    /// <summary>
                    /// Gets the value.
                    /// </summary>
                    string Value
                    {
                        get;
                    }
                }

                /// <summary>
                /// DataMiner service property interface.
                /// </summary>
                public interface IDmsServiceProperty : Skyline.DataMiner.Library.Common.Properties.IDmsProperty<Skyline.DataMiner.Library.Common.Properties.IDmsServicePropertyDefinition>
                {
                    /// <summary>
                    /// Gets the service this property belongs to.
                    /// </summary>
                    Skyline.DataMiner.Library.Common.IDmsService Service
                    {
                        get;
                    }
                }

                /// <summary>
                /// DataMiner view property interface.
                /// </summary>
                public interface IDmsViewProperty : Skyline.DataMiner.Library.Common.Properties.IDmsProperty<Skyline.DataMiner.Library.Common.Properties.IDmsViewPropertyDefinition>
                {
                    /// <summary>
                    /// Gets the view this property belongs to.
                    /// </summary>
                    Skyline.DataMiner.Library.Common.IDmsView View
                    {
                        get;
                    }
                }

                /// <summary>
                /// DataMiner writable property interface.
                /// </summary>
                public interface IWritableProperty : System.ComponentModel.INotifyPropertyChanged
                {
                    /// <summary>
                    /// Gets or sets the property value.
                    /// </summary>
                    string Value
                    {
                        get;
                        set;
                    }
                }

                /// <summary>
                /// Represents a DMS element property definition.
                /// </summary>
                internal class DmsElementPropertyDefinition : Skyline.DataMiner.Library.Common.Properties.DmsPropertyDefinition, Skyline.DataMiner.Library.Common.Properties.IDmsElementPropertyDefinition
                {
                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsElementPropertyDefinition"/> class.
                    /// </summary>
                    /// <param name = "dms">Instance of the DMS.</param>
                    /// <param name = "config">The configuration received from SLNet.</param>
                    internal DmsElementPropertyDefinition(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Net.Messages.PropertyConfig config): base(dms, config)
                    {
                    }

                    /// <summary>
                    /// Specifies if the object exists in the DataMiner System.
                    /// </summary>
                    /// <returns><c>true</c> if the element property exists in the DataMiner System; otherwise, <c>false</c>.</returns>
                    public override bool Exists()
                    {
                        return Dms.PropertyExists(name, Skyline.DataMiner.Library.Common.PropertyType.Element);
                    }

                    /// <summary>
                    /// Returns a string that represents the current object.
                    /// </summary>
                    /// <returns>A string that represents the current object.</returns>
                    public override string ToString()
                    {
                        return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Element property: {0}", name);
                    }
                }

                /// <summary>
                /// Parent class for all types of DMS properties definitions.
                /// </summary>
                internal abstract class DmsPropertyDefinition : Skyline.DataMiner.Library.Common.Properties.IDmsPropertyDefinition
                {
                    /// <summary>
                    /// Instance of the DMS class.
                    /// </summary>
                    protected readonly Skyline.DataMiner.Library.Common.IDms dms;
                    /// <summary>
                    /// The name of the property.
                    /// </summary>
                    protected string name;
                    /// <summary>
                    /// The id of the property.
                    /// </summary>
                    protected int id;
                    /// <summary>
                    /// Specifies if the property is available for alarm filtering.
                    /// </summary>
                    protected bool isAvailableForAlarmFiltering;
                    /// <summary>
                    /// Specifies if the property is read only.
                    /// </summary>
                    protected bool isReadOnly;
                    /// <summary>
                    /// Specifies if the property is visible in the Surveyor.
                    /// </summary>
                    protected bool isVisibleInSurveyor;
                    /// <summary>
                    /// The regular expression.
                    /// </summary>
                    protected string regex;
                    /// <summary>
                    /// The associated discrete entries with the property.
                    /// </summary>
                    protected System.Collections.Generic.List<Skyline.DataMiner.Library.Common.Properties.IDmsPropertyEntry> entries;
                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsPropertyDefinition"/> class.
                    /// </summary>
                    /// <param name = "dms">Instance of DMS.</param>
                    /// <param name = "config">The property configuration received from SLNet.</param>
                    protected DmsPropertyDefinition(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Net.Messages.PropertyConfig config)
                    {
                        if (dms == null)
                        {
                            throw new System.ArgumentNullException("dms");
                        }

                        if (config == null)
                        {
                            throw new System.ArgumentNullException("config");
                        }

                        this.dms = dms;
                        Parse(config);
                    }

                    /// <summary>
                    /// Gets the name of the property.
                    /// </summary>
                    public string Name
                    {
                        get
                        {
                            return name;
                        }
                    }

                    /// <summary>
                    /// Gets the ID of the property.
                    /// </summary>
                    public int Id
                    {
                        get
                        {
                            return id;
                        }
                    }

                    /// <summary>
                    /// Gets a value indicating whether the property is available for alarm filtering.
                    /// </summary>
                    public bool IsAvailableForAlarmFiltering
                    {
                        get
                        {
                            return isAvailableForAlarmFiltering;
                        }
                    }

                    /// <summary>
                    /// Gets a value indicating whether the property is read only or not.
                    /// </summary>
                    public bool IsReadOnly
                    {
                        get
                        {
                            return isReadOnly;
                        }
                    }

                    /// <summary>
                    /// Gets a value indicating whether or not the property is visible in the surveyor.
                    /// </summary>
                    public bool IsVisibleInSurveyor
                    {
                        get
                        {
                            return isVisibleInSurveyor;
                        }
                    }

                    /// <summary>
                    /// Gets the regular expression of the property.
                    /// </summary>
                    public string Regex
                    {
                        get
                        {
                            return regex;
                        }
                    }

                    /// <summary>
                    /// Gets the discrete entries associated with the property.
                    /// </summary>
                    public System.Collections.ObjectModel.ReadOnlyCollection<Skyline.DataMiner.Library.Common.Properties.IDmsPropertyEntry> Entries
                    {
                        get
                        {
                            return entries.AsReadOnly();
                        }
                    }

                    /// <summary>
                    /// Gets the DMS instance.
                    /// </summary>
                    internal Skyline.DataMiner.Library.Common.IDms Dms
                    {
                        get
                        {
                            return dms;
                        }
                    }

                    /// <summary>
                    /// Determines whether the object exists in the DataMiner System.
                    /// </summary>
                    /// <returns><c>true</c> if the property exists in the DataMiner System; otherwise, <c>false</c>.</returns>
                    public abstract bool Exists();
                    /// <summary>
                    /// Checks if the provided input value matches the definition of the property.
                    /// </summary>
                    /// <param name = "value">The input value.</param>
                    /// <returns><c>true</c> if the input is valid; otherwise, <c>false</c>.</returns>
                    public bool IsValidInput(string value)
                    {
                        if (!System.String.IsNullOrWhiteSpace(regex))
                        {
                            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(regex);
                            return r.Match(value).Success;
                        }

                        return true;
                    }

                    /// <summary>
                    /// Parses the SLNet object.
                    /// </summary>
                    /// <param name = "config">Property configuration object.</param>
                    internal void Parse(Skyline.DataMiner.Net.Messages.PropertyConfig config)
                    {
                        name = config.Name;
                        id = config.ID;
                        isAvailableForAlarmFiltering = config.IsFilterEnabled;
                        isReadOnly = config.IsReadOnly;
                        isVisibleInSurveyor = config.IsVisibleInSurveyor;
                        regex = config.RegEx;
                        if (config.Entries != null)
                        {
                            Skyline.DataMiner.Net.Messages.PropertyConfigEntry[] propertyConfigEntries = config.Entries;
                            entries = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.Properties.IDmsPropertyEntry>(propertyConfigEntries.Length);
                            foreach (Skyline.DataMiner.Net.Messages.PropertyConfigEntry entry in propertyConfigEntries)
                            {
                                Skyline.DataMiner.Library.Common.Properties.DmsPropertyEntry temp = new Skyline.DataMiner.Library.Common.Properties.DmsPropertyEntry(entry.Value, entry.Metric);
                                entries.Add(temp);
                            }
                        }
                        else
                        {
                            entries = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.Properties.IDmsPropertyEntry>();
                        }
                    }
                }

                internal class DmsServicePropertyDefinition : Skyline.DataMiner.Library.Common.Properties.DmsPropertyDefinition, Skyline.DataMiner.Library.Common.Properties.IDmsServicePropertyDefinition
                {
                    /// <summary>
                    ///     Initializes a new instance of the <see cref = "DmsServicePropertyDefinition"/> class.
                    /// </summary>
                    /// <param name = "dms">Instance of the DMS.</param>
                    /// <param name = "config">The configuration received from SLNet.</param>
                    internal DmsServicePropertyDefinition(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Net.Messages.PropertyConfig config): base(dms, config)
                    {
                    }

                    /// <summary>
                    ///     Returns a value indicating whether the service property exists in the DataMiner System.
                    /// </summary>
                    /// <returns><c>true</c> if the service property exists in the DataMiner System; otherwise, <c>false</c>.</returns>
                    public override bool Exists()
                    {
                        return this.Dms.PropertyExists(this.Name, Skyline.DataMiner.Library.Common.PropertyType.Service);
                    }

                    /// <summary>
                    ///     Returns a string that represents the current object.
                    /// </summary>
                    /// <returns>A string that represents the current object.</returns>
                    public override string ToString()
                    {
                        return string.Format(System.Globalization.CultureInfo.InvariantCulture, "Service property: {0}", this.Name);
                    }
                }

                /// <summary>
                /// Represents a DMS view property definitions.
                /// </summary>
                internal class DmsViewPropertyDefinition : Skyline.DataMiner.Library.Common.Properties.DmsPropertyDefinition, Skyline.DataMiner.Library.Common.Properties.IDmsViewPropertyDefinition
                {
                    /// <summary>
                    /// Initializes a new instance of the <see cref = "DmsViewPropertyDefinition"/> class.
                    /// </summary>
                    /// <param name = "dms">Instance of the DMS.</param>
                    /// <param name = "config">The configuration received from SLNet.</param>
                    internal DmsViewPropertyDefinition(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Net.Messages.PropertyConfig config): base(dms, config)
                    {
                    }

                    /// <summary>
                    /// Returns a value indicating whether the view property exists in the DataMiner System.
                    /// </summary>
                    /// <returns><c>true</c> if the view property exists in the DataMiner System; otherwise, <c>false</c>.</returns>
                    public override bool Exists()
                    {
                        return Dms.PropertyExists(Name, Skyline.DataMiner.Library.Common.PropertyType.View);
                    }

                    /// <summary>
                    /// Returns a string that represents the current object.
                    /// </summary>
                    /// <returns>A string that represents the current object.</returns>
                    public override string ToString()
                    {
                        return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "View property: {0}", Name);
                    }
                }

                /// <summary>
                /// DataMiner element property definition interface.
                /// </summary>
                public interface IDmsElementPropertyDefinition : Skyline.DataMiner.Library.Common.Properties.IDmsPropertyDefinition
                {
                }

                /// <summary>
                /// DataMiner property definition interface.
                /// </summary>
                public interface IDmsPropertyDefinition : Skyline.DataMiner.Library.Common.IDmsObject
                {
                    /// <summary>
                    /// Gets the property name.
                    /// </summary>
                    string Name
                    {
                        get;
                    }

                    /// <summary>
                    /// Gets the property entries.
                    /// </summary>
                    System.Collections.ObjectModel.ReadOnlyCollection<Skyline.DataMiner.Library.Common.Properties.IDmsPropertyEntry> Entries
                    {
                        get;
                    }

                    /// <summary>
                    /// Gets the property ID.
                    /// </summary>
                    int Id
                    {
                        get;
                    }

                    /// <summary>
                    /// Gets a value indicating whether the property is available for alarm filtering.
                    /// </summary>
                    bool IsAvailableForAlarmFiltering
                    {
                        get;
                    }

                    /// <summary>
                    /// Gets a value indicating whether the property is read-only.
                    /// </summary>
                    bool IsReadOnly
                    {
                        get;
                    }

                    /// <summary>
                    /// Gets a value indicating whether the property is visible in the Surveyor.
                    /// </summary>
                    bool IsVisibleInSurveyor
                    {
                        get;
                    }

                    /// <summary>
                    /// Gets the regular expression the property value must conform to.
                    /// </summary>
                    string Regex
                    {
                        get;
                    }

                    /// <summary>
                    /// Checks if the provided input value matches the definition of the property.
                    /// </summary>
                    /// <param name = "value">The input value.</param>
                    /// <returns><c>true</c> if the input is valid; otherwise, <c>false</c>.</returns>
                    bool IsValidInput(string value);
                }

                /// <summary>
                /// DataMiner service property definition interface.
                /// </summary>
                public interface IDmsServicePropertyDefinition : Skyline.DataMiner.Library.Common.Properties.IDmsPropertyDefinition
                {
                }

                /// <summary>
                /// DataMiner view property definition interface.
                /// </summary>
                public interface IDmsViewPropertyDefinition : Skyline.DataMiner.Library.Common.Properties.IDmsPropertyDefinition
                {
                }
            }

            /// <summary>
            /// Property collection interface.
            /// </summary>
            /// <typeparam name = "TProperty">The property type.</typeparam>
            /// <typeparam name = "TPropertyDefinition">The property definition type.</typeparam>
            public interface IPropertyCollection<TProperty, TPropertyDefinition> : System.Collections.Generic.IEnumerable<TProperty> where TProperty : Skyline.DataMiner.Library.Common.Properties.IDmsProperty<TPropertyDefinition> where TPropertyDefinition : Skyline.DataMiner.Library.Common.Properties.IDmsPropertyDefinition
            {
                /// <summary>
                /// Gets the number of properties in this collection.
                /// </summary>
                /// <value>The number of properties in this collection.</value>
                int Count
                {
                    get;
                }

                /// <summary>
                /// Gets the property associated with the specified name.
                /// </summary>
                /// <param name = "property">The name of the property.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "property"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentOutOfRangeException">An invalid value that is not a member of the set of values.</exception>
                /// <returns>The property.</returns>
                TProperty this[string property]
                {
                    get;
                }
            }

            /// <summary>
            /// Property configuration collection interface.
            /// </summary>
            public interface IPropertConfigurationCollection : System.Collections.Generic.IEnumerable<Skyline.DataMiner.Library.Common.PropertyConfiguration>
            {
                /// <summary>
                /// Gets the number of properties in this collection.
                /// </summary>
                int Count
                {
                    get;
                }

                /// <summary>
                /// Gets the property configuration associated with the specified name.
                /// </summary>
                /// <param name = "property">The name of the property.</param>
                /// <returns>The property configuration.</returns>
                Skyline.DataMiner.Library.Common.PropertyConfiguration this[string property]
                {
                    get;
                }
            }

            /// <summary>
            /// Property definition collection interface.
            /// </summary>
            /// <typeparam name = "T">The property definition type.</typeparam>
            public interface IPropertyDefinitionCollection<out T> : System.Collections.Generic.IEnumerable<T> where T : Skyline.DataMiner.Library.Common.Properties.IDmsPropertyDefinition
            {
                /// <summary>
                /// Gets the number of property definitions in this collection.
                /// </summary>
                int Count
                {
                    get;
                }

                /// <summary>
                /// Gets the property definition associated with the specified name.
                /// </summary>
                /// <param name = "property">The name of the property.</param>
                /// <returns>The property definition.</returns>
                T this[string property]
                {
                    get;
                }
            }

            internal class PropertyCollection<T, U> : Skyline.DataMiner.Library.Common.IPropertyCollection<T, U> where T : Skyline.DataMiner.Library.Common.Properties.IDmsProperty<U> where U : Skyline.DataMiner.Library.Common.Properties.IDmsPropertyDefinition
            {
                private readonly System.Collections.Generic.ICollection<T> collection = new System.Collections.Generic.List<T>();
                /// <summary>
                /// Initializes a new instance of the <see cref = "PropertyCollection&lt;T, U&gt;"/> class.
                /// </summary>
                /// <param name = "properties">The properties to initialize the collection with.</param>
                public PropertyCollection(System.Collections.Generic.IDictionary<System.String, T> properties)
                {
                    foreach (T value in properties.Values)
                    {
                        collection.Add(value);
                    }
                }

                /// <summary>
                /// Gets the number of properties in the collection.
                /// </summary>
                /// <value>The number of properties in this collection.</value>
                public int Count
                {
                    get
                    {
                        return collection.Count;
                    }
                }

                /// <summary>
                /// Gets the item at the specified index.
                /// </summary>
                /// <param name = "index">The name of the property.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "index"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentOutOfRangeException">An invalid value that is not a member of the set of values.</exception>
                /// <returns>The property with the specified name.</returns>
                public T this[string index]
                {
                    get
                    {
                        if (index == null)
                        {
                            throw new System.ArgumentNullException("index");
                        }

                        T property = System.Linq.Enumerable.SingleOrDefault(collection, p => p.Definition.Name.Equals(index, System.StringComparison.OrdinalIgnoreCase));
                        if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(property, default(T)))
                        {
                            throw new System.ArgumentOutOfRangeException("index");
                        }
                        else
                        {
                            return property;
                        }
                    }
                }

                /// <summary>
                /// Returns an enumerator that iterates through a collection.
                /// </summary>
                /// <returns>An enumerator that can be used to iterate through the collection.</returns>
                public System.Collections.Generic.IEnumerator<T> GetEnumerator()
                {
                    return collection.GetEnumerator();
                }

                /// <summary>
                /// Returns an enumerator that iterates through a collection.
                /// </summary>
                /// <returns>An <see cref = "IEnumerator"/> object that can be used to iterate through the collection.</returns>
                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
                {
                    return ((System.Collections.IEnumerable)collection).GetEnumerator();
                }
            }

            /// <summary>
            /// Represents a property configuration collection.
            /// </summary>
            internal class PropertyConfigurationCollection : Skyline.DataMiner.Library.Common.IPropertConfigurationCollection
            {
                private readonly System.Collections.Generic.ICollection<Skyline.DataMiner.Library.Common.PropertyConfiguration> collection = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.PropertyConfiguration>();
                /// <summary>
                /// Initializes a new instance of the <see cref = "PropertyConfigurationCollection"/> class.
                /// </summary>
                /// <param name = "properties">The available property configurations.</param>
                public PropertyConfigurationCollection(System.Collections.Generic.IDictionary<System.String, Skyline.DataMiner.Library.Common.PropertyConfiguration> properties)
                {
                    foreach (Skyline.DataMiner.Library.Common.PropertyConfiguration value in properties.Values)
                    {
                        collection.Add(value);
                    }
                }

                /// <summary>
                /// Gets the amount of configurations in the collection.
                /// </summary>
                public int Count
                {
                    get
                    {
                        return collection.Count;
                    }
                }

                /// <summary>
                /// Gets the configuration based on the name.
                /// </summary>
                /// <param name = "propertyName">The name of the configuration.</param>
                /// <returns>The matching configuration object.</returns>
                public Skyline.DataMiner.Library.Common.PropertyConfiguration this[string propertyName]
                {
                    get
                    {
                        if (propertyName == null)
                        {
                            throw new System.ArgumentNullException("propertyName");
                        }

                        Skyline.DataMiner.Library.Common.PropertyConfiguration property = System.Linq.Enumerable.SingleOrDefault(collection, p => p.Definition.Name.Equals(propertyName, System.StringComparison.OrdinalIgnoreCase));
                        if (property == null)
                        {
                            throw new System.ArgumentOutOfRangeException("propertyName");
                        }
                        else
                        {
                            return property;
                        }
                    }
                }

                /// <summary>
                /// Returns a reference to an enumerator object which is used to iterate over the collection.
                /// </summary>
                /// <returns>Returns a reference to an enumerator object, which is used to iterate over a Collection object.</returns>
                public System.Collections.Generic.IEnumerator<Skyline.DataMiner.Library.Common.PropertyConfiguration> GetEnumerator()
                {
                    return collection.GetEnumerator();
                }

                /// <summary>
                /// Returns a reference to an enumerator object which is used to iterate over the collection.
                /// </summary>
                /// <returns>Returns a reference to an enumerator object, which is used to iterate over a Collection object.</returns>
                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
                {
                    return ((System.Collections.IEnumerable)collection).GetEnumerator();
                }
            }

            /// <summary>
            /// Represents a collection of property definitions.
            /// </summary>
            /// <typeparam name = "T">The property type.</typeparam>
            internal class PropertyDefinitionCollection<T> : Skyline.DataMiner.Library.Common.IPropertyDefinitionCollection<T> where T : Skyline.DataMiner.Library.Common.Properties.IDmsPropertyDefinition
            {
                private readonly System.Collections.Generic.ICollection<T> collection = new System.Collections.Generic.List<T>();
                /// <summary>
                /// Initializes a new instance of the <see cref = "PropertyDefinitionCollection&lt;T&gt;"/> class.
                /// </summary>
                /// <param name = "properties">The properties to initialize the collection with.</param>
                public PropertyDefinitionCollection(System.Collections.Generic.IDictionary<System.String, T> properties)
                {
                    foreach (T value in properties.Values)
                    {
                        collection.Add(value);
                    }
                }

                /// <summary>
                /// Gets tne number of items in the collection.
                /// </summary>
                public int Count
                {
                    get
                    {
                        return collection.Count;
                    }
                }

                /// <summary>
                /// Gets the item at the specified index.
                /// </summary>
                /// <param name = "index">The name of the property.</param>
                /// <exception cref = "ArgumentNullException"><paramref name = "index"/> is <see langword = "null"/>.</exception>
                /// <exception cref = "ArgumentOutOfRangeException">An invalid value that is not a member of the set of values.</exception>
                /// <returns>The property with the specified name.</returns>
                public T this[string index]
                {
                    get
                    {
                        if (index == null)
                        {
                            throw new System.ArgumentNullException("index");
                        }

                        T property = System.Linq.Enumerable.SingleOrDefault(collection, p => p.Name.Equals(index, System.StringComparison.OrdinalIgnoreCase));
                        if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(property, default(T)))
                        {
                            throw new System.ArgumentOutOfRangeException("index");
                        }
                        else
                        {
                            return property;
                        }
                    }
                }

                /// <summary>
                /// Returns an enumerator that iterates through a collection.
                /// </summary>
                /// <returns>An enumerator that can be used to iterate through the collection.</returns>
                public System.Collections.Generic.IEnumerator<T> GetEnumerator()
                {
                    return collection.GetEnumerator();
                }

                /// <summary>
                /// Returns an enumerator that iterates through a collection.
                /// </summary>
                /// <returns>An <see cref = "IEnumerator"/> object that can be used to iterate through the collection.</returns>
                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
                {
                    return ((System.Collections.IEnumerable)collection).GetEnumerator();
                }
            }

            internal static class Logger
            {
                private const long SizeLimit = 3 * 1024 * 1024;
                private const string LogFileName = @"C:\Skyline DataMiner\logging\ClassLibrary.txt";
                private const string LogPositionPlaceholder = "**********";
                private const int PlaceHolderSize = 10;
                private static long logPositionPlaceholderStart = -1;
                private static System.Threading.Mutex loggerMutex;
#pragma warning disable S3963 // "static" fields should be initialized inline

                static Logger()
                {
                    System.Security.AccessControl.MutexSecurity mutexSecurity = new System.Security.AccessControl.MutexSecurity();
                    var accessRule = new System.Security.AccessControl.MutexAccessRule(new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null), System.Security.AccessControl.MutexRights.Synchronize | System.Security.AccessControl.MutexRights.Modify, System.Security.AccessControl.AccessControlType.Allow);
                    mutexSecurity.AddAccessRule(accessRule);
                    bool createdNew;
                    loggerMutex = new System.Threading.Mutex(false, "clpMutex", out createdNew, mutexSecurity);
                }

#pragma warning restore S3963 // "static" fields should be initialized inline

                public static void Log(string message)
                {
                    try
                    {
                        loggerMutex.WaitOne();
                        string logPrefix = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "|";
                        long messageByteCount = System.Text.Encoding.UTF8.GetByteCount(message);
                        // Safeguard for large messages.
                        if (messageByteCount > SizeLimit)
                        {
                            message = "WARNING: message \"" + message.Substring(0, 100) + " not logged as it is too large (over " + SizeLimit + " bytes).";
                        }

                        long limit = SizeLimit / 2; // Safeguard: limit messages. If safeguard removed, the limit would be: SizeLimit - placeholder size - prefix length - 4 (2 * CR LF).
                        if (messageByteCount > limit)
                        {
                            long overhead = messageByteCount - limit;
                            int partToRemove = (int)overhead / 4; // In worst case, each char takes 4 bytes.
                            if (partToRemove == 0)
                            {
                                partToRemove = 1;
                            }

                            while (messageByteCount > limit)
                            {
                                message = message.Substring(0, message.Length - partToRemove);
                                messageByteCount = System.Text.Encoding.UTF8.GetByteCount(message);
                            }
                        }

                        int byteCount = System.Text.Encoding.UTF8.GetByteCount(message);
                        long positionOfPlaceHolder = GetPlaceHolderPosition();
                        System.IO.Stream fileStream = null;
                        try
                        {
                            fileStream = new System.IO.FileStream(LogFileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite);
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileStream))
                            {
                                fileStream = null;
                                if (positionOfPlaceHolder == -1)
                                {
                                    sw.BaseStream.Position = 0;
                                    sw.Write(logPrefix);
                                    sw.WriteLine(message);
                                    logPositionPlaceholderStart = byteCount + logPrefix.Length;
                                    sw.WriteLine(LogPositionPlaceholder);
                                }
                                else
                                {
                                    sw.BaseStream.Position = positionOfPlaceHolder;
                                    if (positionOfPlaceHolder + byteCount + 4 + PlaceHolderSize > SizeLimit)
                                    {
                                        // Overwrite previous placeholder.
                                        byte[] placeholder = System.Text.Encoding.UTF8.GetBytes("          ");
                                        sw.BaseStream.Write(placeholder, 0, placeholder.Length);
                                        sw.BaseStream.Position = 0;
                                    }

                                    sw.Write(logPrefix);
                                    sw.WriteLine(message);
                                    sw.Flush();
                                    logPositionPlaceholderStart = sw.BaseStream.Position;
                                    sw.WriteLine(LogPositionPlaceholder);
                                }
                            }
                        }
                        finally
                        {
                            if (fileStream != null)
                            {
                                fileStream.Dispose();
                            }
                        }
                    }
                    catch
                    {
                    // Do nothing.
                    }
                    finally
                    {
                        loggerMutex.ReleaseMutex();
                    }
                }

                private static long SetToStartOfLine(System.IO.StreamReader streamReader, long startPosition)
                {
                    System.IO.Stream stream = streamReader.BaseStream;
                    for (long position = startPosition - 1; position > 0; position--)
                    {
                        stream.Position = position;
                        if (stream.ReadByte() == '\n')
                        {
                            return position + 1;
                        }
                    }

                    return 0;
                }

                private static long GetPlaceHolderPosition()
                {
                    long result = -1;
                    System.IO.Stream fileStream = null;
                    try
                    {
                        fileStream = System.IO.File.Open(LogFileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
                        using (System.IO.StreamReader streamReader = new System.IO.StreamReader(fileStream))
                        {
                            fileStream = null;
                            streamReader.DiscardBufferedData();
                            long startOfLinePosition = SetToStartOfLine(streamReader, logPositionPlaceholderStart);
                            streamReader.DiscardBufferedData();
                            streamReader.BaseStream.Position = startOfLinePosition;
                            string line;
                            long postionInFile = startOfLinePosition;
                            while ((line = streamReader.ReadLine()) != null)
                            {
                                if (line == LogPositionPlaceholder)
                                {
                                    streamReader.DiscardBufferedData();
                                    result = postionInFile;
                                    break;
                                }
                                else
                                {
                                    postionInFile = postionInFile + System.Text.Encoding.UTF8.GetByteCount(line) + 2;
                                }
                            }

                            // If this point is reached, it means the placeholder was still not found.
                            if (result == -1 && startOfLinePosition > 0)
                            {
                                streamReader.DiscardBufferedData();
                                streamReader.BaseStream.Position = 0;
                                while ((line = streamReader.ReadLine()) != null)
                                {
                                    if (line == LogPositionPlaceholder)
                                    {
                                        streamReader.DiscardBufferedData();
                                        result = streamReader.BaseStream.Position - PlaceHolderSize - 2;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (fileStream != null)
                        {
                            fileStream.Dispose();
                        }
                    }

                    return result;
                }
            }
        }
    }

    namespace DeveloperCommunityLibrary.InteractiveAutomationToolkit
    {
        /// <summary>
        ///     Event loop of the interactive Automation script.
        /// </summary>
        public class InteractiveController
        {
            private bool isManualModeRequested;
            private System.Action manualAction;
            private Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Dialog nextDialog;
            /// <summary>
            ///     Initializes a new instance of the <see cref = "InteractiveController"/> class.
            ///     This object will manage the event loop of the interactive Automation script.
            /// </summary>
            /// <param name = "engine">Link with the SLAutomation process.</param>
            /// <exception cref = "ArgumentNullException">When engine is null.</exception>
            public InteractiveController(Skyline.DataMiner.Automation.IEngine engine)
            {
                if (engine == null)
                {
                    throw new System.ArgumentNullException("engine");
                }

                Engine = engine;
            }

            /// <summary>
            ///     Gets the dialog that is shown to the user.
            /// </summary>
            public Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Dialog CurrentDialog
            {
                get;
                private set;
            }

            /// <summary>
            ///     Gets the link to the SLManagedAutomation process.
            /// </summary>
            public Skyline.DataMiner.Automation.IEngine Engine
            {
                get;
                private set;
            }

            /// <summary>
            ///     Gets a value indicating whether the event loop is updated manually or automatically.
            /// </summary>
            public bool IsManualMode
            {
                get;
                private set;
            }

            /// <summary>
            ///     Gets a value indicating whether the event loop has been started.
            /// </summary>
            public bool IsRunning
            {
                get;
                private set;
            }

            /// <summary>
            ///     Starts the application event loop.
            ///     Updates the displayed dialog after each user interaction.
            ///     Only user interaction on widgets with the WantsOnChange property set to true will cause updates.
            ///     Use <see cref = "RequestManualMode"/> if you want to manually control when the dialog is updated.
            /// </summary>
            /// <param name = "startDialog">Dialog to be shown first.</param>
            public void Run(Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Dialog startDialog)
            {
                if (startDialog == null)
                {
                    throw new System.ArgumentNullException("startDialog");
                }

                nextDialog = startDialog;
                if (IsRunning)
                {
                    throw new System.InvalidOperationException("Already running");
                }

                IsRunning = true;
                while (true)
                {
                    try
                    {
                        if (isManualModeRequested)
                        {
                            RunManualAction();
                        }
                        else
                        {
                            CurrentDialog = nextDialog;
                            CurrentDialog.Show();
                        }
                    }
                    catch (System.Exception)
                    {
                        IsRunning = false;
                        IsManualMode = false;
                        throw;
                    }
                }
            }

            private void RunManualAction()
            {
                isManualModeRequested = false;
                IsManualMode = true;
                manualAction();
                IsManualMode = false;
            }
        }

        internal static class UiResultsExtensions
        {
            public static bool GetChecked(this Skyline.DataMiner.Automation.UIResults uiResults, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.CheckBox checkBox)
            {
                return uiResults.GetChecked(checkBox.DestVar);
            }

            public static string GetString(this Skyline.DataMiner.Automation.UIResults uiResults, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.InteractiveWidget interactiveWidget)
            {
                return uiResults.GetString(interactiveWidget.DestVar);
            }

            public static bool WasButtonPressed(this Skyline.DataMiner.Automation.UIResults uiResults, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Button button)
            {
                return uiResults.WasButtonPressed(button.DestVar);
            }

            public static bool WasCollapseButtonPressed(this Skyline.DataMiner.Automation.UIResults uiResults, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.CollapseButton button)
            {
                return uiResults.WasButtonPressed(button.DestVar);
            }

            public static System.Collections.Generic.IEnumerable<System.String> GetExpandedItemKeys(this Skyline.DataMiner.Automation.UIResults uiResults, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.TreeView treeView)
            {
                string[] expandedItems = uiResults.GetExpanded(treeView.DestVar);
                if (expandedItems == null)
                    return new string[0];
                return System.Linq.Enumerable.ToList(System.Linq.Enumerable.Where(expandedItems, x => !System.String.IsNullOrWhiteSpace(x)));
            }

            public static System.Collections.Generic.IEnumerable<System.String> GetCheckedItemKeys(this Skyline.DataMiner.Automation.UIResults uiResults, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.TreeView treeView)
            {
                string result = uiResults.GetString(treeView.DestVar);
                if (System.String.IsNullOrEmpty(result))
                    return new string[0];
                return result.Split(new char[]{';'}, System.StringSplitOptions.RemoveEmptyEntries);
            }
        }

        /// <summary>
        ///     A button that can be pressed.
        /// </summary>
        public class Button : Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.InteractiveWidget
        {
            private bool pressed;
            /// <summary>
            ///     Initializes a new instance of the <see cref = "Button"/> class.
            /// </summary>
            /// <param name = "text">Text displayed in the button.</param>
            public Button(string text)
            {
                Type = Skyline.DataMiner.Automation.UIBlockType.Button;
                Text = text;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref = "Button"/> class.
            /// </summary>
            public Button(): this(System.String.Empty)
            {
            }

            /// <summary>
            ///     Gets or sets the tooltip.
            /// </summary>
            /// <exception cref = "ArgumentNullException">When the value is <c>null</c>.</exception>
            public string Tooltip
            {
                get
                {
                    return BlockDefinition.TooltipText;
                }

                set
                {
                    if (value == null)
                    {
                        throw new System.ArgumentNullException("value");
                    }

                    BlockDefinition.TooltipText = value;
                }
            }

            /// <summary>
            ///     Triggered when the button is pressed.
            ///     WantsOnChange will be set to true when this event is subscribed to.
            /// </summary>
            public event System.EventHandler<System.EventArgs> Pressed
            {
                add
                {
                    OnPressed += value;
                    WantsOnChange = true;
                }

                remove
                {
                    OnPressed -= value;
                    if (OnPressed == null || !System.Linq.Enumerable.Any(OnPressed.GetInvocationList()))
                    {
                        WantsOnChange = false;
                    }
                }
            }

            private event System.EventHandler<System.EventArgs> OnPressed;
            /// <summary>
            ///     Gets or sets the text displayed in the button.
            /// </summary>
            public string Text
            {
                get
                {
                    return BlockDefinition.Text;
                }

                set
                {
                    BlockDefinition.Text = value;
                }
            }

            internal override void LoadResult(Skyline.DataMiner.Automation.UIResults uiResults)
            {
                pressed = Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.UiResultsExtensions.WasButtonPressed(uiResults, this);
            }

            /// <inheritdoc/>
            internal override void RaiseResultEvents()
            {
                if ((OnPressed != null) && pressed)
                {
                    OnPressed(this, System.EventArgs.Empty);
                }

                pressed = false;
            }
        }

        /// <summary>
        ///     A checkbox that can be selected or cleared.
        /// </summary>
        public class CheckBox : Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.InteractiveWidget
        {
            private bool changed;
            private bool isChecked;
            /// <summary>
            ///     Initializes a new instance of the <see cref = "CheckBox"/> class.
            /// </summary>
            /// <param name = "text">Text displayed next to the checkbox.</param>
            public CheckBox(string text)
            {
                Type = Skyline.DataMiner.Automation.UIBlockType.CheckBox;
                IsChecked = false;
                Text = text;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref = "CheckBox"/> class.
            /// </summary>
            public CheckBox(): this(System.String.Empty)
            {
            }

            private event System.EventHandler<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.CheckBox.CheckBoxChangedEventArgs> OnChanged;
            private event System.EventHandler<System.EventArgs> OnChecked;
            private event System.EventHandler<System.EventArgs> OnUnChecked;
            /// <summary>
            ///     Gets or sets a value indicating whether the checkbox is selected.
            /// </summary>
            public bool IsChecked
            {
                get
                {
                    return isChecked;
                }

                set
                {
                    isChecked = value;
                    BlockDefinition.InitialValue = value.ToString();
                }
            }

            /// <summary>
            ///     Gets or sets the displayed text next to the checkbox.
            /// </summary>
            public string Text
            {
                get
                {
                    return BlockDefinition.Text;
                }

                set
                {
                    BlockDefinition.Text = value;
                }
            }

            /// <summary>
            ///     Gets or sets the tooltip.
            /// </summary>
            /// <exception cref = "ArgumentNullException">When the value is <c>null</c>.</exception>
            public string Tooltip
            {
                get
                {
                    return BlockDefinition.TooltipText;
                }

                set
                {
                    if (value == null)
                    {
                        throw new System.ArgumentNullException("value");
                    }

                    BlockDefinition.TooltipText = value;
                }
            }

            internal override void LoadResult(Skyline.DataMiner.Automation.UIResults uiResults)
            {
                bool result = Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.UiResultsExtensions.GetChecked(uiResults, this);
                if (WantsOnChange)
                {
                    changed = result != IsChecked;
                }

                IsChecked = result;
            }

            /// <inheritdoc/>
            internal override void RaiseResultEvents()
            {
                if (!changed)
                {
                    return;
                }

                if (OnChanged != null)
                {
                    OnChanged(this, new Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.CheckBox.CheckBoxChangedEventArgs(IsChecked));
                }

                if ((OnChecked != null) && IsChecked)
                {
                    OnChecked(this, System.EventArgs.Empty);
                }

                if ((OnUnChecked != null) && !IsChecked)
                {
                    OnUnChecked(this, System.EventArgs.Empty);
                }

                changed = false;
            }

            /// <summary>
            ///     Provides data for the <see cref = "Changed"/> event.
            /// </summary>
            public class CheckBoxChangedEventArgs : System.EventArgs
            {
                internal CheckBoxChangedEventArgs(bool isChecked)
                {
                    IsChecked = isChecked;
                }

                /// <summary>
                ///     Gets a value indicating whether the checkbox has been checked.
                /// </summary>
                public bool IsChecked
                {
                    get;
                    private set;
                }
            }
        }

        /// <summary>
        ///		A button that can be used to show/hide a collection of widgets.
        /// </summary>
        public class CollapseButton : Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.InteractiveWidget
        {
            private const string COLLAPSE = "Collapse";
            private const string EXPAND = "Expand";
            private string collapseText;
            private string expandText;
            private bool pressed;
            private bool isCollapsed;
            /// <summary>
            /// Initializes a new instance of the CollapseButton class.
            /// </summary>
            /// <param name = "linkedWidgets">Widgets that are linked to this collapse button.</param>
            /// <param name = "isCollapsed">State of the collapse button.</param>
            public CollapseButton(System.Collections.Generic.IEnumerable<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget> linkedWidgets, bool isCollapsed)
            {
                Type = Skyline.DataMiner.Automation.UIBlockType.Button;
                LinkedWidgets = new System.Collections.Generic.List<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget>(linkedWidgets);
                CollapseText = COLLAPSE;
                ExpandText = EXPAND;
                IsCollapsed = isCollapsed;
                WantsOnChange = true;
            }

            /// <summary>
            /// Initializes a new instance of the CollapseButton class.
            /// </summary>
            /// <param name = "isCollapsed">State of the collapse button.</param>
            public CollapseButton(bool isCollapsed = false): this(new Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget[0], isCollapsed)
            {
            }

            private event System.EventHandler<System.EventArgs> OnPressed;
            /// <summary>
            /// Indicates if the collapse button is collapsed or not.
            /// If the collapse button is collapsed, the IsVisible property of all linked widgets is set to false.
            /// If the collapse button is not collapsed, the IsVisible property of all linked widgets is set to true.
            /// </summary>
            public bool IsCollapsed
            {
                get
                {
                    return isCollapsed;
                }

                set
                {
                    isCollapsed = value;
                    BlockDefinition.Text = value ? ExpandText : CollapseText;
                    foreach (Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget widget in GetAffectedWidgets(this, value))
                    {
                        widget.IsVisible = !value;
                    }
                }
            }

            /// <summary>
            /// Gets or sets the text to be displayed in the collapse button when the button is expanded.
            /// </summary>
            public string CollapseText
            {
                get
                {
                    return collapseText;
                }

                set
                {
                    if (System.String.IsNullOrWhiteSpace(value))
                        throw new System.ArgumentException("The Collapse text cannot be empty.");
                    collapseText = value;
                    if (!IsCollapsed)
                        BlockDefinition.Text = collapseText;
                }
            }

            /// <summary>
            ///     Gets or sets the tooltip.
            /// </summary>
            /// <exception cref = "ArgumentNullException">When the value is <c>null</c>.</exception>
            public string Tooltip
            {
                get
                {
                    return BlockDefinition.TooltipText;
                }

                set
                {
                    if (value == null)
                    {
                        throw new System.ArgumentNullException("value");
                    }

                    BlockDefinition.TooltipText = value;
                }
            }

            /// <summary>
            /// Gets or sets the text to be displayed in the collapse button when the button is collapsed.
            /// </summary>
            public string ExpandText
            {
                get
                {
                    return expandText;
                }

                set
                {
                    if (System.String.IsNullOrWhiteSpace(value))
                        throw new System.ArgumentException("The Expand text cannot be empty.");
                    expandText = value;
                    if (IsCollapsed)
                        BlockDefinition.Text = expandText;
                }
            }

            /// <summary>
            /// Collection of widgets that are affected by this collapse button.
            /// </summary>
            public System.Collections.Generic.List<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget> LinkedWidgets
            {
                get;
                private set;
            }

            internal override void LoadResult(Skyline.DataMiner.Automation.UIResults uiResults)
            {
                pressed = Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.UiResultsExtensions.WasCollapseButtonPressed(uiResults, this);
            }

            internal override void RaiseResultEvents()
            {
                if (pressed)
                {
                    IsCollapsed = !IsCollapsed;
                    if (OnPressed != null)
                        OnPressed(this, System.EventArgs.Empty);
                }

                pressed = false;
            }

            /// <summary>
            /// Retrieves a list of Widgets that are affected when the state of the provided collapse button is changed.
            /// This method was introduced to support nested collapse buttons.
            /// </summary>
            /// <param name = "collapseButton">Collapse button that is checked.</param>
            /// <param name = "collapse">Indicates if the top collapse button is going to be collapsed or expanded.</param>
            /// <returns>List of affected widgets.</returns>
            private static System.Collections.Generic.List<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget> GetAffectedWidgets(Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.CollapseButton collapseButton, bool collapse)
            {
                System.Collections.Generic.List<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget> affectedWidgets = new System.Collections.Generic.List<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget>();
                affectedWidgets.AddRange(collapseButton.LinkedWidgets);
                var nestedCollapseButtons = System.Linq.Enumerable.OfType<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.CollapseButton>(collapseButton.LinkedWidgets);
                foreach (Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.CollapseButton nestedCollapseButton in nestedCollapseButtons)
                {
                    if (collapse)
                    {
                        // Collapsing top collapse button
                        affectedWidgets.AddRange(GetAffectedWidgets(nestedCollapseButton, collapse));
                    }
                    else if (!nestedCollapseButton.IsCollapsed)
                    {
                        // Expanding top collapse button
                        affectedWidgets.AddRange(GetAffectedWidgets(nestedCollapseButton, collapse));
                    }
                }

                return affectedWidgets;
            }
        }

        /// <summary>
        ///     A drop-down list.
        /// </summary>
        public class DropDown : Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.InteractiveWidget
        {
            private readonly System.Collections.Generic.HashSet<System.String> options = new System.Collections.Generic.HashSet<System.String>();
            private bool changed;
            private string previous;
            /// <summary>
            ///     Initializes a new instance of the <see cref = "DropDown"/> class.
            /// </summary>
            public DropDown(): this(System.Linq.Enumerable.Empty<string>())
            {
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref = "DropDown"/> class.
            /// </summary>
            /// <param name = "options">Options to be displayed in the list.</param>
            /// <param name = "selected">The selected item in the list.</param>
            /// <exception cref = "ArgumentNullException">When options is null.</exception>
            public DropDown(System.Collections.Generic.IEnumerable<System.String> options, string selected = null)
            {
                Type = Skyline.DataMiner.Automation.UIBlockType.DropDown;
                SetOptions(options);
                if (selected != null)
                    Selected = selected;
                ValidationText = "Invalid Input";
                ValidationState = Skyline.DataMiner.Automation.UIValidationState.NotValidated;
            }

            private event System.EventHandler<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.DropDown.DropDownChangedEventArgs> OnChanged;
            /// <summary>
            ///     Gets or sets the possible options.
            /// </summary>
            public System.Collections.Generic.IEnumerable<System.String> Options
            {
                get
                {
                    return options;
                }

                set
                {
                    SetOptions(value);
                }
            }

            /// <summary>
            ///     Gets or sets the selected option.
            /// </summary>
            public string Selected
            {
                get
                {
                    return BlockDefinition.InitialValue;
                }

                set
                {
                    BlockDefinition.InitialValue = value;
                }
            }

            /// <summary>
            ///     Gets or sets the tooltip.
            /// </summary>
            /// <exception cref = "ArgumentNullException">When the value is <c>null</c>.</exception>
            public string Tooltip
            {
                get
                {
                    return BlockDefinition.TooltipText;
                }

                set
                {
                    if (value == null)
                    {
                        throw new System.ArgumentNullException("value");
                    }

                    BlockDefinition.TooltipText = value;
                }
            }

            /// <summary>
            ///		Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
            ///		This should be used by the client to add a visual marker on the input field.
            /// </summary>
            /// <remarks>Available from DataMiner Feature Release 10.0.5 and 10.0.1.0 Main Release.</remarks>
            public Skyline.DataMiner.Automation.UIValidationState ValidationState
            {
                get
                {
                    return BlockDefinition.ValidationState;
                }

                set
                {
                    BlockDefinition.ValidationState = value;
                }
            }

            /// <summary>
            ///		Gets or sets the text that is shown if the validation state is invalid.
            ///		This should be used by the client to add a visual marker on the input field.
            /// </summary>
            /// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
            public string ValidationText
            {
                get
                {
                    return BlockDefinition.ValidationText;
                }

                set
                {
                    BlockDefinition.ValidationText = value;
                }
            }

            /// <summary>
            ///     Gets or sets a value indicating whether a filter box is available for the drop-down list.
            /// </summary>
            /// <remarks>Available from DataMiner 9.5.6 onwards.</remarks>
            public bool IsDisplayFilterShown
            {
                get
                {
                    return BlockDefinition.DisplayFilter;
                }

                set
                {
                    BlockDefinition.DisplayFilter = value;
                }
            }

            /// <summary>
            ///     Gets or sets a value indicating whether the options are sorted naturally.
            /// </summary>
            /// <remarks>Available from DataMiner 9.5.6 onwards.</remarks>
            public bool IsSorted
            {
                get
                {
                    return BlockDefinition.IsSorted;
                }

                set
                {
                    BlockDefinition.IsSorted = value;
                }
            }

            /// <summary>
            ///     Adds an option to the drop-down list.
            /// </summary>
            /// <param name = "option">Option to add.</param>
            /// <exception cref = "ArgumentNullException">When option is null.</exception>
            public void AddOption(string option)
            {
                if (option == null)
                {
                    throw new System.ArgumentNullException("option");
                }

                if (!options.Contains(option))
                {
                    options.Add(option);
                    BlockDefinition.AddDropDownOption(option);
                }
            }

            /// <summary>
            ///     Sets the displayed options.
            ///     Replaces existing options.
            /// </summary>
            /// <param name = "optionsToSet">Options to set.</param>
            /// <exception cref = "ArgumentNullException">When optionsToSet is null.</exception>
            public void SetOptions(System.Collections.Generic.IEnumerable<System.String> optionsToSet)
            {
                if (optionsToSet == null)
                {
                    throw new System.ArgumentNullException("optionsToSet");
                }

                ClearOptions();
                foreach (string option in optionsToSet)
                {
                    AddOption(option);
                }

                if (Selected == null || !System.Linq.Enumerable.Contains(optionsToSet, Selected))
                {
                    Selected = System.Linq.Enumerable.FirstOrDefault(optionsToSet);
                }
            }

            internal override void LoadResult(Skyline.DataMiner.Automation.UIResults uiResults)
            {
                string selectedValue = Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.UiResultsExtensions.GetString(uiResults, this);
                if (WantsOnChange)
                {
                    changed = selectedValue != Selected;
                }

                previous = Selected;
                Selected = selectedValue;
            }

            /// <inheritdoc/>
            internal override void RaiseResultEvents()
            {
                if (changed && (OnChanged != null))
                {
                    OnChanged(this, new Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.DropDown.DropDownChangedEventArgs(Selected, previous));
                }

                changed = false;
            }

            private void ClearOptions()
            {
                options.Clear();
                RecreateUiBlock();
            }

            /// <summary>
            ///     Provides data for the <see cref = "Changed"/> event.
            /// </summary>
            public class DropDownChangedEventArgs : System.EventArgs
            {
                internal DropDownChangedEventArgs(string selected, string previous)
                {
                    Selected = selected;
                    Previous = previous;
                }

                /// <summary>
                ///     Gets the previously selected option.
                /// </summary>
                public string Previous
                {
                    get;
                    private set;
                }

                /// <summary>
                ///     Gets the option that has been selected.
                /// </summary>
                public string Selected
                {
                    get;
                    private set;
                }
            }
        }

        /// <summary>
        /// A widget that requires user input.
        /// </summary>
        public abstract class InteractiveWidget : Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget
        {
            /// <summary>
            /// Initializes a new instance of the InteractiveWidget class.
            /// </summary>
            protected InteractiveWidget()
            {
                BlockDefinition.DestVar = System.Guid.NewGuid().ToString();
                WantsOnChange = false;
            }

            /// <summary>
            ///     Gets the alias that will be used to retrieve the value entered or selected by the user from the UIResults object.
            /// </summary>
            /// <remarks>Use methods <see cref = "UiResultsExtensions"/> to retrieve the result instead.</remarks>
            internal string DestVar
            {
                get
                {
                    return BlockDefinition.DestVar;
                }
            }

            /// <summary>
            ///     Gets or sets a value indicating whether the control is enabled in the UI.
            ///     Disabling causes the widgets to be grayed out and disables user interaction.
            /// </summary>
            /// <remarks>Available from DataMiner 9.5.3 onwards.</remarks>
            public bool IsEnabled
            {
                get
                {
                    return BlockDefinition.IsEnabled;
                }

                set
                {
                    BlockDefinition.IsEnabled = value;
                }
            }

            /// <summary>
            ///     Gets or sets a value indicating whether an update of the current value of the dialog box item will trigger an
            ///     event.
            /// </summary>
            /// <remarks>Is <c>false</c> by default except for <see cref = "Button"/>.</remarks>
            public bool WantsOnChange
            {
                get
                {
                    return BlockDefinition.WantsOnChange;
                }

                set
                {
                    BlockDefinition.WantsOnChange = value;
                }
            }

            internal abstract void LoadResult(Skyline.DataMiner.Automation.UIResults uiResults);
            internal abstract void RaiseResultEvents();
        }

        /// <summary>
        ///     A label is used to display text.
        ///     Text can have different styles.
        /// </summary>
        public class Label : Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget
        {
            private Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.TextStyle style;
            /// <summary>
            ///     Initializes a new instance of the <see cref = "Label"/> class.
            /// </summary>
            /// <param name = "text">The text that is displayed by the label.</param>
            public Label(string text)
            {
                Type = Skyline.DataMiner.Automation.UIBlockType.StaticText;
                Style = Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.TextStyle.None;
                Text = text;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref = "Label"/> class.
            /// </summary>
            public Label(): this("Label")
            {
            }

            /// <summary>
            ///     Gets or sets the text style of the label.
            /// </summary>
            public Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.TextStyle Style
            {
                get
                {
                    return style;
                }

                set
                {
                    style = value;
                    BlockDefinition.Style = StyleToUiString(value);
                }
            }

            /// <summary>
            ///     Gets or sets the displayed text.
            /// </summary>
            public string Text
            {
                get
                {
                    return BlockDefinition.Text;
                }

                set
                {
                    BlockDefinition.Text = value;
                }
            }

            /// <summary>
            ///     Gets or sets the tooltip.
            /// </summary>
            /// <exception cref = "ArgumentNullException">When the value is <c>null</c>.</exception>
            public string Tooltip
            {
                get
                {
                    return BlockDefinition.TooltipText;
                }

                set
                {
                    if (value == null)
                    {
                        throw new System.ArgumentNullException("value");
                    }

                    BlockDefinition.TooltipText = value;
                }
            }

            private static string StyleToUiString(Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.TextStyle textStyle)
            {
                switch (textStyle)
                {
                    case Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.TextStyle.None:
                        return null;
                    case Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.TextStyle.Title:
                        return "Title1";
                    case Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.TextStyle.Bold:
                        return "Title2";
                    case Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.TextStyle.Heading:
                        return "Title3";
                    default:
                        throw new System.ArgumentOutOfRangeException("textStyle", textStyle, null);
                }
            }
        }

        /// <summary>
        ///     Widget that is used to edit and display text.
        /// </summary>
        public class TextBox : Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.InteractiveWidget
        {
            private bool changed;
            private string previous;
            /// <summary>
            ///     Initializes a new instance of the <see cref = "TextBox"/> class.
            /// </summary>
            /// <param name = "text">The text displayed in the text box.</param>
            public TextBox(string text)
            {
                Type = Skyline.DataMiner.Automation.UIBlockType.TextBox;
                Text = text;
                PlaceHolder = System.String.Empty;
                ValidationText = "Invalid Input";
                ValidationState = Skyline.DataMiner.Automation.UIValidationState.NotValidated;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref = "TextBox"/> class.
            /// </summary>
            public TextBox(): this(System.String.Empty)
            {
            }

            private event System.EventHandler<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.TextBox.TextBoxChangedEventArgs> OnChanged;
            /// <summary>
            ///     Gets or sets a value indicating whether users are able to enter multiple lines of text.
            /// </summary>
            public bool IsMultiline
            {
                get
                {
                    return BlockDefinition.IsMultiline;
                }

                set
                {
                    BlockDefinition.IsMultiline = value;
                }
            }

            /// <summary>
            ///     Gets or sets the text displayed in the text box.
            /// </summary>
            public string Text
            {
                get
                {
                    return BlockDefinition.InitialValue;
                }

                set
                {
                    BlockDefinition.InitialValue = value;
                }
            }

            /// <summary>
            ///     Gets or sets the tooltip.
            /// </summary>
            /// <exception cref = "ArgumentNullException">When the value is <c>null</c>.</exception>
            public string Tooltip
            {
                get
                {
                    return BlockDefinition.TooltipText;
                }

                set
                {
                    if (value == null)
                    {
                        throw new System.ArgumentNullException("value");
                    }

                    BlockDefinition.TooltipText = value;
                }
            }

            /// <summary>
            ///		Gets or sets the text that should be displayed as a placeholder.
            /// </summary>
            /// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
            public string PlaceHolder
            {
                get
                {
                    return BlockDefinition.PlaceholderText;
                }

                set
                {
                    BlockDefinition.PlaceholderText = value;
                }
            }

            /// <summary>
            ///		Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
            ///		This should be used by the client to add a visual marker on the input field.
            /// </summary>
            /// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
            public Skyline.DataMiner.Automation.UIValidationState ValidationState
            {
                get
                {
                    return BlockDefinition.ValidationState;
                }

                set
                {
                    BlockDefinition.ValidationState = value;
                }
            }

            /// <summary>
            ///		Gets or sets the text that is shown if the validation state is invalid.
            ///		This should be used by the client to add a visual marker on the input field.
            /// </summary>
            /// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
            public string ValidationText
            {
                get
                {
                    return BlockDefinition.ValidationText;
                }

                set
                {
                    BlockDefinition.ValidationText = value;
                }
            }

            internal override void LoadResult(Skyline.DataMiner.Automation.UIResults uiResults)
            {
                string value = Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.UiResultsExtensions.GetString(uiResults, this);
                if (WantsOnChange)
                {
                    changed = value != Text;
                    previous = Text;
                }

                Text = value;
            }

            /// <inheritdoc/>
            internal override void RaiseResultEvents()
            {
                if (changed && OnChanged != null)
                {
                    OnChanged(this, new Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.TextBox.TextBoxChangedEventArgs(Text, previous));
                }

                changed = false;
            }

            /// <summary>
            ///     Provides data for the <see cref = "Changed"/> event.
            /// </summary>
            public class TextBoxChangedEventArgs : System.EventArgs
            {
                internal TextBoxChangedEventArgs(string value, string previous)
                {
                    Value = value;
                    Previous = previous;
                }

                /// <summary>
                ///     Gets the text before the change.
                /// </summary>
                public string Previous
                {
                    get;
                    private set;
                }

                /// <summary>
                ///     Gets the changed text.
                /// </summary>
                public string Value
                {
                    get;
                    private set;
                }
            }
        }

        /// <summary>
        ///  A tree view structure.
        /// </summary>
        public class TreeView : Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.InteractiveWidget
        {
            private System.Collections.Generic.Dictionary<System.String, System.Boolean> checkedItemCache;
            private System.Collections.Generic.Dictionary<System.String, System.Boolean> collapsedItemCache; // TODO: should only contain Items with LazyLoading set to true
            private System.Collections.Generic.Dictionary<System.String, Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem> lookupTable;
            private bool itemsChanged = false;
            private System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem> changedItems = new System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem>();
            private bool itemsChecked = false;
            private System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem> checkedItems = new System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem>();
            private bool itemsUnchecked = false;
            private System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem> uncheckedItems = new System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem>();
            private bool itemsExpanded = false;
            private System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem> expandedItems = new System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem>();
            private bool itemsCollapsed = false;
            private System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem> collapsedItems = new System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem>();
            /// <summary>
            ///		Initializes a new instance of the <see cref = "TreeView"/> class.
            /// </summary>
            /// <param name = "treeViewItems"></param>
            public TreeView(System.Collections.Generic.IEnumerable<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem> treeViewItems)
            {
                Type = Skyline.DataMiner.Automation.UIBlockType.TreeView;
                Items = treeViewItems;
            }

            private event System.EventHandler<System.Collections.Generic.IEnumerable<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem>> OnChanged;
            private event System.EventHandler<System.Collections.Generic.IEnumerable<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem>> OnChecked;
            private event System.EventHandler<System.Collections.Generic.IEnumerable<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem>> OnUnchecked;
            private event System.EventHandler<System.Collections.Generic.IEnumerable<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem>> OnExpanded;
            private event System.EventHandler<System.Collections.Generic.IEnumerable<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem>> OnCollapsed;
            /// <summary>
            /// Returns the top-level items in the tree view.
            /// The TreeViewItem.ChildItems property can be used to navigate further down the tree.
            /// </summary>
            public System.Collections.Generic.IEnumerable<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem> Items
            {
                get
                {
                    return BlockDefinition.TreeViewItems;
                }

                set
                {
                    if (value == null)
                        throw new System.ArgumentNullException("value");
                    BlockDefinition.TreeViewItems = new System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem>(value);
                    UpdateItemCache();
                }
            }

            /// <summary>
            /// Returns all items in the tree view that are selected.
            /// </summary>
            public System.Collections.Generic.IEnumerable<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem> CheckedItems
            {
                get
                {
                    return GetCheckedItems();
                }
            }

            /// <summary>
            /// Returns all leaves (= items without children) in the tree view that are selected.
            /// </summary>
            public System.Collections.Generic.IEnumerable<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem> CheckedLeaves
            {
                get
                {
                    return System.Linq.Enumerable.Where(GetCheckedItems(), x => !System.Linq.Enumerable.Any(x.ChildItems));
                }
            }

            /// <summary>
            /// Returns all nodes (= items with children) in the tree view that are selected.
            /// </summary>
            public System.Collections.Generic.IEnumerable<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem> CheckedNodes
            {
                get
                {
                    return System.Linq.Enumerable.Where(GetCheckedItems(), x => System.Linq.Enumerable.Any(x.ChildItems));
                }
            }

            /// <summary>
            /// This method is used to update the cached TreeViewItems and lookup table.
            /// </summary>
            internal void UpdateItemCache()
            {
                checkedItemCache = new System.Collections.Generic.Dictionary<System.String, System.Boolean>();
                collapsedItemCache = new System.Collections.Generic.Dictionary<System.String, System.Boolean>();
                lookupTable = new System.Collections.Generic.Dictionary<System.String, Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem>();
                foreach (var item in GetAllItems())
                {
                    try
                    {
                        checkedItemCache.Add(item.KeyValue, item.IsChecked);
                        if (item.SupportsLazyLoading)
                            collapsedItemCache.Add(item.KeyValue, item.IsCollapsed);
                        lookupTable.Add(item.KeyValue, item);
                    }
                    catch (System.Exception e)
                    {
                        throw new Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.TreeViewDuplicateItemsException(item.KeyValue, e);
                    }
                }
            }

            /// <summary>
            /// Returns all items in the TreeView that are checked.
            /// </summary>
            /// <returns>All checked TreeViewItems in the TreeView.</returns>
            private System.Collections.Generic.IEnumerable<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem> GetCheckedItems()
            {
                return System.Linq.Enumerable.Where(lookupTable.Values, x => x.ItemType == Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem.TreeViewItemType.CheckBox && x.IsChecked);
            }

            /// <summary>
            /// Iterates over all items in the tree and returns them in a flat collection.
            /// </summary>
            /// <returns>A flat collection containing all items in the tree view.</returns>
            public System.Collections.Generic.IEnumerable<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem> GetAllItems()
            {
                System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem> allItems = new System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem>();
                foreach (var item in Items)
                {
                    allItems.Add(item);
                    allItems.AddRange(GetAllItems(item.ChildItems));
                }

                return allItems;
            }

            /// <summary>
            /// This method is used to recursively go through all the items in the TreeView.
            /// </summary>
            /// <param name = "children">List of TreeViewItems to be visited.</param>
            /// <returns>Flat collection containing every item in the provided children collection and all underlying items.</returns>
            private System.Collections.Generic.IEnumerable<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem> GetAllItems(System.Collections.Generic.IEnumerable<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem> children)
            {
                System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem> allItems = new System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem>();
                foreach (var item in children)
                {
                    allItems.Add(item);
                    allItems.AddRange(GetAllItems(item.ChildItems));
                }

                return allItems;
            }

            /// <summary>
            ///     Gets or sets the tooltip.
            /// </summary>
            /// <exception cref = "ArgumentNullException">When the value is <c>null</c>.</exception>
            public string Tooltip
            {
                get
                {
                    return BlockDefinition.TooltipText;
                }

                set
                {
                    if (value == null)
                    {
                        throw new System.ArgumentNullException("value");
                    }

                    BlockDefinition.TooltipText = value;
                }
            }

            internal override void LoadResult(Skyline.DataMiner.Automation.UIResults uiResults)
            {
                var checkedItemKeys = Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.UiResultsExtensions.GetCheckedItemKeys(uiResults, this); // this includes all checked items
                var expandedItemKeys = Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.UiResultsExtensions.GetExpandedItemKeys(uiResults, this); // this includes all expanded items with LazyLoading set to true
                // Check for changes
                // Expanded Items
                System.Collections.Generic.List<System.String> newlyExpandedItems = System.Linq.Enumerable.ToList(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(collapsedItemCache, x => System.Linq.Enumerable.Contains(expandedItemKeys, x.Key) && x.Value), x => x.Key));
                if (System.Linq.Enumerable.Any(newlyExpandedItems) && OnExpanded != null)
                {
                    itemsExpanded = true;
                    expandedItems = new System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem>();
                    foreach (string newlyExpandedItemKey in newlyExpandedItems)
                    {
                        expandedItems.Add(lookupTable[newlyExpandedItemKey]);
                    }
                }

                // Collapsed Items
                System.Collections.Generic.List<System.String> newlyCollapsedItems = System.Linq.Enumerable.ToList(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(collapsedItemCache, x => !System.Linq.Enumerable.Contains(expandedItemKeys, x.Key) && !x.Value), x => x.Key));
                if (System.Linq.Enumerable.Any(newlyCollapsedItems) && OnCollapsed != null)
                {
                    itemsCollapsed = true;
                    collapsedItems = new System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem>();
                    foreach (string newyCollapsedItemKey in newlyCollapsedItems)
                    {
                        collapsedItems.Add(lookupTable[newyCollapsedItemKey]);
                    }
                }

                // Checked Items
                System.Collections.Generic.List<System.String> newlyCheckedItemKeys = System.Linq.Enumerable.ToList(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(checkedItemCache, x => System.Linq.Enumerable.Contains(checkedItemKeys, x.Key) && !x.Value), x => x.Key));
                if (System.Linq.Enumerable.Any(newlyCheckedItemKeys) && OnChecked != null)
                {
                    itemsChecked = true;
                    checkedItems = new System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem>();
                    foreach (string newlyCheckedItemKey in newlyCheckedItemKeys)
                    {
                        checkedItems.Add(lookupTable[newlyCheckedItemKey]);
                    }
                }

                // Unchecked Items
                System.Collections.Generic.List<System.String> newlyUncheckedItemKeys = System.Linq.Enumerable.ToList(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(checkedItemCache, x => !System.Linq.Enumerable.Contains(checkedItemKeys, x.Key) && x.Value), x => x.Key));
                if (System.Linq.Enumerable.Any(newlyUncheckedItemKeys) && OnUnchecked != null)
                {
                    itemsUnchecked = true;
                    uncheckedItems = new System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem>();
                    foreach (string newlyUncheckedItemKey in newlyUncheckedItemKeys)
                    {
                        uncheckedItems.Add(lookupTable[newlyUncheckedItemKey]);
                    }
                }

                // Changed Items
                System.Collections.Generic.List<System.String> changedItemKeys = new System.Collections.Generic.List<System.String>();
                changedItemKeys.AddRange(newlyCheckedItemKeys);
                changedItemKeys.AddRange(newlyUncheckedItemKeys);
                if (System.Linq.Enumerable.Any(changedItemKeys) && OnChanged != null)
                {
                    itemsChanged = true;
                    changedItems = new System.Collections.Generic.List<Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem>();
                    foreach (string changedItemKey in changedItemKeys)
                    {
                        changedItems.Add(lookupTable[changedItemKey]);
                    }
                }

                // Persist states
                foreach (Skyline.DataMiner.Net.AutomationUI.Objects.TreeViewItem item in lookupTable.Values)
                {
                    item.IsChecked = System.Linq.Enumerable.Contains(checkedItemKeys, item.KeyValue);
                    item.IsCollapsed = !System.Linq.Enumerable.Contains(expandedItemKeys, item.KeyValue);
                }

                UpdateItemCache();
            }

            /// <inheritdoc/>
            internal override void RaiseResultEvents()
            {
                // Expanded items
                if (itemsExpanded && OnExpanded != null)
                    OnExpanded(this, expandedItems);
                // Collapsed items
                if (itemsCollapsed && OnCollapsed != null)
                    OnCollapsed(this, collapsedItems);
                // Checked items
                if (itemsChecked && OnChecked != null)
                    OnChecked(this, checkedItems);
                // Unchecked items
                if (itemsUnchecked && OnUnchecked != null)
                    OnUnchecked(this, uncheckedItems);
                // Changed items
                if (itemsChanged && OnChanged != null)
                    OnChanged(this, changedItems);
                itemsExpanded = false;
                itemsCollapsed = false;
                itemsChecked = false;
                itemsUnchecked = false;
                itemsChanged = false;
                UpdateItemCache();
            }
        }

        /// <summary>
        ///     Base class for widgets.
        /// </summary>
        public class Widget
        {
            private Skyline.DataMiner.Automation.UIBlockDefinition blockDefinition = new Skyline.DataMiner.Automation.UIBlockDefinition();
            /// <summary>
            /// Initializes a new instance of the Widget class.
            /// </summary>
            protected Widget()
            {
                Type = Skyline.DataMiner.Automation.UIBlockType.Undefined;
                IsVisible = true;
                SetHeightAuto();
                SetWidthAuto();
            }

            /// <summary>
            ///     Gets or sets the fixed height (in pixels) of the widget.
            /// </summary>
            /// <exception cref = "ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
            public int Height
            {
                get
                {
                    return BlockDefinition.Height;
                }

                set
                {
                    if (value <= 0)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    BlockDefinition.Height = value;
                }
            }

            /// <summary>
            ///     Gets or sets a value indicating whether the widget is visible in the dialog.
            /// </summary>
            public bool IsVisible
            {
                get;
                set;
            }

            /// <summary>
            ///     Gets or sets the maximum height (in pixels) of the widget.
            /// </summary>
            /// <exception cref = "ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
            public int MaxHeight
            {
                get
                {
                    return BlockDefinition.MaxHeight;
                }

                set
                {
                    if (value <= -2)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    BlockDefinition.MaxHeight = value;
                }
            }

            /// <summary>
            ///     Gets or sets the maximum width (in pixels) of the widget.
            /// </summary>
            /// <exception cref = "ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
            public int MaxWidth
            {
                get
                {
                    return BlockDefinition.MaxWidth;
                }

                set
                {
                    if (value <= -2)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    BlockDefinition.MaxWidth = value;
                }
            }

            /// <summary>
            ///     Gets or sets the minimum height (in pixels) of the widget.
            /// </summary>
            /// <exception cref = "ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
            public int MinHeight
            {
                get
                {
                    return BlockDefinition.MinHeight;
                }

                set
                {
                    if (value <= -2)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    BlockDefinition.MinHeight = value;
                }
            }

            /// <summary>
            ///     Gets or sets the minimum width (in pixels) of the widget.
            /// </summary>
            /// <exception cref = "ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
            public int MinWidth
            {
                get
                {
                    return BlockDefinition.MinWidth;
                }

                set
                {
                    if (value <= -2)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    BlockDefinition.MinWidth = value;
                }
            }

            /// <summary>
            ///     Gets or sets the UIBlockType of the widget.
            /// </summary>
            public Skyline.DataMiner.Automation.UIBlockType Type
            {
                get
                {
                    return BlockDefinition.Type;
                }

                protected set
                {
                    BlockDefinition.Type = value;
                }
            }

            /// <summary>
            ///     Gets or sets the fixed width (in pixels) of the widget.
            /// </summary>
            /// <exception cref = "ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
            public int Width
            {
                get
                {
                    return BlockDefinition.Width;
                }

                set
                {
                    if (value <= 0)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    BlockDefinition.Width = value;
                }
            }

            /// <summary>
            /// Margin of the widget.
            /// </summary>
            public Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Margin Margin
            {
                get
                {
                    return new Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Margin(BlockDefinition.Margin);
                }

                set
                {
                    BlockDefinition.Margin = value.ToString();
                }
            }

            internal Skyline.DataMiner.Automation.UIBlockDefinition BlockDefinition
            {
                get
                {
                    return blockDefinition;
                }
            }

            /// <summary>
            ///     Set the height of the widget based on its content.
            /// </summary>
            public void SetHeightAuto()
            {
                BlockDefinition.Height = -1;
                BlockDefinition.MaxHeight = -1;
                BlockDefinition.MinHeight = -1;
            }

            /// <summary>
            ///     Set the width of the widget based on its content.
            /// </summary>
            public void SetWidthAuto()
            {
                BlockDefinition.Width = -1;
                BlockDefinition.MaxWidth = -1;
                BlockDefinition.MinWidth = -1;
            }

            /// <summary>
            /// Ugly method to clear the internal list of DropDown items that can't be accessed.
            /// </summary>
            protected void RecreateUiBlock()
            {
                Skyline.DataMiner.Automation.UIBlockDefinition newUiBlockDefinition = new Skyline.DataMiner.Automation.UIBlockDefinition();
                System.Reflection.PropertyInfo[] propertyInfo = typeof(Skyline.DataMiner.Automation.UIBlockDefinition).GetProperties();
                foreach (System.Reflection.PropertyInfo property in propertyInfo)
                {
                    if (property.CanWrite)
                    {
                        property.SetValue(newUiBlockDefinition, property.GetValue(blockDefinition));
                    }
                }

                blockDefinition = newUiBlockDefinition;
            }
        }

        /// <summary>
        ///     A dialog represents a single window that can be shown.
        ///     You can show widgets in the window by adding them to the dialog.
        ///     The dialog uses a grid to determine the layout of its widgets.
        /// </summary>
        public abstract class Dialog
        {
            private const string Auto = "auto";
            private readonly System.Collections.Generic.Dictionary<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.IWidgetLayout> widgetLayouts = new System.Collections.Generic.Dictionary<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.IWidgetLayout>();
            private readonly System.Collections.Generic.Dictionary<System.Int32, System.String> columnDefinitions = new System.Collections.Generic.Dictionary<System.Int32, System.String>();
            private readonly System.Collections.Generic.Dictionary<System.Int32, System.String> rowDefinitions = new System.Collections.Generic.Dictionary<System.Int32, System.String>();
            private int height;
            private int maxHeight;
            private int maxWidth;
            private int minHeight;
            private int minWidth;
            private int width;
            private bool isEnabled = true;
            /// <summary>
            /// Initializes a new instance of the <see cref = "Dialog"/> class.
            /// </summary>
            /// <param name = "engine"></param>
            protected Dialog(Skyline.DataMiner.Automation.IEngine engine)
            {
                if (engine == null)
                {
                    throw new System.ArgumentNullException("engine");
                }

                Engine = engine;
                width = -1;
                height = -1;
                MaxHeight = System.Int32.MaxValue;
                MinHeight = 1;
                MaxWidth = System.Int32.MaxValue;
                MinWidth = 1;
                RowCount = 0;
                ColumnCount = 0;
                Title = "Dialog";
                AllowOverlappingWidgets = false;
            }

            /// <summary>
            /// Gets or sets a value indicating whether overlapping widgets are allowed or not.
            /// Can be used in case you want to add multiple widgets to the same cell in the dialog.
            /// You can use the Margin property on the widgets to place them apart.
            /// </summary>
            public bool AllowOverlappingWidgets
            {
                get;
                set;
            }

            /// <summary>
            ///     Triggered when the back button of the dialog is pressed.
            /// </summary>
            public event System.EventHandler<System.EventArgs> Back;
            /// <summary>
            ///     Triggered when the forward button of the dialog is pressed.
            /// </summary>
            public event System.EventHandler<System.EventArgs> Forward;
            /// <summary>
            ///     Triggered when there is any user interaction.
            /// </summary>
            public event System.EventHandler<System.EventArgs> Interacted;
            /// <summary>
            ///     Gets the number of columns of the grid layout.
            /// </summary>
            public int ColumnCount
            {
                get;
                private set;
            }

            /// <summary>
            ///     Gets the link to the SLAutomation process.
            /// </summary>
            public Skyline.DataMiner.Automation.IEngine Engine
            {
                get;
                private set;
            }

            /// <summary>
            ///     Gets or sets the fixed height (in pixels) of the dialog.
            /// </summary>
            /// <remarks>
            ///     The user will still be able to resize the window,
            ///     but scrollbars will appear immediately.
            ///     <see cref = "MinHeight"/> should be used instead as it has a more desired effect.
            /// </remarks>
            /// <exception cref = "ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
            public int Height
            {
                get
                {
                    return height;
                }

                set
                {
                    if (value <= 0)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    height = value;
                }
            }

            /// <summary>
            ///     Gets or sets the maximum height (in pixels) of the dialog.
            /// </summary>
            /// <remarks>
            ///     The user will still be able to resize the window past this limit.
            /// </remarks>
            /// <exception cref = "ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
            public int MaxHeight
            {
                get
                {
                    return maxHeight;
                }

                set
                {
                    if (value <= 0)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    maxHeight = value;
                }
            }

            /// <summary>
            ///     Gets or sets the maximum width (in pixels) of the dialog.
            /// </summary>
            /// <remarks>
            ///     The user will still be able to resize the window past this limit.
            /// </remarks>
            /// <exception cref = "ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
            public int MaxWidth
            {
                get
                {
                    return maxWidth;
                }

                set
                {
                    if (value <= 0)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    maxWidth = value;
                }
            }

            /// <summary>
            ///     Gets or sets the minimum height (in pixels) of the dialog.
            /// </summary>
            /// <exception cref = "ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
            public int MinHeight
            {
                get
                {
                    return minHeight;
                }

                set
                {
                    if (value <= 0)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    minHeight = value;
                }
            }

            /// <summary>
            ///     Gets or sets the minimum width (in pixels) of the dialog.
            /// </summary>
            /// <exception cref = "ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
            public int MinWidth
            {
                get
                {
                    return minWidth;
                }

                set
                {
                    if (value <= 0)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    minWidth = value;
                }
            }

            /// <summary>
            ///     Gets the number of rows in the grid layout.
            /// </summary>
            public int RowCount
            {
                get;
                private set;
            }

            /// <summary>
            ///		Gets or sets a value indicating whether the interactive widgets within the dialog are enabled or not.
            /// </summary>
            public bool IsEnabled
            {
                get
                {
                    return isEnabled;
                }

                set
                {
                    isEnabled = value;
                    foreach (Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget widget in Widgets)
                    {
                        Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.InteractiveWidget interactiveWidget = widget as Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.InteractiveWidget;
                        if (interactiveWidget != null && !(interactiveWidget is Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.CollapseButton))
                        {
                            interactiveWidget.IsEnabled = isEnabled;
                        }
                    }
                }
            }

            /// <summary>
            ///     Gets or sets the title at the top of the window.
            /// </summary>
            /// <remarks>Available from DataMiner 9.6.6 onwards.</remarks>
            public string Title
            {
                get;
                set;
            }

            /// <summary>
            ///     Gets widgets that are added to the dialog.
            /// </summary>
            public System.Collections.Generic.IEnumerable<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget> Widgets
            {
                get
                {
                    return widgetLayouts.Keys;
                }
            }

            /// <summary>
            ///     Gets or sets the fixed width (in pixels) of the dialog.
            /// </summary>
            /// <remarks>
            ///     The user will still be able to resize the window,
            ///     but scrollbars will appear immediately.
            ///     <see cref = "MinWidth"/> should be used instead as it has a more desired effect.
            /// </remarks>
            /// <exception cref = "ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
            public int Width
            {
                get
                {
                    return width;
                }

                set
                {
                    if (value <= 0)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    width = value;
                }
            }

            /// <summary>
            ///     Adds a widget to the dialog.
            /// </summary>
            /// <param name = "widget">Widget to add to the dialog.</param>
            /// <param name = "widgetLayout">Location of the widget on the grid layout.</param>
            /// <returns>The dialog.</returns>
            /// <exception cref = "ArgumentNullException">When the widget is null.</exception>
            /// <exception cref = "ArgumentException">When the widget has already been added to the dialog.</exception>
            public Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Dialog AddWidget(Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget widget, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.IWidgetLayout widgetLayout)
            {
                if (widget == null)
                {
                    throw new System.ArgumentNullException("widget");
                }

                if (widgetLayouts.ContainsKey(widget))
                {
                    throw new System.ArgumentException("Widget is already added to the dialog");
                }

                widgetLayouts.Add(widget, widgetLayout);
                System.Collections.Generic.SortedSet<System.Int32> rowsInUse;
                System.Collections.Generic.SortedSet<System.Int32> columnsInUse;
                this.FillRowsAndColumnsInUse(out rowsInUse, out columnsInUse);
                return this;
            }

            /// <summary>
            ///     Adds a widget to the dialog.
            /// </summary>
            /// <param name = "widget">Widget to add to the dialog.</param>
            /// <param name = "row">Row location of widget on the grid.</param>
            /// <param name = "column">Column location of the widget on the grid.</param>
            /// <param name = "horizontalAlignment">Horizontal alignment of the widget.</param>
            /// <param name = "verticalAlignment">Vertical alignment of the widget.</param>
            /// <returns>The dialog.</returns>
            /// <exception cref = "ArgumentNullException">When the widget is null.</exception>
            /// <exception cref = "ArgumentException">When the widget has already been added to the dialog.</exception>
            public Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Dialog AddWidget(Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget widget, int row, int column, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.HorizontalAlignment horizontalAlignment = Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.HorizontalAlignment.Left, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.VerticalAlignment verticalAlignment = Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.VerticalAlignment.Center)
            {
                AddWidget(widget, new Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.WidgetLayout(row, column, horizontalAlignment, verticalAlignment));
                return this;
            }

            /// <summary>
            ///     Adds a widget to the dialog.
            /// </summary>
            /// <param name = "widget">Widget to add to the dialog.</param>
            /// <param name = "fromRow">Row location of widget on the grid.</param>
            /// <param name = "fromColumn">Column location of the widget on the grid.</param>
            /// <param name = "rowSpan">Number of rows the widget will use.</param>
            /// <param name = "colSpan">Number of columns the widget will use.</param>
            /// <param name = "horizontalAlignment">Horizontal alignment of the widget.</param>
            /// <param name = "verticalAlignment">Vertical alignment of the widget.</param>
            /// <returns>The dialog.</returns>
            /// <exception cref = "ArgumentNullException">When the widget is null.</exception>
            /// <exception cref = "ArgumentException">When the widget has already been added to the dialog.</exception>
            public Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Dialog AddWidget(Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget widget, int fromRow, int fromColumn, int rowSpan, int colSpan, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.HorizontalAlignment horizontalAlignment = Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.HorizontalAlignment.Left, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.VerticalAlignment verticalAlignment = Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.VerticalAlignment.Center)
            {
                AddWidget(widget, new Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.WidgetLayout(fromRow, fromColumn, rowSpan, colSpan, horizontalAlignment, verticalAlignment));
                return this;
            }

            /// <summary>
            ///     Shows the dialog window.
            ///     Also loads changes and triggers events when <paramref name = "requireResponse"/> is <c>true</c>.
            /// </summary>
            /// <param name = "requireResponse">If the dialog expects user interaction.</param>
            /// <remarks>Should only be used when you create your own event loop.</remarks>
            public void Show(bool requireResponse = true)
            {
                Skyline.DataMiner.Automation.UIBuilder uib = Build();
                uib.RequireResponse = requireResponse;
                Skyline.DataMiner.Automation.UIResults uir = Engine.ShowUI(uib);
                if (requireResponse)
                {
                    LoadChanges(uir);
                    RaiseResultEvents(uir);
                }
            }

            private static string AlignmentToUiString(Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.HorizontalAlignment horizontalAlignment)
            {
                switch (horizontalAlignment)
                {
                    case Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.HorizontalAlignment.Center:
                        return "Center";
                    case Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.HorizontalAlignment.Left:
                        return "Left";
                    case Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.HorizontalAlignment.Right:
                        return "Right";
                    case Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.HorizontalAlignment.Stretch:
                        return "Stretch";
                    default:
                        throw new System.ComponentModel.InvalidEnumArgumentException("horizontalAlignment", (int)horizontalAlignment, typeof(Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.HorizontalAlignment));
                }
            }

            private static string AlignmentToUiString(Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.VerticalAlignment verticalAlignment)
            {
                switch (verticalAlignment)
                {
                    case Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.VerticalAlignment.Center:
                        return "Center";
                    case Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.VerticalAlignment.Top:
                        return "Top";
                    case Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.VerticalAlignment.Bottom:
                        return "Bottom";
                    case Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.VerticalAlignment.Stretch:
                        return "Stretch";
                    default:
                        throw new System.ComponentModel.InvalidEnumArgumentException("verticalAlignment", (int)verticalAlignment, typeof(Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.VerticalAlignment));
                }
            }

            /// <summary>
            /// Checks if any visible widgets in the Dialog overlap.
            /// </summary>
            /// <exception cref = "OverlappingWidgetsException">Thrown when two visible widgets overlap with each other.</exception>
            private void CheckIfVisibleWidgetsOverlap()
            {
                if (AllowOverlappingWidgets)
                    return;
                foreach (Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget widget in widgetLayouts.Keys)
                {
                    if (!widget.IsVisible)
                        continue;
                    Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.IWidgetLayout widgetLayout = widgetLayouts[widget];
                    for (int column = widgetLayout.Column; column < widgetLayout.Column + widgetLayout.ColumnSpan; column++)
                    {
                        for (int row = widgetLayout.Row; row < widgetLayout.Row + widgetLayout.RowSpan; row++)
                        {
                            foreach (Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget otherWidget in widgetLayouts.Keys)
                            {
                                if (!otherWidget.IsVisible || widget.Equals(otherWidget))
                                    continue;
                                Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.IWidgetLayout otherWidgetLayout = widgetLayouts[otherWidget];
                                if (column >= otherWidgetLayout.Column && column < otherWidgetLayout.Column + otherWidgetLayout.ColumnSpan && row >= otherWidgetLayout.Row && row < otherWidgetLayout.Row + otherWidgetLayout.RowSpan)
                                {
                                    throw new Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.OverlappingWidgetsException(System.String.Format("The widget overlaps with another widget in the Dialog on Row {0}, Column {1}, RowSpan {2}, ColumnSpan {3}", widgetLayout.Row, widgetLayout.Column, widgetLayout.RowSpan, widgetLayout.ColumnSpan));
                                }
                            }
                        }
                    }
                }
            }

            private string GetRowDefinitions(System.Collections.Generic.SortedSet<System.Int32> rowsInUse)
            {
                string[] definitions = new string[rowsInUse.Count];
                int currentIndex = 0;
                foreach (int rowInUse in rowsInUse)
                {
                    string value;
                    if (rowDefinitions.TryGetValue(rowInUse, out value))
                    {
                        definitions[currentIndex] = value;
                    }
                    else
                    {
                        definitions[currentIndex] = Auto;
                    }

                    currentIndex++;
                }

                return System.String.Join(";", definitions);
            }

            private string GetColumnDefinitions(System.Collections.Generic.SortedSet<System.Int32> columnsInUse)
            {
                string[] definitions = new string[columnsInUse.Count];
                int currentIndex = 0;
                foreach (int columnInUse in columnsInUse)
                {
                    string value;
                    if (columnDefinitions.TryGetValue(columnInUse, out value))
                    {
                        definitions[currentIndex] = value;
                    }
                    else
                    {
                        definitions[currentIndex] = Auto;
                    }

                    currentIndex++;
                }

                return System.String.Join(";", definitions);
            }

            private Skyline.DataMiner.Automation.UIBuilder Build()
            {
                // Check rows and columns in use
                System.Collections.Generic.SortedSet<System.Int32> rowsInUse;
                System.Collections.Generic.SortedSet<System.Int32> columnsInUse;
                this.FillRowsAndColumnsInUse(out rowsInUse, out columnsInUse);
                // Check if visible widgets overlap and throw exception if this is the case
                CheckIfVisibleWidgetsOverlap();
                // Initialize UI Builder
                var uiBuilder = new Skyline.DataMiner.Automation.UIBuilder{Height = Height, MinHeight = MinHeight, Width = Width, MinWidth = MinWidth, RowDefs = GetRowDefinitions(rowsInUse), ColumnDefs = GetColumnDefinitions(columnsInUse), Title = Title};
                System.Collections.Generic.KeyValuePair<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.IWidgetLayout> defaultKeyValuePair = default(System.Collections.Generic.KeyValuePair<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.IWidgetLayout>);
                int rowIndex = 0;
                int columnIndex = 0;
                foreach (int rowInUse in rowsInUse)
                {
                    columnIndex = 0;
                    foreach (int columnInUse in columnsInUse)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.IWidgetLayout> keyValuePair in System.Linq.Enumerable.Where(widgetLayouts, x => x.Key.IsVisible && x.Key.Type != Skyline.DataMiner.Automation.UIBlockType.Undefined && x.Value.Row.Equals(rowInUse) && x.Value.Column.Equals(columnInUse)))
                        {
                            if (keyValuePair.Equals(defaultKeyValuePair))
                                continue;
                            // Can be removed once we retrieve all collapsed states from the UI
                            Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.TreeView treeView = keyValuePair.Key as Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.TreeView;
                            if (treeView != null)
                                treeView.UpdateItemCache();
                            Skyline.DataMiner.Automation.UIBlockDefinition widgetBlockDefinition = keyValuePair.Key.BlockDefinition;
                            Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.IWidgetLayout widgetLayout = keyValuePair.Value;
                            widgetBlockDefinition.Column = columnIndex;
                            widgetBlockDefinition.ColumnSpan = widgetLayout.ColumnSpan;
                            widgetBlockDefinition.Row = rowIndex;
                            widgetBlockDefinition.RowSpan = widgetLayout.RowSpan;
                            widgetBlockDefinition.HorizontalAlignment = AlignmentToUiString(widgetLayout.HorizontalAlignment);
                            widgetBlockDefinition.VerticalAlignment = AlignmentToUiString(widgetLayout.VerticalAlignment);
                            widgetBlockDefinition.Margin = widgetLayout.Margin.ToString();
                            uiBuilder.AppendBlock(widgetBlockDefinition);
                        }

                        columnIndex++;
                    }

                    rowIndex++;
                }

                return uiBuilder;
            }

            /// <summary>
            /// Used to retrieve the rows and columns that are being used and updates the RowCount and ColumnCount properties based on the Widgets added to the dialog.
            /// </summary>
            /// <param name = "rowsInUse">Collection containing the rows that are defined by the Widgets in the Dialog.</param>
            /// <param name = "columnsInUse">Collection containing the columns that are defined by the Widgets in the Dialog.</param>
            private void FillRowsAndColumnsInUse(out System.Collections.Generic.SortedSet<System.Int32> rowsInUse, out System.Collections.Generic.SortedSet<System.Int32> columnsInUse)
            {
                rowsInUse = new System.Collections.Generic.SortedSet<System.Int32>();
                columnsInUse = new System.Collections.Generic.SortedSet<System.Int32>();
                foreach (System.Collections.Generic.KeyValuePair<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Widget, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.IWidgetLayout> keyValuePair in this.widgetLayouts)
                {
                    if (keyValuePair.Key.IsVisible && keyValuePair.Key.Type != Skyline.DataMiner.Automation.UIBlockType.Undefined)
                    {
                        for (int i = keyValuePair.Value.Row; i < keyValuePair.Value.Row + keyValuePair.Value.RowSpan; i++)
                        {
                            rowsInUse.Add(i);
                        }

                        for (int i = keyValuePair.Value.Column; i < keyValuePair.Value.Column + keyValuePair.Value.ColumnSpan; i++)
                        {
                            columnsInUse.Add(i);
                        }
                    }
                }

                this.RowCount = rowsInUse.Count;
                this.ColumnCount = columnsInUse.Count;
            }

            private void LoadChanges(Skyline.DataMiner.Automation.UIResults uir)
            {
                foreach (Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.InteractiveWidget interactiveWidget in System.Linq.Enumerable.OfType<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.InteractiveWidget>(Widgets))
                {
                    if (interactiveWidget.IsVisible)
                    {
                        interactiveWidget.LoadResult(uir);
                    }
                }
            }

            private void RaiseResultEvents(Skyline.DataMiner.Automation.UIResults uir)
            {
                if (Interacted != null)
                {
                    Interacted(this, System.EventArgs.Empty);
                }

                if (uir.WasBack() && (Back != null))
                {
                    Back(this, System.EventArgs.Empty);
                    return;
                }

                if (uir.WasForward() && (Forward != null))
                {
                    Forward(this, System.EventArgs.Empty);
                    return;
                }

                // ToList is necessary to prevent InvalidOperationException when adding or removing widgets from a event handler.
                System.Collections.Generic.List<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.InteractiveWidget> intractableWidgets = System.Linq.Enumerable.ToList(System.Linq.Enumerable.Where(System.Linq.Enumerable.OfType<Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.InteractiveWidget>(Widgets), widget => widget.WantsOnChange));
                foreach (Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.InteractiveWidget intractable in intractableWidgets)
                {
                    intractable.RaiseResultEvents();
                }
            }
        }

        /// <summary>
        ///		Dialog used to display a message.
        /// </summary>
        public class MessageDialog : Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Dialog
        {
            private readonly Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Label messageLabel = new Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Label();
            /// <summary>
            /// Initializes a new instance of the <see cref = "MessageDialog"/> class without a message.
            /// </summary>
            /// <param name = "engine">Link with DataMiner.</param>
            public MessageDialog(Skyline.DataMiner.Automation.IEngine engine): base(engine)
            {
                OkButton = new Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Button("OK")
                {Width = 150};
                AddWidget(messageLabel, 0, 0);
                AddWidget(OkButton, 1, 0);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref = "MessageDialog"/> class with a specific message.
            /// </summary>
            /// <param name = "engine">Link with DataMiner.</param>
            /// <param name = "message">Message to be displayed in the dialog.</param>
            public MessageDialog(Skyline.DataMiner.Automation.IEngine engine, System.String message): this(engine)
            {
                Message = message;
            }

            /// <summary>
            /// Message to be displayed in the dialog.
            /// </summary>
            public string Message
            {
                get
                {
                    return messageLabel.Text;
                }

                set
                {
                    messageLabel.Text = value;
                }
            }

            /// <summary>
            /// Button that is displayed below the message.
            /// </summary>
            public Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Button OkButton
            {
                get;
                private set;
            }
        }

        /// <summary>
        /// This exception is used to indicate that two widgets have overlapping positions on the same dialog.
        /// </summary>
        [System.Serializable]
        public class OverlappingWidgetsException : System.Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref = "OverlappingWidgetsException"/> class.
            /// </summary>
            public OverlappingWidgetsException()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref = "OverlappingWidgetsException"/> class with a specified error message.
            /// </summary>
            /// <param name = "message">The message that describes the error.</param>
            public OverlappingWidgetsException(string message): base(message)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref = "OverlappingWidgetsException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
            /// </summary>
            /// <param name = "message">The error message that explains the reason for the exception.</param>
            /// <param name = "inner">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
            public OverlappingWidgetsException(string message, System.Exception inner): base(message, inner)
            {
            }

            /// <summary>
            /// Initializes a new instance of the OverlappingWidgetException class with the serialized data.
            /// </summary>
            /// <param name = "info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
            /// <param name = "context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
            protected OverlappingWidgetsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
            {
            }
        }

        /// <summary>
        /// This exception is used to indicate that a tree view contains multiple items with the same key.
        /// </summary>
        [System.Serializable]
        public class TreeViewDuplicateItemsException : System.Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref = "TreeViewDuplicateItemsException"/> class.
            /// </summary>
            public TreeViewDuplicateItemsException()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref = "TreeViewDuplicateItemsException"/> class with a specified error message.
            /// </summary>
            /// <param name = "key">The key of the duplicate tree view items.</param>
            public TreeViewDuplicateItemsException(string key): base(System.String.Format("An item with key {0} is already present in the TreeView", key))
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref = "TreeViewDuplicateItemsException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
            /// </summary>
            /// <param name = "key">The key of the duplicate tree view items.</param>
            /// <param name = "inner">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
            public TreeViewDuplicateItemsException(string key, System.Exception inner): base(System.String.Format("An item with key {0} is already present in the TreeView", key), inner)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref = "TreeViewDuplicateItemsException"/> class with the serialized data.
            /// </summary>
            /// <param name = "info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
            /// <param name = "context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
            protected TreeViewDuplicateItemsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
            {
            }
        }

        /// <summary>
        /// Specifies the horizontal alignment of a widget added to a dialog or section.
        /// </summary>
        public enum HorizontalAlignment
        {
            /// <summary>
            /// Specifies that the widget will be centered across its assigned cell(s).
            /// </summary>
            Center,
            /// <summary>
            /// Specifies that the widget will be aligned to the left across its assigned cell(s).
            /// </summary>
            Left,
            /// <summary>
            /// Specifies that the widget will be aligned to the right across its assigned cell(s).
            /// </summary>
            Right,
            /// <summary>
            /// Specifies that the widget will be stretched horizontally across its assigned cell(s).
            /// </summary>
            Stretch
        }

        /// <summary>
        /// Used to define the position of an item in a grid layout.
        /// </summary>
        public interface ILayout
        {
            /// <summary>
            ///     Gets the column location of the widget on the grid.
            /// </summary>
            /// <remarks>The top-left position is (0, 0) by default.</remarks>
            int Column
            {
                get;
            }

            /// <summary>
            ///     Gets the row location of the widget on the grid.
            /// </summary>
            /// <remarks>The top-left position is (0, 0) by default.</remarks>
            int Row
            {
                get;
            }
        }

        /// <summary>
        /// Used to define the position of a widget in a grid layout.
        /// </summary>
        public interface IWidgetLayout : Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.ILayout
        {
            /// <summary>
            ///     Gets how many columns the widget spans on the grid.
            /// </summary>
            int ColumnSpan
            {
                get;
            }

            /// <summary>
            ///     Gets or sets the horizontal alignment of the widget.
            /// </summary>
            Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.HorizontalAlignment HorizontalAlignment
            {
                get;
                set;
            }

            /// <summary>
            ///     Gets or sets the margin around the widget.
            /// </summary>
            /// <exception cref = "ArgumentNullException">When the value is null.</exception>
            Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Margin Margin
            {
                get;
                set;
            }

            /// <summary>
            ///     Gets how many rows the widget spans on the grid.
            /// </summary>
            int RowSpan
            {
                get;
            }

            /// <summary>
            ///     Gets or sets the vertical alignment of the widget.
            /// </summary>
            Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.VerticalAlignment VerticalAlignment
            {
                get;
                set;
            }
        }

        /// <summary>
        /// Defines the whitespace that is displayed around a widget.
        /// </summary>
        public class Margin
        {
            private int bottom;
            private int left;
            private int right;
            private int top;
            /// <summary>
            /// Initializes a new instance of the Margin class.
            /// </summary>
            /// <param name = "left">Amount of margin on the left-hand side of the widget in pixels.</param>
            /// <param name = "top">Amount of margin at the top of the widget in pixels.</param>
            /// <param name = "right">Amount of margin on the right-hand side of the widget in pixels.</param>
            /// <param name = "bottom">Amount of margin at the bottom of the widget in pixels.</param>
            public Margin(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            /// <summary>
            /// Initializes a new instance of the Margin class.
            /// A margin is by default 3 pixels wide.
            /// </summary>
            public Margin(): this(3, 3, 3, 3)
            {
            }

            /// <summary>
            /// Initializes a new instance of the Margin class based on a string.
            /// This string should have the following syntax: left;top;right;bottom
            /// </summary>
            /// <exception cref = "ArgumentException">If the string does not match the predefined syntax, or if any of the margins is not a number.</exception>
            /// <param name = "margin">Margin in string format.</param>
            public Margin(string margin)
            {
                if (System.String.IsNullOrWhiteSpace(margin))
                {
                    left = 0;
                    top = 0;
                    right = 0;
                    bottom = 0;
                    return;
                }

                string[] splitMargin = margin.Split(';');
                if (splitMargin.Length != 4)
                    throw new System.ArgumentException("Margin should have the following format: left;top;right;bottom");
                if (!System.Int32.TryParse(splitMargin[0], out left))
                    throw new System.ArgumentException("Left margin is not a number");
                if (!System.Int32.TryParse(splitMargin[1], out top))
                    throw new System.ArgumentException("Top margin is not a number");
                if (!System.Int32.TryParse(splitMargin[2], out right))
                    throw new System.ArgumentException("Right margin is not a number");
                if (!System.Int32.TryParse(splitMargin[3], out bottom))
                    throw new System.ArgumentException("Bottom margin is not a number");
            }

            /// <summary>
            /// Amount of margin in pixels at the bottom of the widget.
            /// </summary>
            public int Bottom
            {
                get
                {
                    return bottom;
                }

                set
                {
                    if (value < 0)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    bottom = value;
                }
            }

            /// <summary>
            /// Amount of margin in pixels at the left-hand side of the widget.
            /// </summary>
            public int Left
            {
                get
                {
                    return left;
                }

                set
                {
                    if (value < 0)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    left = value;
                }
            }

            /// <summary>
            /// Amount of margin in pixels at the right-hand side of the widget.
            /// </summary>
            public int Right
            {
                get
                {
                    return right;
                }

                set
                {
                    if (value < 0)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    right = value;
                }
            }

            /// <summary>
            /// Amount of margin in pixels at the top of the widget.
            /// </summary>
            public int Top
            {
                get
                {
                    return top;
                }

                set
                {
                    if (value < 0)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    top = value;
                }
            }

            /// <inheritdoc/>
            public override string ToString()
            {
                return System.String.Join(";", new object[]{left, top, right, bottom});
            }
        }

        /// <summary>
        /// Used to define the position of a section in another section or dialog.
        /// </summary>
        public class SectionLayout : Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.ILayout
        {
            private int column;
            private int row;
            /// <summary>
            /// Initializes a new instance of the <see cref = "SectionLayout"/> class.
            /// </summary>
            /// <param name = "row">Row index of the cell that the top-left cell of the section will be mapped to.</param>
            /// <param name = "column">Column index of the cell that the top-left cell of the section will be mapped to.</param>
            public SectionLayout(int row, int column)
            {
                this.row = row;
                this.column = column;
            }

            /// <summary>
            ///     Gets or sets the column location of the section on the dialog grid.
            /// </summary>
            /// <remarks>The top-left position is (0, 0) by default.</remarks>
            public int Column
            {
                get
                {
                    return column;
                }

                set
                {
                    if (value < 0)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    column = value;
                }
            }

            /// <summary>
            ///     Gets or sets the row location of the section on the dialog grid.
            /// </summary>
            /// <remarks>The top-left position is (0, 0) by default.</remarks>
            public int Row
            {
                get
                {
                    return row;
                }

                set
                {
                    if (value < 0)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    row = value;
                }
            }
        }

        /// <summary>
        /// Style of the displayed text.
        /// </summary>
        public enum TextStyle
        {
            /// <summary>
            /// Default value, no explicit styling.
            /// </summary>
            None = 0,
            /// <summary>
            /// Text should be styled as a title.
            /// </summary>
            Title = 1,
            /// <summary>
            /// Text should be styled in bold.
            /// </summary>
            Bold = 2,
            /// <summary>
            /// Text should be styled as a heading.
            /// </summary>
            Heading = 3
        }

        /// <summary>
        /// Specifies the vertical alignment of a widget added to a dialog or section.
        /// </summary>
        public enum VerticalAlignment
        {
            /// <summary>
            /// Specifies that the widget will be centered vertically across its assigned cell(s).
            /// </summary>
            Center,
            /// <summary>
            /// Specifies that the widget will be aligned to the top of its assigned cell(s).
            /// </summary>
            Top,
            /// <summary>
            /// Specifies that the widget will be aligned to the bottom of its assigned cell(s).
            /// </summary>
            Bottom,
            /// <summary>
            /// Specifies that the widget will be stretched vertically across its assigned cell(s).
            /// </summary>
            Stretch
        }

        /// <inheritdoc/>
        public class WidgetLayout : Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.IWidgetLayout
        {
            private int column;
            private int columnSpan;
            private Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Margin margin;
            private int row;
            private int rowSpan;
            /// <summary>
            /// Initializes a new instance of the <see cref = "WidgetLayout"/> class.
            /// </summary>
            /// <param name = "fromRow">Row index of top-left cell.</param>
            /// <param name = "fromColumn">Column index of the top-left cell.</param>
            /// <param name = "rowSpan">Number of vertical cells the widget spans across.</param>
            /// <param name = "columnSpan">Number of horizontal cells the widget spans across.</param>
            /// <param name = "horizontalAlignment">Horizontal alignment of the widget.</param>
            /// <param name = "verticalAlignment">Vertical alignment of the widget.</param>
            public WidgetLayout(int fromRow, int fromColumn, int rowSpan, int columnSpan, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.HorizontalAlignment horizontalAlignment = Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.HorizontalAlignment.Left, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.VerticalAlignment verticalAlignment = Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.VerticalAlignment.Top)
            {
                Row = fromRow;
                Column = fromColumn;
                RowSpan = rowSpan;
                ColumnSpan = columnSpan;
                HorizontalAlignment = horizontalAlignment;
                VerticalAlignment = verticalAlignment;
                Margin = new Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Margin();
            }

            /// <summary>
            /// Initializes a new instance of the <see cref = "WidgetLayout"/> class.
            /// </summary>
            /// <param name = "row">Row index of the cell where the widget is placed.</param>
            /// <param name = "column">Column index of the cell where the widget is placed.</param>
            /// <param name = "horizontalAlignment">Horizontal alignment of the widget.</param>
            /// <param name = "verticalAlignment">Vertical alignment of the widget.</param>
            public WidgetLayout(int row, int column, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.HorizontalAlignment horizontalAlignment = Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.HorizontalAlignment.Left, Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.VerticalAlignment verticalAlignment = Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.VerticalAlignment.Top): this(row, column, 1, 1, horizontalAlignment, verticalAlignment)
            {
            }

            /// <summary>
            ///     Gets or sets the column location of the widget on the grid.
            /// </summary>
            public int Column
            {
                get
                {
                    return column;
                }

                set
                {
                    if (value < 0)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    column = value;
                }
            }

            /// <summary>
            ///     Gets or sets how many columns the widget spans on the grid.
            /// </summary>
            public int ColumnSpan
            {
                get
                {
                    return columnSpan;
                }

                set
                {
                    if (value <= 0)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    columnSpan = value;
                }
            }

            /// <inheritdoc/>
            public Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.HorizontalAlignment HorizontalAlignment
            {
                get;
                set;
            }

            /// <inheritdoc/>
            public Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.Margin Margin
            {
                get
                {
                    return margin;
                }

                set
                {
                    if (value == null)
                    {
                        throw new System.ArgumentNullException("value");
                    }

                    margin = value;
                }
            }

            /// <summary>
            ///     Gets or sets the row location of the widget on the grid.
            /// </summary>
            public int Row
            {
                get
                {
                    return row;
                }

                set
                {
                    if (value < 0)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    row = value;
                }
            }

            /// <summary>
            ///     Gets or sets how many rows the widget spans on the grid.
            /// </summary>
            public int RowSpan
            {
                get
                {
                    return rowSpan;
                }

                set
                {
                    if (value <= 0)
                    {
                        throw new System.ArgumentOutOfRangeException("value");
                    }

                    rowSpan = value;
                }
            }

            /// <inheritdoc/>
            public Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.VerticalAlignment VerticalAlignment
            {
                get;
                set;
            }

            /// <inheritdoc/>
            public override bool Equals(object obj)
            {
                Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.WidgetLayout other = obj as Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit.WidgetLayout;
                if (other == null)
                    return false;
                bool rowMatch = Row.Equals(other.Row);
                bool columnMatch = Column.Equals(other.Column);
                bool rowSpanMatch = RowSpan.Equals(other.RowSpan);
                bool columnSpanMatch = ColumnSpan.Equals(other.ColumnSpan);
                bool horizontalAlignmentMatch = HorizontalAlignment.Equals(other.HorizontalAlignment);
                bool verticalAlignmentMatch = VerticalAlignment.Equals(other.VerticalAlignment);
                bool rowParamsMatch = rowMatch && rowSpanMatch;
                bool columnParamsMatch = columnMatch && columnSpanMatch;
                bool alignmentParamsMatch = horizontalAlignmentMatch && verticalAlignmentMatch;
                return rowParamsMatch && columnParamsMatch && alignmentParamsMatch;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                return Row ^ Column ^ RowSpan ^ ColumnSpan ^ (int)HorizontalAlignment ^ (int)VerticalAlignment;
            }
        }
    }
}