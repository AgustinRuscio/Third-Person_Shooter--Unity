//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _forceJump;

    private bool _aiming = false;
    private bool _shooting = false;

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

    void Awake()
    {
        _playerController = new PlayerController(this);

        _playerView = new PlayerView(_myAnimator, this);

        _shootTimer = new GenericTimer(_shootCoolDown);

        _myRigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _playerController.ArtificialFixedUpdate();
    }

    void Update()
    {
        _playerController.ArtificialUpdate();
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
        else if (_onSprint && dir.z > 0)
        {
            realSpeed = (_speed * 2);
            dir.x = 0;

            if(dir.z < 0)
            {
                dir.z = 0;
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
        if (inFloor)
        {
            _myRigidBody.AddForce(Vector3.up * _forceJump, ForceMode.Impulse);
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
        
    }

    public void Shoot(bool onShoot)
    {

        _shooting = onShoot;

        //_playerView.SetShootAnim(_shooting);

        if (_aiming && _shooting)
        {
            _shootTimer.RunTimer();

            if (_shootTimer.CheckCoolDown(false))
            {
                Debug.Log("Shooting");

                //                 RaycastHit hit;
                // 
                //                 var raycast = Physics.Raycast(_shootPoint.position,_shootPoint.forward, out hit, 1000);
            }
            else
            {
                Debug.Log("OnCoolDown");
                StartCoroutine(OnCoolDown(_timeOnCoolDown));
            }

        }
    }

    IEnumerator OnCoolDown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _shootTimer.ResetTimer();
    }

    #endregion
}
