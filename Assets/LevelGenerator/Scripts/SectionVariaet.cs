using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using LevelGenerator.Scripts;
public class SectionVariaet : MonoBehaviour
{
    [Tooltip("how likely the section will keep the default variant")]
    public uint defaultWeight = 1;
    public List<SVariant> variants = new List<SVariant>();
    public int GetWeightedRandomNumber()
    {
        int total = 0;
        foreach (SVariant v in variants)
        {
            total += (int)v.weight;
        }
        return Random.Range(0, total + (int)defaultWeight);
    }
    public SVariant[] GetByTag(string[] tag)
    {
        variants.ForEach(x => print(x.addTags));
        return variants.Where(x => !tag.Except(x.addTags).Any()).ToArray();
    }
    public void GenerateVariant(int overrideIndex = -1)
    {
        if (overrideIndex >= 0)
        {
            if (overrideIndex < variants.Count)
            {

            }
            return;
        }
        int variantChoice = GetWeightedRandomNumber();
        bool hasChosen = false;
        int current = 0;
        foreach (SVariant v in variants)
        {
            current += (int)v.weight;
            if (variantChoice < current && !hasChosen)
            {
                ActivateVariant(v);
                hasChosen = true;
            }
            else
            {
                v.variantObject.SetActive(false);
            }
        }
    }
    public void ActivateVariant(SVariant v)
    {
        v.variantObject.SetActive(true);
        Section section = GetComponent<Section>();
        if (section != null)
        {
            List<string> temp = new List<string>();
            temp.AddRange(section.Tags);
            temp.AddRange(v.addTags);
            section.Tags = temp.ToArray();

            temp.Clear();
            temp.AddRange(section.CreatesTags);
            temp.AddRange(v.addCreateTags);
            section.CreatesTags = temp.ToArray();
        }
    }
}
[System.Serializable]
public class SVariant
{
    public GameObject variantObject;
    public uint weight = 1;
    public string[] addTags;
    public string[] addCreateTags;
}



