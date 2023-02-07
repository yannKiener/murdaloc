﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FriendlySpell : AbstractSpell
{
	public override void Cast(Character caster, Character target)
	{
		if (IsCastable(caster,target))
		{
			if (target != null) {
				target = caster;
			}
			caster.AddResource (caster.GetMaxResource() - caster.GetCurrentResource ());
			caster.ApplyHeal (100);
		}
		else
		{
			//Impossible de lancer le sort
		}

	}

	public override bool IsCastable(Character caster, Character target){
		return base.IsCastable (caster, target);
	}
}
