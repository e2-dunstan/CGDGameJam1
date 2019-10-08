using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossStage
{
    STAGE1 = 0,
    STAGE2 = 1,
    STAGE3 = 2,
    STAGE4 = 3,
}

public class VultureBossManager : MonoBehaviour
{
    [SerializeField] private VultureEnemy vulture;
    [SerializeField] private VultureMovement vultureMovement;
    [SerializeField] private int currentBossHealth;
    [SerializeField] private bool canAttack = true;

    private BossStage bossStage = BossStage.STAGE1;

    private float timeOnCurrentStage = 0.0f;

    [SerializeField] private float timeBetweenShootingEasy = 2.0f;
    [SerializeField] private float timeBetweenShootingHard = 5.0f;

    [SerializeField] private int numberOfTimesShot = 0;

    [SerializeField] private GameObject EntranceDoor;
    [SerializeField] private GameObject ExitDoor;

    private GameObject player;
    private float distanceFromPlayer;

    public float distanceEnemyCanShootNearPlayer = 20.0f;

    private float timeStunned = 0.0f;

    public float timeTillResumeFromStunned = 2.0f;

    private bool hasBegun = false;
    private bool hasStartSequenceFinished = false;

    public GameObject entranceCollider;

    public GameUI gameUI;

    // Start is called before the first frame update
    void Start()
    {
        currentBossHealth = vulture.health;
        player = Player.Instance().gameObject;
        vulture.enemyState = Enemy.EnemyState.IDLE;
        canAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasBegun == true && hasStartSequenceFinished == true)
        {
            if (currentBossHealth != vulture.health)
            {
                currentBossHealth = vulture.health;
                ChangeBossStage();
            }

            if (vulture.enemyState == Enemy.EnemyState.STUNNED)
            {
                canAttack = false;
                timeOnCurrentStage = 0.0f;
                numberOfTimesShot = 0;
                timeStunned = timeStunned + Time.deltaTime;

                if(timeStunned >= timeTillResumeFromStunned)
                {
                    timeStunned = 0.0f;
                    vulture.enemyState = Enemy.EnemyState.WALKING;
                    vultureMovement.ForceMoveToDefaultPosition();
                }
            }
            else
            {
                timeOnCurrentStage += Time.deltaTime;
            }


            switch (bossStage)
            {
                case BossStage.STAGE1:

                    break;
                case BossStage.STAGE2:
                    if (timeBetweenShootingEasy < (timeOnCurrentStage / numberOfTimesShot)
                        && canAttack == true && CalculateDistanceFromPlayer() > distanceEnemyCanShootNearPlayer)
                    {
                        PromptEasyShoot();
                    }
                    break;
                case BossStage.STAGE3:
                    if (timeBetweenShootingHard < (timeOnCurrentStage / numberOfTimesShot)
                        && canAttack == true && CalculateDistanceFromPlayer() > distanceEnemyCanShootNearPlayer)
                    {
                        vulture.enemyMovement.moveSpeed = 2.5f;
                        PromptHardShoot();
                    }
                    break;
            }

            CalculateDistanceFromPlayer();
        }
        else if(hasBegun != true)
        {
            if(entranceCollider.GetComponent<BossEntranceTrigger>().hasBeenTriggered == true)
            {
                StartBoss();
            }
        }
    }

    private void ChangeBossStage()
    {
        switch (vulture.health)
        {
            case 2:
                bossStage = BossStage.STAGE2;
                vultureMovement.ForceMoveToDefaultPosition();
                break;
            case 1:
                bossStage = BossStage.STAGE3;
                vultureMovement.ForceMoveToShootingPosition();
                break;
            case 0:
                bossStage = BossStage.STAGE4;
                //Open door to leave
                canAttack = false;
                vulture.gameObject.GetComponent<Collider2D>().enabled = false;
                ExitDoor.GetComponent<Door>().OpenDoor();
                FinishBoss();
                break;

        }

        AttackCooldown(2.0f);
    }

    private void PromptEasyShoot()
    {
        numberOfTimesShot++;
        vulture.EasyShootAttack();
    }

    private float CalculateDistanceFromPlayer()
    {
        //Will remove this gameObject find, using until this is merged with alex's work, then we're implementing something else

        distanceFromPlayer = Vector2.Distance(vulture.gameObject.transform.position, player.transform.position);
        return distanceFromPlayer;
    }

    private void PromptHardShoot()
    {
        numberOfTimesShot++;
        vulture.HardShootAttack();
    }

    private void AttackCooldown(float _time)
    {
        canAttack = false;
        StartCoroutine("AttackCooldownCoroutine", _time);
    }

    IEnumerator AttackCooldownCoroutine(float _time)
    {
        yield return new WaitForSeconds(_time);
        canAttack = true;
    }

    private void StartBoss()
    {
        hasBegun = true;
        Player.Instance().ChangePlayerState(Player.PlayerState.NOINPUT);
        Player.Instance().PlayerMovement.SetPlayerVelocity(Vector2.zero);
        Player.Instance().PlayerMovement.Rigidbody.velocity = Vector2.zero;

        Player.Instance().PlayerCombat.isInBossScene = true;
        Player.Instance().WebManager.SetWebSwingOffset(5.0f);

        EntranceDoor.GetComponent<Door>().CloseDoor();

        gameUI.bottomText.text = "Ah, Spiderman!";

        StartCoroutine("BeginAfterTime", 2.0f);

    }

    IEnumerator BeginAfterTime(float _time)
    {
        yield return new WaitForSeconds(_time);
        gameUI.bottomText.text = "Welcome to my lair!";
        yield return new WaitForSeconds(_time);
        gameUI.bottomText.text = "Shall we begin?!";
        yield return new WaitForSeconds(_time);
        gameUI.bottomText.text = "";
        hasStartSequenceFinished = true;
        Player.Instance().WebManager.SetWebSwingOffset(5.0f);
        vulture.enemyState = Enemy.EnemyState.WALKING;
        Player.Instance().ChangePlayerState(Player.Instance().PreviousPlayerState);
    }

    private void FinishBoss()
    {
        Player.Instance().PlayerCombat.isInBossScene = false;
        Player.Instance().WebManager.SetWebSwingOffset(25.0f);
        ScoreManager.Instance.AddScore(6000);

        gameUI.bottomText.text = "Curse you spiderman!";

        StartCoroutine("FinishAfterTime", 2.0f);
    }

    IEnumerator FinishAfterTime(float _time)
    {
        yield return new WaitForSeconds(_time);
        gameUI.bottomText.text = "";
    }
}
