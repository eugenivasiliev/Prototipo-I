using UnityEngine;
using UnityEngine.Localization.Settings;
public class LocalizationButton : MonoBehaviour
{
    public void ChangeToEnglish() { 
    
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];

    }
    public void ChangeToSpanish() { 
    
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[2];

    }

    public void ChangeToCatalan() {

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];

    }
}
