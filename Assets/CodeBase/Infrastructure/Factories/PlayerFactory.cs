using Cinemachine;
using CodeBase;
using UnityEngine;

public class PlayerFactory
{
    private Vector3 _playerPosition;
    private Vector3 _playerDirection;
    private GameObject _playerPrefab;
    private GameObject _player;
    private InputController _inputController;
    private MoveController _moveController;

    public PlayerFactory(GameObject playerPrefab)
    {
        _playerPrefab = playerPrefab;
        _player = GameObject.Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);

        _inputController = _player.GetComponent<InputController>();
        _moveController = _player.GetComponent<MoveController>();
        _inputController.InputUpdated += OnPlayerInputUpdated;
    }

    private void OnPlayerInputUpdated(InputData obj)
    {
        Vector3 newDirection = new Vector3(obj.Horizontal, obj.Vertical, 0);

        if (newDirection.sqrMagnitude <= 0.3f)
        {
            return;
        }

        _playerDirection = newDirection;
    }

    public IAbilityHandler GetPlayerAbilityHandler()
    {
        return _player.GetComponentInChildren<IAbilityHandler>();
    }

    public Vector3 GetPlayerPosition()
    {
        if (_player)
            _playerPosition = _player.transform.position;

        return _playerPosition;
    }

    public Vector3 GetPlayerDirection() =>
        _moveController.LastMoveVector;

    public void BindCameraToPlayer(CinemachineVirtualCamera cinemachineVirtualCamera)
    {
        cinemachineVirtualCamera.Follow = _player.transform;
    }
}