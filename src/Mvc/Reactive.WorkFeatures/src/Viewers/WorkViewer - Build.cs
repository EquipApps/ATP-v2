using EquipApps.Testing;
using System;
using System.Threading.Tasks;

namespace EquipApps.WorkBench.ViewModels
{
    public partial class WorkViewer
    {
        private volatile ITest test;

        private async Task ClearTest()
        {
            try
            {
                //-- 
                test?.Dispose();
            }
            finally
            {
                test    = null;
                HesTest = false;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        private async Task TestCreateAsync()
        {
            try
            {
                //-- Флаг создание
                IsBuilding = true;

                //-- 
                var  canAccept = await Interactions.HandleAcceptSettings();
                if (!canAccept)
                    return;

                //-- Подчистка
                await ClearTest();

                

                var newTest  = await testFactory.CreateTestAsync();
                if (newTest != null)
                {
                    test = newTest;
                    HesTest = true;
                }

            }
            catch (Exception ex)
            {
                //-- Ошибки выводим в UI.
                await Interactions.HandleCreateException(ex);
            }
            finally
            {
                //-- Флаг идет проверка
                IsBuilding = false;
            }
        }

        private async Task TestCleanAsync()
        {
            try
            {
                //-- Флаг создание
                IsBuilding = true;

                await ClearTest();

                
            }
            catch (Exception ex)
            {
                //-- Ошибки выводим в UI.
                await Interactions.HandleCreateException(ex);
            }
            finally
            {
                //-- Флаг идет проверка
                IsBuilding = false;
            }
        }
    }
}
