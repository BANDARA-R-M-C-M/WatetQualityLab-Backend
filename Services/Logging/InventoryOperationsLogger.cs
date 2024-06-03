using Serilog.Core;

namespace Project_v1.Services.Logging {
    public class InventoryOperationsLogger {
        private readonly Logger _inventoryLogger;

        public InventoryOperationsLogger(Logger inventoryLogger) {
            _inventoryLogger = inventoryLogger;
        }

        public void LogInformation(string message) {
            _inventoryLogger.Information(message);
        }
    }
}
