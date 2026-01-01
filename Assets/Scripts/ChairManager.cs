using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairManager : MonoBehaviour
{
    public static ChairManager Instance;
    public Chair[] chairs;

    void Awake() => Instance = this;

    public Chair GetEmptyChair()
    {
        foreach (var chair in chairs)
        {
            if (!chair.isOccupied)
            {
                chair.isOccupied = true;
                return chair;
            }
        }
        return null;
    }
}
