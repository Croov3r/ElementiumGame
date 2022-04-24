using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleSpells : MonoBehaviour
{
    public int emeralds = 0;
    public List<GameObject> cards;
    public List<SpellScript> spells;

    List<Slider> sliders = new List<Slider>();
    List<GameObject> buttonGos = new List<GameObject>();

    List<Image> images;

    public void CastSpell(GameObject Spell)
    {
        Instantiate(Spell);
    }
    
    public void AddEmeralds(int x)
    {
        emeralds += x;
        foreach (Slider s in sliders)
        {
            s.value = emeralds;
        }
    }

    public void Start()
    {
        foreach (GameObject card in cards)
        {
            buttonGos.Add(card.transform.GetChild(0).gameObject);
            sliders.Add(card.transform.GetChild(1).GetComponent<Slider>());
        }
        for (int i = 0; i < 6; i++)
        {
            buttonGos[i].GetComponent<Image>().sprite = spells[i].sprite;
        }
        AddEmeralds(0);
    }
}
