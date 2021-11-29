﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;

namespace IDal
{
    namespace DO
    {
        public struct Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; } //not here
            public override string ToString()
            {
                return ($"drone id: {Id}, drone model{Model}\n");
            }
        }
    }
}
