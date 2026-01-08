using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CupIngredient
{
    Espresso,
    Water,
    Milk,
    Choco
}

[System.Serializable]
public class CupData
{
    public List<CupIngredient> ingredients = new List<CupIngredient>();
    public int maxCount = 4;
}

public class CupManager : MonoBehaviour
{

    public static CupManager Instance;

    [Header("Cup Data")]
    public CupData cupData = new CupData();

    [Header("Cup UI")]
    public Image[] cupSlots;   // 슬롯 4개

    [Header("Ingredient Sprites")]
    public Sprite milkSprite;
    public Sprite waterSprite;
    public Sprite chocoSprite;
    public Sprite espressoSprite;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        ClearCup();
    }

    public bool AddIngredient(CupIngredient type)
    {
        if (cupData.ingredients.Count >= cupData.maxCount)
        {
            return false;
        }

        cupData.ingredients.Add(type);

        UpdateCupUI();

        return true;
    }

    public void ClearCup()
    {
        cupData.ingredients.Clear();
        UpdateCupUI();
    }

    public int GetIngredientCount()
    {
        return cupData.ingredients.Count;
    }

    public List<CupIngredient> GetIngredients()
    {
        return cupData.ingredients;
    }
    Sprite GetSprite(CupIngredient type)
    {
        switch (type)
        {
            case CupIngredient.Milk:
                return milkSprite;
            case CupIngredient.Water:
                return waterSprite;
            case CupIngredient.Choco:
                return chocoSprite;
            case CupIngredient.Espresso:
                return espressoSprite;
            default:
                return null;
        }
    }
    void UpdateCupUI()
    {
        //  전부 초기화
        for (int i = 0; i < cupSlots.Length; i++)
        {
            cupSlots[i].sprite = null;
            cupSlots[i].enabled = false;
        }

        //  현재 재료만큼 채우기
        for (int i = 0; i < cupData.ingredients.Count; i++)
        {
            cupSlots[i].sprite = GetSprite(cupData.ingredients[i]);
            cupSlots[i].enabled = true;
        }
    }


}
