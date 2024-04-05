using System.Threading;
using CTC.SystemCore;
using Cysharp.Threading.Tasks;
using Spine.Unity;
using UnityEngine;

public class DokzaSoundHandler : MonoBehaviour
{
	public SkeletonAnimation DokzaSkeletonAnimation;

	private CancellationTokenSource _dokzaSFXCTS;
	
	private Vector3 _adjustedPos;
	private RaycastHit _soundHit;
	private int _layerMask;

	private string _curSoundMatTag = "";

	// Initializer
	private void Start()
	{
		DokzaSkeletonAnimation.state.Event += receiveSpineEvent;
		_layerMask = Global.Physics.GetMaskByIndex(24);
	}
	
	private async void OnEnable()
	{
		_dokzaSFXCTS = new CancellationTokenSource();
		await detectSoundCol(_dokzaSFXCTS.Token);
	}
	
	private void OnDisable()
	{
		DokzaSkeletonAnimation.state.Event -= receiveSpineEvent;
		_dokzaSFXCTS?.Cancel();
	}
	
	// Utils
	private Vector3 returnAdjustedPos()
	{
		_adjustedPos = transform.position;
		_adjustedPos.y -= 0.5f;
		return _adjustedPos;
	}
	
	// Functions
	private async UniTask detectSoundCol(CancellationToken cancellationToken)
	{
		while (true)
		{
			if (cancellationToken.IsCancellationRequested) 
				return;

			if (Physics.Raycast(returnAdjustedPos(), transform.TransformDirection(Vector3.up), out _soundHit, 1f,
				    _layerMask))
			{
				_curSoundMatTag = _soundHit.collider.tag;
			}
			
			await UniTask.WaitForFixedUpdate();
		}
	}

	private void receiveSpineEvent(Spine.TrackEntry trackEntry, Spine.Event e)
	{
		GlobalService.SoundManager.PlayPCSound(e.Data.Name, _curSoundMatTag, transform.position);
	}
}