using EquipApps.Mvc.Controllers;
using NLib.AtpNetCore.Mvc.ModelBinding.Attribute;
using System;

namespace AtpNetCore.Mvc.Core.Tests.Mok
{
    #region Index

    public class IndexController
    {

    }

    public class Index1Controller
    {

    }
    
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
        public override void Finally()
        {
            throw new NotImplementedException();
        }

        public override void InitializeComponent()
        {
            throw new NotImplementedException();
        }
    }
    public class Mok3Controller : ControllerBase<MokModel>
    {
    }












    public class Mok1NumberController
    {
    }

    public class Mok2NumberController : ControllerBase
    {
        public override void Finally()
        {
            throw new NotImplementedException();
        }

        public override void InitializeComponent()
        {
            throw new NotImplementedException();
        }
    }

    public class Mok3NumberController : ControllerBase<MokModel>
    {
    }
}
