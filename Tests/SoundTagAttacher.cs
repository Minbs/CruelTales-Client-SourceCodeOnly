using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SoundTagAttacher : MonoBehaviour
{
#if UNITY_EDITOR
	[Button]
    public void AttachSoundTag()
    {
	    Queue<Transform> reteriveQueue = new Queue<Transform>();
	    reteriveQueue.Enqueue(transform);
	    
	    while (reteriveQueue.Count > 0)
	    {
		    var curTransform = reteriveQueue.Dequeue();

		    for (int i = 0; i < curTransform.childCount; i++)
		    {
				reteriveQueue.Enqueue(curTransform.GetChild(i));
		    }
		    
		    string[] tokens = curTransform.name.Split("_");
		    if (tokens.Length < 2 || tokens[0].ToLower() != Global.ObjectPrefix.SoundCollider)
		    {
			    continue;
		    }

		    var SoundMatTag = tokens[1];
		    curTransform.gameObject.tag = SoundMatTag;
	    }      
    }
#endif
}
