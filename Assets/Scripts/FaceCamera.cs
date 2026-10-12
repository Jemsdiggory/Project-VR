using UnityEngine;

// Membuat objek selalu menghadap ke pemain (kamera VR).
// Pasang di Canvas skor dan di prefab hati.
public class FaceCamera : MonoBehaviour
{
    private Transform cam;

    private void LateUpdate()
    {
        if (cam == null)
        {
            if (Camera.main == null) return;
            cam = Camera.main.transform;
        }

        Vector3 dir = transform.position - cam.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;

        transform.rotation = Quaternion.LookRotation(dir);
    }
}
