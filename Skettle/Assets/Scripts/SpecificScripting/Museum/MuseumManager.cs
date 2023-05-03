using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumManager : MonoBehaviour, I_ProgressConditional
{
    [System.Serializable]
    public struct PaintingSectors
    {
        public int moneyThreshold;
        public int upToPaintingNum;

        public PaintingSectors(int money, int paintings)
        {
            moneyThreshold = money;
            upToPaintingNum = paintings;
        }
    }

    public PaintingSectors nextGoal;

    public List<GameObject> paintings;

    public List<PaintingSectors> paintingActivation;

    ProgressContainer progressC;

    int moneyGiven = 0;

    public void ShowOrHide(ProgressContainer data)
    {
        progressC = data;

        foreach (GameObject go in paintings)
        {
            go.SetActive(false);
        }

        //Default for testing purposes if no sectors assigned
        if (paintingActivation == null)
        {
            paintingActivation = new List<PaintingSectors>();
            paintingActivation.Add(new PaintingSectors(5,5));
        }

        moneyGiven = progressC.progress.moneyGivenToMuseum;

        ShowHidePaintings(moneyGiven);
    }

    void ShowHidePaintings(int moneyGiven)
    {
        PaintingSectors highestAchieved = new PaintingSectors(0,0);
        int highestCount = 0;

        int count = 0;
        foreach(PaintingSectors ps in paintingActivation)
        {
            if(ps.moneyThreshold <= moneyGiven)
            {
                highestAchieved = ps;
                highestCount = count;
            }

            count++;
        }

        for(int i = 0; i < highestAchieved.upToPaintingNum; i++)
        {
            paintings[i].SetActive(true);
        }

        /*
        //If we haven't unlocked everything, then get the next count to show as a goal potentially
        if(paintingActivation.Count-1 > highestCount)
        {
            nextGoal = paintingActivation[highestCount++];
        }
        */
    }

    public void GiveMoney(int giveMoney)
    {
        moneyGiven += giveMoney;
        progressC.progress.moneyGivenToMuseum = moneyGiven;

        //If you hit a goal, the curator needs to tell you to come back later to see what's there
    }
}
