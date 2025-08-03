using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    [SerializeField] private int maxDistance = 99;
    [SerializeField] private float lerpDuration = 5f;
    public bool isMoving {  get; private set; }
    public bool isReturning {  get; private set; }

    private PlayerUnit owner;
    private Vector3 targetPosition;
    private EnemyUnit inCollisionEnemy;
    private Collider2D collider;

    public int MaxDistance {  get { return maxDistance; } }

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

        transform.localPosition = Vector3.zero;
    }

    public void ExecuteThrow()
    {
        transform.parent = null;
        collider.enabled = true;
        StartCoroutine(LerpToTargetPosition(transform.position, targetPosition));
    }
    public void ExecuteReturn()
    {
        isReturning = true;
        StartCoroutine(LerpToTargetPosition(transform.position, owner.WeaponParent().position));
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
            transform.parent = owner.WeaponParent();
        } else
        {
            owner.BoomerageThrew();
        }
        isReturning = false;
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
