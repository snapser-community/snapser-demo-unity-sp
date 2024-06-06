using UnityEngine;
using Utilities;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Collider))]
public class Player : MonoBehaviour
{
    [SerializeField] private Transform playerStartPositionTransform;
    [SerializeField] private Animator playerAnimator;
    // Movement speed
    [SerializeField] float speed = 2;
    // Flap force
    [SerializeField] float upForce = 25;
    [SerializeField] float downForce = 50;

    
    public string GetSavedColor => PlayerPrefs.GetString(GameConstants.PlayerColor);
    public float PlayerStartXPosition { get; set; } = 0f;
    public float DistanceTraveled => transform.position.x - PlayerStartXPosition;
    
    Rigidbody2D _rBody;
    private float _gScale;

    private static Player _instance;
    public static Player Instance
    {
        get
        {
            if (_instance == null) _instance = GameObject.FindObjectOfType<Player>();
            return _instance;
        }
    }
    
    private void Start()
    {
        _rBody = GetComponent<Rigidbody2D>();
        _gScale = _rBody.gravityScale;
        
        InitializePlayer();
    }
    
    void FixedUpdate() 
    {
        if (GameManager.Instance.IsInStartMenu)
        {
            _rBody.gravityScale = 0;
            return;
        }

        _rBody.gravityScale = _gScale;
        
        // Move towards the right
        _rBody.velocity = Vector2.right * speed;
        
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
           _rBody.AddForce(Vector2.up * upForce);
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            _rBody.AddForce(Vector2.down * downForce);
        
        // if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        //     _rBody.AddForce(Vector2.up * upForce);
        // else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        //     _rBody.velocity = Vector2.down * downForce;
    }

    public void InitializePlayer()
    {
        Transform t = transform;
        t.position = playerStartPositionTransform.position;
        PlayerStartXPosition = t.position.x;
    }

    public void SetPlayerColor(bool isBlue = true)
    {
        string color = isBlue ? GameConstants.PlayerColorBlue : GameConstants.PlayerColorRed;
        // _spriteRenderer.sprite = isBlue ? blueSprite : redSprite;
        playerAnimator.SetBool("playRedAnimation", !isBlue);
        PlayerPrefs.SetString(GameConstants.PlayerColor, color);
    }
    
    void OnCollisionEnter2D(Collision2D coll)
    {
        _rBody.velocity = Vector2.zero;
        GameManager.Instance.EndRun(DistanceTraveled);
        InitializePlayer();
    }
}
