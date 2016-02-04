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
            {
                return new ConvexHull(points);
            }

            //Splt the points collection into two halves
            ConvexHull left = SplitAndCombine(points.GetRange(0, (int)Math.Floor((double)points.Count / 2)));
            ConvexHull right = SplitAndCombine(points.GetRange((int)Math.Floor((double)points.Count / 2), (int)Math.Ceiling((double)points.Count / 2)));



            return null;
        }

        public void MergeCVH(Object leftCVH, Object rightCVH)
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
        PointF[] points;
        int size;
        int currentLoc;

        public ConvexHull(List<PointF> pointsList)
        {
            size = pointsList.Count;
            points = new PointF[size];
            currentLoc = 0;

            for (int i = 0; i < pointsList.Count; i++)
                points[i] = pointsList[i];
        }

        //Advances the current location index by one (circular) and returns the Point Object at current Location
        public PointF advance()
        {
            if (currentLoc == (size - 1))
                currentLoc = 0;
            else
                currentLoc++;

            return points[currentLoc];
        }

        public PointF current()
        {
            return points[currentLoc];
        }

    }
}
