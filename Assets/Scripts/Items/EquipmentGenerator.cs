﻿using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class EquipmentGenerator  {
    private static List<EquipmentType> weaponTypes = new List<EquipmentType>() { EquipmentType.Axe, EquipmentType.TwoHandedAxe, EquipmentType.Sword, EquipmentType.TwoHandedSword, EquipmentType.Mace, EquipmentType.TwoHandedMace, EquipmentType.Dagger, EquipmentType.Staff };

    private static List<EquipmentType> TwoHandedWeaponTypes = new List<EquipmentType>() {EquipmentType.TwoHandedAxe,EquipmentType.TwoHandedSword, EquipmentType.TwoHandedMace, EquipmentType.Staff};

    public static Equipment GenerateEquipment(int maxLevel)
    {
        //Quality multiplier : 
        //Green = 0.7
        //Blue = 1
        //Epic = 1.3

        //Add quality at multiplier

        //Make a random type
        float maxMainStats = Constants.MainStatMultiplier * maxLevel;
        float maxOffStats = Constants.OffStatMultiplier * maxLevel;
        EquipmentType itemType = GetRandomEquipmentType();
        int damagePerSecondOnWeapon = Constants.BaseAutoAttackDPS + Constants.AutoAttackDPSPerLevel * maxLevel;

        Stats stats = GenerateStatsForType(itemType, maxMainStats, maxOffStats, damagePerSecondOnWeapon);
        string name = GetRandomNameForType(itemType, stats);
        Sprite sprite = GetRandomSpriteForType(itemType);

        Equipment result = new Equipment(name,"",maxLevel,stats,itemType);
        result.SetImage(sprite);
        return result;
    }

    private static Sprite GetRandomSpriteForType(EquipmentType type)
    {
        Sprite[] sprites = DatabaseUtils.LoadAllSpritesForType(type);
        Sprite result;
        if(sprites != null && sprites.Length > 0){
            result = sprites[UnityEngine.Random.Range(0, sprites.Length-1)];
        } else {
            result = InterfaceUtils.LoadSpriteForItem("Default");
        }
        return result;
    }


    private static string GetRandomNameForType (EquipmentType type, Stats stats)
    {
        Stat mainStat = stats.GetMaxMainStat();
        Stat offStat = stats.GetMaxOffStat();

        string mainStatName = "";
        if(stats.GetStat(mainStat) > 1)
        {
            JSONArray mainStatNames = DatabaseUtils.GetJsonStatNames(mainStat);
            mainStatName = mainStatNames[UnityEngine.Random.Range(0, mainStatNames.Count)];
        }

        string offStatName = "";
        if (stats.GetStat(offStat) > 1)
        {
            JSONArray offStatNames = DatabaseUtils.GetJsonStatNames(offStat);
            offStatName = offStatNames[UnityEngine.Random.Range(0, offStatNames.Count)];
        }

        JSONArray names = DatabaseUtils.GetJsonCategoryName(type);
        string name = names[UnityEngine.Random.Range(0, names.Count)];

        string result = "";
        if(offStatName.Length > 1)
        {
            result += offStatName + " ";
        }


        if (mainStatName.Length > 1)
        {
            bool mainStatNameFirst = UnityEngine.Random.Range(0,3) >= 1;
            if (mainStatNameFirst)
            {
                result += mainStatName + "'s ";
                result += name;
            } else
            {
                result += name;
                result += " of the " + mainStatName;
            }
        } else
        {
            result += name;

        }
        return result;
    }
        

    private static EquipmentType GetRandomEquipmentType()
    {
        Array values = Enum.GetValues(typeof(EquipmentType));
        
        return (EquipmentType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    private static Stats AddStatFromEnumRandomly(List<Stat> statList, float number)
    {
        Stats stats = new Stats(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        Stat stat = statList[UnityEngine.Random.Range(0, statList.Count)];
        stats.AddStat(stat, number);
        return stats;
    }

    private static Stats GenerateStatsForType(EquipmentType type, float maxMainStats, float maxOffStats, int attackDamage)
    {
        float stat1Multiplier = UnityEngine.Random.Range(30,51) ;
        float stat2Multiplier = UnityEngine.Random.Range(30,51) ;
        float stat3Multiplier = UnityEngine.Random.Range(0, 100-(stat1Multiplier+stat2Multiplier)) ;
        float offStat1Multiplier = UnityEngine.Random.Range(30, 71) ;
        float offStat2Multiplier = UnityEngine.Random.Range(20, 31) ;
        float offStat3Multiplier = UnityEngine.Random.Range(0, 100-(offStat1Multiplier+offStat2Multiplier)) ;
        List<Stat> mainStatList = GetMainStatList(type);
        List<Stat> offStatList = GetOffStatList(type);

        Stats result = AddStatFromEnumRandomly(mainStatList, maxMainStats * stat1Multiplier / 100);
        result.Add(AddStatFromEnumRandomly(mainStatList, maxMainStats * stat2Multiplier / 100));
        result.Add(AddStatFromEnumRandomly(mainStatList, maxMainStats * stat3Multiplier / 100));
        result.Add(AddStatFromEnumRandomly(offStatList, maxOffStats * offStat1Multiplier / 100));
        result.Add(AddStatFromEnumRandomly(offStatList, maxOffStats * offStat2Multiplier / 100));
        result.Add(AddStatFromEnumRandomly(offStatList, maxOffStats * offStat3Multiplier / 100));

        if (IsWeapon(type))
        {
            float attackSpeed = (float)Math.Round(GetAttackSpeedForType(type), 1);
            result.AddStat(Stat.autoAttackDamage, attackDamage * attackSpeed);
            result.AddStat(Stat.autoAttackTime, attackSpeed);
        }

        if (IsTwoHanded(type))
        {
            result.MultiplyBy(Constants.TwoHandedStatMultiplier);
        }
        
        return result;
    }

    private static float GetAttackSpeedForType(EquipmentType t)
    {
        switch (t)
        {
            case EquipmentType.Sword:
                return UnityEngine.Random.Range(1.5f,2.5f);
            case EquipmentType.TwoHandedSword:
                return UnityEngine.Random.Range(2.5f, 4f);
            case EquipmentType.Axe:
                return UnityEngine.Random.Range(1.5f, 2.5f);
            case EquipmentType.TwoHandedAxe:
                return UnityEngine.Random.Range(2.5f, 4f);
            case EquipmentType.Mace:
                return UnityEngine.Random.Range(1.5f, 2.5f);
            case EquipmentType.TwoHandedMace:
                return UnityEngine.Random.Range(2.5f, 4f);
            case EquipmentType.Dagger:
                return UnityEngine.Random.Range(0.8f, 2f);
            case EquipmentType.Staff:
                return UnityEngine.Random.Range(2.5f, 4f);
        }
        return 5;
    }

    public static bool IsTwoHanded(EquipmentType itemType)
    {
        return TwoHandedWeaponTypes.Contains(itemType);
    }

    public static bool IsWeapon(EquipmentType itemType)
    {
        return weaponTypes.Contains(itemType);
    }

    private static List<Stat> GetMainStatList(EquipmentType type)
    {
        return EquipmentCategories.GetCategory(type).GetPossibleMainStats();
    }

    private static List<Stat> GetOffStatList(EquipmentType type)
    {
        return EquipmentCategories.GetCategory(type).GetPossibleOffStats();
    }
}