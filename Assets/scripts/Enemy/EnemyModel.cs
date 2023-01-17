using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : Entity, IDamageable
{
    private bool _canMove;
    private bool _death;

    private float _originaSpeed;


    [SerializeField]
    private float _speed;

    [SerializeField]
    private Animator _animator;

    private EnemyView enemyView;

    [SerializeField]
    private Transform _playerPosition;

    [SerializeField]
    private Rigidbody _enemyRigidBody;

    [SerializeField]
    private ParticleSystem _bloodParticles;

    [SerializeField]
    private ParticleSystem _deathParticles;

    protected override void Awake()
    {
        base.Awake();

        enemyView = new EnemyView(this, _animator);

        _originaSpeed = _speed;
    }


    void Update()
    {
        if (_canMove && !_death)
            MoveEnemy();

    }

    private void OnTriggerEnter(Collider other)
    {
        //Start ScreamAnim
        var player = other.gameObject.GetComponent<PlayerModel>();
        
        if(player != null && !_death)
        {
            transform.LookAt(_playerPosition.position);
            enemyView.DetectionAnim(true);
        }
    }

    private void MoveEnemy()
    {
        //Enemy look at player
        transform.LookAt(_playerPosition.position);

        //Move Enemy
        transform.position = Vector3.MoveTowards(transform.position, _playerPosition.position, Time.deltaTime * _speed);
        //transform.position = Vector3.Lerp(transform.position, _playerPosition.position, _speed * Time.deltaTime);
    }

    //Actually Active enemyMovement when the Scream Anim Ends. It's an Anim Event
    public void ActivateMovement() => _canMove = true;

    public void DesactivateMovement()
    {
        _canMove = false;
        enemyView.DetectionAnim(false);
    }

    public void RunAttackAnim() => enemyView.AttackAnim();
    

    #region Interacews

    protected override void CheckLife()
    {
        if (life < 0)
        {
            _death = true;
            enemyView.Death();
            _speed = 0;
            _deathParticles.Play();
            Destroy(gameObject, 7);
        }
    }

    public void TakeDamage(float damage)
    {
        life -= damage;

        float random = Random.Range(0, 101);
        Debug.Log(random);

        if(random == 3)
        {
            enemyView.GetHurtkAnim();
            _speed = 0;
        }

        if(random < 5)
            _bloodParticles.Play();

        _speed *=  0.5f;

        StartCoroutine(recoverSpeed());
        CheckLife();
    }

    IEnumerator recoverSpeed()
    {
        yield return new WaitForSeconds(2f);
        _speed = _originaSpeed;
    }

    public void TakeDamage(float damage, Vector3 dir)
    {
        life -= damage;
        _enemyRigidBody.AddForce(dir);
    }
    public void FallDamage(float distanceFall, float damage) { }//Enemies won´t have falling damage

    #endregion
}