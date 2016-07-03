using UnityEngine;
using System.Collections;

public class AudioHandler : MonoBehaviour 
{
    public AudioClip[] backgroundAudio;

	void Start () 
    {
        AudioSource source = GetComponent<AudioSource>();

        source.clip = backgroundAudio[Random.Range(0, backgroundAudio.Length)];
        source.Play();
	}
}
