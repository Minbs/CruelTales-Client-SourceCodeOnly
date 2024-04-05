using System;
using System.Collections;
using System.Collections.Generic;
using CTC.GUI;
using CTC.SystemCore;
using CTC.Tests;
using CTC.Utils.Coroutines;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CTC
{
    public class View_ScreenEffect : ViewBaseWithContext
    {
	    private const string LAYOUT_GROUP = "Layout Group";

	    [SerializeField, TitleGroup(LAYOUT_GROUP)]
	    private GameObject Layout_ScreenEffect;


	    private const string IMAGE_GROUP = "Image Group";

	    [SerializeField, TitleGroup(IMAGE_GROUP)]
	    private Image ScreenEffect_Img;
		
		
	    // ViewBaseWithContext
	    public Context_ScreenEffect ScreenEffectContext;

	    // private Variables
	    private CoroutineRunner _screenEffectRunner;
	    private bool _isRunning = false;
	    private Action _screenEffectEndAction = null;
	    
	    protected override void Awake()
	    {
		    base.Awake();
		    this.ScreenEffectContext = this.CurrentContext as Context_ScreenEffect;
		    
		    _screenEffectRunner = new(this);
		    _screenEffectRunner.BindOnStartCallback(onRunnerStart);
		    _screenEffectRunner.BindOnEndCallback(onRunnerEnd);
	    }

	    protected void OnDisable()
	    {
		    _screenEffectRunner.Stop();
	    }

	    private void onRunnerStart()
	    {
		    _isRunning = true;
		    ScreenEffect_Img.gameObject.SetActive(true);
	    }

	    private void onRunnerEnd()
	    {
		    _screenEffectEndAction?.Invoke();
		    _isRunning = false;
		    ScreenEffect_Img.gameObject.SetActive(false);
	    }

	    public void StartEffect(Color targetColor, float speed, Action action = null)
	    {
		    _screenEffectEndAction = action;
		    _screenEffectRunner.Start(screenEffectEnumerator(targetColor, speed));
	    }
	    
	    private IEnumerator screenEffectEnumerator(Color targetColor, float _speed)
	    {
		    float _lerpVal = 0f;
		    
		    Color _startColor = targetColor;
		    Color _endColor = targetColor;
		    _endColor.a = 0f;

		    while (true)
		    {
			    if (_lerpVal >= 1f)
			    {
				    ScreenEffect_Img.color = Color.Lerp(_startColor, _endColor, _lerpVal);
				    break;
			    }
			    
			    ScreenEffect_Img.color = Color.Lerp(_startColor, _endColor, _lerpVal);
			    _lerpVal += Time.deltaTime * _speed;
			    
			    yield return null;
		    }
		    
		    yield break;
	    }
	    
	    /*
	    // Test Codes
	    private void Update()
	    {
		    if (Input.GetKeyDown(KeyCode.Backspace))
		    {
			    StartEffect(Color.cyan, 3f);
		    }
	    }
	    */
    }
}
