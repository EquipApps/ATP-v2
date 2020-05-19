using EquipApps.Hardware;
using EquipApps.Hardware.ValueDecorators;

namespace EquipApps.WorkBench.Tools.External.Advantech
{
    /// <summary>
    /// Линия с цифровым поведением.
    /// </summary>
    public class PCI_DI_Line : ValueBehaviorBase<byte>,
        IValueComonent<byte>
    {
        private readonly ValueDecoratorTransaction<byte> valueTransaction;

        public PCI_DI_Line()
        {
            valueTransaction = new ValueDecoratorTransaction<byte>(this);
        }

        /// <inheritdoc/> 
        public override byte Value
        {
            get => valueTransaction.Value;
            protected set => valueTransaction.Value = value;
        }

        internal void Enlist()
        {
            valueTransaction.Enlist();
        }


        byte IValueComonent<byte>.Value
        {
            get => base.Value;
            set => base.Value = value;
        }
    }
}
