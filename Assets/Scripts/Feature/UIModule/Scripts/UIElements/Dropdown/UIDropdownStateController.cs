using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public class UIDropdownStateController : MonoBehaviour
{
    [Header("CanvasGroups")]
    [SerializeField] private CanvasGroup normalGroup;
    [SerializeField] private CanvasGroup disabledGroup;
    
    public Action<int> onDropdownValueChanged;

    private TMP_Dropdown _dropdown;

    private void Awake()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
        _dropdown.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnEnable()
    {
        UpdateState();
    }

    private void OnDisable()
    {
        _dropdown.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(int index)
    {
        onDropdownValueChanged?.Invoke(index);
    }

    public void SetInteractable(bool interactable)
    {
        _dropdown.interactable = interactable;
        UpdateState();
    }

    private void UpdateState()
    {
        if (_dropdown.interactable)
            SetCanvasGroup(normalGroup);
        else
            SetCanvasGroup(disabledGroup);
    }

    private void SetCanvasGroup(CanvasGroup active)
    {
        foreach (var group in new[] { normalGroup, disabledGroup })
        {
            bool isActive = group == active;
            group.alpha = isActive ? 1f : 0f;
            group.blocksRaycasts = isActive;
            group.interactable = isActive;
        }
    }
}
