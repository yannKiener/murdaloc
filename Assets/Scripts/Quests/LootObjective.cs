﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootObjective : Objective {

    string lootName;
    int itemCount;
    bool isOver = false;

    public LootObjective(string name, int count)
    {
        Debug.Log("new lootObjective : loot " + count + " " + name);
        this.lootName = name;
        this.itemCount = count;
    }

    public bool IsOver()
    {
        return isOver;
    }

    public void Update(Hostile enemy)
    {
        if(FindUtils.GetInventoryGrid().HasItem(lootName) == itemCount)
        {
            isOver = true;
        } else
        {
            isOver = false;
        }
    }
}
