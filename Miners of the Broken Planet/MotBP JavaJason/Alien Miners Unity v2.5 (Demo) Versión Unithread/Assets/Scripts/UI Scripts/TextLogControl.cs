using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLogControl : MonoBehaviour {

    [SerializeField]
    private GameObject textTemplate;

    private List<GameObject> textList;

    private void Awake()
    {
        transform.GetComponentInChildren<Scrollbar>().size = 0;
    }


    private void Start()
    {
        textList = new List<GameObject>();
    }

    public void LogText(string newTextString, Color newColour)
    {
        if (textList.Count == 10)
        {
            GameObject tempText = textList[0];
            Destroy(tempText);
            textList.Remove(tempText);
        }

        GameObject newText = Instantiate(textTemplate) as GameObject;
        newText.SetActive(true);

        newText.GetComponent<TextLogItem>().SetText(newTextString, newColour);
        newText.transform.SetParent(textTemplate.transform.parent, false);

        textList.Add(newText.gameObject);
    }
}
