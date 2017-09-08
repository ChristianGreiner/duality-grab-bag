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

namespace ChristianGreiner.Duality.Plugins.GrabBag.AI.BehaviorTree.Decorators
{
    public class Inverter<AIContext> : IDecorator<AIContext>
    {
        private readonly IBehaviorNode<AIContext> decoratedNode;
        private BehaviorResult behaviorResult;

        public event EventHandler OnBehaved;

        public Inverter(IBehaviorNode<AIContext> decoratedNode)
        {
            this.decoratedNode = decoratedNode;
        }

        public IEnumerable<IBehaviorNode<AIContext>> SubNodes
        { get { yield return decoratedNode; } }

        public string Name { get; set; } = string.Empty;

        public BehaviorResult Behave(AIContext context)
        {
            if (behaviorResult != BehaviorResult.Running)
            {
                return InvokeAndReturn();
            }

            switch (decoratedNode.Behave(context))
            {
                case BehaviorResult.Success:
                    behaviorResult = BehaviorResult.Failure;
                    break;

                case BehaviorResult.Failure:
                    behaviorResult = BehaviorResult.Success;
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