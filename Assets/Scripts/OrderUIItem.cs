using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class OrderUIPrefabEntry
{
    public RecipeType recipeType;
    public GameObject orderUIPrefab;
}


public class OrderUIItem : MonoBehaviour
{
    public Image timerFill;

    Customer owner;
    Recipe recipe;
    float timeLimit;
    float currentTime;

    public void Init(Customer customer, Recipe recipe)
    {
        owner = customer;
        this.recipe = recipe;
        timerFill.fillAmount = 1f;
    }



    public void UpdateTimer(float ratio)
    {
        timerFill.fillAmount = Mathf.Clamp01(ratio);
    }
}
