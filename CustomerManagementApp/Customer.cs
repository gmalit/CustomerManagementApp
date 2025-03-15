namespace CustomerManagementApp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public int Age { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; }

        public DateTime LastPurchaseDate { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        [StringLength(64)]
        public string PasswordHash { get; set; }

        [StringLength(24)]
        public string Salt { get; set; }
    }
}
