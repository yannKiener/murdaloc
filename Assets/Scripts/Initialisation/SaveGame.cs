﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveGame {
    string name;
    int cash;
    int level;
    float expPercent;
    int rsrc;
    List<string> spellList;
    List<Equipment> charSheetItems;
    List<Item> inventoryItems;
    Dictionary<string, bool> status;
    string lastScene;

    public SaveGame()
    {
        CharacterSheet chSheet = FindUtils.GetCharacterSheetGrid();
        Inventory inv = FindUtils.GetInventoryGrid();
        Player player = FindUtils.GetPlayer();

        this.cash = inv.GetCash();
        this.name = player.GetName();
        this.level = player.GetLevel();
        this.expPercent = player.GetExp();
        Resource rs = player.GetResourceType();
        if (rs is Rage)
        {
            rsrc = 1;
        } else if (rs is Energy)
        {
            rsrc = 2;
        } else
        {
            rsrc = 3;
        }
        this.spellList = player.GetSpells().Keys.ToList<string>();
        this.charSheetItems = chSheet.GetEquipments();
        this.inventoryItems = inv.GetItems();
        this.status = DialogStatus.GetAllStatus();
        lastScene = SceneManager.GetActiveScene().name;
    }

    public string GetLastScene()
    {
        return lastScene;
    }

    public void LoadData()
    {
        Resource r;
        if(rsrc == 1)
        {
            r = new Rage();
        } else if(rsrc == 2)
        {
            r = new Energy();
        } else
        {
            r = new Mana();
        }

        List<Spell> sList = new List<Spell>();
        foreach(string spellName in spellList)
        {
            sList.Add(Spells.Get(spellName));
        }

        FindUtils.GetPlayer().InitializeWith(name, level, expPercent, r, sList);

        FindUtils.GetInventoryGrid().AddCash(cash);

        foreach (Equipment item in charSheetItems)
        {
            item.LoadImage();
            FindUtils.GetCharacterSheetGrid().EquipEquipment(item);
        }
        foreach (Item item in inventoryItems)
        {
            item.LoadImage();

            if (item is Consumable)
            {
                ((Consumable)item).LoadSpell();
            }
            FindUtils.GetInventoryGrid().AddItem(item);
        }

        DialogStatus.SetAllStatus(status);
        
    }
}