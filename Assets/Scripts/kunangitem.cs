using UnityEngine;

public class KunangItem : MonoBehaviour
{
    [Header("Pengaturan Waktu")]
    [Tooltip("Sesuaikan dengan panjang animasi 'absorbed'")]
    public float destroyDelay = 1.5f; 

    [Header("Referensi")]
    public Altar altarScript; 

    private Animator anim;
    private bool hasBeenAbsorbed = false;
    private bool isPlayerNearby = false; // Penanda apakah pemain ada di dekat kunang-kunang

    void Start()
    {
        anim = GetComponent<Animator>();
        
        if (altarScript == null)
        {
            altarScript = FindObjectOfType<Altar>();
        }
    }

    void Update()
    {
        // Cek input HANYA jika pemain dekat dan item belum diambil
        if (isPlayerNearby && !hasBeenAbsorbed)
        {
            // Jika tombol E ditekan
            if (Input.GetKeyDown(KeyCode.E))
            {
                Absorb();
            }
        }
    }

    // Dipanggil saat Pemain masuk area Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Tekan E untuk mengambil Kunang-kunang"); // Cek Console untuk memastikan terdeteksi
        }
    }

    // Dipanggil saat Pemain keluar area Trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    public void Absorb()
    {
        hasBeenAbsorbed = true;

        // 1. Animasi
        if (anim != null)
        {
            anim.SetTrigger("isAbsorbed"); 
        }

        // 2. Tambah poin ke Altar
        if (altarScript != null)
        {
            altarScript.TambahKunangKunang();
        }

        // 3. Hancurkan objek setelah delay
        Destroy(gameObject, destroyDelay);
    }
}