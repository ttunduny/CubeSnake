using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;

    public AudioSource sfx;
    public AudioSource backgroundMusic;
    public AudioClip eating;
    public AudioClip backgroundSound;

    [SerializeField]
    private bool hasGameStarted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManagerInstance = this;
    }


    public void PlayEatingSound()
    {
        sfx.PlayOneShot(eating);
    }

    public void PlayBackgroundMusic()
    {
        backgroundMusic.Play();
    }

    public bool isGameRunning()
    {
        return hasGameStarted;
    }

    public void startGame()
    {
        hasGameStarted = true;
    }
    
}
