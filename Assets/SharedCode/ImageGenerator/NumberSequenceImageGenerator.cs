using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSequenceImageGenerator : ImageGeneratorBase
{
    public uNumber number;
    public double start;
    public double end;
    public double gap;
    IEnumerator Start()
    {
        for (double i = start; i < end; i+=gap)
        {
            number.Value = i;
            Generate(i);
            yield return new WaitForSeconds(0.5f);
        }
        number.Value = end;
        Generate(end);
    }
}
