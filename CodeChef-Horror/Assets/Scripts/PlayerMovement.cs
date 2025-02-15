using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(1, 50)]
    [SerializeField] float Speed = 5;
    [HideInInspector]
    public Vector3 movementWSAD, move;
    Rigidbody rb;
    [HideInInspector]
    public bool CanThouMove = true;
    Camera mainCam;
    float point, angle = 0;
    public PlayerInputActions inputActions;
    Vector2 mousePos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;
        inputActions = new PlayerInputActions();
    }

    #region InputActions Enabling
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    #endregion

    private void FixedUpdate()
    {
        move = new Vector3(movementWSAD.normalized.x * Speed * Time.deltaTime, 0, movementWSAD.normalized.z * Speed * Time.deltaTime);
        //move = transform.TransformDirection(move);

        rb.MovePosition(move + transform.position);
    }

    private void Update()
    {
        angle = transform.rotation.y;
        Rotate();

        Vector2 MovementInput = inputActions.Gameplay.Movement.ReadValue<Vector2>();
        mousePos = inputActions.Gameplay.Rotation.ReadValue<Vector2>();

        movementWSAD = new Vector3(MovementInput.x, 0, MovementInput.y);
        Vector3 mousePosInWorld = mainCam.ScreenToWorldPoint(mousePos) - new Vector3(.1f, 0, 10f);

        if (mousePosInWorld == transform.position)
        {
            transform.LookAt(new Vector3(0, 0, 0));
        }
    }

    void Rotate()
    {
        Ray ray = mainCam.ScreenPointToRay(mousePos);

        Plane groundPlane = new Plane(Vector3.up, Vector3.up);

        if (groundPlane.Raycast(ray, out point))
        {
            Vector3 Point = ray.GetPoint(point);
            Vector3 CorrectedLookAT = new Vector3(Point.x, transform.position.y, Point.z);
            transform.LookAt(CorrectedLookAT);
        }
    }
}