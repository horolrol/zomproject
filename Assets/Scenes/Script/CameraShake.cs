using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.1f;  // 흔들림 시간
    public float shakeAmount = 0.1f;    // 흔들림 세기
    public float decreaseFactor = 1.0f; // 감소 속도

    private Vector3 originalPos;
    private float currentShakeDuration = 0f;

    void OnEnable()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 좌클릭
        {
            StartShake();
        }

        if (currentShakeDuration > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            currentShakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            currentShakeDuration = 0f;
            transform.localPosition = originalPos;
        }
    }

    public void StartShake()
    {
        currentShakeDuration = shakeDuration;
    }
}
