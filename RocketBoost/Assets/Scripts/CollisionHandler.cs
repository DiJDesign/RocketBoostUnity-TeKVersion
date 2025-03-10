using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float loadDelay = 3.0f;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] ParticleSystem[] particleSystems;

    AudioSource audioSource;

    bool isControllable = true;
    bool isCollidable = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other)
    {
        if(!isControllable || !isCollidable) { return; }

        switch (other.gameObject.tag)
        {
            case "Fuel":
                Debug.Log("燃料だ！");
                break;
            case "LaunchPad":
                Debug.Log("ゴーゴー！");
                break;
            case "FinishPad":
                Debug.Log("よくやった");
                LandingSquence();
                break;
            default:
                Debug.Log("おい、さわらないで！");
                StartCrashSequence();
                break;
        }
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if(Keyboard.current.lKey.wasPressedThisFrame)
        {
            Debug.Log("次のレベルをデバッグとして読み込んだ。");
            LoadNextLevel();
        }
        else if(Keyboard.current.cKey.wasPressedThisFrame)
        {
            isCollidable = !isCollidable;
            if(!isCollidable)
            {
                Debug.Log("衝突は無効にした。");
            }
            else
            {
                Debug.Log("衝突は有効にした。");
            }
        }
    }

    void StartCrashSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(audioClips[0]); // Crash
        particleSystems[0].Play();
        GetComponent<RocketMovement>().enabled = false;
        Invoke("ReloadLevel", loadDelay);
    }

    void LandingSquence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(audioClips[1]); // Success
        particleSystems[1].Play();
        GetComponent<RocketMovement>().enabled = false;
        Invoke("LoadNextLevel", loadDelay);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex;
        nextSceneIndex++;
        if(nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadSceneAsync(nextSceneIndex);
    }
}
