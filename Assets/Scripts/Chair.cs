using UnityEngine;

public enum SitDirection
{
    Left,
    Right
}

public class Chair : MonoBehaviour
{

    [Header("Chair Settings")]
    public SitDirection sitDirection;

    public Transform sitPoint;

    public bool IsOccupied { get; private set; }
    private void Awake()
    {
        IsOccupied = false;
    }

    public bool TryOccupy()
    {
        if (IsOccupied)
            return false;

        IsOccupied = true;
        return true;
    }

    public void Release()
    {
        IsOccupied = false;
    }

}
