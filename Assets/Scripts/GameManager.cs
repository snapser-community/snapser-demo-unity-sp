using Obstacles;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string clusterUrlOverride;
    
    private SnapserManager _snapserManager;

    public bool IsInStartMenu { get; private set ; }
    
    public static event UnityAction OnGameStarted;
    public static event UnityAction<float> OnGameRunEnded;
    public static event UnityAction OnGameReset;
    
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null) 
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    #region Game Object Lifecycle 
    
    private void Awake()
    {
        string cUrl = clusterUrlOverride.IsEmptyOrNull() ? GameConstants.DefaultClusterURL : clusterUrlOverride;
        _snapserManager = new SnapserManager(cUrl);
    }

    private void Start()
    {
        InitializeGame(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        if ((Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand)) && Input.GetKey(KeyCode.R) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            _snapserManager.ResetSnapserServices();
            InitializeGame(false, true);
            OnGameReset?.Invoke();
        }
    }
    
    #endregion
    
    #region Game Lifecycle 

    public void InitializeGame(bool initializeLevel = false, bool deletePlayerPrefs = false)
    {
        IsInStartMenu = true;
        
        if (deletePlayerPrefs)
            PlayerPrefs.DeleteAll();
        
        if (initializeLevel)
           ObstacleManager.Instance.InitializeLevel();
    }

    void StartGame()
    {
        IsInStartMenu = false;
        OnGameStarted?.Invoke();
    }

    public void EndRun(float score)
    {
        InitializeGame();
        OnGameRunEnded?.Invoke(score);
    }
    
    #endregion
    
}