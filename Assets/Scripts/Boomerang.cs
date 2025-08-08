using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    [SerializeField] private int maxDistance = 99;
    [SerializeField] private float lerpDuration = 5f;
    [SerializeField] private float rotationSpeed;
    public bool isMoving {  get; private set; }
    public bool isReturning {  get; private set; }
    public bool held { get; private set; }

    private PlayerUnit owner;
    private Collider2D collider;

    public int MaxDistance {  get { return maxDistance; } }

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
        held = true; // Start with the boomerang held
    }

    private void LateUpdate()
    {
        if (held == false)
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed));
        }
    }

    public void Initialize(PlayerUnit owner)
    {
        this.owner = owner;
        transform.localPosition = Vector3.zero;
    }

    public void ExecuteThrow(Tile targetTile, ICommandInvoker invoker)
    {
        StartCoroutine(ThrowRoutine(targetTile, invoker));
    }
    public void ExecuteReturn(Vector3 targetPosition, ICommandInvoker invoker)
    {
        StartCoroutine(CatchRoutine(targetPosition, invoker));
    }

    IEnumerator CatchRoutine(Vector3 targetPosition, ICommandInvoker invoker)
    {
        isReturning = true;
        isMoving = true;
        /*owner.WeaponParent().position;*/
        yield return LerpToTargetPosition(transform.position, targetPosition);
        isMoving = false;
        isReturning = false;
        collider.enabled = false;
        held = true;
        owner.BoomerangeCatch();
        invoker.FinishedCommandExecution(null);
    }

    IEnumerator ThrowRoutine(Tile targetTile, ICommandInvoker invoker)
    {
        yield return new WaitForSeconds(.5f);
        transform.parent = null;
        held = false;
        collider.enabled = true;
        isMoving = true;
        yield return LerpToTargetPosition(transform.position, targetTile.gameObject.transform.position);
        isMoving = false;
        GridManager.Instance.AssignWeaponToTile(targetTile, transform);
        owner.BoomerangeThrew();
        invoker.FinishedCommandExecution(null);
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

        transform.position = targetPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyUnit enemy;
        collision.gameObject.TryGetComponent<EnemyUnit>(out enemy);

        if (enemy != null) {
            enemy.TakeDamage();
            enemy = null;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyUnit enemy;
        collision.gameObject.TryGetComponent<EnemyUnit>(out enemy);

        if (enemy != null)
        {
            enemy.TakeDamage();
            enemy = null;
        }
    }
}
