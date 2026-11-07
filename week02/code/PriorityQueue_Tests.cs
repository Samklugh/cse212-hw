using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    // Scenario: Enqueue increasing priorities, then remove every item.
    // Expected Result: Highest priority first, including the last queued item; queue ends empty.
    // Defect(s) Found: The search skipped the last item and Dequeue never removed its selection.
    public void TestPriorityQueue_1()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("low", 1);
        priorityQueue.Enqueue("middle", 2);
        priorityQueue.Enqueue("high", 3);
        Assert.AreEqual("high", priorityQueue.Dequeue());
        Assert.AreEqual("middle", priorityQueue.Dequeue());
        Assert.AreEqual("low", priorityQueue.Dequeue());
        Assert.AreEqual("[]", priorityQueue.ToString());
    }

    [TestMethod]
    // Scenario: Enqueue several items with equal highest priority and a lower priority item.
    // Expected Result: Tied items leave in FIFO order before the lower priority item.
    // Defect(s) Found: The comparison selected later equal-priority items instead of the first.
    public void TestPriorityQueue_2()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("first", 5);
        priorityQueue.Enqueue("second", 5);
        priorityQueue.Enqueue("low", 1);
        priorityQueue.Enqueue("third", 5);
        Assert.AreEqual("first (Pri:5), second (Pri:5), low (Pri:1), third (Pri:5)",
            priorityQueue.ToString().Trim('[', ']'));
        Assert.AreEqual("first", priorityQueue.Dequeue());
        Assert.AreEqual("second", priorityQueue.Dequeue());
        Assert.AreEqual("third", priorityQueue.Dequeue());
        Assert.AreEqual("low", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Dequeue an empty queue, then enqueue and remove a single item and dequeue again.
    // Expected Result: Both empty operations throw InvalidOperationException with the required message.
    // Defect(s) Found: Initial empty handling was correct; missing removal prevented exhaustion.
    public void TestPriorityQueue_EmptyAndSingleItem()
    {
        var priorityQueue = new PriorityQueue();
        var error = Assert.ThrowsException<InvalidOperationException>(() => priorityQueue.Dequeue());
        Assert.AreEqual("The queue is empty.", error.Message);
        priorityQueue.Enqueue("only", -2);
        Assert.AreEqual("only", priorityQueue.Dequeue());
        error = Assert.ThrowsException<InvalidOperationException>(() => priorityQueue.Dequeue());
        Assert.AreEqual("The queue is empty.", error.Message);
    }
}
