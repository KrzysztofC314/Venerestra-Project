using System;
using UnityEngine;

public class PlayerMovement : PlayableObject
{
    public static PlayerMovement Instance;
    public float Velocity = 2;
    public float powerIncreasingSpeed = 0.1f;
    public float exhaustionSpeed = 0.1f;
    public float increasePowerQty = 30f;
    private Animator Animator;
    private Rigidbody2D Rigidbody;
    private int maxPhysicalPower = 100;
    private int maxSocialBattery = 100;
    private float physicalPower = 100f;
    private float socialBattery = 100f;
    private float powerIncreasingTime;
    private bool eating;
    public LayerMask itemsLayer;
    public float collisionWidth = .2f;
    public float collisionHeight => _collisionHeight ??= GameController.Instance.resolutionY / 100f;
    private float? _collisionHeight;
    public bool isHeadedLeft;
    public bool isMoving;
    public bool isTurnedBack;
    private bool isSitting;
    public LayerMask clickableItemsLayer;
    public GameObject thoughtPrefab;
    public override Vector3 cameraSpawnPosition => GetCameraSpawnPosition();

    private Vector3 GetCameraSpawnPosition()
    {
        return new(transform.position.x, Camera.main.transform.position.y);
    }

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        CameraFollows = true;
        Run = BaseUpdate;
    }

    void BaseUpdate()
    {
        float direction = Input.GetAxisRaw("Horizontal");
        if (direction > 0)
            GoRight();
        else if (direction < 0)
            GoLeft();
        else if (Math.Abs(Rigidbody.velocity.x) < 0.01)
            StandIdle();
        if (Input.GetKeyDown(KeyCode.E))
            EatIfPossible();
        if (Input.GetKeyDown(GameController.useItemKey))
            UseItem();
        /*var collider = Physics2D.OverlapBox(transform.position, new(collisionWidth, collisionHeight), itemsLayer);
        if (collider != null)
            collider.GetComponent<ItemBehaviour>().Activate();
        else 
            ItemBehaviour.Deactivate();*/
        var clickableItem = Physics2D.OverlapPoint(usableCamera.ScreenToWorldPoint(Input.mousePosition), clickableItemsLayer);
        if (clickableItem != null)
            clickableItem.GetComponent<ClickableItem>().Activate();
        else
            ClickableItem.Deactivate();
        if (Input.GetMouseButtonDown(0))
            UseClickableItem();
        SetAnimation(isHeadedLeft, isMoving, isTurnedBack, isSitting);
    }

    private void UseClickableItem()
    {
        if (ClickableItem.ActivatedItem != null)
            Prompt(ClickableItem.ActivatedItem.afterClickPrompt);
    }

    public void TurnBack(float time)
    {
        turnedBackTimer = time;
        isTurnedBack = true;
        isMoving = false;
        leftMirrorZone = false;
        Run += BeTurnedBack;
    }

    public bool leftMirrorZone = false;
    private float turnedBackTimer;

    private Action BeTurnedBack => () =>
    {
        turnedBackTimer -= Time.deltaTime;
        if (turnedBackTimer <= 0 || leftMirrorZone)
        {
            isTurnedBack = false;
            Run -= BeTurnedBack;
            leftMirrorZone = false;
        }
    };

    public void Prompt(string prompt) 
    {
        Prompt(prompt, 5);
    }

    public void Prompt(string prompt, float timeShown)
    {
        Thought.secondsShown = timeShown;
        var promptObject = Instantiate(thoughtPrefab, Camera.main.transform);
        promptObject.GetComponent<Thought>().Initialize(prompt);
        /*var scale = transform.localScale.x;
        promptObject.transform.localScale = new(1 / scale, 1 / scale);*/
    }

    void GoRight()
    {
        Go(1);
    }

    void GoLeft()
    {
        Go(-1);
    }

    void Go(int direction)
    {
        direction = Math.Sign(direction);
        if (Exhaust())
        {
            isHeadedLeft = direction == -1;
            isMoving = true;
            isSitting = false;
            Rigidbody.velocity = new Vector3(direction * Velocity, 0, 0);
            Debug.Log($"si³a fizyczna: {physicalPower}");
        }
        else
        {
            isMoving = false;
            Debug.Log($"za ma³o si³y fizycznej");
        }
    }

    void StandIdle()
    {
        isMoving = false;
    }

    bool Exhaust()
    {
        if (physicalPower <= 0)
            return false;
        physicalPower -= Time.fixedDeltaTime * exhaustionSpeed;
        return true;
    }

    void EatIfPossible()
    {
        if (!eating && physicalPower < maxPhysicalPower)
            Eat();
    }

    void Eat()
    {
        eating = true;
        Animator.SetTrigger("eat");
        powerIncreasingTime = increasePowerQty / powerIncreasingSpeed;
        Run += IncreasePower();
    }

    Action IncreasePower() => IncreasePower(powerIncreasingTime, powerIncreasingSpeed);

    Action IncreasePower(float increasingTime, float increasingSpeed)
    {
        powerIncreasingTime = increasingTime;
        return () =>
        {
            Debug.Log($"time: {powerIncreasingTime}, power: {physicalPower}");
            if (physicalPower < maxPhysicalPower && powerIncreasingTime > 0)
            {
                var time = Time.fixedDeltaTime;
                powerIncreasingTime -= time;
                physicalPower += time * increasingSpeed;
                Debug.Log($"Delta time: {time}");
            }
            else
            {
                eating = false;
                powerIncreasingTime = 0;
                physicalPower = Math.Min(physicalPower, maxPhysicalPower);
                Run = BaseUpdate;
            }
        };
    }

    private void NotPlaying() { }

    public override void Play()
    {
        base.Play();
        Run = BaseUpdate;
    }

    public override void Stop()
    {
        TurnOff();
    }

    private void SetAnimation(bool isHeadedLeft = false, bool isMoving = false, bool isTurnedBack = false, bool isSitting = false)
    {
        Animator.SetBool("isHeadedLeft", isHeadedLeft);
        Animator.SetBool("isMoving", isMoving);
        Animator.SetBool("isTurnedBack", isTurnedBack);
        Animator.SetBool("isSitting", isSitting);
    }
}


