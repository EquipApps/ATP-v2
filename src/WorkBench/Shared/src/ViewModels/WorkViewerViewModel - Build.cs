using EquipApps.Testing;
using System;
using System.Threading.Tasks;

namespace EquipApps.WorkBench.ViewModels
{
    public partial class WorkViewerViewModel
    {
        private volatile ITest test;

        private void ClearTest()
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
                GC.WaitForFullGCComplete();
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
                ClearTest();

                await testService.CleanAsync();
                await logsService.CleanAsync();


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

                ClearTest();

                await testService.CleanAsync();
                await logsService.CleanAsync();
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
