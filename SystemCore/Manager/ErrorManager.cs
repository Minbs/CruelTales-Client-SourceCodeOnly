namespace CTC.SystemCore
{
	public enum ClientError
	{
		Unknown = 0,
		PlatformDRM_Steam,
	}

	public class ErrorManager : IManager
	{
		public ClientError ErrorType { get; private set; }
		public object Context { get; private set; }

		public string GetErrorMessage()
		{
			switch (ErrorType)
			{
				case ClientError.PlatformDRM_Steam:
					return $"There is no steam client!<br>Check your Steam client is running.";

				default:
					return $"Unknown error occur!";
			}
		}

		public void OnErrorOccur(ClientError errorType, object context = null)
		{
			ErrorType = errorType;
			Context = context;
		}

		public void Initialize()
		{
		}

		public void Release()
		{
		}
	}
}
