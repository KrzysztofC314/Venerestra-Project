using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance;
    public Transform Target;
    public float acceleration = .5f;
    public float damp = .8f;
    public float maxSpeed = 1.5f;
    public float cameraConstraintOffset = .5f;
    private Camera _camera;
    internal Rigidbody2D Rigidbody;
    private float furthestPlace;
    public float[] constraints;
    private int bgWidth = 2120; //= _camera.pixelHeight * 4 = _camera.pixelHeight * width / 200
    private int bgHeight = 795; //= _camera.pixelHeight * 3/2 = _camera.pixelHeight * height / 200
    public int width = 800;
    public int height = 300;
    public float targetX;
    public float targetConstrainedX;
    public float constantY = 0;
    private float scaledHeight => _camera.orthographicSize * _camera.pixelHeight;
    private float scaledWidth => _camera.orthographicSize * _camera.pixelWidth;

    private Action Run;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _camera = GetComponent<Camera>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Initialize();
        Run = BaseUpdate;
    }

    void Update()
    {
        Run();
    }

    private void Initialize()
    {
        Debug.Log($"size: {_camera.orthographicSize}, height: {scaledHeight} ({_camera.pixelHeight}), width: {scaledWidth} ({_camera.pixelWidth})");
        bgWidth = _camera.pixelHeight * width / 200;
        bgHeight = _camera.pixelHeight * height / 200;
        _camera.orthographicSize = bgHeight / (float)_camera.scaledPixelHeight;
        Debug.Log($"size: {_camera.orthographicSize}, height: {scaledHeight} ({_camera.pixelHeight}), width: {scaledWidth} ({_camera.pixelWidth})");
        Debug.Log($"bg: {bgWidth}x{bgHeight}");
        furthestPlace = Math.Abs(bgWidth - scaledWidth) / _camera.pixelHeight + cameraConstraintOffset;
        Debug.Log($"furthestPlace: {furthestPlace}");
        constraints = new[] { -furthestPlace, furthestPlace };
        ForceInstantAdjust();
    }

    public void Initialize(int width, int height)
    {
        this.width = width;
        this.height = height;
        Initialize();
    }

    void BaseUpdate()
    {
        targetX = Target.position.x;
        var target = AddConstraints(Target.position);
        targetConstrainedX = target.x;
        var distance = Distance(transform.position, target);
        if (distance > 0)
        {
            Rigidbody.velocity = UnitizeVector(target - transform.position) * maxSpeed * Math.Min(damp, distance) / damp;
        }
        else
        {
            Rigidbody.velocity = Vector3.zero;
        }
        AddConstraints(transform);
    }

    public void Follow(Transform transform)
    {
        Target = transform;
        Run = BaseUpdate;
    }

    public void Unfollow()
    {
        Run = () => { };
    }

    public void StopAndSetPosition(float x, float y)
    {
        Rigidbody.velocity = Vector3.zero;
        transform.position = new Vector3(x, y, transform.position.z);
    }

    public void StopAndSetPosition(Vector3 position)
    {
        StopAndSetPosition(position.x, position.y);
    }

    float Distance(Vector3 vector1, Vector3 vector2)
    {
        float x = vector1.x - vector2.x;
        float y = vector1.y - vector2.y;
        return (float)Math.Sqrt(x * x + y * y);
    }

    Vector3 UnitizeVector(Vector3 vector)
    {
        if (vector.x == 0 && vector.y == 0)
            return new Vector3(0, 0, vector.z);
        float modulus = (float)Math.Sqrt(vector.x * vector.x + vector.y * vector.y);
        return new Vector3(vector.x / modulus, vector.y / modulus, vector.z);
    }

    private Vector3 AddConstraints(Transform transform)
    {
        var vector = AddConstraints(transform.position);
        if (vector.x != transform.position.x)
        {
            transform.position = vector;
        }
        return vector;
    }

    private Vector3 AddConstraints(Vector3 vector)
    {
        float x = Math.Max(vector.x, constraints[0]);
        x = Math.Min(x, constraints[1]);
        if (x == vector.x)
            return new Vector3(vector.x, constantY, vector.z);
        return new Vector3(x, constantY, vector.z);
    }

    public void ForceInstantAdjust()
    {
        transform.position = AddConstraints(new Vector3(Target.position.x, Target.position.y, transform.position.z));
    }
}
