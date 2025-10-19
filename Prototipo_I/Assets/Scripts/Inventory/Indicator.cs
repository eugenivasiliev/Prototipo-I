using UnityEngine;

public class Indicator : MonoBehaviour
{
    public int CurrentIndex { get; private set; }
    private RectTransform rt;
    [SerializeField] private float scaledToItem = 1.3f;

    void Update()
    {
        for(int i = 0; i < Inventory.Instance.InventorySpace; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                CurrentIndex = i;
                rt.position = Inventory.Instance.GetItemUIPosition(i);
            }
        }
    }

    public void Initialize(Vector2 itemSize)
    {
        rt = GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemSize.x * scaledToItem);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemSize.y * scaledToItem);
        rt.position = new Vector2(-Screen.width, -Screen.height);
        CurrentIndex = -1;
    }
}
