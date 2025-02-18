using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Para cambiar de escena

public class PlayerController_Script : MonoBehaviour
{
    // Variables de movimiento
    public float speed;
    public float mouseSensitivity;
    public float scaleStep;
    
    private float rotationX = 0f;
    private float rotationY = 0f;

    // UI
    public Text textPosition;
    public Text textRotation;
    public Text textScale;

    // Variables para colisiones
    public AudioSource coinSound; // Sonido al recoger la moneda
    public Transform respawnPoint; // Punto donde reaparece el personaje
    public GameObject door; // Puerta que aparecerá tras eliminar un objeto

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MovePlayer();
        RotatePlayer();
        ScalePlayer();
        UpdateUI();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.Translate(moveX, 0, moveZ);
    }

    void RotatePlayer()
    {
        rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);
        transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
    }

    void ScalePlayer()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            transform.localScale += Vector3.one * scaleStep;
        }
          
        if (Input.GetKeyDown(KeyCode.M))
        {
            transform.localScale -= Vector3.one * scaleStep;
            transform.localScale = new Vector3(
                Mathf.Max(transform.localScale.x, 0.2f), 
                Mathf.Max(transform.localScale.y, 0.2f), 
                Mathf.Max(transform.localScale.z, 0.2f)
            );
        }
    }

    void UpdateUI()
    {
        if (textPosition != null && textRotation != null && textScale != null)
        {
            textPosition.text = $"Posición\nX: {transform.position.x:F2}\nY: {transform.position.y:F2}\nZ: {transform.position.z:F2}"; 
            textRotation.text = $"Rotación\nX: {transform.rotation.eulerAngles.x:F2}\nY: {transform.rotation.eulerAngles.y:F2}"; 
            textScale.text = $"Escala\nX: {transform.localScale.x:F2}\nY: {transform.localScale.y:F2}\nZ: {transform.localScale.z:F2}"; 
        }
    }

    // Función para detectar colisiones con objetos específicos
    private void OnTriggerEnter(Collider other)
    {
        // Si colisiona con la moneda
        if (other.gameObject.CompareTag("Coin"))
        {
            if (coinSound != null)
            {
                coinSound.Play(); // Reproduce sonido
            }
            Destroy(other.gameObject); // Destruye la moneda
            RespawnPlayer(); // Mueve el personaje a otra posición
        }

        // Si colisiona con un objeto trigger que debe desaparecer y hacer aparecer la puerta
        if (other.gameObject.CompareTag("Enemy")) // Cambia "Enemy" por el tag correcto
        {
            Destroy(other.gameObject); // Elimina el objeto trigger
            if (door != null)
            {
                door.SetActive(true); // Activa la puerta
            }
        }

        // Si colisiona con la puerta, carga el menú principal
        if (other.gameObject.CompareTag("Door")) // Asegúrate de que la puerta tiene este tag
        {
            SceneManager.LoadScene("MainMenu"); // Cambia "MainMenu" por el nombre de tu escena
        }
    }

    // Mueve el personaje a otra posición después de recoger la moneda
    private void RespawnPlayer()
    {
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position; // Teletransporta al personaje
        }
    }
}
