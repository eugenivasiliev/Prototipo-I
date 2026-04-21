using System;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [Serializable]
    public struct MinimapIcon
    {
        public Transform reference;
        public RectTransform icon;
        public bool rotating;

        public MinimapIcon(Transform reference, RectTransform icon, bool rotating)
        {
            this.reference = reference;
            this.icon = icon;
            this.rotating = rotating;
        }
    }

    [SerializeField] private Transform projectionLowerLeft;
    [SerializeField] private Transform projectionTopRight;

    [SerializeField] private float mapWidth;
    [SerializeField] private float mapHeight;

    [SerializeField] private Transform minimapPanel;

    //[SerializeField] private MinimapIcon background;
    [SerializeField] private MinimapIcon player;
    [SerializeField] private MinimapIcon house;

    [SerializeField] private List<MinimapIcon> enemies;
    [SerializeField] private GameObject enemyIconPrefab;

    [SerializeField] private List<MinimapIcon> attackPlots;
    [SerializeField] private GameObject attackPlotsPrefab;
    [SerializeField] private List<MinimapIcon> defensePlots;
    [SerializeField] private GameObject defensePlotsPrefab;
    [SerializeField] private List<MinimapIcon> utilityPlots;
    [SerializeField] private GameObject utilityPlotsPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < attackPlots.Count; i++)
            attackPlots[i] = new MinimapIcon(attackPlots[i].reference, Instantiate(attackPlotsPrefab, minimapPanel).GetComponent<RectTransform>(), false);

        for (int i = 0; i < defensePlots.Count; i++)
            defensePlots[i] = new MinimapIcon(defensePlots[i].reference, Instantiate(defensePlotsPrefab, minimapPanel).GetComponent<RectTransform>(), false);

        for (int i = 0; i < utilityPlots.Count; i++)
            utilityPlots[i] = new MinimapIcon(utilityPlots[i].reference, Instantiate(utilityPlotsPrefab, minimapPanel).GetComponent<RectTransform>(), false);
    }

    // Update is called once per frame
    void Update()
    {
        //PlaceInMap(background);
        PlaceInMap(player);
        PlaceInMap(house);
        for (int i = 0; i < enemies.Count; i++)
        {
            while (enemies[i].reference == null)
            {
                Destroy(enemies[i].icon.gameObject);
                enemies.RemoveAt(i);
            }

            if(i < enemies.Count)
                PlaceInMap(enemies[i]);
        }

        for (int i = 0; i < attackPlots.Count; i++)
            PlaceInMap(attackPlots[i]);

        for (int i = 0; i < defensePlots.Count; i++)
            PlaceInMap(defensePlots[i]);

        for (int i = 0; i < utilityPlots.Count; i++)
            PlaceInMap(utilityPlots[i]);
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(new MinimapIcon(enemy.transform, Instantiate(enemyIconPrefab, minimapPanel).GetComponent<RectTransform>(), true));
    }
    private void PlaceInMap(MinimapIcon icon)
    {
        icon.icon.anchorMin = WorldToNormalMapPoint(icon.reference.position);
        icon.icon.anchorMax = icon.icon.anchorMin;
        icon.icon.gameObject.SetActive(
            !(icon.icon.anchorMin.x < 0 || icon.icon.anchorMin.x > 1 ||
            icon.icon.anchorMin.y < 0 || icon.icon.anchorMin.y > 1));
        if(icon.rotating)
            icon.icon.eulerAngles = Vector3.zero + icon.reference.eulerAngles.y * Vector3.forward;
    }
    private Vector2 WorldToNormalMapPoint(Vector3 worldPoint)
    {
        Vector2 normalisedPoint = new Vector2(
            (worldPoint - projectionLowerLeft.position).x / (projectionTopRight.position - projectionLowerLeft.position).x,
            (worldPoint - projectionLowerLeft.position).z / (projectionTopRight.position - projectionLowerLeft.position).z
            );

        return normalisedPoint;
    }

    private Vector2 WorldToMapPoint(Vector3 worldPoint)
    {
        Vector2 normalisedPoint = new Vector2(
            (worldPoint - projectionLowerLeft.position).x / (projectionTopRight.position - projectionLowerLeft.position).x,
            (worldPoint - projectionLowerLeft.position).z / (projectionTopRight.position - projectionLowerLeft.position).z
            );

        Vector2 mapPoint = new Vector2(mapWidth * normalisedPoint.x, mapHeight * normalisedPoint.y);

        return mapPoint;
    }
}
