using System;
using System.Collections.Generic;

namespace ppcalc
{
    class SuffixTrie<T> where T : class, IComparable<T>
    {
        class Node<U> where U : class, IComparable<U>
        {
            public Node<U> child;
            public Node<U> sibling;
            public Node<U> link;
            public Node<U> parent;
            public int ID;
            public int edgeStart;
            public int edgeEnd;
            public int depth;

            public Node(int _ID)
            {
                link = parent = child = sibling = null;
                ID = _ID;
                depth = edgeStart = edgeEnd = 0;
            }

            public Node(Node<U> _parent, int _ID, int _start, int _end)
            {
                link = sibling = child = null;
                parent = _parent;
                ID = _ID;
                edgeEnd = _end;
                edgeStart = _start;
                depth = parent.depth + (_end - _start) + 1;
            }

            public Node<U> getChild(T c)
            {
                Node<U> temp = this.child;
                while (temp != null && s[temp.edgeStart] != c)
                {
                    temp = temp.sibling;
                }
                return temp;
            }

            public Node<U> addChild(Node<U> newNode)
            {
                if (this.child == null)
                {
                    this.child = newNode;
                }
                else
                {
                    if (s[newNode.edgeStart].CompareTo(s[this.child.edgeStart]) < 0)
                    {
                        newNode.sibling = this.child;
                        this.child = newNode;
                    }
                    else if (s[newNode.edgeStart] == s[this.child.edgeStart])
                    {
                        newNode.sibling = this.child.sibling;
                        newNode.child = this.child.child;
                        this.child = newNode;
                    }
                    else
                    {
                        Node<U> temp = this.child.sibling;
                        Node<U> prevTemp = this.child;
                        while (temp != null && s[newNode.edgeStart].CompareTo(s[temp.edgeStart]) > 0)
                        {
                            prevTemp = temp;
                            temp = temp.sibling;
                        }
                        if (temp != null && s[newNode.edgeStart] != s[temp.edgeStart])
                        {
                            newNode.sibling = temp;
                        }
                        else if (temp != null)
                        {
                            newNode.sibling = temp.sibling;
                            newNode.child = temp.child;
                        }
                        prevTemp.sibling = newNode;
                    }
                }
                return newNode;
            }

            public bool isLeaf()
            {
                return this.child == null;
            }
        }

		int leafID = 0;
        int internalID;
        static List<T> s;
		Node<T> root;

        public SuffixTrie(List<T> _s)
        {
            s = _s;
            internalID = s.Count + 1;
            root = new Node<T>(giveMeAnID(true));
            root.link = root.parent = root;
            addSuffixes();
        }

        int giveMeAnID(bool isLeafID)
        {
            return isLeafID ? leafID++ : internalID++;
        }

        void printChildren(Node<T> n)
        {
            if (n.isLeaf())
            {
                Console.WriteLine("Leaf ID = " + n.ID);
                return;
            }
            Node<T> next = n.child;
            while (next != null)
            {
                printChildren(next);
                next = next.sibling;
            }
        }

        Node<T> nodeHop(Node<T> vPrime, int betaStart, int betaEnd)
        {
            Node<T> cur = vPrime;
            Node<T> temp;
            int betaremain = betaEnd - betaStart + 1;

            while (betaremain > 0)
            {
                temp = cur.getChild(s[betaStart]);
                betaremain -= ((temp.edgeEnd - temp.edgeStart) + 1);
                if (betaremain >= 0)
                {
                    cur = temp;
                    betaStart += ((cur.edgeEnd - cur.edgeStart) + 1);
                }
            }
            if (betaremain < 0)
            {//if there is more that we can match
                cur = cur.getChild(s[betaStart]);
                for (int i = 0; i < betaEnd - betaStart + 2; i++)
                {
                    if (cur.edgeStart + i > cur.edgeEnd)
                    {
                        Console.WriteLine("betaremain error");
                    }
                    if (s[betaStart + i] != s[cur.edgeStart + i])
                    {
                        Node<T> newInternal = cur.parent.addChild(new Node<T>(cur.parent, internalID++, betaStart, betaStart + i - 1));
                        newInternal.child = cur;
                        cur.sibling = null;
                        cur.parent = newInternal;
                        cur.edgeStart += i;
                        if (cur.depth <= 0)
                        {
                            Console.WriteLine("badnode");
                        }
                        return newInternal;
                    }
                }
            }
            return cur;
        }

