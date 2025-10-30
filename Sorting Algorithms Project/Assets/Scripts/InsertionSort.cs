using System.Collections.Generic;
using UnityEngine;

public class InsertionSort : MonoBehaviour
{
    public void ISort(List<int> valueList)
    {
        for (int i = 0; i < valueList.Count - 1; i++)
        {
            if (valueList[i + 1] < valueList[i])
            {
                int j = i;
                while (j >= 0 && valueList[i + 1] < valueList[j])
                {
                    j--;
                }

                int endPos = j + 1;
                int value = valueList[i + 1];
                valueList.RemoveAt(i + 1);
                valueList.Insert(endPos, value);
            }
        }
    }
}
