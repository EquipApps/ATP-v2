using EquipApps.Mvc.ModelBinding;
using EquipApps.Mvc.ModelBinding.Property;
using EquipApps.Mvc.Objects;
using System;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Binders
{
    public class DataTextModelBinder : IModelBinder
    {
        private string format;
        private Tuple<int, PropertyExtractor>[] tuples;



        public DataTextModelBinder(string format, Tuple<int, PropertyExtractor>[] tuples)
        {
            this.format = format ?? throw new ArgumentNullException(nameof(format));
            this.tuples = tuples ?? throw new ArgumentNullException(nameof(format));
        }


        public BindingResult Bind(TestObject framworkElement, int offset = 0)
        {
            if (framworkElement == null)
            {
                throw new ArgumentNullException(nameof(framworkElement));
            }

            try
            {
                var args = new object[tuples.Length];

                for (int i = 0; i < tuples.Length; i++)
                {
                    var sourceIndex = tuples[i].Item1;
                    var propertyExtractor = tuples[i].Item2;


                    if (sourceIndex < 0)
                    {
                        //-- Индекс меньше нуля дата контекст не нужен
                        args[i] = propertyExtractor(null);
                    }
                    else
                    {
                        try
                        {
                            var index = sourceIndex - offset;
                            var dataContext = framworkElement.GetDataContext(index);

                            if (dataContext == null)
                            {
                                args[i] = string.Format("<ERROR(DataContext[{0}] is NULL)>", sourceIndex);
                            }
                            else
                            {
                                args[i] = propertyExtractor(dataContext);
                            }

                        }
                        catch (Exception e)
                        {
                            args[i] = string.Format("<ERROR({0})>", e.Message);
                        }
                    }
                }

                var value = string.Format(format, args);

                return BindingResult.Success(value);

            }
            catch (Exception ex)
            {
                return BindingResult.Failed(ex);
            }
        }

    }
}
