using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInterception : MonoBehaviour
{
    #region ATTRIBUTES
    public bool IsOn;

    [Header("Target params")]
    public Transform TargetTf;
    public Vector3 TargetVel;

    [Header("Missile params")]
    public Transform MissileTf;
    public float MissileSpeed = 5f;
    public Vector3 MissileVel;

    public float SmallerDist;
    public float CurrDist;
    #endregion ATTRIBUTES


    #region METHODS
    [ContextMenu("Restart simulation")]
    void RestartSim()
    {
        IsOn = true;

        MissileVel = Interception.CalculateInterceptVelocity(MissileTf.position, TargetTf.position, TargetVel, MissileSpeed);

        SmallerDist = Vector3.Distance(MissileTf.position, TargetTf.position);
    }

    private void Update()
    {
        if (!IsOn) return;

        TargetTf.position += TargetVel * Time.deltaTime;
        MissileTf.position += MissileVel * Time.deltaTime;

        CurrDist = Vector3.Distance(TargetTf.position, MissileTf.position);

        if (CurrDist < SmallerDist)
        {
            SmallerDist = CurrDist;
        }

        if (CurrDist <= 0.5f)
        {
            IsOn = false;
            Debug.Log("Reached target!");
        }
    }

    #endregion METHODS
}
