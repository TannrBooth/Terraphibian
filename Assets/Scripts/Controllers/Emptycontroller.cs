using UnityEngine;

namespace Terraphibian
{
    [CreateAssetMenu(fileName = "EmptyController", menuName = "InputController/EmptyController")]
    public class EmptyController : InputController
    {

        public override bool RetrieveJumpInput(GameObject gameObject)
        {
            return false;
        }

        public override float RetrieveMoveInput(GameObject gameObject)
        {
            return 0f;
        }

        public override float RetrieveUpDownInput(GameObject gameObject)
        {
            return 0f;
        }

        public override bool RetrieveMeleeInput(GameObject gameObject)
        {
            return false;
        }

        public override bool RetrieveMeleeInputDown(GameObject gameObject)
        {
            return false;
        }
    }
}