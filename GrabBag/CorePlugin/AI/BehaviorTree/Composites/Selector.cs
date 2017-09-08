/*
    The MIT License (MIT)

    Copyright (c) 2015 George Michaelides

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.

    FROM: https://github.com/gmich/Gem/
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace ChristianGreiner.Duality.Plugins.GrabBag.AI.BehaviorTree.Composites
{
    /// <summary>
    /// Selector. Iterates BehaviorNodes in sequence and terminates upon failure. Behaves like the logical AND operator
    /// </summary>
    /// <typeparam name="AIContext">The context to act upon</typeparam>
    public class Selector<AIContext> : IComposite<AIContext>
    {
        private readonly IBehaviorNode<AIContext>[] nodes;
        private Stack<IBehaviorNode<AIContext>> pendingNodes;
        private BehaviorResult behaviorResult;

        public event EventHandler OnBehaved;

        public Selector(IBehaviorNode<AIContext>[] nodes)
        {
            this.nodes = nodes;
            pendingNodes = new Stack<IBehaviorNode<AIContext>>(Enumerable.Reverse(nodes));
        }

        public void Reset()
        {
            pendingNodes = new Stack<IBehaviorNode<AIContext>>(nodes.Reverse());
        }

        public string Name { get; set; } = string.Empty;

        public IEnumerable<IBehaviorNode<AIContext>> SubNodes
        { get { return nodes; } }

        private bool HasProcessedAllNodes => pendingNodes.Count == 0;

        public BehaviorResult Behave(AIContext context)
        {
            if (HasProcessedAllNodes)
            {
                return InvokeAndReturn();
            }

            var currentNode = pendingNodes.Pop();

            switch (behaviorResult = currentNode.Behave(context))
            {
                case BehaviorResult.Failure:
                    Behave(context);
                    break;

                case BehaviorResult.Running:
                    //reevaluate the next iteration
                    pendingNodes.Push(currentNode);
                    return InvokeAndReturn();

                case BehaviorResult.Success:
                    //stop iterating nodes
                    pendingNodes.Clear();
                    break;
            }

            return InvokeAndReturn();
        }

        private BehaviorResult InvokeAndReturn()
        {
            OnBehaved?.Invoke(this, new BehaviorInvokationEventArgs(behaviorResult));
            return behaviorResult;
        }
    }
}