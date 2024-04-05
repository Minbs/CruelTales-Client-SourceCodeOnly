using CTC.DataBind.Contexts;
using Slash.Unity.DataBind.Core.Data;
using System;

namespace CTC.GUI.Gameplay.Timer
{
	public class Context_Timer : Context
	{
		private readonly Property<float>currentTimerCountProperty = new();
		private readonly Property<float> maxTimerCountProperty = new();
		private readonly Property<float> timerSliderValueProperty = new();
	

		public float CurrentTimerCount
		{
			get => currentTimerCountProperty.Value;
			set => currentTimerCountProperty.Value = value;
		}

		public float MaxTimerCount
		{
			get => maxTimerCountProperty.Value;
			set => maxTimerCountProperty.Value = value;
		}

		public float TimerSliderValue
		{
			//get  { return currentTimerCountProperty.Value / maxTimerCountProperty.Value; }
			get => timerSliderValueProperty.Value;
			set => timerSliderValueProperty.Value = value;
		}

		/*
		public void UpdateTimerSliderValue()
		{
			TimerSliderValue = CurrentTimerCount / MaxTimerCount;
		}
		*/ 
		/*
		public string CurrentTimerInt
		{
			get 
			{
				int num =(int)Math.Ceiling(currentTimerCountProperty.Value);
				return num.ToString(); 
			}
			set => currentTimerCountProperty.Value = int.Parse(value);
		}
		*/

	}
}
