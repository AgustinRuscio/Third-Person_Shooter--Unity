//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerModel : Entity, IDamageable
{
    [SerializeField]
    private float _speed;

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
    private float _shootDistance;

    [SerializeField]
    private float _timeOnCoolDown;

    [SerializeField]
    private float _granadeRange;

    [SerializeField]
    private int _granadeAvalible;

    private int _maxGranadeAvalible = 3;

    private float _startofFall;

    [SerializeField]
    private float _minFallDistance;

    private bool _aiming = false;
    private bool _shooting = false;
    private bool _falling;
    private bool _onSprint = false;
    private bool _isCrouching;
    private bool _lauchGranade;
    private bool _wasGrounded;
    private bool _wasFalling;
    private bool _isDeadly;
    private bool _death;

    private PlayerController _playerController;

    private PlayerView _playerView;

    private GenericTimer _shootHolesTimer;
    private GenericTimer _shootSoundTimer;

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

    public LayerMask shootableMasks;

    [SerializeField]
    private GameObject _laser;

    [SerializeField]
    private Transform _shootPoint;

    [SerializeField]
    private GameObject _shootParticles;

    [SerializeField]
    private GameObject _shootBar;

    [SerializeField]
    private AudioSource _shootSoundData;

    [SerializeField]
    private AudioSource _notAmmoSoundData;

    [SerializeField]
    private Transform _lauchGranadePoint;

    [SerializeField]
    private GameObject _granade;

    [SerializeField]
    private SoundData _soundThrowGranade;

    [SerializeField]
    private SoundData _jumpSound;

    [SerializeField]
    private SoundData _exhaustSound;

    [SerializeField]
    private SoundData _hurtSound;

    [SerializeField]
    private SoundData _deathSound;

    [SerializeField]
    private GameObject _heartBeatSound;

    [SerializeField]
    private ParticleSystem _bloodParticles;

    [SerializeField]
    private ParticleSystem _deathParticles;

    protected override void Awake()
    {
        base.Awake();
        _playerController = new PlayerController(this);

        _playerView = new PlayerView(_myAnimator, this);

        _myRigidBody = GetComponent<Rigidbody>();

        _shootHolesTimer = new GenericTimer(0.1f);
        _shootSoundTimer = new GenericTimer(0.15f);

        _colliderOriginalPos = _myCollider.center;

        _originalColliderlHeight = _myCollider.height;

        _stamina = _maxStamina;

        _maxShootTime = _shootTime;

        Hud.instance.UpdateHealthBar(life, maxLife, _isDeadly);

        Hud.instance.UpdateGranadeImages(_granadeAvalible);

        EventManager.Suscribe(ManagerKeys.GranadeAdded, AddGranade);
        EventManager.Suscribe(ManagerKeys.LifeAdded, AddLife);
    }

    private void FixedUpdate()
    {
        _playerController.ArtificialFixedUpdate();

        #region fallDamage
        bool g = inFloor;

        if (!_wasFalling && _falling)
            _startofFall = transform.position.y;
        

        if (!_wasGrounded && g)   
            FallDamage(_startofFall, 25);
        

        _wasGrounded = g;
        _wasFalling = _falling;
        #endregion
  
    }

    void Update()
    {
        _playerController.ArtificialUpdate();

        Hud.instance.UpdateStaminaBar(_stamina, _maxStamina);
        Hud.instance.UpdateShootBar(_shootTime, _maxShootTime); 

        _shootSoundTimer.RunTimer();
        _playerView.SetCrouchAnim(_isCrouching);

        UnityEngine.Debug.Log(inFloor);

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

    private void OnTriggerEnter(Collider other)
    {
        var item = other.gameObject.GetComponent<IItem>();

        if (item != null)
            item.OnGrab();
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

        if(_stamina < 25)
        {
            AudioManager.instance.AudioPlay(_exhaustSound, transform.position);
        }

        if(_aiming || _lauchGranade || _death)
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

        if (inFloor && !_aiming && !_death)
        {
            _myRigidBody.AddForce(Vector3.up * realJumpForce, ForceMode.Impulse);
            _stamina -= _staminaReduceForJump;
            _playerView.Jump();
            AudioManager.instance.AudioPlay(_jumpSound, transform.position);
        }
    }

    public void Crouch(bool crouched)
    {
        if (!_death)
        {
            _isCrouching = crouched;

            CameraController.instance.SetCrouchCamera(_isCrouching);

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
        
    }

    public bool inFloor
    {
        get
        {
            return (Physics.Raycast(transform.position + new Vector3(0,0.2f,0), Vector3.down, 0.3f));
        }
    }

    public Quaternion RotateWithMouse(float y)
    {
        if (!_death)
        {
            transform.localRotation = Quaternion.Euler(0, transform.localRotation.y + y * 30f, 0);

            return transform.localRotation;
        }
        
        return new Quaternion(0,0,0,0);
    }

    public void Aim(bool Aimning)
    {
        if (!_death)
        {

            _aiming = Aimning;

            _playerView.SetAimAnim(_aiming);

            CameraController.instance.SetAimCamera(_aiming);

            _shootBar.SetActive(_aiming);

            _laser.SetActive(_aiming);
        }
    }

    public void Shoot(bool onShoot)
    {
        _shooting = onShoot;

        if (_aiming && _shooting && inFloor && !_death)
        {
            if (_shootTime > 0)
            {
                _shootHolesTimer.RunTimer();

                RaycastHit hit;

                var Raycast = Physics.Raycast(_shootPoint.position,_shootPoint.forward, out hit, _shootDistance, shootableMasks);

                if(Raycast)
                {
                    var hitable = hit.collider.gameObject.GetComponent<IHitable>();

                    if (hitable != null)
                    {
                        if (_shootHolesTimer.CheckCoolDown())
                        {
                            hitable.OnHit(hit.point);
                            _shootHolesTimer.ResetTimer();
                        }
                    }

                    var explosive = hit.collider.gameObject.GetComponent<IExplosive>();

                    if (explosive != null)
                    {
                        explosive.OnExplosion();
                    }
                }


                if (_shootSoundTimer.CheckCoolDown())
                {
                    _shootSoundData.Play();
                    _shootSoundTimer.ResetTimer();
                }

                _shootTime -= 35f * Time.deltaTime;

                _playerView.SetShootAnim(true);
                
                _shootParticles.SetActive(true);
            }
            else
            {
                _playerView.SetShootAnim(false);

                _shootSoundData.Stop();

                if (_shootSoundTimer.CheckCoolDown())
                {
                    _notAmmoSoundData.Play();
                    _shootSoundTimer.ResetTimer();
                }               

                _shootParticles.SetActive(false);
            }
        }
        else
        {
            _playerView.SetShootAnim(false);
            _shootSoundData.Stop();

            _notAmmoSoundData.Stop();

            _shootParticles.SetActive(false);
        }   
    }

    public void LaunchGranade()
    {
        if (_aiming && _granadeAvalible > 0 && !_death)
        {
            _playerView.Granade();
            _lauchGranade = true;
            AudioManager.instance.AudioPlay(_soundThrowGranade, transform.position);
        }
    }

    //The anim Event will execute the method
    public void ThoughGranade()
    {
        GameObject granadeInstance = Instantiate(_granade, _lauchGranadePoint.position, _lauchGranadePoint.rotation);
        granadeInstance.GetComponent<Rigidbody>().AddForce(_lauchGranadePoint.forward * _granadeRange, ForceMode.Impulse);
        _granadeAvalible--;

        EventManager.Trigger(ManagerKeys.GranadeNumber, _granadeAvalible);
    }

    public void SetLauchGranadeFalse() => _lauchGranade = false;
    
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 dir = Vector3.down * 0.3f;

        

        Gizmos.DrawRay(transform.position + new Vector3(0,0.2f,0), dir);
    }

    #region Interfaces

    public void TakeDamage(float damage)
    {
        life -= damage;

        EventManager.Trigger(ManagerKeys.LifeEvent, life, maxLife, _isDeadly);
        AudioManager.instance.AudioPlay(_hurtSound, transform.position);

        _playerView.GetHurt(true);
        _bloodParticles.Play();

        CheckLife();
    }

    public void TakeDamage(float damage, Vector3 dir)
    {
        life -= damage;
        _myRigidBody.AddForce(dir, ForceMode.Impulse);

        EventManager.Trigger(ManagerKeys.LifeEvent, life, maxLife, _isDeadly);
        AudioManager.instance.AudioPlay(_hurtSound, transform.position);

        _playerView.GetHurt(true);
        _bloodParticles.Play();

        CheckLife();
    }

    public void FallDamage(float distanceFall, float damage)
    {
        float fallDistance = distanceFall - transform.position.y;

        if(fallDistance > _minFallDistance)
        {
            life -= damage;

            EventManager.Trigger(ManagerKeys.LifeEvent, life, maxLife, _isDeadly);
            AudioManager.instance.AudioPlay(_hurtSound, transform.position);

            _playerView.GetHurt(true);
            _bloodParticles.Play();


            CheckLife();
        }
        else
        {
            UnityEngine.Debug.Log("not enought");
        }

        
    }

    protected override void CheckLife()
    {
        if(life <= 0)
        {
            _death = true;

            _playerView.GetHurt(false);

            AudioManager.instance.AudioPlay(_deathSound, transform.position);
            _playerView.Death();
            _deathParticles.Play();

            EventManager.Trigger(ManagerKeys.Death);
        }

        if(life < (maxLife * 0.25))
        {
            DeadlyStatus();
        }
    }
    #endregion

    private void DeadlyStatus()
    {
        if (!_death)
        {
            _heartBeatSound.SetActive(true);
            realSpeed = (_speed * 0.5f);
            _isDeadly = true;
            EventManager.Trigger(ManagerKeys.LifeEvent, life, maxLife, _isDeadly);
        }
    }

    private void NormalStatus()
    {
        realSpeed = _speed;
        _heartBeatSound.SetActive(false);
        _isDeadly = false;
        EventManager.Trigger(ManagerKeys.LifeEvent, life, maxLife, _isDeadly);
    }

    private void AddGranade(params object[] granadeAddedParameter)
    {
        if (_granadeAvalible < _maxGranadeAvalible)
        {
            _granadeAvalible += (int)granadeAddedParameter[0];

            if (_granadeAvalible > _maxGranadeAvalible)
                _granadeAvalible = _maxGranadeAvalible;

            Destroy((GameObject)granadeAddedParameter[1]);

            EventManager.Trigger(ManagerKeys.GranadeNumber, _granadeAvalible);
            AudioManager.instance.AudioPlay((SoundData)granadeAddedParameter[2], transform.position);
        }
        
    }

    private void AddLife(params object[] lifeAddedParameters)
    {
        if(life < maxLife)
        {
            life += (float)lifeAddedParameters[0];

            if (life > (maxLife * 0.25))
            {
                NormalStatus();
            }

            if (life > maxLife)
                life = maxLife;

            Destroy((GameObject)lifeAddedParameters[1]);

            EventManager.Trigger(ManagerKeys.LifeEvent, life, maxLife, _isDeadly);
            AudioManager.instance.AudioPlay((SoundData)lifeAddedParameters[2], transform.position);
        }
        
    }
}