using UnityEngine;

public class AbsorbableItem : MonoBehaviour
{
    [Header("Settings")]
    public float absorbSpeed = 10f;
    public float stopDistance = 0.5f;

    private bool isBeingAbsorbed = false;
    private Transform playerTarget;

    void Start()
    {
        // Daftarkan diri ke CoinManager agar terhitung totalnya
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.RegisterCoin();
        }
    }

    void Update()
    {
        if (isBeingAbsorbed && playerTarget != null)
        {
            // Logika terbang ke arah player (Suction effect)
            transform.position = Vector3.MoveTowards(transform.position, playerTarget.position, absorbSpeed * Time.deltaTime);

            // Jika sudah sangat dekat dengan player
            if (Vector3.Distance(transform.position, playerTarget.position) < stopDistance)
            {
                OnAbsorbedComplete();
            }
        }
    }

    // Fungsi ini dipanggil oleh PlayerController
    public void StartAbsorb(Transform target)
    {
        if (isBeingAbsorbed) return; // Supaya tidak dipanggil berkali-kali
        
        isBeingAbsorbed = true;
        playerTarget = target;
        
        // Opsional: Matikan physics/gravity agar terbangnya mulus
        if(TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }
        
        // Opsional: Matikan collider agar tidak menabrak player saat terbang
        if(TryGetComponent<Collider>(out Collider col))
        {
            col.enabled = false;
        }
    }

    void OnAbsorbedComplete()
    {
        // Lapor ke CoinManager
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.CollectCoin();
        }

        // Efek suara atau partikel bisa ditambahkan di sini
        
        // Hancurkan benda
        Destroy(gameObject);
    }
}