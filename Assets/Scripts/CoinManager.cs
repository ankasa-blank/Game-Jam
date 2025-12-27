using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
     public static CoinManager Instance;

    [Header("Coin Settings")]
    public int targetCoin = 10;
    public int currentCoin = 0;

    [Header("Win Settings")]
    public string winSceneName = "WinScene";

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddCoin(int amount)
    {
        currentCoin += amount;
        Debug.Log("Coin: " + currentCoin + " / " + targetCoin);

        if (currentCoin >= targetCoin)
            WinGame();
    }

    void WinGame()
    {
        Debug.Log("YOU WIN!");
        SceneManager.LoadScene(winSceneName);
    }
}
