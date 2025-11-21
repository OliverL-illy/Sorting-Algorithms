using System.Collections.Generic;
using UnityEngine;

public class SelectionSort : MonoBehaviour
{
    public WindowGraph windowGraph;
    private List<int> valueList;

    public void SSort()
    {
        valueList = windowGraph.valueList;
        
        for (int i = 0; i < valueList.Count; i++)
        {
            if (valueList[i] > valueList[i+1])
            {

            }
        }
        windowGraph.ShowGraph(valueList, -1, (int _i) => "" + (_i + 1), (float _f) => "" + Mathf.RoundToInt(_f));
    }
}
