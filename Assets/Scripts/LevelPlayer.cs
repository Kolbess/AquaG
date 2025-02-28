using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPlayer : MonoBehaviour
{
    public GameObject[] objectsToSpawn; // List of objects to spawn in waves
    public float waveInterval = 5f; // Time between waves
    public float npcInterval = 6f;
    public float spawnInterval = 1f; // Time between spawning each object in a wave
    public int objectsPerWave = 5; // Number of objects to spawn per wave
    private float waveTimer = 0f;
    private float spawnTimer = 0f;
    private int objectsSpawned = 0;
    private bool isSpawningWave = false;

    [SerializeField] private TimeManager levelTimeManager;
    [SerializeField] private ScoreManager levelScoreManager;
    
    public TextMeshProUGUI levelInstructionsText; // Reference to the UI Text for level instructions
    public TextMeshProUGUI educationalMessageText; // Reference to the UI Text for educational message

    private bool levelStarted = false;
    private bool levelCompleted = false;
    private float levelStartTime = 0f;
    public float LevelDurationTime = 60.0f;
    public string currentLevelname;
    public AudioMangaer audiomanager;

    private void Start()
    {
        levelTimeManager.timer = LevelDurationTime;
        // Ensure both texts are initially hidden
        currentLevelname = SceneManager.GetActiveScene().name;
        levelInstructionsText.transform.parent.gameObject.SetActive(false);
        levelInstructionsText.gameObject.SetActive(false);
        if (currentLevelname != "EndGame")
        {
            educationalMessageText.transform.parent.gameObject.SetActive(false);
            educationalMessageText.gameObject.SetActive(false);
        }

        Debug.Log(currentLevelname);
        // Show instructions at the start of the level
        ShowLevelInstructions();
        audiomanager = FindObjectOfType<AudioMangaer>();
        if (currentLevelname == "Level1")
        {
            audiomanager.Play("Ethan Sturock - Summer Time (freetouse.com)");
        } else if (currentLevelname == "Level2")
        {
            audiomanager.Play("Aylex - Spring (freetouse.com)");
        }
    }
    private void Update()
    {
        if (currentLevelname == "Level1")
        {
            if (Input.GetKey(KeyCode.E) && !levelStarted)
            {
                HideLevelInstructions();
            }

            // Wait for the wave interval before starting a new wave
            if (levelTimeManager.timer > 0.0f && levelTimeManager.startTimer)
            {
                if (!isSpawningWave)
                {
                    waveTimer += Time.deltaTime;
                    if (waveTimer >= waveInterval)
                    {
                        StartWave();
                    }
                }
                else
                {
                    // Spawn objects at regular intervals during the wave
                    spawnTimer += Time.deltaTime;
                    if (spawnTimer >= spawnInterval && objectsSpawned < objectsPerWave)
                    {
                        SpawnObject();
                        spawnTimer = 0f; // Reset spawn timer
                    }
                }
            }

            if (CheckLevelCompletion() && !levelCompleted)
            {
                CompleteLevel();
            }

            if (levelCompleted)
            {
                Debug.Log(Input.GetKey(KeyCode.E));
                if (Input.GetKey(KeyCode.E))
                {
                    TransitionToNextLevel();
                }
            }
        } else if (currentLevelname == "Level2")
        {
            if (Input.GetKey(KeyCode.E) && !levelStarted)
            {
                HideLevelInstructions();
            }

            if (levelTimeManager.timer > 0.0f && levelTimeManager.startTimer)
            {
                if (!isSpawningWave)
                {
                    waveTimer += Time.deltaTime;
                    if (waveTimer >= npcInterval)
                    {
                        SpawnNpc();
                        waveTimer = 0.0f;
                    }
                }
            }
            if (CheckLevelCompletion() && !levelCompleted)
            {
                CompleteLevel();
            }

            if (levelCompleted)
            {
                Debug.Log(Input.GetKey(KeyCode.E));
                if (Input.GetKey(KeyCode.E))
                {
                    TransitionToNextLevel();
                }
            }
        } else if (currentLevelname == "EndGame")
        {
            if (Input.GetKey(KeyCode.E))
            {
                TransitionToNextLevel();
            }
        }
    }

    private void StartWave()
    {
        isSpawningWave = true;
        waveTimer = 0f; // Reset wave timer for next wave
        objectsSpawned = 0; // Reset object counter for the wave
    }

    private void SpawnObject()
    {
        // Spawn a random object from the objectsToSpawn array
        GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
        Instantiate(objectToSpawn, transform.position, Quaternion.identity); // Spawn at the spawner's position
        objectsSpawned++; // Increment the spawned object counter
        
        if (objectsSpawned >= objectsPerWave)
        {
            isSpawningWave = false; // Wave is complete
        }
    }

    private void SpawnNpc() //Level 2 func
    {
        Instantiate(objectsToSpawn[0]);
    }
    
    private void ShowLevelInstructions()
    {
        levelInstructionsText.gameObject.SetActive(true); // Show the level instructions
        levelInstructionsText.transform.parent.gameObject.SetActive(true); // Show panel behind
        if (currentLevelname == "Level1")
        {
            Debug.Log("Level1 Info");
            levelInstructionsText.text =
                "Welcome to Level 1! Clean up the river by removing trash and stopping pollution. Use WASD to move and Hold Space Bar to interact.<br><br>Press 'E' to start."; // Set the message
        } else if (currentLevelname == "Level2")
        {
            levelInstructionsText.text =
                "Welcome to Level 2!<br>The lake is in danger—toxic algae and illegal dumping are polluting the water." +
                " Remove algae blooms and stop polluters while staying on the move! <br><br>Use WASD to move and Hold Space Bar to interact. <br><br><br>Press 'E' to start.";
            Debug.Log("Level2 Info");
        }
    }
    
    private bool CheckLevelCompletion()
    {
        if (levelTimeManager.timer <= 0.0f)
        {
            return true;
        }
        return false;
    }
    
    private void CompleteLevel()
    {
        // if (currentLevelname == "Level1")
        // {
        //     audiomanager.Stop("Ethan Sturock - Summer Time (freetouse.com");
        // } else if (currentLevelname == "Level2")
        // {
        //     audiomanager.Stop("Aylex - Spring (freetouse.com)");
        // }
        Scores.currentscore += ScoreManager.instance.score;
        Time.timeScale = 0f;
        levelCompleted = true;
        ShowEducationalMessage();
    }
    
    private void ShowEducationalMessage()
    {
        educationalMessageText.gameObject.SetActive(true); // Show educational message
        educationalMessageText.transform.parent.gameObject.SetActive(true);
        if (currentLevelname == "Level1")
        {
            educationalMessageText.text =
                "Well done, Aqua Guardian!<br> This river provides drinking water and a home for many animals," +
                " but pollution makes it dangerous for both. Trash and chemicals in rivers can poison fish, harm wildlife," +
                " and even reach our homes. To keep rivers clean, we must all help—throw waste in the right place," +
                " use less plastic, and avoid pouring chemicals down the drain.<br> Every small action protects our water!" +
                "<br><br>Press 'E' To continue.";
        } else if (currentLevelname == "Level2")
        {
            educationalMessageText.text =
                "Well done, Aqua Guardian!<br> Lakes store fresh water for people and wildlife, but pollution threatens them." +
                " Toxic algae blooms grow when waste and chemicals enter the water, making it unsafe for fish, animals, and even us." +
                " Stopping illegal dumping and reducing pollution helps keep lakes clean and healthy." +
                " <br>Every action matters—protect our water! <br><br>Press 'E' to continue.";

        } else if (currentLevelname == "EndGame")
        {
            
        }
    }
    
    private void TransitionToNextLevel()
    {
        Debug.Log("Transitioning");
        LoadNextScene();
    }
    
    private void LoadNextScene()
    {
        Debug.Log("Loading Next Scene");
        if (currentLevelname == "Level1")
        {
            SceneManager.LoadScene("Level2");
            currentLevelname = "Level2";
        } else if (currentLevelname == "Level2")
        {
            SceneManager.LoadScene("EndGame"); 
            currentLevelname = "EndGame";
        } else if (currentLevelname == "EndGame")
        {
            SceneManager.LoadScene("StartMenu");
            currentLevelname = "StartMenu";
        }
    }
    
    private void HideLevelInstructions()
    {
        Time.timeScale = 1f;
        levelInstructionsText.gameObject.SetActive(false); // Hide Text
        levelInstructionsText.transform.parent.gameObject.SetActive(false); // Hide Panel
        levelStarted = true; // Mark level as started
        levelTimeManager.startTimer = true;
    }
}
