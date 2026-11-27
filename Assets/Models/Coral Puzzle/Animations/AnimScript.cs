using UnityEngine;
using UnityEngine.InputSystem;

public class SwimmingController : MonoBehaviour
{
    private Animator anim;
    [Header("Player Input Handler")]
    [SerializeField] private PlayerInputHandler _inputHandler;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isMovingForward = _inputHandler.MoveInput.y >= 0.1f;

        anim.SetBool("Is_Swimming", isMovingForward);
    }
}
