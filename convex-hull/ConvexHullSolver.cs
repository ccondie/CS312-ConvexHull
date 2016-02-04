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
            List<System.Drawing.PointF> sortedPoints = pointList.Sort();
            foreach(System.Drawing.PointF point in pointList)
            {
                Console.WriteLine(point.ToString());
            }

        }

        public void SolveCVHLeft(List<System.Drawing.PointF> pointList)
        {

        }

        public void SolveCVHRight()
        {

        }

        public void MergeCVH(Object leftCVH, Object rightCVH)
        {

        }


    }
}
