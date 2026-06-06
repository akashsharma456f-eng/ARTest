using System;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public static EventHandler Instance { get; private set; }
    Dictionary<GlobleEventEnum, List<Action>> keyValuePairs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void Subscribe(GlobleEventEnum globleEventEnum,Action action)
    {
        if (keyValuePairs == null)
        {
            keyValuePairs = new Dictionary<GlobleEventEnum, List<Action>>();
        }
        if (!keyValuePairs.ContainsKey(globleEventEnum))
        {
            keyValuePairs[globleEventEnum]=new List<Action>();
        }
        if (!keyValuePairs[globleEventEnum].Contains(action))
        {
            keyValuePairs[globleEventEnum].Add(action);
        }
    }

    public void UnSubscribe(GlobleEventEnum globleEventEnum, Action action)
    {
        if(keyValuePairs!=null && keyValuePairs.ContainsKey(globleEventEnum) && keyValuePairs[globleEventEnum].Contains(action))
        {
            keyValuePairs[globleEventEnum].Remove(action);
        }
    }

    public void Notify(GlobleEventEnum eventEnum) 
    {
        for (int i = 0; i < keyValuePairs[eventEnum].Count; i++) 
        {
            keyValuePairs[eventEnum][i]?.Invoke();
        }
    }

}

public enum GlobleEventEnum
{
    Show,
    Hide
}
