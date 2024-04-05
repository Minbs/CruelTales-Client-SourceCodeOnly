using System;
using System.Collections.Generic;
using CT.Logger;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace CTC.SystemCore
{
	public class SoundManager : MonoBehaviour, IManager, IInitializable
	{
		// Log
		private static readonly ILog _log = LogManager.GetLogger(typeof(SoundManager));

		// Is initialized
		private bool _isInitialized = false;
		
		// Effect sounds
		private Dictionary<int, EventInstance> _sfxEventInstanceTable = new();

		//private EventInstance[] _sfxEventInstanceArr;
		//private int _sfxEventInstanceArrIdx = 0;

		// Background sound
		private EventInstance _ambEventInstance;
		private EventInstance _bgmEventInstance;

		private const string _bgmParam = "BGM_Param";

		private float _bgmBusVolume = 1f;
		
		public void Initialize()
		{
			_isInitialized = true;
		}

		public void Release()
		{
			_ambEventInstance.release();
			_bgmEventInstance.release();

			var sounds = _sfxEventInstanceTable.Values;
			foreach (var sound in sounds)
			{
				sound.release();
			}
		}

		public bool IsInitialized()
		{
			return _isInitialized;
		}
		
		// Test Codes
		private void Start()
		{
			//PlayBGM("Sans");
		}

		private void Update()
		{
			/*
			if (Input.GetKeyDown(KeyCode.O))
			{
				_bgmEventInstance.stop(STOP_MODE.ALLOWFADEOUT);
			}
			if (Input.GetKeyDown(KeyCode.P))
			{
				PlayBGM("Sans");
			}

			if (Input.GetKeyDown(KeyCode.U))
			{
				_bgmBusVolume -= 0.1f;
				FMODUnity.RuntimeManager.GetBus("bus:/BGM").setVolume(_bgmBusVolume);
			}

			if (Input.GetKeyDown(KeyCode.I))
			{
				_bgmBusVolume += 0.1f;
				FMODUnity.RuntimeManager.GetBus("bus:/BGM").setVolume(_bgmBusVolume);
			}
			*/
		}

		/// <summary>
		/// Play AMB to accommodate scene.
		/// </summary>
		public void PlayAMB(string sceneName)
		{
			//_ambEventInstance.stop(STOP_MODE.ALLOWFADEOUT);
			
			// 신네임을 가져올 딕셔너리?
			if (!TryCreateInstance("event:/AMB/AMB_Default", out var instance))
			{
				_log.Error("There is no AMB sound!");
				return;
			}

			RuntimeManager.AttachInstanceToGameObject(instance, transform);
			instance.start();
		}
		
		/// <summary>
		/// Play BGM to accommodate scene.
		/// </summary>
		public void PlayBGM(string sceneName)
		{
			//_ambEventInstance.stop(STOP_MODE.ALLOWFADEOUT);

			// 신네임을 가져올 딕셔너리?
			if (!TryCreateInstance("event:/BGM/BGM_Default", out var instance))
			{
				_log.Error("There is no BGM sound!");
				return;
			}

			RuntimeManager.AttachInstanceToGameObject(instance, transform);
			instance.start();
		}

		public void ChangeBGMParam(float value)
		{
			_bgmEventInstance.setParameterByName(_bgmParam, value);
		}

		/// <summary>
		/// Play Dokza's sound to accommodate action.
		/// </summary>
		/// <param name="spineEvent"></param>
		/// <param name="objectTag"></param>
		/// <param name="position"></param>
		public void PlayPCSound(string spineEvent, string objectTag, Vector3 position)
		{
			playSound(SoundUtils.GetPathFromSpineEvent(spineEvent), objectTag, position);
		}

		/// <summary>
		/// Function for play sound using EventInstance pool.
		/// </summary>
		private void playSound(string path, string objectTag, Vector3 position)
		{
			if (TryCreateInstance(path, out var instance))
			{
				instance.set3DAttributes(position.To3DAttributes());
				instance.setParameterByName("SoundMatType", SoundUtils.GetParamFromTag(objectTag));
				instance.start();
			}
			else
			{
				_log.Error($"There is no sound : {path}");
			}
		}

		/// <summary>
		/// Function for play sound using EventInstance pool.
		/// </summary>
		private void playSound(string path, Vector3 position)
		{
			if (TryCreateInstance(path, out var instance))
			{
				instance.set3DAttributes(position.To3DAttributes());
				instance.start();
			}
			else
			{
				_log.Error($"There is no sound : {path}");
			}
		}

		public static bool TryCreateInstance(string path, out EventInstance instance)
		{
			instance = default(EventInstance);
			try
			{
				instance = RuntimeManager.CreateInstance(path);
				return true;
			}
			catch (Exception e)
			{
				_log.Error($"There is no such sound instance in : {path}\n{e.Message}");
			}

			return false;
		}
	}
}