using System.Collections;
using Objectives;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class ObjectivesUI : SlidingPanelUI
    {
        [SerializeField] private TMP_Text text;

        protected override void Update()
        {
            base.Update();

            if (ObjectivesManager.Instance.AllObjectivesComplete())
            {
                text.text = "You completed all objectives!";
                text.color = Color.green;
                return;
            }

            text.text = "Objectives:\n";
            foreach (var obj in ObjectivesManager.Instance.Objectives)
            {
                if ((obj as IObjective).IsCompleted) continue;
                text.text += (obj as IObjective).Text();
            }
        }
    }
}