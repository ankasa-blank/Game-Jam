using UnityEngine;
using UnityEngine.UI; // Diperlukan untuk Slider dan UI standar

public class CampfireEndGame : MonoBehaviour
{
    [Header("--- Pengaturan Utama ---")]
    public ParticleSystem apiParticle;
    public float durasiTahan = 3.0f;

    [Header("--- Input ---")]
    public KeyCode tombolNyalakan = KeyCode.E;
    public KeyCode tombolDuduk = KeyCode.C;

    [Header("--- UI References (DRAG & DROP DI SINI) ---")]
    [Tooltip("Panel UI besar bertuliskan 'Thank You For Playing' (Screen Space)")]
    public GameObject thankYouPanel; 

    [Tooltip("UI Instruksi 'Tekan E' yang melayang di atas api (World Space)")]
    public GameObject instructionUI; 

    [Tooltip("Slider Loading (Screen Space atau World Space)")]
    public Slider progressBarSlider;
    
    [Tooltip("Canvas pembungkus Slider (agar bisa disembunyikan)")]
    public GameObject sliderCanvas;


    // Internal Variables
    private bool isPlayerNearby = false;
    private bool isGameFinished = false; // Pengganti isAltarLit
    private bool isSitting = false;
    private float currentHoldTimer = 0f;

    void Start()
    {
        // 1. Matikan api & Panel Tamat di awal
        if (apiParticle != null) apiParticle.Stop();
        if (thankYouPanel != null) thankYouPanel.SetActive(false);

        // 2. Sembunyikan Slider
        if (sliderCanvas != null) sliderCanvas.SetActive(false);
        if (progressBarSlider != null) progressBarSlider.value = 0;

        // 3. Pastikan Instruksi "Tekan E" Muncul di awal
        if (instructionUI != null) instructionUI.SetActive(true);
    }

    void Update()
    {
        // Jika game sudah tamat, hentikan input E (tapi mungkin masih boleh duduk?)
        // Di sini saya hentikan semua input E agar tidak memicu ulang.
        if (isGameFinished) return;

        if (isPlayerNearby)
        {
            HandleSitting(); // Masih bisa duduk/berdiri
            HandleLighting(); // Cek input E
        }
        else
        {
            ResetTimer();
        }
    }

    void HandleLighting()
    {
        if (Input.GetKey(tombolNyalakan))
        {
            currentHoldTimer += Time.deltaTime;

            // Tampilkan & Update Slider
            if (sliderCanvas != null) sliderCanvas.SetActive(true);
            if (progressBarSlider != null) progressBarSlider.value = currentHoldTimer / durasiTahan;

            // Cek jika waktu tahan terpenuhi
            if (currentHoldTimer >= durasiTahan)
            {
                TriggerEndGame();
            }
        }
        else if (Input.GetKeyUp(tombolNyalakan))
        {
            ResetTimer();
        }
    }

    void HandleSitting()
    {
        if (Input.GetKeyDown(tombolDuduk))
        {
            isSitting = !isSitting;
            // Masukkan kode animasi duduk di sini jika ada
            Debug.Log(isSitting ? "Player Duduk" : "Player Berdiri");
        }
    }

    void TriggerEndGame()
    {
        isGameFinished = true;
        currentHoldTimer = durasiTahan;

        // 1. Nyalakan Api
        if (apiParticle != null) apiParticle.Play();

        // 2. Sembunyikan UI Instruksi & Slider (Bersihkan layar)
        if (instructionUI != null) instructionUI.SetActive(false);
        if (sliderCanvas != null) sliderCanvas.SetActive(false);

        // 3. MUNCULKAN PANEL "THANK YOU"
        if (thankYouPanel != null) 
        {
            thankYouPanel.SetActive(true);
            Debug.Log("Game Selesai: Panel muncul.");
        }

        // Opsional: Matikan pergerakan player di sini jika mau
    }

    void ResetTimer()
    {
        currentHoldTimer = 0f;
        if (progressBarSlider != null) progressBarSlider.value = 0;
        if (sliderCanvas != null) sliderCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            ResetTimer();
        }
    }
}