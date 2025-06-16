using UnityEngine;
using UnityEngine.EventSystems;

public class SelectObject : MonoBehaviour
{
    [SerializeField] private GameObject selectObject;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectObject);
    }
}
