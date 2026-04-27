using System.Collections.Generic;
using System.Linq;
using Enemies;
using Farm;
using Objectives;
using UnityEngine;
using TowerDefense;
using Combat;
using UI;

namespace GameMode {
    public class WinLossTracker : MonoBehaviour
    {
        [Header("Fade")]
        [SerializeField] private FadingUI fadeOut;

        [Header("Win")]
        [SerializeField] private WinConditionUI win;
        [SerializeField] private ObjectivesManager objectivesManager;

        [Header("Loss")]
        [SerializeField] private Base home;
        [SerializeField] private LossConditionUI loss;
        [SerializeField] private EnemyManager enemyManager;
        [SerializeField] private Transform plots;
        private List<Plot> plotList = new List<Plot>();

        void Start()
        {
            plotList = plots.GetComponentsInChildren<Plot>().ToList<Plot>();
        }

        void Update()
        {
            if (objectivesManager.AllObjectivesComplete())
            {
                fadeOut.gameObject.SetActive(true);
                win.gameObject.SetActive(true);
            }

            if (home != null && ((IDamageable)home).Health < 5)
            {
                fadeOut.gameObject.SetActive(true);
                loss.gameObject.SetActive(true);
            }
        }

    }
}