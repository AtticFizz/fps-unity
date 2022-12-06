using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(UIController))]
public class WeaponController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Min(1f)] private float aimDistance = 100f;

    [Header("Prefabs")]
    [SerializeField] private GameObject selectedWeapon;
    [SerializeField] private List<GameObject> weapons;

    [Header("Prefab Parts")]
    [SerializeField] private Transform handPoint;
    [SerializeField] private Transform cameraFPS;

    [Header("Inputs")]
    [SerializeField] private KeyCode keyNext = KeyCode.E;
    [SerializeField] private KeyCode keyPrevious = KeyCode.Q;

    [Header("Events")]
    [SerializeField] private UnityEvent onAimStart;
    [SerializeField] private UnityEvent onAimStop;

    private readonly int Left = 0;
    private readonly int Right = 1;
    // private readonly int Middle = 2;

    private GameObject weapon;
    private Transform sightsPoint;
    private Transform holdPoint;

    private GunController weaponController;
    private UIController uiController;

    private bool aiming;
    private int selectedWeaponIndex;

    private void Start()
    {
        uiController = GetComponent<UIController>();

        selectedWeaponIndex = weapons.FindIndex(w => w == selectedWeapon);
        aiming = false;

        CreateWeaponInHand();
    }

    private void Update()
    {
        UpdateAimInput();
        UpdateWeaponAim();
        UpdateShootInput();
        UpdateSelectNextWeaponInput();
        UpdateSelectPreviousWeaponInput();
    }

    private void UpdateWeaponAim()
    {
        if (!aiming)
        {
            weapon.transform.position = handPoint.transform.position;
            Vector3 toHandle = handPoint.transform.position - holdPoint.transform.position;
            weapon.transform.position += toHandle;
            Vector3 aimPoint = cameraFPS.transform.position + aimDistance * cameraFPS.transform.forward;
            transform.LookAt(aimPoint);
        }
        else
        {
            weapon.transform.position = cameraFPS.transform.position;
            weapon.transform.rotation = cameraFPS.transform.rotation;
            Vector3 toSights = cameraFPS.transform.position - sightsPoint.transform.position;
            weapon.transform.position += toSights;
        }
    }

    private void UpdateAimInput()
    {
        if (Input.GetMouseButton(Right))
        {
            aiming = true;
            onAimStart.Invoke();
        }
        else
        {
            aiming = false;
            onAimStop.Invoke();
        }
    }

    private void UpdateShootInput()
    {
        int currentFill = weaponController.MagazineCurrentFill;
        int maxFill = weaponController.MagazineCapacity;

        if (Input.GetMouseButtonDown(Left))
        {
            if (currentFill > 0)
            {
                weaponController.Shoot();
                currentFill = weaponController.MagazineCurrentFill;
                uiController.SetAmmoAmount(currentFill, maxFill);
            }
        }
    }


    private void CreateWeaponInHand()
    {
        weapon = Instantiate(selectedWeapon, transform);
        weaponController = weapon.GetComponent<GunController>();
        sightsPoint = weaponController.sightsPoint;
        holdPoint = weaponController.holdPoint;

        int currentFill = weaponController.MagazineCurrentFill;
        int maxFill = weaponController.MagazineCapacity;
        uiController.SetWeaponName(selectedWeapon.name);
        uiController.SetAmmoAmount(currentFill, maxFill);
    }

    private void UpdateSelectNextWeaponInput()
    {
        if (Input.GetKeyDown(keyNext))
        {
            SelectNextWeapon();
        }
    }

    private void SelectNextWeapon()
    {
        Destroy(weapon);
        selectedWeaponIndex++;
        selectedWeaponIndex %= weapons.Count;
        selectedWeapon = weapons[selectedWeaponIndex];
        CreateWeaponInHand();
    }

    private void UpdateSelectPreviousWeaponInput()
    {
        if (Input.GetKeyDown(keyPrevious))
        {
            SelectPreviousWeapon();
        }
    }

    private void SelectPreviousWeapon()
    {
        Destroy(weapon);
        selectedWeaponIndex--;
        selectedWeaponIndex = selectedWeaponIndex < 0 ? weapons.Count - 1 : selectedWeaponIndex;
        selectedWeapon = weapons[selectedWeaponIndex];
        CreateWeaponInHand();
    }
}
