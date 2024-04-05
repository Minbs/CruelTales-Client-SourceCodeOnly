using Cysharp.Threading.Tasks;
using Steamworks;
using UnityEngine;

namespace CTC.Networks.SteamworksCore
{
	public static class SteamUtilsExtension
	{
		public static async UniTask<Texture2D> GetTextureFromSteamIDAsync(SteamId id)
		{
			var avatarImage = await SteamFriends.GetLargeAvatarAsync(id);
			Steamworks.Data.Image image = avatarImage.Value;

			int width = (int)image.Width;
			int height = (int)image.Height;

			Texture2D texture = new Texture2D(width, height);

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					Steamworks.Data.Color pixel = image.GetPixel(x, y);
					texture.SetPixel(x, height - y, pixel.ToUnityColor());
				}
			}

			texture.Apply();
			return texture;
		}
	}
}
