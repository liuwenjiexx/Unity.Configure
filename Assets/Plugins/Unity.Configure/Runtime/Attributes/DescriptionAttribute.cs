﻿using System;

namespace UnityEngine.Configure
{
    public class DescriptionAttribute : Attribute
    {
        public DescriptionAttribute(string description)
        {
            this.Description = description;
        }

        public string Description { get; set; }

    }



}
