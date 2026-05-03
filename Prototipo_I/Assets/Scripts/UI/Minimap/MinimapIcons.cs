using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Minimap
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
}