﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room16 : MonoBehaviour
{

    [SerializeField]
    private Transform player1;
    [SerializeField]
    private Transform player2;
    [SerializeField]
    private Slider blackKnightHealthBar;
    [SerializeField]
    private Transform backdoor;
    [SerializeField]
    private MusicManager musicManager;

    private Transform door;

    private BoxCollider2D room;
    private Bounds roomBounds;

    private bool playersInside = false;
    private bool bossAlreadyDied = false;
    private bool alreadyEnteredOnce = false;

    private GameObject enemy;
    private GameObject boss;
    private EnemyHealth bossHealth;
    private Vector3 spawnPosition;

    void Start()
    {
        room = transform.FindChild("RoomBounds").GetComponent<BoxCollider2D>();
        roomBounds = room.bounds;
        door = transform.FindChild("Door");

        enemy = transform.FindChild("Enemies").gameObject;
        boss = enemy.transform.FindChild("BlackKnight").gameObject;
        spawnPosition = boss.transform.position;

        enemy.SetActive(false);
        boss.SetActive(false);
        blackKnightHealthBar.gameObject.SetActive(false);
        bossHealth = boss.GetComponent<EnemyHealth>();

        StartCoroutine(CheckIfEnemiesAreDead());
    }

    void Update()
    {
        ActivateEnemies();
        OpenDoor();
    }

    private IEnumerator CheckIfEnemiesAreDead()
    {
        if (bossHealth.health.CurrentVal <= 0)
        {
            if (!bossAlreadyDied)
            {
                musicManager.StopBossMusic1();
                musicManager.PlayBackgroundMusic();
            }
            blackKnightHealthBar.gameObject.SetActive(false);
            bossAlreadyDied = true;
            backdoor.GetChild(0).gameObject.SetActive(true);
            backdoor.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(CheckIfEnemiesAreDead());
        }
    }

    private void OpenDoor()
    {
        if (!boss.activeSelf)
        {
            door.GetChild(0).gameObject.SetActive(true);
            door.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            door.GetChild(0).gameObject.SetActive(false);
            door.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void ActivateEnemies()
    {
        if (roomBounds.Contains(player1.position) && roomBounds.Contains(player2.position) && !bossAlreadyDied)
        {
            if (!playersInside)
            {
                playersInside = true;
                enemy.SetActive(true);
                boss.SetActive(true);
                blackKnightHealthBar.GetComponentInChildren<Text>().text = "The Black Knight";
                blackKnightHealthBar.gameObject.SetActive(true);
                musicManager.StopBackGroundMusic();
                musicManager.PlayBossMusic1();
            }
            if (boss.activeSelf)
            {
                backdoor.GetChild(0).gameObject.SetActive(false);
                backdoor.GetChild(1).gameObject.SetActive(true);
                if (!alreadyEnteredOnce)
                {
                    boss.GetComponent<BlackKnighController>().StartPrefightDialog();
                    alreadyEnteredOnce = true;
                }
            }
            else
            {
                backdoor.GetChild(0).gameObject.SetActive(true);
                backdoor.GetChild(1).gameObject.SetActive(false);
            }
        }
    }


    public void resetRoom()
    {
        boss.transform.position = spawnPosition;
        playersInside = false;
        enemy.SetActive(false);
        boss.SetActive(false);
        backdoor.GetChild(0).gameObject.SetActive(true);
        backdoor.GetChild(1).gameObject.SetActive(false);
        blackKnightHealthBar.gameObject.SetActive(false);
        bossHealth.health.CurrentVal = bossHealth.health.MaxVal;
        musicManager.StopBossMusic1();
        musicManager.PlayBackgroundMusic();
    }
}
