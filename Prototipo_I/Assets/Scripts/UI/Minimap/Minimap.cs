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

        public MinimapIcon(Transform reference, RectTransform icon)
        {
            this.reference = reference;
            this.icon = icon;
        }
    }

    [SerializeField] private Transform projectionLowerLeft;
    [SerializeField] private Transform projectionTopRight;

    [SerializeField] private float mapWidth;
    [SerializeField] private float mapHeight;

    [SerializeField] private Transform minimapPanel;

    [SerializeField] private MinimapIcon player;
    [SerializeField] private MinimapIcon house;

    [SerializeField] private List<MinimapIcon> enemies;
    [SerializeField] private GameObject enemyIconPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(new MinimapIcon(enemy.transform, Instantiate(enemyIconPrefab, minimapPanel).GetComponent<RectTransform>()));
    }
    private void PlaceInMap(MinimapIcon icon)
    {
        icon.icon.anchorMin = WorldToNormalMapPoint(icon.reference.position);
        icon.icon.anchorMax = icon.icon.anchorMin;
        icon.icon.gameObject.SetActive(
            !(icon.icon.anchorMin.x < 0 || icon.icon.anchorMin.x > 1 ||
            icon.icon.anchorMin.y < 0 || icon.icon.anchorMin.y > 1));
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
