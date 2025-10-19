using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.instance.PlayMusic("TitleScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
