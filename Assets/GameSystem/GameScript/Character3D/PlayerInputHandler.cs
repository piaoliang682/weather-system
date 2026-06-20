using UnityEngine;
[RequireComponent(typeof(Rigidbody))] 
public class PlayerInputHandler : MonoBehaviour 
{ 
    private Transform cameraTransform;
    [Header("Key Bindings")] 
    public KeyCode forwardKey = KeyCode.W; 
    public KeyCode backKey = KeyCode.S; 
    public KeyCode leftKey = KeyCode.A; 
    public KeyCode rightKey = KeyCode.D;

    public KeyCode turnLeftKey = KeyCode.Q;
    public KeyCode turnRightKey = KeyCode.E;

    public KeyCode jumpKey = KeyCode.Space; 
    public KeyCode runKey = KeyCode.LeftShift; 
    public KeyCode attackKey = KeyCode.Mouse0;
    public KeyCode interactKey = KeyCode.E;
    public KeyCode greetKey = KeyCode.T;
    public KeyCode MouseLockKey = KeyCode.L;
    // customizable in Inspector
    public KeyCode usekey = KeyCode.U; // customizable in Inspector


    public VariableJoystick moveJoystick; 
    public VariableJoystick attackJoystick;


    private Move moveScript; 
    private Attack attackScript; 
    private Jump jumpScript; 
    private Turret turretScript;
    private Interact interactScript;
    private Carry carryScript;
    private InventoryController inventoryControllerScript;
    private Greet greetScript;


    private void Awake()
    {

        GameReference.Player = gameObject;

        if (SpawnPointManager.Instance != null)
        {
            if (SpawnPointManager.Instance.GetCurrentSpawnPoint() == null) return;
            gameObject.transform.position=SpawnPointManager.Instance.GetCurrentSpawnPoint().Position;
            gameObject.transform.rotation = SpawnPointManager.Instance.GetCurrentSpawnPoint().Rotation;
        }
    }
    void Start() 
    {

        cameraTransform = GameReference.CameraTransform;
        moveScript = GetComponent<Move>(); // assumes script is on same GameObject
                                           
        attackScript = GetComponent<Attack>(); 
        // assumes script is on same GameObject
        jumpScript = GetComponent<Jump>(); 
        // assumes script is on same GameObject
        turretScript = GetComponent<Turret>(); 
        // assumes script is on same GameObject
        inventoryControllerScript = GetComponent<InventoryController>();

        interactScript = GetComponent<Interact>();
        carryScript = GetComponent<Carry>();
        greetScript = GetComponent<Greet>();

        if (attackScript == null) 
            Debug.LogWarning("No Attack script found on player!"); 
    } 
    void FixedUpdate() 
    { 
        float movex = 0f, movez = 0f;
        // 🎮 Joystick Input (if assigned) 
        // 🧭 Keyboard Input
        if (Input.GetKey(forwardKey)) 
            movez += 1f;
        if (Input.GetKey(backKey))
            movez -= 1f; 
        if (Input.GetKey(leftKey)) movex -= 1f; 
        if (Input.GetKey(rightKey)) movex += 1f; 
        if (moveJoystick != null) 
        { 
            // Add joystick axes — combine with keyboard if both are used
            movex += moveJoystick.Horizontal; 
            movez += moveJoystick.Vertical; 
        }
        //Debug.Log(movex);
        Vector2 moveDir = new Vector2(movex, movez); 
        bool isRun = Input.GetKey(runKey); 
        if(moveScript!=null)
            moveScript.HandleMovement(moveDir.x, moveDir.y, isRun);

        // 🔄 TURN LEFT / RIGHT
        float turn = 0f;
        if (Input.GetKey(turnLeftKey)) turn -= 1f;
        if (Input.GetKey(turnRightKey)) turn += 1f;

        if (turn != 0f)
        {
            moveScript.HandleRotation(turn  * Time.fixedDeltaTime);
        }


        float attackx ; 
        float attackz;
        if (attackJoystick != null)
        {
            attackx = attackJoystick.Horizontal; 
            attackz = attackJoystick.Vertical; 
            //Debug.Log($"{attackJoystick.Horizontal},{attackJoystick.Vertical}"); 
            // Normalize vector to avoid faster diagonal movement
            Vector3 camF = cameraTransform.forward; camF.y = 0f; camF.Normalize(); 
            Vector3 camR = cameraTransform.right; camR.y = 0f; camR.Normalize(); 
            Vector3 lookDir = (camR * attackx + camF * attackz).normalized; 
            turretScript.AimDirection( lookDir); 
            }



    } 
    void Update() 
    { 
        if (Input.GetKeyDown(jumpKey)) 
        { 
            jumpScript.HandleJump(); 
        } 
        if (Input.GetKeyDown(usekey)) 
        { 
            inventoryControllerScript.HandleUse();
        } 
        if (Input.GetKeyDown(attackKey)&&attackScript!=null) 
        { 
            attackScript.HandleAttack(); 
        }
        if (Input.GetKeyDown(interactKey))
        {
            if (carryScript!=null)
                carryScript.HandleCarry();
            if (interactScript != null)
                interactScript.HandleInteraction();
        }
        if (Input.GetKeyDown(greetKey))
        {
            greetScript.TriggerBehaviour();
        }
    } 
}