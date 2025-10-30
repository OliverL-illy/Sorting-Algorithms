using System.Collections.Generic;
using UnityEngine;

public class InsertionSort : MonoBehaviour
{
    public void ISort(List<int> valueList)
    {
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
    }
}
