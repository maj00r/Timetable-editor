using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RJ_Editor
{
    class Waypoint
    {
        
        private double _position;
        private string _name;
        public Waypoint()
        {

        }
        public double Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public string Name
        {
            get; set;
        }
    }
    class Platform : Waypoint
    {
        private string _name;
        private int _type; // 1 main station, 2 other stops
        private bool _trainFront; // true - front of train stops at this position, false - end
        public Platform()
        {

        }
       
        public int Type
        {
            get; set;
        }
        public bool TrainFront
        {
            get; set;
        }
    }
    class SpeedChange : Waypoint
    {
        private int _newSpeed;
        private bool _trainFront; // true - change speed if front of train is at this position - else - end
        private double _distanceBuffer; // if train length > this value : change speed at (position - this)
        public SpeedChange()
        {

        }
        public int NewSpeed { get; set; }
        public bool TrainFornt { get; set; }
        public double DistanceBuffer { get; set; }

    }
    class MilepostChange : Waypoint
    {
        private double _newMilepost;
        public MilepostChange()
        {

        }
        public double NewMilepost { get; set; }
    }
}
