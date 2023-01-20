//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections.Generic;

public enum ManagerKeys
{
    LifeEvent,
    ExplosionEvent,
    GranadeNumber,
    GranadeAdded,
    LifeAdded,
    Death,
    ResetScene,
    PauseGame,
    ResumeGame,
}

public static class EventManager
{
    public static Dictionary<ManagerKeys, EventMethod> EventContainer = new Dictionary<ManagerKeys, EventMethod>();

    public delegate void EventMethod(params object[] parameters);

    public static void Suscribe(ManagerKeys eventType, EventMethod method)
    {
        if (EventContainer.ContainsKey(eventType))
            EventContainer[eventType] += method;
        else
            EventContainer.Add(eventType, method);
    }

    public static void UnSuscribe(ManagerKeys eventType, EventMethod method)
    {
        if (EventContainer.ContainsKey(eventType))
        {
            EventContainer[eventType] -= method;

            if (EventContainer[eventType] == null)
                EventContainer.Remove(eventType);
        }
    }

    public static void Trigger(ManagerKeys eventType, params object[] parameters)
    {
        if (EventContainer.ContainsKey(eventType))
            EventContainer[eventType](parameters);
    }
}