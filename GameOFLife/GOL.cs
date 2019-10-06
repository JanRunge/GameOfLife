using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace GameOFLife
{
    class GOL
    {
        public bool IsRunning;
        private bool stopFlag = false;
        public bool[][] GameField;
        List<bool[][]> OldGameFields = new List<bool[][]>();
        public void create(int x, int y)
        {
            GameField = new bool[x][];

            for (int i=0;i<x; i++)
            {
                GameField[i] = new bool[y];
            }
            populateRandomly();

        }
        public void create(int x, int y,bool color)
        {
            GameField = new bool[x][];

            for (int i = 0; i < x; i++)
            {
                GameField[i] = new bool[y];
            }
            populate(color);

        }

        private void populateRandomly() {
            Random r = new Random();
            foreach(bool[] row in GameField)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    row[i] = (r.Next(0, 2) == 0);
                }
            }
        }
        private void populate(bool color)
        {
            Random r = new Random();
            foreach (bool[] row in GameField)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    row[i] =color;
                }
            }
        }
        public List<int[]> nextIteration()
        {
            OldGameFields.Add(this.GameField);
            bool[][] GameField = this.GameField.Select(a => a.ToArray()).ToArray();
            //returns a list of all coords that changed
            List<int[]> l = new List<int[]>();
            for (int x = 0; x < GameField.Length; x++)
            {
                for (int y = 0; y < GameField[0].Length; y++)
                {
                    int neighbors = CountAliveNeighbors(x,y);
                    int val = getValue(x, y);
                    if (val == 0 && neighbors == 3)
                    {
                        GameField[x][y] = true;
                        l.Add(new int[] { x, y });
                    } else if (val==1 && neighbors<2) {
                        GameField[x][y] = false;
                        l.Add(new int[] { x, y });
                    }
                    else if (val == 1 && (neighbors ==2 || neighbors == 3))
                    {
                    }
                    else if (val == 1 && neighbors>3)
                    {
                        GameField[x][y] = false;
                        l.Add(new int[] { x, y });
                    }

                }
            }
            this.GameField = GameField;
            return l;
            
        }
        public int CountAliveNeighbors(int x,int y)
        {
            int sum = 0;
            sum = sum 
                + getValue(x-1, y-1)
                + getValue(x-1, y)
                + getValue(x-1, y+1)
                + getValue(x, y-1)
                + getValue(x, y+1)
                + getValue(x+1, y-1)
                + getValue(x+1, y)
                + getValue(x+1, y+1);
            return sum;
        }
        public int getValue(int x,int y)
        {

            if (x > GameField.Length - 1 || x < 0 || y > GameField[0].Length - 1 || y < 0)
            {
                return 0;
            }
            else if (GameField[x][y])
            {
                return 1;
            }
            return 0;
        }
        public void changeGamefield(int x, int y, bool val)
        {
            OldGameFields.Add(this.GameField);
            this.GameField[x][y] = val;
        }
        public void run(MainWindow m)
        {
            //run the sim
            stopFlag = false;
            IsRunning = true;
            while (!stopFlag)
            {
                List<int[]> changedFields = this.nextIteration();
                if (changedFields.Count == 0) {
                    stopFlag = true;
                }
                m.Dispatcher.Invoke(() => {
                    m.show(this.GameField, changedFields);
                });
                Thread.Sleep(300);//sleep to let the user actually see stuff
                //sleep at the end, not beginning, so that we dont change the field After we have been stopped
                
            }
            IsRunning = false;
        }
        public void runOnce(MainWindow m)
        {
            //show the next field in the sim
            List<int[]> l = this.nextIteration();
            m.Dispatcher.Invoke(() => {
                m.show(this.GameField, l);
            });
        }
        public void stop() {
            //stop the sim
            stopFlag = true;
            //do not answer before the execution got stopped
            while (IsRunning)
            {
                Thread.Sleep(50);
            }
            return;
        }
        public void rewindOnce(MainWindow m)
        {
            //show the previous field
            bool[][] t = PreviousField();
            m.Dispatcher.Invoke(() => {
                m.show(this.GameField);
            });
        }
        private bool[][] PreviousField() //get the last Field before the current one
        {
            if (OldGameFields.Count<1)
            {
                return null;
            }
            this.GameField = OldGameFields[OldGameFields.Count - 1];

            OldGameFields.RemoveAt(OldGameFields.Count - 1); 
            //remove the most current Historyentry
            //we could keep this and reuse it later, but thats too much effort for such low performance gain
            return this.GameField;

        }

        
        public void rewind(MainWindow m) {
            
            stopFlag = false;
            IsRunning = true;
            while (!stopFlag)
            {
                
                PreviousField();
                m.Dispatcher.Invoke(() => {
                    m.show(this.GameField);
                });
                Thread.Sleep(300);
            }
            IsRunning = false;
        }
    }
}
