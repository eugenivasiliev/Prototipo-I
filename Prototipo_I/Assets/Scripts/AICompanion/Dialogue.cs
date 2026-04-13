using System;
using UnityEngine;

namespace AICompanion
{
    [Serializable]
    public struct OneTimeDialogue
    {
        [SerializeField] private string id;
        public string Id { get { return id; } }
        [SerializeField] private string text;
        public string Text { get { return text; } }
        public bool hasTriggered;

        public float lifeTime;
    }
}