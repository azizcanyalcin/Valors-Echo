using UnityEngine;

public class Lever : InteractableObject
{
    [SerializeField] private GameObject platform;
    [SerializeField] private float speed;
    [SerializeField] private float startY;
    [SerializeField] private float endY;

    private Vector3 startPosition;
    private Vector3 endPosition;
    [SerializeField] private bool movingToEnd = true;
    [SerializeField] private bool movingToStart = false;
    private bool canPlatformMove = false;

    protected override void Start()
    {
        base.Start();

        startPosition = new Vector3(platform.transform.position.x, startY, platform.transform.position.z);
        endPosition = new Vector3(platform.transform.position.x, endY, platform.transform.position.z);

        platform.transform.position = startPosition;
    }

    void Update()
    {
        if (canPlatformMove)
            MovePlatform();
    }

    void MovePlatform()
    {
        if (movingToEnd)
        {
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, endPosition, speed * Time.deltaTime);

            if (Vector3.Distance(platform.transform.position, endPosition) < 0.01f)
            {
                movingToEnd = false;
            }
        }
        if (movingToStart)
        {
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, startPosition, speed * Time.deltaTime);

            if (Vector3.Distance(platform.transform.position, startPosition) < 0.01f)
            {
                movingToEnd = true;
            }
        }
    }
    protected override void OnTriggerStay2D(Collider2D other)
    {
        base.OnTriggerStay2D(other);
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }
    protected override void OpenObject()
    {
        base.OpenObject();
        canPlatformMove = true;
    }
}
