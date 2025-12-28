using UnityEngine;
using UnityEngine.SceneManagement;

public class AltarWin : MonoBehaviour
{
    [Header("Win Condition")]
    public bool requireAllCoins = true;

    [Header("Win Behaviour")]
    public bool useSceneTransition = true;
    public string winSceneName = "WinScene";

    public bool useAnimation = false;
    public Animator altarAnimator;
    public string winTriggerName = "Win";

    private bool hasWon = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasWon) return;

        if (!other.CompareTag("Player")) return;

        if (requireAllCoins && !CoinManager.Instance.IsAllCoinCollected())
        {
            Debug.Log("❌ Koin belum lengkap!");
            return;
        }

        Win();
    }

    void Win()
    {
        hasWon = true;
        Debug.Log("🏆 PLAYER MENANG!");

        if (useAnimation && altarAnimator != null)
        {
            altarAnimator.SetTrigger(winTriggerName);
        }

        if (useSceneTransition)
        {
            Invoke(nameof(LoadWinScene), 1.5f);
        }
    }

    void LoadWinScene()
    {
        SceneManager.LoadScene(winSceneName);
    }
}
