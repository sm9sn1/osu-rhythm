using System;

namespace ppcalc
{
    class SuffixTrie
    {
        class Node
        {
            public Node child;
            public Node sibling;
            public Node link;
            public Node parent;
            public int ID;
            public int edgeStart;
            public int edgeEnd;
            public int stringDepth;

            public Node(int _ID)
            {
                link = parent = child = sibling = null;
                ID = _ID;
                stringDepth = edgeStart = edgeEnd = 0;
            }

            public Node(Node _parent, int _ID, int _start, int _end)
            {
                link = sibling = child = null;
                parent = _parent;
                ID = _ID;
                edgeEnd = _end;
                edgeStart = _start;
                stringDepth = parent.stringDepth + (_end - _start) + 1;
                if (stringDepth <= 0)
                {
                    Console.WriteLine("badnode");
                }
            }

            public Node getChild(char c)
            {
                /*if (this.child == null)
                {
                    return null;
                }
                else
                {*/
                    Node temp = this.child;
                    while (temp != null && s[temp.edgeStart] != c)
                    {
                        temp = temp.sibling;
                    }
                    //null if not found or desired node
                    return temp;
                //}
            }

            public Node addChild(Node newNode)
            {
                if (this.child == null)
                {
                    this.child = newNode;
                }
                else
                {
                    if (s[newNode.edgeStart] < s[this.child.edgeStart])
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
                        Node temp = this.child.sibling;
                        Node prevTemp = this.child;
                        while (temp != null && s[newNode.edgeStart] > s[temp.edgeStart])
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
        static string s;
        static string alphabet;
		Node root;

        public SuffixTrie(string _s, string _Alphabet)
        {
            s = _s;
            alphabet = _Alphabet;
            internalID = s.Length + 1;
            root = new Node(giveMeAnID(true));
            root.link = root.parent = root;
            addSuffixes();
        }

        int giveMeAnID(bool isLeafID)
        {
            return isLeafID ? leafID++ : internalID++;
        }

        void printChildren(Node n)
        {
            if (n.isLeaf())
            {
                Console.WriteLine("Leaf ID = " + n.ID);
                return;
            }
            Node next = n.child;
            while (next != null)
            {
                printChildren(next);
                next = next.sibling;
            }
        }

        Node nodeHop(Node vPrime, int betaStart, int betaEnd)
        {
            Node cur = vPrime;
            Node temp;
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
                        Node newInternal = cur.parent.addChild(new Node(cur.parent, internalID++, betaStart, betaStart + i - 1));
                        newInternal.child = cur;
                        cur.sibling = null;
                        cur.parent = newInternal;
                        cur.edgeStart += i;
                        if (cur.stringDepth <= 0)
                        {
                            Console.WriteLine("badnode");
                        }
                        return newInternal;
                    }
                }
            }
            return cur;
        }

        void findCommon(Node n, Node deepest)
        {
            if (!n.isLeaf())
            {
                if (n.stringDepth > deepest.stringDepth)
                {
                    deepest = n;
                }
            }
            Node next = n.child;
            while (next != null)
            {
                findCommon(next, deepest);
                next = next.sibling;
            }
        }

        Node FindPath(Node v, int start, int end)
        {
            Node next = v.getChild(s[start]);
            Node parent = v;
            int i;
            while (start <= end)
            {
                if (next == null)
                {
                    return parent.addChild(new Node(parent, giveMeAnID(true), start, end));
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
                            Node newInternal = parent.addChild(new Node(parent, giveMeAnID(false), start, start + i - 1));
                            next.edgeStart += i;
                            if (next.stringDepth <= 0)
                            {
                                Console.WriteLine("badnode");
                            }
                            next.parent = newInternal;
                            Node newLeaf = new Node(newInternal, giveMeAnID(true), start + i, end);
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
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~THERES BEEN AN ERROR~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
            return null;
        }

        Node case1a(Node leaf, int start, int end)
        {
            Node v = leaf.parent.link;
            return FindPath(v, start + v.stringDepth, end);
        }

        Node case1b(Node leaf, int start, int end)
        {
            return FindPath(root, start, end);
        }

        Node case2a(Node leaf, int start, int end)
        {
            Node uPrime = leaf.parent.parent;
            int betaStart = leaf.parent.edgeStart;
            int betaEnd = leaf.parent.edgeEnd;
            Node vPrime = uPrime.link;
            Node v = nodeHop(vPrime, betaStart, betaEnd);
            leaf.parent.link = v;
            return FindPath(v, start + v.stringDepth, end);
        }

        Node case2b(Node leaf, int start, int end)
        {
            Node uPrime = leaf.parent.parent;
            int betaStart = leaf.parent.edgeStart + 1;
            int betaEnd = leaf.parent.edgeEnd;
            Node vPrime = uPrime.link;
            Node v = nodeHop(vPrime, betaStart, betaEnd);
            leaf.parent.link = v;
            return FindPath(v, start + v.stringDepth, end);
        }

        void addSuffixes()
        {
            Node newLeaf = FindPath(root, 0, s.Length - 1);
            for (int i = 1; i < (int)s.Length; i++)
            {
                if (newLeaf.parent.link != null)
                {
                    if (newLeaf.parent != root)
                    {
                        newLeaf = case1a(newLeaf, i, s.Length - 1);
                    }
                    else
                    {
                        newLeaf = case1b(newLeaf, i, s.Length - 1);
                    }
                }
                else
                {
                    if (newLeaf.parent.parent != root)
                    {
                        newLeaf = case2a(newLeaf, i, s.Length - 1);
                    }
                    else
                    {
                        newLeaf = case2b(newLeaf, i, s.Length - 1);
                    }
                }
            }
        }
    }
}
