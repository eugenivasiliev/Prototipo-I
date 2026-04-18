using UnityEngine;

public class TutorialBlocker : MonoBehaviour
{
    [SerializeField] KeyCode whatKey;

    private void Update()
    {
        if (Input.GetKeyDown(whatKey))
        {
            KeyPressed();
        }
    }

    void KeyPressed() { 
    
        Destroy(gameObject);
    }
}
