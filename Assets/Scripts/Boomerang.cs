using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int maxDistance = 99;
    public float lerpDuration = 5f;
    public bool isMoving = false;
    public bool isReturning = false;

    private PlayerUnit owner;
    private Vector3 targetPosition;
    private EnemyUnit inCollisionEnemy;
    private Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
    }

    public void Initialize(PlayerUnit owner, Tile targetPosition = null)
    {
        this.owner = owner;
        if (targetPosition != null)
        {
            this.targetPosition = targetPosition.transform.position;
        } else
        {
            this.targetPosition = Vector3.zero;
        }
            transform.position = owner.transform.position;
    }

    public void ExecuteThrow()
    {
        transform.parent = null;
        collider.enabled = true;
        StartCoroutine(LerpToTargetPosition(transform.position, targetPosition));
    }

    IEnumerator LerpToTargetPosition(Vector3 initialPosition, Vector3 targetPosition)
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPosition, elapsedTime / lerpDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        transform.position = targetPosition; // Ensure it reaches the exact target position
        isMoving = false;

        if (isReturning)
        {
            collider.enabled = false;
        } else
        {
            owner.BoomerageThrew();
        }
        isReturning = false;
        if (owner.transform.position == transform.position) transform.parent = owner.transform;
    }

    public void ExecuteReturn()
    {
        isReturning = true;
        StartCoroutine(LerpToTargetPosition(transform.position, owner.transform.position));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.TryGetComponent<EnemyUnit>(out inCollisionEnemy);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyUnit enemy;
        collision.gameObject.TryGetComponent<EnemyUnit>(out enemy);

        if (inCollisionEnemy == enemy)
        {
            inCollisionEnemy.TakeDamage();
        }
    }
}
