﻿using Adapters.Gateways.Activity;
using Adapters.Gateways.Base;
using Domain.Contracts.Notice;

namespace Adapters.Gateways.Notice;
public class CreateNoticeRequest : CreateNoticeInput, IRequest
{
    new public IList<BaseActivityType>? Activities { get; set; }
}