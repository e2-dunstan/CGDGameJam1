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

    // Start is called before the first frame update
    void Start()
    {
        currentBossHealth = vulture.health;
    }

    // Update is called once per frame
    void Update()
    {
      
        if(currentBossHealth != vulture.health)
        {
            currentBossHealth = vulture.health;
            ChangeBossStage();
        }

        if(vulture.enemyState == Enemy.EnemyState.STUNNED)
        {
            canAttack = false;
            timeOnCurrentStage = 0.0f;
            numberOfTimesShot = 0;
        }
        else
        {
            timeOnCurrentStage += Time.deltaTime;
        }

        
        switch(bossStage)
        { 
            case BossStage.STAGE1:
               
                break;
            case BossStage.STAGE2:
                if (timeBetweenShootingEasy < (timeOnCurrentStage / numberOfTimesShot)
                    && canAttack == true)
                {
                    PromptEasyShoot();
                }
                break;
            case BossStage.STAGE3:
                if (timeBetweenShootingHard < (timeOnCurrentStage / numberOfTimesShot)
                    && canAttack == true)
                {
                    PromptHardShoot();
                }
                break;
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
                //Open door to leave
                canAttack = false;
                ExitDoor.GetComponent<Door>().OpenDoor();
                break;

        }

        AttackCooldown(2.0f);
    }

    private void PromptEasyShoot()
    {
        numberOfTimesShot++;
        vulture.EasyShootAttack();
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
}
