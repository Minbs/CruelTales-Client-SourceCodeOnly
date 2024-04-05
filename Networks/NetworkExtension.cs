using System.Net;

namespace CTC.Networks
{
	public static class NetworkExtension
	{
		public static bool IsValidPort(int port)
		{
			return port > 0 && port < ushort.MaxValue;
		}

		/// <summary>IP Port 주소를 파싱합니다. 127.0.0.1:8000 형태입니다.</summary>
		/// <param name="s"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public static bool TryParseIPEndPoint(string s, out IPEndPoint result)
		{
			int splitIndex = s.LastIndexOf(':');

			if (splitIndex > 0 && splitIndex + 1 < s.Length)
			{
				string ipValue = s[..splitIndex];
				string portValue = s[(splitIndex + 1)..];

				if (IPAddress.TryParse(ipValue, out var ipAddress)
					&& int.TryParse(portValue, out var port)
					&& IsValidPort(port))
				{
					result = new IPEndPoint(ipAddress, port);
					return true;
				}
			}

			result = null;
			return false;
		}
	}
}
