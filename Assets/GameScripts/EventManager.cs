using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace LOM
{

    using UnityActionList = List<UnityAction<EventObj>>;
    public class EventManager : MonoBehaviour
    {
        private Dictionary<string, UnityActionList> eventDictionary;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private static EventManager eventManager;

        public static EventManager instance
        {
            get
            {
                if (!eventManager)
                {
                    eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                    if (!eventManager)
                    {
                        Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                    }
                    else
                    {
                        eventManager.Init();
                    }
                }

                return eventManager;
            }
        }

        void Init()
        {
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<string, UnityActionList>();
            }
        }

        public static void RegisterEvent(string eventName, UnityAction<EventObj> aCallBack, bool bDontRegisterTheSameEvent = false)
        {
            UnityActionList actions = null;
            if (instance.eventDictionary.TryGetValue(eventName, out actions))
            {
                if (bDontRegisterTheSameEvent && actions.IndexOf(aCallBack) != -1)
                {
                    //dont register it again : (
                }
                else
                    actions.Add(aCallBack);
            }
            else
            {
                actions = new UnityActionList();
                actions.Add(aCallBack);
                instance.eventDictionary.Add(eventName, actions);
            }
        }

        public static void RemoveEvent(string eventName, UnityAction<EventObj> aCallBack)
        {
            if (eventManager == null) return;
            UnityActionList actions = null;
            if (instance.eventDictionary.TryGetValue(eventName, out actions))
            {
                actions.Remove(aCallBack);
            }
        }

        public static void TriggerEvent(EventObj anEventObj)
        {
            UnityActionList actions = null;
            if (instance.eventDictionary.TryGetValue(anEventObj.eventName, out actions))
            {
                for (int i = actions.Count - 1; i >= 0; i--) //warning: go backwards cuz actions may remove themselves from the list
                    actions[i].Invoke(anEventObj);
            }
        }
    }
}