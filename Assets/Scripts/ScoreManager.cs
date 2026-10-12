using TMPro;
using UnityEngine;

// Menyimpan jumlah hati dan meng-update tampilan UI.
// Pasang di GameObject kosong bernama "ScoreManager".
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TMP_Text heartText;
    [SerializeField] private Transform heartIcon;

    [Header("Efek mantul ikon")]
    [SerializeField] private float bounceAmount = 0.3f;
    [SerializeField] private float bounceDecay = 4f;

    public int Hearts { get; private set; }

    private Vector3 iconBaseScale;
    private float bounce;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (heartIcon != null) iconBaseScale = heartIcon.localScale;
        UpdateText();
    }

    public void AddHeart(int amount = 1)
    {
        Hearts += amount;
        UpdateText();
        bounce = 1f;
    }

    private void Update()
    {
        if (heartIcon == null || bounce <= 0f) return;

        bounce = Mathf.Max(0f, bounce - Time.deltaTime * bounceDecay);
        heartIcon.localScale = iconBaseScale * (1f + bounceAmount * bounce);
    }

    private void UpdateText()
    {
        if (heartText != null) heartText.text = "x " + Hearts;
    }
}
