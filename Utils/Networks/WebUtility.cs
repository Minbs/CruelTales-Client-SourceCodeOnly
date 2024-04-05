using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace CTC.Utils.Networks
{
	public static class WebUtility
	{
		public static async UniTask<string> GetDataOnEditorAsync(string url)
		{
			UnityWebRequest www = UnityWebRequest.Get(url);
			await www.SendWebRequest();
			return www.downloadHandler.text;
		}
	}
}