using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(AudioClip))]
public class Music : MonoBehaviour
{
	[SerializeField] AudioClip audioClip;
	void Awake()
	{
		// if (audioClip != null)
		// {
		// 	AudioManager.Instance.SetMusic(audioClip);
		// }
	}
	// Start is called before the first frame update
	void Start()
	{
		if (audioClip != null)
		{
			AudioManager.Instance.SetMusic(audioClip);
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
