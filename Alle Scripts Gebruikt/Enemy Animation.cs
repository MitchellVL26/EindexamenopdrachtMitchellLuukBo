using UnityEngine;

public class ForceWalkAlways : MonoBehaviour
{
    public Animator animator;
    [Tooltip("Full path to the walk state in Layer 0, e.g. 'Base Layer.Walk' or 'Base Layer.Locomotion.Walk'")]
    public string walkStatePath = "Base Layer.Walk";

    [Header("Optional (works if your controller has these)")]
    public string speedParam = "Speed";      // for a 1D blend tree
    public float walkSpeedValue = 0.5f;      // stays in the walk range
    public string isWalkingParam = "IsWalking"; // for simple bool-driven state

    int walkHash, speedHash, walkBoolHash;

    void Awake()
    {
        if (!animator) animator = GetComponentInChildren<Animator>(true);
        if (!animator) { Debug.LogError("ForceWalkAlways: No Animator found."); enabled = false; return; }

        walkHash = Animator.StringToHash(walkStatePath);
        speedHash = Animator.StringToHash(speedParam);
        walkBoolHash = Animator.StringToHash(isWalkingParam);

        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate; // animate even off-screen
        animator.applyRootMotion = false; // doesn’t affect playing, but safe for NavMesh
    }

    void Start()
    {
        // First kick
        TryDriveParameters();
        animator.Play(walkHash, 0, 0f);
    }

    void Update()
    {
        // Keep forcing walk every frame so nothing can kick it back to idle
        TryDriveParameters();
        if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash != walkHash)
            animator.CrossFade(walkHash, 0.1f, 0, 0f);
    }

    void TryDriveParameters()
    {
        // If your controller has these params, keep them in a “walking” state too
        foreach (var p in animator.parameters)
        {
            if (p.nameHash == speedHash && p.type == AnimatorControllerParameterType.Float)
                animator.SetFloat(speedHash, walkSpeedValue);
            if (p.nameHash == walkBoolHash && p.type == AnimatorControllerParameterType.Bool)
                animator.SetBool(walkBoolHash, true);
        }
    }
}
