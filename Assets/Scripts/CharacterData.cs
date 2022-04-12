using System.Collections;
using System.Collections.Generic;

public class CharacterData
{
    Character character;
    public void LoadCharacter(List<Character> list, string name)
    {
        switch (name)
        {
            case "astronauta bert":
                character = new AstronautaBert();
                break;
            case "bert wick":
                character = new BertWick();
                break;
            case "bert pogromca":
                character = new BertPogromca();
                break;
            case "bert ventura":
                character = new BertVentura();
                break;
            case "bert who":
                character = new BertWho();
                break;
            case "bert zawodowiec":
                character = new BertZawodowiec();
                break;
            case "berta amazonka":
                character = new BertaAmazonka();
                break;
            case "berta gejsza":
                character = new BertaGejsza();
                break;
            case "berta sjw":
                character = new BertaSJW();
                break;
            case "berta trojanska":
                character = new BertaTrojanska();
                break;
            case "bertka idolka":
                character = new BertkaIdolka();
                break;
            case "bertka serferka":
                character = new BertkaSerferka();
                break;
            case "bertolaj":
                character = new Bertolaj();
                break;
            case "bertonator":
                character = new Bertonator();
                break;
            case "big mad b":
                character = new BigMadB();
                break;
            case "che bert":
                character = new CheBert();
                break;
            case "eberta":
                character = new EBerta();
                break;
            case "gotka berta":
                character = new GotkaBerta();
                break;
            case "konstabl bert":
                character = new KonstablBert();
                break;
            case "koszmar z bertwood":
                character = new KoszmarZBertwood();
                break;
            case "kowboj bert":
                character = new KowbojBert();
                break;
            case "krol popu bert":
                character = new KrolPopuBert();
                break;
            case "krzyzowiec bert":
                character = new KrzyzowiecBert();
                break;
            case "ksiezniczka berta":
                character = new KsiezniczkaBerta();
                break;
            case "kuglarz bert":
                character = new KuglarzBert();
                break;
            case "misiek bert":
                character = new MisiekBert();
                break;
            case "papiez bert II":
                character = new PapiezBertII();
                break;
            case "prezydent bert":
                character = new PrezydentBert();
                break;
            case "prymus bert":
                character = new PrymusBert();
                break;
            case "ronin bert":
                character = new RoninBert();
                break;
            case "rycerz berti":
                character = new RycerzBerti();
                break;
            case "samuraj bert":
                character = new SamurajBert();
                break;
            case "sedzia bertt":
                character = new SedziaBertt();
                break;
            case "shaolin bert":
                character = new ShaolinBert();
                break;
            case "stary bert i moze":
                character = new StaryBert();
                break;
            case "superfan bert":
                character = new SuperfanBert();
                break;
            case "tankbert":
                character = new Tankbert();
                break;
            case "trener pokebertow":
                character = new TrenerPokebertow();
                break;
            case "zalobny bert":
                character = new ZalobnyBert();
                break;
            case "zombert":
                character = new Zombert();
                break;
            default:
                throw new System.Exception("Not recognized name: " + name);
        }
        if (character != null)
        {
            list.Add(character);
            if (list[list.Count - 1].Name != name) 
                throw new System.Exception("Not matching names:" + list[list.Count - 1].Name + ", " + name);
        }
    }
}
