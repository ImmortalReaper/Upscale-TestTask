using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDropdownAutoScroll : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private ScrollRect scrollRect;
    
    private List<UIDropdownToggleStateController> toggleStateControllers = new();
    private RectTransform content;
    private Dictionary<UIDropdownToggleStateController, Action> actionMap = new();
    
    private void Start()
    {
        scrollRect = transform.GetComponent<ScrollRect>();
        if (scrollRect != null)
        {
            content = scrollRect.content;
        }
        
        UIDropdownToggleStateController[] toggles = GetComponentsInChildren<UIDropdownToggleStateController>();
        toggleStateControllers.AddRange(toggles.Where(toggle => toggle.gameObject.activeSelf));
        
        for(int i = 0; i < toggleStateControllers.Count; i++)
        {
            int index = i;
            var toggle = toggleStateControllers[i];
            
            Action scrollAction = () => StartCoroutine(ScrollToSelected(index));
            actionMap[toggle] = scrollAction;
            
            toggle.OnSelected += scrollAction;
        }
    }

    private void OnDestroy()
    {
        foreach (var kvp in actionMap)
        {
            if (kvp.Key != null)
            {
                kvp.Key.OnSelected -= kvp.Value;
            }
        }
        actionMap.Clear();
    }

    private IEnumerator ScrollToSelected(int selectedIndex)
    {
        yield return new WaitForEndOfFrame();
    
        if (scrollRect == null || content == null || selectedIndex >= toggleStateControllers.Count) 
            yield break;
    
        // Специальная обработка крайних элементов
        if (selectedIndex == 0)
        {
            scrollRect.verticalNormalizedPosition = 1f;
            yield break;
        }
    
        if (selectedIndex == toggleStateControllers.Count - 1)
        {
            scrollRect.verticalNormalizedPosition = 0f;
            yield break;
        }
    
        // Для элементов в середине
        RectTransform targetRect = toggleStateControllers[selectedIndex].transform as RectTransform;
        if (targetRect == null) yield break;
    
        float viewportHeight = scrollRect.viewport.rect.height;
        float contentHeight = content.rect.height;
        float scrollableHeight = contentHeight - viewportHeight;
    
        if (scrollableHeight <= 0) yield break;
    
        // Получаем центр элемента относительно content
        float targetY = Mathf.Abs(targetRect.anchoredPosition.y);
        float targetCenter = targetY + (targetRect.rect.height * 0.5f);
    
        // Позиционируем центр элемента в центр viewport
        float scrollPosition = targetCenter - (viewportHeight * 0.5f);
        float normalizedPosition = 1f - (scrollPosition / scrollableHeight);
    
        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(normalizedPosition);
    }
}
