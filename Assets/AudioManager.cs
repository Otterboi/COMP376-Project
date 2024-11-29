using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour

{
	private static AudioManager instance = null;
	public static AudioManager Instance
	{
		get { return instance; }
	}
	[SerializeField] AudioSource music;
	[SerializeField] AudioSource background_sounds;
	// Start is called before the first frame update
	void Start()
	{
		AudioSource[] t = GetComponents<AudioSource>();
		if (t.Length < 2)
		{
			Debug.LogWarning("Missing Audio Source");
		}
		else
		{
			music = t[0];
			background_sounds = t[1];
		}
	}
	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
	// Update is called once per frame
	void Update()
	{

	}
	public void PlayMusic()
	{
		if (music.isPlaying) return;
		music.Play();
	}

	public void PlayBackgroundSounds()
	{
		if (background_sounds.isPlaying) return;
		background_sounds.Play();
	}
	public void StopMusic()
	{
		if (!music.isPlaying) return;
		music.Stop();
	}

	public void StopBackgroundSounds()
	{
		if (!background_sounds.isPlaying) return;
		background_sounds.Stop();
	}
	public void SetMusic(AudioClip audioClip)
	{
		Debug.Log("Updating Music File");
		if (music.clip != audioClip) 
		{
			// music.Stop();
			StopMusic();
			music.clip = audioClip;
			PlayMusic();
			// music.Play();
		}
	}
}
