using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace _2_convex_hull
{
    class ConvexHullSolver
    {
        System.Drawing.Graphics g;
        System.Windows.Forms.PictureBox pictureBoxView;

        public ConvexHullSolver(System.Drawing.Graphics g, System.Windows.Forms.PictureBox pictureBoxView)
        {
            this.g = g;
            this.pictureBoxView = pictureBoxView;
        }

        public void Refresh()
        {
            // Use this especially for debugging and whenever you want to see what you have drawn so far
            pictureBoxView.Refresh();
        }

        public void Pause(int milliseconds)
        { 
            // Use this especially for debugging and to animate your algorithm slowly
            pictureBoxView.Refresh();
            System.Threading.Thread.Sleep(milliseconds);
        }

        public void Solve(List<System.Drawing.PointF> pointList)
        {
            // TODO: Insert your code here

            //Sort points from left to right
            pointList.Sort(new PointComparitor());

            ConvexHull final = SplitAndCombine(pointList);

        }

        public ConvexHull SplitAndCombine(List<System.Drawing.PointF> points)
        {
            //DIAG: output the points
            foreach (System.Drawing.PointF point in points)
                Console.WriteLine(point.ToString());
            Console.WriteLine();

            //If there is only one point in the list of points, we have reached base case, return a CVH with just that point
            if (points.Count == 1)
                return new ConvexHull(points[0]);

            Console.WriteLine("A");

            //Splt the points collection into two halves
            ConvexHull left = SplitAndCombine(points.GetRange(0, (int)Math.Floor((double)points.Count / 2)));
            ConvexHull right = SplitAndCombine(points.GetRange((int)Math.Floor((double)points.Count / 2), (int)Math.Ceiling((double)points.Count / 2)));

            Console.WriteLine("B");

            //Merge the two halves together
            ConvexHull combinec = MergeCVH(left, right);

            Console.WriteLine("C");

            return null;
        }

        public ConvexHull MergeCVH(ConvexHull leftCVH, ConvexHull rightCVH)
        {
            //calculate topTanLeftPt and topTanRightPt
            calcTopTan(leftCVH, rightCVH);

            //calculate botTanLeftPt and botTanRightPt
            calcBotTan(leftCVH, rightCVH);

            //create new CVH with starting point of the left most point of leftCVH
            ConvexHull newHull = new ConvexHull(leftCVH.getLeftPoint());
            leftCVH.moveToLeft();
            //counter through LCVH until you add botTanLeftPt
            while(leftCVH.getCurrentIndex() != leftCVH.getBotTanIndex())
            {
                newHull.add(leftCVH.counter());
            }

            //add RCVH botTan point to new CVH
            newHull.add(rightCVH.getBotTanPoint());

            //move RCVH to botTan point
            rightCVH.moveToBotTan();

            //counter through RCVH until you add topTanRightPt
            while (rightCVH.getCurrentIndex() != rightCVH.getTopTanIndex())
            {
                newHull.add(rightCVH.counter());
            }

            //add LCVH topTan to the new CVH
            newHull.add(leftCVH.getBotTanPoint());

            //move LCVH to topTan point
            leftCVH.moveToTopTan();

            //add until you encounter leftCVH
            while (leftCVH.getCurrentIndex() != leftCVH.getLeftIndex())
            {
                newHull.add(leftCVH.counter());
            }
            newHull.delete(newHull.size() - 1);

            //return
            return newHull;
        }

        //function updates the location of the topTan points for both left and right
        public void calcTopTan(ConvexHull leftCVH, ConvexHull rightCVH)
        {
            leftCVH.setTopTan(leftCVH.getRightIndex());
            rightCVH.setTopTan(rightCVH.getLeftIndex());

            while (!leftToRightTop(leftCVH, rightCVH) || !rightToLeftTop(leftCVH, rightCVH)) { }
        }

        public Boolean leftToRightTop(ConvexHull leftCVH, ConvexHull rightCVH)
        {
            int runCount = 0;

            double slope = 0;

            double deltaX = rightCVH.getTopTanPoint().X - leftCVH.getTopTanPoint().X;
            double deltaY = rightCVH.getTopTanPoint().Y - leftCVH.getTopTanPoint().Y;
            slope = deltaY / deltaX;

            Boolean hasDecreased = false;
            rightCVH.moveToTopTan();
            
            while (!hasDecreased)
            {
                runCount++;
                //calc the slope for the next point clockwise on right
                deltaX = rightCVH.clock().X - leftCVH.getTopTanPoint().X;
                deltaY = rightCVH.getCurrentPoint().Y - leftCVH.getTopTanPoint().Y;

                //if the new slope decrecess
                if((deltaY/deltaX) - slope <= 0)
                {
                    hasDecreased = true;
                    //step back
                    rightCVH.counter();
                    //assign the new index for topTanRight
                    rightCVH.setTopTan(rightCVH.getCurrentIndex());

                    if(runCount == 1)
                        return true;
                }
                else
                {
                    slope = deltaY / deltaX;
                }   
            }

            return false;
        }

        public Boolean rightToLeftTop(ConvexHull leftCVH, ConvexHull rightCVH)
        {
            double slope = 0;
            int runCount = 0;

            double deltaX = rightCVH.getTopTanPoint().X - leftCVH.getTopTanPoint().X;
            double deltaY = rightCVH.getTopTanPoint().Y - leftCVH.getTopTanPoint().Y;
            slope = deltaY / deltaX;

            Boolean hasIncreased = false;
            leftCVH.moveToTopTan();

            while (!hasIncreased)
            {
                runCount++;

                //calc the slope for the next point counter on left
                deltaX = rightCVH.getTopTanPoint().X - leftCVH.counter().X;
                deltaY = rightCVH.getTopTanPoint().Y - leftCVH.getCurrentPoint().Y;

                //if the new slope decrecess
                if ((deltaY / deltaX) - slope >= 0)
                {
                    hasIncreased = true;
                    //step back
                    leftCVH.clock();
                    //assign the new index for topTanRight
                    leftCVH.setTopTan(leftCVH.getCurrentIndex());

                    if (runCount == 1)
                        return true;
                }
                else
                {
                    slope = deltaY / deltaX;
                }
            }

            return false;
        }


        //function updates the location of the topTan points for both left and right
        public void calcBotTan(ConvexHull leftCVH, ConvexHull rightCVH)
        {
            leftCVH.setBotTan(leftCVH.getRightIndex());
            rightCVH.setBotTan(rightCVH.getLeftIndex());

            while (!leftToRightBot(leftCVH, rightCVH) || !rightToLeftBot(leftCVH, rightCVH)) { }
        }

        public Boolean leftToRightBot(ConvexHull leftCVH, ConvexHull rightCVH)
        {
            int runCount = 0;

            double slope = 0;

            double deltaX = rightCVH.getBotTanPoint().X - leftCVH.getBotTanPoint().X;
            double deltaY = rightCVH.getBotTanPoint().Y - leftCVH.getBotTanPoint().Y;
            slope = deltaY / deltaX;

            Boolean hasIncreased = false;
            rightCVH.moveToBotTan();

            while (!hasIncreased)
            {
                runCount++;
                //calc the slope for the next point clockwise on right
                deltaX = rightCVH.counter().X - leftCVH.getBotTanPoint().X;
                deltaY = rightCVH.getCurrentPoint().Y - leftCVH.getBotTanPoint().Y;

                //if the new slope decrecess
                if ((deltaY / deltaX) - slope >= 0)
                {
                    hasIncreased = true;
                    //step back
                    rightCVH.clock();
                    //assign the new index for topTanRight
                    rightCVH.setBotTan(rightCVH.getCurrentIndex());

                    if (runCount == 1)
                        return true;
                }
                else
                {
                    slope = deltaY / deltaX;
                }
            }

            return false;
        }

        public Boolean rightToLeftBot(ConvexHull leftCVH, ConvexHull rightCVH)
        {
            double slope = 0;
            int runCount = 0;

            double deltaX = rightCVH.getBotTanPoint().X - leftCVH.getBotTanPoint().X;
            double deltaY = rightCVH.getBotTanPoint().Y - leftCVH.getBotTanPoint().Y;
            slope = deltaY / deltaX;

            Boolean hasDecreased = false;
            leftCVH.moveToBotTan();

            while (!hasDecreased)
            {
                runCount++;

                //calc the slope for the next point counter on left
                deltaX = rightCVH.getBotTanPoint().X - leftCVH.clock().X;
                deltaY = rightCVH.getBotTanPoint().Y - leftCVH.getCurrentPoint().Y;

                //if the new slope decrecess
                if ((deltaY / deltaX) - slope <= 0)
                {
                    hasDecreased = true;
                    //step back
                    leftCVH.counter();
                    //assign the new index for topTanRight
                    leftCVH.setBotTan(leftCVH.getCurrentIndex());

                    if (runCount == 1)
                        return true;
                }
                else
                {
                    slope = deltaY / deltaX;
                }
            }

            return false;
        }


























        class PointComparitor : IComparer<PointF>{
            public int Compare(PointF x, PointF y){
                if (x.X < y.X)
                    return -1;
                else
                    return 1;
            }
        }
    }

    class ConvexHull
    {
        List<PointF> cvhPoints;
        int currentLoc;

        int leftPoint;
        int rightPoint;

        int botTan;
        int topTan;

        public ConvexHull(PointF point)
        {
            cvhPoints = new List<PointF>();
            cvhPoints.Add(point);
            currentLoc = 0;
            leftPoint = 0;
            rightPoint = 0;
        }

        public void add(PointF point)
        {
            double leftX = cvhPoints[leftPoint].X;
            double rightX = cvhPoints[rightPoint].X;

            cvhPoints.Add(point);
            if (point.X < leftX)
                leftPoint = cvhPoints.Count - 1;

            if (point.X > rightX)
                rightPoint = cvhPoints.Count - 1;
        }

        public void delete(int index)
        {
            cvhPoints.RemoveAt(index);
        }

        public int size()
        {
            return cvhPoints.Count;
        }


        //Advances the current location index by one (circular) and returns the Point Object at current Location
        public PointF counter()
        {
            if (currentLoc == (cvhPoints.Count - 1))
                currentLoc = 0;
            else
                currentLoc++;

            return cvhPoints[currentLoc];
        }

        public PointF clock()
        {
            if (currentLoc == (0))
                currentLoc = cvhPoints.Count - 1;
            else
                currentLoc--;

            return cvhPoints[currentLoc];
        }



        //Things pertaining to current point
        public int getCurrentIndex()
        {
            return currentLoc;
        }


        public PointF getCurrentPoint()
        {
            return cvhPoints[currentLoc];
        }

        //Things pertaining to the left most point
        public void moveToLeft()
        {
            currentLoc = leftPoint;
        }

        public PointF getLeftPoint()
        {
            return cvhPoints[leftPoint];
        }

        public int getLeftIndex()
        {
            return leftPoint;
        }

        //Things pertaining to the right most point
        public void moveToRight()
        {
            currentLoc = rightPoint;
        }

        public PointF getRightPoint()
        {
            return cvhPoints[rightPoint];
        }

        public int getRightIndex()
        {
            return rightPoint;
        }

        

        
        //TopTan
        public void moveToTopTan()
        {
            currentLoc = topTan;
        }

        public void setTopTan(int newTopTan)
        {
            topTan = newTopTan;
        }

        public PointF getTopTanPoint()
        {
            return cvhPoints[topTan];
        }

        public int getTopTanIndex()
        {
            return topTan;
        }

        //BotTan
        public void moveToBotTan()
        {
            currentLoc = botTan;
        }

        public void setBotTan(int newBotTan)
        {
            botTan = newBotTan;
        }

        public PointF getBotTanPoint()
        {
            return cvhPoints[botTan];
        }

        public int getBotTanIndex()
        {
            return botTan;
        }

    }
}
