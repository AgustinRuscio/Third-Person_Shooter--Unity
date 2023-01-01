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
    private float realSpeed;

    [SerializeField]
    private float _forceJump;

    private float realJumpForce;

    [SerializeField]
    private float _stamina;

    [SerializeField]
    private float _staminaReduceForJump;

    [SerializeField]
    private float _maxStamina;

    [SerializeField]
    private float _shootTime;

    [SerializeField]
    private float _maxShootTime;

    [SerializeField]
    private float _shootCoolDown;

    [SerializeField]
    private float _timeOnCoolDown;

    private bool _aiming = false;
    private bool _shooting = false;
    private bool _falling;
    private bool _onSprint = false;
    private bool _isCrouching;

    

    private PlayerController _playerController;

    private PlayerView _playerView;



    [SerializeField]
    private Animator _myAnimator;

    [SerializeField]
    private CapsuleCollider _myCollider;

    private Vector3 _colliderOriginalPos;

    [SerializeField]
    private Vector3 _colliderCrouchPos;

    private float _originalColliderlHeight;

    [SerializeField]
    private float _crouchColliderlHeight;

    private Rigidbody _myRigidBody;

    [SerializeField]
    private Transform _shootPoint;

    [SerializeField]
    private GameObject _shootBar;

    void Awake()
    {
        _playerController = new PlayerController(this);

        _playerView = new PlayerView(_myAnimator, this);

        _myRigidBody = GetComponent<Rigidbody>();

        _colliderOriginalPos = _myCollider.center;

        _originalColliderlHeight = _myCollider.height;

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


        _playerView.SetCrouchAnim(_isCrouching);

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
            _falling = true;
        else
            _falling = false;

        _playerView.Falling(_falling);
    }

    #region MOVEMENT
    public void MovePlayer(Vector3 dir, bool sprint)
    {
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

            realJumpForce = _forceJump * 1.5f;

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
        else if (_isCrouching)
        {
            realSpeed = _speed * 0.6f;
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
        if (!_onSprint && _stamina >= _staminaReduceForJump)
        {
            realJumpForce = _forceJump;
        }

        if(_stamina < _staminaReduceForJump)
        {
            realJumpForce = _forceJump * 0.5f;
        }

        if (inFloor && !_aiming)
        {
            _myRigidBody.AddForce(Vector3.up * realJumpForce, ForceMode.Impulse);
            _stamina -= _staminaReduceForJump;
            _playerView.Jump();
        }
    }

    public bool inFloor
    {
        get
        {
            return (Physics.Raycast(transform.position, Vector3.down, 0.3f));
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

                _playerView.SetShootAnim(true);
            }
            else
                _playerView.SetShootAnim(false);
        }
        else
        {
            _playerView.SetShootAnim(false);
        }

        
    }

    public void Crouch(bool crouched)
    {
        _isCrouching = crouched;

        if (_isCrouching)
        {
            _myCollider.center = _colliderCrouchPos;
            _myCollider.height = _crouchColliderlHeight;
        }
        else
        {
            _myCollider.center = _colliderOriginalPos;
            _myCollider.height = _originalColliderlHeight;
        }
    }

    #endregion
}