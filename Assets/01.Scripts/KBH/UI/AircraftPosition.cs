using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AircraftPosition : MonoBehaviour
{
    private EventTrigger _eventTrigger;
    public event Action MouseDownEvt;
    public event Action MouseUpEvt;
    public event Action MouseOverEvt;


    private void Awake()
    {
        _eventTrigger = GetComponent<EventTrigger>();

        {
            var pointerDownTrigger = new EventTrigger.Entry();

            pointerDownTrigger.callback.AddListener(HandlePointerDown);
            pointerDownTrigger.eventID = EventTriggerType.PointerDown;

            _eventTrigger.triggers.Add(pointerDownTrigger);
        }

        {
            var pointerUpTrigger = new EventTrigger.Entry();

            pointerUpTrigger.callback.AddListener(HandlePointerUp);
            pointerUpTrigger.eventID = EventTriggerType.PointerUp;

            _eventTrigger.triggers.Add(pointerUpTrigger);
        }

        {
            var pointerOverTrigger = new EventTrigger.Entry();

            pointerOverTrigger.callback.AddListener(HandlePointerEnter);
            pointerOverTrigger.eventID = EventTriggerType.PointerEnter;

            _eventTrigger.triggers.Add(pointerOverTrigger);
        }
    }


    private void HandlePointerDown(BaseEventData data)
    {
        MouseDownEvt?.Invoke();
    }

    private void HandlePointerUp(BaseEventData data)
    {
        MouseUpEvt?.Invoke();
    }

    private void HandlePointerEnter(BaseEventData data)
    {
        MouseOverEvt?.Invoke();
    }

}
