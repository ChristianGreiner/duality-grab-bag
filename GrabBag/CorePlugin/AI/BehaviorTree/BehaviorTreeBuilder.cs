using ChristianGreiner.Duality.Plugins.GrabBag.AI.BehaviorTree.Composites;
using ChristianGreiner.Duality.Plugins.GrabBag.AI.BehaviorTree.Decorators;
using ChristianGreiner.Duality.Plugins.GrabBag.AI.BehaviorTree.Leaves;
using System;
using System.Collections.Generic;

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

using System.Linq;

namespace ChristianGreiner.Duality.Plugins.GrabBag.AI.BehaviorTree
{
    public class BehaviorTreeBuilder<AIContext>
    {
        private readonly Stack<Group> cachedNodes = new Stack<Group>();
        private Decorator<AIContext> decorator = node => node;

        public BehaviorTreeBuilder()
        {
            cachedNodes.Push(new Group((node, group) => { }));
        }

        private class Group
        {
            public Group(Action<IEnumerable<IBehaviorNode<AIContext>>, Group> groupEndCallback)
            {
                GroupEndCallback = groupEndCallback;
            }

            public List<IBehaviorNode<AIContext>> Nodes { get; } = new List<IBehaviorNode<AIContext>>();

            public Action<IEnumerable<IBehaviorNode<AIContext>>, Group> GroupEndCallback { get; }
        }

        public BehaviorTreeBuilder<AIContext> Behavior(
            Func<AIContext, BehaviorResult> processedBehavior,
            Func<AIContext, BehaviorResult> initialBehavior = null)
        {
            Add(decorator(
            (initialBehavior == null) ?
            new Behavior<AIContext>(processedBehavior) :
            new Behavior<AIContext>(processedBehavior, initialBehavior)));

            return this;
        }

        private void Add(IBehaviorNode<AIContext> node)
        {
            var group = cachedNodes.Pop();

            group.Nodes.Add(node);
            cachedNodes.Push(group);
        }

        public BehaviorTreeBuilder<AIContext> End
        {
            get
            {
                var group = cachedNodes.Pop();
                var previousGroup = cachedNodes.Pop();
                cachedNodes.Push(previousGroup);
                group.GroupEndCallback(group.Nodes, previousGroup);
                return this;
            }
        }

        public BehaviorTreeBuilder<AIContext> Decorate(Decorator<AIContext> newDecorator)
        {
            decorator = node =>
            {
                decorator = n => n;
                return newDecorator(node);
            };
            return this;
        }

        public BehaviorTreeBuilder<AIContext> Question(Predicate<AIContext> behaviorTest)
        {
            Add(decorator(new Question<AIContext>(behaviorTest)));
            return this;
        }

        public BehaviorTreeBuilder<AIContext> Sequence
        {
            get
            {
                var group = new Group((nodes, grp) =>
                {
                    grp.Nodes.Add(decorator(new Sequence<AIContext>(nodes.ToArray())));
                });
                cachedNodes.Push(group);
                return this;
            }
        }

        public BehaviorTreeBuilder<AIContext> Selector
        {
            get
            {
                var group = new Group((nodes, grp) =>
                {
                    grp.Nodes.Add(decorator(new Selector<AIContext>(nodes.ToArray())));
                });
                cachedNodes.Push(group);
                return this;
            }
        }

        public IBehaviorNode<AIContext> Tree
        {
            get
            {
                if (cachedNodes.Count != 1)
                {
                    throw new Exception("Root should contain only one node");
                }
                return cachedNodes.Pop().Nodes[0];
            }
        }
    }
}