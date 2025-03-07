using UnityEngine;
using UnityEngine.InputSystem;

public class RocketMovement : MonoBehaviour
{
    // インプットアクション
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotate;
    [SerializeField] float vertThrust = 1000.0f;
    [SerializeField] float rotatePower = 500.0f;
    [SerializeField] AudioClip engineSoundClip;

    Rigidbody rb;
    AudioSource audioSource;

    void OnEnable()
    {
        thrust.Enable();
        rotate.Enable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessRotate();
    }

    void FixedUpdate()
    {
        ProcessThrust();
    }

    void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            rb.AddRelativeForce(0.0f, vertThrust * Time.fixedDeltaTime, 0.0f);
            if(!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(engineSoundClip);
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    void ProcessRotate()
    {
        float rotationInput = rotate.ReadValue<float>();

        if(rotationInput > 0.0f)
        {
            ApplyRotation(-rotatePower);
        }
        else if(rotationInput < 0.0f)
        {
            ApplyRotation(rotatePower);
        }
    }

    void ApplyRotation(float rotateDir)
    {
        rb.freezeRotation = true; // PhysicsSystemを止まる
        transform.Rotate(0.0f, 0.0f, rotateDir * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