        void findCommon(Node<T> n, Node<T> deepest)
        {
            if (!n.isLeaf())
            {
                if (n.depth > deepest.depth)
                {
                    deepest = n;
                }
            }
            Node<T> next = n.child;
            while (next != null)
            {
                findCommon(next, deepest);
                next = next.sibling;
            }
        }

        Node<T> FindPath(Node<T> v, int start, int end)
        {
            Node<T> next = v.getChild(s[start]);
            Node<T> parent = v;
            int i;
            while (start <= end)
            {
                if (next == null)
                {
                    return parent.addChild(new Node<T>(parent, giveMeAnID(true), start, end));
                }
                else
                {
                    for (i = 0; i < end - start + 1; i++)
                    {
                        if (next.edgeStart + i > next.edgeEnd)
                        {
                            break;
                        }
                        if (s[next.edgeStart + i] != s[start + i])
                        {
                            Node<T> newInternal = parent.addChild(new Node<T>(parent, giveMeAnID(false), start, start + i - 1));
                            next.edgeStart += i;
                            next.parent = newInternal;
                            Node<T> newLeaf = new Node<T>(newInternal, giveMeAnID(true), start + i, end);
                            newInternal.child = next;
                            next.sibling = null;
                            newInternal.addChild(newLeaf);
                            return newLeaf;
                        }
                    }
                }
                start += i;
                parent = next;
                next = next.getChild(s[start]);
            }
            Console.WriteLine("~~~~~~~THERES BEEN AN ERROR~~~~~~~\n");
            return null;
        }

        Node<T> case1a(Node<T> leaf, int start, int end)
        {
            Node<T> v = leaf.parent.link;
            return FindPath(v, start + v.depth, end);
        }

        Node<T> case1b(Node<T> leaf, int start, int end)
        {
            return FindPath(root, start, end);
        }

        Node<T> case2a(Node<T> leaf, int start, int end)
        {
            Node<T> uPrime = leaf.parent.parent;
            int betaStart = leaf.parent.edgeStart;
            int betaEnd = leaf.parent.edgeEnd;
            Node<T> vPrime = uPrime.link;
            Node<T> v = nodeHop(vPrime, betaStart, betaEnd);
            leaf.parent.link = v;
            return FindPath(v, start + v.depth, end);
        }

        Node<T> case2b(Node<T> leaf, int start, int end)
        {
            Node<T> uPrime = leaf.parent.parent;
            int betaStart = leaf.parent.edgeStart + 1;
            int betaEnd = leaf.parent.edgeEnd;
            Node<T> vPrime = uPrime.link;
            Node<T> v = nodeHop(vPrime, betaStart, betaEnd);
            leaf.parent.link = v;
            return FindPath(v, start + v.depth, end);
        }

        void addSuffixes()
        {
            Node<T> newLeaf = FindPath(root, 0, s.Count - 1);
            for (int i = 1; i < (int)s.Count; i++)
            {
                if (newLeaf.parent.link != null)
                {
                    if (newLeaf.parent != root)
                    {
                        newLeaf = case1a(newLeaf, i, s.Count - 1);
                    }
                    else
                    {
                        newLeaf = case1b(newLeaf, i, s.Count - 1);
                    }
                }
                else
                {
                    if (newLeaf.parent.parent != root)
                    {
                        newLeaf = case2a(newLeaf, i, s.Count - 1);
                    }
                    else
                    {
                        newLeaf = case2b(newLeaf, i, s.Count - 1);
                    }
                }
            }
        }
    }
}
