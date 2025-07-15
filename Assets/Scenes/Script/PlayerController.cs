using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Scripting.APIUpdating;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float runSpeed;
    private float applySpeed;

    [SerializeField]
    private float jumpForce;

    private float lastJumpTime;
    [SerializeField] private float jumpCooldown = 0.1f;


    private bool isRun = false;
    private bool isGround = true;

    private CapsuleCollider Cap;

    [SerializeField]
    private float lookSensitivity;

    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    [SerializeField]
    private Camera theCamera;

    private Rigidbody myRigid;

    [SerializeField] 
    private float maxStamina = 100f;
    [SerializeField] 
    private float staminaDrain = 20f;  // �ʴ� �Ҹ�
    [SerializeField] 
    private float staminaRegen = 10f;  // �ʴ� ȸ����
    [SerializeField] 
    private Slider staminaSlider;
    [SerializeField] 
    private float regenDelay = 2f; // �� �߰���
    private float lastShiftReleaseTime = -Mathf.Infinity; // �� �߰���

    public Animator ani;


    private float stamina;
    private CharacterController controller;

    void Start()
    {
        Cap = GetComponent<CapsuleCollider>();
        controller = GetComponent<CharacterController>();
        stamina = maxStamina;
        myRigid = GetComponent<Rigidbody>();
        theCamera = FindObjectOfType<Camera>();
        applySpeed = walkSpeed;
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        if (staminaSlider != null)
            staminaSlider.value = stamina / maxStamina;
        Move();
        CameraRotation();
        CharacterRotation();
        if (Input.GetKeyDown(KeyCode.R))
        {
            ani.SetTrigger("Reloading"); // "Reload" Ʈ���� �ߵ�
        }
    }

    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, Cap.bounds.extents.y + 0.1f);
    }

    private void TryJump()
    {
        if (Time.time - lastJumpTime < jumpCooldown) return;

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    private void Jump()
    {
        myRigid.linearVelocity = transform.up * jumpForce;
        lastJumpTime = Time.time;
    }

    private void TryRun()
    {
        bool isHoldingShift = Input.GetKey(KeyCode.LeftShift);
        bool justReleasedShift = Input.GetKeyUp(KeyCode.LeftShift);

        // �޸��� ����: Shift�� ������ �ְ� ���¹̳��� 0���� ū ��츸
        if (isHoldingShift && stamina > 0f)
        {
            // �޸��� ���·� ��ȯ
            Running();
            ani.SetBool("Running", true);

            // ���¹̳� �Ҹ�
            stamina -= staminaDrain * Time.deltaTime;
            stamina = Mathf.Max(stamina, 0f);

            // ���¹̳��� 0�� �Ǹ� ������ �޸��� ���
            if (stamina <= 0f)
            {
                RunningCancel();
                ani.SetBool("Running", false);
            }
        }
        else
        {
            // �޸��� ���
            RunningCancel();

            // Shift�� ���� �� ȸ�� Ÿ�̸� ����
            if (justReleasedShift)
            {
                lastShiftReleaseTime = Time.time;
                ani.SetBool("Running", false);
            }

            // ���� �ð��� �����ٸ� ���¹̳� ȸ�� ����
            if (Time.time - lastShiftReleaseTime >= regenDelay)
            {
                stamina += staminaRegen * Time.deltaTime;
                stamina = Mathf.Min(stamina, maxStamina);
            }
        }

        // ���¹̳� UI ����
        if (staminaSlider != null)
            staminaSlider.value = stamina / maxStamina;
    }


    private void Running()
    {
        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancel(){
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void Move()
    {
        // �¿� �Է��� �޾� �޸��� �Ǵ� �ȱ� ���� ���� (��, �� �Ǵ� A, D Ű)
        float _moveDirX = Input.GetAxisRaw("Horizontal");

        // �յ� �Է��� �޾� �޸��� �Ǵ� �ȱ� ���� ���� (��, �� �Ǵ� W, S Ű)
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        // �÷��̾ �ٶ󺸴� ���� �������� �¿� �̵� ���� ����
        Vector3 _moveHorizontal = transform.right * _moveDirX;

        // �÷��̾ �ٶ󺸴� ���� �������� �յ� �̵� ���� ����
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        // �Է� ���� ���͸� ����ȭ�� �� ���� �̵� �ӵ� (�ȱ� or �޸���) ����
        // ���¹̳��� 0�̸� applySpeed�� �ȱ� �ӵ��� ��ȯ�Ǿ�� ��
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;


        // ���� �̵� ó��: �޸��� ���̸� ������ �̵�, ���¹̳� 0�̸� ���� �ȱ� ��ȯ
        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);

        ani.SetBool("Walking", true);
        if (_velocity.magnitude == 0f)
        {
            ani.SetBool("Walking", false);
        }
        

    }

    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
    public float GetStaminaPercent()
    {
        return stamina / maxStamina;
    }
    

}

