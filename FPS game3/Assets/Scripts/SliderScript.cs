using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private Slider Sensitivityslider;
    [SerializeField] private Slider fovslider;

    [SerializeField] private TextMeshProUGUI fovText;
    [SerializeField] private TextMeshProUGUI sliderText;

    private void Start()
    {
        Sensitivityslider.onValueChanged.AddListener((v) =>
        {
            //sliderText.text = v.ToString("0.00");
            //PlayerLook.mouseSensitivity = v.ConvertTo<int>();
            PlayerLook.oldMouseSensitivity = v.ConvertTo<int>();
            sliderText.text = PlayerLook.oldMouseSensitivity.ToString("0.00");
        });
        /*fovslider.onValueChanged.AddListener((v) =>
        {
            //sliderText.text = v.ToString("0.00");
            //PlayerLook.mouseSensitivity = v.ConvertTo<int>();
            PlayerLook.FOV = v.ConvertTo<int>();
            fovText.text = PlayerLook.FOV.ToString("0.00");
        });
        */
    }
}