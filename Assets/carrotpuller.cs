using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carrotpuller : MonoBehaviour
  {
     [Header("Pull Settings")]
    public float maxPullHeight = 2f;
    public float pullSpeed = 5f;

    [Header("References")]
    public Transform Carrot;                  // The part that moves upward
    public SpriteRenderer frontDirt;          // Stays visible during pull
    public carrotgrower grower;               // Notifies when pulled

    private Vector3 startPos;
    private Vector3 originalScale;
    private Quaternion originalRotation;
    private bool isDragging = false;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        startPos = Carrot.position;
        originalScale = Carrot.localScale;
        originalRotation = Carrot.localRotation;
    }

    public void BeginDrag()
    {
        isDragging = true;
    }

    public void EndDrag()
    {
        isDragging = false;

        if (Carrot.position.y >= startPos.y + maxPullHeight * 0.9f)
        {
            OnCarrotPulled();
        }
        else
        {
            ResetCarrot();
        }
    }

    void Update()
    {
        if (!isDragging) return;

        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        float targetY = Mathf.Clamp(mouseWorld.y, startPos.y, startPos.y + maxPullHeight);
        Vector3 newPos = new Vector3(startPos.x, targetY, startPos.z);

        Carrot.position = Vector3.Lerp(Carrot.position, newPos, Time.deltaTime * pullSpeed);

        float progress = (Carrot.position.y - startPos.y) / maxPullHeight;

        // 🎬 Squash & Stretch
        float stretchY = Mathf.Lerp(originalScale.y, originalScale.y * 1.4f, progress);
        float squashX = Mathf.Lerp(originalScale.x, originalScale.x * 0.8f, progress);
        Carrot.localScale = new Vector3(squashX, stretchY, originalScale.z);

        // 🌀 Wiggle
        float wiggle = Mathf.Sin(Time.time * 30f) * 5f * progress;
        Carrot.localRotation = Quaternion.Euler(0, 0, wiggle);
    }

    void OnCarrotPulled()
    {
        Debug.Log("Carrot pulled!");

        // Reset visuals before flying
        Carrot.localScale = originalScale;
        Carrot.localRotation = originalRotation;

        StartCoroutine(FlyTowardCursorAndDestroy());
    }

    IEnumerator FlyTowardCursorAndDestroy()
    {
        float duration = 0.5f;
        float elapsed = 0f;

        Vector3 startFlyPos = Carrot.position;

        // Get cursor direction
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = startFlyPos.z;
        Vector3 direction = (mouseWorld - startFlyPos).normalized;

        // Rotate carrot to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Carrot.localRotation = Quaternion.Euler(25, 25, angle);

        Vector3 targetFlyPos = startFlyPos + direction * 5f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            // Position: fly toward cursor
            Carrot.position = Vector3.Lerp(startFlyPos, targetFlyPos, t);

            // Rotation: add spin
            float spin = Mathf.Lerp(0f, 720f, t);
            Carrot.localRotation = Quaternion.Euler(0, 0, angle + spin);

            // Scale: shrink
            float scaleFactor = Mathf.Lerp(1f, 0f, t);
            Carrot.localScale = originalScale * scaleFactor;

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (grower != null)
            grower.NotifyCarrotPulled();

        Destroy(gameObject);
    }

    void ResetCarrot()
    {
        Carrot.position = startPos;
        Carrot.localScale = originalScale;
        Carrot.localRotation = originalRotation;
    }
}
