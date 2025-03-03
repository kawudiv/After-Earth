using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public Image meleeWeaponIcon;
    public Image rangedWeaponIcon;
    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(1,1,1,0.5f);

    void Start()
    {
        UpdateWeaponUI(true);
    }

    public void UpdateWeaponUI(bool isMeleeEquipped)
    {
        if(isMeleeEquipped)
        {
            meleeWeaponIcon.color = activeColor;
            rangedWeaponIcon.color = inactiveColor;
        }
        else
        {
            meleeWeaponIcon.color = inactiveColor;
            rangedWeaponIcon.color = activeColor;
        }
    }
}