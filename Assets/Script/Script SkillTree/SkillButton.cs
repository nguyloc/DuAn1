using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Image skillImage;
    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI skillDesText;

    public int skillButtonID;

    public void PressSkillButton()
    {
        SkillManager.instance.activateSkill = transform.GetComponent<Skill>();

        skillImage.sprite = SkillManager.instance.skills[skillButtonID].skillSprite;
        skillNameText.text = SkillManager.instance.skills[skillButtonID].skillName;
        skillDesText.text = SkillManager.instance.skills[skillButtonID].skillDes;
    }
}
