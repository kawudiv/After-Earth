using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public Image slot1Border;
    public Image slot2Border;

    private Color activeColor = Color.white;
    private Color inactiveColor = Color.black;

    void Start()
    {
        UpdateWeaponUI(true);
    }

    public void UpdateWeaponUI(bool isMeleeEquipped)
    {
        slot1Border.color = isMeleeEquipped ? activeColor : inactiveColor;
        slot2Border.color = isMeleeEquipped ? inactiveColor : activeColor;
    }
}
