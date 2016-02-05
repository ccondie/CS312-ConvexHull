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
            

            //Splt the points collection into two halves
            ConvexHull left = SplitAndCombine(points.GetRange(0, (int)Math.Floor((double)points.Count / 2)));
            ConvexHull right = SplitAndCombine(points.GetRange((int)Math.Floor((double)points.Count / 2), (int)Math.Ceiling((double)points.Count / 2)));

            //Merge the two halves together
            ConvexHull combinec = MergeCVH(left, right);


            return null;
        }

        public ConvexHull MergeCVH(ConvexHull leftCVH, ConvexHull rightCVH)
        {
            //calculate topTanLeftPt and topTanRightPt

            //calculate botTanLeftPt and botTanRightPt

            //create new CVH with starting point of the left most point of leftCVH
            //counter through LCVH until you add botTanLeftPt
            //add RCVH botTan point to new CVH
            //move RCVH to botTan point
            //counter through RCVH until you add topTanRightPt
            //add LCVH topTan to the new CVH
            //move LCVH to topTan point
            //add until you encounter leftCVH

            //return

            return null;
        }

        //function updates the location of the topTan points for both left and right
        public void calcTopTan(ConvexHull leftCVH, ConvexHull rightCVH)
        {

        }

        //function updates the location of the botTan points for both left and right
        public void calcBotTan(ConvexHull leftCVH, ConvexHull rightCVH)
        {

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

        public void goToLeft()
        {
            currentLoc = leftPoint;
        }

        public void goToRight()
        {
            currentLoc = rightPoint;
        }

        public PointF current()
        {
            return cvhPoints[currentLoc];
        }

        public void add(PointF point)
        {
            double leftX = cvhPoints[leftPoint].X;
            double rightX = cvhPoints[rightPoint].X;

            cvhPoints.Add(point);
            if (point.X < leftX)
                leftX = cvhPoints.Count - 1;

            if (point.X > rightX)
                rightX = cvhPoints.Count - 1;
        }

    }
}
