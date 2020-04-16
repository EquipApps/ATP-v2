namespace EquipApps.Mvc
{
    /// <summary>
    /// Позволяет кастомизировать <see cref="IMvcFeature"/>.
    /// </summary>
    public interface IMvcFeatureConvetion
    {
        void Apply(IMvcFeature feature);
    }
}
