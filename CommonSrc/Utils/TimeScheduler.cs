using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IE.CommonSrc.Utils
{
	public class TimeScheduler
	{
		static private TimeScheduler scheduler_;
		private IDictionary<string, TimeSchedulerTask> currentTasks;
        private bool timerRunning_ = true;

		private TimeScheduler()
		{
			currentTasks = new Dictionary<string, TimeSchedulerTask>();

			LogMessage("Starting scheduler thread");

			var task3 = new Task(TimeSchedulerRunner,
					TaskCreationOptions.LongRunning | TaskCreationOptions.PreferFairness);

			task3.Start();
		}

		//
		// This is the main method in our thread - it looks to see which timers 
		// should be fired waiting a short while between each one....
		/// 
		///
		private void TimeSchedulerRunner()
		{
			LogMessage("Scheduler thread is running");
			bool workDone = false;
			while (timerRunning_)
			{
				try
				{
					if (workDone == false)
					{
						Task.Delay(TimeSpan.FromSeconds(2)).Wait();             // We wait two seconds....
					}
					else
					{
						workDone = false;
					}
					long nowInSeconds = DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;
					TimeSchedulerTask task = GetNextTask();
					if (task != null)
					{
						LogMessage("Checking time on task " + task.TaskName + " taskStartTime=" + task.TaskStartTime + " now=" + nowInSeconds);
						if (task.TaskStartTime <= nowInSeconds)
						{
							workDone = true;

							LogMessage("About to execute task " + task.TaskName);
							currentTasks.Remove(task.TaskName);             // Remove task from list - we are about to execute it...

							// Ok....we should be running this task now...
							Task<TimeSpan> func = task.InvokeFunction.Invoke();
							if (func != null)
							{
								func.Wait();

								if (func.IsFaulted)
								{
									LogMessage("Task " + task.TaskName + " faulted=" + func.Exception.Message + " stack=" + func.Exception.StackTrace);
								}

								//
								// Now we need to re-schedule it....
								//
								task.SetNextStartTime(func.Result);
								LogMessage("Rescheduling task " + task.TaskName + " for " + task.TaskStartTime);
								currentTasks.Add(task.TaskName, task);      // Put back onto queue for next time....
							}
							else
							{
								LogMessage("task " + task.TaskName + " returned null - will not be re-scheduled");
							}
						}
					}
				}
				catch (Exception e)
				{
					LogMessage("Runner has errored - " + e.Message);
				}
			}
		}

		private void LogMessage(string msg)
		{
			System.Diagnostics.Debug.WriteLine("IE: " + msg);
		}

		private TimeSchedulerTask GetNextTask()
		{
			if (currentTasks.Count == 0)
			{
				return null;
			}
			return currentTasks.Select(x => x.Value).OrderBy(x => x.TaskStartTime).First();
		}

		public void AddTask(string taskName, TimeSpan initialInterval, Func<Task<TimeSpan>> tick)
		{
			TimeSchedulerTask task = new TimeSchedulerTask(taskName, initialInterval, tick);
			if (currentTasks.ContainsKey(task.TaskName))
			{
				currentTasks.Remove(task.TaskName);
			}
			currentTasks.Add(task.TaskName, task);
		}

		public void RemoveTask(string taskName)
		{
			if (currentTasks.ContainsKey(taskName))
			{
				currentTasks.Remove(taskName);
			}
		}

        public void CloseDownScheduler() {
            timerRunning_ = false;
        }

		static public TimeScheduler GetTimeScheduler()
		{
			if (scheduler_ == null)
			{
				scheduler_ = new TimeScheduler();
			}

			return scheduler_;
		}
	}

	public class TimeSchedulerTask
	{
		public TimeSchedulerTask(string name, TimeSpan initialInterval, Func<Task<TimeSpan>> tick)
		{
			TaskName = name;
			SetNextStartTime(initialInterval);
			InvokeFunction = tick;
		}

		public void SetNextStartTime(TimeSpan when)
		{
			long startTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;
			startTime += (long)when.TotalSeconds;
			TaskStartTime = startTime;
		}
		public Func<Task<TimeSpan>> InvokeFunction { get; private set; }

		public string TaskName { get; private set; }
		public long TaskStartTime { get; private set; }
	}
}
