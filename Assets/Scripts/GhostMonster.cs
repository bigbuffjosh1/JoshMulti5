using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GhostMonster : MonoBehaviour
{
    public Transform doorIn;
    public Transform standPoint;
    public Transform doorOut;
    public float speed = 2f;
    public float waitTime = 3f;

    private bool playerDetected = false;
    private bool sequenceRunning = false;

    [Header("Monster Trigger")]
    public InputActionReference triggerMonsterAction;

    void Start()
    {
    }
    private void OnEnable()
    {
        if (triggerMonsterAction != null)
        {
            Debug.Log("registered");
            triggerMonsterAction.action.Enable();
            triggerMonsterAction.action.performed += OnTriggerMonster;
        }
        else
        {
            Debug.Log("null");
        }
    }

    private void OnDisable()
    {
        if (triggerMonsterAction != null)
            triggerMonsterAction.action.performed -= OnTriggerMonster;
    }

    private void OnTriggerMonster(InputAction.CallbackContext context)
    {
        Debug.Log("triggered1");
        if (!sequenceRunning)
        {
            Debug.Log("triggered2");
            sequenceRunning = true;
            StartCoroutine(MonsterSequence());
        }
    }

    IEnumerator MonsterSequence()
    {
        // Walk in
        yield return StartCoroutine(MoveTo(doorIn.position));

        // Walk to stand point
        yield return StartCoroutine(MoveTo(standPoint.position));

        // Wait at stand point
        float timer = 0f;
        while (timer < waitTime)
        {
            if (playerDetected)
            {
                Debug.Log("Player spotted!");
                break;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        // Walk out
        yield return StartCoroutine(MoveTo(doorOut.position));

        // Optional: destroy monster or loop
        sequenceRunning = false;
    }

    IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            // Optional: rotate towards target
            Vector3 dir = (target - transform.position).normalized;
            if (dir != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 5f * Time.deltaTime);

            yield return null;
        }
    }

    // Call this later when player detected
    public void OnPlayerDetected()
    {
        playerDetected = true;
    }
}
