using UnityEngine;
using UnityEngine.UI;
using TMPro; // Asegúrate de tener TextMeshPro instalado (suele venir por defecto en Unity)

[RequireComponent(typeof(TiendaManager))]
public class TiendaUI_Rapida : MonoBehaviour
{
    private TiendaManager tiendaManager;

    // Referencias de UI generadas por código
    private TextMeshProUGUI textoPuntos;
    private Button botonComprar;
    private TextMeshProUGUI textoBoton;

    void Awake()
    {
        tiendaManager = GetComponent<TiendaManager>();
        CrearCanvasYElementosUI();
    }

    void Update()
    {
        ActualizarUI();
    }

    void CrearCanvasYElementosUI()
    {
        // 1. Crear el Canvas principal
        GameObject canvasGO = new GameObject("Canvas_PruebaUI");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // 2. Crear texto de Puntos
        GameObject textoGO = new GameObject("Texto_Puntos");
        textoGO.transform.SetParent(canvasGO.transform, false);
        textoPuntos = textoGO.AddComponent<TextMeshProUGUI>();
        textoPuntos.fontSize = 28;
        textoPuntos.color = Color.black;

        RectTransform rectTexto = textoPuntos.GetComponent<RectTransform>();
        rectTexto.anchorMin = new Vector2(0.1f, 0.8f);
        rectTexto.anchorMax = new Vector2(0.5f, 0.9f);
        rectTexto.anchoredPosition = Vector2.zero;

        // 3. Crear texto de Instrucción (Espacio)
        GameObject instruccionGO = new GameObject("Texto_Instruccion");
        instruccionGO.transform.SetParent(canvasGO.transform, false);
        TextMeshProUGUI textoInstruccion = instruccionGO.AddComponent<TextMeshProUGUI>();
        textoInstruccion.text = "¡Pulsa [ESPACIO] para sumar 1000 puntos!";
        textoInstruccion.fontSize = 18;
        textoInstruccion.color = Color.darkBlue;

        RectTransform rectInstruccion = textoInstruccion.GetComponent<RectTransform>();
        rectInstruccion.anchorMin = new Vector2(0.1f, 0.7f);
        rectInstruccion.anchorMax = new Vector2(0.5f, 0.8f);
        rectInstruccion.anchoredPosition = Vector2.zero;

        // 4. Crear el Botón de Compra
        GameObject botonGO = new GameObject("Boton_ComprarRadio");
        botonGO.transform.SetParent(canvasGO.transform, false);
        botonComprar = botonGO.AddComponent<Button>();
        Image imagenBoton = botonGO.AddComponent<Image>();
        imagenBoton.color = new Color(0.2f, 0.6f, 1f); // Azul bonito

        RectTransform rectBoton = botonGO.GetComponent<RectTransform>();
        rectBoton.anchorMin = new Vector2(0.1f, 0.5f);
        rectBoton.anchorMax = new Vector2(0.35f, 0.65f);
        rectBoton.anchoredPosition = Vector2.zero;

        // 5. Crear el Texto dentro del Botón
        GameObject textoBotonGO = new GameObject("Texto_Boton");
        textoBotonGO.transform.SetParent(botonGO.transform, false);
        textoBoton = textoBotonGO.AddComponent<TextMeshProUGUI>();
        textoBoton.fontSize = 20;
        textoBoton.alignment = TextAlignmentOptions.Center;
        textoBoton.color = Color.white;

        RectTransform rectTextoBoton = textoBoton.GetComponent<RectTransform>();
        rectTextoBoton.anchorMin = Vector2.zero;
        rectTextoBoton.anchorMax = Vector2.one;
        rectTextoBoton.sizeDelta = Vector2.zero;

        // 6. Asignar la acción del botón por código
        botonComprar.onClick.AddListener(() => {
            tiendaManager.IntentarComprarMejora("radio");
        });

        // Asegurar que hay un EventSystem para que funcionen los clics
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }

    void ActualizarUI()
    {
        if (textoPuntos != null)
        {
            textoPuntos.text = "Puntos: " + tiendaManager.puntosPrueba.ToString("F0");
        }

        // Buscar la mejora "radio" en la lista para actualizar el texto del botón dinámicamente
        if (textoBoton != null && tiendaManager.listaMejoras != null)
        {
            Mejora mejoraRadio = tiendaManager.listaMejoras.Find(m => m.idMejora == "radio");
            if (mejoraRadio != null)
            {
                textoBoton.text = "Comprar " + mejoraRadio.nombreMostrado +
                                  "\nNivel: " + mejoraRadio.nivelActual + "/" + mejoraRadio.nivelMaximo +
                                  "\nCosto: " + mejoraRadio.CostoActual.ToString("F0");
            }
            else
            {
                textoBoton.text = "Crea una mejora con ID 'radio'\nen el Inspector del TiendaManager";
            }
        }
    }
}