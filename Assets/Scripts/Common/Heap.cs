using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;



    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public T GetFirst()
    {
        return items[0];
    }

    public T Remove(T item)
    {
        if(Contains(item))
        {
            T removedItem = items[item.HeapIndex];
            currentItemCount--;
            items[item.HeapIndex] = items[currentItemCount];
            items[item.HeapIndex].HeapIndex = removedItem.HeapIndex;
            SortDown(items[item.HeapIndex]);
            return removedItem;
        }
        return default(T);
    }

    public T RemoveByIndex(int index)
    {
        if (Contains(index))
        {
            T removedItem = items[index];
            currentItemCount--;
            items[index] = items[currentItemCount];
            items[index].HeapIndex = removedItem.HeapIndex;
            SortDown(items[index]);
            return removedItem;
        }
        return default(T);
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count { get { return currentItemCount; } }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    public bool Contains(int index)
    {
        return index < currentItemCount;
    }

    void SortDown(T item)
    {
        while(true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;

            int swapindex = 0;
            //查看是否有Left ，如果有先设置SwapIndex为Left
            if(childIndexLeft < currentItemCount)
            {
                swapindex = childIndexLeft;
                //再次看是否有Right ，如果有则尝试比较Left和Right
                if (childIndexRight < currentItemCount)
                {
                    //比较Left和Right， 优先的那个替换为Swapindex
                    if(items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapindex = childIndexRight;
                    }
                }

                if(item.CompareTo(items[swapindex])< 0)
                {
                    Swap(item, items[swapindex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }

        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1)/2;

        while(true)
        {
            T parentItem = items[parentIndex];
            if(item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap (T itema, T itemb)
    {
        items[itema.HeapIndex] = itemb;
        items[itemb.HeapIndex] = itema;
        int itemindex = itema.HeapIndex;
        itema.HeapIndex = itemb.HeapIndex;
        itemb.HeapIndex = itemindex;
    }

}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}

