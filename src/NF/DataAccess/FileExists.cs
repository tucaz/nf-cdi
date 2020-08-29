using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NF.DataAccess
{
    public class Wait
    {
        public static async Task<bool> UntilFileExists(string path, int timeout=5000)
        {
            var source = new CancellationTokenSource();

            try
            {
                source.CancelAfter(timeout);
                var token = source.Token;

                var found = await Task.Factory.StartNew(async () =>
                {
                    var exists = File.Exists(path);

                    while (!exists)
                    {
                        await Task.Delay(1000, token);
                        exists = File.Exists(path);
                    }

                    return true;
                }, token);
                
                return await found;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            finally
            {
                source = null;
            }
        }
    }
}