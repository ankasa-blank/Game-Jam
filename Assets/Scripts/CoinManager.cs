using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [Header("Coin Settings")]
    public int targetCoin = 10;
    public int currentCoin = 0;

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
    }

    public bool IsAllCoinCollected()
    {
        return currentCoin >= targetCoin;
    }
}
