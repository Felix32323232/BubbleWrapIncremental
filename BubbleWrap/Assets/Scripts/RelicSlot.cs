using UnityEngine;
using UnityEngine.EventSystems;

/// Va en cada reliquia (un Image). Click = comprar, ratón encima = mostrar info.
public class RelicSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string idMejora;               // "puntos_permanentes", "combo_base", "inicio_burbujas"
    [SerializeField] RenacimientoManager manager;
    [SerializeField] RelicTooltip tooltip;
    [SerializeField] float hoverScale = 1.1f;       // la reliquia crece un poco al pasar el ratón

    bool hovering;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        manager.ComprarEspecial(idMejora);
        if (hovering) Mostrar(); // refresca nivel y coste al instante
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
        transform.localScale = Vector3.one * hoverScale;
        Mostrar();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Salir();
    }

    void OnDisable()
    {
        if (hovering) Salir();
    }

    void Mostrar()
    {
        if (tooltip != null) tooltip.Show(manager.TextoInfo(idMejora), (RectTransform)transform);
    }

    void Salir()
    {
        hovering = false;
        transform.localScale = Vector3.one;
        if (tooltip != null) tooltip.Hide();
    }
}