using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimalUI : MonoBehaviour
{
    private Animal animal;
    private Canvas canvas;

    [SerializeField] private TextMeshProUGUI statusText;

    private void Awake()
    {
        if (animal == null)
            animal = GetComponentInParent<Animal>();

        if (canvas == null)
            canvas = GetComponentInChildren<Canvas>();
    }

    private void LateUpdate()
    {
        if (animal == null) return;

        canvas.transform.LookAt(Camera.main.transform);
        canvas.transform.Rotate(0, 180f, 0);

        statusText.text = "";
        //statusText.text = $"Estado AI: {animal.GetComponent<AnimalAI>()?.CurrentStateName ?? "N/A"}\n" +
        //              $"Hambriento: {animal.IsHungry}\n" +
        //              $"Puede Reproducirse: {animal.canBreed}\n" +
        //              $"Veces Comido: {animal.MealsEaten}/{animal.MaxMealsEaten}";
    }
}
