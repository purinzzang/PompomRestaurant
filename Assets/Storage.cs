using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Newtonsoft.Json;

[System.Serializable]
public class Ingredient
{
    public string name;
    public int count;
    public bool isRefrigerator;
}

public class Storage : MonoBehaviour
{
    public List<Ingredient> ingredientList = new List<Ingredient>();
    public GameObject storagePanel;
    public bool isRefrigerator;

    private void Start()
    {
        InitStorage();
    }

    void InitStorage()
    {
        TextAsset json = Resources.Load<TextAsset>("IngredientData");
        List<Ingredient> ingredientData = JsonConvert.DeserializeObject<List<Ingredient>>(json.text);
        for (int i = 0; i < ingredientData.Count; i++)
        {
            if(ingredientData[i].isRefrigerator == this.isRefrigerator)
            {
                ingredientList.Add(ingredientData[i]);
            }
        }
    }

    public void ShowStorage()
    {
        if (this.isRefrigerator)
        {
            storagePanel.GetComponentInChildren<TextMeshProUGUI>().text = "<냉장고>";
        }
        else
        {
            storagePanel.GetComponentInChildren<TextMeshProUGUI>().text = "<선반>";
        }

        ScrollRect scrollRect = storagePanel.GetComponentInChildren<ScrollRect>();
        if(scrollRect.content.transform.childCount > 1)
        {
            for (int i = 1; i < scrollRect.content.transform.childCount; i++)
            {
                Destroy(scrollRect.content.transform.GetChild(i).gameObject);
            }
        }
        GameObject button = scrollRect.content.transform.GetChild(0).gameObject;
        for(int i = 0; i < ingredientList.Count; i++)
        {
            GameObject newButton = Instantiate(button, button.transform.parent);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = ingredientList[i].name + " X " + ingredientList[i].count;
            string name = ingredientList[i].name;
            newButton.GetComponent<Button>().onClick.AddListener(() => OnClickIngredientButton(name, newButton));
            newButton.SetActive(true);
        }
        storagePanel.SetActive(true);
    }

    public void HideStorage()
    {
        storagePanel.SetActive(false);
        ScrollRect scrollRect = storagePanel.GetComponentInChildren<ScrollRect>();
        for (int i = 1; i < scrollRect.content.transform.childCount; i++)
        {
            Destroy(scrollRect.content.transform.GetChild(i).gameObject);
        }
    }

    void OnClickIngredientButton(string name, GameObject button)
    {
        int index = ingredientList.FindIndex(x => x.name == name);
        if (index > -1)
        {
            Player.instance.GetIngredient(name);
            ingredientList[index].count--;
            if (ingredientList[index].count > 0)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = name + " X " + ingredientList[index].count;
            }
            else
            {
                ingredientList.RemoveAt(index);
                Destroy(button);
            }
        }
    }

    void SaveStorage()
    {
        string jsonString = JsonConvert.SerializeObject(ingredientList, Formatting.Indented);
        string filePath = Path.Combine(Application.persistentDataPath, "ingredient_storage_newtonsoft.json"); 
        try
        {
            File.WriteAllText(filePath, jsonString);
            Debug.Log("재료 목록(Newtonsoft Json) 저장 완료! 저장 경로: " + filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("재료 목록(Newtonsoft Json) 저장 실패! 오류: " + e.Message);
        }
    }
}
