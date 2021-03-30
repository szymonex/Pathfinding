using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item) // musimy sprawdzi� czy dany typ mo�e np by� por�wnywany i sortowany w stercie, z pomoc� przychodz� interface
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
        items[0] = items[currentItemCount]; //bierzemy item z ko�ca heap i wk��damy go na pierwsze miejsce, zamiast obecnego pierwszego itema
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem (T item)
    {
        SortUp(item); //b�dziemy tylko podnosi� priorytet w stercie, nie chcemy nigdy obni�a�
    }

    public int Count { get { return currentItemCount; } }

    public bool Contains(T item) //czy heap zawiera dany item
    {
        return Equals(items[item.HeapIndex], item);
    }

    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1; //ze wzoru z planszy
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            //sprawdzamy czy ma tylko jednego childa, czy dw�ch
            if(childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;
                if(childIndexRight < currentItemCount)
                {
                    //sprawdzamy, kt�ry z dw�ch child�w ma wi�kszy priority i musimy przestawi� swapIndex na child z wy�szym priority
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                if(item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else //if parent doesn't have any children (czyli te� jest na w�a�ciwej pozycji)
            {
                return;
            }
        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2; //ze wzoru z planszy

        while (true)
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

    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex; //temp
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    public int HeapIndex { get; set; }
}
