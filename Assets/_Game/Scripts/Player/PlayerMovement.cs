using UnityEngine;
using OrbRaiders.UI;

namespace OrbRaiders.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Configuration")]
        [SerializeField] private float acceleration = 25.0f;
        [SerializeField] private float deceleration = 30.0f;
        [SerializeField] private float rotationSpeed = 14.0f;
        [SerializeField] private float arenaRadius = 18.0f;

        private PlayerStats stats;
        private VirtualJoystick joystick;
        private Vector3 currentVelocity;
        private CharacterController characterController;

        public Vector3 MovementDirection { get; private set; }
        public bool IsMoving => MovementDirection.sqrMagnitude > 0.01f;

        private void Awake()
        {
            stats = GetComponent<PlayerStats>();
            characterController = GetComponent<CharacterController>();
        }

        public void SetJoystick(VirtualJoystick activeJoystick)
        {
            joystick = activeJoystick;
        }

        private void Update()
        {
            Vector2 inputDir = Vector2.zero;
            if (joystick != null)
            {
                inputDir = joystick.InputDirection;
            }
            else
            {
                // Fallback for Keyboard testing
                inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            }

            MovementDirection = new Vector3(inputDir.x, 0, inputDir.y).normalized;

            float targetSpeed = stats != null ? stats.MoveSpeed : 5.5f;
            Vector3 targetVelocity = MovementDirection * targetSpeed;

            float lerpRate = MovementDirection.magnitude > 0 ? acceleration : deceleration;
            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, lerpRate * Time.deltaTime);

            if (characterController != null)
            {
                characterController.Move(currentVelocity * Time.deltaTime);
            }
            else
            {
                transform.position += currentVelocity * Time.deltaTime;
            }

            // Boundary clamping
            Vector3 pos = transform.position;
            if (pos.magnitude > arenaRadius)
            {
                transform.position = pos.normalized * arenaRadius;
            }

            // Smooth rotation towards movement direction
            if (MovementDirection.sqrMagnitude > 0.05f)
            {
                Quaternion targetRot = Quaternion.LookRotation(MovementDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
