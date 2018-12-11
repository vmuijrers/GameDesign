using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

//Add you messages here
public enum MessageEnum
{
	NONE            = 0,
    ON_REMOVE_ABSTRACT_COMPONENT = 1,
    ON_ADD_ABSTRACT_COMPONENT = 2,
    ON_SACRIFICE    = 3,
    GAME_OVER       = 4,
    ON_CHARACTER_DEAD = 5,
    ON_BREAK_CARRIAGE = 6,
    GAME_START        = 7,
    ON_BRANCH_DANGER  = 8,
    RESTART_GAME      = 9,
    SLOWMOTION        = 10



}

public static class Message
{

    private static Dictionary<MessageEnum, Dictionary<GameObject, Action>> messages = new Dictionary<MessageEnum, Dictionary<GameObject, Action>>();

    //Message.SendMessage(MyEventNum.HitEvent, gameObject, otherGameobject)
    public static void SendMessage(MessageEnum ev, params GameObject[] objs)
    {
        if (ev == MessageEnum.NONE)
        {
            Debug.Log("Message sender is not setup for Event! " + ev.ToString() );
            return;
        }
        if (messages.ContainsKey(ev))
        {
            if (objs.Length > 0)
            {
                for(int i = objs.Length -1; i >=0; i--)
                {
                    GameObject obj = objs[i];
                    if (messages[ev].ContainsKey(obj))
                    {
                        messages[ev][obj].Invoke();
                    }
                }
            }
            else
            {
                List<KeyValuePair<GameObject, Action>> values = messages[ev].ToList();
                for (int i = values.Count - 1; i >= 0; i--)
                //foreach (KeyValuePair<GameObject, Action> kv in messages[ev])
                {
                    values[i].Value.Invoke();
                }

            }
        }
        else
        {
            Debug.Log("No Listeners for event: "+ev.ToString() );
        }
    }


    public static void AddListener(MessageEnum ev, GameObject listener, Action action)
    {
        if(ev == MessageEnum.NONE)
        {
            Debug.Log("Listener is not setup by: " + listener.name);
            return;
        }

        if (!messages.ContainsKey(ev))
        {
            messages.Add(ev, new Dictionary<GameObject, Action>());
        }
        if (!messages[ev].ContainsKey(listener))
        {
            messages[ev].Add(listener, null);

        }
        messages[ev][listener] += action;

    }
    public static void RemoveListener(MessageEnum ev, GameObject gameObject, Action action)
    {
        if (ev == MessageEnum.NONE)
        {
            Debug.Log("Remove Listener is not setup!");
            return;
        }
        if (messages.ContainsKey(ev))
        {
            if (messages[ev].ContainsKey(gameObject))
            {
                
                messages[ev][gameObject] -= action;
                if (messages[ev][gameObject] == null)
                {
                    messages[ev].Remove(gameObject);
                }
            }
            
        }

        
    }

}

public static class Message<T>
{
    private static Dictionary<MessageEnum, Dictionary<GameObject, Action<T>>> messages = new Dictionary<MessageEnum, Dictionary<GameObject, Action<T>>>();

    public static void SendMessage(MessageEnum ev, T arg, params GameObject[] objs)
    {
        if (ev == MessageEnum.NONE)
        {
            Debug.Log("Message sender is not setup!");
            return;
        }
        if (messages.ContainsKey(ev))
        {
            if (objs.Length > 0)
            {
                for (int i = objs.Length - 1; i >= 0; i--)
                {
                    if (messages[ev].ContainsKey(objs[i]))
                    {
                        messages[ev][objs[i]].Invoke(arg);
                    }
                }
            }
            else
            {

                List<KeyValuePair<GameObject, Action<T>>> values = messages[ev].ToList();
                for (int i = values.Count - 1; i >= 0; i--)
                {
                    values[i].Value.Invoke(arg);
                }
                //foreach (KeyValuePair<GameObject, Action<T>> kv in messages[ev])
                //{
                //    kv.Value.Invoke(arg);
                //}

            }
        }
        else
        {
            Debug.Log("No LIsterners");
        }
    }
    public static void AddListener(MessageEnum ev, GameObject gameObject, Action<T> action)
    {

        if (ev == MessageEnum.NONE)
        {
            Debug.Log("Listener is not setup!");
            return;
        }
        if (!messages.ContainsKey(ev))
        {
            messages.Add(ev, new Dictionary<GameObject, Action<T>>());
        }
        if (!messages[ev].ContainsKey(gameObject))
        {
            messages[ev].Add(gameObject, null);
        }
        messages[ev][gameObject] += action;

    }
    public static void RemoveListener(MessageEnum ev, GameObject gameObject, Action<T> action)
    {
        if (ev == MessageEnum.NONE)
        {
            Debug.Log("Remove Listener is not setup!");
            return;
        }
        if (messages.ContainsKey(ev))
        {
            if (messages[ev].ContainsKey(gameObject))
            {
                messages[ev][gameObject] -= action;
                if (messages[ev][gameObject] == null)
                {
                    messages[ev].Remove(gameObject);
                }
            }
            
        }

        
    }
}
