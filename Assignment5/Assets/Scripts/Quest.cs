using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public static int nextIndex = 0;
    public int id;
    public int index;
    public string title;
    public string description;
    public string reward;
    public bool isCompleted;
    public Dictionary<string, int> rewardDict = new Dictionary<string, int>();
    public int state;

    public Quest(int id, string title, string description, string reward, bool indexCount)
    {
        if (nextIndex < 15)
        {
            if (indexCount)
            {
                index = getNextIndex();
            }
            else
            {
                index = -1;
            }

            this.id = id;
            this.title = title;
            this.description = description;
            this.reward = reward;

            string[] items = reward.Split(", ");
            foreach (string item in items)
            {
                string[] item_num = item.Split(" x ");
                int num = int.Parse(item_num[1]);
                rewardDict.Add(item_num[0], num);
            }

            this.state = 0;
            this.isCompleted = false;
        }
    }

    int getNextIndex()
    {
        ++nextIndex;
        return nextIndex - 1;
    }
}