using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RJ_Editor
{
	/*
	 = Kodowanie pliku win-1250
= Dane oddzielone spacją, jeden wpis pociąg w jednej linii, "none" jeśli parametr jest zbędny, 0 i 1 jako bool, dummy jest nieużywane
= Pierwsza linia
	= "info"
	= string	Skrót stacji(dostępne "Tw", "Ls", "Lm")
	= dd.mm.yyyy albo yyyy	Data początku symulacji, wskazuje na poniedziałek jesli pelna data
	= string	Autor
	= string	Opis

	 */
	class Timetable
	{
		private string stationName, author, fileDescription;
		private DateTime startDate;


		public Timetable timeTabInst;
		public Timetable(string startDate, string stationName, string author, string fileDescription)
		{
			StartDate = startDate;
			StationName = stationName;
			Author = author;
			FileDescription = fileDescription;
		}

		public Timetable()
		{
		}

		public string StationName
		{
			get { return stationName; }
			set
			{
				if (value.ToUpper() == "TW" || value.ToUpper() == "LS" || value.ToUpper() == "LM")
				{
					stationName = value;
				}
				else
					throw new ArgumentOutOfRangeException(value + " nie jest właściwą nazwą posterunku");
			}
		}
		public string Author { get => author; set => author = value; }
		public string FileDescription { get => fileDescription; set => fileDescription = value; }
		public string StartDate
		{
			get
			{
				if (startDate.Month == 1 && startDate.Day == 1)
					return startDate.ToString("yyyy", CultureInfo.CreateSpecificCulture("de-DE"));
				return startDate.ToString("d", CultureInfo.CreateSpecificCulture("de-DE"));
			}
			set
			{
				Regex rFullDate = new Regex(@"^(\d+).(\d+).(\d+)");
				Regex rSimpleDate = new Regex(@"^(\d+)");
				Match m;
				DateTime date;
				if (rFullDate.IsMatch(value))
				{
					m = rFullDate.Match(value);
					DateTime tmp = new DateTime(Convert.ToInt32(m.Groups[3].Value),
						Convert.ToInt32(m.Groups[2].Value),
						Convert.ToInt32(m.Groups[1].Value));
					if (tmp.DayOfWeek == DayOfWeek.Monday)
						startDate = tmp;
					else
						throw new ArgumentException(value + " is not a Monday");
				}
				else if (rSimpleDate.IsMatch(value))
				{
					m = rSimpleDate.Match(value);
					startDate = new DateTime(Convert.ToInt32(m.Groups[1].Value), 1, 1);
				}
				else
					throw new ArgumentException(value + " is not a valid date");

			}
		}





	}
}
