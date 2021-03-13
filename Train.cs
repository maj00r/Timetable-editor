using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RJ_Editor
{
	class Train
	{

		private bool fromPostToWaypoint, isExtractView, isSWDRView, isSWDRQuality, changeNumberAuto, thisArrivalWasOvervritten;

		private string exitDays, phoneDescription, trainName, fromPost, toPost, departureSignal,

			type3L, carieer, stationTrackNumber, relationFrom, relationTo, extractDays,

			additionalInfoSWDR, loadSWDR, operatingNotesSWDR, trainTypeSWDR, timetableTypeSWDR,

			stopTypeSWDR, nextInputSignal, trainSetString, exitTracks, identW4;
		private TimeSpan? exitTime, previousDeparture, thisArrival, thisDeparture, nextArrival, orderedStop;
		private ushort? exitVelocity, beforeProbability, delayProbability, maxBefore, maxDelay,
			waitForPreviousInCycle;
		private ushort priority,  gaugeProbability, twrProbability;
		private short exitProbability;
		private uint trainNumber, colorExtract;
		private uint? previousInCycle, nextInCycle;
		
		#region Contructors and properties

		public Train()
		{

		}

		public Train(Train original)
		{
			FromPostToWaypoint = original.fromPostToWaypoint;
			IsExtractView = original.isExtractView;
			IsSWDRView = original.isSWDRView;
			IsSWDRQuality = original.isSWDRQuality;
			ChangeNumberAuto = original.changeNumberAuto;
			ExitDays = original.exitDays;
			PhoneDescription = original.phoneDescription;
			TrainName = original.trainName;
			FromPost = original.fromPost;
			ToPost = original.toPost;
			DepartureSignal = original.departureSignal;
			Type3L = original.type3L;
			Carieer = original.carieer;
			StationTrackNumber = original.stationTrackNumber;
			RelationFrom = original.relationFrom;
			RelationTo = original.relationTo;
			ExtractDays = original.extractDays;
			AdditionalInfoSWDR = original.additionalInfoSWDR;
			LoadSWDR = original.loadSWDR;
			OperatingNotesSWDR = original.operatingNotesSWDR;
			TrainTypeSWDR = original.trainTypeSWDR;
			TimetableTypeSWDR = original.timetableTypeSWDR;
			StopTypeSWDR = original.stopTypeSWDR;
			NextInputSignal = original.nextInputSignal;
			TrainSetString = original.trainSetString;
			ExitTracks = original.exitTracks;
			IdentW4 = original.identW4;
			ExitTime = original.exitTime;
			PreviousDeparture = original.previousDeparture;
			ThisArrival = original.thisArrival;
			ThisDeparture = original.thisDeparture;
			NextArrival = original.nextArrival;
			OrderedStop = original.orderedStop;
			ExitVelocity = original.exitVelocity;
			BeforeProbability = original.beforeProbability;
			DelayProbability = original.delayProbability;
			MaxBefore = original.maxBefore;
			MaxDelay = original.maxDelay;
			PreviousInCycle = original.previousInCycle;
			WaitForPreviousInCycle = original.waitForPreviousInCycle;
			NextInCycle = original.nextInCycle;
			TrainNumber = original.trainNumber;
			Priority = original.priority;
			ColorExtract = original.colorExtract;
			GaugeProbability = original.gaugeProbability;
			TwrProbability = original.twrProbability;
			ExitProbability = original.exitProbability;
			ThisArrivalWasOvervritten = original.thisArrivalWasOvervritten;
		}

		public bool FromPostToWaypoint { get => fromPostToWaypoint; set => fromPostToWaypoint = value; }
		public bool IsExtractView { get => isExtractView; set => isExtractView = value; }
		public bool IsSWDRView { get => isSWDRView; set => isSWDRView = value; }
		public bool IsSWDRQuality { get => isSWDRQuality; set => isSWDRQuality = value; }
		public bool ChangeNumberAuto { get => changeNumberAuto; set => changeNumberAuto = value; }
		public string ExitDays { get => exitDays; set => exitDays = value; }
		public string PhoneDescription { get => phoneDescription; set => phoneDescription = value; }
		public string TrainName { get => trainName; set => trainName = value; }
		public string FromPost { get => fromPost; set => fromPost = value; }
		public string ToPost { get => toPost; set => toPost = value; }
		public string DepartureSignal { get => departureSignal; set => departureSignal = value; }
		public string Type3L { get => type3L; set => type3L = value; }
		public string Carieer { get => carieer; set => carieer = value; }
		public string StationTrackNumber { get => stationTrackNumber; set => stationTrackNumber = value; }
		public string RelationFrom { get => relationFrom; set => relationFrom = value; }
		public string RelationTo { get => relationTo; set => relationTo = value; }
		public string ExtractDays { get => extractDays; set => extractDays = value; }
		public string AdditionalInfoSWDR { get => additionalInfoSWDR; set => additionalInfoSWDR = value; }
		public string LoadSWDR { get => loadSWDR; set => loadSWDR = value; }
		public string OperatingNotesSWDR { get => operatingNotesSWDR; set => operatingNotesSWDR = value; }
		public string TrainTypeSWDR { get => trainTypeSWDR; set => trainTypeSWDR = value; }
		public string TimetableTypeSWDR { get => timetableTypeSWDR; set => timetableTypeSWDR = value; }
		public string StopTypeSWDR { get => stopTypeSWDR; set => stopTypeSWDR = value; }
		public string NextInputSignal { get => nextInputSignal; set => nextInputSignal = value; }
		public string TrainSetString { get => trainSetString; set => trainSetString = value; }
		public string ExitTracks { get => exitTracks; set => exitTracks = value; }
		public string IdentW4 { get => identW4; set => identW4 = value; }
		public TimeSpan? ExitTime { get => exitTime; set => exitTime = value; }
		public TimeSpan? PreviousDeparture { get => previousDeparture; set => previousDeparture = value; }
		public TimeSpan? ThisArrival { get => thisArrival; set => thisArrival = value; }
		public TimeSpan? ThisDeparture { get => thisDeparture; set => thisDeparture = value; }
		public TimeSpan? NextArrival { get => nextArrival; set => nextArrival = value; }
		public TimeSpan? OrderedStop { get => orderedStop; set => orderedStop = value; }
		public ushort? ExitVelocity { get => exitVelocity; set => exitVelocity = value; }
		public ushort? BeforeProbability { get => beforeProbability; set => beforeProbability = value; }
		public ushort? DelayProbability { get => delayProbability; set => delayProbability = value; }
		public ushort? MaxBefore { get => maxBefore; set => maxBefore = value; }
		public ushort? MaxDelay { get => maxDelay; set => maxDelay = value; }
		public uint? PreviousInCycle { get => previousInCycle; set => previousInCycle = value; }
		public ushort? WaitForPreviousInCycle { get => waitForPreviousInCycle; set => waitForPreviousInCycle = value; }
		public uint? NextInCycle { get => nextInCycle; set => nextInCycle = value; }
		public uint TrainNumber { get => trainNumber; set => trainNumber = value; }
		public ushort Priority { get => priority; set => priority = value; }
		public uint ColorExtract { get => colorExtract; set => colorExtract = value; }
		public ushort GaugeProbability { get => gaugeProbability; set => gaugeProbability = value; }
		public ushort TwrProbability { get => twrProbability; set => twrProbability = value; }
		public short ExitProbability { get => exitProbability; set => exitProbability = value; }
		public bool ThisArrivalWasOvervritten { get; set; }
		#endregion
		public void TimeInterval(double interval)
		{
			
			ExitTime = AddInterval(ExitTime, interval);
			PreviousDeparture = AddInterval(PreviousDeparture,interval);
			ThisArrival = AddInterval(ThisArrival, interval);
			ThisDeparture = AddInterval(ThisDeparture, interval);
			NextArrival = AddInterval(NextArrival, interval);

		   
		}
		public TimeSpan? AddInterval(TimeSpan? timeSpan, double interval)
		{
			TimeSpan day = new TimeSpan(1, 0, 0, 0);
			if (timeSpan.HasValue)
			{
				var tmp = timeSpan.Value + TimeSpan.FromMinutes(interval);
				if (tmp < TimeSpan.Zero) tmp += day;
				else if (tmp >= day) tmp -= day;
				return tmp;
			}
			return null;
		}

	}
}
