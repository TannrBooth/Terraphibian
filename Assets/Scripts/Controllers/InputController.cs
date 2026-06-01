using UnityEngine;

namespace Terraphibian
{
    public abstract class InputController : ScriptableObject
    {
        public abstract float RetrieveMoveInput(GameObject gameObject);

        public abstract float RetrieveUpDownInput(GameObject gameObject);

        public abstract bool RetrieveJumpInput(GameObject gameObject);

        public abstract bool RetrieveMeleeInput(GameObject gameObject);

        public abstract bool RetrieveMeleeInputDown(GameObject gameObject);

        public abstract bool RetrieveSowInput(GameObject gameObject);

        public abstract bool RetrieveSowInputDown(GameObject gameObject);
    }
}
