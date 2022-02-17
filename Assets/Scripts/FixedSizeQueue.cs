using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedLengthQueue<T> : Queue<T>
{
    // Size of the queue to maintain
    public int Length { get; private set; }

    // Construct the queue of a set size
    public FixedLengthQueue(int length)
    {
        Length = length;
    }

    // Enqueue an object, dequeue if size exceeded
    // Lock not needed for concurrect executions => only called from once / run
    public new void Enqueue(T obj)
    {
        // If size if already too big, an error has occured somewhere
        if (Count > Length)
        {
            // Log error for debugging, will be fixed either way
            Debug.LogError("Fixed Length Queue size exceeded unexpectedly");
        }
        
        // Dequeue till room to enqueue
        while (Count >= Length)
        {
            Dequeue();
        }

        // add new object
        base.Enqueue(obj);
    }
}
