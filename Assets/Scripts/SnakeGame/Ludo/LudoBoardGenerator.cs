using UnityEngine;
using TMPro;
public class LudoBoardGenerator : MonoBehaviour
{
    public RectTransform mainPath;
    public GameObject cellPrefab;

    float cellSize = 32f;
    float scale = 0.8f;
    [ContextMenu("Generate Ludo Path")]
    public void GenerateLudoPath() 
    { 
       
        while (mainPath.childCount > 0)
        { 
            DestroyImmediate(mainPath.GetChild(0).gameObject);
        } 
        
        Vector2[] path = new Vector2[] 
        {
            new(120,-240), new(120,-200),
            new(120,-160), new(120,-120), 
            new(120,-80), new(120,-40),
            new(80,0), new(40,0), new(0,0),
            new(-40,0), new(-80,0), new(-120,0),
            new(-160,40), new(-160,80), new(-120,80),
            new(-80,80), new(-40,80), new(0,80),
            new(40,80), new(80,80), new(120,120),
            new(120,160), new(120,200), new(120,240)
            , new(120,280), new(120,320), new(160,360),
            new(200,360), new(200,320), new(200,280),
            new(200,240), new(200,200), new(200,160),
            new(200,120), new(240,80), new(280,80),
            new(320,80), new(360,80), new(400,80), 
            new(440,80), new(480,40), new(480,0),
            new(440,0), new(400,0), new(360,0), 
            new(320,0), new(280,0), new(240,0), new(200,-40),
            new(200,-80), new(200,-120), new(200,-160),
            new(200,-40),
            new(160,-200), new(140,-220), new(120,-240)
        };
        for (int i = 0; i < path.Length; i++) 
        { 
            GameObject cell = Instantiate(cellPrefab, mainPath);
            RectTransform rt = cell.GetComponent<RectTransform>();
            rt.anchoredPosition = path[i] * scale;
            cell.name = $"Cell_{i:00}";
            TMP_Text txt = cell.GetComponentInChildren<TMP_Text>();
            if (txt != null)
                txt.text = i.ToString(); 
        }
        Debug.Log("Ludo path generated!");
    }
}