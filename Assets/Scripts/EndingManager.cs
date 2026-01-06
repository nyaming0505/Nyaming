using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance;

    public int bugCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddBugCount()
    {
        bugCount++;
        Debug.Log("현재 버그 카운트: " + bugCount);
    }

    public void ResetBugs()
    {
        bugCount = 0;
    }
}