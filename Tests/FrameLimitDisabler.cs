using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CTC
{
    public class FrameLimitDisabler : MonoBehaviour
    {
	    void Start()
        {
	        Invoke(nameof(disableFrameLimit), 0.1f);
        }

	    private void disableFrameLimit()
	    {
		    Application.targetFrameRate = 300;
	    }
    }
}
