//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerModel : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _forceJump;

    [SerializeField]
    private float _stamina;

    [SerializeField]
    private float _maxStamina;

    [SerializeField]
    private float _shootTime;

    [SerializeField]
    private float _maxShootTime;

    private bool _aiming = false;
    private bool _shooting = false;

    private bool _falling;

    private bool _onSprint = false;

    [SerializeField]
    private float _shootCoolDown;

    [SerializeField]
    private float _timeOnCoolDown;


    private PlayerController _playerController;

    private PlayerView _playerView;

    private GenericTimer _shootTimer;


    [SerializeField]
    private Animator _myAnimator;

    private Rigidbody _myRigidBody;

    [SerializeField]
    private Transform _shootPoint;

    [SerializeField]
    private GameObject _shootBar;

    void Awake()
    {
        _playerController = new PlayerController(this);

        _playerView = new PlayerView(_myAnimator, this);

        _shootTimer = new GenericTimer(_shootCoolDown);

        _myRigidBody = GetComponent<Rigidbody>();

        _stamina = _maxStamina;

        _maxShootTime = _shootTime;
    }

    private void FixedUpdate()
    {
        _playerController.ArtificialFixedUpdate();
    }

    void Update()
    {
        _playerController.ArtificialUpdate();

        Hud.instance.UpdateStaminaBar(_stamina, _maxStamina);
        Hud.instance.UpdateShootBar(_shootTime, _maxShootTime);

        if (!_onSprint)
        {
            _stamina += 15f * Time.deltaTime;

            if(_stamina > _maxStamina)
                _stamina = _maxStamina;
            CameraController.instance.SetSprintCamera(_onSprint);
        }

        if (!_shooting)
        {
            _shootTime += 15f * Time.deltaTime;

            if(_shootTime > _maxShootTime)
                _shootTime = _maxShootTime;     
        }

        if (!inFloor)
        {
            _falling = true;
        }
        else
        {
            _falling = false;
        }

        _playerView.Falling(_falling);
    }

    #region MOVEMENT
    public void MovePlayer(Vector3 dir, bool sprint)
    {
        float realSpeed;

        _onSprint = sprint;

        _playerView.Run(dir.x, dir.z, sprint, _aiming);

        if (!inFloor )
        {
            realSpeed = (_speed * 0.5f);
        }
        else if (_onSprint && dir.z > 0 && _stamina > 0)
        {
            realSpeed = (_speed * 2);
            dir.x = 0;

            if(dir.z < 0)
            {
                dir.z = 0;
            }  

            _stamina -= 45f * Time.deltaTime;
            CameraController.instance.SetSprintCamera(_onSprint);

            if (_stamina <= 0)
            {
                _stamina = 0;
            }
        }
        else
        {
            realSpeed = _speed;
        }

        if(_aiming == true)
        {
            realSpeed = 0;
        }

        Vector3 pos = transform.forward * dir.z;

        pos += transform.right * dir.x;
        pos *= realSpeed * Time.deltaTime;

        pos += transform.up * _myRigidBody.velocity.y;

        _myRigidBody.velocity = pos;
    }

    public void Jump()
    {
        if (inFloor && !_aiming)
        {
            _myRigidBody.AddForce(Vector3.up * _forceJump, ForceMode.Impulse);
            _playerView.Jump();
        }
    }

    public bool inFloor
    {
        get
        {
            return (Physics.Raycast(transform.position, Vector3.down, 0.1f));
        }
    }

    public Quaternion RotateWithMouse(float y)
    {
        transform.localRotation = Quaternion.Euler(0, transform.localRotation.y + y * 30f, 0);

        return transform.localRotation;
    }

    public void Aim(bool Aimning)
    {
        _aiming = Aimning;

        _playerView.SetAimAnim(_aiming);

        CameraController.instance.SetAimCamera(_aiming);

        _shootBar.SetActive(_aiming);
    }

    public void Shoot(bool onShoot)
    {

        _shooting = onShoot;

        if (_aiming && _shooting)
        {
            if (_shootTime > 0)
            {
                Debug.Log("Sonido de disparo");

                RaycastHit hit;
                var raycast = Physics.Raycast(_shootPoint.position,_shootPoint.forward, out hit, 1000);

                _shootTime -= 35f * Time.deltaTime;
            }
            else
            {
                Debug.Log("Sonido de no balas");
            }

        }
    }

    #endregion
}
