﻿using System.ComponentModel;
using Neurotoxin.ScOut.Data.Attributes;
using Neurotoxin.ScOut.Data.Entities;

namespace Neurotoxin.ScOut.Data.Relations
{
    [DisplayName("SQL Update")]
    [Icon("link.png")]
    [AllowedTargetEntityType(typeof(ColumnEntity))]
    public class UpdateRelation : RelationBase
    {
    }
}