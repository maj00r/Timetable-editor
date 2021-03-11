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

		private static string fileContent = "";
		internal static List<Train> Trains { get => trains; set => trains = value; }
        internal static Timetable Timetable { get => timetable; set => timetable = value; }

		public static void FindPreviousInCycle()
        {
			foreach(Train train in Trains)
            {
				if (train.PreviousInCycle.HasValue)
                {
					if (!(Trains.Any(item => item.TrainNumber == train.PreviousInCycle.Value)))
						throw new ArgumentNullException("PreviousInCycle","Train " + train.TrainNumber + " has reference to " + train.PreviousInCycle + " which is missing in timetable");

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
		
			foreach(Train train in Trains)
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
		public static async void parse(string filePath)
		{
			OpenFile(filePath);
			Timetable = parseTimetable();
			trains = parseTrains();
		}
		private static void OpenFile(string filePath)
		{
			
			Encoding encoding = Encoding.Default;
			using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
			using (StreamReader reader = new StreamReader(fileStream, Encoding.Default))
			{
				fileContent = reader.ReadToEnd();
				encoding = reader.CurrentEncoding;
				reader.Close();
			}
			byte[] encBytes = encoding.GetBytes(fileContent);
			byte[] utf8Bytes = Encoding.Convert(encoding, Encoding.UTF8, encBytes);
			fileContent = Encoding.UTF8.GetString(utf8Bytes);

			
		}
		private static TimeSpan? parseTime(string s)
		{
			Regex timeShort = new Regex(@"^\d\d:\d\d$");
			Regex timeLong = new Regex(@"^\d\d:\d\d:\d\d$");
			if (timeLong.IsMatch(s))
				return TimeSpan.ParseExact(s, "g", CultureInfo.CurrentCulture, TimeSpanStyles.None);
			else if (timeShort.IsMatch(s))
				return TimeSpan.ParseExact(s, "h\\:mm", CultureInfo.CurrentCulture, TimeSpanStyles.None);
			else
				return null;
		}
		private static bool parseBool(string s)
		{
			if (s == "1")
				return true;
			else
				return false;
		}
		
		private static string parseString(string s)
		{
			if (s == "none")
				 return null;
			return s.Replace('_', ' ');
		}
		private static List<Train> parseTrains()
		{
			trains.Clear();
			Regex regex = new Regex(@"(\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+)");
			
			MatchCollection trainsInput = regex.Matches(fileContent);

			int trainsCounts = trainsInput.Count;
		   
			foreach (Match match in trainsInput)
			{
				Train train = new Train();
				{
					train.ExitTracks = parseString(match.Groups[1].Value);
					
					train.FromPostToWaypoint = parseBool(match.Groups[2].Value);
					train.ExitDays = parseString(match.Groups[3].Value);


					train.ExitTime = parseTime(match.Groups[4].Value);


					try
					{
						train.ExitVelocity = Convert.ToUInt16(match.Groups[5].Value);
					}
					catch (FormatException)
					{
						train.ExitVelocity = null;
					}
					try
					{
						train.ExitProbability = Convert.ToInt16(match.Groups[6].Value);
					}
					catch (FormatException)
					{
						train.ExitProbability = 100;
					}
					try
					{
						train.BeforeProbability = Convert.ToUInt16(match.Groups[7].Value);
					}
					catch (FormatException)
					{
						train.BeforeProbability = null;
					}
					try
					{
						train.DelayProbability = Convert.ToUInt16(match.Groups[8].Value);
					}
					catch (FormatException)
					{
						train.DelayProbability = null;
					}
					try
					{
						train.MaxBefore = Convert.ToUInt16(match.Groups[9].Value);
					}
					catch (FormatException)
					{
						train.MaxBefore = null;
					}
					try
					{
						train.MaxDelay = Convert.ToUInt16(match.Groups[10].Value);
					}
					catch (FormatException)
					{
						train.MaxDelay = null;
					}
					try
					{
						train.TrainNumber = Convert.ToUInt32(match.Groups[11].Value);
					}
					catch (FormatException)
					{
						throw new ArgumentException("Train number " + match.Groups[11].Value + " is not valid unsigned int32");
					}
					try
					{
						train.Priority = Convert.ToUInt16(match.Groups[12].Value);
					}
					catch (FormatException)
					{
						train.Priority = 6;
					}
					try
					{
						train.PreviousInCycle = Convert.ToUInt32(match.Groups[13].Value);
					}
					catch (FormatException)
					{
						train.PreviousInCycle = null;
					}
					try
					{
						train.WaitForPreviousInCycle = Convert.ToUInt16(match.Groups[14].Value);
					}
					catch (FormatException)
					{
						train.WaitForPreviousInCycle = null;
					}
					train.TrainSetString = parseString(match.Groups[16].Value);
					train.IdentW4 = parseString(match.Groups[17].Value);
					train.DepartureSignal = parseString(match.Groups[18].Value);
					train.ThisDeparture = parseTime(match.Groups[19].Value);
					train.PhoneDescription = parseString(match.Groups[22].Value);
					train.TrainName = parseString(match.Groups[24].Value);
					train.FromPost = parseString(match.Groups[25].Value);
					train.ToPost = parseString(match.Groups[26].Value);

					try
					{
						int tmp = Convert.ToInt32(match.Groups[27].Value);
						if (tmp < 0)
						{
							train.ChangeNumberAuto = true;
							train.NextInCycle = (uint?)Math.Abs(tmp);
						}
					}
					catch (FormatException)
					{
						train.ChangeNumberAuto = false;
						train.NextInCycle = null;
					}

					train.NextInputSignal = parseString(match.Groups[28].Value);
					try
					{
						train.ColorExtract = Convert.ToUInt32(match.Groups[29].Value);
					}
					catch (Exception)
					{
						train.ColorExtract = 16717567;  // pink 
						//throw new FormatException(match.Groups[29].Value + " is not a valid color as decimal");
					}
					Regex BIGsmall = new Regex(@"([A-Z]+)([a-z]+)");
					Match BIGsamllMatch = BIGsmall.Match(match.Groups[30].Value);

					train.Type3L = BIGsamllMatch.Groups[1].Value;
					train.Carieer = BIGsamllMatch.Groups[2].Value;
					train.PreviousDeparture = parseTime(match.Groups[31].Value);
					train.ThisArrival = parseTime(match.Groups[32].Value);
					if (!train.ThisDeparture.HasValue && !train.ThisArrival.HasValue)
                    {
						throw new ArgumentNullException("Train " + train.TrainNumber + " arrival and departure times cannot be empty");
                    }
					train.StationTrackNumber = parseString(match.Groups[33].Value);
					train.NextArrival = parseTime(match.Groups[34].Value);
					if (!(train.PreviousDeparture.HasValue == train.ThisArrival.HasValue))
                    {
						throw new ArgumentNullException("Train " + train.TrainNumber + " must have both previous departure and arrival fields empty or valid");
                    }
					if (!(train.NextArrival.HasValue == train.ThisDeparture.HasValue))
					{
						throw new ArgumentNullException("Train " + train.TrainNumber + " must have both previous departure and arrival fields empty or valid");
					}
					train.RelationFrom = parseString(match.Groups[35].Value);
					train.RelationTo = parseString(match.Groups[36].Value);
					train.ExtractDays = parseString(match.Groups[37].Value);
					train.AdditionalInfoSWDR = parseString(match.Groups[38].Value);
					train.IsExtractView = parseBool(match.Groups[39].Value);
					train.IsSWDRView = parseBool(match.Groups[40].Value);
					train.LoadSWDR = parseString(match.Groups[41].Value);
					train.OperatingNotesSWDR = parseString(match.Groups[42].Value);
					train.TrainTypeSWDR = parseString(match.Groups[43].Value);
					train.TimetableTypeSWDR = parseString(match.Groups[44].Value);
					try
					{
						train.GaugeProbability = Convert.ToUInt16(match.Groups[45].Value);

					}
					catch (FormatException)
					{
						train.GaugeProbability = 0;
					}
					try
					{
						train.TwrProbability = Convert.ToUInt16(match.Groups[46].Value);

					}
					catch (FormatException)
					{
						train.TwrProbability = 0;
					}
					train.IsSWDRQuality = parseBool(match.Groups[47].Value);
					
					try
					{
						NumberFormatInfo provider = new NumberFormatInfo();
						provider.NumberDecimalSeparator = ".";
						train.OrderedStop = TimeSpan.FromMinutes(Convert.ToDouble(match.Groups[48].Value.Replace(',', '.'), provider));
					}
					catch(FormatException)
					{
						train.OrderedStop = null;
					}
					train.StopTypeSWDR = parseString(match.Groups[49].Value);
					
				}

				trains.Add(train);
			}

			return trains;

		}

		private static Timetable parseTimetable()
		{
			Timetable instance;
			Regex r = new Regex(@"^info (\S+) (\S+) (\S+)? (\S+)?");
			Match m = r.Match(fileContent);
			instance = new Timetable(m.Groups[2].Value, m.Groups[1].Value.Replace('_', ' '), m.Groups[3].Value.Replace('_', ' '), m.Groups[4].Value.Replace('_', ' '));
			return instance;
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
