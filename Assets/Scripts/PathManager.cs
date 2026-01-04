using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static PathManager Instance;

    [Header("Path Points")]
    public Transform exitMidPoint;
    public Transform enterPoint;
    public Transform exitPoint;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public Vector2 GetEnterPoint()
    {
        return enterPoint.position;
    }
    public Vector2 GetExitMidPoint()
    {
        return exitMidPoint.position;
    }
    public Vector2 GetExitPoint()
    {
        return exitPoint.position;
    }
}
