using UnityEngine;

public class Start : MonoBehaviour
{
    public GameObject Background;  // ���� ���� ĵ����

    public void Hide()
    {
        Background.SetActive(false);
    }
}
