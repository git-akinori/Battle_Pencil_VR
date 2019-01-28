﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonsterBehaviour : MonoBehaviour
{

    [SerializeField]
    MonsterModel monsterModel;
    public MonsterModel MonsterModel { get { return monsterModel; } }

    MonsterContext monsterContext = null;

    // カウンター用
    public int enemyPower = 0;
    public bool isAttack = false;

    Transform standingTransform;
    public Animator MonsterAnimator { get; private set; }

    void Awake()
    {
        monsterContext = new MonsterContext();
        MonsterAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        if (gameObject.tag == "Player")
        {
            MonsterManager.Instance.PlayerMonsterBehaviour = this;
        }
        else if (gameObject.tag == "CPU")
        {
            MonsterManager.Instance.ComputerMonsterBehaviour = this;
        }

        GetComponentInParent<OperatorController>().OperatorModel.monsterBehaviour = this;
    }

    void FixedUpdate()
    {
        if (BattleManager.Instance.IsFight)
            monsterContext.ExecuteUpdate();
    }

    public IEnumerator GetJumpimgOnuma(GameObject jumpObj, Vector3 targetPosition, float tAngle)
    {
        StartCoroutine(JumpingOnuma(jumpObj, targetPosition, tAngle));
        yield return null;
    }

    IEnumerator JumpingOnuma(GameObject jumpObj, Vector3 targetPosition, float tAngle)
    {
        Debug.Log("超エキサイティング！");
        // 標的の座標
        // 射出角度
        float angle = tAngle;
        Vector3 velocity = CalculateVelocity(jumpObj.transform.position, targetPosition, angle);

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(velocity * rigidbody.mass, ForceMode.Impulse);

        yield return null;
    }



    private Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
    {
        // 射出角をラジアンに変換
        float rad = angle * Mathf.PI / 180;

        // 水平方向の距離x
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));

        // 垂直方向の距離y
        float y = pointA.y - pointB.y;

        // 斜方投射の公式を初速度について解く
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            // 条件を満たす初速を算出できなければVector3.zeroを返す
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
    }
}
