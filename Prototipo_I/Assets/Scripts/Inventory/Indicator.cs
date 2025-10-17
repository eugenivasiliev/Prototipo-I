using UnityEngine;

public class Indicator : MonoBehaviour
{
    public int CurrentIndex { get; private set; }
    private RectTransform rt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rt = GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 90);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 90);
        rt.position = Inventory.Instance.GetItemUIPosition(0);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < Inventory.Instance.inventorySpace; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                CurrentIndex = i;
                rt.position = Inventory.Instance.GetItemUIPosition(i);
            }
        }
    }
}
