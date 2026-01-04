using System.Collections.Generic;
using UnityEngine;

public enum RecipeType
{
    Espresso,
    Americano,
    CafeLatte,
    ChocoLatte,
    CafeMocha,
    DoubleShot
}

[CreateAssetMenu(menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    public RecipeType recipeType;
    public string recipeName;
    public List<CupIngredient> ingredients;
    public int score;
}
public class RecipeDatabase : MonoBehaviour
{
    public static RecipeDatabase Instance;
    List<Recipe> recipes = new List<Recipe>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        InitRecipes();
    }

    void InitRecipes()
    {
        recipes.Add(CreateRecipe(
            RecipeType.Espresso,
            "에스프레소",
            new List<CupIngredient> { CupIngredient.Espresso },
            100
        ));

        recipes.Add(CreateRecipe(
            RecipeType.Americano,
            "아메리카노",
            new List<CupIngredient>
            {
                CupIngredient.Espresso,
                CupIngredient.Water
            },
            150
        ));

        recipes.Add(CreateRecipe(
            RecipeType.CafeLatte,
            "카페라떼",
            new List<CupIngredient>
            {
                CupIngredient.Espresso,
                CupIngredient.Milk
            },
            180
        ));

        recipes.Add(CreateRecipe(
            RecipeType.ChocoLatte,
            "초코라떼",
            new List<CupIngredient>
            {
                CupIngredient.Milk,
                CupIngredient.Choco
            },
            160
        ));

        recipes.Add(CreateRecipe(
            RecipeType.CafeMocha,
            "카페모카",
            new List<CupIngredient>
            {
                CupIngredient.Espresso,
                CupIngredient.Milk,
                CupIngredient.Choco
            },
            220
        ));

        recipes.Add(CreateRecipe(
            RecipeType.DoubleShot,
            "더블샷",
            new List<CupIngredient>
            {
                CupIngredient.Espresso,
                CupIngredient.Espresso
            },
            200
        ));
    }

    Recipe CreateRecipe(
        RecipeType type,
        string name,
        List<CupIngredient> ingredients,
        int score)
    {
        // ? ScriptableObject 생성 방식 OK
        Recipe recipe = ScriptableObject.CreateInstance<Recipe>();
        recipe.recipeType = type;
        recipe.recipeName = name;
        recipe.ingredients = ingredients;
        recipe.score = score;

        return recipe;
    }

    public Recipe GetRandomRecipe()
    {
        if (recipes.Count == 0)
            return null;

        return recipes[Random.Range(0, recipes.Count)];
    }

    public void DebugRandomRecipe()
    {
        Recipe recipe = GetRandomRecipe();

        if (recipe == null)
        {
            Debug.LogWarning("? 레시피가 비어있습니다.");
            return;
        }

        string ingredientList = "";
        foreach (var ing in recipe.ingredients)
            ingredientList += ing + " ";

        Debug.Log($" 랜덤 레시피: {recipe.recipeName} / 재료: {ingredientList}");
    }
}
