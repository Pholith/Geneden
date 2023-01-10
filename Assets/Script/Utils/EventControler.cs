using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class EventControler : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent methodToCall;

        [SerializeField]
        private UnityEvent methodToCallWhenNotActivated;

        public bool Activated
        {
            get { return activated; }
            set { activated = value; }
        }
        [SerializeField]
        private bool activated = false;

        public void Switch()
        {
            activated = !activated;
        }

        private void Update()
        {
            if (activated)
            {
                methodToCall?.Invoke();
            } else
            {
                methodToCallWhenNotActivated?.Invoke();
            }
        }

        public void SetOnForXSeconds(int seconds)
        {
            activated = true;
            StartCoroutine(ExampleCoroutine(seconds));
        }

        private IEnumerator ExampleCoroutine(int seconds)
        {
            //yield on a new YieldInstruction that waits for 5 seconds.
            yield return new WaitForSeconds(seconds);
            activated = false;
        }
    }
}

