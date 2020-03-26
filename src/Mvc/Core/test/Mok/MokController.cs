using EquipApps.Mvc;
using NLib.AtpNetCore.Mvc.ModelBinding.Attribute;
using System;

namespace AtpNetCore.Mvc.Core.Tests.Mok
{
    #region Index

    public class IndexAttribute : Attribute, IDisplayIndexMetadata
    {
        public IndexAttribute(int index)
        {
            Index = index;
        }

        public int? Index { get; private set; }
    }

    public class IndexController
    {

    }

    public class Index1Controller
    {

    }
    [Index(3)]
    public class Index2Controller
    {

    }

    #endregion

















    public class MokController
    {

    }
    public class Mok1Controller
    {
    }
    public class Mok2Controller : ControllerBase
    {
    }
    public class Mok3Controller : ControllerBase<MokModel>
    {
    }












    public class Mok1NumberController
    {
    }

    public class Mok2NumberController : ControllerBase
    {
    }

    public class Mok3NumberController : ControllerBase<MokModel>
    {
    }
}
