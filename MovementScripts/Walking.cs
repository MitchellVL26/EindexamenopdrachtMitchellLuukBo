using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        // Determine movement direction
        if (Input.GetKey(KeyCode.W))
        {
            PlayAnimation("Walk forwards");
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            PlayAnimation("Walk backwards");
            transform.position -= transform.forward * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            PlayAnimation("Strafe left");
            transform.position -= transform.right * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
           
            PlayAnimation("Strafe right");
            transform.position += transform.right * moveSpeed * Time.deltaTime;
            
        }
        else
        {
            PlayAnimation("Idleman");
        }
    }

    // Plays an animation only if it’s not already playing
    void PlayAnimation(string animationName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName(animationName))
        {
            animator.Play(animationName);
        }
    }
}
 