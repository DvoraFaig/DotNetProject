using System;
using System.Collections.Generic;
using System.Text;
using BO;


namespace BL
{
    public sealed partial class BL : BlApi.Ibl
    {
        private List<Drone> dronesInBL { get; set; }
    }
}