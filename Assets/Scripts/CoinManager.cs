using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int totalCoins = 0;
    public int collectedCoins = 0;

    void Awake()
    {
        // Setup Singleton sederhana
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Fungsi dipanggil oleh Item saat game mulai
    public void RegisterCoin()
    {
        totalCoins++;
    }

    // Fungsi dipanggil saat Item berhasil di-absorb
    public void CollectCoin()
    {
        collectedCoins++;
        Debug.Log($"Coin Collected: {collectedCoins}/{totalCoins}");
    }

    // Fungsi yang dipanggil oleh AltarWin kamu
    public bool IsAllCoinCollected()
    {
        return collectedCoins >= totalCoins;
    }
}