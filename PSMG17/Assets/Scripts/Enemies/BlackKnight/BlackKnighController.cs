﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackKnighController : MonoBehaviour {

    [SerializeField] private Transform[] targets;
    [SerializeField] private float movementSpeed;
    [SerializeField] private GameObject bloodPool;
    [SerializeField] private DialogManager diaMan;

    [SerializeField]
    private TextAsset preFightText;
    [SerializeField]
    private TextAsset loseArmsText;
    [SerializeField]
    private TextAsset loseLegsText;

    private Transform target;
    private Rigidbody2D enemy;
    private EnemyHealth enemyHealth;

    private float aggroTime = 4f;
    private Vector2 moveDirection;
    private bool alreadyFought = false;

    void Awake()
    {
        target = targets[0];
        enemy = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<EnemyHealth>();
        
    }

    void OnEnable()
    {
        StartCoroutine(SelectNearestTarget());
    }

    IEnumerator SelectNearestTarget()
    {
        float distanceToCurrentTarget = Vector2.Distance(enemy.position, target.position);

        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[0].gameObject.activeSelf)
            {
                target = targets[1];
            }
            else if (!targets[1].gameObject.activeSelf)
            {
                target = targets[0];
            }
            else if (distanceToCurrentTarget > Vector2.Distance(enemy.position, targets[i].position))
            {
                target = targets[i];
            }
        }

        yield return new WaitForSeconds(aggroTime);
        StartCoroutine(SelectNearestTarget());
    }

    void Start () {
		
	}
	
	void Update () {
        CalculateMoveDirection();
    }

    void FixedUpdate()
    {
        if (!diaMan.isRunning)
        {
            enemy.AddForce(moveDirection * movementSpeed);
        }
    }

    private void CalculateMoveDirection()
    {
        moveDirection = (target.position - enemy.transform.position).normalized;
    }

    void SpawnBloodPool()
    {
        Vector3 poolSpawnPosition = transform.position;
        Quaternion poolRoation = new Quaternion();  //do not rotate
        Instantiate(bloodPool, poolSpawnPosition, poolRoation, transform.parent);
    }

    public void StartPrefightDialog()
    {
        diaMan.StartDialog(preFightText, "The Black Knight");
    }

    void StartLosingArmsDialog()
    {
        diaMan.StartDialog(loseArmsText, "The Black Knight");
    }

    void StartLosingLegsDialog()
    {
        diaMan.StartDialog(loseLegsText, "The Black Knight");
    }
}
