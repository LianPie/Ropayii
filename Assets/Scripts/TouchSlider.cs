using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityAction OnPointerDownEvent;
    public UnityAction OnPointerUpEvent;

    public UnityAction<float> OnPointerDragEvent;

    private Slider uiSlider;

    private void Awake()
    {
        uiSlider = GetComponent<Slider>();
        uiSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (OnPointerDownEvent != null)
        {
            OnPointerDownEvent.Invoke();
        }
        if (OnPointerDragEvent != null)
        {
            OnPointerDragEvent.Invoke(uiSlider.value);
        }
        Debug.Log("touch started: " + gameObject.name);
    }
    private void OnSliderValueChanged(float value)
    {
        if (OnPointerDragEvent != null)
        {
            OnPointerDragEvent.Invoke(value);
        }
        Debug.Log("slider went weeee");
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerPress == gameObject)
        {
            if (OnPointerUpEvent != null)
            {
                OnPointerUpEvent.Invoke();
                uiSlider.value = 0f;
            }
            Debug.Log("touch Ended: " + gameObject.name);

        }
    }

    private void OnDestroy()
    {
        //remove listener to avoid memory leaks
        uiSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

}
