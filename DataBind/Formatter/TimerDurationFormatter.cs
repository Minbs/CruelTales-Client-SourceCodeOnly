//using System.Text;
//using Slash.Unity.DataBind.Core.Presentation;
//using UnityEngine;
//namespace CTC.Assets.Scripts.DataBind.Formatter
//{
//	public class TimerDurationFormatter : DataProvider
//	{
//		private const int SecondsPerMinute = 60;

//		/// <summary>
//		///     Data value which contains the duration in seconds.
//		/// </summary>
//		[Tooltip("Data value which contains the duration in seconds.")]
//		public DataBinding Argument;

//		/// <summary>
//		///     Text to show after the seconds value, if shown.
//		/// </summary>
//		[Tooltip("Text to show after the seconds value, if shown.")]
//		public DataBinding SecondsSymbol;

//		/// <summary>
//		///     <para>
//		///         How many time units to show.
//		///     </para>
//		///     <para>
//		///         Example: If set to two, will show hours and
//		///         minutes if specified time is shorter than a day,
//		///         or minutes and seconds if
//		///         specified time is shorter than an hour.
//		///     </para>
//		/// </summary>
//		[Tooltip(
//			"How many time units to show. Example: If set to two, will show hours and minutes if specified time is shorter than a day, or minutes and seconds if specified time is shorter than an hour."
//		)]
//		public int TimeUnits;

//		/// <inheritdoc />
//		public override object Value
//		{
//			get
//			{
//				var remainingSeconds = this.Argument.GetValue<float>();
//				var remainingTimeUnits = this.TimeUnits;

//				var daysSymbol = this.DaysSymbol.GetValue<string>();
//				var hoursSymbol = this.HoursSymbol.GetValue<string>();
//				var minutesSymbol = this.MinutesSymbol.GetValue<string>();
//				var secondsSymbol = this.SecondsSymbol.GetValue<string>();

//				// Split time into time units.
//				var text = new StringBuilder();

//				// Days.
//				var totalDays = remainingSeconds / SecondsPerDay;
//				var totalFullDays = (int)totalDays;

//				if (remainingTimeUnits > 0 && totalFullDays > 0)
//				{
//					remainingSeconds %= SecondsPerDay;
//					text.AppendFormat("{0}{1}", totalFullDays, daysSymbol);
//					--remainingTimeUnits;
//				}

//				// Hours.
//				var totalHours = remainingSeconds / SecondsPerHour;
//				var totalFullHours = (int)totalHours;

//				if (remainingTimeUnits > 0 && totalFullHours > 0)
//				{
//					remainingSeconds %= SecondsPerHour;
//					text.AppendFormat(" {0:00}{1}", totalFullHours, hoursSymbol);
//					--remainingTimeUnits;
//				}

//				// Minutes.
//				var totalMinutes = remainingSeconds / SecondsPerMinute;
//				var totalFullMinutes = (int)totalMinutes;

//				if (remainingTimeUnits > 0 && totalFullMinutes > 0)
//				{
//					remainingSeconds %= SecondsPerMinute;
//					text.AppendFormat(" {0:00}{1}", totalFullMinutes, minutesSymbol);
//					--remainingTimeUnits;
//				}

//				// Seconds.
//				var totalFullSeconds = (int)remainingSeconds;

//				if (remainingTimeUnits > 0 && totalFullSeconds > 0)
//				{
//					text.AppendFormat(" {0:00}{1}", totalFullSeconds, secondsSymbol);
//				}

//				return text.ToString().Trim();
//			}
//		}

//		/// <inheritdoc />
//		public override void Deinit()
//		{
//			this.RemoveBinding(this.Argument);
//			this.RemoveBinding(this.DaysSymbol);
//			this.RemoveBinding(this.HoursSymbol);
//			this.RemoveBinding(this.MinutesSymbol);
//			this.RemoveBinding(this.SecondsSymbol);
//		}

//		/// <inheritdoc />
//		public override void Init()
//		{
//			this.AddBinding(this.Argument);
//			this.AddBinding(this.DaysSymbol);
//			this.AddBinding(this.HoursSymbol);
//			this.AddBinding(this.MinutesSymbol);
//			this.AddBinding(this.SecondsSymbol);
//		}

//		/// <inheritdoc />
//		protected override void UpdateValue()
//		{
//			this.OnValueChanged();
//		}
//	}
//}
