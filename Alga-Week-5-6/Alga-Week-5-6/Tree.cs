using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alga_Week_5_6
{

    public class Tree
    {
        public Room Data { get; }
        private LinkedList<Tree> children;

        public Tree(Room data)
        {
            this.Data = data;
            children = new LinkedList<Tree>();
        }

        public void AddChild(Room data)
        {
            AddTree(new Tree(data));
        }

        public void AddTree(Tree tree)
        {
            children.AddFirst(tree);
        }

        public Tree GetChild(Room child)
        {
            if (Data == child)
                return this;

            foreach (Tree n in children)
            {
                Tree foundChild = n.GetChild(child);

                if (foundChild != null)
                    return foundChild;
            }
                
            return null;
        }
    }
}