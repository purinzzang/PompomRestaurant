using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        ScrollRect scrollRect = Player.instance.scroll2;
        if (scrollRect.content.childCount > 1)
        {
            for (int i = 1; i < scrollRect.content.childCount; i++)
            {
                Destroy(scrollRect.content.GetChild(i).gameObject);
            }
        }
        if (scrollRect.gameObject.activeInHierarchy)
        {
            scrollRect.gameObject.SetActive(false);
        }

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
                    ShowAvailableRecipe(recipeBook[i].result);
                }
            }

        }
    }

    void ShowAvailableRecipe(string result)
    {
        ScrollRect scrollRect = Player.instance.scroll2;

        GameObject newButton = Instantiate(scrollRect.content.GetChild(0), scrollRect.content).gameObject;
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = result;
        newButton.SetActive(true);

        if (!scrollRect.gameObject.activeInHierarchy)
        {
            scrollRect.gameObject.SetActive(true);
        }
    }


    public void ClearReadyList()
    {
        if (readyList.Count > 0)
        {
            readyList.Clear();
        }
    }
}
