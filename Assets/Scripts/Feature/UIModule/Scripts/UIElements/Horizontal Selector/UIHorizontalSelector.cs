using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHorizontalSelector : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [Header("Display")]
    [SerializeField] private TextMeshProUGUI titleText;
    [Header("Indicators")]
    [SerializeField] private Transform indicatorContainer;
    [SerializeField] private UISelectorIndicator indicatorPrefab;
    [Header("Data")]
    [SerializeField] private List<string> options = new();
    
    public Action<int> onSelectionChanged;

    private int _currentIndex;

    private List<UISelectorIndicator> _indicators = new();

    private void Awake()
    {
        prevButton.onClick.AddListener(OnPrev);
        nextButton.onClick.AddListener(OnNext);
    }

    private void Start()
    {
        for (int i = 0; i < options.Count; i++)
        {
            UISelectorIndicator indicator = Instantiate(indicatorPrefab, indicatorContainer);
            _indicators.Add(indicator);
        }
        SetIndex(0, false);
    }

    private void OnPrev() => SetIndex(_currentIndex - 1, true);
    private void OnNext() => SetIndex(_currentIndex + 1, true);
    
    private void SetIndex(int newIndex, bool notify)
    {
        if (options.Count == 0) return;
        
        if (newIndex < 0) newIndex = options.Count - 1;
        if (newIndex >= options.Count) newIndex = 0;

        _currentIndex = newIndex;
        RefreshVisuals();

        if (notify)
            onSelectionChanged?.Invoke(_currentIndex);
    }
    
    private void RefreshVisuals()
    {
        titleText.text = options[_currentIndex];
        _indicators.ForEach(indicator => indicator.SetSelected(false));
        _indicators[_currentIndex].SetSelected(true);
    }
}
