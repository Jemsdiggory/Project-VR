using UnityEngine;

// Hati yang muncul, naik ke atas, lalu memudar dan hilang.
// Pasang di prefab hati (yang punya SpriteRenderer).
public class FloatingHeart : MonoBehaviour
{
    [SerializeField] private float riseSpeed = 0.4f;
    [SerializeField] private float lifetime = 1.2f;
    [SerializeField] private float drift = 0.1f;

    private SpriteRenderer sprite;
    private Vector3 startScale;
    private Vector3 driftDir;
    private float timer;

    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        startScale = transform.localScale;

        Vector3 random = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        driftDir = random.normalized * drift;

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float t = timer / lifetime;

        transform.position += (Vector3.up * riseSpeed + driftDir) * Time.deltaTime;

        // Efek "pop" di awal kemunculan
        float pop = t < 0.2f ? Mathf.Lerp(0.5f, 1.1f, t / 0.2f) : 1f;
        transform.localScale = startScale * pop;

        if (sprite != null)
        {
            Color c = sprite.color;
            c.a = 1f - t;
            sprite.color = c;
        }
    }
}
