using UnityEngine;
using UnityEngine.Playables; // Wajib ada untuk Timeline

public class Altar : MonoBehaviour
{
    [Header("Pengaturan Misi")]
    public int targetKunang = 6;
    [SerializeField] private int jumlahTerkumpul = 0;

    [Header("Cutscene Settings")]
    public PlayableDirector timelineDirector; // Masukkan GameObject CutsceneDirector disini
    public GameObject playerObject; // Masukkan Player untuk dimatikan geraknya
    public GameObject uiGameplay; // Masukkan UI koin/darah untuk disembunyikan (opsional)

    public void TambahKunangKunang()
    {
        jumlahTerkumpul++;
        
        if (jumlahTerkumpul >= targetKunang)
        {
            MulaiEnding();
        }
    }

    void MulaiEnding()
    {
        Debug.Log("Scene Ending Dimulai...");

        // 1. Matikan kontrol pemain agar tidak bisa jalan-jalan saat cutscene
        if (playerObject != null)
        {
            // Matikan script controller-nya saja, jangan objeknya (supaya masih terlihat)
            playerObject.GetComponent<PlayerController>().enabled = false;
            // Set animasi ke idle atau duduk jika mau
            playerObject.GetComponent<Animator>().SetBool("isRunning", false);
        }

        // 2. Sembunyikan UI Gameplay yang mengganggu
        if (uiGameplay != null)
        {
            uiGameplay.SetActive(false);
        }

        // 3. Mainkan Timeline
        if (timelineDirector != null)
        {
            timelineDirector.Play();
        }
    }
}