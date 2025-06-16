using UnityEngine;
using UnityEngine.UI;

public class UISelectorIndicator : MonoBehaviour
{
    [SerializeField] private Image selectedImage;
    [SerializeField] private Image unselectedImage;
    
    public void SetSelected(bool isSelected)
    {
        if (selectedImage != null)
            selectedImage.gameObject.SetActive(isSelected);
        if (unselectedImage != null)
            unselectedImage.gameObject.SetActive(!isSelected);
    }
}
