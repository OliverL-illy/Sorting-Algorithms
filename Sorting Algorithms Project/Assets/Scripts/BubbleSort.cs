using System.Collections.Generic;
using UnityEngine;

public class BubbleSort : MonoBehaviour
{
    public WindowGraph windowGraph;
    private List<int> valueList;
    private int passes = -1;
    public void BSort() 
    { 
        valueList = windowGraph.valueList;
        while (passes != 0)
        {
            passes = 0;
            for (int i = 0; i < valueList.Count - 1; i++)
            {
                if (valueList[i] > valueList[i + 1])
                {
                    pass(valueList, i);
                    passes++;
                }
            }
        }

        windowGraph.ShowGraph(valueList, -1, (int _i) => "" + (_i + 1), (float _f) => "" + Mathf.RoundToInt(_f));
    }

    private void pass(List<int> valueList, int i)
    {
        int temp = valueList[i];
        valueList[i] = valueList[i + 1];
        valueList[i + 1] = temp;
    }
}
