using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// El cuadro de información que sale al pasar el ratón por una reliquia.
/// Se coloca a la izquierda de la tienda especial, a la altura de la reliquia.
public class RelicTooltip : MonoBehaviour
{
    [SerializeField] TMP_Text texto;
    [SerializeField] RectTransform panelReferencia; // PanelTiendaEspecial

    public void Show(string info, RectTransform slot)
    {
        RectTransform rt = (RectTransform)transform;
        texto.text = info;

        // x: borde izquierdo del panel de la tienda | y: centro de la reliquia
        Vector3 izquierdaPanel = panelReferencia.TransformPoint(new Vector3(panelReferencia.rect.xMin, 0f, 0f));
        Vector3 centroSlot = slot.TransformPoint(slot.rect.center);
        rt.position = new Vector3(izquierdaPanel.x - 10f, centroSlot.y, centroSlot.z);

        gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rt); // ajusta la altura al texto
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}