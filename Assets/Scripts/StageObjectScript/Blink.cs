using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
	private float _nextTime;
	[SerializeField]private float _interval = 0.1f;

	bool _isBlink = false;

	private Renderer _meshRenderer;

	// Use this for initialization
	void Start()
	{
		_nextTime = Time.time;

		_meshRenderer = GetComponentInChildren<Renderer>();
	}

	// Update is called once per frame
	void Update()
	{
		if (_isBlink == false){ 
			return;
		}

		if (Time.time > _nextTime)
		{
			_meshRenderer.enabled = !_meshRenderer.enabled;

			_nextTime += _interval;
		}
	}

	public void StartBlink(){
		_isBlink = true;
	}

	public void EndBlink() { 
		_isBlink=false;
		_meshRenderer.enabled = true;
	}
}
