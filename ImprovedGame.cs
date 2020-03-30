using System;

public class AnimalTree
{
    Node root;

    // Tree Node 
    public class Node
    {
        public string content;
        public int data;
        public Node yes;
        public Node no;
        public Node(int data)
        {
            this.data = data;
            this.no = null;
            this.yes = null;
        }
        public void DisplayNode()
        {
            Console.Write(content + " ");
        }
    }

    //inserting the nodes in level order.
    public Node insertLevelOrder(int[] array, Node root, int i)
    {
        if (i < array.Length)
        {
            Node temp = new Node(array[i]);
            root = temp;

            // inserting the no (left) decision. the formula for finding node data in the array is 2 * i + 1  
            root.no = insertLevelOrder(array, root.no, 2 * i + 1);

            // inserting the yes (right)  decision. the formula for the finding the node data in the array is 2 * i + 2
            root.yes = insertLevelOrder(array, root.yes, 2 * i + 2);
        }
        return root;
    }

    //The inOrder Node makes it easier orgainze the text file 
    public void inOrder(Node root,string[] questions)
    {
        if (root != null)
        {
            root.content = questions[root.data];
            inOrder(root.no, questions);
            inOrder(root.yes, questions);
        }
    }

    // Driver code 
    public static void Main(String[] args)
    {
        //String questions
        string[] questions = System.IO.File.ReadAllLines(@"C:\Users\Bradley\Desktop\improvedAnimalData.txt");
        AnimalTree tree = new AnimalTree();


        //setting up array for tree building
        int[] treeArray = new int[questions.Length];

        for(int i=0; i <questions.Length; i++)
        {
            treeArray[i] = i;
        }

        tree.root = tree.insertLevelOrder(treeArray, tree.root, 0);
        tree.inOrder(tree.root, questions);

        Node tempNode = new Node(0);
        bool activeGame = true;



        //The game continues to go until the While loop is broken
        while (activeGame == true)
        {

            int i = tempNode.data;
            string decision;
            string newQuestion;
            string newAnimal;

            if (i == 0)
            {
                tree.root.DisplayNode();
                Console.WriteLine("\t Yes or No");
                decision = Console.ReadLine();
                if (decision == "Yes")
                {
                    i = tree.root.yes.data;
                    tempNode = tree.root.yes;
                }
                else
                {
                    i = tree.root.no.data;
                    tempNode = tree.root.no;
                }
            }

            //display the root node, and start from there
            tempNode.DisplayNode();


            //Code wise see if the answer has a '*' at the start of the string text
            //Logic wise see if is an animal instead of a question
            if (tempNode.content[0] == '*')
            {

                Console.WriteLine("\t yes or no");
                decision = Console.ReadLine();

                //Game is over the user has found their animal
                if(decision == "yes")
                {
                    Console.WriteLine("\t GameOver!");
                }

                //Update the question
                else
                {
                    Console.WriteLine("\t What animal were you looking for?");
                    newAnimal = Console.ReadLine();


                    //after the new animal is inputted form it into a question.
                    newAnimal = "**is it a " + newAnimal;

                    //Find a new question to ask the user 
                    Console.WriteLine("What is a question that I can use to differentiate between my guess and your answer");
                    newQuestion = Console.ReadLine();


                    //Update the Questions array before we save the array to the text file at the end of the Main
                    //I'm going to update the array using the same method we used to insert new "leaf" nodes. 
                    questions[i] = newQuestion;
                    questions[2 * i + 2] = newAnimal;
                    questions[2 * i + 1] = tempNode.content;

                }

                // break out of the game, and end the while loop
                activeGame = false;
                break;
            }


            //Take the user answer and make a choice from there 
            Console.WriteLine("\t yes or no");
            decision = Console.ReadLine();
            if(decision == "yes")
            {
                i = tempNode.yes.data;
                tempNode = tempNode.yes;
            }
            else
            {
                i = tempNode.no.data;
                tempNode = tempNode.no;
            }
        }

        //Saves the questions array back to the text file
        System.IO.File.WriteAllLines(@"C:\Users\Bradley\Desktop\improvedAnimalData.txt", questions);
    }
}