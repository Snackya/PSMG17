﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalBarrier : MonoBehaviour {

    private List<Transform> circles = new List<Transform>();
    private List<Transform> pillars = new List<Transform>();

    private bool circlesReactivating = false;
    private float pillarReactivationTime = 10f;
    private float circleReactivationTime = 2f;

    [HideInInspector]
    public bool barrierActive = true;
    private bool playersInside;

	void Start ()
    {
        FillCirclesList();
        FillPillarsList();
        playersInside = GetComponentInParent<Room20>().playersInside;
	}

    private void FillCirclesList()
    {
        for (int i = 0; i < transform.FindChild("Circles").childCount; i++)
        {
            circles.Add(transform.FindChild("Circles").GetChild(i));
        }
    }

    private void FillPillarsList()
    {
        for (int i = 0; i < transform.FindChild("Pillars").childCount; i++)
        {
            pillars.Add(transform.FindChild("Pillars").GetChild(i));
        }
    }

    void Update ()
    {
        playersInside = GetComponentInParent<Room20>().playersInside;
        if (playersInside)
        {
            DeactivateCircles();
            ReactivateCircles();
        }
	}

    private void ReactivateCircles()
    {
        int inactiveCircleCounter = 0;
        foreach (Transform circle in circles)
        {
            if (!circle.gameObject.activeSelf) inactiveCircleCounter++;
        }
        if (inactiveCircleCounter == circles.Count && !circlesReactivating)
        {
            circlesReactivating = true;
            barrierActive = false;
            StartCoroutine(ActivateCircles());
        }
    }

    private IEnumerator ActivateCircles()
    {
        for (int i = 0; i < pillars.Count; i++)
        {
            yield return new WaitForSeconds(pillarReactivationTime / pillars.Count);
            ActivatePillar(i);
        }
        for (int i = circles.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(circleReactivationTime / circles.Count);
            circles[i].gameObject.SetActive(true);
            circles[i].GetComponent<CircleScript>().RotateCircles();
        }
        circlesReactivating = false;
        barrierActive = true;
    }

    private void ActivatePillar(int index)
    {
        pillars[index].GetComponent<EnemyHealth>().health.CurrentVal = 
            pillars[index].GetComponent<EnemyHealth>().health.MaxVal;
        pillars[index].GetChild(0).gameObject.SetActive(true);
        pillars[index].GetChild(1).gameObject.SetActive(false);
    }

    private void DeactivateCircles()
    {
        int counter = 0;
        for (int i = 0; i < pillars.Count; i++)
        {
            if (pillars[i].GetChild(1).gameObject.activeSelf)
            {
                counter++;
            }
        }
        if (counter >= 1) circles[0].gameObject.SetActive(false);
        if (counter >= 2) circles[1].gameObject.SetActive(false);
        if (counter >= 3) circles[2].gameObject.SetActive(false);
        if (counter == 4) circles[3].gameObject.SetActive(false);
    }
}
