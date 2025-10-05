using System;

namespace Project.FeatureToggle.Domain.Constants;

public readonly struct FeatureFilter
{
    public const string ALL = "all";
    public const string ACTIVE = "active";
    public const string INACTIVE = "inactive";
}
