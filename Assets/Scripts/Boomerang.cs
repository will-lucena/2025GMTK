using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    [SerializeField] private int maxDistance = 99;
    [SerializeField] private float lerpDuration = 5f;
    [SerializeField] private float rotationSpeed;
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

    private void LateUpdate()
    {
        if (transform.parent == null)
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed));
        }
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
        StartCoroutine(ThrowRoutine());
    }
    public void ExecuteReturn()
    {
        StartCoroutine(CatchRoutine());
    }

    IEnumerator CatchRoutine()
    {
        isReturning = true;
        isMoving = true;
        yield return LerpToTargetPosition(transform.position, owner.WeaponParent().position);
        isMoving = false;
        isReturning = false;
        collider.enabled = false;
        transform.parent = owner.WeaponParent();
        owner.BoomerangeCatch();
    }

    IEnumerator ThrowRoutine()
    {
        yield return new WaitForSeconds(.5f);
        transform.parent = null;
        collider.enabled = true;
        isMoving = true;
        yield return LerpToTargetPosition(transform.position, targetPosition);
        isMoving = false;
        owner.BoomerangeThrew();
    }

    IEnumerator LerpToTargetPosition(Vector3 initialPosition, Vector3 targetPosition)
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPosition, elapsedTime / lerpDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        transform.position = targetPosition; // Ensure it reaches the exact target position
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
