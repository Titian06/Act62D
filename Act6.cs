using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    public GameObject circlePrefab; // Prefab del círculo a instanciar
    public Transform spawnPosition; // Punto de generación sobre el contenedor
    public GameObject[] containerBarriers; // Barreras laterales
    public int maxCircles = 10; // Número máximo de círculos antes de desaparecer uno
    public float minSpawnInterval = 2f; // Intervalo mínimo para la aparición de círculos
    public float maxSpawnInterval = 10f; // Intervalo máximo para la aparición de círculos
    public float spawnRadius = 5f; // Radio del área de generación de círculos

    private Queue<GameObject> circleQueue = new Queue<GameObject>();
    private int generatedCircleCount = 0;

    void Start()
    {
        InvokeRepeating("SpawnCircle", 0f, Random.Range(minSpawnInterval, maxSpawnInterval));
    }

    void Update()
    {
        // Acciones para cuando hay círculos disponibles (mover y acumular círculos)

    }

    private void SpawnCircle()
    {
        if (generatedCircleCount >= maxCircles)
        {
            // Elimina un círculo aleatorio
            if (circleQueue.Count > 0)
            {
                GameObject circleToRemove = circleQueue.Dequeue();
                Destroy(circleToRemove);
                generatedCircleCount--;
            }
        }

        // Generar posición aleatoria en el plano horizontal
        Vector3 randomOffset = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius));
        Vector3 randomPosition = spawnPosition.position + randomOffset;

        GameObject circle = InstantiateCircle(randomPosition);
        circleQueue.Enqueue(circle);
        generatedCircleCount++;
    }

    private GameObject InstantiateCircle(Vector3 position)
    {
        GameObject circle = Instantiate(circlePrefab, position, Quaternion.identity);
        Rigidbody2D rigidbody = circle.GetComponent<Rigidbody2D>();
        if (rigidbody != null)
        {
            rigidbody.AddForce(Vector2.down * 200f, ForceMode2D.Force); //Fuerza de caida del circulo
        }

        return circle;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Si un círculo colisiona con una de las barreras, destruirlo
        if (collision.gameObject.CompareTag("Circle"))
        {
            Destroy(collision.gameObject);
            generatedCircleCount--;
        }
    }
}