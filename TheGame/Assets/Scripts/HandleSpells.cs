using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleSpells : MonoBehaviour
{
    public void CastSpell(GameObject Spell)
    {
        Instantiate(Spell);
    }
}
