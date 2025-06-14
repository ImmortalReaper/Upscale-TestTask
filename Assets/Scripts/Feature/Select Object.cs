using UnityEngine;
using UnityEngine.EventSystems;

public class SelectObject : MonoBehaviour
{
    [SerializeField] private GameObject selectObject;

    void Awake()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectObject);
    }
}
