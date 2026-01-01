using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe
{
    public string recipeName;
    public List<CupIngredient> ingredients;
    public int score;
}

public class RecipeDatabase : MonoBehaviour
{
    public static RecipeDatabase Instance;

    public List<Recipe> recipes = new List<Recipe>();


    void Awake()
    {
        Instance = this;
        InitRecipes();
    }
    void InitRecipes()
    {
        recipes.Add(new Recipe
        {
            recipeName = "에스프레소",
            ingredients = new List<CupIngredient> { CupIngredient.Espresso },
            score = 100
        });

        recipes.Add(new Recipe
        {
            recipeName = "아메리카노",
            ingredients = new List<CupIngredient>
            {
                CupIngredient.Espresso,
                CupIngredient.Water
            },
            score = 150
        });

        recipes.Add(new Recipe
        {
            recipeName = "카페라떼",
            ingredients = new List<CupIngredient>
            {
                CupIngredient.Espresso,
                CupIngredient.Milk
            },
            score = 180
        });

        recipes.Add(new Recipe
        {
            recipeName = "초코라떼",
            ingredients = new List<CupIngredient>
            {
                CupIngredient.Milk,
                CupIngredient.Choco
            },
            score = 160
        });

        recipes.Add(new Recipe
        {
            recipeName = "카페모카",
            ingredients = new List<CupIngredient>
            {
                CupIngredient.Espresso,
                CupIngredient.Milk,
                CupIngredient.Choco
            },
            score = 220
        });

        recipes.Add(new Recipe
        {
            recipeName = "더블샷",
            ingredients = new List<CupIngredient>
            {
                CupIngredient.Espresso,
                CupIngredient.Espresso
            },
            score = 200
        });
    }

    public Recipe GetRandomRecipe()
    {
        return recipes[Random.Range(0, recipes.Count)];
    }

    public void DebugRandomRecipe()
    {
        Recipe recipe = RecipeDatabase.Instance.GetRandomRecipe();

        if (recipe == null)
        {
            Debug.LogWarning("?? 레시피가 비어있습니다.");
            return;
        }

        string ingredientList = "";

        foreach (var ing in recipe.ingredients)
        {
            ingredientList += ing.ToString() + " ";
        }

        Debug.Log($"?? 랜덤 레시피: {recipe.recipeName} / 재료: {ingredientList}");
    }
}
