using Cinemachine;
using CodeBase.Configs;
using CodeBase.Domain;
using FMODUnity;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class PlayerBuilder
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IPropertyProvider _propertyProvider;
        private readonly IModelProvider _modelProvider;
        private readonly IAbilityUpgradeService _abilityUpgradeService;
        private readonly AbilityFactory _abilityFactory;
        private readonly IAudioPlayerService _audioPlayerService;

        private Player _player;

        private Vector3 _playerPosition;
        private Vector3 _playerDirection;
        private GameObject _playerPrefab;
        private InputController _inputController;
        private MoveController _moveController;
        private CinemachineVirtualCamera _currentCamera;
        private Camera _playerCamera;

        public PlayerBuilder(
            IModelProvider modelProvider,
            IConfigurationProvider configurationProvider,
            IPropertyProvider propertyProvider,
            IAbilityUpgradeService abilityUpgradeService,
            AbilityFactory abilityFactory,
            IAudioPlayerService audioPlayerService)
        {
            _modelProvider = modelProvider;
            _configurationProvider = configurationProvider;
            _propertyProvider = propertyProvider;
            _abilityUpgradeService = abilityUpgradeService;
            _abilityFactory = abilityFactory;
            _audioPlayerService = audioPlayerService;
        }

        public Player Build(AbilityConfigSO initialAbilityConfigSO)
        {
            _player = GameObject.Instantiate(_modelProvider.Get<HeroModel>().CurrentSelectedHero.Prefab, Vector3.zero,
                Quaternion.identity);
            _inputController = _player.GetComponent<InputController>();
            _inputController.Initialize();
            _moveController = _player.GetComponent<MoveController>();
            _inputController.InputUpdated += OnPlayerInputUpdated;
            _player.Initialize(_propertyProvider, initialAbilityConfigSO, _abilityFactory,
                _audioPlayerService);
            _abilityUpgradeService.BindToPlayer(_player);
            return _player;
        }

        private void OnPlayerInputUpdated(InputData inputData)
        {
            Vector3 newDirection = new Vector3(inputData.Horizontal, inputData.Vertical, 0);

            if (newDirection.sqrMagnitude <= 0.3f)
                return;

            _playerDirection = newDirection;
        }

        public IAbilityHandler GetPlayerAbilityHandler() =>
            _player.GetComponentInChildren<IAbilityHandler>();

        public Vector3 GetPlayerPosition()
        {
            if (_player)
                _playerPosition = _player.transform.position;

            return _playerPosition;
        }

        public Vector3 GetPlayerDirection() =>
            _moveController.LastMoveVector;

        public void BindCameraToPlayer()
        {
            _currentCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
            _currentCamera.Follow = _player.transform;
            _playerCamera = GameObject.FindObjectOfType<Camera>();
            var listener = GameObject.FindObjectOfType<StudioListener>();
            listener.enabled = false;
            _player.gameObject.AddComponent<StudioListener>().BindAttenuation(_player.gameObject);
        }

        public void BindEventsHandler(PlayerEventHandler playerEventHandler)
        {
            IDamageable damagable = _player.GetComponent<IDamageable>();
            playerEventHandler.Initialize(damagable);
        }

        public void RespawnPlayer()
        {
            if (_player == null)
                return;

            _player.GetComponent<IDamageable>().Respawn();
        }

        public Camera GetPlayerCamera() => _playerCamera;
    }
}