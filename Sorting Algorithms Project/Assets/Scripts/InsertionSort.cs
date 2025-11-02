using System.Collections.Generic;
using UnityEngine;

public class InsertionSort : MonoBehaviour
{
    public WindowGraph windowGraph;
    private List<int> valueList;
    public void ISort()
    {
        valueList = windowGraph.valueList;

        for (int i = 1; i < valueList.Count; i++)
        {
            if (valueList[i] < valueList[i - 1])
            {
                int j = i - 1;
                while (j >= 0 && valueList[i] < valueList[j])
                {
                    j--;
                }

                int endPos = j + 1;
                int value = valueList[i];
                valueList.RemoveAt(i);
                valueList.Insert(endPos, value);
            }
        }

        windowGraph.ShowGraph(valueList, -1, (int _i) => "" + (_i + 1), (float _f) => "" + Mathf.RoundToInt(_f));
    }
}
