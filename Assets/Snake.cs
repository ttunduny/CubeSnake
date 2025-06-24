using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using System.Collections;

public class Snake : MonoBehaviour
{

    public float moveSpeed = 3f;
    public float bodyMoveSpeed = 3f;
    public float rotateSpeed = 180f;
    public int gap = 20;
    public float blendSpeed = 2f;

    public GameObject snakeBodyPrefab;
    public GameObject foodPrefab;

    public Material snakeMaterial;
    public Color currentColor;

    private List<GameObject> snakeBodyParts = new List<GameObject>();
    private List<Vector3> previousPositions = new List<Vector3>();

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public int score = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get the Snake Renderer
        currentColor = snakeMaterial.color;

        SpawnFood();
        int highScore = PlayerPrefs.GetInt("highscore", 0);
        highScoreText.text = "Highest: " + highScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameManagerInstance.isGameRunning()==false)
        {
            return;
        }
        //Move the snake
        transform.position += (transform.forward * moveSpeed * Time.deltaTime);


        float rotation = Input.GetAxis("Horizontal");

        //Rotate the snake
        transform.Rotate(Vector3.up * rotateSpeed * rotation *  Time.deltaTime);

        previousPositions.Insert(0, transform.position);

        //Move Children

        // Move body parts
        int index = 0;
        foreach (var body in snakeBodyParts)
        {
            Vector3 point = previousPositions[Mathf.Clamp(index * gap, 0, previousPositions.Count - 1)];

            // Move body towards the point along the snakes path
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * bodyMoveSpeed * Time.deltaTime;

            // Rotate body towards the point along the snakes path
            body.transform.LookAt(point);

            index++;
        }

        scoreText.text = score.ToString();

        

    }

    private void GrowSnake()
    {
        Vector3 spawnPosition;

        if (snakeBodyParts.Count == 0)
        {
            spawnPosition = transform.position; // Head's position if first part
        }
        else
        {
            spawnPosition = snakeBodyParts[snakeBodyParts.Count - 1].transform.position;
        }

        GameObject bodyPart = Instantiate(snakeBodyPrefab, spawnPosition, Quaternion.identity);
        snakeBodyParts.Add(bodyPart);

        //Make Unique Materials for Each Body Part

        Renderer bodyRenderer = bodyPart.GetComponentInChildren<Renderer>();
        Material newMat =  new Material(bodyRenderer.sharedMaterial);
        bodyRenderer.material = newMat;

        //Increment Score
        score++;

        //Check if its HighScore then Update
        int highScore = PlayerPrefs.GetInt("highscore",0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("highscore", score);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Food")
        {
            GameManager.gameManagerInstance.PlayEatingSound();
            GrowSnake();
            //Get the Material Color

            //Change Snake Material Color
            Renderer foodRenderer = other.gameObject.GetComponent<Renderer>();
            Material foodMat = foodRenderer.material;
            currentColor = foodMat.color;
            ChangeColor();

            Destroy(other.gameObject);
            SpawnFood();

        }
    }



    private void ChangeColor()
    {
        StopAllCoroutines();
        StartCoroutine(LerpToColor(snakeMaterial, currentColor, 0.1f));

        foreach(var body in snakeBodyParts)
        {
            Renderer r = body.GetComponentInChildren<Renderer>();
            
            Material mat = r.material;
            StartCoroutine(LerpToColor(mat, currentColor, 0.1f));

        }
    }
    private IEnumerator LerpToColor(Material materialInstance, Color targetColor, float delay)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        Color startColor = materialInstance.color;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * blendSpeed;
            materialInstance.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        materialInstance.color = targetColor; // Snap to target to avoid precision issues
    }

    private void SpawnFood()
    {
        int maxAttempts = 100;
        float checkRadius = 0.5f;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            float x = Random.Range(-8f, 8f);
            float z = Random.Range(-8f, 8f);
            Vector3 pos = new Vector3(x, 0, z);


            // Check against head
            if (Vector3.Distance(transform.position, pos) < checkRadius)
                continue;

            // Check against all body parts
            bool overlaps = false;
            foreach (var body in snakeBodyParts)
            {
                if (Vector3.Distance(body.transform.position, pos) < checkRadius)
                {
                    overlaps = true;
                    break;
                }
            }

            if (overlaps)
                continue;

            // Passed all checks, spawn the food
            GameObject food = Instantiate(foodPrefab);
            food.transform.position = pos;
            return;

        }


    }
}
