using System;
using CT.Common.Tools.Data;
using UnityEngine;

namespace Convension
{
	public class CodeConAttribute : Attribute
	{

	}

	public enum SomeType
	{
		None = 0,
		Apple,
		Banana,
	}

	public interface ICodeInterface
	{
		public string Name { get; }
		public void OnClick();
	}

	public readonly struct Point
	{
		public readonly int X;
		public readonly int Y;

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	public class CodeConvention : ICodeInterface
	{
		[field: SerializeField, CodeCon]
		public int ObjectId { get; set; }
		public string Name => nameof(CodeConvention);
		public string HtmlCode;
		public bool IsRunning => ObjectId != 0;
		public bool HasHtmlCode => HtmlCode != null;

		public event Action OnInitialized;
		public event Action OnUiAction;

		public const int WIDTH = 20;
		public readonly int Height = 40;

		//private float _deltaTime = 10.0f;
		private int[,] _intArray2D;

		public CodeConvention()
		{
			_intArray2D = new int[WIDTH, Height];

			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < WIDTH; x++)
				{
					_intArray2D[y, x] = x * y;
				}
			}
		}

		public void Update()
		{
			Console.WriteLine($"Curent Time : {DateTime.UtcNow}");
		}

		public void OnClick() => OnUiAction?.Invoke();

		public void OnStarted()
		{
			OnInitialized?.Invoke();
		}

		public string GetSomething()
		{
			if (TryGetSomething(out var result))
			{
			}
			else
			{

			}

			CodeConvention c = new CodeConvention();
			Console.WriteLine();

			SomeType s = SomeType.None;
			switch (s)
			{
				case SomeType.Apple:
					break;

				case SomeType.Banana:
					break;

				default:
					break;
			}

			return null;
		}

		public bool TryGetSomething(out string result)
		{
			if (ObjectId != 0)
			{
				result = "abc";
				return true;
			}
			result = string.Empty; // "";
			return false;
		}

		public void GetGenericThing<T>() where T : class
		{
			something();
			somehitng2();

			void something()
			{

			}

			static void somehitng2()
			{

			}
		}

		public void GetGenericThing2<Some, Value>()
		{
			//if (false)
			//	return;
		}

		public void GetGenericThing3<T1, T2, T3>()
		{
		}

		private void Category_functionName(int width, int height,
										   int a,
										   string b)
		{
		}


		public void Category_Function(int width, int height,
									  float other, int some,
									  string context, long tick)
		{
		}

		public void Server_Some() {}
		public void Client_Some() {}

		private void handlePacket(Action callback)
		{
			callback?.Invoke();
		}

		public override string ToString()
		{
			return $"ID({ObjectId}) IsRunning({IsRunning})";
		}
	}

	public static class CodeConventionExtension
	{
		public static bool HasObjectId(this CodeConvention codeConvention)
		{
			return codeConvention.ObjectId > 0;
		}
	}
}
