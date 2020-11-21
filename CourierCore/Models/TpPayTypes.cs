﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace CourierCore.Models
{
    public partial class TpPayTypes
    {
        public TpPayTypes()
        {
            TpCheckPayments = new HashSet<TpCheckPayments>();
            TpGuestDeliveries = new HashSet<TpGuestDeliveries>();
        }

        public Guid PytpId { get; set; }
        public Guid PytpPtgrId { get; set; }
        public Guid? PytpPbfmId { get; set; }
        public int PytpPytcId { get; set; }
        public Guid? PytpDelId { get; set; }
        public string PytpName { get; set; }
        public string PytpDescription { get; set; }
        public byte[] PytpSmallPicture { get; set; }
        public byte[] PytpPicture { get; set; }
        public string PytpPictureFileName { get; set; }
        public int? PytpOrder { get; set; }
        public bool PytpIsCash { get; set; }
        public bool PytpIsDisabled { get; set; }

        public virtual ICollection<TpCheckPayments> TpCheckPayments { get; set; }
        public virtual ICollection<TpGuestDeliveries> TpGuestDeliveries { get; set; }
    }
}