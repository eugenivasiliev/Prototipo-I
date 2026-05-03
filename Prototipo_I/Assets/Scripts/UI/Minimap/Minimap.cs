using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Minimap
{

    public class Minimap : MonoBehaviour
    {
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

        [SerializeField] private List<MinimapIcon> spawnZones;
        [SerializeField] private MinimapIcon spawnZoneIcon;
        [SerializeField] private GameObject spawnZoneIconPrefab;

        void Update()
        {
            PlaceInMap(background);
            PlaceInMap(player);
            //PlaceInMap(house);
            for (int i = enemies.Count - 1; i >= 0; --i)
            {
                if (enemies[i].reference == null)
                {
                    Destroy(enemies[i].icon.gameObject);
                    enemies.RemoveAt(i);
                    continue;
                }
                
                PlaceInMap(enemies[i]);
            }

            foreach(MinimapIcon icon in spawnZones)
                ClampedPlaceInMap(icon);
        }

        public void AddEnemy(GameObject enemy)
        {
            enemies.Add(new MinimapIcon(enemy.transform, Instantiate(enemyIconPrefab, minimapPanel).GetComponent<RectTransform>(), true));
        }

        public void AddSpawnZone(GameObject spawnZone)
        {
            spawnZones.Add(new MinimapIcon(spawnZone.transform, Instantiate(spawnZoneIconPrefab, minimapPanel).GetComponent<RectTransform>(), false));
        }

        public void ClearSpawnZones()
        {
            spawnZones.Clear();
        }

        private void PlaceInMap(MinimapIcon icon)
        {
            icon.icon.anchorMin = WorldToNormalMapPoint(icon.reference.position);
            icon.icon.anchorMax = icon.icon.anchorMin;
            icon.icon.gameObject.SetActive(
                !(icon.icon.anchorMin.x < 0 || icon.icon.anchorMin.x > 1 ||
                icon.icon.anchorMin.y < 0 || icon.icon.anchorMin.y > 1));
            if (icon.rotating)
                icon.icon.eulerAngles = Vector3.zero - icon.reference.eulerAngles.y * Vector3.forward;
        }
        private void ClampedPlaceInMap(MinimapIcon icon)
        {
            icon.icon.anchorMin = Utils.Utils.Clamp(
                WorldToNormalMapPoint(icon.reference.position),
                Vector2.one * 0.1f,
                Vector2.one * 0.9f
                );
            icon.icon.anchorMax = icon.icon.anchorMin;
            icon.icon.gameObject.SetActive(
                !(icon.icon.anchorMin.x < 0 || icon.icon.anchorMin.x > 1 ||
                icon.icon.anchorMin.y < 0 || icon.icon.anchorMin.y > 1));
            if (icon.rotating)
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
}