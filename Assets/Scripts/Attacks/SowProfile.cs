using System.Collections.Generic;
using Terraphibian;
using UnityEngine;

namespace Terraphibian
{
    public enum SowDirection
    {
        Neutral,
        Up,
        Down
    }

    [System.Serializable]
    public class SowData
    {
        public Vector2 localPosition;
        public Vector2 localScale = Vector2.one;
        public float rotationZ;
    }


    [System.Serializable]
    public class SowConfig
    {
        public SowDirection direction;
        public SowData data;
    }

    [CreateAssetMenu(fileName = "SowProfile", menuName = "Combat/Sow Profile")]
    public class SowProfile : ScriptableObject
    {
        public List<SowConfig> sowConfigs = new();
    }
}