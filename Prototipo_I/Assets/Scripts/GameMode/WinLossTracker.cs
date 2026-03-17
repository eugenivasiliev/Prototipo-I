using System.Collections.Generic;
using System.Linq;
using Enemies;
using Farm;
using Objectives;
using UnityEngine;
using TowerDefense;

namespace GameMode {
    public class WinLossTracker : MonoBehaviour
    {
        [Header("Win")]
        [SerializeField] private WinConditionUI win;
        [SerializeField] private ObjectivesManager objectivesManager;

        [Header("Loss")]
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
            if(objectivesManager.AllObjectivesComplete())
                win.gameObject.SetActive(true);

            if (Base.instance.Health < 5.0f)
                loss.gameObject.SetActive(true);            
        }

    }
}