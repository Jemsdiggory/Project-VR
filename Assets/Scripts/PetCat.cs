using UnityEngine;

// Mendeteksi tangan VR yang mengelus kucing.
// Elus = tangan masuk area kucing DAN bergerak.
// Setiap total gerakan mencapai strokeDistance, pemain dapat 1 hati.
// Pasang di GameObject induk "Cat".
public class PetCat : MonoBehaviour
{
    [Header("Tangan (drag dari XR Origin > Camera Offset)")]
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    [Header("Area Pat Pat")]
    [Tooltip("Titik tengah punggung kucing. Kosongkan = pakai posisi objek ini.")]
    [SerializeField] private Transform petCenter;
    [SerializeField] private float petRadius = 0.25f;

    [Header("Aturan Pat Pat")]
    [Tooltip("Total gerakan tangan (meter) di area kucing untuk dapat 1 hati.")]
    [SerializeField] private float strokeDistance = 0.4f;
    
    [SerializeField] private float minHandSpeed = 0.1f;
    
    [SerializeField] private float cooldown = 0.5f;

    [Header("Feedback")]
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Transform heartSpawnPoint;
    [SerializeField] private Transform catModel;
    [SerializeField] private AudioSource purrSound;

    private Vector3 lastLeft;
    private Vector3 lastRight;
    private float progress;
    private float lastHeartTime = -99f;

    private Vector3 modelBaseScale;
    private float squish;

    private void Start()
    {
        if (leftHand != null) lastLeft = leftHand.position;
        if (rightHand != null) lastRight = rightHand.position;
        if (catModel != null) modelBaseScale = catModel.localScale;
    }

    private void Update()
    {
        Vector3 center = petCenter != null ? petCenter.position : transform.position;

        float moved = TrackHand(leftHand, ref lastLeft, center)
                    + TrackHand(rightHand, ref lastRight, center);

        if (moved > 0f)
        {
            progress += moved;
            if (purrSound != null && !purrSound.isPlaying) purrSound.Play();
        }

        if (progress >= strokeDistance && Time.time - lastHeartTime >= cooldown)
        {
            progress = 0f;
            lastHeartTime = Time.time;
            GiveHeart();
        }

        AnimateSquish();
    }

    private float TrackHand(Transform hand, ref Vector3 lastPos, Vector3 center)
    {
        if (hand == null) return 0f;

        Vector3 pos = hand.position;
        float delta = Vector3.Distance(pos, lastPos);
        lastPos = pos;

        if (Vector3.Distance(pos, center) > petRadius) return 0f;

        // Abaikan lompatan besar, misalnya saat teleport.
        if (delta > 0.2f) return 0f;

        float speed = delta / Mathf.Max(Time.deltaTime, 0.0001f);
        if (speed < minHandSpeed) return 0f;

        return delta;
    }

    private void GiveHeart()
    {
        if (ScoreManager.Instance != null) ScoreManager.Instance.AddHeart();

        if (heartPrefab != null)
        {
            Vector3 spawnPos = heartSpawnPoint != null
                ? heartSpawnPoint.position
                : transform.position + Vector3.up * 0.4f;
            Instantiate(heartPrefab, spawnPos, Quaternion.identity);
        }

        squish = 1f;
    }

    private void AnimateSquish()
    {
        if (catModel == null || squish <= 0f) return;

        squish = Mathf.Max(0f, squish - Time.deltaTime * 4f);
        float s = 0.08f * squish;
        catModel.localScale = Vector3.Scale(modelBaseScale, new Vector3(1f + s, 1f - s, 1f + s));
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 center = petCenter != null ? petCenter.position : transform.position;
        Gizmos.color = new Color(1f, 0.4f, 0.6f, 0.6f);
        Gizmos.DrawWireSphere(center, petRadius);
    }
}
