using System;
using System.Collections.Generic;
using System.Text;
using IBL.BO;


namespace BL
{
    public sealed partial class BL : IBL.Ibl
    {
        private List<Drone> dronesInBL { get; set; }
    }
}