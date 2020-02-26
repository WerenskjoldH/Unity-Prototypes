using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimatorFunctionsScript : MonoBehaviour
{
    [SerializeField] MenuControllerScript menuController;
    public TextMeshProUGUI uiText;
    public string defaultText;

    void Start()
    {
        defaultText = uiText.text;
    }

    public void TextSelected()
    {
        uiText.text = "-" + defaultText + "-";
    }

    public void TextUnselected()
    {
        uiText.text = defaultText;
    }

    void PlaySound(AudioClip audioClip)
    {
        if (audioClip == null)
            return;
        menuController.audioSource.PlayOneShot(audioClip);
    }
}
