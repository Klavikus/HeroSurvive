using System.Collections.Generic;
using Cinemachine;
using CodeBase;
using CodeBase.Abilities;
using CodeBase.Factories;
using CodeBase.Stats;
using UnityEngine;

public class CompositionRoot : MonoBehaviour
{
    [SerializeField] private AbilityConfigSO[] _abilityConfigSo;
    [SerializeField] private PlayerStatsSO _playerStatsSO;
    [SerializeField] private PlayerStatsSO _additionalStatsSO;

    [SerializeField] private CoroutineRunner _coroutineRunner;
    [SerializeField] private Transform _poolContainer;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private IAbilityHandler _abilityHandler;
    private AbilityProjectionFactory _abilityProjectionFactory;
    private AbilityFactory _abilityFactory;
    private TargetService _targetService;
    private PlayerFactory _playerFactory;
    private EnemyFactory _enemyFactory;
    private EnemySpawnService _enemySpawnService;
    private UpgradeService _upgradeService;

    private PlayerStatsHandler _playerStatsHandler;

    public void UpdateStats()
    {
        _playerStatsHandler.UpgradeStats(_additionalStatsSO.GetStats());
        _abilityHandler.UpdateAbilityData(_playerStatsHandler.GetStats());
    }

    private void Start()
    {
        _playerFactory = new PlayerFactory(_playerPrefab);
        _enemyFactory = new EnemyFactory(_enemyPrefab);
        _abilityHandler = _playerFactory.GetPlayerAbilityHandler();
        _targetService = new TargetService(_playerFactory, _enemyFactory);
        _enemySpawnService = new EnemySpawnService(_targetService, _enemyFactory);
        _abilityProjectionFactory = new AbilityProjectionFactory(_targetService, _poolContainer);
        _abilityFactory = new AbilityFactory(_abilityProjectionFactory, _coroutineRunner, _targetService);

        _playerStatsHandler = new PlayerStatsHandler(_playerStatsSO.GetStats());

        // _upgradeService = new UpgradeService(playerStats);

        _abilityHandler.Initialize();

        foreach (AbilityConfigSO abilityConfigSo in _abilityConfigSo)
        {
            AbilityData abilityData = new AbilityData(abilityConfigSo);
            Ability ability = _abilityFactory.Create(abilityData, _abilityProjectionFactory.CreateProjectionPool(abilityData));
            _abilityHandler.AddAbility(ability);
        }

        _enemySpawner.Initialize(_enemySpawnService);
        _playerFactory.BindCameraToPlayer(_cinemachineVirtualCamera);
    }
}

public class EnemySpawnService
{
    private TargetService _targetService;
    private EnemyFactory _enemyFactory;

    public EnemySpawnService(TargetService targetService, EnemyFactory enemyFactory)
    {
        _targetService = targetService;
        _enemyFactory = enemyFactory;
    }

    public void SpawnWaveInCircle(int amount, float radius)
    {
        float step = 360f / amount;
        Vector3 _targetPoint = _targetService.GetPlayerPosition();

        for (int i = 0; i < amount; i++)
        {
            GameObject enemy =
                _enemyFactory.Create(at: _targetPoint + Quaternion.Euler(0, 0, step * i) * Vector3.up * radius);
            enemy.GetComponent<EnemyAI>().Initialize(_targetService);
        }
    }
}

public class EnemyFactory
{
    private GameObject _enemyPrefab;
    private List<EnemyAI> _enemies;

    public EnemyFactory(GameObject enemyPrefab)
    {
        _enemyPrefab = enemyPrefab;
        _enemies = new List<EnemyAI>();
    }

    public GameObject Create(Vector3 at)
    {
        GameObject result = GameObject.Instantiate(_enemyPrefab, at, Quaternion.identity);
        _enemies.Add(result.GetComponent<EnemyAI>());
        result.GetComponent<EnemyAI>().Die += OnEnemyDied;
        return result;
    }

    private void OnEnemyDied(EnemyAI enemyAI)
    {
        _enemies.Remove(enemyAI);
        enemyAI.Die -= OnEnemyDied;
    }

    public Vector3 GetClosestEnemy(Vector3 to)
    {
        if (_enemies.Count == 0)
        {
            return Vector3.zero;
        }

        float minDistance = Vector3.Distance(_enemies[0].transform.position, to);
        int targetEnemyId = 0;

        for (var i = 0; i < _enemies.Count; i++)
        {
            var newDistance = Vector3.Distance(_enemies[i].transform.position, to);
            if (newDistance < minDistance)
            {
                minDistance = newDistance;
                targetEnemyId = i;
            }
        }

        return _enemies[targetEnemyId].transform.position;
    }
}

public class TargetService
{
    private readonly PlayerFactory _playerFactory;
    private readonly EnemyFactory _enemyFactory;

    public TargetService(PlayerFactory playerFactory, EnemyFactory enemyFactory)
    {
        _playerFactory = playerFactory;
        _enemyFactory = enemyFactory;
    }

    public Vector3 GetPlayerPosition() =>
        _playerFactory.GetPlayerPosition();

    public Vector3 GetPlayerDirection() =>
        _playerFactory.GetPlayerDirection();

    public Vector3 GetClosestEnemyToPlayer() =>
        _enemyFactory.GetClosestEnemy(_playerFactory.GetPlayerPosition());
}