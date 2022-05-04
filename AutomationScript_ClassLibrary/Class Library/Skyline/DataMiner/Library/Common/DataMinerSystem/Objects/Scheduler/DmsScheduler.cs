namespace Skyline.DataMiner.Library.Common
{
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Net.Messages.Advanced;

	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Represents the DataMiner Scheduler component.
	/// </summary>
	internal class DmsScheduler : IDmsScheduler
	{
		private readonly IDma myDma;

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsScheduler"/> class.
		/// </summary>
		/// <param name="agent">The agent to which this scheduler component belongs to.</param>
		public DmsScheduler(IDma agent)
		{
			myDma = agent;
		}

		/// <summary>
		/// Retrieves all tasks on this agent.
		/// </summary>
		/// <returns>The tasks.</returns>
		public IEnumerable<IDmsSchedulerTask> GetTasks()
		{
			GetInfoMessage message = new GetInfoMessage
			{
				DataMinerID = myDma.Id,
				Type = InfoType.SchedulerTasks
			};

			var allMessages = myDma.Dms.Communication.SendMessage(message);

			foreach (var msg in allMessages)
			{
				GetSchedulerTasksResponseMessage typedMsg = (GetSchedulerTasksResponseMessage)msg;
				var tasks = typedMsg.Tasks;
				foreach (var task in tasks)
				{
					SchedulerTask typedTask = (SchedulerTask)task;
					yield return new DmsSchedulerTask(typedTask);
				}
			}
		}

		/// <summary>
		/// Creates the specified task.
		/// Replaces: slScheduler.SetInfo(userCookie, TSI_CREATE (1), taskData, out response);.
		/// </summary>
		/// <param name="createData">Array of data as expected by old Interop call.</param>
		/// <returns>The ID of the created task.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="createData"/> is <see langword="null"/>.</exception>
		public int CreateTask(object[] createData)
		{
			if (createData == null)
			{
				throw new ArgumentNullException("createData");
			}

			SetSchedulerInfoMessage message = new SetSchedulerInfoMessage
			{
				DataMinerID = myDma.Id,
				What = 1,
				Info = Int32.MaxValue,
				Ppsa = new Net.Messages.PPSA(createData)
			};

			SetSchedulerInfoResponseMessage result = (SetSchedulerInfoResponseMessage)myDma.Dms.Communication.SendSingleResponseMessage(message);

			return result.iRet;
		}

		/// <summary>
		/// Updates the specified task.
		/// Replaces: slScheduler.SetInfo(userCookie, TSI_UPDATE (2), taskData, out response);
		/// </summary>
		/// <param name="updateData">Array of data as expected by old Interop call.</param>
		/// <returns>Returns 0 if successful.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="updateData"/> is <see langword="null"/>.</exception>
		public int UpdateTask(object[] updateData)
		{
			if (updateData == null)
			{
				throw new ArgumentNullException("updateData");
			}

			SetSchedulerInfoMessage message = new SetSchedulerInfoMessage
			{
				DataMinerID = myDma.Id,
				What = 2,
				Info = Int32.MaxValue,
				Ppsa = new Net.Messages.PPSA(updateData)
			};

			SetSchedulerInfoResponseMessage result = (SetSchedulerInfoResponseMessage)myDma.Dms.Communication.SendSingleResponseMessage(message);

			return result.iRet;
		}

		/// <summary>
		/// Replaces: slScheduler.SetInfo(userCookie, TSI_DELETE (3), Convert.ToUInt32(TaskId), out response);
		/// </summary>
		/// <param name="taskId">The ID of the deleted task.</param>
		/// <returns>Returns 0 if successful.</returns>
		public int DeleteTask(int taskId)
		{
			SetSchedulerInfoMessage message = new SetSchedulerInfoMessage()
			{
				DataMinerID = myDma.Id,
				What = 3,
				Info = taskId
			};

			SetSchedulerInfoResponseMessage result = (SetSchedulerInfoResponseMessage)myDma.Dms.Communication.SendSingleResponseMessage(message);

			return result.iRet;
		}

		/// <summary>
		/// Retrieves the Scheduler status.
		/// Replaces: slScheduler.GetInfo(userCookie, TSI_STATUS (13), out response);
		/// </summary>
		public object GetStatus()
		{
			GetSchedulerInfoMessage message = new GetSchedulerInfoMessage
			{
				DataMinerID = myDma.Id,
				What = 13
			};

			GetSchedulerInfoResponseMessage result = (GetSchedulerInfoResponseMessage)myDma.Dms.Communication.SendSingleResponseMessage(message);
			return PSA.ToInteropArray(result.psaRet);
		}
	}
}