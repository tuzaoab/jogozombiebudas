using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSwitcher : MonoBehaviour
{
    [Header("Weapons")]
    public GameObject pistol;
    public GameObject shotgun;

    [Header("UI")]
    public Image weaponIndicator;     // O quadrado no canto da tela
    public Sprite pistolSprite;       // Sprite para pistola
    public Sprite shotgunSprite;      // Sprite para escopeta

    private GameObject activeWeapon;

    void Start()
    {
        activeWeapon = pistol;

        pistol.SetActive(true);
        shotgun.SetActive(false);

        weaponIndicator.sprite = pistolSprite;

        UpdateAmmoUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(pistol);
            weaponIndicator.sprite = pistolSprite;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(shotgun);
            weaponIndicator.sprite = shotgunSprite;
        }
    }

    void SwitchWeapon(GameObject newWeapon)
    {
        if (activeWeapon == newWeapon)
            return;

        activeWeapon.SetActive(false);
        newWeapon.SetActive(true);
        activeWeapon = newWeapon;

        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        // Verifica pistola
        Gun gun = activeWeapon.GetComponent<Gun>();
        if (gun != null)
        {
            gun.UpdateAmmoUI();
            return;
        }

        // Verifica escopeta
        Escopeta escopeta = activeWeapon.GetComponent<Escopeta>();
        if (escopeta != null)
        {
            escopeta.UpdateAmmoUI();
            return;
        }
    }
}
