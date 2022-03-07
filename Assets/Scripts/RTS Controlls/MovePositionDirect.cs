using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePositionDirect : MonoBehaviour, IMovePosition
{
    public Vector3 movePosition;

    public bool moving;

    private void Awake()
    {
        movePosition = transform.position;
    }

    public void SetMovePosition(Vector3 movePosition)
    {
        this.movePosition = movePosition;
    }


    private Vector3 moveDir;
    void Update()
    {
        if (moving)
        {
            if (Vector3.Distance(movePosition, transform.position) >= 1.0f)
            {
                moveDir = (movePosition - transform.position).normalized;
            }

            if (Vector3.Distance(movePosition, transform.position) < 1.0f) moveDir = Vector3.zero;
            GetComponent<IMoveVelocity>().SetVelocity(moveDir);
        }
    }
}
