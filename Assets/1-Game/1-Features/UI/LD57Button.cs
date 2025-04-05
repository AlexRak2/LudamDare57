using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

namespace LD57.UI
{
public class LD57Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image highlightImage, iconToScale;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Button button;
    public Button Button => button;
    private void Awake() 
    {
        if(highlightImage != null) highlightImage.enabled = false;
        if(button != null) button.onClick.AddListener(OnMouseDown);
    }
    public void OnMouseDown() 
    {
        // IAudioRequester.Instance.PlaySFX(SFXData.ButtonClick);
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if(!button.interactable) return;

        // IAudioRequester.Instance.PlaySFX(SFXData.ButtonHover);
        BloomItemScale(transform, 1.025f, 0.1f);
        if(buttonText != null) BloomItemScaleTemp(buttonText.transform, 1.1f, 0.1f);
        if(iconToScale != null) BloomItemScaleTemp(iconToScale.transform, 1.1f, 0.1f);
        if(highlightImage != null) highlightImage.enabled = true;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        BloomItemScale(transform, 1f, 0.1f);
        if(highlightImage != null) highlightImage.enabled = false;
    }
    public static async void BloomItemScale(Transform transform, float endScale, float duration = 0.1f)
    {
        float t = 0;
        float startScale = transform.localScale.x;
        while(t < 1) {
            t += Time.unscaledDeltaTime / duration;
            transform.localScale = new Vector3(
                Mathf.Lerp(startScale, endScale, t), 
                Mathf.Lerp(startScale, endScale, t),
                Mathf.Lerp(startScale, endScale, t));
            await Task.Yield();
        }
    }
    public static async void BloomItemScaleTemp(Transform transform, float endScale, float duration = 0.1f)
    {
        float t = 0;
        float startScale = transform.localScale.x;
        while(t < 1) {
            t += Time.unscaledDeltaTime / duration;
            transform.localScale = new Vector3(
                Mathf.Lerp(startScale, endScale, t), 
                Mathf.Lerp(startScale, endScale, t),
                Mathf.Lerp(startScale, endScale, t));
            await Task.Yield();
        }
        //lerp back to original scale
        t = 0;
        while(t < 1) {
            t += Time.unscaledDeltaTime / duration;
            if(transform == null) return;
            transform.localScale = new Vector3(
                Mathf.Lerp(endScale, startScale, t), 
                Mathf.Lerp(endScale, startScale, t),
                Mathf.Lerp(endScale, startScale, t));
            await Task.Yield();
        }
    }
}
}