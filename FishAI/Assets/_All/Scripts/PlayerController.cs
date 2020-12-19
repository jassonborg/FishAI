using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private int level = 1;
    private Text levelText;
    public float experience {get; private set;}
    private Transform xpBar;
    

    void Start () 
    {
        xpBar = UIController.instance.transform.Find("xpBar");
        levelText = UIController.instance.transform.Find("levelText").GetComponent<Text>();
        SetExperience(0);
    }


    public void SetExperience(float exp)
    {
        //add experience
        experience += exp;
        float experienceNeeded = GameLogic.ExperienceForNextLevel(level);
        float previousExperience = GameLogic.ExperienceForNextLevel(level - 1);
        //level up
        while(experience >= experienceNeeded)
        {
            LevelUp();
            experienceNeeded = GameLogic.ExperienceForNextLevel(level);
            previousExperience = GameLogic.ExperienceForNextLevel(level - 1);
        }
         xpBar.GetComponent<Image>().fillAmount = (experience - previousExperience)/(experienceNeeded - previousExperience);
    }

    void LevelUp()
    {
        level++;
        levelText.text = level.ToString("0");
    }

   
}
