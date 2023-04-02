using System;
using Application.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Identity
{
	public class GymWorker
	{
        public Guid Id { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
