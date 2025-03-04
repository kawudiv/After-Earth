using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public Image slot1Border;
    public Image slot2Border;

    private Color activeColor = Color.white;
    private Color inactiveColor = Color.black;

    public void UpdateWeaponUI(bool isMeleeEquipped, bool isRangedEquipped)
    {
        slot1Border.color = isMeleeEquipped ? activeColor : inactiveColor;
        slot2Border.color = isRangedEquipped ? activeColor : inactiveColor;
    }
}
