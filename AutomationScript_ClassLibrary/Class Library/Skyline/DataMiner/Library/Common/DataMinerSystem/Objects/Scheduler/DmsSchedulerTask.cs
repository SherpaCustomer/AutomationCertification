namespace Skyline.DataMiner.Library.Common
{
	using Skyline.DataMiner.Net.Messages;

	using System;

	/// <summary>
	/// Represents a Scheduler task.
	/// </summary>
	internal class DmsSchedulerTask : IDmsSchedulerTask
	{
		/// <summary>
		/// Gets a value indicating whether to choose the Agent.
		/// </summary>
		/// <value><c>true</c> to choose Agent; otherwise, <c>false</c>.</value>
		public bool ChooseAgent { get; set; }

		/// <summary>
		/// Gets the description of the task.
		/// </summary>
		/// <value>The description of the task.</value>
		public string Description { get; set; }

		/// <summary>
		/// Gets a value indicating whether the task is enabled.
		/// </summary>
		/// <value><c>true</c> if the task is enabled; otherwise, <c>false</c>.</value>
		public bool IsEnabled { get; set; }

		/// <summary>
		/// Gets the end time of the task.
		/// </summary>
		/// <value>The end time of the task.</value>
		public DateTime EndTime { get; set; }

		/// <summary>
		/// Gets a value indicating whether the task has run.
		/// </summary>
		/// <value><c>true</c> if the task has run; otherwise, <c>false</c>.</value>
		public int HasRun { get; set; }

		/// <summary>
		/// Gets a value indicating whether the task is finished.
		/// </summary>
		/// <value><c>true</c> if the task is finished; otherwise, <c>false</c>.</value>
		public bool IsFinished { get; set; }

		/// <summary>
		/// Gets the ID of the Agent that is handling the task.
		/// </summary>
		/// <value>The ID of the Agent that is handling the task.</value>
		public int HandlingAgentId { get; set; }

		/// <summary>
		/// Gets the ID of the task.
		/// </summary>
		/// <value>The ID of the task.</value>
		public int Id { get; set; }

		/// <summary>
		/// Get the result of the last run.
		/// </summary>
		/// <value>The result of the last run.</value>
		public string LastRunResult { get; set; }

		/// <summary>
		/// Get the time it took for the last run.
		/// </summary>
		/// <value>The time it took for the last run.</value>
		public string LastRunTime { get; set; }

		/// <summary>
		/// Gets the time until the next run.
		/// </summary>
		/// <value>The time until the next run.</value>
		public string NextRunTime { get; set; }

		/// <summary>
		/// Gets the number of repetitions of this task.
		/// </summary>
		/// <value>The number of repetitions of this task.</value>
		public int Repetitions { get; set; }

		/// <summary>
		/// Gets the repetition interval.
		/// </summary>
		/// <value>The repetition interval.</value>
		public string RepetitionInterval { get; set; }

		/// <summary>
		/// Gets the repetition interval expressed in minutes.
		/// </summary>
		/// <value>The repetition interval expressed in minutes.</value>
		public string RepetitionIntervalInMinutes { get; set; }

		/// <summary>
		/// Gets a value indicating whether the task is visible.
		/// </summary>
		/// <value><c>true</c> if the task is visible; otherwise, <c>false</c>.</value>
		public bool Show { get; set; }

		/// <summary>
		/// Gets the start time of the task.
		/// </summary>
		/// <value>The start time of the task.</value>
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets the name of the task.
		/// </summary>
		/// <value>The name of the task.</value>
		public string TaskName { get; set; }

		/// <summary>
		/// Gets the repetition type.
		/// </summary>
		/// <value>The repetition type.</value>
		public DmsSchedulerRepetitionType RepetitionType { get; set; }

		internal DmsSchedulerTask()
		{
		}

		internal DmsSchedulerTask(SchedulerTask slnetTask)
		{
			ChooseAgent = slnetTask.ChooseDMA;
			Description = slnetTask.Description;
			IsEnabled = slnetTask.Enabled;
			EndTime = slnetTask.EndTime;
			HasRun = slnetTask.Executed;
			IsFinished = slnetTask.Finished;
			HandlingAgentId = slnetTask.HandlingDMA;
			Id = slnetTask.Id;
			LastRunResult = slnetTask.LastExecuteResult;
			LastRunTime = slnetTask.LastRunTime;
			NextRunTime = slnetTask.NextRunTime;
			Repetitions = slnetTask.Repeat;
			RepetitionInterval = slnetTask.RepeatInterval;
			RepetitionIntervalInMinutes = slnetTask.RepeatIntervalInMinutes;
			Show = slnetTask.Show;
			StartTime = slnetTask.StartTime;
			TaskName = slnetTask.TaskName;
			RepetitionType = ConvertSLNetRepeatType(slnetTask.RepeatType);
			//slnetTask.Actions;
			//slnetTask.FinalActions;
		}

		private static DmsSchedulerRepetitionType ConvertSLNetRepeatType(SchedulerRepeatType type)
		{
			if (Enum.IsDefined(typeof(DmsSchedulerRepetitionType), (int)type))
			{
				return (DmsSchedulerRepetitionType)(int)type;
			}
			else
			{
				return DmsSchedulerRepetitionType.Undefined;
			}
		}
	}
}