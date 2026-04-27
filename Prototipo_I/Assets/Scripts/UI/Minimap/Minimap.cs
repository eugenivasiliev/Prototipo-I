using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Serializable]
    public struct MinimapScalableIcon
    {
        public Transform bottomLeft;
        public Transform topRight;
        public RectTransform icon;
        public Image scaledImage;

        public MinimapScalableIcon(Transform bottomLeft, Transform topRight, RectTransform icon, Image scaledImage)
        {
            this.bottomLeft = bottomLeft;
            this.topRight = topRight;
            this.icon = icon;
            this.scaledImage = scaledImage;
        }
    }

    [SerializeField] private Transform projectionLowerLeft;
    [SerializeField] private Transform projectionTopRight;

    [SerializeField] private float mapWidth;
    [SerializeField] private float mapHeight;

    [SerializeField] private Transform minimapPanel;

    [SerializeField] private MinimapScalableIcon background;
    [SerializeField] private MinimapIcon player;
    [SerializeField] private MinimapIcon house;

    [SerializeField] private List<MinimapIcon> enemies;
    [SerializeField] private GameObject enemyIconPrefab;

    void Update()
    {
        PlaceInMap(background);
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
            icon.icon.eulerAngles = Vector3.zero - icon.reference.eulerAngles.y * Vector3.forward;
    }
    private void PlaceInMap(MinimapScalableIcon icon)
    {
        icon.icon.anchorMin = WorldToNormalMapPoint(icon.bottomLeft.position);
        icon.icon.anchorMax = WorldToNormalMapPoint(icon.topRight.position);
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
