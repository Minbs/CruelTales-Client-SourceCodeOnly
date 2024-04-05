using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTC.SystemCore
{
	public interface IManager
	{
		public void Initialize();
		public void Release();
	}

	public interface IInitializable
	{
		public bool IsInitialized();
	}

	public interface IFinalizable
	{
		public bool IsFinalized();
	}
}
