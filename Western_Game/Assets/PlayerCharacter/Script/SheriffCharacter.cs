using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;

public class SheriffCharacter : MonoBehaviour
{

    public UnityEvent<SheriffCharacter> OnInteract = new UnityEvent<SheriffCharacter>();



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
    private GameObject _interactionUI;
    [SerializeField]
    private TextMeshProUGUI _dynamiteCountText;

    [Space(20)]
    [SerializeField]
    private Transform _projectileSpawner;
    [SerializeField]
    private ProjectileBehavior _projectilePrefab;

    [Space(20)]
    [SerializeField]
    private ExplosiveObject _dynamitePrefab;
    [SerializeField]
    private Transform _explosiveSpawner;

    [Header("Parameters")]
    public int maxAmmo;

    public float fireRate;

    public float interactionDistance;
    public LayerMask interactionLayerMask;

    public int DynamiteCount = 0;



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
        _playerInputs.Player.SpawnDynamite.started += SpanDynamite;


        SetPowerActive(false);
        _canShoot = true;
        _chargerAmmo = maxAmmo;
        _dynamiteCountText.text = DynamiteCount.ToString();
    }

    private CollectableObject currentCollectable;

    [System.Obsolete]
    private void FixedUpdate()
    {
        RaycastHit hit;
        CollectableObject collectableObject;

        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, interactionDistance, interactionLayerMask))
        {
            if(hit.collider.gameObject.TryGetComponent<CollectableObject>(out collectableObject))
            {

                if (currentCollectable != collectableObject || currentCollectable == null)
                {
                    if(currentCollectable != null)
                    {
                        OnInteract.RemoveListener(currentCollectable.Collect);
                    }
                    

                    currentCollectable = collectableObject;

                    OnInteract.AddListener(collectableObject.Collect);

                   
                }

                if (!_interactionUI.active)
                {
                    _interactionUI.SetActive(true);
                }
                
            }
            else
            {
                OnInteract.RemoveAllListeners();
                currentCollectable = null;

                if (_interactionUI.active)
                {
                    _interactionUI.SetActive(false);
                }
            }
        }
        else
        {
            OnInteract.RemoveAllListeners();
            currentCollectable = null;

            if (_interactionUI.active)
            {
                _interactionUI.SetActive(false);
            }
        }
    }

    public void Interact(InputAction.CallbackContext obj)
    {
        OnInteract.Invoke(this);
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

    public void AddDynamite()
    {
        DynamiteCount++;
        _dynamiteCountText.text = DynamiteCount.ToString();
    }

    public void SpanDynamite(InputAction.CallbackContext obj)
    {
        if(DynamiteCount > 0)
        {
            ExplosiveObject newDynamite = Instantiate(_dynamitePrefab);

            newDynamite.transform.position = _explosiveSpawner.transform.position;
            newDynamite.GetComponent<Rigidbody>().AddForce(new Vector3(0, 100f, 0) + _camera.transform.forward * 500f);


            DynamiteCount--;
            _dynamiteCountText.text = DynamiteCount.ToString();
        }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (_camera != null)
        {
            Gizmos.DrawWireSphere(_camera.transform.position + _camera.transform.forward * interactionDistance, 0.25f);
        }
       

        
    }

}
