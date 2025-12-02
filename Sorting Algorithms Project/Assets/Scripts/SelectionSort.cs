using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SelectionSort : MonoBehaviour
{
    public WindowGraph windowGraph;
    private List<int> valueList;

    private int smallest;

    public void SSort()
    {
        valueList = windowGraph.valueList;

        for (int j = 0; j < valueList.Count - 1; j++)
        {
            smallest = j;

            for (int i = j; i < valueList.Count; i++)
            {
                if (valueList[i] < valueList[smallest])
                {
                    smallest = i;
                }
            }
            int smallestValue = valueList[smallest];
            valueList.RemoveAt(smallest);
            valueList.Insert(j, smallestValue);
        }

        windowGraph.ShowGraph(valueList, -1, (int _i) => "" + (_i + 1), (float _f) => "" + Mathf.RoundToInt(_f));
    }
}
