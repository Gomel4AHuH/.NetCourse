using System.Data;

namespace ManageCitizens.Services
{
    internal class LockButtonService
    {
        private void UpdateStatus(bool status)
        {
            status = !status;
        }
    }
}
