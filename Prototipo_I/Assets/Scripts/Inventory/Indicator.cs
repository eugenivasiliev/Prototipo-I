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
        PlayerController.Inputs.FindAction("Alpha1").canceled += ctx => { UpdateIndex(0); };
        PlayerController.Inputs.FindAction("Alpha2").canceled += ctx => { UpdateIndex(1); };
        PlayerController.Inputs.FindAction("Alpha3").canceled += ctx => { UpdateIndex(2); };
        PlayerController.Inputs.FindAction("Alpha4").canceled += ctx => { UpdateIndex(3); };
        PlayerController.Inputs.FindAction("Alpha5").canceled += ctx => { UpdateIndex(4); };
        PlayerController.Inputs.FindAction("Alpha6").canceled += ctx => { UpdateIndex(5); };
        PlayerController.Inputs.FindAction("Alpha7").canceled += ctx => { UpdateIndex(6); };
        PlayerController.Inputs.FindAction("Alpha8").canceled += ctx => { UpdateIndex(7); };
    }

    private void UpdateIndex(int index)
    {
        Item curItem = Inventory.Instance.GetCurrentItem();
        if (curItem != null && curItem is IInteractable)
            (curItem as IInteractable).Unbind();

        CurrentIndex = index;
        rt.position = Inventory.Instance.GetItemUIPosition(CurrentIndex);

        curItem = Inventory.Instance.GetCurrentItem();
        if (curItem is IInteractable)
            (curItem as IInteractable).Bind();
    }
}
