using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Swarm : MonoBehaviour
{

    [System.Serializable]
    public struct InvaderType
    {
        public string name;
        //public Sprite[] sprites;
        public GameObject invaderPref;
        public int points;
        public int rowCount;
    }

    public static Swarm Instance;

    [Header("Spawning")]
    [SerializeField]
    private InvaderType[] invaderTypes;

    [SerializeField]
    private int columnCount = 11;

    [SerializeField]
    private int ySpacing;

    [SerializeField]
    private int xSpacing;

    [SerializeField]
    private Transform spawnStartPoint;

    [SerializeField]
    private Vector3 invaderScale;

    private float minX;

    private int killCount;
    private System.Collections.Generic.Dictionary<string, int> pointsMap;


    [Space]
    [Header("Movement")]
    [SerializeField]
    private float speedFactor = 10f;

    [SerializeField]
    private float acceleration = 0.5f;

    [SerializeField]
    private float accelerationFrequency = 10f;

    [SerializeField]
    private float movementFrequency = 5f;

    [SerializeField]
    private float minMovementFrequency = 0.1f;

    [SerializeField]
    private Transform playerPos;

    private float minY;
    private float currentY;
    private Transform[,] invadersClassification;
    private int rowCount;
    [HideInInspector]  public bool isMovingRight = true;
    private float maxX;
    private float currentX;
    private float xIncrement;
    [SerializeField]
    private float maxXIncrementation;
    private float accelerationTimer;
    private float movementTimer;

    [SerializeField]
    private EnemyBulletSpawner bulletSpawnerPrefab;

    [HideInInspector] public int numberOfInvader;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        currentY = spawnStartPoint.position.y;
        minY = playerPos.position.y;

        minX = spawnStartPoint.position.x;

        GameObject swarm = new GameObject { name = "Swarm" };
        Vector2 currentPos = spawnStartPoint.position;

        foreach (var invaderType in invaderTypes)
        {
            rowCount += invaderType.rowCount;
        }

        //maxX = minX + 2f * xSpacing * columnCount;
        maxX = minX + maxXIncrementation;

        currentX = minX;
        invadersClassification = new Transform[rowCount, columnCount];

        pointsMap = new System.Collections.Generic.Dictionary<string, int>();

        int rowIndex = 0;
        foreach (var invaderType in invaderTypes)
        {
            var invaderName = invaderType.name.Trim();

            pointsMap[invaderName] = invaderType.points;

            for (int i = 0, len = invaderType.rowCount; i < len; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    GameObject invader = Instantiate(invaderType.invaderPref);
                    //GameObject invader = new GameObject() { name = invaderName };
                    //invader.AddComponent<SimpleAnimation>().sprites = invaderType.sprites;
                    //var shadowCaster = invader.AddComponent<ShadowCaster2D>();
                    //shadowCaster.useRendererSilhouette = false;
                    //shadowCaster.castsShadows = false;

                    //GameObject lightChild = new GameObject("PointLight2D");
                    //var light = lightChild.AddComponent<Light2D>();
                    //light.lightType = (Light2D.LightType)LightType.Point;
                    //light.alphaBlendOnOverlap = false;

                    //lightChild.transform.SetParent(invader.transform);


                    invader.transform.position = currentPos;
                    invader.transform.SetParent(swarm.transform);
                    invader.tag = "Enemy";

                    invadersClassification[rowIndex, j] = invader.transform;

                    currentPos.x += xSpacing;

                    invader.transform.localScale = invaderScale;
                    numberOfInvader++;
                }

                currentPos.x = minX;
                currentPos.y -= ySpacing;

                rowIndex++;
            }
        }

        for (int i = 0; i < columnCount; i++)
        {
            var bulletSpawner = Instantiate(bulletSpawnerPrefab);
            bulletSpawner.transform.SetParent(swarm.transform);
            bulletSpawner.column = i;
            bulletSpawner.currentRow = rowCount - 1;
            bulletSpawner.Setup();
        }

        xIncrement = speedFactor;
        accelerationTimer = accelerationFrequency;
        movementTimer = movementFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        //xIncrement = speedFactor * Time.deltaTime;
        accelerationTimer -= Time.deltaTime;
        if (accelerationTimer <= 0)
        {
            if (movementFrequency >= minMovementFrequency)
            {
                movementFrequency -= acceleration;
                accelerationTimer = accelerationFrequency;
            }

            if (movementFrequency <= minMovementFrequency)
            {
                movementFrequency = minMovementFrequency;
            }
        }

        movementTimer -= Time.deltaTime;
        if (movementTimer <= 0)
        {
            if (isMovingRight)
            {
                currentX += xIncrement;
                if (currentX < maxX)
                {
                    MoveInvaders(xIncrement, 0);
                }
                else
                {
                    ChangeDirection();
                }
            }
            else
            {
                currentX -= xIncrement;
                if (currentX > minX)
                {
                    MoveInvaders(-xIncrement, 0);
                }
                else
                {
                    ChangeDirection();
                }
            }

            movementTimer = movementFrequency;
        }
    }

    private void MoveInvaders(float x, float y)
    {
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                invadersClassification[i, j].Translate(x, y, 0);
            }
        }
    }

    private void ChangeDirection()
    {
        isMovingRight = !isMovingRight;
        MoveInvaders(0, -ySpacing);

        currentY -= ySpacing;
        if (currentY < minY)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }

    public Transform GetInvader(int row, int column)
    {
        if (row < 0 || column < 0 || row >= invadersClassification.GetLength(0) || column >= invadersClassification.GetLength(1))
        {
            return null;
        }
        return invadersClassification[row, column];
    }
    public void IncreaseDeathCount()
    {
        killCount++;
        if (killCount >= invadersClassification.Length)
        {
            GameManager.Instance.TriggerGameOver(false);
            return;
        }
    }

    public int GetPoints(string alienName)
    {
        if (pointsMap.ContainsKey(alienName))
        {
            return pointsMap[alienName];
        }
        return 0;
    }

    //public void AnimationOnDeath(GameObject invader)
    //{
    //    var invaderSpriteRenderer = invader.GetComponent<SpriteRenderer>();
    //    var invaderAlpha = invaderSpriteRenderer.color.a;
    //    for (float i = 1; invaderAlpha > 0; i = i - 0.1f )
    //    {
    //        var color = new Color(invaderSpriteRenderer.color.r, invaderSpriteRenderer.color.g, invaderSpriteRenderer.color.b, i);
    //        invaderSpriteRenderer.color = color;
    //    }

    //    if (invaderAlpha <= 0)
    //    {
    //        invaderSpriteRenderer.enabled = false;
    //    }
    //}

}
