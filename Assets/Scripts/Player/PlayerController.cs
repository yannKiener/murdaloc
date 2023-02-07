﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	// Use this for initialization;
    void Start()
    {
        Player player = gameObject.AddComponent<Player>();
        player.Initialize("Speaf");
		player.AddSpell (Spells.Get ("fireball"));
		player.AddSpell (Spells.Get ("splash"));
		player.AddSpell (Spells.Get ("POTTU"));
		player.SetActionBarSlot(0,"fireball");
		player.SetActionBarSlot(1,"splash");
		player.SetActionBarSlot(2,"pottu");
        print(player.GetName()); 

	}
	
	// Update is called once per frame
	void Update () {

	}
}