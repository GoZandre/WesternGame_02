using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyBehavior : MonoBehaviour
{

    [Header("Parameters")]

    private UnityEvent onMoveCharacter = new UnityEvent();

    public UnityEvent OnDie = new UnityEvent();

    public float maxHealth;
    private float health;

    [Space(10)]

    [SerializeField]
    private Transform[] _path;

    private int pathIndex;

    [Space(10)]

    public float minWaitOnPoint;
    public float maxWaitOnPoint;


    private NavMeshAgent _agent;



    [Header("References")]
    [SerializeField]
    private HealthBarBehavior _healthBarPrefab;

    private HealthBarBehavior _healthBar;

    [SerializeField]
    private Transform _healthBarTransform;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        pathIndex = 0;
    }

    private void Start()
    {
        health = maxHealth;

        MoveToNextPath();
        

    }

    private void Update()
    {
        onMoveCharacter.Invoke();
    }

    public void MoveToNextPath()
    {
        _agent.SetDestination(_path[pathIndex].position);
        onMoveCharacter.AddListener(OnMove);
    }

    public void OnMove()
    {
        if(_agent.enabled)
        {
            if (!_agent.pathPending && _agent.remainingDistance <= .5f)
            {
                OnDestinationReached();
            }

        }
        
    }

    private void OnDestinationReached()
    {
        onMoveCharacter.RemoveListener(OnMove);

        if(pathIndex < _path.Length - 1)
        {
            pathIndex++;
        }
        else
        {
            pathIndex = 0;
        }

       

        if(maxWaitOnPoint >= 0)
        {
            StartCoroutine(DelayUntileNextPath());
        }
        else
        {
            MoveToNextPath();
        }
    }


    public void CreateHealthBar()
    {
        Canvas canvasRenderer = GameManager.Instance.mainCanvas;

        _healthBar = Instantiate(_healthBarPrefab, canvasRenderer.transform);
        _healthBar.enemyTransform = _healthBarTransform;

        _healthBar.UpdateHealth(health / maxHealth);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            OnDie.Invoke();


            if (_healthBar != null)
            {
                Destroy(_healthBar.gameObject);
            }
            Destroy(gameObject);
        }

        if(_healthBar == null)
        {
            CreateHealthBar();
        }

        _healthBar.UpdateHealth(health / maxHealth);
    }

    public IEnumerator DelayUntileNextPath()
    {
        float timeToWait = Random.Range(minWaitOnPoint, maxWaitOnPoint);

        yield return new WaitForSeconds(timeToWait);

        MoveToNextPath();
    }
}
