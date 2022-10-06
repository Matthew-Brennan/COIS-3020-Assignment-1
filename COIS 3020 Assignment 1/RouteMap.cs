using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_3020_Assignment_1
{
    public class AirportNode
    {
        public string name;
        public string code;
        public List<AirportNode> destinations;

        //Constructor
        public AirportNode(string name, string code)
        {
            this.name = name;
            this.code = code;
            this.destinations = new List<AirportNode>();
        }

        //properties
        public string Name
        {
            get { return name; }
        }

        public string Code
        {
            get { return code; }
        }

        public List<AirportNode> Destinations
        {
            get { return destinations; }
        }

        //Methods
        public bool AddDestination(AirportNode destAirport)
        {
            //if the airport is already a destination don't allow a duplicate
            if (Destinations.Contains(destAirport))
            {
                return false;
            }
            else
            {
                Destinations.Add(destAirport);
                return true;
            }

        }


        public bool RemoveDestination(AirportNode destAirport)
        {
            //if the airport is a valid destination, remove it
            if (Destinations.Contains(destAirport))
            {
                Destinations.Remove(destAirport);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            string nodeString = "";
            nodeString += "Airport " + name + " with Destinations: ";
            for (int i = 0; i < Destinations.Count; i++)
            {
                nodeString += "\n\t" + Destinations[i].Name + "  | " + Destinations[i].Code;
            }
            //nodeString += "\n";
            return nodeString;
        }


    }






    //Class RouteMap
    public class RouteMap<T>
    {
        //fields
        public List<AirportNode> nodes = new List<AirportNode>();

        //default constructor
        public RouteMap()
        {

        }

        //properties
        public int Count { get { return nodes.Count; } }

        IList<AirportNode> Nodes { get { return nodes; } }

        //find airport by name
        public AirportNode FindAirportName(string name)
        {
            foreach (AirportNode node in nodes)
            {
                if (node.Name.Equals(name))
                {
                    return node;
                }
            }
            return null;
        }

        public AirportNode FindAirportCode(string code)
        {
            foreach (AirportNode node in nodes)
            {
                if (node.Code.Equals(code))
                {
                    return node;
                }
            }
            return null;
        }

        public bool AddAirport(AirportNode value)
        {
            if (FindAirportName(value.Name) != null)
            {
                return false;
            }
            if (FindAirportName(value.Code) != null)
            {
                return false;
            }
            else
            {
                nodes.Add(new AirportNode(value.Name, value.Code));
                return true;
            }
        }


        public bool RemoveNode(AirportNode value)
        {
            AirportNode nodeToRemove = FindAirportName(value.name);
            if (nodeToRemove == null)
            {
                return false;
            }
            else
            {
                nodes.Remove(nodeToRemove);

                //we need to remove the node from Airport list in each graph node
                foreach (AirportNode node in nodes)
                {
                    node.RemoveDestination(nodeToRemove);
                }
                return true;
            }
        }

        public bool AddRoute(AirportNode origin, AirportNode dest)
        {
            AirportNode node1 = origin;
            AirportNode node2 = dest;

            if (node1 == null || node2 == null || node1.Equals(node2))
            {
                return false;
            }
            else if (node1.Destinations.Contains(node2))
            {
                return false;
            }
            else
            {
                node1.AddDestination(node2);
                //node2.AddDestination(node1);
                return true;
            }
        }

        public bool RemoveRoute(AirportNode origin, AirportNode dest)
        {
            AirportNode node1 = origin;
            AirportNode node2 = dest;
            if (node1 == null || node2 == null)
            {
                return false;
            }
            else if (!node1.Destinations.Contains(node2))
            {
                return false;
            }
            else
            {
                node1.RemoveDestination(node2);
                //node2.RemoveDestination(node1);
                return true;
            }

        }

        public override string ToString()
        {
            string nodesString = "";
            for (int i = 0; i < Count; i++)
            {
                nodesString += nodes[i].ToString();
                if (i < Count - 1)
                {
                    nodesString += "\n";
                }
            }

            return nodesString;
        }

        public string FastestRoute(AirportNode origin, AirportNode destination)
        {
            String returnString = "";
            if (origin == null || destination == null)
            {
                //this should never happen
                return "Missing Airport";
            }
            else if (origin == destination)
            {
                return origin.ToString();
            }
            else
            {
                var queue = new Queue<AirportNode>();
                var previous = new Dictionary<AirportNode, AirportNode>();

                queue.Enqueue(origin);

                while (queue.Count > 0)
                {
                    var vertex = queue.Dequeue();
                    foreach (var city in vertex.Destinations)
                    {
                        if (previous.ContainsKey(city))
                        {
                            continue;
                        }
                        previous[city] = vertex;
                        queue.Enqueue(city);
                    }
                }


                var path = new List<AirportNode>();

                var current = destination;
                while (!current.Equals(origin))
                {
                    //if theres no route to the destination finish
                    if(!previous.ContainsKey(current))
                    {
                        return ("No route to "+ destination.Name + " from "+ origin.Name);
                    }
                    path.Add(current);
                    current = previous[current];

                };

                path.Add(origin);
                path.Reverse();

                //Format the return string
                returnString = "\nThe Shortest path from "+ origin.Name +" to " + destination.Name + ":";
                foreach (var i in path)
                {
                    if (i.Equals(path.Last()))
                    {
                        returnString += i.Name;
                    }
                    else
                    {
                        returnString += i.Name + " => ";
                    }
                }
                return returnString;
            }
        }


    }
}

