using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class DialogueControl : MonoBehaviour
{

    public GameObject text;

    /// <summary>
    /// Initialize speech bubble
    /// </summary>
    /// <param name="text">String person or monster is saying</param>
    /// <param name="offSet">The amount of offset of the speech bubble from saying person monster, new Vector(-0.1, 1.2) works well, but it depends on the talking sprite size</param>
    /// <param name="parent">The person/monster talking</param>
    public void Init(string text, Vector2 offSet, GameObject parent)
    {
        float offsetx = 0f;
        float offsety = 0f;
        transform.parent = parent.transform;
        

        int scalability = text.Length;

        RectTransform rt = GetComponent<RectTransform>();

        while (scalability > 5) { 
            if (scalability > 5) { rt.sizeDelta = Vector2.right + rt.sizeDelta; scalability -= 5; offsetx -= 0.5f; }
            if (scalability > 5) { rt.sizeDelta = Vector2.right + rt.sizeDelta; scalability -= 5; offsetx -= 0.5f; }
            if (scalability > 5) { rt.sizeDelta = Vector2.up + rt.sizeDelta; scalability -= 10; offsety += 0.5f; }
        }
        offSet.x += offsetx;
        offSet.y += offsety;
        transform.localPosition = offSet;
        this.text.GetComponent<RectTransform>().offsetMin = new Vector2(rt.sizeDelta.x * 0.1f, rt.sizeDelta.y * 0.1f);
        this.text.GetComponent<RectTransform>().offsetMax = new Vector2(rt.sizeDelta.x * -0.1f, rt.sizeDelta.y * -0.1f);
        StartCoroutine(WriteText(0.07f, text));
    }

    private IEnumerator WriteText(float cd, string toWrite)
    {
        int index = 0;
        text.GetComponent<TMP_Text>().fontSize = 0.2f;
        while (index <= toWrite.Length)
        {
            yield return new WaitForSeconds(cd);
            text.GetComponent<TMP_Text>().text = text.GetComponent<TMP_Text>().text + toWrite[index];
            index++;
            if(index >= toWrite.Length)
            {
                break;
            }
        }
        yield return new WaitForSeconds(4.0f);
        Destroy(gameObject);
    }
}
