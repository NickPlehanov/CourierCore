﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace CourierCore.Models
{
    public partial class TpCheckPayments
    {
        public Guid ChpyId { get; set; }
        public Guid ChpyChckId { get; set; }
        public Guid ChpyPytpId { get; set; }
        public Guid? ChpyAtopId { get; set; }
        public int ChpyChpcId { get; set; }
        public decimal ChpySum { get; set; }

        public virtual TpChecks ChpyChck { get; set; }
        public virtual TpPayTypes ChpyPytp { get; set; }
    }
}