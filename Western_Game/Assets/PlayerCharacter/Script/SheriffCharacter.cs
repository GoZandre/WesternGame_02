using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SheriffCharacter : MonoBehaviour
{
    public PlayerInputs _playerInputs;
    [System.NonSerialized]
    public CameraShake _cameraShake;

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

    private void Awake()
    {
        _playerInputs = new PlayerInputs();
        _cameraShake = Camera.main.GetComponent<CameraShake>();
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

        _canShoot = true;
        _chargerAmmo = maxAmmo;
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

            //Play effects
            StartCoroutine(_cameraShake.Shake(.2f, .5f));

            StartCoroutine(LockShootDelay(fireRate));
        }

    }


    public void Reload(InputAction.CallbackContext obj)
    {
        if(_chargerAmmo < maxAmmo)
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
            if(hit.collider.gameObject.TryGetComponent<GrabbableObject>(out grabbedObject))
            {

                Debug.Log(hit.collider.gameObject);
                _lasso.GrabObject(hit);

            }
        }

    }



        private IEnumerator LockShootDelay(float delay)
    {
        _canShoot = false;

        yield return new WaitForSeconds(fireRate);

        _canShoot = true;
    }


}
