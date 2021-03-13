using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RJ_Editor
{
	static class Logic_Parser
	{
		private static Timetable timetable = new Timetable();
		private static List<Train> trains = new List<Train>();

		
		internal static List<Train> Trains { get => trains; set => trains = value; }
		internal static Timetable Timetable { get => timetable; set => timetable = value; }



		public static void FindPreviousInCycle()
		{
			foreach (Train train in Trains)
			{
				if (train.PreviousInCycle.HasValue)
				{
					if (!(Trains.Any(item => item.TrainNumber == train.PreviousInCycle.Value)))
						throw new ArgumentNullException("PreviousInCycle", "Train " + train.TrainNumber + " has reference to " + train.PreviousInCycle + " which is missing in timetable");

				}
			}
		}
		private static int CompareTimeSpans(Train x, Train y)
		{
			if (x.ThisArrival.HasValue && x.ThisDeparture.HasValue)
				throw new InvalidOperationException("train" + x.TrainNumber + " have arrival and departure values empty");
			if (y.ThisArrival.HasValue && y.ThisDeparture.HasValue)
				throw new InvalidOperationException("train" + y.TrainNumber + " have arrival and departure values empty");


			if (x.ThisArrival.HasValue)
			{
				if (y.ThisArrival.HasValue)
				{
					return x.ThisArrival.Value.CompareTo(y.ThisArrival.Value);
				}
				else
				{
					return x.ThisArrival.Value.CompareTo(y.ThisDeparture.Value);
				}
			}
			else
			{
				if (y.ThisArrival.HasValue)
				{
					return x.ThisDeparture.Value.CompareTo(y.ThisArrival.Value);
				}
				else
				{
					return 0;


				}
			}
		}
		public static void SortByTime()
		{

			foreach (Train train in Trains)
			{
				if (train.ThisArrival.HasValue == false)
				{
					train.ThisArrival = train.ThisDeparture.Value;
					train.ThisArrivalWasOvervritten = true;
				}
			}
			Trains.Sort(CompareTimeSpans);
			foreach (Train train in Trains)
			{
				if (train.ThisArrivalWasOvervritten)
					train.ThisArrival = null;
			}

		}
		

		public static Train DuplicateAndInterval(double interval, Train train_)
		{
			Train train = new Train(train_);

			// this is reference i need clone :<


			//swap relations, posts
			string tmp = train.RelationFrom;
			train.RelationFrom = train.RelationTo;
			train.RelationTo = tmp;
			tmp = train.FromPost;
			train.FromPost = train.ToPost;
			train.ToPost = tmp;
			// swaping timespans bad
			if (train.ThisDeparture.HasValue && train.ThisArrival.HasValue && train.NextArrival.HasValue && train.PreviousDeparture.HasValue)
			{
				TimeSpan stationStop = train.ThisDeparture.Value.Subtract(train.ThisArrival.Value);
				TimeSpan incomingTime = train.ThisArrival.Value.Subtract(train.PreviousDeparture.Value);
				TimeSpan outcomingTime = train.NextArrival.Value.Subtract(train.ThisDeparture.Value);

			}

			if (train.NextInCycle.HasValue)
			{
				train.TrainNumber = train.NextInCycle.Value;
			}
			else
			{
				//swaping first and second digit of number if it contains more than 2 digits
				if (train.TrainNumber >= 100)
				{
					int[] numberArray = train.TrainNumber.ToString().Select(o => int.Parse(o.ToString())).ToArray();
					int uiTmp = numberArray[0];
					numberArray[0] = numberArray[1];
					numberArray[1] = uiTmp;
					train.TrainNumber = Convert.ToUInt32(string.Join("", numberArray));
				}
				train.TrainNumber += 1;
			}
			if (train.FromPost == "wi")
				train.ExitTracks = "2,1";
			else
				train.ExitTracks = "1,2";
			train.TimeInterval(interval);
			return train;
		}
	}
}
