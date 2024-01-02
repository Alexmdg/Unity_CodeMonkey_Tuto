using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInputs gameInput;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform kitchenObjectHoldingPoint;
    private bool isWalking;
    private float playerRadius = 0.7f;
    private float playerHeight = 2f;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;



    /**************KITCHENOBJECTPARENT_INTERFACE****************/
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void SetKitchenObject(KitchenObject kitchenObject) => this.kitchenObject = kitchenObject;
    public void ClearKitchenObject() => this.kitchenObject = null;
    public bool HasKitchenObject() => this.kitchenObject != null;
    public Transform GetKitchenObjectFollowTransform() => kitchenObjectHoldingPoint;
    /***********************************************************/

    public event EventHandler OnPickUp;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAltAction += GameInput_OnInteractAltAction;
    }

    private void Awake()
    {
        if (Instance != null) 
        {
            Debug.LogError("Error");
        }
        Instance = this;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying())
            return;
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
            OnPickUp?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnInteractAltAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            Debug.Log("Player : OnInteractAlternate();");
            selectedCounter.InteractAlternate(this);
        }
    }

    private void Update()
    {
        HandleMovment();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetNormalizedMovementVector();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero) 
        {
            lastInteractDir = moveDir;
        }

        float interactDist = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDist, layerMask)) 
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) 
            {
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }
    private void HandleMovment()
    {
        Vector2 inputVector = gameInput.GetNormalizedMovementVector();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        float moveDist = moveSpeed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDist);
        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDist);
            if (canMove) 
            {
                moveDir = moveDirX;
            }
            else 
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ , moveDist);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }
        }                
        if (canMove)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        transform.forward = Vector3.Slerp(transform.forward, moveDir, moveSpeed * Time.deltaTime);
        isWalking = moveDir != Vector3.zero;
    }

    public bool IsWalking() 
    {
        return isWalking;
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
    }

}
