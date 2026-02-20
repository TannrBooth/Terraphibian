using System.Collections.Generic;
using UnityEngine;

namespace Terraphibian
{
    public class AttackBoxController : MonoBehaviour
    {
        [SerializeField] private AttackBoxProfile _attackBoxProfile;

        private Dictionary<AttackDirection, AttackBoxData> _configLookup;
        private Vector3 _defaultLocalPosition;
        private Vector3 _defaultLocalScale;
        private Quaternion _defaultLocalRotation;
        private Melee _melee;
        private Move _move;

        public bool _pushbackApplied;


        public void Awake()
        {
            _move = GetComponentInParent<Move>();
            _melee = GetComponentInParent<Melee>();

            _pushbackApplied = false;

            // Store defaults to restore after attack
            _defaultLocalPosition = transform.localPosition;
            _defaultLocalScale = transform.localScale;
            _defaultLocalRotation = transform.localRotation;

            // Build lookup
            _configLookup = new Dictionary<AttackDirection, AttackBoxData>();
            foreach (var config in _attackBoxProfile.attackConfigs)
            {
                _configLookup[config.direction] = config.data;
            }
        }

        public void ApplyConfig(AttackDirection dir)
        {
            if (_configLookup.TryGetValue(dir, out var data))
            {
                transform.localPosition = data.localPosition;
                transform.localScale = data.localScale;
                transform.localRotation = Quaternion.Euler(0, 0, data.rotationZ);
            }
            else
            {
                Debug.LogWarning($"No attack box config found for direction {dir}");
            }
        }

        public void ResetTransform()
        {
            transform.localPosition = _defaultLocalPosition;
            transform.localScale = _defaultLocalScale;
            transform.localRotation = _defaultLocalRotation;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Wall") && !_pushbackApplied)
            {
                if (_move != null)
                    _move.ApplyPushback(-_move.transform.localScale.normalized, _melee._pushbackMultiplier);
            }
            if (col.CompareTag("Enemy"))
            {
                if (_pushbackApplied) return; // Prevent multiple pushbacks per attack
                // Player pushback
                if (_move != null)
                    
                    _move.ApplyPushback(-_move.transform.localScale.normalized,  _melee._pushbackMultiplier);
                    _pushbackApplied = true;
                // Enemy pushback
                if (col.TryGetComponent<Move>(out var enemyMove))
                    enemyMove.ApplyPushback(_move.transform.localScale.normalized, 1/col.GetComponent<Move>()._pushbackWeight);

                // TODO: Deal damage
            }

        }
    }
}