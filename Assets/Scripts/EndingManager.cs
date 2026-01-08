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
    }

    public void ResetBugs()
    {
        bugCount = 0;
    }
}