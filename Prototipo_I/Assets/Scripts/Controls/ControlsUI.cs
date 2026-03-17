using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsUI : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    private InputSystem_Actions inputs;

    [SerializeField] private Vector4 hiddenPos;
    [SerializeField] private Vector4 visiblePos;

    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField, Range(0, 1)] private float animationDuration;

    private bool isHidden = true;
    void Start()
    {
        if (inputs == null) inputs = new InputSystem_Actions();
        inputs.Player.Enable();
        inputs.Player.objectives_toggle.performed += Toggle;

        StartCoroutine(ToggleAnim());
    }

    void Toggle(InputAction.CallbackContext ctx)
    {
        StartCoroutine(ToggleAnim());
    }

    private IEnumerator ToggleAnim()
    {
        Vector4 startPos = (isHidden) ? hiddenPos : visiblePos;
        Vector4 endPos = (isHidden) ? visiblePos : hiddenPos;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / animationDuration;
            float alpha = animationCurve.Evaluate(t);

            Vector2 anchorMin = new Vector2(
                (1 - alpha) * startPos.x + alpha * endPos.x,
                (1 - alpha) * startPos.y + alpha * endPos.y
                );

            Vector2 anchorMax = new Vector2(
                (1 - alpha) * startPos.z + alpha * endPos.z,
                (1 - alpha) * startPos.w + alpha * endPos.w
                );

            panel.anchorMin = anchorMin;
            panel.anchorMax = anchorMax;

            yield return new WaitForEndOfFrame();
        }

        isHidden = !isHidden;

        yield return null;
    }
}
