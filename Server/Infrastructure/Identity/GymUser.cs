﻿using Application.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Identity
{
    public class GymUser
    {
        public Guid Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime ExpiresOn { get; set; }

        [Required]
        [DefaultValue("1")]
        public GymUserType Type { get; set; }

        //[DefaultValue(typeof(DateTime), "01/01/0001")] // TODO
        public DateTime LastCheckIn { get; set; }

        [Required]
        [DefaultValue("false")]
        public bool IsEmailConfirmed { get; set; }

        [Required]
        [DefaultValue("false")]
        public bool IsFrozen { get; set; }

        [Required]
        [DefaultValue("false")]
        public bool IsInActive { get; set; }

        [Required]
        [DefaultValue(0)]
        public int NumberOfArrivals { get; set; }

        //[DefaultValue(typeof(DateTime), "01/01/0001")] // TODO
        public DateTime FreezeDate { get; set; }
    }
}
