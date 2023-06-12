using UnityEngine;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance;

	public AudioSource audioSource;
    public AudioClip win, lose, button, explode, ticking, bomb, doubleKill, tripleKill, multiKill;
    //public AudioClip[] hit;
    //public AudioClip[] push;

    void Start()
    {
        instance = this;
        try
        {
            audioSource = GetComponent<AudioSource>();
        }
        catch { }
    }

    public void PlaySound(AudioClip clip){
        audioSource.PlayOneShot(clip);
	}
}
