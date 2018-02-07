using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
/// <summary>
/// 给控件注册事件
/// 可以把物体本身注册到UIManager
/// </summary>
public class UIBehavior : MonoBehaviour {

    private void Awake()
    {
        UIManager.Instance.RegisterGo(gameObject.name, gameObject);
    }
    public void AddButtonListener(UnityAction action)
    {
        if (action != null)
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(action);
        }
    }
    public void RemoveButtonListener(UnityAction action)
    {
        if (action != null)
        {
            GetComponent<Button>().onClick.RemoveListener(action);
        }
    }
    public void AddToggleListener(UnityAction<bool> action)
    {
        if (action != null)
        {
            GetComponent<Toggle>().onValueChanged.AddListener(action);
        }
    }
    public void RemoveToggleListener(UnityAction<bool> action)
    {
        if (action != null)
        {
            GetComponent<Toggle>().onValueChanged.RemoveListener(action);
        }
    }
    public void AddSliderListener(UnityAction<float> action)
    {
        if (action != null)
        {
            GetComponent<Slider>().onValueChanged.AddListener(action);
        }
    }
    public void RemoveSliderListener(UnityAction<float> action)
    {
        if (action != null)
        {
            GetComponent<Slider>().onValueChanged.AddListener(action);
        }
    }

    public void AddInputListener(UnityAction<string> action)
    {
        if (action != null)
        {
            GetComponent<InputField>().onValueChanged.AddListener(action);
        }
    }
    public void RemoveInputListener(UnityAction<string> action)
    {
        if (action != null)
        {
            GetComponent<InputField>().onValueChanged.RemoveListener(action);
        }
    }
}
