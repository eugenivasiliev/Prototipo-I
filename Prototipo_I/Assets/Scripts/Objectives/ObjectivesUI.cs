using TMPro;
using UnityEngine;

public class ObjectivesUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ObjectivesManager.Instance.AllObjectivesComplete())
        {
            text.text = "You completed all objectives!";
            text.color = Color.green;
            return;
        }

        text.text = "Objectives:\n";
        foreach(var obj in ObjectivesManager.Instance.Objectives)
        {
            if ((obj as IObjective).IsCompleted) continue;
            text.text += (obj as IObjective).Text();
        }
    }
}
