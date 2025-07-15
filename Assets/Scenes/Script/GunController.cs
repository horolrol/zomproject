using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;
    private AudioSource audioSource;

    [Header("Sound Options")]
    [Range(0f, 1f)]
    [SerializeField]
    private float fireVolume = 1f;

    [SerializeField]
    private float firePitch = 1f;

    [SerializeField]
    private float startTime = 0f; // 몇 초부터 재생할지 (재생 위치)

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GunFireRateCalc();
        TryFire();
    }

    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    private void TryFire()
    {
        if (Input.GetMouseButton(0) && currentFireRate <= 0)
        {
            Fire();
        }
    }

    private void Fire()
    {
        currentFireRate = currentGun.fireRate;
        Shoot();
    }

    private void Shoot()
    {
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Debug.Log("총알 발사");
    }

    private void PlaySE(AudioClip _clip)
    {
        if (_clip == null) return;

        audioSource.clip = _clip;
        audioSource.volume = fireVolume;
        audioSource.pitch = firePitch;

        // 재생 위치 지정
        audioSource.time = Mathf.Clamp(startTime, 0f, _clip.length - 0.01f);
        audioSource.Play();
    }
}
