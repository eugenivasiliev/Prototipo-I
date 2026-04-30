using UnityEngine;
using UnityEngine.Localization.Settings;

public class CutsceneSelector : MonoBehaviour
{
    [SerializeField] private GameObject cutsceneEng;
    [SerializeField] private GameObject cutsceneCast;
    [SerializeField] private GameObject cutsceneCat;
    void Start()
    {
        if (LocalizationSettings.SelectedLocale.Identifier.Code == "en")
        {
            cutsceneEng.SetActive(true);
        }
        else if (LocalizationSettings.SelectedLocale.Identifier.Code == "es")
        {
            cutsceneCast.SetActive(true);
        }
        else if (LocalizationSettings.SelectedLocale.Identifier.Code == "ca")
        {
            cutsceneCat.SetActive(true);
        }

    }
}
