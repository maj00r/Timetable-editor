using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RJ_Editor
{
    class TrainDriver
    {
        // there is a random delay when the train is leaving neighbour station
        // ~ 20 sec.
        //private static DateTime appearDelay = new DateTime(0, 0, 0, 0, 0, 20);
        public static int[] brakingDistance = new int[161];
        private static double DECELERATION = 1.45; // m/ss
        /*
         * This class is using metric system except Waypoint.Position values (kilometers)
         */
        private double velocity;
        private int distance, lengthMetric, lengthAxes, power, massTotal, massLoco;
        private bool isElectric;

        public double Velocity { get => velocity; set => velocity = value / 3.6; }
        public int Distance { get; set; }
        public int LengthMetric
        {
            get { return lengthMetric; }
            set { lengthMetric = lengthAxes * 5; }
        }
        public int LenghtAxes
        {
            get { return lengthAxes; }
            set 
            {
                lengthAxes = 13;
            }
        }


        /*  s = a tt /2
         *  a= v/t
         *  t = v/a 
         * s = a* (v/a)^2   /   2
         * 
         */

        public static void fillBrakingDistance()
        {
            for(int i =0; i <brakingDistance.Length; i++)
            {
                double velocity = i / 3.6;
                brakingDistance[i] = (int)Math.Ceiling(velocity * velocity  / (2 * DECELERATION));
            }
        }
        public TrainDriver(Train train)
        {
           // this.Velocity = train.ExitVelocity;
           // this.Distance = 0;

        }
    }
}
