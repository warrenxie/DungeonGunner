using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public float waitToLoad = 4f;
    public string nextLevel;

    public bool isPaused;
    public int currentCoin;

    public Transform startPoint;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerController.instance.transform.position = startPoint.position;
        PlayerController.instance.canMove = true;

        currentCoin = CharacterTracker.instance.currentCoins;

        Time.timeScale = 1f;

        UIController.instance.coinText.text = currentCoin.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public IEnumerator LevelEnd()
    {
        AudioManager.instance.PlayWin();
        PlayerController.instance.canMove = false;
        UIController.instance.StartFadeToBlack();
        yield return new WaitForSeconds(waitToLoad);

        CharacterTracker.instance.currentCoins = currentCoin;
        CharacterTracker.instance.currentHealth = PlayerHealthController.instance.currentHealth;
        CharacterTracker.instance.maxHealth = PlayerHealthController.instance.maxHealth;

        SceneManager.LoadScene(nextLevel);
    }
    public void PauseUnpause()
    {
        if (!isPaused)
        {
            UIController.instance.pauseMenu.SetActive(true);
            isPaused = true;

            Time.timeScale = 0f;
        }
        else
        {
            UIController.instance.pauseMenu.SetActive(false);
            isPaused = false;

            Time.timeScale = 1f;
        }
    }
    public void GetCoins(int amount)
    {
        currentCoin += amount;
        UIController.instance.coinText.text = currentCoin.ToString();
    }
    public void SpendCoins(int amount)
    {
        currentCoin -= amount;

        if(currentCoin <0)
        {
            currentCoin = 0;
        }
        UIController.instance.coinText.text = currentCoin.ToString();
    }
}
