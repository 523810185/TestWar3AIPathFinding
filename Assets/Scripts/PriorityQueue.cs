using System;
using System.Collections.Generic;

/// <summary>
/// 在C#层提供一个优先队列的实现
/// 注意：默认是大根堆，每次弹出的是优先级最高的，可以设置<see cref="popGreatPriorityValue"/>参数来改变这个性质
/// </summary>
/// <typeparam name="T"></typeparam>
public class PriorityQueue<T>
{
    IComparer<T> comparer;
    T[] heap;
    int cmpSign = 1; // 1 是大根堆，-1是小根堆

    public int Count { get; private set; }

    private PriorityQueue(int capacity, IComparer<T> comparer, bool popGreatPriorityValue)
    {
        this.comparer = (comparer == null) ? Comparer<T>.Default : comparer;
        this.heap = new T[capacity];
        cmpSign = popGreatPriorityValue ? 1 : -1;
    }

    public class Builder
    {
        int capacity = 16;
        IComparer<T> comparer = Comparer<T>.Default;
        bool popGreatPriorityValue = true;

        public Builder SetCapacity(int capacity) 
        {
            this.capacity = capacity;
            return this;
        }
        public Builder SetComparer(IComparer<T> comparer) 
        {
            this.comparer = comparer;
            return this;
        }
        public Builder SetPopGreatPriorityValue(bool popGreatPriorityValue) 
        {
            this.popGreatPriorityValue = popGreatPriorityValue;
            return this;
        }

        public PriorityQueue<T> Build() 
        {
            return new PriorityQueue<T>(capacity, comparer, popGreatPriorityValue);
        }
    }

    public void Push(T v)
    {
        if (Count >= heap.Length) Array.Resize(ref heap, Count * 2);
        heap[Count] = v;
        SiftUp(Count++);
    }

    public T Pop()
    {
        var v = Top();
        heap[0] = heap[--Count];
        if (Count > 0) SiftDown(0);
        return v;
    }

    public T Top()
    {
        if (Count > 0) return heap[0];
        throw new InvalidOperationException("优先队列为空");
    }

    public void Clear()
    {
        Array.Clear(heap, 0, heap.Length);
        Count = 0;
    }

    void SiftUp(int n)
    {
        var v = heap[n];
        for (var n2 = n / 2; n > 0 && InnerCompare(v, heap[n2]) > 0; n = n2, n2 /= 2) heap[n] = heap[n2];
        heap[n] = v;
    }

    void SiftDown(int n)
    {
        var v = heap[n];
        for (var n2 = n * 2; n2 < Count; n = n2, n2 *= 2)
        {
            if (n2 + 1 < Count && InnerCompare(heap[n2 + 1], heap[n2]) > 0) n2++;
            if (InnerCompare(v, heap[n2]) >= 0) break;
            heap[n] = heap[n2];
        }
        heap[n] = v;
    }

    /// <summary>
    /// 内部的排序方法，会根据参数进行大根堆和小根堆的所需比较
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private int InnerCompare(T a, T b) 
    {
        return cmpSign * comparer.Compare(a, b);
    }
}