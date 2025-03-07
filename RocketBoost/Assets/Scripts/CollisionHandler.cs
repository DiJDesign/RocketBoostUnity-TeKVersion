using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float loadDelay = 3.0f;
    [SerializeField] AudioClip[] audioClips;

    AudioSource audioSource;

    bool isControllable = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other)
    {
        if(!isControllable) { return; }

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

    void StartCrashSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(audioClips[0]); // Crash
        GetComponent<RocketMovement>().enabled = false;
        Invoke("ReloadLevel", loadDelay);
    }

    void LandingSquence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(audioClips[1]); // Success
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
