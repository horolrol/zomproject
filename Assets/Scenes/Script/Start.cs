using UnityEngine;

public class Start : MonoBehaviour
{
    public GameObject Background;  // ²ô°í ½ÍÀº Äµ¹ö½º

    public void Hide()
    {
        Background.SetActive(false);
    }
}
