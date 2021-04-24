using UnityEngine;
using UnityEngine.AI;

public class EnemyFlee : MonoBehaviour, IActorTemplate
{

    [SerializeField]
    SOActorModel actorModel;
    int health;
    int travelSpeed;
    int hitPower;
    int score;
    bool gameStarts = false;

    NavMeshAgent enemyAgent;
    GameObject player;
    [SerializeField]
    float enemyDistanceRun = 200;

    void Start()
    {
        ActorStats(actorModel);
        Invoke("DelayedStart", 0.5f);
    }

    void DelayedStart()
    {
        gameStarts = true;
        player = GameObject.FindGameObjectWithTag("Player");
        enemyAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (gameStarts)
        {
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);

                if (distance < enemyDistanceRun)
                {
                    //where we are minus the players position
                    Vector3 dirToPlayer = transform.position - player.transform.position;

                    //where we are and the dire
                    Vector3 newPos = transform.position + dirToPlayer;
                    enemyAgent.SetDestination(newPos);
                }
            }
        }
    }

    public void ActorStats(SOActorModel actorModel)
    {
        health = actorModel.health;
        GetComponent<NavMeshAgent>().speed = actorModel.speed;
        hitPower = actorModel.hitPower;
        score = actorModel.score;
    }

    public void TakeDamage(int incomingDamage)
    {
        health -= incomingDamage;
    }
    public int SendDamage()
    {
        return hitPower;
    }
    public void Die()
    {
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        // if the player or their bullet hits you....
        if (other.tag == "Player")
        {
            if (health >= 1)
            {
                health -= other.GetComponent<IActorTemplate>().SendDamage();
            }
            if (health <= 0)
            {
                //died by player, apply score to 
                GameManager.Instance.GetComponent<ScoreManager>().SetScore(score);
                Die();
            }
        }
    }
}
