using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName; //총의 이름
    public float fange; // 사정거리
    public float accuracy; // 정확도
    public float fireRate; // 연사속도
    public float reloadTime; // 재장전 속도

    public int damage; // 대미지

    public int currentBulletCount; // 현재 탄알집에 남아있는 총알의 개수
    public int carryBulletCount; // 현재 소유하고 있는 총알의 개수

    public Animator ani;

    public ParticleSystem muzzleFlash;

    public AudioClip fire_Sound;

}