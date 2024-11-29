using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationResetter : MonoBehaviour
{
    public void ResetHurtAnimation(Animator animator, float delay)
    {
        StartCoroutine(ResetHurtCoroutine(animator, delay));
    }

    private IEnumerator ResetHurtCoroutine(Animator animator, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (animator != null)
        {
            Debug.Log("Resetting isHurt");
            animator.SetBool("isHurt", false);
            Destroy(gameObject); // Destroy the animation reset object
        }
    }
}
