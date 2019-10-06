﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossStage
{
    STAGE1 = 0,
    STAGE2 = 1,
    STAGE3 = 2,
    STAGE4 = 3,
    STAGE5 = 4
}

public class VultureBossManager : MonoBehaviour
{
    [SerializeField] private VultureEnemy vulture;
    [SerializeField] private VultureMovement vultureMovement;
    [SerializeField] private int currentBossHealth;

    private BossStage bossStage = BossStage.STAGE1;

    private float timeOnCurrentStage = 0.0f;

    [SerializeField] private float timeBetweenShootingEasy = 2.0f;
    [SerializeField] private float timeBetweenShootingHard = 5.0f;

    [SerializeField] private int numberOfTimesShot = 0;

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

        switch(bossStage)
        {
            case BossStage.STAGE1:

                break;
            case BossStage.STAGE2:
                timeOnCurrentStage += Time.deltaTime;
                if(timeBetweenShootingEasy < (timeOnCurrentStage / numberOfTimesShot))
                {
                    PromptEasyShoot();
                }
                break;
            case BossStage.STAGE3:
                timeOnCurrentStage += Time.deltaTime;
                if (timeBetweenShootingEasy < (timeOnCurrentStage / numberOfTimesShot))
                {
                    PromptHardShoot();
                }
                break;
            case BossStage.STAGE4:
                timeOnCurrentStage += Time.deltaTime;
                if(timeBetweenShootingEasy < (timeOnCurrentStage / numberOfTimesShot))
                {
                    PromptEasyShoot();
                }
                else if (timeBetweenShootingHard < (timeOnCurrentStage / numberOfTimesShot))
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
            case 4:
                bossStage = BossStage.STAGE2;
                vultureMovement.ForceMoveToDefaultPosition();
                break;
            case 3:
                bossStage = BossStage.STAGE3;
                vultureMovement.ForceMoveToShootingPosition();
                break;
            case 2:
                bossStage = BossStage.STAGE4;
                vultureMovement.ForceMoveToShootingPosition();
                break;
            case 1:
                bossStage = BossStage.STAGE5;
                break;

        }
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
}