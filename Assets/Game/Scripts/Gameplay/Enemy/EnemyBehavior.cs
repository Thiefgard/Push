using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    enum EnemyCategory
    {
        Base,
        Fast,
        Range,
        Giant
    }
    public static EnemyBehavior Instance;
    [SerializeField] EnemyCategory category;
    [SerializeField] EnemyBullet bulletPref;
    [SerializeField] Transform bulletIns;
    [SerializeField] float firsMeetRadius = 8f;
    [SerializeField] int energyValue = 3;
    [SerializeField] float timeBetweenFind = 1f;
    [SerializeField] float agentSpeed;
    [SerializeField] float shootRadius;
    [SerializeField] float timeBetweenShoot;
    [SerializeField] bool readyShoot;

    [SerializeField] public bool isContactWeapon = false;
    [SerializeField] bool isFollowHit = false;
    [SerializeField] bool isOnEdge = false;
    public Rigidbody enemyRb;

    public bool isInBoundery;
    public bool isTrapped;
    bool isFirstCatch = false;
    bool isHited = false;
    [SerializeField]bool isOnGround = false;

    private Vector3 targetToShoot;
    private float counterShoot = 0f;
    float counterCatch = 0f;
    public NavMeshAgent agent;
    private GameObject player;
    private Animator anim;
    private AudioSource auido;

    private NavMeshHit hit;

    private Vector3 ray;

    private void Awake()
    {
        Instance = this;
        anim = GetComponent<Animator>();
        anim.speed = 2f;
    }
   
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        agent.enabled = true;
        anim.SetBool("isOnBound", true);
        auido = GetComponent<AudioSource>();

        player = Player.Instance.gameObject;
        
        isInBoundery = true;
        enemyRb = GetComponent<Rigidbody>();
        if (category == EnemyCategory.Range) agent.speed = 0f;
        else agent.speed = agentSpeed;
        agent.speed = agentSpeed;
        print(category);
        

        //StartCoroutine(RiseUp());
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(transform.position, transform.position + enemyRb.velocity.normalized * enemyRb.velocity.magnitude);
    //}

    void Update()
    {
        if (isTrapped || !isInBoundery)
        {
            return;
        }
        NavMesh.FindClosestEdge(transform.position, out hit, NavMesh.AllAreas);
        OnTheEdge(hit);

        if (gameObject != null && player != null && !isOnEdge )
        {
            CheckHit(isHited);
            IsFollowHit(isFollowHit);
            CheckStuck();

            float magnitude = enemyRb.velocity.magnitude;
            if (magnitude < 2f)
            {
                enemyRb.velocity = Vector3.zero;
            }
            //if (!isFirstCatch) enemyRb.isKinematic = true;
            //else enemyRb.isKinematic = false;
            if (isInBoundery && !isHited && !isFollowHit)
            {
                enemyRb.constraints = RigidbodyConstraints.FreezePositionY;
                enemyRb.constraints = RigidbodyConstraints.FreezeRotation;
            }
            else enemyRb.constraints = RigidbodyConstraints.FreezeRotation;
            if (isInBoundery && agent.enabled)
            {
                counterShoot += Time.deltaTime;
                counterCatch += Time.deltaTime;
                MovementAnimation();
                if (IsInRadius(transform, player.transform, firsMeetRadius))
                {
                    isFirstCatch = true;
                }
                if (isFirstCatch && !isHited && !isOnGround && counterCatch >= timeBetweenFind)
                {
                    counterCatch = 0;
                    FindTarget(player);
                }


                if (isContactWeapon)
                {
                    agent.speed = 0;
                    agent.isStopped = true;
                }
                else
                {
                    agent.speed = agentSpeed;
                }
                if (category == EnemyCategory.Range)
                {
                    if (isFirstCatch && IsInRadius(transform, player.transform, shootRadius) && counterShoot >= timeBetweenShoot && !isHited && !isOnGround)
                    {
                        if(Vector3.Distance(transform.position, player.transform.position) > 5f)
                        {
                            StartCoroutine(Shoot());
                        }
                    }
                }
            }
        }

    }

    public void FallDownOutZone()
    {
        isInBoundery = false;
        Vector3 velocity = enemyRb.velocity.normalized;
        gameObject.layer = LayerMask.NameToLayer("EnemyDie");

        //todo Marker
        GetComponent<OffscreenMarker>().Activate(false);

        //todo need to do after die
        OffscreenMarkersCameraScript.Instance().Remove(GetComponent<OffscreenMarker>());

        //anim.SetBool("hitted", true);
        agent.enabled = false;
        enemyRb.velocity = Vector3.zero;
        //enemyRb.AddForce(velocity * 100, ForceMode.Impulse);
        enemyRb.AddForce(-Vector3.up * 300, ForceMode.Impulse);
        GetHit();
    }

    private void FindTarget(GameObject player)
    {
        if (agent == null) return;
        if (player == null) return;
        if (agent.isStopped == true) return;
        targetToShoot = player.gameObject.transform.Find("TargetToShoot").position;
        agent.SetDestination(player.transform.position);
    }

    private bool IsInRadius(Transform point, Transform target, float radius) // add timer
    {
        float distance = Vector3.Distance(point.position, target.position);
        return distance <= radius;
    }

    private void MovementAnimation()
    {
        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        anim.SetFloat("forwardSpeed", speed);
    }
   
    

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player" && !isHited && !isOnGround)
        {
            StartCoroutine(MeleHit());
        }
        if(collision.collider.tag == "Weapon")
        {
            isContactWeapon = true;
            
            GetHit();
        }
        if (collision.collider.CompareTag("Trap"))
        {
            agent.isStopped = true;
            agent.enabled = false;
        }
        
    }
  

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Weapon")
        {
            isContactWeapon = false;
            GetHit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") )
        {
            if (/*other.gameObject.GetComponent<Rigidbody>().velocity.magnitude < 0.2f &&*/ other.gameObject.GetComponent<EnemyBehavior>().isContactWeapon)
            {
                isContactWeapon = true;
            }
            FollowHit(other, other.gameObject.GetComponent<Rigidbody>().velocity.magnitude);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<EnemyBehavior>().isContactWeapon == false)
            {
                isContactWeapon = false;
            }
        }
    }

    public void GetHit()
    {
        if(anim.GetBool("hitted") == false)
        {
            //auido.Play();
            anim.SetBool("hitted", true);
            isHited = true;
        }
    }

    void FallDown()
    {
        isOnGround = true;
    }
    private void CheckHit(bool hitted)
    {
        if (isInBoundery)
        {
            if (hitted)
            {
                if (agent.enabled == true)
                {
                    agent.isStopped = true;
                    agent.enabled = false;
                }
                if(enemyRb.velocity.magnitude < 3f)
                {
                    enemyRb.velocity =  Vector3.zero;
                }

                if (isInBoundery && isOnGround)
                {
                    TryToGetUp(enemyRb.velocity.magnitude);
                }
            }
            else
            {
                if(agent.enabled == false && isInBoundery)
                {
                    agent.enabled = true;
                    agent.isStopped = false;
                }
            }
        }
    }
    private void TryToGetUp(float magnitude)
    {
        if (magnitude < 0.1f)
        {
            anim.SetTrigger("tryToGetUp");
            //OnTheEdge(hit);
        }
    }
    //anim method to getUp
    public void GetUp()
    {
        anim.SetBool("hitted", false);
        if (isInBoundery)
        {
            isOnGround = false;
            isHited = false;
            anim.ResetTrigger("tryToGetUp");
        }
        else return;

    }

    private void HitPlayer(GameObject player)
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        player.transform.GetComponent<Rigidbody>().AddForce(direction * 1050, ForceMode.Impulse);

    }
    IEnumerator MeleHit()
    {
        agent.isStopped = true;
        HitPlayer(player);
        yield return new WaitForSeconds(2f);
        agent.isStopped = false;
        yield return null;
    }

    void ShootPlayer()
    {
        Vector3 direction = (targetToShoot - bulletIns.position) * 2f;
        transform.rotation = Quaternion.LookRotation(direction);
        if(direction != null)
        {
            EnemyBullet bullet = Instantiate(bulletPref, Level.Instance.transform);
            bullet.gameObject.transform.position = bulletIns.position;
            bullet.gameObject.transform.rotation = bulletIns.transform.rotation;
            bullet.Init(direction);
            counterShoot = 0;
        }
    }
    IEnumerator Shoot()
    {
        agent.isStopped = true;
        //yield return new WaitForSeconds(0.8f);
        ShootPlayer();
        yield return new WaitForSeconds(2f);
        agent.isStopped = false;
        yield break;

    }
    public void Die()
    {
        Player.Instance.weapon.AddEnergy(energyValue);
        anim.SetFloat("forwardSpeed", 0);
        GetComponentInParent<SpawnPoint>().EnemyDie(this.gameObject);

        Destroy(this.gameObject, 1.5f);
    }

    IEnumerator RiseUp()
    {
        agent.enabled = false;
        anim.SetTrigger("summon");
        yield return new WaitForSeconds(1f);
        agent.enabled = true;
        anim.ResetTrigger("summon");
        yield break;
    }

    void FollowHit(Collider other, float magnitude)
    {
        if (other.GetComponent<EnemyBehavior>().isHited)
        {
            if (magnitude > 1)
            {
                isFollowHit = true;
            }
        }
    }

    void IsFollowHit(bool hit)
    {
        if (hit && isInBoundery)
        {
            if (agent.enabled == true)
            {
                agent.isStopped = true;
                agent.enabled = false;
                //agentObstacle.enabled = true;
            }
            anim.SetFloat("forwardSpeed", 0);
            isContactWeapon = false;
            if (enemyRb.velocity.magnitude < 1)
            {
                enemyRb.velocity = Vector3.zero;
                agent.enabled = true;
                agent.isStopped = false;
                isFollowHit = false;
                isFirstCatch = true;
                //agentObstacle.enabled = false;


                //StartCoroutine(CheckRay());
            }
        }
    }

    void OnTheEdge(NavMeshHit hit)
    {
            if ((transform.position - hit.position).magnitude < 0.01f )
            {
                Vector3 velocity = enemyRb.velocity.normalized;
                agent.enabled = true;
                agent.isStopped = true;
                agent.enabled = false;
                anim.SetFloat("forwardSpeed", 0);

                if (!isOnEdge )
                {
                    //var direction = hit.position - transform.position;
                    gameObject.layer = LayerMask.NameToLayer("EnemyDie");
                    //direction.y = 0;
                    //enemyRb.AddForce(velocity * 20, ForceMode.Impulse);
                    isOnEdge = true;
                }
            }
    }

    IEnumerator CheckRay()
    {
        if(agent != null || player != null)
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(transform.position, player.transform.position, 2f);

            foreach(RaycastHit hit in hits)
            {
                if(hit.collider != null)
                {
                    yield return new WaitForSeconds(0.8f);
                    isContactWeapon = false;
                    //if (hit.collider.tag == "Enemy" && agent.enabled)
                    //{
                    //    if (!CheckSustain(hit.collider.GetComponent<EnemyBehavior>()))
                    //    {
                    //        yield return new WaitForSeconds(0.8f);
                    //        isContactWeapon = false;
                    //    }
                    //}
                }
                
                else
                {
                    isContactWeapon = false;
                }
            }
            
            yield break;
        }
        
    }
   

    private bool CheckSustain(EnemyBehavior eb)
    {

        return eb.isContactWeapon == false && eb.isOnGround == false;
    }

    private void CheckStuck()
    {
        if (isContactWeapon || isFollowHit)
        {
            if (!IsInRadius(this.transform, player.transform, 4f))
            {
                StartCoroutine(CheckRay());
            }
        }
    }
   
}
