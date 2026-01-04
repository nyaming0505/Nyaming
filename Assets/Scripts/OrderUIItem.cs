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
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        timerFill.fillAmount = 1 - (currentTime / timeLimit);
    }
}
