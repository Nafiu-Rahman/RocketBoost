using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;

    AudioSource audioSource;

    bool isControllable = true;
    bool isCollidable = true;
    [SerializeField] AudioClip Crash;
    [SerializeField] AudioClip Success;
    [SerializeField] ParticleSystem CrashParticles;
    [SerializeField] ParticleSystem SuccessParticles;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebugKeys();    
    }

    private void RespondToDebugKeys()
    {
        //if (Keyboard.current.lKey.isPressed)
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            nextLevel();
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame) //wasPresseedThisFrame use kora hoise 1 bar check korar jonno...otherwise loop cholte thake on off on off
        {
            isCollidable = !isCollidable;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isControllable || !isCollidable) { return; }
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly object");
                break;
            case "Fuel":
                Debug.Log("Fuel detected");
                break;
            case "Finish":
                StartFinishSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }
    void StartCrashSequence()
    {
        audioSource.Stop();
        isControllable = false;
        audioSource.PlayOneShot(Crash);
        CrashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("reloadLevel", levelLoadDelay);

    }

    void StartFinishSequence()
    {
        audioSource.Stop();
        isControllable = false;
        audioSource.PlayOneShot(Success);
        SuccessParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("nextLevel", levelLoadDelay);       
    }
        void reloadLevel()
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentScene);
            Debug.Log("You dead");
        }

        void nextLevel()
        {
            
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            int nextScene = currentScene + 1;

            if (nextScene == SceneManager.sceneCountInBuildSettings)
            {
                nextScene = 0;
            }
            SceneManager.LoadScene(nextScene);
        }
}

