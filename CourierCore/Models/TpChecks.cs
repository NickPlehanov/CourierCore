﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace CourierCore.Models
{
    public partial class TpChecks
    {
        public TpChecks()
        {
            TpCheckPayments = new HashSet<TpCheckPayments>();
        }

        public Guid ChckId { get; set; }
        public Guid ChckUsrId { get; set; }
        public Guid ChckDevId { get; set; }
        public Guid ChckPrchId { get; set; }
        public int ChckChcktypId { get; set; }
        public Guid? ChckFsopId { get; set; }
        public Guid? ChckDevIdPrinter { get; set; }
        public int? ChckDostId { get; set; }
        public int ChckChstId { get; set; }
        public DateTime ChckDate { get; set; }
        public string ChckName { get; set; }
        public decimal? ChckChange { get; set; }

        public virtual TpPreChecks ChckPrch { get; set; }
        public virtual TpUsers ChckUsr { get; set; }
        public virtual ICollection<TpCheckPayments> TpCheckPayments { get; set; }
    }
}