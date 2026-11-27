using UnityEngine;
using UnityEngine.InputSystem;

public class AnimScript : MonoBehaviour
{
    private Animator anim;

    [Header("Player Input Handler")]
    [SerializeField] private PlayerInputHandler _inputHandler;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // True only when pressing forward (W / Up Arrow / Left Stick Up)
        bool isMovingForward = _inputHandler.MoveInput.y >= 0.1f;

        anim.SetBool("Is_Swimming", isMovingForward);
    }
}