using System.Collections.Generic;
using UnityEngine;

public class BubbleSort : MonoBehaviour
{
    public void BSort(List<int> valueList)
    {
        for (int i = 0; i < valueList.Count - 1; i++)
        {
            int passed = 0;
            if (valueList[i] > valueList[i + 1])
            {
                pass(valueList, i);
                passed++;
            }
        }
    }

    private void pass(List<int> valueList, int i)
    {
        int temp = valueList[i];
        valueList[i] = valueList[i + 1];
        valueList[i + 1] = temp;
    }
}
