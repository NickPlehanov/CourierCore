﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace CourierCore.Models
{
    public partial class TpPreChecks
    {
        public TpPreChecks()
        {
            TpChecks = new HashSet<TpChecks>();
            TpPreCheckItems = new HashSet<TpPreCheckItems>();
        }

        public Guid PrchId { get; set; }
        public Guid PrchDvsnId { get; set; }
        public Guid PrchArchId { get; set; }
        public Guid PrchUsrId { get; set; }
        public Guid PrchDevId { get; set; }
        public int PrchPcstId { get; set; }
        public int PrchPrchtypId { get; set; }
        public DateTime PrchDate { get; set; }
        public string PrchName { get; set; }

        public virtual TpUsers PrchUsr { get; set; }
        public virtual ICollection<TpChecks> TpChecks { get; set; }
        public virtual ICollection<TpPreCheckItems> TpPreCheckItems { get; set; }
    }
}