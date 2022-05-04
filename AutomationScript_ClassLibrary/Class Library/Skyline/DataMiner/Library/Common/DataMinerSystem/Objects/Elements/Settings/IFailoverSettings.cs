namespace Skyline.DataMiner.Library.Common
{
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
		string ForceAgent { get; set; }

		/// <summary>
		/// Gets a value indicating whether the element is a failover element and is online on the backup agent instead of this agent.
		/// </summary>
		/// <value><c>true</c> if the element is a failover element and is online on the backup agent instead of this agent; otherwise, <c>false</c>.</value>
		bool IsOnlineOnBackupAgent { get; }

		/// <summary>
		/// Gets or sets a value indicating whether the element is a failover element that needs to keep running on the same DataMiner agent event after switching.
		/// </summary>
		/// <value><c>true</c> if the element is a failover element that needs to keep running on the same DataMiner agent event after switching; otherwise, <c>false</c>.</value>
		bool KeepOnline { get; set; }
    }
}