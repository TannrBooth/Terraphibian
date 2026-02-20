using System.Collections.Generic;
using Terraphibian;
using UnityEngine;

namespace Terraphibian
{
    public enum AttackDirection
    {
        Neutral,
        Up,
        Down
    }

    [System.Serializable]
    public class AttackBoxData
    {
        public Vector2 localPosition;
        public Vector2 localScale = Vector2.one;
        public float rotationZ;
        public float duration = 0.1f;
    }


    [System.Serializable]
    public class AttackBoxConfig
    {
        public AttackDirection direction;
        public AttackBoxData data;
    }

    [CreateAssetMenu(fileName = "AttackBoxProfile", menuName = "Combat/Attack Box Profile")]
    public class AttackBoxProfile : ScriptableObject
    {
        public List<AttackBoxConfig> attackConfigs = new();
    }
}