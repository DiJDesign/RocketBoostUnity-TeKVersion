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
    [SerializeField] ParticleSystem[] boosterParticles;

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
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(0.0f, vertThrust * Time.fixedDeltaTime, 0.0f);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(engineSoundClip);
        }
        if (!boosterParticles[0].isPlaying)
        {
            boosterParticles[0].Play();
        }
    }

    void StopThrusting()
    {
        audioSource.Stop();
        boosterParticles[0].Stop();
    }

    void ProcessRotate()
    {
        float rotationInput = rotate.ReadValue<float>();

        if(rotationInput > 0.0f)
        {
            PlayLeftBoosterParticles();
            ApplyRotation(-rotatePower);
        }
        else if(rotationInput < 0.0f)
        {
            PlayRightBoosterParticles();
            ApplyRotation(rotatePower);
        }
        else
        {
            StopSideBoostParticles();
        }
    }

    void ApplyRotation(float rotateDir)
    {
        rb.freezeRotation = true; // PhysicsSystemを止まる
        transform.Rotate(0.0f, 0.0f, rotateDir * Time.deltaTime);
        rb.freezeRotation = false;
    }

    void PlayRightBoosterParticles()
    {
        if (!boosterParticles[2].isPlaying)
        {
            boosterParticles[1].Stop();
            boosterParticles[2].Play();
        }
    }

    void PlayLeftBoosterParticles()
    {
        if (!boosterParticles[1].isPlaying)
        {
            boosterParticles[2].Stop();
            boosterParticles[1].Play();
        }
    } 

       void StopSideBoostParticles()
    {
        boosterParticles[1].Stop();
        boosterParticles[2].Stop();
    }
}
