using CT.Common.Tools.Data;
using CT.Logger;
using CTC.Data;

namespace CTC.SystemCore
{
	public class UserDataManager : IManager
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(UserDataManager));

		public UserData UserData { get; private set; }

		public void LoadData()
		{
			UserData = LoadFromFile<UserData>();
		}

		public void SaveData()
		{
			SaveToFile(UserData);
		}

		public void Initialize() => LoadData();
		public void Release() => SaveData();

		private static string getDataFileName<T>()
		{
			string typeName = typeof(T).Name;
			return typeName.ToSnakeCase();
		}

		private static string getSavePath<T>()
		{
			return Global.Path.GetUserDataFilePath
			(
				getDataFileName<T>() + Global.File.JsonExtension
			);
		}

		/// <summary>객체를 지정된 위치에서 불러옵니다. 존재하지 않는다면 디폴드 값을 생성합니다.</summary>
		/// <returns>불러온 객체입니다.</returns>
		public static T LoadFromFile<T>() where T : new()
		{
			string loadPath = getSavePath<T>();

			var result = JsonHandler.TryReadObject<T>(loadPath);
			if (result.ResultType == JobResultType.Success)
			{
				_log.Info($"Json {nameof(T)} loaded!");
				return result.Value;
			}
			else
			{
				_log.Warn($"There is no json {nameof(T)}. Create default {nameof(T)}!");
				return new T();
			}
		}

		/// <summary>객체를 지정된 위치에 파일로 저장합니다.</summary>
		/// <param name="instance">저장할 객체 인스턴스입니다.</param>
		public static void SaveToFile<T>(T instance) where T : new()
		{
			if (instance == null)
			{
				instance = new T();
				_log.Warn($"There is no {nameof(T)} to save. Create default {nameof(T)} and save it.");
			}

			string savePath = getSavePath<T>();

			var result = JsonHandler.TryWriteObject(savePath, instance);
			if (result.ResultType == JobResultType.Success)
			{
				_log.Info($"Save {nameof(T)} to json format!");
			}
			else
			{
				_log.Error($"Fail to save {nameof(T)}!");
			}
		}
	}
}
