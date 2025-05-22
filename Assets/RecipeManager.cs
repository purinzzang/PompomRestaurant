using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe
{
    public string[] ingredients;
    public string type;
    public string result;
}

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager instance;

    public List<Recipe> recipeBook = new List<Recipe>();
    public List<string> readyList = new List<string>();

    private void Awake()
    {
        instance = this;
    }

    public bool CheckRecipe(string name, string type)
    {
        

        if (readyList.Contains(name))
        {
            readyList.Remove(name);
            CheckRecipeBook(type);
            return false;
        }
        else
        {
            readyList.Add(name);
            CheckRecipeBook(type);
            return true;
        }
    }

    void CheckRecipeBook(string type)
    {
        for (int i = 0; i < recipeBook.Count; i++)
        {
            if(type != recipeBook[i].type)
            {
                continue;
            }
            for (int j = 0; j < recipeBook[i].ingredients.Length; j++)
            {
                if (!readyList.Contains(recipeBook[i].ingredients[j]))
                {
                    break;
                }
                if (j == recipeBook[i].ingredients.Length - 1)
                {
                    Debug.Log(recipeBook[i].result);
                }
            }

        }
    }
}
