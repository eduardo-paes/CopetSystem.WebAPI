﻿using Adapters.Gateways.Base;

namespace Adapters.Gateways.StudentAssistanceScholarship
{
    public class DetailedReadStudentAssistanceScholarshipResponse : Response
    {
        public Guid? Id { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}