﻿using EquipApps.Mvc.Routing;
using NLib.AtpNetCore.Mvc.ModelBinding.Attribute;
using System;

namespace EquipApps.Mvc
{
    /// <summary>
    /// Формерует Test Case 
    /// </summary>   
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CaseAttribute : OrderValueAttribute, IDisplayFormatTitleMetadata, IDisplayIndexMetadata
    {
        public CaseAttribute(int number, string title)
            : base("controller", number.ToString())
        {
            Index = number;
            TitleFormat = title;
        }

        public CaseAttribute(string order, string title)
            : base("controller", order)
        {
            
            TitleFormat = title;
        }

        public CaseAttribute(string title)
            : base("controller", string.Empty)
        {
            TitleFormat = title;
        }


        public string TitleFormat { get; }


        public int? Index { get; }
    }
}
