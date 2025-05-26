using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[System.Serializable]
public class Recipe
{
    public string[] ingredients;
    public string type;
    public string result;
    public float time;
}

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager instance;

    // 레시피 모음 
    public List<Recipe> recipeBook = new List<Recipe>();
    // 선택한 재료 리스트 
    public List<string> readyList = new List<string>();

    private void Awake()
    {
        instance = this;
    }

    // 재료 선택할 때마다 호출
    // 이미 선택한 재료인지 확인 
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

    // 선택한 조리 타입 중에서 재료 이름으로 해당하는 레시피 검색
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
                    ShowAvailableRecipe(i);
                }
            }

        }
    }

    // 가능한 레시피가 있으면 조리 버튼을 표시 
    void ShowAvailableRecipe(int recipeIndex)
    {
        ScrollRect scrollRect = Player.instance.scroll2;

        GameObject newButton = Instantiate(scrollRect.content.GetChild(0), scrollRect.content).gameObject;
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = recipeBook[recipeIndex].result;
        newButton.GetComponent<Button>().onClick.AddListener(() => OnClickCookButton(recipeIndex));
        newButton.SetActive(true);

        if (!scrollRect.gameObject.activeInHierarchy)
        {
            scrollRect.gameObject.SetActive(true);
        }
    }

    // 선택한 재료 리스트 비우기 
    public void ClearReadyList()
    {
        if (readyList.Count > 0)
        {
            readyList.Clear();
        }
    }

    void OnClickCookButton(int recipeIndex)
    {
        StartCoroutine(CookCo(recipeIndex));
    }

    IEnumerator CookCo(int recipeIndex)
    {
        Player.instance.UseIngredients(recipeBook[recipeIndex].ingredients);

        for(int i = 0; i < recipeBook[recipeIndex].time; i++)
        {
            yield return new WaitForSeconds(1f);
        }

        Player.instance.GetIngredient(recipeBook[recipeIndex].result);
    }
}
