using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Necesario para TextMeshPro

public class WorldSpaceDialogueSystem : MonoBehaviour
{
    public static WorldSpaceDialogueSystem Instance { get; private set; }

    [Header("Settings")]
    [Tooltip("Prefab opcional con un TextMeshPro. Si se deja vacío, se creará uno por código.")]
    [SerializeField] private GameObject textPrefab; 
    [SerializeField] private float typingSpeed = 0.05f;

    private TextMeshPro dialogueTextComponent;
    private GameObject dialogueObject;
    
    private Coroutine typingCoroutine;
    private List<string> currentLines;
    private int currentLineIndex = 0;
    private bool isTyping = false;

    private void Awake()
    {
        // Configuración del Singleton para fácil acceso
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        InitializeDialogueObject();
    }

    private void InitializeDialogueObject()
    {
        if (textPrefab != null)
        {
            dialogueObject = Instantiate(textPrefab);
            dialogueTextComponent = dialogueObject.GetComponentInChildren<TextMeshPro>();
        }
        else
        {
            // Instanciar el objeto desde cero si no hay prefab
            dialogueObject = new GameObject("WorldSpaceDialogueText");
            dialogueTextComponent = dialogueObject.AddComponent<TextMeshPro>();
            
            // Configuración básica para que sea visible en World Space
            dialogueTextComponent.alignment = TextAlignmentOptions.Center;
            dialogueTextComponent.fontSize = 5;
            dialogueTextComponent.color = Color.white;
            
            // Añadir un RectTransform con tamaño predeterminado
            RectTransform rect = dialogueTextComponent.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(10, 5);
        }

        // Hacerlo hijo de este manager para mantener limpia la jerarquía (Opcional)
        dialogueObject.transform.SetParent(transform);

        // Desactivar el render inicialmente
        SetRenderActive(false);
    }

    /// <summary>
    /// Activa o desactiva el renderizado del texto.
    /// </summary>
    public void SetRenderActive(bool isActive)
    {
        if (dialogueTextComponent != null)
        {
            // Solo desactivamos el componente renderer para no romper otras lógicas si las hubiera
            dialogueTextComponent.enabled = isActive;
        }
    }

    /// <summary>
    /// Función principal que instancia/reposiciona el texto y comienza a mostrar las líneas.
    /// </summary>
    public void PlayDialogue(List<string> linesToDisplay, Vector3 position)
    {
        if (linesToDisplay == null || linesToDisplay.Count == 0) return;

        // Reposicionar el objeto
        dialogueObject.transform.position = position;

        // Activar el render
        SetRenderActive(true);

        currentLines = linesToDisplay;
        currentLineIndex = 0;

        ShowNextLine();
    }

    /// <summary>
    /// Muestra la siguiente línea o termina la línea actual de golpe si se está escribiendo.
    /// </summary>
    public void ShowNextLine()
    {
        // Si está escribiendo, forzamos a que termine la línea actual inmediatamente
        if (isTyping)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            dialogueTextComponent.text = currentLines[currentLineIndex - 1];
            isTyping = false;
            return;
        }

        // Si hay más líneas, mostramos la siguiente
        if (currentLineIndex < currentLines.Count)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeText(currentLines[currentLineIndex]));
            currentLineIndex++;
        }
        else
        {
            // Terminó el diálogo
            SetRenderActive(false);
            dialogueTextComponent.text = string.Empty;
        }
    }

    /// <summary>
    /// Corrutina para lograr el efecto de máquina de escribir.
    /// </summary>
    private IEnumerator TypeText(string line)
    {
        isTyping = true;
        dialogueTextComponent.text = "";

        // Mostramos letra por letra
        foreach (char letter in line.ToCharArray())
        {
            dialogueTextComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    /// <summary>
    /// Opcional: Hace que el texto siempre mire hacia la cámara principal para que sea legible.
    /// </summary>
    private void Update()
    {
        if (dialogueTextComponent != null && dialogueTextComponent.enabled)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                // Hacer que el texto mire a la cámara
                dialogueObject.transform.rotation = Quaternion.LookRotation(dialogueObject.transform.position - mainCamera.transform.position);
            }
        }
    }
}
