using UnityEngine;

public class EquipmentUIController : MonoBehaviour
{
    public GameObject jugImage;
    public GameObject crookImage;
    public GameObject boltImage;

    public enum ItemType { None, Jug, Crook, Bolt }

    public void SetEquipmentDisplay(ItemType item)
    {
        jugImage.SetActive(false);
        crookImage.SetActive(false);
        boltImage.SetActive(false);

        switch (item)
        {
            case ItemType.Jug:
                jugImage.SetActive(true);
                break;
            case ItemType.Crook:
                crookImage.SetActive(true);
                break;
            case ItemType.Bolt:
                boltImage.SetActive(true);
                break;
            case ItemType.None:
            default:
                // All images are already off
                break;
        }
    }
}

