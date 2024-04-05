using Slash.Unity.DataBind.Core.Data;
using Slash.Unity.DataBind.Core.Presentation;
using UnityEngine;

public class Test_LogicForDataBinding : MonoBehaviour
{
	private Context mContext;

	private void Awake()
	{
		mContext = GetComponent<ContextHolder>().Context as Context;
 	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			mContext.SetValue("Test_Message", Random.Range(0, 10000).ToString());
			var s = (string)mContext.GetValue("Test_Message");
		}
	}
}
