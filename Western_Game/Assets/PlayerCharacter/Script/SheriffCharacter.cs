using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class SheriffCharacter : MonoBehaviour
{

    public UnityEvent OnInteract = new UnityEvent();

    public PlayerInputs _playerInputs;
    [System.NonSerialized]
    public CameraShake _cameraShake;

    private CharacterController _controller;

    private Camera _camera;

    [Header("References")]
    [SerializeField]
    private Animator _gunAnimator;
    [SerializeField]
    private LassoBehvior _lasso;


    [Space(20)]
    [SerializeField]
    private Transform _projectileSpawner;
    [SerializeField]
    private ProjectileBehavior _projectilePrefab;

    [Header("Parameters")]
    public int maxAmmo;

    public float fireRate;





    //Input variables

    private int _chargerAmmo;
    private bool _canShoot;

    private bool canUsePower;


    private void Awake()
    {
        _playerInputs = new PlayerInputs();
        _cameraShake = Camera.main.GetComponent<CameraShake>();
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main;

    }

    private void OnEnable()
    {
        _playerInputs.Enable();
    }

    private void OnDisable()
    {
        _playerInputs.Disable();
    }

    private void Start()
    {
        _playerInputs.Player.Fire.started += Fire;
        _playerInputs.Player.Reload.started += Reload;
        _playerInputs.Player.LassoLaunch.started += LaunchLasso;
        _playerInputs.Player.SwitchPower.started += SwitchPower;
        _playerInputs.Player.VoodooPower.started += VoodooPower;
        _playerInputs.Player.Interact.started += Interact;


        SetPowerActive(false);
        _canShoot = true;
        _chargerAmmo = maxAmmo;
    }

    public void Interact(InputAction.CallbackContext obj)
    {
        OnInteract.Invoke();
    }

    public void Fire(InputAction.CallbackContext obj)
    {
        if (_canShoot & _chargerAmmo > 0)
        {
            _gunAnimator.SetTrigger("Fire");
            _canShoot = false;

            _chargerAmmo--;

            //Spawn orijectile

            ProjectileBehavior newProjectile = Instantiate(_projectilePrefab, _projectileSpawner);
            newProjectile.transform.parent = null;

            RaycastHit hit;

            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 1000f))
            {
                Vector3 direction = hit.point - _projectileSpawner.transform.position;

                newProjectile.transform.rotation = Quaternion.LookRotation(direction);
            }

            //Play effects
            StartCoroutine(_cameraShake.Shake(.2f, .5f));

            StartCoroutine(LockShootDelay(fireRate));
        }

    }


    public void Reload(InputAction.CallbackContext obj)
    {
        if (_chargerAmmo < maxAmmo)
        {
            _canShoot = false;
            _gunAnimator.SetTrigger("Reload");

            _chargerAmmo = maxAmmo;

            StartCoroutine(LockShootDelay(.46f));
        }

    }

    public void LaunchLasso(InputAction.CallbackContext obj)
    {

        RaycastHit hit;
        GrabbableObject grabbedObject;

        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 200f))
        {
            if (hit.collider.gameObject.TryGetComponent<GrabbableObject>(out grabbedObject))
            {
                _lasso.OnUngrabObject.AddListener(grabbedObject.OnUngrab);

                grabbedObject.OnGrab();

                _lasso.GrabObject(hit);

            }
            else
            {
                _lasso.UngrabObject();
            }
        }
        else
        {
            _lasso.UngrabObject();
        }

    }

    public void SetPowerActive(bool isActive)
    {
        canUsePower = isActive;
    }

    public void SwitchPower(InputAction.CallbackContext obj)
    {
        if (canUsePower)
        {
            GetComponent<FirstPersonController>().enabled = false;



            Vector3 position = transform.position;



            transform.position = _lasso.grabbedObject.position;

            _lasso.grabbedObject.position = position;

            _lasso.UngrabObject();

            StartCoroutine(ReloadFirstPersonController());
        }
    }
    public void VoodooPower(InputAction.CallbackContext obj)
    {
    }


    private IEnumerator ReloadFirstPersonController()
    {
        yield return null;
        GetComponent<FirstPersonController>().enabled = true;
    }


    private IEnumerator LockShootDelay(float delay)
    {
        _canShoot = false;

        yield return new WaitForSeconds(fireRate);

        _canShoot = true;
    }


}
