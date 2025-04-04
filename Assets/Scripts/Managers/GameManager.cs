using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Gamemode Settings")]
    [SerializeField] private float timeToComplete = 300;

    [Header("Runtime Gamemode Stats")]
    [SerializeField] private float currentTime = 0;
    [SerializeField] private bool gamemodeRunning;

    [Header("Gamemode Objects")]
    [SerializeField] private UIManager uiManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(Instance);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            var objective = FindAnyObjectByType<EnterObjective>();
            if (objective != null)
            {
                objective.OnObjectiveEnter += CompleteGame;
            }
        }
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            if(uiManager == null)
            {
                uiManager = FindAnyObjectByType<UIManager>();
            }

            if(currentTime < timeToComplete)
            {
                gamemodeRunning = true;
                currentTime += Time.deltaTime;
            }

            if(currentTime >= timeToComplete)
            {
                CompleteGame();
            }
        }
    }

    private void CompleteGame()
    {
        gamemodeRunning = false;
        currentTime = 0;
        ChangeScene(0);
    }

    public void ChangeScene(int sceneIndex)
    {
        uiManager = null;
        SceneManager.LoadScene(sceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public float CurrentTime
    {
        get { return currentTime; }
        private set { currentTime = value; }
    }
    public float TimeToComplete
    {
        get { return timeToComplete; }
        private set { timeToComplete = value; }
    }

    public bool GamemodeRunning {
        get { return gamemodeRunning; } 
        private set { gamemodeRunning = value; } 
    }

}
