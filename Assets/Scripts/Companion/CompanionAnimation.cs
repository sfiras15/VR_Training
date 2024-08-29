using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CompanionAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ActivateMovingAnimation(bool value)
    {
        if (animator != null)
        {
            animator.SetBool("isMoving", value);
        }
    }

    public IEnumerator ActivateWavingAnimation(bool value)
    {
        if (animator != null)
        {
            // Stop any existing coroutine before starting a new one to prevent overlapping
            StopAllCoroutines();
            // Set the waving animation to true
            animator.SetBool("isWaving", value);

            if (value) // Only wait if we are starting the waving animation
            {
                // Get the duration of the waving animation
                float animationDuration = GetAnimationClipLength("Waving"); // make sure it's spelled the same way in the AnimationController

                // Wait for the duration of the animation
                yield return new WaitForSeconds(animationDuration);

                // Set the waving animation to false after it ends
                animator.SetBool("isWaving", false);
            }
        }
    }

    // Helper method to get the length of the animation clip
    private float GetAnimationClipLength(string animationName)
    {
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;

        foreach (AnimationClip clip in controller.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }

        Debug.LogWarning("Animation clip not found: " + animationName);
        return 0f;
    }
}
