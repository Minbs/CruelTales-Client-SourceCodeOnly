using System.Collections;
using System.Collections.Generic;
using CTC.DataBind.Contexts;
using Slash.Unity.DataBind.Core.Data;
using UnityEngine;

namespace CTC.Tests
{
    public class Context_ScreenEffect : ContextWithView<View_ScreenEffect>
    {
	    private readonly Property<string> titleProperty = new Property<string>();
	    public string Title
	    {
		    get => titleProperty.Value;
		    set => titleProperty.Value = value;
	    }
		
	    private readonly Property<string> contentProperty = new Property<string>();
	    public string Content
	    {
		    get => contentProperty.Value;
		    set => contentProperty.Value = value;
	    }
    }
}
