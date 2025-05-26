using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player instance;

    // 들고있는 재료 리스트 
    public List<Ingredient> ingredientList = new List<Ingredient>();

    public GameObject myStoragePanel;
    public ScrollRect scroll1, scroll2;

    string curType;

    private void Awake()
    {
        instance = this;
    }

    // 선택한 재료 얻기 
    public void GetIngredient(string name)
    {
        int index = ingredientList.FindIndex(x => x.name == name);
        if (index > -1)
        {
            ingredientList[index].count++;
        }
        else
        {
            Ingredient newIngredient = new Ingredient();
            newIngredient.name = name;
            newIngredient.count = 1;
            ingredientList.Add(newIngredient);
        }
    }

    // 재료 사용하기 
    public void UseIngredients(string[] ingredients)
    {
        HideMyStorage();

        for (int i = 0; i < ingredients.Length; i++)
        {
            int index = ingredientList.FindIndex(x => x.name == ingredients[i]);
            if (index == -1)
            {
                continue;
            }
            else
            {
                ingredientList[index].count--;
                if (ingredientList[index].count <= 0)
                {
                    ingredientList.RemoveAt(index);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Shelf" || other.gameObject.tag == "Refrigerator")
        {
            other.GetComponent<Storage>().ShowStorage();
        }
        else if(other.gameObject.tag == "Fry" || other.gameObject.tag == "Boil")
        {
            ShowMyStorage();
            curType = other.gameObject.tag;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Shelf" || other.gameObject.tag == "Refrigerator")
        {
            other.GetComponent<Storage>().HideStorage();
        }
        else if (other.gameObject.tag == "Fry" || other.gameObject.tag == "Boil")
        {
            HideMyStorage();
        }
    }

    // 재료 선택창 표시 
    void ShowMyStorage()
    {
        RecipeManager.instance.ClearReadyList();
        if (scroll1.content.transform.childCount > 1)
        {
            for (int i = 1; i < scroll1.content.transform.childCount; i++)
            {
                Destroy(scroll1.content.transform.GetChild(i).gameObject);
            }
        }
        GameObject button = scroll1.content.transform.GetChild(0).gameObject;
        for (int i = 0; i < ingredientList.Count; i++)
        {
            GameObject newButton = Instantiate(button, button.transform.parent);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = ingredientList[i].name + " X " + ingredientList[i].count;
            string name = ingredientList[i].name;
            GameObject check = newButton.transform.Find("check").gameObject;
            newButton.GetComponent<Button>().onClick.AddListener(() => OnClickIngredientButton(name, check));
            newButton.SetActive(true);
        }
        myStoragePanel.SetActive(true);
    }

    // 재료 선택창 숨기기 
    void HideMyStorage()
    {
        myStoragePanel.SetActive(false);
        
        for (int i = 1; i < scroll1.content.transform.childCount; i++)
        {
            Destroy(scroll1.content.transform.GetChild(i).gameObject);
        }
        RecipeManager.instance.ClearReadyList();

        for (int i = 1; i < scroll2.content.transform.childCount; i++)
        {
            Destroy(scroll2.content.transform.GetChild(i).gameObject);
        }
        scroll2.gameObject.SetActive(false);
    }

    // 재료 선택 버튼 클릭 
    void OnClickIngredientButton(string name, GameObject check)
    {
        check.SetActive(RecipeManager.instance.CheckRecipe(name, curType));
    }
}
