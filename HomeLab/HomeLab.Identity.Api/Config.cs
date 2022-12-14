﻿// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Duende.IdentityServer.Models;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
           new IdentityResource[]
           {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
           };

}