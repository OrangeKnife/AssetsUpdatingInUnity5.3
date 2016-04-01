namespace LOM
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;
    using UnityEngine.Events;

    public class FullScreenOverlay : MonoBehaviour
    {
        public Text displayText;
        private string registeredEventName;
        private UnityAction<EventObj> registeredCallBack;


        public void Initialize(string text, float delayToKill = 3)
        {
            StartCoroutine(KillOverlay(delayToKill, text));
        }

        public void Initialize(string text, string eventName)
        {
            displayText.text = text;
            EventManager.RegisterEvent(eventName, OverlayCallback);
            registeredEventName = eventName;
            registeredCallBack = OverlayCallback;
        }

        private void RemoveEventAndCallBack()
        {
            if (registeredEventName != "" && registeredCallBack != null)
                EventManager.RemoveEvent(registeredEventName, registeredCallBack);
        }

        public void OverlayCallback(EventObj eo)
        {
            RemoveEventAndCallBack();
            if (eo.paramInt == Constants.INT_SUCCEED)
                Destroy(gameObject);
            else
                StartCoroutine(KillOverlay(2, eo.paramString));
        }

        private IEnumerator KillOverlay(float howLong, string err)
        {
            displayText.text = err;
            yield return new WaitForSeconds(howLong);
            Destroy(gameObject);
        }
    }
}