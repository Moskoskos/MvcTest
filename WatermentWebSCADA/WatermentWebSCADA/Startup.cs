﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using codingfreaks.samples.Identity.Models;


[assembly: OwinStartupAttribute(typeof(WatermentWebSCADA.Startup))]
namespace WatermentWebSCADA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }


    }

}