using System;
using UnityEngine;
using System.Collections;
public static class CorroutinesKid
{
    public static IEnumerator ScaleAnimation(Transform transform,Vector3 scale,int upSpeed,int downSpeed,Action onAnimationEnd)
    {
        var delta = (scale-transform.localScale)/upSpeed;
        var startScale = transform.localScale;
        for (int i = 0; i < upSpeed; i++)
        {
            transform.localScale += delta;
            yield return null;
        }
        delta = (startScale - transform.localScale)/downSpeed;
        for (int i = 0; i < downSpeed; i++)
        {
            transform.localScale += delta;
            yield return null;
        }
        onAnimationEnd?.Invoke();
    }
    public static IEnumerator MoveLocalAndBack(Transform transform,Vector3 target,int speed,Action onAnimationEnd)
    {
        var delta = (target-transform.localPosition)/speed;
        var startPosition = transform.localPosition;
        for (int i = 0; i < speed; i++)
        {
            transform.localPosition += delta;
            yield return null;
        }
        delta = (startPosition - transform.localPosition)/speed;
        for (int i = 0; i < speed; i++)
        {
            transform.localPosition += delta;
            yield return null;
        }
        onAnimationEnd?.Invoke();
    }
    public static IEnumerator MoveLocal(Transform transform,Vector3 target,int speed,Action onAnimationEnd)
    {
        var delta = (target-transform.localPosition)/speed;
        for (int i = 0; i < speed; i++)
        {
            transform.localPosition += delta;
            yield return null;
        }
        onAnimationEnd?.Invoke();
    }
    public static IEnumerator Move(Transform transform,Vector3 target,int speed,Action onAnimationEnd)
    {
        var delta = (target-transform.position)/speed;
        for (int i = 0; i < speed; i++)
        {
            transform.position += delta;
            yield return null;
        }
        onAnimationEnd?.Invoke();
    }
}
