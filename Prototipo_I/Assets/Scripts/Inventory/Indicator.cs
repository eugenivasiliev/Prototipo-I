using UnityEngine;

public class Indicator : MonoBehaviour
{
    public int CurrentIndex { get; private set; }
    private RectTransform rt;
    [SerializeField] private float scaledToItem = 1.3f;

    public void Initialize(Vector2 itemSize)
    {
        rt = GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemSize.x * scaledToItem);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemSize.y * scaledToItem);
        rt.position = new Vector2(-Screen.width, -Screen.height);
        CurrentIndex = -1;

        //Performed individually, since local variables aren't conserved as constant
        PlayerController.Inputs.FindAction("Alpha1").canceled += ctx => { CurrentIndex = 0; MoveToCurItem(); };
        PlayerController.Inputs.FindAction("Alpha2").canceled += ctx => { CurrentIndex = 1; MoveToCurItem(); };
        PlayerController.Inputs.FindAction("Alpha3").canceled += ctx => { CurrentIndex = 2; MoveToCurItem(); };
        PlayerController.Inputs.FindAction("Alpha4").canceled += ctx => { CurrentIndex = 3; MoveToCurItem(); };
        PlayerController.Inputs.FindAction("Alpha5").canceled += ctx => { CurrentIndex = 4; MoveToCurItem(); };
        PlayerController.Inputs.FindAction("Alpha6").canceled += ctx => { CurrentIndex = 5; MoveToCurItem(); };
        PlayerController.Inputs.FindAction("Alpha7").canceled += ctx => { CurrentIndex = 6; MoveToCurItem(); };
        PlayerController.Inputs.FindAction("Alpha8").canceled += ctx => { CurrentIndex = 7; MoveToCurItem(); };
    }

    private void MoveToCurItem() => rt.position = Inventory.Instance.GetItemUIPosition(CurrentIndex);
}
