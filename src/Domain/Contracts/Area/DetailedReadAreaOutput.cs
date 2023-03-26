﻿using Domain.Contracts.MainArea;

namespace Domain.Contracts.Area
{
    public class DetailedReadAreaOutput : BaseAreaContract
    {
        public Guid? Id { get; set; }
        public DateTime? DeletedAt { get; set; }
        public virtual DetailedMainAreaOutput? MainArea { get; set; }
    }
}