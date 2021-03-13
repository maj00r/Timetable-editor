using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RJ_Editor
{
	class WriterToFile
	{
		public static void Write(string filePath)
		{
			FileStream fs_save = new FileStream(filePath, FileMode.Create, FileAccess.Write);
			StreamWriter sw = new StreamWriter(fs_save, Encoding.Default);
			sw.BaseStream.Seek(0, SeekOrigin.End);
			//sw.Write("ZAŻÓŁĆ GĘŚLĄ JAŹŃ");
			writeTimetable();
			writeTrains();
			sw.Close();

			void writeTimetable()
			{

				sw.WriteLine("info {0} {1} {2} {3}", Logic_Parser.Timetable.StationName, Logic_Parser.Timetable.StartDate, writeString(Logic_Parser.Timetable.Author), writeString(Logic_Parser.Timetable.FileDescription));
			}
			void writeTrains()
			{
				foreach (Train t in Logic_Parser.Trains)
				{
					string orderedstop;
					if (t.OrderedStop.HasValue)
						orderedstop = t.OrderedStop.Value.TotalMinutes.ToString().Replace(',', '.');
					else
						orderedstop = "none";

					sw.WriteLine("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13} 0 {14} {15} {16} {17} none none {18} none {19} {20} {21} {22}{23} {24} {25} {26}{27} {28} {29} {30} {31} {32} {33} {34} {35} {36} {37} {38} {39} {40} {41} {42} {43} {44} {45} {46}"
						//fields here
						, writeString(t.ExitTracks), writeBool(t.FromPostToWaypoint, false), writeString(t.ExitDays), writeTimeSpan(t.ExitTime), writeNumber<ushort?>(t.ExitVelocity), writeNumber<short>(t.ExitProbability), writeNumber<ushort?>(t.BeforeProbability), writeNumber<ushort?>(t.DelayProbability),
						writeNumber<ushort?>(t.MaxBefore), writeNumber<ushort?>(t.MaxDelay), writeNumber<uint>(t.TrainNumber), writeNumber(t.Priority), writeNumber(t.PreviousInCycle), writeNumber(t.WaitForPreviousInCycle), writeString(t.TrainSetString), writeString(t.IdentW4), writeString(t.DepartureSignal), writeTimeSpan(t.ThisDeparture), writeString(t.PhoneDescription),
						writeString(t.TrainName), writeString(t.FromPost), writeString(t.ToPost), writeBool(t.ChangeNumberAuto, true), writeNumber(t.NextInCycle), writeString(t.NextInputSignal), writeNumber(t.ColorExtract), writeString(t.Type3L), writeString(t.Carieer), writeTimeSpan(t.PreviousDeparture), writeTimeSpan(t.ThisArrival), writeString(t.StationTrackNumber), writeTimeSpan(t.NextArrival),
						writeString(t.RelationFrom), writeString(t.RelationTo), writeString(t.ExtractDays), writeString(t.AdditionalInfoSWDR), writeBool(t.IsExtractView, false), writeBool(t.IsSWDRView, false), writeString(t.LoadSWDR), writeString(t.OperatingNotesSWDR), writeString(t.TrainTypeSWDR), writeString(t.TimetableTypeSWDR), writeNumber(t.GaugeProbability), writeNumber(t.TwrProbability), writeBool(t.IsSWDRQuality, false),
						orderedstop, writeString(t.StopTypeSWDR)
						);
				}
			}


		}
		private static string writeBool(bool b, bool minusIfTrue)
		{
			if (minusIfTrue)
				return b ? "-" : "";
			return b ? "1" : "0";
		}
		private static string writeString(string s)
		{
			if (s == "" || s == null)
				return "none";

			return s.Replace(' ', '_');
		}
		static CultureInfo cultureInfo = new CultureInfo("fr-FR");
		private static string writeTimeSpan(TimeSpan? t)
		{
			return t.HasValue ? t.Value.ToString("hh\\:mm\\:ss", cultureInfo) : "none";
		}
		private static string writeNumber<T>(T n)
		{
			return n == null ? "none" : n.ToString();
		}
	}
}
