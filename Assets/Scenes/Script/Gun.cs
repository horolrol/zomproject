using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName; //���� �̸�
    public float fange; // �����Ÿ�
    public float accuracy; // ��Ȯ��
    public float fireRate; // ����ӵ�
    public float reloadTime; // ������ �ӵ�

    public int damage; // �����

    public int currentBulletCount; // ���� ź������ �����ִ� �Ѿ��� ����
    public int carryBulletCount; // ���� �����ϰ� �ִ� �Ѿ��� ����

    public Animator ani;

    public ParticleSystem muzzleFlash;

    public AudioClip fire_Sound;

}