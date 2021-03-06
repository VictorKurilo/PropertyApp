﻿namespace TechAssesment.Core.Models
{
    public class Property
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ApplicationUser Owner { get; set; }

        public string OwnerId { get; set; }
    }
}