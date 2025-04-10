using UnityEngine;
using UnityEngine.UI;

public class EquipmentUIController : MonoBehaviour
{
    [SerializeField] private Image equipmentIcon;

    [SerializeField] private Sprite defaultIcon;
    [SerializeField] private Sprite crookIcon;
    [SerializeField] private Sprite jugIcon;
    [SerializeField] private Sprite boltIcon;

    public void SetEquipmentIcon(string iconName)
    {
        switch (iconName)
        {
            case "Crook":
                equipmentIcon.sprite = crookIcon;
                break;
            case "Jug":
                equipmentIcon.sprite = jugIcon;
                break;
            case "Lightning":
                equipmentIcon.sprite = boltIcon;
                break;
            default:
                equipmentIcon.sprite = defaultIcon;
                break;
        }
    }
}