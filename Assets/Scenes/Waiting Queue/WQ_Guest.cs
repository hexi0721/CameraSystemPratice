using System;
using System.Collections;
using UnityEngine;

public class WQ_Guest : MonoBehaviour
{

    Vector3 targetPostion;

    private void Awake()
    {
        targetPostion = transform.position;
    }

    private void Update()
    {
        HandleMoveMent();
    }

    private void HandleMoveMent()
    {
        float moveSpeed = 5f;
        Vector3 dir = (targetPostion - transform.position).normalized;
        float distance = Vector3.Distance(targetPostion, transform.position);

        if (distance > 0)
        {
            Vector3 newMovePosition = transform.position + dir * moveSpeed * Time.deltaTime;
            float newDistance = Vector3.Distance(targetPostion, newMovePosition);

            if (distance < newDistance)
            {
                newMovePosition = targetPostion;
            }

            transform.position = newMovePosition;
        }
    }

    public void MoveTo(Vector3 targetPostion)
    {
        this.targetPostion = targetPostion;

    }
    public void MoveTo(Vector3 targetPostion, Action action)
    {
        this.targetPostion = targetPostion;

        StartCoroutine(Moving(action));

    }

    private IEnumerator Moving(Action action)
    {
        while(transform.position != targetPostion)
        {
            yield return null;
        }

        action?.Invoke();
    }

    public void DoSomething(Action action)
    {
        
        StartCoroutine(something(action));
    }

    private IEnumerator something(Action action)
    {
        yield return new WaitForSeconds(4f);
        action?.Invoke();
    }
    
}
