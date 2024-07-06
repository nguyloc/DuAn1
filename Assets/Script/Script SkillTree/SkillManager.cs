using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public Skill[] skills;
    public SkillButton[] Skillbuttons;

    public Skill activateSkill;

    [Header("STAGE 02")] 
    public Sprite defaultFrame;
    public Sprite activateFrame;

    public int totalPoint;
    public int reaminPoint;//current skill point
    public TextMeshProUGUI pointText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != null)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        reaminPoint = totalPoint;

        DisPlaySkillPoint();
        UpdateSkillImage();
      
    }

    private void DisPlaySkillPoint()
    {
        pointText.text = reaminPoint + "/" + totalPoint;
    }

    private void UpdateSkillImage()
    {
        for(int i = 0; i < skills.Length; i++)
        {
            if (skills[i].isUpgrade)// locked - Image is 100% color
            {
                skills[i].GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
                skills[i].transform.GetChild(0).GetComponent<Image>().sprite = activateFrame; //gray frame
            }
            else
            {
                skills[i].GetComponent<Image>().color = new Vector4(0.39f,0.39f,0.39f,1);
                skills[i].transform.GetChild(0).GetComponent<Image>().sprite = defaultFrame;// gold frame
            }
        }
    }

    public void UpgradeButton()
    {
         if(!activateSkill.isUpgrade && reaminPoint >= 1)
        {
            for(int i = 0; i < activateSkill.previousSkill.Length; i++)
            {
                if (activateSkill.previousSkill[i].isUpgrade)
                {
                    activateSkill.isUpgrade = true;
                    reaminPoint -= 1;
                }
                else
                {
                    Debug.Log("--Upgrade your previous skill first");
                }
            }
           
        }
        else
        {
            Debug.Log("Not enough skill point "+ activateSkill + " is already upgraded yet ");
        }

        UpdateSkillImage();
        DisPlaySkillPoint();
       
    }

    public void ResetButton()
    {
        reaminPoint = totalPoint;
        for(int i = 0; i < skills.Length;i++)
        {
            skills[i].isUpgrade = false;
        }
        DisPlaySkillPoint();
        UpdateSkillImage();
    }

   
}
