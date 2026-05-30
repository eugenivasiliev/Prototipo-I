using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    private EnemyCounterList.EnemyCounterInfo info;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image image;
    public EnemyCounterList.EnemyCounterInfo Info {
        get
        {
            return info;
        }

        set
        {
            info = value;
            text.text = value.amount + " " + value.name;
            image.sprite = value.sprite;
        }
    }
}
