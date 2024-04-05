using System.Drawing;
using System.Runtime.CompilerServices;
using CT.Common;
using CT.Common.Gameplay;

namespace CTC.Tests
{
	public static class SkinSetHelper
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int[] GetArray(SkinSet skinSet)
		{
			int[] returnArr = new int[15];

			returnArr[0] = skinSet.Back;
			returnArr[1] = skinSet.Tail;
			returnArr[2] = skinSet.Cheek;
			returnArr[3] = skinSet.Dress;
			returnArr[4] = skinSet.Eyes;
			returnArr[5] = skinSet.Eyebrow;
			returnArr[6] = skinSet.FaceAcc;
			returnArr[7] = skinSet.Hair;
			returnArr[8] = skinSet.HairAcc;
			returnArr[9] = skinSet.HairHelmet;
			returnArr[10] = skinSet.Headgear;
			returnArr[11] = skinSet.Lips;
			returnArr[12] = skinSet.Nose;
			returnArr[13] = skinSet.Shoes;
			returnArr[14] = skinSet.Hammer;
			
			return returnArr;
		}
		
		public static SkinSet GetDefaultSkinSet(int idx = 0)
		{
			SkinSet returnSkinSet = new SkinSet();

			returnSkinSet.Back = 2000003;
			returnSkinSet.Tail = 3000005;
			returnSkinSet.Cheek = 4000001;
			returnSkinSet.Dress = 5000002;
			returnSkinSet.Eyebrow = 10000001;
			returnSkinSet.FaceAcc = 13000001;
			returnSkinSet.Hair = 14000002;
			returnSkinSet.HairAcc = 15000005;

			returnSkinSet.SkinColor = new NetColor(1f, 1f, 1f, 1f);
			returnSkinSet.HairColor = new NetColor(0.5f, 0.5f, 0.5f, 1f);
			
			switch (idx)
			{
				case 1:
					returnSkinSet.HairHelmet = 16000001;
					break;
				
				case 2:
					returnSkinSet.HairHelmet = 16000002;
					break;
			}
			
			return returnSkinSet;
		}
	}
}