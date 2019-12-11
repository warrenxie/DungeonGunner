using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource levelMusic, gameOverMusic;
    public AudioSource victoryMusic;
    public AudioSource[] sfx;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayerGameOver()
    {
        levelMusic.Stop();
        gameOverMusic.Play();
    }
    public void PlayWin()
    {
        levelMusic.Stop();
        victoryMusic.Play();
    }

    public void PlayerSFX(int sfxNumber)
    {
        sfx[sfxNumber].Stop();
        sfx[sfxNumber].Play();
    }
}
