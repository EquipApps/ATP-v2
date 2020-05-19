using EquipApps.WorkBench.Tools.External.Advantech.PCI_1762;
using EquipApps.WorkBench.Tools.External.GwINSTEK.PSH_Series.PSH_3610;
using Microsoft.Extensions.Options;

namespace B.EK.Configure
{
    /// <summary>
    /// Конфигурация PSH-3610
    /// </summary>
    public class ConfigureOptionsHardwarePci1762 : IConfigureOptions<PCI_1762_Options>
    {
     

        public void Configure(PCI_1762_Options options)
        {
            //-- Регистрация устройств

            var dev1 = new PCI_1762_Device(1, 1);
            dev1.Relays[0].Name = "K24";    // "ЭПК1+"
            dev1.Relays[1].Name = "K26";
            dev1.Relays[2].Name = "K28";
            dev1.Relays[3].Name = "K30";

            dev1.Relays[4].Name = "K40";
            dev1.Relays[5].Name = "K42";
            dev1.Relays[6].Name = "K44";
            dev1.Relays[7].Name = "K46";

            dev1.Relays[8].Name = "K32";
            dev1.Relays[9].Name = "K34";
            dev1.Relays[10].Name = "K36";
            dev1.Relays[11].Name = "K38";

            dev1.Relays[12].Name = "K48";
            dev1.Relays[13].Name = "K50";
            dev1.Relays[14].Name = "K52";
            dev1.Relays[15].Name = "K54";   // "ЭПК16+"

            options.DeviceCollection.Add(dev1);

            var dev2 = new PCI_1762_Device(1, 1);
            dev2.Relays[0].Name = "K25";    // "ЭПК1-"
            dev2.Relays[1].Name = "K27";
            dev2.Relays[2].Name = "K29";
            dev2.Relays[3].Name = "K31";

            dev2.Relays[4].Name = "K41";
            dev2.Relays[5].Name = "K43";
            dev2.Relays[6].Name = "K45";
            dev2.Relays[7].Name = "K47";

            dev2.Relays[8].Name = "K33";
            dev2.Relays[9].Name = "K35";
            dev2.Relays[10].Name = "K37";
            dev2.Relays[11].Name = "K39";

            dev2.Relays[12].Name = "K49";
            dev2.Relays[13].Name = "K51";
            dev2.Relays[14].Name = "K53";
            dev2.Relays[15].Name = "K55";   // "ЭПК16-"

        }
    }
}
