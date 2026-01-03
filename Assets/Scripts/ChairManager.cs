using UnityEngine;

public class ChairManager : MonoBehaviour
{
    public static ChairManager Instance;
    public Chair[] chairs;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // ?? 현재 점유된 의자 수
    public int GetOccupiedCount()
    {
        int count = 0;
        foreach (var chair in chairs)
        {
            if (chair.IsOccupied)
                count++;
        }
        return count;
    }

    // ?? 빈 의자가 있는지만 확인
    public bool HasEmptyChair()
    {
        foreach (var chair in chairs)
        {
            if (!chair.IsOccupied)
                return true;
        }
        return false;
    }

    // ?? 실제 의자 배정
    public Chair GetEmptyChair()
    {
        foreach (var chair in chairs)
        {
            if (chair.TryOccupy())
            {
                return chair;
            }
        }
        return null;
    }


}
