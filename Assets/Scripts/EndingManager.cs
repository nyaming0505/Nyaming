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

    // 버그를 찾았을 때 호출
    public void AddBugCount()
    {
        bugCount++;
        Debug.Log("현재 버그 카운트: " + bugCount);

        if (bugCount >= 3)
        {
            Debug.Log("진엔딩 조건 달성!");
        }
    }

    public void ResetBugs()
    {
        bugCount = 0;
    }
}