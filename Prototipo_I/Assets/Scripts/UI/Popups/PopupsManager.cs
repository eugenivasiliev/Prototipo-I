using System.Collections;
using TMPro;
using UnityEngine;

public class PopupsManager : Singleton<PopupsManager>
{

    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float popupDuration = 0.5f;

    private CanvasGroup canvasGroup;
    private Coroutine currentPopup;

    private void Awake()
    {
        InitSingleton();
        DontDestroyOnLoad(gameObject);

        canvasGroup = popupPanel.GetComponent<CanvasGroup>();
        
        popupPanel.SetActive(false);
    }

    public void ShowMessage(string message, float duration = 3f)
    {
        if (currentPopup != null)
            StopCoroutine(currentPopup);

        currentPopup = StartCoroutine(PopupCoroutine(message, duration));
    }

    private IEnumerator PopupCoroutine(string message, float duration)
    {
        popupPanel.SetActive(true);
        messageText.text = message;

        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f, popupDuration));

        yield return new WaitForSeconds(duration);

        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 1f, 0f, popupDuration));

        popupPanel.SetActive(false);
        currentPopup = null;
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup group, float start, float end, float time)
    {
        float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, end, elapsed / time);
            yield return null;
        }
        group.alpha = end;
    }
}
