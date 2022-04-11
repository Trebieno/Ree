

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocity : MonoBehaviour, IMoveVelocity {

    [SerializeField] private float moveSpeed;

    private Vector3 velocityVector;
    private new Rigidbody2D rigidbody2D;

    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        
    }

    public void SetVelocity(Vector3 velocityVector) {
        this.velocityVector = velocityVector;
    }

    private void FixedUpdate() {
        rigidbody2D.velocity = velocityVector * moveSpeed;
    }

    public void Disable() {
        enabled = false;
        rigidbody2D.velocity = Vector3.zero;
    }

    public void Enable() {
        enabled = true;
    }

}
